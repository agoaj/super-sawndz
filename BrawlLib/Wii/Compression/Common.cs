using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Compression
{
    public enum CompressionType : byte
    {
        None = 0x0,
        LZ77 = 0x1,
        Huffman = 0x2,
        RunLength = 0x3,
        Differential = 0x8
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CompressionHeader
    {
        private uint _data;

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public CompressionType Algorithm
        {
            get { return (CompressionType)(_data >> 4 & 0xF); }
            set { _data = (_data & 0xFFFFFF0F) | (((uint)value & 0x0F) << 4); }
        }
        public int Parameter
        {
            get { return (int)(_data & 0x0F); }
            set { _data = (_data & 0xFFFFFFF0) | ((uint)value & 0x0F); }
        }
        public int ExpandedSize
        {
            get  {return (int)(_data >> 8); }
            set { _data = ((uint)value << 8) | (_data & 0xFF); }
        }

        public VoidPtr Data { get { return Address + 4; } }
    }
}
