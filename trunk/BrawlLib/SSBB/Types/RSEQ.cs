using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RSEQHeader
    {
        public const uint Tag = 0x51455352;

        public SSBBCommonHeader _header;

        public bint _dataOffset;
        public bint _dataLength;
        public bint _lablOffset;
        public bint _lablLength;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public RSEQ_DATAHeader* Data { get { return (RSEQ_DATAHeader*)(Address + _dataOffset); } }
        public RSEQ_LABLHeader* Labl { get { return (RSEQ_LABLHeader*)(Address + _lablOffset); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RSEQ_DATAHeader
    {
        public const uint Tag = 0x41544144;

        public uint _tag;
        public bint _size;
        public bint _numEntries;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public RSEQ_DATAEntry* Entries { get { return (RSEQ_DATAEntry*)(Address + 12); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RSEQ_DATAEntry
    {
        public const int Size = 7;

        public bfloat _float;
        public byte _unk1;
        public byte _unk2;
        public byte _unk3;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RSEQ_LABLHeader
    {
        public const uint Tag = 0x4C42414C;

        public uint _tag;
        public bint _size;
        public bint _numEntries;

        public void Set(int size, int count)
        {
            _tag = Tag;
            _size = size;
            _numEntries = count;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public bint* EntryOffset { get { return (bint*)(Address + 12); } }

        public RSEQ_LABLEntry* Get(int index)
        {
            bint* offset = (bint*)(Address + 8);
            return (RSEQ_LABLEntry*)((int)offset + offset[index + 1]);
        }

        public string GetString(int index)
        {
            bint* offset = (bint*)(Address + 8);
            return ((RSEQ_LABLEntry*)((int)offset + offset[index + 1]))->Name;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RSEQ_LABLEntry
    {
        public bint _id; //Index for something?
        public bint _stringLength;

        public void Set(int id, string str)
        {
            int len = str.Length;
            int i = 0;
            sbyte* dPtr = (sbyte*)(Address + 8);
            char* sPtr;

            _id = id;
            _stringLength = len;

            fixed (char* s = str)
            {
                sPtr = s;
                while (i++ < len) 
                    *dPtr++ = (sbyte)*sPtr++;
            }

            //Trailing zero
            *dPtr++ = 0;

            //Padding
            while((i++ & 3) != 0)
                *dPtr++ = 0;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public string Name 
        {
            get { return new string((sbyte*)Address + 8); }
        }
    }
}
