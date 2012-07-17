using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0GroupNode : ResourceNode
    {
        internal ResourceGroup* Group { get { return (ResourceGroup*)WorkingUncompressed.Address; } }

        public override ResourceType ResourceType { get { return ResourceType.MDL0Group; } }

        public SCN0GroupNode() : base() { }
        public SCN0GroupNode(string name) : base() { _name = name; }

        internal void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (SCN0EntryNode n in Children)
                n.GetStrings(table);
        }

        protected override bool OnInitialize()
        {
            return true;
        }

        public int _groupLen, _entryLen, keyLen, lightLen;
        protected override int OnCalculateSize(bool force)
        {
            _groupLen = 0x18 + UsedChildren.Count * 0x10;
            _entryLen = 0;
            foreach (SCN0EntryNode n in Children)
            {
                _entryLen += n.CalculateSize(true);
                keyLen += n.keyLen;
                lightLen += n.lightLen;
            }
            return _entryLen + _groupLen + keyLen + lightLen;
        }
        public VoidPtr _dataAddr, keyframeAddress, lightArrayAddress;
        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ResourceGroup* group = (ResourceGroup*)address;
            *group = new ResourceGroup(UsedChildren.Count);

            int nodeIndex = 0;

            ResourceEntry* entry = group->First;
            foreach (SCN0EntryNode n in Children)
            {
                if (n.Name != "<null>")
                {
                    (entry++)->_dataOffset = (int)_dataAddr - (int)group;
                    n._nodeIndex = nodeIndex++;
                    n._realIndex = n.Index;
                }
                else
                    n._nodeIndex = n._realIndex = -1;

                n.keyframeAddr = keyframeAddress;
                n.lightAddr = (RGBAPixel*)lightArrayAddress;

                n.Rebuild(_dataAddr, n._calcSize, true);

                _dataAddr += n._calcSize;
                keyframeAddress += n.keyLen;
                lightArrayAddress += n.lightLen;
            }
        }

        protected internal virtual void PostProcess(VoidPtr scn0Address, VoidPtr dataAddress, StringTable stringTable)
        {
            ResourceGroup* group = (ResourceGroup*)dataAddress;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (SCN0EntryNode n in UsedChildren)
            {
                dataAddress = (VoidPtr)group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                n.PostProcess(scn0Address, dataAddress, stringTable);
            }
        }

        public List<ResourceNode> UsedChildren
        {
            get
            {
                List<ResourceNode> l = new List<ResourceNode>();
                foreach (SCN0EntryNode n in Children)
                    if (n.Name != "<null>")
                        l.Add(n);
                return l;
            }
        }
    }

    public unsafe class SCN0EntryNode : ResourceNode
    {
        internal SCN0CommonHeader* Header { get { return (SCN0CommonHeader*)WorkingUncompressed.Address; } }

        public VoidPtr scn0Addr;

        public VoidPtr keyframeAddr;
        public RGBAPixel* lightAddr;
        public int keyLen, lightLen;

        public int _length, _scn0Offset, _stringOffset, _nodeIndex, _realIndex;

        [Category("SCN0 Entry")]
        public int Length { get { return _length; } }//set { _length = value; SignalPropertyChange(); } }
        [Category("SCN0 Entry")]
        public int SCN0Offset { get { return _scn0Offset; } }//set { _scn0Offset = value; SignalPropertyChange(); } }
        [Category("SCN0 Entry")]
        public int NodeIndex { get { return _nodeIndex; } }//set { _nodeIndex = value; SignalPropertyChange(); } }
        [Category("SCN0 Entry")]
        public int RealIndex { get { return _realIndex; } }//set { _realIndex = value; SignalPropertyChange(); } }

        internal virtual void GetStrings(StringTable table) { if (Name != "<null>") table.Add(Name); }

        protected override bool OnInitialize()
        {
            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;
            else
                _name = "<null>";

            SetSizeInternal(Header->_length);

            _length = Header->_length;
            _scn0Offset = Header->_scn0Offset;
            _stringOffset = Header->_stringOffset;
            _nodeIndex = Header->_nodeIndex;
            _realIndex = Header->_realIndex;

            return false;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            SCN0CommonHeader* header = (SCN0CommonHeader*)address;
            if (Name == "<null>")
            {
                header->_scn0Offset = (int)scn0Addr - (int)address;
                header->_nodeIndex = header->_realIndex = -1;
                header->_stringOffset = 0;
            }
            header->_length = _length = length;
        }

        protected internal virtual void PostProcess(VoidPtr scn0Address, VoidPtr dataAddress, StringTable stringTable)
        {
            SCN0CommonHeader* header = (SCN0CommonHeader*)dataAddress;
            header->_length = _length;
            header->_scn0Offset = (int)scn0Address - (int)dataAddress;
            if (Name != "<null>")
            {
                header->ResourceStringAddress = stringTable[Name] + 4;
                header->_nodeIndex = _nodeIndex;
                header->_realIndex = _realIndex;
            }
            else
            {
                header->_nodeIndex = header->_realIndex = -1;
                header->_stringOffset = 0;
            }
        }
    }
}
