using System;
using BrawlLib.SSBBTypes;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARGroupNode : RSAREntryNode
    {
        internal INFOGroupHeader* Header { get { return (INFOGroupHeader*)WorkingUncompressed.Address; } }
        internal override int StringId { get { return Header->_id; } }

        public override ResourceType ResourceType { get { return ResourceType.RSARGroup; } }

        internal List<RSARFileNode> _files = new List<RSARFileNode>();

        private int _id;
        private int _magic;
        private int _unk1, _unk2;

        public int Id { get { return _id; } }
        public int Magic { get { return _magic; } }
        public int Unknown1 { get { return _unk1; } }
        public int Unknown2 { get { return _unk2; } }

        public List<RSARFileNode> Files { get { return _files; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _id = Header->_id;
            _magic = Header->_magic;
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;

            //Get file references
            RSARNode rsar = RSARNode;
            VoidPtr offset = &rsar.Header->INFOBlock->_collection;
            //ResourceNode parent = rsar.Children[1];
            RuintList* list = Header->GetCollection(offset);
            int count = list->_numEntries;
            for (int i = 0; i < count; i++)
            {
                INFOGroupEntry* entry = (INFOGroupEntry*)list->Get(offset, i);
                int id = entry->_fileId;
                foreach (RSARFileNode node in rsar.Files)
                {
                    if (id == node._fileIndex)
                    {
                        _files.Add(node);
                        break;
                    }
                }
                //_files.Add(rsar.Files[id] as RSARFileNode);
            }

            return false;
        }
    }
}
