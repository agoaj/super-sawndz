using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Drawing2D;
using BrawlLib.SSBBTypes;
using BrawlLib.Imaging;
using BrawlLib.IO;

namespace BrawlLib.Wii.Textures
{
    public unsafe abstract class TextureConverter
    {
        public static readonly TextureConverter I4 = new I4();
        public static readonly TextureConverter IA4 = new IA4();
        public static readonly TextureConverter I8 = new I8();
        public static readonly TextureConverter IA8 = new IA8();
        public static readonly TextureConverter RGB565 = new RGB565();
        public static readonly TextureConverter RGB5A3 = new RGB5A3();
        public static readonly TextureConverter CI4 = new CI4();
        public static readonly TextureConverter CI8 = new CI8();
        public static readonly CMPR CMPR = new CMPR();
        public static readonly TextureConverter RGBA8 = new RGBA8();

        public abstract WiiPixelFormat RawFormat { get; }
        //public abstract PixelFormat DecodedFormat { get; }
        public abstract int BitsPerPixel { get; }
        public abstract int BlockWidth { get; }
        public abstract int BlockHeight { get; }
        public bool IsIndexed { get { return ((RawFormat == WiiPixelFormat.CI4) || (RawFormat == WiiPixelFormat.CI8)); } }

        protected ColorPalette _workingPalette;

        public int GetMipOffset(int width, int height, int mipLevel) { return GetMipOffset(ref width, ref height, mipLevel); }
        public int GetMipOffset(ref int width, ref int height, int mipLevel)
        {
            int offset = 0;
            while (mipLevel-- > 1)
            {
                offset += ((width.Align(BlockWidth) * height.Align(BlockHeight)) * BitsPerPixel) >> 3;
                width = Math.Max(width >> 1, 1);
                height = Math.Max(height >> 1, 1);
            }
            return offset;
        }
        public int GetFileSize(int width, int height, int mipLevels, bool REFT)
        {
            return GetMipOffset(width, height, mipLevels + 1) + (REFT ? 0x20 : 0x40);
        }

        //public virtual void GeneratePreviewIndexed(Bitmap src, Bitmap dst, int numColors, WiiPaletteFormat format)
        //{
        //    _cachedPalette = src.GeneratePalette(QuantizationAlgorithm.WeightedAverage, numColors);
        //    _paletteFormat = format;
        //    _cachedPalette.Clamp(format);

        //    src.CopyTo(dst);
        //    dst.Clamp(_cachedPalette);
        //}

        //public virtual void GeneratePreview(Bitmap src, Bitmap dst)
        //{
        //    src.CopyTo(dst);
        //    dst.Clamp(RawFormat);
        //}

        public virtual FileMap EncodeTextureIndexed(Bitmap src, int mipLevels, int numColors, WiiPaletteFormat format, QuantizationAlgorithm algorithm, out FileMap paletteFile)
        {
            using (Bitmap indexed = src.Quantize(algorithm, numColors, RawFormat, format, null))
            {
                return EncodeTextureIndexed(indexed, mipLevels, format, out paletteFile);
            }
            //ColorPalette pal = src.GeneratePalette(algorithm, numColors);
            //pal.Clamp(format);
            //return EncodeTextureIndexed(src, mipLevels, pal, format, out paletteFile);
        }
        //public virtual FileMap EncodeTextureIndexed(Bitmap src, int mipLevels, ColorPalette palette, WiiPaletteFormat format, out FileMap paletteFile)
        //{
        //    _workingPalette = palette;
        //    FileMap map = EncodeTexture(src, mipLevels);
        //    paletteFile = EncodePalette(palette, format);
        //    _workingPalette = null;
        //    return map;
        //}

