using System;

namespace BrawlLib.Imaging
{
    public interface IColorSource
    {
        bool HasPrimary { get; }
        ARGBPixel PrimaryColor { get; set; }
        string PrimaryColorName { get; }

        int ColorCount { get; }
        ARGBPixel GetColor(int index);
        void SetColor(int index, ARGBPixel color);
    }
}
