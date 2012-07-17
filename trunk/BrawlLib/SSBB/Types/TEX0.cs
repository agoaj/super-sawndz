using System;
using System.Runtime.InteropServices;
using BrawlLib.Wii.Textures;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TEX0
    {
        public const int Size = 0x40;
        public const uint Tag = 0x30584554;

        public BRESCommonHeader _header;
        public bint _headerLen;
        public bint _stringOffset;
        public bint _hasPalette;
        public bshort _width;
        public bshort _height;
        public bint _pixelFormat;
        public bint _levelOfDetail;
        public bint _unknown;
        public bfloat _lodBias;
        fixed uint _padding[4];

        internal VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
        public VoidPtr PixelData { get { return (VoidPtr)Address + _headerLen; } }
        public WiiPixelFormat PixelFormat
        {
            get { return (WiiPixelFormat)(int)_pixelFormat; }
            set { _pixelFormat = (int)value; }
        }
        public bool HasPalette
        {
            get { return _hasPalette != 0; }
            set { _hasPalette = (value) ? 1 : 0; }
        }

        public TEX0(int width, int height, WiiPixelFormat format, int mipLevels)
        {
            _header._tag = Tag;
            _header._size = TextureConverter.Get(format).GetMipOffset(width, height, mipLevels + 1) + Size;
            _header._version = 1;
            _header._bresOffset = 0;

            _headerLen = Size;
            _stringOffset = 0;
            _hasPalette = ((format == WiiPixelFormat.CI4) || (format == WiiPixelFormat.CI8)) ? 1 : 0;
            _width = (short)width;
            _height = (short)height;
            _pixelFormat = (int)format;
            _levelOfDetail = mipLevels;
            _unknown = 0;
            _lodBias = mipLevels - 1.0f;

            fixed (uint* p = _padding)
                for (int i = 0; i < 4; i++) p[i] = 0;
        }
    }
}
