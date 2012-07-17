using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.Wii.Models;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using BrawlLib.Modeling;

namespace BrawlLib.OpenGL
{
    public class GLPolygon
    {
        public List<GLPrimitive> _primitives = new List<GLPrimitive>();
        //public int _nodeIndex;
        public GLNode _node;

        internal ushort[] _weights;
        internal VertexCodec _vertices;
        internal VertexCodec _normals;
        internal ColorCodec _colors1, _colors2;
        internal VertexCodec[] _uvData = new VertexCodec[8];

        public int _index, _nodeIndex;
        public bool _enabled = true;
        public GLModel _model;

        public List<GLMaterial> _materials = new List<GLMaterial>();

        //public GLPolygon(MDL0PolygonNode node)
        //{
        //}

        //public unsafe GLPolygon(GLModel model, MDL0PolygonNode polygon)
        //{
        //    _model = model;

        //    GLPrimitive prim;
        //    _index = polygon.ItemId;
        //    _nodeIndex = polygon.NodeId;

        //    Primitive p = new Primitive();
        //    MDL0Polygon* header = polygon.Header;
        //    VoidPtr address = header->PrimitiveData;
        //    EntrySize e = new EntrySize(header->_flags);

        //    _vertices = new VertexCodec(VertexCodec.ExtractVertices(header->VertexData), false);
        //    if (header->_normalId != -1)
        //        _normals = new VertexCodec(VertexCodec.ExtractNormals(header->NormalData), false);
        //    if (header->_colorId1 != -1)
        //        _colors1 = new ColorCodec(ColorCodec.ToRGBA(ColorCodec.ExtractColors(header->ColorData1)));
        //    if (header->_colorId2 != -1)
        //        _colors2 = new ColorCodec(ColorCodec.ToRGBA(ColorCodec.ExtractColors(header->ColorData2)));

        //    MDL0UVData* uvPtr;
        //    for (int i = 0; (i < 8) && ((uvPtr = header->GetUVData(i)) != null); i++)
        //        _uvData[i] = new VertexCodec(VertexCodec.ExtractUVs(uvPtr));

        //    ushort[] nodeBuffer = new ushort[16];
        //    while ((prim = ModelConverter.ExtractPrimitive(ref address, e, this, nodeBuffer, ref _nodeIndex)) != null)
        //        _primitives.Add(prim);
        //}

        private uint[] _textureIds = new uint[8];
        internal unsafe void Render(GLContext context)
        {
            if (!_enabled)
                return;

            if (_materials.Count != 0)
            {
                _materials[0].Bind(context, _textureIds);
            }
            else
            {
                return;
            }

            context.glEnable(GLEnableCap.Texture2D);

            foreach (GLPrimitive prim in _primitives)
                prim.Render(context, _textureIds);

            context.glDisable((uint)GLEnableCap.Texture2D);
        }

        internal void Rebuild()
        {
            foreach (GLPrimitive prim in _primitives)
                prim.Rebuild();
        }
    }
}