        public virtual FileMap EncodeREFTTextureIndexed(Bitmap src, int mipLevels, WiiPaletteFormat format)
        {
            if (!src.IsIndexed())
                throw new ArgumentException("Source image must be indexed.");

            FileMap texMap = EncodeREFTTexture(src, mipLevels, format, true);
            return texMap;
        }
        public virtual FileMap EncodeTextureIndexed(Bitmap src, int mipLevels, WiiPaletteFormat format, out FileMap paletteFile)
        {
            if (!src.IsIndexed())
                throw new ArgumentException("Source image must be indexed.");

            FileMap texMap = EncodeTexture(src, mipLevels);
            paletteFile = EncodePalette(src.Palette, format);
            return texMap;

            //int w = src.Width, h = src.Height;
            //int bw = BlockWidth, bh = BlockHeight;
            //int aw = w.Align(bw), ah = h.Align(bh);

            //paletteFile = null;
            //FileMap texFile = FileMap.FromTempFile(GetFileSize(w, h, mipLevels));
            //try
            //{
            //    //Build TEX header
            //    TEX0* header = (TEX0*)texFile.Address;
            //    *header = new TEX0(w, h, RawFormat, mipLevels);

            //    int sStep = bw * Image.GetPixelFormatSize(src.PixelFormat) / 8;
            //    int dStep = bw * bh * BitsPerPixel / 8;
            //    VoidPtr baseAddr = header->PixelData;
            //    using (DIB dib = DIB.FromBitmap(src, bw, bh, src.PixelFormat))
            //    {
            //        for (int i = 1; i <= mipLevels; i++)
            //        {
            //            int mw = w, mh = h;
            //            VoidPtr dAddr = baseAddr;
            //            //int mw = dib.Width, mh = dib.Height, aw = mw.Align(BlockWidth);
            //            //VoidPtr dstAddr = header->PixelData;
            //            if (i != 1)
            //            {
            //                dAddr += GetMipOffset(ref mw, ref mh, i);
            //                using (Bitmap mip = src.GenerateMip(i))
            //                {
            //                    dib.ReadBitmap(mip, mw, mh);
            //                }
            //            }

            //            mw = mw.Align(bw);
            //            mh = mh.Align(bh);

            //            int bStride = mw * BitsPerPixel / 8;
            //            for (int y = 0; y < mh; y += bh)
            //            {
            //                VoidPtr sPtr = (int)dib.Scan0 + (y * dib.Stride);
            //                VoidPtr dPtr = dAddr + (y * bStride);
            //                for (int x = 0; x < mw; x += bw, dPtr += dStep, sPtr += sStep)
            //                    EncodeBlock((ARGBPixel*)sPtr, dPtr, aw);
            //            }
            //        }
            //    }

            //    paletteFile = EncodePalette(src.Palette, format);
            //    return texFile;
            //}
            //catch (Exception x)
            //{
            //    texFile.Dispose();
            //    return null;
            //}
        }

        public virtual FileMap EncodeREFTTexture(Bitmap src, int mipLevels, WiiPaletteFormat format, bool usePalette)
        {
            int w = src.Width, h = src.Height;
            int bw = BlockWidth, bh = BlockHeight;
            //int aw = w.Align(bw), ah = h.Align(bh);
            ColorPalette pal = src.Palette;

            PixelFormat fmt = src.IsIndexed() ? src.PixelFormat : PixelFormat.Format32bppArgb;

            //int fileSize = GetMipOffset(w, h, mipLevels + 1) + 0x20;
            FileMap fileView = FileMap.FromTempFile(GetFileSize(w, h, mipLevels, true) + (usePalette ? (pal.Entries.Length * 2) : 0));
            //FileMap fileView = FileMap.FromTempFile(fileSize);
            try
            {
                //Build REFT image header
                REFTData* header = (REFTData*)fileView.Address;
                *header = new REFTData((ushort)w, (ushort)h, (byte)RawFormat);
                header->_imagelen = (uint)fileView.Length - 0x20;

                int sStep = bw * Image.GetPixelFormatSize(fmt) / 8;
                int dStep = bw * bh * BitsPerPixel / 8;
                VoidPtr baseAddr = (byte*)header + 0x20;

                using (DIB dib = DIB.FromBitmap(src, bw, bh, fmt))
                    for (int i = 1; i <= mipLevels; i++)
                        EncodeLevel(baseAddr, dib, src, dStep, sStep, i);

                if (usePalette)
                {
                    int count = pal.Entries.Length;

                    header->_colorCount = (ushort)count;
                    header->_pltFormat = (byte)format;

                    switch (format)
                    {
                        case WiiPaletteFormat.IA8:
                            {
                                IA8Pixel* dPtr = (IA8Pixel*)header->PaletteData;
                                for (int i = 0; i < count; i++)
                                    dPtr[i] = (IA8Pixel)pal.Entries[i];
                                break;
                            }
                        case WiiPaletteFormat.RGB565:
                            {
                                wRGB565Pixel* dPtr = (wRGB565Pixel*)header->PaletteData;
                                for (int i = 0; i < count; i++)
                                    dPtr[i] = (wRGB565Pixel)pal.Entries[i];
                                break;
                            }
                        case WiiPaletteFormat.RGB5A3:
                            {
                                wRGB5A3Pixel* dPtr = (wRGB5A3Pixel*)header->PaletteData;
                                for (int i = 0; i < count; i++)
                                    dPtr[i] = (wRGB5A3Pixel)pal.Entries[i];
                                break;
                            }
                    }
                }

                return fileView;
            }
            catch (Exception x)
            {
                //MessageBox.Show(x.ToString());
                fileView.Dispose();
                return null;
            }
        }

