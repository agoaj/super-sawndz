using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ROOTHeader
    {
        public const int Tag = 0x746F6F72;

        public uint _tag;
        public bint _size;
        public ResourceGroup _master;

        //public BRESHeader* BRESHeader
        //{
        //    get
        //    {
        //        uint* ptr;
        //        fixed (ROOTHeader* p = &this)
        //            for (ptr = (uint*)p; *ptr != SmashBox.BRESHeader.Tag; ptr--) ;
        //        return (BRESHeader*)ptr;
        //    }
        //}

        public ROOTHeader(int size, int numEntries)
        {
            _tag = Tag;
            _size = size;
            _master = new ResourceGroup(numEntries);
        }
    }
}
