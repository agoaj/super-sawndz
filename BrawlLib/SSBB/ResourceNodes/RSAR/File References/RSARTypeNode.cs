using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARTypeNode : RSAREntryNode
    {
        internal INFOTypeEntry* Header { get { return (INFOTypeEntry*)WorkingUncompressed.Address; } }
        internal override int StringId { get { return Header->_typeId; } }

        public override ResourceType ResourceType { get { return ResourceType.RSARType; } }

        private uint _flags;
        private int _typeId;

        [Category("INFO Type")]
        public uint Flags { get { return _flags; } set { _flags = value; SignalPropertyChange(); } }
        [Category("INFO Type")]
        public int TypeId { get { return _typeId; } set { _typeId = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _flags = Header->_flags;
            _typeId = Header->_typeId;

            return false;
        }
    }
}
