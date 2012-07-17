using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    unsafe struct SHP0v3
    {
        public const uint Tag = 0x30504853;
        public const int Size = 0x28;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _stringListOffset; //List of vertex node strings
        public bint _stringOffset;
        public bint _pad; //0
        public bshort _numFrames;
        public bshort _numEntries;
        public bint _loop; //0x00, 0x01

        public SHP0v3(int loop, short frames, short entries)
        {
            _header._tag = Tag;
            _header._size = Size;
            _header._version = 3;
            _header._bresOffset = 0;

            _dataOffset = 0x28;
            _pad = 0;
            _numFrames = frames;
            _loop = loop;
            _stringOffset = 0;
            _numEntries = entries;

            _stringListOffset = 0;
            _stringOffset = 0;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }

        public bint* StringEntries { get { return (bint*)(Address + _stringListOffset); } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SHP0v4
    {
        public const uint Tag = 0x30504853;
        public const int Size = 0x28;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _stringListOffset; //List of vertex node strings
        public bint _pad;
        public bint _stringOffset;
        public bint _loop;
        public bshort _numFrames;
        public bshort _numEntries;

        public SHP0v4(int loop, short frames, short entries)
        {
            _header._tag = Tag;
            _header._size = Size;
            _header._version = 4;
            _header._bresOffset = 0;

            _dataOffset = 0x28;
            _pad = 0;
            _numFrames = frames;
            _loop = loop;
            _stringOffset = 0;
            _numEntries = entries;

            _stringListOffset = 0;
            _stringOffset = 0;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }

        public bint* StringEntries { get { return (bint*)(Address + _stringListOffset); } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SHP0Entry
    {
        public const int Size = 0x14;

        public bint _flags;
        public bint _stringOffset;
        public bshort _nameIndex;
        public bshort _numIndices;
        public bint _fixedFlags; //Bit is set if entry is fixed
        public bint _indiciesOffset;
        
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        //Aligned to 4 bytes
        public bshort* Indicies { get { return (bshort*)(Address + _indiciesOffset); } }

        public bint* EntryOffset { get { return (bint*)(Address + (_indiciesOffset - 4 * _numIndices)); } }
        public SHP0KeyframeEntries* GetEntry(int index)
        {
            bint* ptr = &EntryOffset[index];
            return (SHP0KeyframeEntries*)((VoidPtr)ptr + *ptr);
        }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SHP0KeyframeEntries
    {
        public bshort _numEntries;
        public bshort _unk1;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public BVec3* Entries { get { return (BVec3*)(Address + 4); } }
    }
}
