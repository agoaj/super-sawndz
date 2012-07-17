using System;

namespace System
{
    public struct BBVS
    {
        public const uint Tag = 0x53564242;
        public const uint Size = 0x40;

        public uint _tag;
        public byte _version;
        public byte _options; //bits
        //0000 0001 = Retrieve corresponding animations
        //0000 0010 = Warn if frame counts differ
        //0000 0100 = Sync loop to anim
        //0000 1000 = Sync tex to obj
        //0001 0000 = Sync obj to vis0
        //0010 0000 = Disable bones on play
        //0100 0000 = Maximized at start
        //1000 0000 = Sync FCs by default
        public short pad1;
        public bfloat tScale, rScale, zScale, _nearZ, _farz, yFov;
        public BVec4 amb, pos, diff, spec;
        public BVec3 defaultCam;
        public int pad2;
        
        public bool RetrieveCorrAnims { get { return (_options >> 0 & 1) != 0; } }
        public bool WarnIfFCsDiffer { get { return (_options >> 1 & 1) != 0; } }
        public bool SyncLoopToAnim { get { return (_options >> 2 & 1) != 0; } }
        public bool SyncTexToObj { get { return (_options >> 3 & 1) != 0; } }
        public bool SyncObjToVIS0 { get { return (_options >> 4 & 1) != 0; } }
        public bool DisableBonesOnPlay { get { return (_options >> 5 & 1) != 0; } }
        public bool Maximize { get { return (_options >> 6 & 1) != 0; } }
        public bool SyncFCs { get { return (_options >> 7 & 1) != 0; } }

        public void SetOptions(bool a, bool b, bool c, bool d, bool e, bool f, bool g, bool h)
        {
            _options = (byte)(
                ((a ? 1 : 0) << 0) |
                ((b ? 1 : 0) << 1) |
                ((c ? 1 : 0) << 2) |
                ((d ? 1 : 0) << 3) |
                ((e ? 1 : 0) << 4) |
                ((f ? 1 : 0) << 5) |
                ((g ? 1 : 0) << 6) |
                ((h ? 1 : 0) << 7));
        }
    }
}
