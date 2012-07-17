using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct THPHeader
    {
        public const uint Size = 0x30;
        public const uint Tag = 0x00504854;

        public fixed char magic[4];          // "THP\0"
        public buint version;                // version number
        public buint bufSize;                // max frame size for buffer computation
        public buint audioMaxSamples;        // max samples of audio data
        public buint frameRate;              // frame per seconds
        public buint numFrames;              // frame count
        public buint firstFrameSize;         // how much to load
        public buint movieDataSize;          // file size
        public buint compInfoDataOffsets;    // offset to component infomation data
        public buint offsetDataOffsets;      // offset to array of frame offsets
        public buint movieDataOffsets;       // offset to first frame (start of movie data) 
        public buint finalFrameDataOffsets;  // offset to final frame

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct THPAudioInfo
    {
        public buint sndChannels;
        public buint sndFrequency;
        public buint sndNumSamples;
        public buint sndNumTracks;   // number of Tracks
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct THPVideoInfo
    {
        public buint xSize;      // width  of video
        public buint ySize;      // height of video
        public buint videoType;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct THPFrameCompInfo
    {
        public buint numComponents;        // a number of Components in a frame
        public fixed char frameComp[16];   // kind of Components
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct THPFrameHeader
    {
        public buint frameSizeNext;
        public buint frameSizePrevious;
        public fixed uint comp[16];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct THPFile
    {
        THPHeader header;
        THPFrameCompInfo frameCompInfo;
        THPVideoInfo videoInfo;      // THP_COMP_VIDEO
        THPAudioInfo audioInfo;      // THP_COMP_AUDIO
    }
}
