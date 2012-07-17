using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RWSDEntryNode : ResourceNode
    {
        internal VoidPtr _offset;
    }

    public unsafe class RWSDGroupNode : ResourceNode
    {
        internal RWSD_DATAHeader* Header { get { return (RWSD_DATAHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.RWSDGroup; } }

        int _index;

        protected override bool OnInitialize()
        {
            _index = Index;
            if (_index == 0)
                _name = "Sounds";
            else
                _name = "Audio";

            return Header->_list._numEntries > 0;
        }

        protected override void OnPopulate()
        {
            int count;

            if (_index == 0)
            {
                //RWSD_DATAHeader* header = Header;
                //VoidPtr offset = &header->_list;
                //count = header->_list._numEntries;

                //LabelItem[] list = ((RWSDNode)_parent)._labels; //Get labels from parent
                //((RWSDNode)_parent)._labels = null; //Clear labels, no more use for them!

                //for (int i = 0; i < count; i++)
                //{
                //    RWSDDataNode node = new RWSDDataNode();
                //    node._offset = offset;
                //    if (list != null)
                //    {
                //        node._soundIndex = list[i].Tag;
                //        node._name = list[i].String;
                //    }
                //    node.Initialize(this, header->_list.Get(offset, i), 0);
                //}
            }
            else
            {
                RWSD_WAVEHeader* header = (RWSD_WAVEHeader*)Header;
                count = header->_entries;
                for (int i = 0; i < count; i++)
                    new RWSDSoundNode().Initialize(this, header->GetEntry(i), 0);
            }
        }

        protected override int OnCalculateSize(bool force)
        {
            int size = 0xC;
            foreach (RWSDEntryNode g in Children)
                size += g.CalculateSize(true);
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }
    }
}
