using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.Wii.Textures
{
    public enum WiiPixelFormat : uint
    {
        I4 = 0,
        I8 = 1,
        IA4 = 2,
        IA8 = 3,
        RGB565 = 4,
        RGB5A3 = 5,
        RGBA8 = 6,
        CI4 = 8,
        CI8 = 9,
        CI14X2 = 10,
        CMPR = 14
    }

    public enum WiiPaletteFormat : uint
    {
        IA8 = 0,
        RGB565 = 1,
        RGB5A3 = 2
    }
}
