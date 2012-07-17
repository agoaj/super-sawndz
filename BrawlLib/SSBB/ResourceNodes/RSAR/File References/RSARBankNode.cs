using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARBankNode : RSAREntryNode
    {
        internal INFOBankEntry* Header { get { return (INFOBankEntry*)WorkingUncompressed.Address; } }
        internal override int StringId { get { return Header->_stringId; } }

        internal RBNKNode _rbnk;

        [Category("INFO Bank")]
        public int FileIndex { get { return Header->_fileId; } }
        [Category("INFO Bank")]
        public int Padding { get { return Header->_padding; } }

        public override ResourceType ResourceType { get { return ResourceType.RSARBank; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _rbnk = RSARNode.Files[FileIndex] as RBNKNode;

            return false;
        }
    }
}
