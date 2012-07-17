using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace BrawlLib.OpenGL
{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    internal struct PixelFormatDescriptor
    {
        public ushort nSize;
        public ushort nVersion;
        public PixelFlags dwFlags;
        public byte iPixelType;
        public byte cColorBits;
        public byte cRedBits;
        public byte cRedShift;
        public byte cGreenBits;
        public byte cGreenShift;
        public byte cBlueBits;
        public byte cBlueShift;
        public byte cAlphaBits;
        public byte cAlphaShift;
        public byte cAccumBits;
        public byte cAccumRedBits;
        public byte cAccumGreenBits;
        public byte cAccumBlueBits;
        public byte cAccumAlphaBits;
        public byte cDepthBits;
        public byte cStencilBits;
        public byte cAuxBuffers;
        public sbyte iLayerType;
        public byte bReserved;
        public uint dwLayerMask;
        public uint dwVisibleMask;
        public uint dwDamageMask;

        public PixelFormatDescriptor(byte colorBits, byte depthBits)
        {
            nSize = 40;
            nVersion = 1;
            dwFlags = PixelFlags.DoubleBuffer | PixelFlags.DrawToWindow | PixelFlags.SupportOpenGL;
            iPixelType = 0;
            cColorBits = colorBits;
            cRedBits = 0;
            cRedShift = 0;
            cGreenBits = 0;
            cGreenShift = 0;
            cBlueBits = 0;
            cBlueShift = 0;
            cAlphaBits = 0;
            cAlphaShift = 0;
            cAccumBits = 0;
            cAccumRedBits = 0;
            cAccumGreenBits = 0;
            cAccumBlueBits = 0;
            cAccumAlphaBits = 0;
            cDepthBits = depthBits;
            cStencilBits = 0;
            cAuxBuffers = 0;
            iLayerType = 0;
            bReserved = 0;
            dwLayerMask = 0;
            dwVisibleMask = 0;
            dwDamageMask = 0;
        }
    }

    //public enum PixelType : byte
    //{
    //    RGBA = 0x00,
    //    ColorIndex = 0x01
    //}
    public enum PixelFlags : uint
    {
        DoubleBuffer = 0x00000001,
        Stereo = 0x00000002,
        DrawToWindow = 0x00000004,
        DrawToBitmap = 0x00000008,
        SupportGDI = 0x00000010,
        SupportOpenGL = 0x00000020,
        GenericFormat = 0x00000040,
        NeedPalette = 0x00000080,
        NeedSystemPalette = 0x00000100,
        SwapExchange = 0x00000200,
        SwapCopy = 0x00000400,
        SwapLayerBuffers = 0x00000800,
        GenericAccelerated = 0x00001000,
        SupportDirectDraw = 0x00002000
    }
}
