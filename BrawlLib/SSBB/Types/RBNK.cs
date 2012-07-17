using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RBNKHeader
    {
        public const uint Tag = 0x4B4E4252;

        public SSBBCommonHeader _header;

        public bint _dataOffset;
        public bint _dataLength;
        public bint _waveOffset;
        public bint _waveLength;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public RBNK_DATAHeader* Data { get { return (RBNK_DATAHeader*)(Address + _dataOffset); } }
        public RWSD_WAVEHeader* Wave { get { return (RWSD_WAVEHeader*)(Address + _waveOffset); } } //Uses same format as RWSD
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RBNK_DATAHeader
    {
        public const uint Tag = 0x41544144;

        public uint _tag;
        public bint _length;
        public RuintList _list; //control == 0x0102

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RBNK_DATAEntry
    {
    }
}