        public virtual FileMap EncodeTexture(Bitmap src, int mipLevels)
        {
            int w = src.Width, h = src.Height;
            int bw = BlockWidth, bh = BlockHeight;
            //int aw = w.Align(bw), ah = h.Align(bh);

            PixelFormat fmt = src.IsIndexed() ? src.PixelFormat : PixelFormat.Format32bppArgb;

            //int fileSize = GetMipOffset(w, h, mipLevels + 1) + 0x40;
            FileMap fileView = FileMap.FromTempFile(GetFileSize(w, h, mipLevels, false));
            //FileMap fileView = FileMap.FromTempFile(fileSize);
            try
            {
                //Build TEX header
                TEX0* header = (TEX0*)fileView.Address;
                *header = new TEX0(w, h, RawFormat, mipLevels);

                int sStep = bw * Image.GetPixelFormatSize(fmt) / 8;
                int dStep = bw * bh * BitsPerPixel / 8;
                VoidPtr baseAddr = header->PixelData;

                using (DIB dib = DIB.FromBitmap(src, bw, bh, fmt))
                    for (int i = 1; i <= mipLevels; i++)
                        EncodeLevel(header, dib, src, dStep, sStep, i);

                return fileView;
            }
            catch (Exception x)
            {
                //MessageBox.Show(x.ToString());
                fileView.Dispose();
                return null;
            }
        }
        internal virtual void EncodeLevel(TEX0* header, DIB dib, Bitmap src, int dStep, int sStep, int level)
        {
            int mw = dib.Width, mh = dib.Height, aw = mw.Align(BlockWidth);
            VoidPtr dstAddr = header->PixelData;
            if (level != 1)
            {
                dstAddr += GetMipOffset(ref mw, ref mh, level);
                using (Bitmap mip = src.GenerateMip(level))
                    dib.ReadBitmap(mip, mw, mh);
            }

            mw = mw.Align(BlockWidth);
            mh = mh.Align(BlockHeight);

            int bStride = mw * BitsPerPixel / 8;
            for (int y = 0; y < mh; y += BlockHeight)
            {
                VoidPtr sPtr = (int)dib.Scan0 + (y * dib.Stride);
                VoidPtr dPtr = dstAddr + (y * bStride);
                for (int x = 0; x < mw; x += BlockWidth, dPtr += dStep, sPtr += sStep)
                    EncodeBlock((ARGBPixel*)sPtr, dPtr, aw);
            }
        }

        internal virtual void EncodeLevel(VoidPtr dstAddr, DIB dib, Bitmap src, int dStep, int sStep, int level)
        {
            int mw = dib.Width, mh = dib.Height, aw = mw.Align(BlockWidth);
            if (level != 1)
            {
                dstAddr += GetMipOffset(ref mw, ref mh, level);
                using (Bitmap mip = src.GenerateMip(level))
                    dib.ReadBitmap(mip, mw, mh);
            }

            mw = mw.Align(BlockWidth);
            mh = mh.Align(BlockHeight);

            int bStride = mw * BitsPerPixel / 8;
            for (int y = 0; y < mh; y += BlockHeight)
            {
                VoidPtr sPtr = (int)dib.Scan0 + (y * dib.Stride);
                VoidPtr dPtr = dstAddr + (y * bStride);
                for (int x = 0; x < mw; x += BlockWidth, dPtr += dStep, sPtr += sStep)
                    EncodeBlock((ARGBPixel*)sPtr, dPtr, aw);
            }
        }

        protected abstract void EncodeBlock(ARGBPixel* sPtr, VoidPtr blockAddr, int width);

