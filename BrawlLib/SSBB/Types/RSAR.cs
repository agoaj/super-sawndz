using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RSARHeader
    {
        public const int Size = 0x40;
        public const uint Tag = 0x52415352;

        public SSBBCommonHeader _header;

        public bint _symbOffset;
        public bint _symbLength;
        public bint _infoOffset;
        public bint _infoLength;
        public bint _fileOffset;
        public bint _fileLength;
        private int _pad1, _pad2, _pad3, _pad4, _pad5, _pad6;

        public void Set(int symbLen, int infoLen, int fileLen)
        {
            int offset = 0x40;

            _header._tag = Tag;
            _header._endian = -2;
            _header._version = 0x103;
            _header._firstOffset = 0x40;
            _header._numEntries = 3;

            _symbOffset = offset;
            _symbLength = symbLen;
            _infoOffset = offset += symbLen;
            _infoLength = infoLen;
            _fileOffset = offset += infoLen;
            _fileLength = fileLen;

            _header._length = offset;
        }

        private VoidPtr Address { get { fixed (RSARHeader* ptr = &this)return ptr; } }

        public SYMBHeader* SYMBBlock { get { return (SYMBHeader*)_header.Entries[0].Address; } }
        public INFOHeader* INFOBlock { get { return (INFOHeader*)_header.Entries[1].Address; } }
        public FILEHeader* FILEBlock { get { return (FILEHeader*)_header.Entries[2].Address; } }
    }

    #region SYMB

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SYMBHeader
    {
        public const uint Tag = 0x424D5953;

        public uint _tag;
        public bint _length;
        public bint _stringOffset;

        public bint _maskOffset1; //For sounds
        public bint _maskOffset2; //For types
        public bint _maskOffset3; //For groups
        public bint _maskOffset4; //For banks

        public SYMBHeader(int length)
        {
            _tag = Tag;
            _length = length;
            _stringOffset = 0x14;
            _maskOffset1 = _maskOffset2 = _maskOffset3 = _maskOffset4 = 0;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        //public VoidPtr StringData { get { return Address + 8 + _stringOffset; } }
        public SYMBMaskHeader* MaskData1 { get { return (SYMBMaskHeader*)(Address + 8 + _maskOffset1); } }
        public SYMBMaskHeader* MaskData2 { get { return (SYMBMaskHeader*)(Address + 8 + _maskOffset2); } }
        public SYMBMaskHeader* MaskData3 { get { return (SYMBMaskHeader*)(Address + 8 + _maskOffset3); } }
        public SYMBMaskHeader* MaskData4 { get { return (SYMBMaskHeader*)(Address + 8 + _maskOffset4); } }

        public uint StringCount { get { return StringOffsets[-1]; } }
        public buint* StringOffsets { get { return (buint*)(Address + 8 + _stringOffset + 4); } }

        //Gets names of file paths seperated by an underscore
        public string GetStringEntry(int index)
        {
            if (index < 0)
                return "<null>";
            return new String((sbyte*)(Address + 8 + StringOffsets[index]));
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SYMBMaskHeader
    {
        public bint _entrySize; //unknown
        public bint _entryNum; //number of entries in block

        private VoidPtr Address { get { fixed (SYMBMaskHeader* ptr = &this)return ptr; } }
        public SYMBMaskEntry* Entries { get { return (SYMBMaskEntry*)(Address + 8); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SYMBMaskEntry
    {
        public const int Size = 0x14;

        public bint _unk1;
        public bint _unk2;
        public bint _unk3;
        public bint _stringId;
        public bint _index;

        public SYMBMaskEntry(int v1, int v2, int v3, int v4, int v5)
        { _unk1 = v1; _unk2 = v2; _unk3 = v3; _stringId = v4; _index = v5; }
    }

    #endregion

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOHeader
    {
        public const uint Tag = 0x6F464E49;

        public SSBBEntryHeader _header;
        public RuintCollection _collection;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public RuintList* Sounds { get { return (RuintList*)_collection[0]; } }
        public RuintList* Banks { get { return (RuintList*)_collection[1]; } }
        public RuintList* Types { get { return (RuintList*)_collection[2]; } }
        public RuintList* Files { get { return (RuintList*)_collection[3]; } }
        public RuintList* Groups { get { return (RuintList*)_collection[4]; } }

        public INFOFooter* Footer { get { return (INFOFooter*)_collection[5]; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOFooter
    {
        bshort _unk1; //8
        bshort _unk2; //16
        bshort _unk3; //4
        bshort _unk4; //4
        bshort _unk5; //8
        bshort _unk6; //32
        bshort _unk7; //32
        bshort _unk8; //0
    }

    #region Sounds
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOSoundEntry
    {
        public bint _stringId;
        public bint _fileId;
        public bint _unk1; // 0
        public ruint _part1Offset; //control 1
        public byte _flag1; //0x20
        public byte _flag2; //0x40
        public byte _flag3; //0x03
        public byte _flag4; //0x00
        public ruint _part2Offset; //control 0x0103
        public bint _unk2; //0
        public bint _unk3; //0
        public bint _unk4; //0

        public INFOSoundPart1* GetPart1(VoidPtr baseAddr) { return (INFOSoundPart1*)(baseAddr + _part1Offset); }
        public INFOSoundPart2* GetPart2(VoidPtr baseAddr) { return (INFOSoundPart2*)(baseAddr + _part2Offset); }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOSoundPart2
    {
        public bint _soundIndex;
        public bint _unk1;
        public bint _unk2;
        public bint _unk3;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOSoundPart1
    {
        public bint _unk1;
        public bint _unk2;
        public bint _unk3;
    }
    #endregion

    #region Banks
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOBankEntry
    {
        public bint _stringId;
        public bint _fileId;
        public bint _padding;
    }
    #endregion

    #region Types
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOTypeEntry
    {
        public bint _typeId;
        public uint _flags;
        public buint _unk1; //always 0
        public buint _unk2; //always 0
    }
    #endregion

    #region Files

    //Files can be a group of raw sounds, sequences, or external audio streams.
    //When they are audio streams, they can be loaded as BGMs using external files, referenced by the _stringOffset field.
    //When they are raw sounds (RWSD), they contain sounds used in action scripts (usually mono).
    //Need more info on sequences and banks.

    //Files can be referenced multiple times using loading groups. The _listOffset field contains a list of those references.
    //When a file is referenced by a group, it is copied to each group's header and data block.

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOFileHeader
    {
        public bint _headerLen; //Includes padding. Set to file size if external file.
        public bint _dataLen; //Includes padding. Zero if external file.
        public bint _magic; //-1
        public ruint _stringOffset; //External file path, only for BGMs. Path is relative to sound folder
        public ruint _listOffset; //List of groups this file belongs to. Empty if external file.

        public RuintList* GetList(VoidPtr baseAddr) { return (RuintList*)(baseAddr + _listOffset); }

        public string GetPath(VoidPtr baseAddr) { return (_stringOffset == 0) ? null : new String((sbyte*)(baseAddr + _stringOffset)); }
    }

    //Attached to a RuintList from INFOSetHeader
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOFileEntry
    {
        public bint _groupId;
        public bint _index;

        public int GroupId { get { return _groupId; } set { _groupId = value; } }
        public int Index { get { return _index; } set { _index = value; } }

        public override string ToString()
        {
            return String.Format("[{0}, {1}]", GroupId, Index);
        }
    }

    #endregion

    #region Groups

    //Groups are a collection of sound files.
    //Files can appear in multiple groups, but the data is actually copied to each group.
    //Groups are laid out in two blocks, first the header block, then the data block.
    //The header block holds all the headers belonging to each file, in sequential order.
    //The data block holds all the audio data belonging to each file, in sequential order.
    //Data referenced in the WAVE section is relative to the file's data, not the whole group block.
    //This means that the headers/data can simply be copied without changing anything.

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOGroupHeader
    {
        public bint _id; //string id
        public bint _magic; //always -1
        public bint _unk1; //always 0
        public bint _unk2; //always 0
        public bint _headerOffset; //Absolute offset from RSAR file. //RWSD Location
        public bint _headerLength; //Total length of all headers in contained sets.
        public bint _dataOffset; //Absolute offset from RSAR file.
        public bint _dataLength; //Total length of all data in contained sets.
        public ruint _listOffset;

        public RuintList* GetCollection(VoidPtr offset) { return (RuintList*)(offset + _listOffset); }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOGroupEntry
    {
        public bint _fileId;
        public bint _headerOffset;
        public bint _headerLength;
        public bint _dataOffset;
        public bint _dataLength;
        public bint _unk;

        public override string ToString()
        {
            return String.Format("[{0:X}]", (uint)_fileId);
        }
    }

    #endregion

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct FILEHeader
    {
        public const uint Tag = 0x454C4946;

        public uint _tag;
        public bint _length;
        private int _p1, _p2, _p3, _p4, _p5, _p6;

        public void Set(int length)
        {
            _tag = Tag;
            _length = length;
            _p1 = _p2 = _p3 = _p4 = _p5 = _p6 = 0;
        }
    }
}
