using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RWSDHeader
    {
        public const uint Tag = 0x44535752;
        public const int Size = 0x20;

        public SSBBCommonHeader _header;

        public bint _dataOffset;
        public bint _dataLength;
        public bint _waveOffset;
        public bint _waveLength;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public RWSD_DATAHeader* Data { get { return (RWSD_DATAHeader*)(Address + _dataOffset); } }
        public RWSD_WAVEHeader* Wave { get { return (RWSD_WAVEHeader*)(Address + _waveOffset); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RWSD_DATAHeader
    {
        public const uint Tag = 0x41514144;

        public uint _tag;
        public bint _length;
        public RuintList _list;

        //public uint _numEntries;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        //public VoidPtr OffsetAddress { get { return Address + 8; } }

        //public ruint* Entries { get { return (ruint*)(Address + 12); } }

        //public DATAEntry* GetEntry(int index)
        //{
        //    return (DATAEntry*)(OffsetAddress + Entries[index]._data);
        //}

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RWSD_DATAEntry
    {
        public ruint _part1Offset;
        public ruint _part2Offset;
        public ruint _part3Offset;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public RWSD_DATAEntryPart1* GetPart1(VoidPtr offset) { return (RWSD_DATAEntryPart1*)_part1Offset.Offset(offset); }
        public RuintList* GetPart2(VoidPtr offset) { return (RuintList*)_part2Offset.Offset(offset); }
        public RuintList* GetPart3(VoidPtr offset) { return (RuintList*)_part3Offset.Offset(offset); }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RWSD_DATAEntryPart1
    {
        public bfloat _unk1;
        public bfloat _unk2;
        public bshort _unk3;
        public bshort _unk4;
        public bint _unk5;
        public bint _unk6;
        public bint _unk7;
        public bint _unk8;
        public bint _unk9;
    }

    //These entries are embedded in a list of lists, using RuintList
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RWSD_DATAEntryPart2
    {
        public bint _unk1; //0
        public bint _unk2; //0
        public bint _unk3; //0
        public bint _unk4; //0

        //[TypeConverter(typeof(IntToHex))]
        //public int Unknown1 { get { return _unk1; } set { _unk1 = value; } }
        //[TypeConverter(typeof(IntToHex))]
        //public int Unknown2 { get { return _unk2; } set { _unk2 = value; } }
        //[TypeConverter(typeof(IntToHex))]
        //public int Unknown3 { get { return _unk3; } set { _unk3 = value; } }
        //[TypeConverter(typeof(IntToHex))]
        //public int Unknown4 { get { return _unk4; } set { _unk4 = value; } }
    }

    //These entries are embedded in a list, using RuintList
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RWSD_DATAEntryPart3
    {
        public bint _index;
        public buint _magic; //0x7F7F7F7F
        public bint _unk1; //0
        public bfloat _unk2; //0.01557922
        public bfloat _unk3; //1.0
        public bint _unk4; //0
        public bint _unk5; //0
        public bint _unk6; //0
        public bint _unk7; //0
        public bint _unk8; //0
        public bint _unk9; //0
        public bint _unk10; //0

        public int Index { get { return _index; } set { _index = value; } }
        public uint Magic { get { return _magic; } set { _magic = value; } }
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; } }
        public float Float1 { get { return _unk2; } set { _unk2 = value; } }
        public float Float2 { get { return _unk3; } set { _unk3 = value; } }
        public int Unknown4 { get { return _unk4; } set { _unk4 = value; } }
        public int Unknown5 { get { return _unk5; } set { _unk5 = value; } }
        public int Unknown6 { get { return _unk6; } set { _unk6 = value; } }
        public int Unknown7 { get { return _unk7; } set { _unk7 = value; } }
        public int Unknown8 { get { return _unk8; } set { _unk8 = value; } }
        public int Unknown9 { get { return _unk9; } set { _unk9 = value; } }
        public int Unknown10 { get { return _unk10; } set { _unk10 = value; } }

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RWSD_WAVEHeader
    {
        public const uint Tag = 0x45564157;

        public buint _tag;
        public buint _length;
        public bint _entries;
        //bint Data Offsets

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public bint* Entries { get { return (bint*)(Address + 12); } }

        public RWSD_WAVEEntry* GetEntry(int index) { return (RWSD_WAVEEntry*)(Address + Entries[index]); }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RWSD_WAVEEntry
    {
        public const int Size = 0x3C + 0x2E;

        public AudioFormatInfo _format;

        public bushort _sampleRate;
        public bushort _unk1; //0x00
        public buint _loopStartSample;
        public bint _nibbles; //Includes ALL data, not just samples
        public bint _unk4; //0x1C
        public bint _offset; //Data offset from beginning of sample block
        public bint _unk5; //0
        public bint _unk6; //0x20
        public bint _unk7; //0
        public bint _unk8; //0x3C
        public int _unk9; //1
        public int _unk10; //1
        public int _unk11; //1
        public int _unk12; //1
        public int _unk13; //0

        public ADPCMInfo _adpcInfo;

        public ADPCMInfo* Info { get { return (ADPCMInfo*)(Address + 0x3C); } }
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public int NumSamples { get { return (_nibbles / 16 * 14) + ((_nibbles % 16) - 2); } }
    }
}
