using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using BrawlLib.Imaging;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe struct CLR0
    {
        public const int Size = 0x24;
        public const int Tag = 0x30524C43;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _stringOffset;
        public bint _unk1;
        public bshort _frames;
        public bshort _entries;
        public bint _unk2;

        public CLR0(int size, int unk1, int frames, int entries, int unk2)
        {
            _header._tag = Tag;
            _header._size = size;
            _header._bresOffset = 0;
            _header._version = 3;

            _dataOffset = Size;
            _stringOffset = 0;
            _unk1 = unk1;
            _frames = (short)frames;
            _entries = (short)entries;
            _unk2 = unk2;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
        public string ResourceString2 { get { return new String((sbyte*)this.ResourceStringAddress2); } }
        public VoidPtr ResourceStringAddress2
        {
            get { return (VoidPtr)this.Address + _unk1; }
            set { _unk1 = (int)value - (int)Address; }
        }
    }

    [Flags]
    public enum CLR0EntryFlags : uint
    {
        Flag1       = 0x1,
        IsSolid1    = 0x2,
        Unk1        = 0x4,
        Unk2        = 0x8,
        Unk3        = 0x10,
        Unk4        = 0x20,
        Unk5        = 0x40,
        Unk6        = 0x80,
        Flag2       = 0x100,
        IsSolid2    = 0x200,
        Unk7        = 0x400,
        Unk8        = 0x800,
        Unk9        = 0x1000,
        Unk10       = 0x2000,
        Flag3       = 0x4000,
        IsSolid3    = 0x8000,
        Unk12       = 0x10000,
        Unk13       = 0x20000,
        Unk14       = 0x40000,
        Unk15       = 0x80000,
        Flag4       = 0x100000,
        Unk16       = 0x200000,
        Unk17       = 0x400000,
        Unk18       = 0x800000,
        Unk19       = 0x1000000,
        Unk20       = 0x2000000,
        Unk21       = 0x4000000,
        Unk22       = 0x8000000,
        Unk23       = 0x10000000,
        Unk24       = 0x20000000,
        Unk25       = 0x40000000,
        Unk26       = 0x80000000,

        IsSolid     = 0x8202,
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct CLR0Entry
    {
        public const int Size = 0x10;

        public bint _stringOffset;
        public buint _flags; //1 Block count?
        public ABGRPixel _colorMask; //Used as a mask for source color before applying frames
        public bint _data; //Could be offset or color! Offset from itself

        public CLR0Entry(CLR0EntryFlags flags, ABGRPixel mask, int offset)
        {
            _stringOffset = 0;
            _flags = (uint)flags;
            _colorMask = mask;
            _data = offset;
        }
        public CLR0Entry(CLR0EntryFlags flags, ABGRPixel mask, ABGRPixel color)
        {
            _stringOffset = 0;
            _flags = (uint)flags;
            _colorMask = mask;
            _data._data = *(int*)&color;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ABGRPixel* Data { get { return (ABGRPixel*)(Address + _data + 12); } }

        public CLR0EntryFlags Flags { get { return (CLR0EntryFlags)(uint)_flags; } set { _flags = (uint)value; } }
        public ABGRPixel SolidColor { get { return *(ABGRPixel*)(Address + 12); } set { *(ABGRPixel*)(Address + 12) = value; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }
}