        public Bitmap DecodeTexture(TEX0* texture) { return DecodeTexture(texture, 1); }
        public virtual Bitmap DecodeTexture(TEX0* texture, int mipLevel)
        {
            int w = (int)(ushort)texture->_width, h = (int)(ushort)texture->_height;
            VoidPtr addr = texture->PixelData + GetMipOffset(ref w, ref h, mipLevel);
            int aw = w.Align(BlockWidth), ah = h.Align(BlockHeight);

            using (DIB dib = new DIB(w, h, BlockWidth, BlockHeight, PixelFormat.Format32bppArgb))
            {
                int sStep = BlockWidth * BlockHeight * BitsPerPixel / 8;
                int bStride = aw * BitsPerPixel / 8;
                for (int y = 0; y < ah; y += BlockHeight)
                {
                    ARGBPixel* dPtr = (ARGBPixel*)dib.Scan0 + (y * aw);
                    VoidPtr sPtr = addr + (y * bStride);
                    for (int x = 0; x < aw; x += BlockWidth, dPtr += BlockWidth, sPtr += sStep)
                        DecodeBlock(sPtr, dPtr, aw);
                }
                return dib.ToBitmap();
            }
        }

        public static Bitmap Decode(VoidPtr addr, int w, int h, int mipLevel, WiiPixelFormat fmt) { return Get(fmt).DecodeTexture(addr, w, h, mipLevel); }
        public virtual Bitmap DecodeTexture(VoidPtr addr, int w, int h, int mipLevel)
        {
            int aw = w.Align(BlockWidth), ah = h.Align(BlockHeight);

            using (DIB dib = new DIB(w, h, BlockWidth, BlockHeight, PixelFormat.Format32bppArgb))
            {
                int sStep = BlockWidth * BlockHeight * BitsPerPixel / 8;
                int bStride = aw * BitsPerPixel / 8;
                for (int y = 0; y < ah; y += BlockHeight)
                {
                    ARGBPixel* dPtr = (ARGBPixel*)dib.Scan0 + (y * aw);
                    VoidPtr sPtr = addr + (y * bStride);
                    for (int x = 0; x < aw; x += BlockWidth, dPtr += BlockWidth, sPtr += sStep)
                        DecodeBlock(sPtr, dPtr, aw);
                }
                return dib.ToBitmap();
            }
        }

        public virtual Bitmap DecodeTextureIndexed(TEX0* texture, PLT0* palette, int mipLevel)
        {
            return DecodeTextureIndexed(texture, DecodePalette(palette), mipLevel);
        }
        public virtual Bitmap DecodeTextureIndexed(TEX0* texture, ColorPalette palette, int mipLevel)
        {
            _workingPalette = palette;
            try { return DecodeTexture(texture, mipLevel); }
            finally { _workingPalette = null; }
        }
        public virtual Bitmap DecodeREFTTextureIndexed(VoidPtr addr, int w, int h, ColorPalette palette, int mipLevel, WiiPixelFormat fmt)
        {
            _workingPalette = palette;
            try { return Decode(addr, w, h, mipLevel, fmt); }
            finally { _workingPalette = null; }
        }
        
        protected abstract void DecodeBlock(VoidPtr blockAddr, ARGBPixel* destAddr, int width);

        public static TextureConverter Get(WiiPixelFormat format)
        {
            switch (format)
            {
                case WiiPixelFormat.I4: return I4;
                case WiiPixelFormat.IA4: return IA4;
                case WiiPixelFormat.I8: return I8;
                case WiiPixelFormat.IA8: return IA8;
                case WiiPixelFormat.RGB565: return RGB565;
                case WiiPixelFormat.RGB5A3: return RGB5A3;
                case WiiPixelFormat.CI4: return CI4;
                case WiiPixelFormat.CI8: return CI8;
                case WiiPixelFormat.CMPR: return CMPR;
                case WiiPixelFormat.RGBA8: return RGBA8;
            }
            return null;
        }

