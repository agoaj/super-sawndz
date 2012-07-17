using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Imaging;
using BrawlLib.Wii.Models;
using System.Collections.Generic;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0ColorNode : MDL0EntryNode
    {
        internal MDL0ColorData* Header { get { return (MDL0ColorData*)WorkingUncompressed.Address; } }
        //protected override int DataLength { get { return Header->_dataLen; } }

        public MDL0PolygonNode[] Objects { get { return _polygons.ToArray(); } }
        internal List<MDL0PolygonNode> _polygons = new List<MDL0PolygonNode>();

        [Category("Color Data")]
        public int TotalLen { get { return Header->_dataLen; } }
        [Category("Color Data")]
        public int MDL0Offset { get { return Header->_mdl0Offset; } }
        [Category("Color Data")]
        public int DataOffset { get { return Header->_dataOffset; } }
        [Category("Color Data")]
        public int StringOffset { get { return Header->_stringOffset; } }
        [Category("Color Data")]
        public int ID { get { return Header->_index; } }
        [Category("Color Data")]
        public bool IsRGBA { get { return Header->_isRGBA != 0; } }
        [Category("Color Data")]
        public WiiColorComponentType Format { get { return (WiiColorComponentType)(int)Header->_format; } }
        [Category("Color Data")]
        public byte EntryStride { get { return Header->_entryStride; } }
        [Category("Color Data")]
        public byte Unknown { get { return Header->_scale; } }
        [Category("Color Data")]
        public int NumEntries { get { return Header->_numEntries; } }

        private List<RGBAPixel> _colors;
        public RGBAPixel[] ColorsAsArray
        {
            get { return _colors == null && Header != null ? (_colors = ColorCodec.ToRGBA(ColorCodec.ExtractColors(Header))).ToArray() : _colors.ToArray(); }
            set { _colors = value.ToList<RGBAPixel>(); SignalPropertyChange(); }
        }
        public List<RGBAPixel> ColorsAsList
        {
            get { return _colors; }
            set { _colors = value; SignalPropertyChange(); }
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            return false;
        }

        public ColorCodec _enc;
        protected override int OnCalculateSize(bool force)
        {
            if (Model._isImport || _changed)
            {
                _enc = new ColorCodec(ColorsAsArray);
                return _enc._dataLen.Align(0x20) + 0x20;
            }
            else return base.OnCalculateSize(force);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            if (Model._isImport || _changed)
            {
                //Write header
                MDL0ColorData* header = (MDL0ColorData*)address;
                header->_dataLen = length;
                header->_dataOffset = 0x20;
                header->_index = _entryIndex;
                header->_isRGBA = _enc._hasAlpha ? 1 : 0;
                header->_format = (int)_enc._outType;
                header->_entryStride = (byte)_enc._dstStride;
                header->_scale = 0;
                header->_numEntries = (ushort)ColorsAsArray.Length;

                //Write data
                _enc.Write((byte*)header + 0x20);
                _enc.Dispose();
                _enc = null;
            }
            else
                base.OnRebuild(address, length, force);
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0ColorData* header = (MDL0ColorData*)dataAddress;
            header->_mdl0Offset = (int)mdlAddress - (int)dataAddress;
            header->_stringOffset = (int)stringTable[Name] + 4 - (int)dataAddress;
            header->_index = Index;
        }
    }
}
