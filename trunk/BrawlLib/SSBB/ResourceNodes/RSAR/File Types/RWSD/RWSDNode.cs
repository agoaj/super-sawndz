using System;
using BrawlLib.SSBBTypes;
using System.IO;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RWSDNode : RSARFileNode
    {
        internal RWSDHeader* Header { get { return (RWSDHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.RWSD; } }

        protected override void GetStrings(LabelBuilder builder)
        {
            foreach (RWSDDataNode node in Children[0].Children)
                builder.Add(node._soundIndex, node._name);
        }

        //Finds labels using LABL block between header and footer, also initializes array
        protected bool GetLabels(int count)
        {
            RWSDHeader* header = (RWSDHeader*)WorkingUncompressed.Address;
            int len = header->_header._length;
            RSEQ_LABLHeader* labl = (RSEQ_LABLHeader*)((int)header + len);

            if ((WorkingUncompressed.Length > len) && (labl->_tag == RSEQ_LABLHeader.Tag))
            {
                _labels = new LabelItem[count];
                count = labl->_numEntries;
                for (int i = 0; i < count; i++)
                {
                    RSEQ_LABLEntry* entry = labl->Get(i);
                    _labels[i] = new LabelItem() { String = entry->Name, Tag = entry->_id };
                }
                return true;
            }

            return false;
        }

        private void ParseBlocks()
        {
            VoidPtr dataAddr = Header;
            int len = Header->_header._length;
            int total = WorkingUncompressed.Length;

            //Look for labl block
            RSEQ_LABLHeader* labl = (RSEQ_LABLHeader*)(dataAddr + len);
            if ((total > len) && (labl->_tag == RSEQ_LABLHeader.Tag))
            {
                int count = labl->_numEntries;
                _labels = new LabelItem[count];
                count = labl->_numEntries;
                for (int i = 0; i < count; i++)
                {
                    RSEQ_LABLEntry* entry = labl->Get(i);
                    _labels[i] = new LabelItem() { String = entry->Name, Tag = entry->_id };
                }
                len += labl->_size;
            }

            //Set data source
            if (total > len)
                _audioSource = new DataSource(dataAddr + len, total - len);
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            ParseBlocks();

            return true;
        }

        protected override void OnPopulate()
        {
            RSARNode rsar = RSARNode;
            SYMBHeader* symb = null;
            RuintList* soundList = null;
            INFOSoundEntry** soundIndices = null;
            VoidPtr soundOffset = null;
            INFOSoundEntry* sEntry;
            RWSDGroupNode g;
            RWSDHeader* rwsd = Header;
            RWSD_DATAHeader* data = rwsd->Data;
            //RWSD_WAVEHeader* wave = rwsd->Wave;
            RuintList* list = &data->_list;
            //RuintList* waveList = &wave->_list;
            int count = list->_numEntries;

            //Get sound info from RSAR (mainly for names)
            if (rsar != null)
            {
                symb = rsar.Header->SYMBBlock;
                soundOffset = &rsar.Header->INFOBlock->_collection;
                soundList = rsar.Header->INFOBlock->Sounds;
                soundIndices = (INFOSoundEntry**)Marshal.AllocHGlobal(count * 4);

                //int sIndex = 0;
                int soundCount = soundList->_numEntries;
                for (int i = 0; i < soundCount; i++)
                    if ((sEntry = (INFOSoundEntry*)soundList->Get(soundOffset, i))->_fileId == _fileIndex)
                        soundIndices[((INFOSoundPart2*)sEntry->GetPart2(soundOffset))->_soundIndex] = sEntry;
            }
            (g = new RWSDGroupNode()).Initialize(this, Header->Data, Header->_dataLength);
            for (int i = 0; i < count; i++)
            {
                RWSD_DATAEntry* entry = (RWSD_DATAEntry*)list->Get(list, i);
                RWSDDataNode node = new RWSDDataNode();
                node._offset = list;
                node.Initialize(g, entry, 0);

                //Attach from INFO block
                if (soundIndices != null)
                {
                    sEntry = soundIndices[i];
                    node._name = symb->GetStringEntry(sEntry->_stringId);
                }
            }

            if (soundIndices != null)
                Marshal.FreeHGlobal((IntPtr)soundIndices);

            //Get labels
            RSARNode parent;
            int count2 = Header->Data->_list._numEntries;
            if ((_labels == null) && ((parent = RSARNode) != null))
            {
                _labels = new LabelItem[count2];// new string[count];

                //Get them from RSAR
                SYMBHeader* symb2 = parent.Header->SYMBBlock;
                INFOHeader* info = parent.Header->INFOBlock;

                VoidPtr offset = &info->_collection;
                RuintList* soundList2 = info->Sounds;
                count2 = soundList2->_numEntries;

                INFOSoundEntry* entry;
                for (int i = 0; i < count2; i++)
                    if ((entry = (INFOSoundEntry*)soundList2->Get(offset, i))->_fileId == _fileIndex)
                        _labels[((INFOSoundPart2*)entry->GetPart2(offset))->_soundIndex] = new LabelItem() { Tag = i, String = symb2->GetStringEntry(entry->_stringId) };
            }

            new RWSDGroupNode().Initialize(this, Header->Wave, Header->_waveLength);
        }

        protected override int OnCalculateSize(bool force)
        {
            int size = RWSDHeader.Size;
            foreach (RWSDGroupNode g in Children)
                size += g.CalculateSize(true);
            return base.OnCalculateSize(force);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }

        public override void Remove()
        {
            if (RSARNode != null)
                RSARNode.Files.Remove(this);
            base.Remove();
        }

        internal static ResourceNode TryParse(DataSource source) { return ((RWSDHeader*)source.Address)->_header._tag == RWSDHeader.Tag ? new RWSDNode() : null; }
    }
}
