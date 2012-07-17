using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [Flags]
    public enum VIS0Flags : int
    {
        None = 0x00,
        Enabled = 0x01,
        Constant = 0x02
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct VIS0v3
    {
        public const uint Tag = 0x30534956;
        public const int Size = 0x24;

        public BRESCommonHeader _header;
        public bint _dataOffset; //0x24
        public bint _stringOffset;
        public bint _pad;
        public bushort _numFrames;
        public bushort _numEntries;
        public bint _loop;

        public VIS0v3(int size, ushort frameCount, ushort numEntries, int loop)
        {
            _header._tag = Tag;
            _header._size = size;
            _header._version = 3;
            _header._bresOffset = 0;
            _dataOffset = 0x24;
            _stringOffset = 0;
            _pad = 0;
            _numFrames = frameCount;
            _numEntries = numEntries;
            _loop = loop;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct VIS0v4
    {
        public const uint Tag = 0x30534956;
        public const int Size = 0x28;

        public BRESCommonHeader _header;
        public bint _dataOffset; //0x28
        public bint _pad1; //0
        public bint _stringOffset;
        public bint _pad2; //0
        public bushort _numFrames;
        public bushort _numEntries;
        public bint _loop;
        
        public VIS0v4(int size, ushort frameCount, ushort numEntries, int loop)
        {
            _header._tag = Tag;
            _header._size = size;
            _header._version = 4;
            _header._bresOffset = 0;
            _dataOffset = 0x28;
            _stringOffset = 0;
            _pad1 = _pad2 = 0;
            _numFrames = frameCount;
            _numEntries = numEntries;
            _loop = loop;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct VIS0Entry
    {
        public const int Size = 8;

        public bint _stringOffset;
        public bint _flags;

        public VIS0Entry(VIS0Flags flags)
        {
            _stringOffset = 0;
            _flags = (int)flags;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public VoidPtr Data { get { return Address + 8; } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }

        public VIS0Flags Flags { get { return (VIS0Flags)(int)_flags; } set { _flags = (int)value; } }
    }
}
