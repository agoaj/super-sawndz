using System;
using BrawlLib.SSBBTypes;
using System.IO;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSEQNode : RSARFileNode
    {
        internal RSEQHeader* Header { get { return (RSEQHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.RSEQ; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            RSARNode rsar = RSARNode;
            if (_name == null)
            if (rsar == null)
                _name = Path.GetFileNameWithoutExtension(_origPath);
            else
                _name = String.Format("[0x{0:X}] Sequence", _fileIndex);
            return true;
        }

        protected override void OnPopulate()
        {
            //RSARNode rsar = RSARNode;
            //SYMBHeader* symb = null;
            //RuintList* soundList = null;
            //INFOSoundEntry** soundIndices = null;
            //VoidPtr soundOffset = null;
            //INFOSoundEntry* sEntry;
            RSEQGroupNode g;
            //RSEQHeader* rwsd = Header;
            //RSEQ_DATAHeader* data = rwsd->Data;
            ////RWSD_WAVEHeader* wave = rwsd->Wave;
            //RuintList* list = &data->_list;
            ////RuintList* waveList = &wave->_list;
            //int count = data->_numEntries;

            ////Get sound info from RSAR (mainly for names)
            //if (rsar != null)
            //{
            //    symb = rsar.Header->SYMBBlock;
            //    soundOffset = &rsar.Header->INFOBlock->_collection;
            //    soundList = rsar.Header->INFOBlock->Sounds;
            //    soundIndices = (INFOSoundEntry**)Marshal.AllocHGlobal(count * 4);

            //    //int sIndex = 0;
            //    int soundCount = soundList->_numEntries;
            //    for (int i = 0; i < soundCount; i++)
            //        if ((sEntry = (INFOSoundEntry*)soundList->Get(soundOffset, i))->_fileId == _fileIndex)
            //            soundIndices[((INFOSoundPart2*)sEntry->GetPart2(soundOffset))->_soundIndex] = sEntry;
            //}
            (g = new RSEQGroupNode()).Initialize(this, Header->Data, Header->_dataLength);
            //for (int i = 0; i < count; i++)
            //{
            //    RWSD_DATAEntry* entry = (RWSD_DATAEntry*)list->Get(list, i);
            //    RWSDDataNode node = new RWSDDataNode();
            //    node._offset = list;
            //    node.Initialize(g, entry, 0);

            //    //Attach from INFO block
            //    if (soundIndices != null)
            //    {
            //        sEntry = soundIndices[i];
            //        node._name = symb->GetStringEntry(sEntry->_stringId);
            //    }
            //}

            //if (soundIndices != null)
            //    Marshal.FreeHGlobal((IntPtr)soundIndices);

            ////Get labels
            //RSARNode parent;
            //int count2 = Header->Data->_list._numEntries;
            //if ((_labels == null) && ((parent = RSARNode) != null))
            //{
            //    _labels = new LabelItem[count2];// new string[count];

            //    //Get them from RSAR
            //    SYMBHeader* symb2 = parent.Header->SYMBBlock;
            //    INFOHeader* info = parent.Header->INFOBlock;

            //    VoidPtr offset = &info->_collection;
            //    RuintList* soundList2 = info->Sounds;
            //    count2 = soundList2->_numEntries;

            //    INFOSoundEntry* entry;
            //    for (int i = 0; i < count2; i++)
            //        if ((entry = (INFOSoundEntry*)soundList2->Get(offset, i))->_fileId == _fileIndex)
            //            _labels[((INFOSoundPart2*)entry->GetPart2(offset))->_soundIndex] = new LabelItem() { Tag = i, String = symb2->GetStringEntry(entry->_stringId) };
            //}

            new RSEQGroupNode().Initialize(this, Header->Labl, Header->_lablLength);
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

        internal static ResourceNode TryParse(DataSource source) { return ((RSEQHeader*)source.Address)->_header._tag == RSEQHeader.Tag ? new RSEQNode() : null; }
    }
}
