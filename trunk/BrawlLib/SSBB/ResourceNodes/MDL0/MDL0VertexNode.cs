using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Modeling;
using BrawlLib.Wii.Models;
using System.Collections.Generic;
using BrawlLib.OpenGL;
using System.Drawing;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0VertexNode : MDL0EntryNode
    {
        internal MDL0VertexData* Header { get { return (MDL0VertexData*)WorkingUncompressed.Address; } }
        //protected override int DataLength { get { return Header->_dataLen; } }

        public MDL0PolygonNode[] Objects { get { return _polygons.ToArray(); } }
        internal List<MDL0PolygonNode> _polygons = new List<MDL0PolygonNode>();

        [Category("Vertex Data")]
        public int TotalLen { get { return Header->_dataLen; } }
        [Category("Vertex Data")]
        public int MDL0Offset { get { return Header->_mdl0Offset; } }
        [Category("Vertex Data")]
        public int DataOffset { get { return Header->_dataOffset; } }
        [Category("Vertex Data")]
        public int StringOffset { get { return Header->_stringOffset; } }
        [Category("Vertex Data")]
        public int ID { get { return Header->_index; } }
        [Category("Vertex Data")]
        public bool IsXYZ { get { return Header->_isXYZ != 0; } }
        [Category("Vertex Data")]
        public WiiVertexComponentType Format { get { return (WiiVertexComponentType)(int)Header->_type; } }
        [Category("Vertex Data")]
        public byte Divisor { get { return Header->_divisor; } }
        [Category("Vertex Data")]
        public byte EntryStride { get { return Header->_entryStride; } }
        [Category("Vertex Data")]
        public short NumVertices { get { return Header->_numVertices; } }
        [Category("Vertex Data")]
        public Vector3 EMin { get { return Header->_eMin; } }
        [Category("Vertex Data")]
        public Vector3 EMax { get { return Header->_eMax; } }
        [Category("Vertex Data")]
        public int Pad1 { get { return Header->_pad1; } }
        [Category("Vertex Data")]
        public int Pad2 { get { return Header->_pad2; } }

        public Vector3[] _vertices;
        public Vector3[] Vertices
        {
            get { return _vertices == null ? _vertices = VertexCodec.ExtractVertices(Header) : _vertices; }
            set { _vertices = value; SignalPropertyChange(); }
        }

        protected override bool OnInitialize()
        {
            MDL0VertexData* header = Header;
            base.OnInitialize();

            SetSizeInternal(header->_dataLen);

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            return false;
        }

        public VertexCodec _enc;
        public bool _forceRebuild = false;
        protected override int OnCalculateSize(bool force)
        {
            if (Model._isImport || _forceRebuild)
            {
                _enc = new VertexCodec(Vertices, false, _forceRebuild);
                return _enc._dataLen.Align(0x20) + 0x40;
            }
            else return base.OnCalculateSize(force);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            if (Model._isImport || _forceRebuild)
            {
                MDL0VertexData* header = (MDL0VertexData*)address;

                header->_dataLen = length;
                header->_dataOffset = 0x40;
                header->_index = _entryIndex;
                header->_isXYZ = _enc._hasZ ? 1 : 0;
                header->_type = (int)_enc._type;
                header->_divisor = (byte)_enc._scale;
                header->_entryStride = (byte)_enc._dstStride;
                header->_numVertices = (short)_enc._srcCount;
                header->_eMin = _enc._min;
                header->_eMax = _enc._max;
                header->_pad1 = header->_pad2 = 0;

                //Write data
                _enc.Write(Vertices, (byte*)address + 0x40);
                _enc.Dispose();
                _enc = null;

                _forceRebuild = false;
            }
            else
                base.OnRebuild(address, length, force);
        }

        public override unsafe void Export(string outPath)
        {
            if(outPath.EndsWith(".obj"))
                Wavefront.Serialize(outPath, this);
            else base.Export(outPath);
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0VertexData* header = (MDL0VertexData*)dataAddress;
            header->_mdl0Offset = (int)mdlAddress - (int)dataAddress;
            header->_stringOffset = (int)stringTable[Name] + 4 - (int)dataAddress;
            header->_index = Index;
        }

        //public VertexGroup GetVertices(string Name, Vector3[] Vertices) { return new VertexGroup() { _name = Name, Vertices = VertexCodec.ExtractVertices(Header) }; }
    }
}
