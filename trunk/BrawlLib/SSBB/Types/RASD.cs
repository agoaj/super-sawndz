using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    unsafe struct RASD
    {
        public const uint Tag = 0x44534152;

        public SSBBCommonHeader _header;

        //These probably repeat for the amount of entries
        public buint _dataOffset;
        public buint _entryLength;
        
        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

    }
}
