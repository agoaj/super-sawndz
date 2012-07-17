using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Textures;
using System.Collections.Generic;
using System.Drawing.Imaging;
using BrawlLib.Imaging;
using System.Drawing;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class PLT0Node : BRESEntryNode, IColorSource
    {
        public override ResourceType ResourceType { get { return ResourceType.PLT0; } }
        internal PLT0* Header { get { return (PLT0*)WorkingUncompressed.Address; } }

        public override int DataAlign { get { return 0x20; } }

        //private int _numColors;
        private WiiPaletteFormat _format;

        private ColorPalette _palette;
        [Browsable(false)]
        public ColorPalette Palette
        {
            get { return _palette == null ? _palette = TextureConverter.DecodePalette(Header) : _palette; }
            set { _palette = value; SignalPropertyChange(); }
        }

        [Category("Palette")]
        public int Colors { get { return Palette.Entries.Length; } }// set { _numColors = value; } }
        [Category("Palette")]
        public WiiPaletteFormat Format { get { return _format; } set { _format = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _palette = null;

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            //_numColors = Header->_numEntries;
            _format = Header->PaletteFormat;

            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            int count = Palette.Entries.Length.Align(16);
            return 0x40 + (count * 2);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TextureConverter.EncodePalette(address, Palette, _format);
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            PLT0* header = (PLT0*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        #region IColorSource Members

        [Browsable(false)]
        public bool HasPrimary { get { return false; } }
        [Browsable(false)]
        public ARGBPixel PrimaryColor { get { return new ARGBPixel(); } set { } }
        [Browsable(false)]
        public string PrimaryColorName { get { return null; } }
        [Browsable(false)]
        public int ColorCount { get { return Palette.Entries.Length; } }
        public ARGBPixel GetColor(int index) { return (ARGBPixel)Palette.Entries[index]; }
        public void SetColor(int index, ARGBPixel color) { Palette.Entries[index] = (Color)color; SignalPropertyChange(); }

        #endregion

        internal static ResourceNode TryParse(DataSource source) { return ((PLT0*)source.Address)->_bresEntry._tag == PLT0.Tag ? new PLT0Node() : null; }
    }
}
