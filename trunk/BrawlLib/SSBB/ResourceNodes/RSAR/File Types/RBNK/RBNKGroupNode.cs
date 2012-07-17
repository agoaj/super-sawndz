using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RBNKEntryNode : ResourceNode
    {
        internal VoidPtr _offset;
    }

    public unsafe class RBNKGroupNode : ResourceNode
    {
        internal RBNK_DATAHeader* Header { get { return (RBNK_DATAHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.RBNKGroup; } }

        int _index;

        protected override bool OnInitialize()
        {
            _index = Index;
            if (_index == 0)
                _name = "Data";
            else
                _name = "Banks";

            return Header->_list._numEntries > 0;
        }

        protected override void OnPopulate()
        {
            int count;

            if (_index == 0)
            {
                RBNK_DATAHeader* header = Header;
                VoidPtr offset = &header->_list;
                count = header->_list._numEntries;

                LabelItem[] list = ((RBNKNode)_parent)._labels; //Get labels from parent
                ((RBNKNode)_parent)._labels = null; //Clear labels, no more use for them!

                for (int i = 0; i < count; i++)
                {
                    RBNKDataNode node = new RBNKDataNode();
                    node._offset = offset;
                    if (list != null)
                    {
                        node._soundIndex = list[i].Tag;
                        node._name = list[i].String;
                    }
                    node.Initialize(this, header->_list.Get(offset, i), 0);
                }
            }
            else
            {
                //Uses same format as RWSD
                //RWSD_WAVEHeader* header = (RWSD_WAVEHeader*)Header;
                //count = header->_entries;
                //for (int i = 0; i < count; i++)
                //    new RWSDSoundNode().Initialize(this, header->GetEntry(i), 0);
            }
        }

        protected override int OnCalculateSize(bool force)
        {
            int size = 0xC;
            foreach (RBNKEntryNode g in Children)
                size += g.CalculateSize(true);
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }
    }
}
