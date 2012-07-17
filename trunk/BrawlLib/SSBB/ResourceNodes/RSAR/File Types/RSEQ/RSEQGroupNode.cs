using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSEQEntryNode : ResourceNode
    {
        internal VoidPtr _offset;
    }

    public unsafe class RSEQGroupNode : ResourceNode
    {
        internal RSEQ_DATAHeader* Header { get { return (RSEQ_DATAHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.RSEQGroup; } }

        int _index;

        protected override bool OnInitialize()
        {
            _index = Index;
            if (_index == 0)
                _name = "Data";
            else
                _name = "Labels";

            return Header->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            if (_index == 0)
                for (int i = 0; i < Header->_numEntries; i++)
                    new RSEQDataNode().Initialize(this, &Header->Entries[i], 0);
            else
                for (int i = 0; i < ((RSEQ_LABLHeader*)Header)->_numEntries; i++)
                    new RSEQLabelNode().Initialize(this, ((RSEQ_LABLHeader*)Header)->Get(i), 0);
        }

        protected override int OnCalculateSize(bool force)
        {
            int size = 0xC;
            foreach (RSEQEntryNode g in Children)
                size += g.CalculateSize(true);
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }
    }
}