        public static Bitmap Decode(TEX0* texture, int mipLevel) { return Get(texture->PixelFormat).DecodeTexture(texture, mipLevel); }
        public static Bitmap DecodeIndexed(TEX0* texture, PLT0* palette, int mipLevel) { return Get(texture->PixelFormat).DecodeTextureIndexed(texture, palette, mipLevel); }
        public static Bitmap DecodeIndexed(TEX0* texture, ColorPalette palette, int mipLevel) { return Get(texture->PixelFormat).DecodeTextureIndexed(texture, palette, mipLevel); }
        public static Bitmap DecodeREFTIndexed(VoidPtr addr, int w, int h, ColorPalette palette, int mipLevel, WiiPixelFormat fmt) { return Get(fmt).DecodeREFTTextureIndexed(addr, w, h, palette, mipLevel, fmt); }
        public static FileMap EncodePalette(ColorPalette pal, WiiPaletteFormat format)
        {
            FileMap fileView = FileMap.FromTempFile((pal.Entries.Length * 2) + 0x40);
            try
            {
                EncodePalette(fileView.Address, pal, format);
                return fileView;
            }
            catch (Exception x)
            {
                fileView.Dispose();
                throw x;
                //MessageBox.Show(x.ToString());
                //fileView.Dispose();
                //return null;
            }
        }
        public static void EncodePalette(VoidPtr destAddr, ColorPalette pal, WiiPaletteFormat format)
        {
            int count = pal.Entries.Length;

            PLT0* header = (PLT0*)destAddr;
            *header = new PLT0(count, format);

            switch (format)
            {
                case WiiPaletteFormat.IA8:
                    {
                        IA8Pixel* dPtr = (IA8Pixel*)header->PaletteData;
                        for (int i = 0; i < count; i++)
                            dPtr[i] = (IA8Pixel)pal.Entries[i];
                        break;
                    }
                case WiiPaletteFormat.RGB565:
                    {
                        wRGB565Pixel* dPtr = (wRGB565Pixel*)header->PaletteData;
                        for (int i = 0; i < count; i++)
                            dPtr[i] = (wRGB565Pixel)pal.Entries[i];
                        break;
                    }
                case WiiPaletteFormat.RGB5A3:
                    {
                        wRGB5A3Pixel* dPtr = (wRGB5A3Pixel*)header->PaletteData;
                        for (int i = 0; i < count; i++)
                            dPtr[i] = (wRGB5A3Pixel)pal.Entries[i];
                        break;
                    }
            }
        }

        public static ColorPalette DecodePalette(PLT0* palette)
        {
            int count = palette->_numEntries;
            ColorPalette pal = ColorPaletteExtension.CreatePalette(ColorPaletteFlags.HasAlpha, count);
            switch (palette->PaletteFormat)
            {
                case WiiPaletteFormat.IA8:
                    {
                        IA8Pixel* sPtr = (IA8Pixel*)palette->PaletteData;
                        for (int i = 0; i < count; i++)
                            pal.Entries[i] = (Color)sPtr[i];
                        break;
                    }
                case WiiPaletteFormat.RGB565:
                    {
                        wRGB565Pixel* sPtr = (wRGB565Pixel*)palette->PaletteData;
                        for (int i = 0; i < count; i++)
                            pal.Entries[i] = (Color)sPtr[i];
                        break;
                    }
                case WiiPaletteFormat.RGB5A3:
                    {
                        wRGB5A3Pixel* sPtr = (wRGB5A3Pixel*)palette->PaletteData;
                        for (int i = 0; i < count; i++)
                            pal.Entries[i] = (Color)(ARGBPixel)sPtr[i];
                        break;
                    }
            }
            return pal;
        }

        public static ColorPalette DecodePalette(VoidPtr address, int count, WiiPaletteFormat format)
        {
            ColorPalette pal = ColorPaletteExtension.CreatePalette(ColorPaletteFlags.HasAlpha, count);
            switch (format)
            {
                case WiiPaletteFormat.IA8:
                    {
                        IA8Pixel* sPtr = (IA8Pixel*)address;
                        for (int i = 0; i < count; i++)
                            pal.Entries[i] = (Color)sPtr[i];
                        break;
                    }
                case WiiPaletteFormat.RGB565:
                    {
                        wRGB565Pixel* sPtr = (wRGB565Pixel*)address;
                        for (int i = 0; i < count; i++)
                            pal.Entries[i] = (Color)sPtr[i];
                        break;
                    }
                case WiiPaletteFormat.RGB5A3:
                    {
                        wRGB5A3Pixel* sPtr = (wRGB5A3Pixel*)address;
                        for (int i = 0; i < count; i++)
                            pal.Entries[i] = (Color)(ARGBPixel)sPtr[i];
                        break;
                    }
            }
            return pal;
        }

        private struct TextureContext
        {
            public TEX0* RawTexture;
            public PLT0* RawPalette;
            int Width, AlignedWidth;
            int Height, AlignedHeight;
            ColorPalette Palette;
        }

    }
}
