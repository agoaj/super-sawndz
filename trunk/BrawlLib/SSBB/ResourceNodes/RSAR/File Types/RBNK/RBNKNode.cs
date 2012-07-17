using System;
using BrawlLib.SSBBTypes;
using System.IO;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RBNKNode : RSARFileNode
    {
        internal RBNKHeader* Header { get { return (RBNKHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.RBNK; } }

        //protected override void GetStrings(LabelBuilder builder)
        //{
        //    foreach (RWSDDataNode node in Children[0].Children)
        //        builder.Add(node._soundIndex, node._name);
        //}

        //Finds labels using LABL block between header and footer, also initializes array
        protected bool GetLabels(int count)
        {
            RBNKHeader* header = (RBNKHeader*)WorkingUncompressed.Address;
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

        protected override bool OnInitialize()
        {
            RSARNode parent;

            //Find bank entry in rsar
            if ((_name == null) && ((parent = RSARNode) != null))
            {
                RSARHeader* rsar = parent.Header;
                RuintList* list = rsar->INFOBlock->Banks;
                VoidPtr offset = &rsar->INFOBlock->_collection;
                SYMBHeader* symb = rsar->SYMBBlock;

                int count = list->_numEntries;
                for (int i = 0; i < count; i++)
                {
                    INFOBankEntry* bank = (INFOBankEntry*)list->Get(offset, i);
                    if (bank->_fileId == _fileIndex)
                    {
                        _name = symb->GetStringEntry(bank->_stringId);
                        break;
                    }
                }
            }
            
            base.OnInitialize();

            ParseBlocks();

            return true;
        }

        protected override void OnPopulate()
        {
            RSARNode rsar = RSARNode;
            SYMBHeader* symb = null;
            RuintList* bankList = null;
            //INFOBankEntry** soundIndices = null;
            VoidPtr soundOffset = null;
            //INFOBankEntry* sEntry;
            RBNKGroupNode g;
            RBNKHeader* rwsd = Header;
            RBNK_DATAHeader* data = rwsd->Data;
            //RWSD_WAVEHeader* wave = rwsd->Wave;
            RuintList* list = &data->_list;
            //RuintList* waveList = &wave->_list;
            int count = list->_numEntries;

            //Get sound info from RSAR (mainly for names)
            if (rsar != null)
            {
                symb = rsar.Header->SYMBBlock;
                soundOffset = &rsar.Header->INFOBlock->_collection;
                bankList = rsar.Header->INFOBlock->Banks;
                //soundIndices = (INFOBankEntry**)Marshal.AllocHGlobal(count * 4);

                //int sIndex = 0;
                //int soundCount = soundList->_numEntries;
                //for (int i = 0; i < soundCount; i++)
                //    if ((sEntry = (INFOBankEntry*)soundList->Get(soundOffset, i))->_fileId == _fileIndex)
                //        soundIndices[((INFOSoundPart2*)sEntry->GetPart2(soundOffset))->_soundIndex] = sEntry;
            }
            (g = new RBNKGroupNode()).Initialize(this, Header->Data, Header->_dataLength);
            for (int i = 0; i < count; i++)
            {
                RBNK_DATAEntry* entry = (RBNK_DATAEntry*)list->Get(list, i);
                RBNKDataNode node = new RBNKDataNode();
                node._offset = list;
                node.Initialize(g, entry, 0);

                //Attach from INFO block
                //if (soundIndices != null)
                //{
                //    sEntry = soundIndices[i];
                //    node._name = symb->GetStringEntry(sEntry->_stringId);
                //}
            }

            //if (soundIndices != null)
            //    Marshal.FreeHGlobal((IntPtr)soundIndices);

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
                RuintList* bankList2 = info->Banks;
                count2 = bankList2->_numEntries;

                //INFOBankEntry* entry;
                //for (int i = 0; i < count2; i++)
                //    if ((entry = (INFOBankEntry*)soundList2->Get(offset, i))->_fileId == _fileIndex)
                //        _labels[((INFOSoundPart2*)entry->GetPart2(offset))->_soundIndex] = new LabelItem() { Tag = i, String = symb2->GetStringEntry(entry->_stringId) };
            }

            new RBNKGroupNode().Initialize(this, Header->Wave, Header->_waveLength);
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

        protected override int OnCalculateSize(bool force)
        {
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

        internal static ResourceNode TryParse(DataSource source) { return ((RBNKHeader*)source.Address)->_header._tag == RBNKHeader.Tag ? new RBNKNode() : null; }
    }
}
