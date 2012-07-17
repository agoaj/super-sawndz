using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using BrawlLib.Wii.Textures;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct REFT
    {
        //Header + string is aligned to 4 bytes

        public const uint Tag = 0x54464552;

        public SSBBCommonHeader _header;
        public uint _tag; //Same as header
        public bint _dataLength; //Size of second REFT block. (file size - 0x18)
        public bint _dataOffset; //Offset from itself. Begins first entry
        public bint _unk1; //0
        public bint _unk2; //0
        public bshort _stringLen;
        public bshort _unk3; //0

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public string IdString
        {
            get { return new String((sbyte*)Address + 0x28); }
            set
            {
                int len = value.Length + 1;
                _stringLen = (short)len;

                byte* dPtr = (byte*)Address + 0x28;
                fixed (char* sPtr = value)
                {
                    for (int i = 0; i < len; i++)
                        *dPtr++ = (byte)sPtr[i];
                }

                //Align to 4 bytes
                while ((len++ & 3) != 0)
                    *dPtr++ = 0;
            }
        }

        public REFTypeObjectTable* Table { get { return (REFTypeObjectTable*)(Address + 0x18 + _dataOffset); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct REFTData
    {
        public buint _unknown;
        public bushort _width;
        public bushort _height;
        public buint _imagelen;
        public byte _format;
        public byte _pltFormat;
        public bushort _colorCount;
        public fixed byte pad[16];

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public REFTData(ushort width, ushort height, byte format)
        {
            _unknown = 0;
            _width = width;
            _height = height;
            _imagelen = 0;
            _format = format;
            _pltFormat = 0;
            _colorCount = 0;
        }

        //From here starts the image.

        public VoidPtr PaletteData { get { return Address + 0x20 + _imagelen; } }
    }
}
