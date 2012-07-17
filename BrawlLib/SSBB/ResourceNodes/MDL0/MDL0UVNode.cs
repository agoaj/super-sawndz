using System;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Models;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0UVNode : MDL0EntryNode
    {
        internal MDL0UVData* Header { get { return (MDL0UVData*)WorkingUncompressed.Address; } }
        //protected override int DataLength { get { return Header->_dataLen; } }

        public MDL0PolygonNode[] Objects { get { return _polygons.ToArray(); } }
        internal List<MDL0PolygonNode> _polygons = new List<MDL0PolygonNode>();

        [Category("UV Data")]
        public int TotalLen { get { return Header->_dataLen; } }
        [Category("UV Data")]
        public int MDL0Offset { get { return Header->_mdl0Offset; } }
        [Category("UV Data")]
        public int DataOffset { get { return Header->_dataOffset; } }
        [Category("UV Data")]
        public int StringOffset { get { return Header->_stringOffset; } }
        [Category("UV Data")]
        public int ID { get { return Header->_index; } }
        [Category("UV Data")]
        public bool IsST { get { return Header->_isST != 0; } }
        [Category("UV Data")]
        public WiiVertexComponentType Format { get { return (WiiVertexComponentType)(int)Header->_format; } }
        [Category("UV Data")]
        public byte Divisor { get { return Header->_divisor; } }
        [Category("UV Data")]
        public byte EntryStride { get { return Header->_entryStride; } }
        [Category("UV Data")]
        public int NumEntries { get { return Header->_numEntries; } }

        [Category("UV Data")]
        public Vector2 Min { get { return Header->_min; } }
        [Category("UV Data")]
        public Vector2 Max { get { return Header->_max; } }

        [Category("UV Data")]
        public int Pad1 { get { return Header->_pad1; } }
        [Category("UV Data")]
        public int Pad2 { get { return Header->_pad2; } }
        [Category("UV Data")]
        public int Pad3 { get { return Header->_pad3; } }
        [Category("UV Data")]
        public int Pad4 { get { return Header->_pad4; } }

        private Vector2[] _points;
        public Vector2[] Points
        {
            get { return _points == null ? _points = VertexCodec.ExtractUVs(Header) : _points; }
            set { _points = value; SignalPropertyChange(); }
        }

        protected override bool OnInitialize()
        {
            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            return false;
        }

        public VertexCodec _enc;
        public bool _forceRebuild = false;
        public bool _forceFloat = false;
        protected override int OnCalculateSize(bool force)
        {
            if (Model._isImport || _forceRebuild)
            {
                _enc = new VertexCodec(Points, _forceRebuild);
                return _enc._dataLen.Align(0x20) + 0x40;
            }
            else return base.OnCalculateSize(force);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            if (Model._isImport || _forceRebuild)
            {
                MDL0UVData* header = (MDL0UVData*)address;

                header->_dataLen = length;
                header->_dataOffset = 0x40;
                header->_index = _entryIndex;
                header->_format = (int)_enc._type;
                header->_divisor = (byte)_enc._scale;
                header->_isST = 1;
                header->_entryStride = (byte)_enc._dstStride;
                header->_numEntries = (ushort)_enc._srcCount;
                header->_min = (Vector2)_enc._min;
                header->_min = (Vector2)_enc._max;
                header->_pad1 = header->_pad2 = header->_pad3 = header->_pad4 = 0;

                _enc.Write(Points, (byte*)address + 0x40);
                _enc.Dispose();
                _enc = null;

                _forceRebuild = false;
            }
            else
                base.OnRebuild(address, length, force);
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0UVData* header = (MDL0UVData*)dataAddress;
            header->_mdl0Offset = (int)mdlAddress - (int)dataAddress;
            header->_stringOffset = (int)stringTable[Name] + 4 - (int)dataAddress;
            header->_index = Index;
        }
    }
}
