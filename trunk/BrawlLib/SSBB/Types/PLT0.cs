using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;
using BrawlLib.Wii.Textures;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PLT0
    {
        public const int Size = 0x40;
        public const uint Tag = 0x30544C50;

        public BRESCommonHeader _bresEntry;
        public buint _headerLen;
        public buint _stringOffset;
        public buint _pixelFormat;
        public bshort _numEntries;
        bushort _unk;
        fixed uint _padding[8];

        private PLT0* Address { get { fixed (PLT0* ptr = &this)return ptr; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (uint)value - (uint)this.Address; }
        }
        public VoidPtr PaletteData { get { return (VoidPtr)this.Address + _headerLen; } }
        public WiiPaletteFormat PaletteFormat
        {
            get { return (WiiPaletteFormat)(uint)_pixelFormat; }
            set { _pixelFormat = (uint)value; }
        }

        public PLT0(int length, WiiPaletteFormat format)
        {
            _bresEntry._tag = Tag;
            _bresEntry._size = (length * 2) + Size;
            _bresEntry._version = 1;
            _bresEntry._bresOffset = 0;

            _headerLen = 0x40;
            _stringOffset = 0;
            _pixelFormat = (uint)format;
            _numEntries = (short)length;
            _unk = 0;

            fixed (uint* p = _padding)
                for (int i = 0; i < 8; i++) p[i] = 0;
        }
    }
}
