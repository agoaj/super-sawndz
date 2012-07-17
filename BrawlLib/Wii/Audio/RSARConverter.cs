using System;
using System.Collections.Generic;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;

namespace BrawlLib.Wii.Audio
{
    public static unsafe class RSARConverter
    {
        internal static int CalculateSize(RSAREntryList entries)
        {
            //Header
            int len = 0x40;

            //SYMB, INFO, FILE Headers
            int symbLen = 0x20, infoLen = 0, fileLen = 0;

            //Calculate SYMB

            //String offsets
            symbLen += entries._strings.Count * 4;

            //Strings are packed tightly with no trailing pad
            symbLen += entries._stringLength;

            //Mask entries
            symbLen += 32; //Headers
            symbLen += (entries._count * 2 - 4) * 20; //Entries

            //Align
            symbLen = symbLen.Align(0x20);


            //Calculate INFO


            //Calculate FILE


            return len + symbLen;
        }

        internal static int EncodeSYMBBlock(SYMBHeader* header, RSAREntryList entries)
        {
            int count = entries._count;
            VoidPtr baseAddr = (VoidPtr)header + 8, dataAddr;
            int len;
            bint* strEntry = (bint*)(baseAddr + 0x18);
            PString pStr = (byte*)strEntry + (count << 2);

            //Strings
            header->_stringOffset = 0x14;
            strEntry[-1] = entries._strings.Count;
            foreach (string s in entries._strings)
            {
                *strEntry++ = (int)(pStr - baseAddr);
                pStr.Write(s, 0, s.Length + 1);
                pStr += s.Length + 1;
            }

            dataAddr = pStr;

            //Sounds
            header->_maskOffset1 = (int)(dataAddr - baseAddr);
            dataAddr += EncodeMaskGroup((SYMBMaskHeader*)dataAddr, entries._sounds);

            //Types
            header->_maskOffset2 = (int)(dataAddr - baseAddr);
            dataAddr += EncodeMaskGroup((SYMBMaskHeader*)dataAddr, entries._types);

            //Groups
            header->_maskOffset3 = (int)(dataAddr - baseAddr);
            dataAddr += EncodeMaskGroup((SYMBMaskHeader*)dataAddr, entries._groups);

            //Banks
            header->_maskOffset4 = (int)(dataAddr - baseAddr);
            dataAddr += EncodeMaskGroup((SYMBMaskHeader*)dataAddr, entries._banks);

            len = ((int)baseAddr).Align(0x20);

            //Fill padding
            byte* p = (byte*)dataAddr;
            for (int i = dataAddr - header; i < len; i++)
                *p++ = 0;

            //Set header
            header->_tag = SYMBHeader.Tag;
            header->_length = len;

            return len;
        }
        internal static int EncodeINFOBlock(INFOHeader* header, RSAREntryList entries)
        {
            return 0;
        }
        internal static int EncodeFILEBlock(FILEHeader* header, RSAREntryList entries)
        {
            return 0;
        }

        private static int EncodeMaskGroup(SYMBMaskHeader* header, List<RSAREntryState> group)
        {
            header->_entrySize = 0xA;
            header->_entryNum = group.Count * 2 - 1;
            SYMBMaskEntry* entry = header->Entries;
            foreach (RSAREntryState s in group)
            {
                *entry++ = new SYMBMaskEntry(0x1FFFF, -1, -1, s._stringId, s._index);
                if (s._index != 0)
                    *entry++ = new SYMBMaskEntry(0, 0, 0, -1, -1);
            }
            return (int)entry - (int)header;
        }
    }

    public struct RSAREntryState
    {
        public ResourceNode _node;
        public int _index;
        public int _stringId;
    }

    public class RSAREntryList
    {
        public int _count = 0;
        public int _stringLength = 0;
        public List<string> _strings = new List<string>();
        public List<RSAREntryState> _sounds = new List<RSAREntryState>();
        public List<RSAREntryState> _types = new List<RSAREntryState>();
        public List<RSAREntryState> _groups = new List<RSAREntryState>();
        public List<RSAREntryState> _banks = new List<RSAREntryState>();

        public void AddEntry(string path, ResourceNode node)
        {
            RSAREntryState state = new RSAREntryState();
            state._node = node;

            if (string.IsNullOrEmpty(path))
                state._stringId = -1;
            else
            {
                int id = _strings.IndexOf(path);
                if (id > 0)
                    state._stringId = id;
                else
                {
                    state._stringId = _strings.Count;
                    _strings.Add(path);
                    _stringLength += path.Length + 1;
                }
            }

            List<RSAREntryState> group;
            if (node is RSARSoundNode)
                group = _sounds;
            else if (node is RSARGroupNode)
                group = _groups;
            else if (node is RSARTypeNode)
                group = _types;
            else
                group = _banks;

            state._index = group.Count;
            group.Add(state);

            _count++;
        }

        public void Clear()
        {
            _strings.Clear();
            _sounds.Clear();
            _types.Clear();
            _groups.Clear();
            _banks.Clear();
            _count = 0;
            _stringLength = 0;
        }
    }
}
