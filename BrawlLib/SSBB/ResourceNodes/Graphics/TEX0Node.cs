using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Textures;
using BrawlLib.Imaging;
using System.Drawing;
using System.Collections.Generic;
using BrawlLib.IO;
using System.Drawing.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TEX0Node : BRESEntryNode, IImageSource
    {
        public override ResourceType ResourceType { get { return ResourceType.TEX0; } }
        internal TEX0* Header { get { return (TEX0*)WorkingUncompressed.Address; } }

        public override int DataAlign { get { return 0x20; } }

        int _width, _height;
        WiiPixelFormat _format;
        int _lod;
        bool _hasPalette;

        [Category("Texture")]
        public int Width { get { return _width; } set { _width = value; } }
        [Category("Texture")]
        public int Height { get { return _height; } set { _height = value; } }
        [Category("Texture")]
        public WiiPixelFormat Format { get { return _format; } set { _format = value; } }
        [Category("Texture")]
        public int LevelOfDetail { get { return _lod; } set { _lod = value; } }
        [Category("Texture")]
        public bool HasPalette { get { return _hasPalette; } set { _hasPalette = value; } }

        public PLT0Node GetPaletteNode() { return ((_parent == null) || (!HasPalette)) ? null : _parent._parent.FindChild("Palettes(NW4R)/" + this.Name, false) as PLT0Node; }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            _width = Header->_width;
            _height = Header->_height;
            _format = Header->PixelFormat;
            _lod = Header->_levelOfDetail;
            _hasPalette = Header->HasPalette;

            return false;
        }

        [Browsable(false)]
        public int ImageCount { get { return _lod; } }
        public Bitmap GetImage(int index)
        {
            PLT0Node plt = GetPaletteNode();
            try
            {
                if (plt != null)
                    return TextureConverter.DecodeIndexed(Header, plt.Palette, index + 1);
                else
                    return TextureConverter.Decode(Header, index + 1);
            }
            catch { return null; }
        }

        public Bitmap GetImage(int index, PLT0Node plt)
        {
            try
            {
                if (plt != null)
                    return TextureConverter.DecodeIndexed(Header, plt.Palette, index + 1);
                else
                    return TextureConverter.Decode(Header, index + 1);
            }
            catch { return null; }
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            TEX0* header = (TEX0*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        public void Replace(Bitmap bmp)
        {
            FileMap tMap, pMap;
            if (HasPalette)
            {
                PLT0Node pn = this.GetPaletteNode();
                tMap = TextureConverter.Get(Format).EncodeTextureIndexed(bmp, LevelOfDetail, pn.Colors, pn.Format, QuantizationAlgorithm.MedianCut, out pMap);
                pn.ReplaceRaw(pMap);
            }
            else
                tMap = TextureConverter.Get(Format).EncodeTexture(bmp, LevelOfDetail);
            ReplaceRaw(tMap);
        }

        public override unsafe void Replace(string fileName)
        {
            Bitmap bmp;
            if (fileName.EndsWith(".tga"))
                bmp = TGA.FromFile(fileName);
            else if (fileName.EndsWith(".png") ||
                fileName.EndsWith(".tiff") || fileName.EndsWith(".tif") ||
                fileName.EndsWith(".bmp") ||
                fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg") ||
                fileName.EndsWith(".gif"))
                bmp = (Bitmap)Bitmap.FromFile(fileName);
            else
            {
                base.Replace(fileName);
                return;
            }

            using (Bitmap b = bmp)
                Replace(b);
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".png"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Png);
            else if (outPath.EndsWith(".tga"))
                using (Bitmap bmp = GetImage(0)) bmp.SaveTGA(outPath);
            else if (outPath.EndsWith(".tiff") || outPath.EndsWith(".tif"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Tiff);
            else if (outPath.EndsWith(".bmp"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Bmp);
            else if (outPath.EndsWith(".jpg") || outPath.EndsWith(".jpeg"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Jpeg);
            else if (outPath.EndsWith(".gif"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Gif);
            else
                base.Export(outPath);
        }

        internal static ResourceNode TryParse(DataSource source) { return ((TEX0*)source.Address)->_header._tag == TEX0.Tag ? new TEX0Node() : null; }
    }
}
