using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Imaging;
using System.ComponentModel;

namespace BrawlLib.Modeling
{
    public class Primitive : IDisposable
    {
        internal GLPrimitiveType _type;

        internal List<Vertex3> _vertices = new List<Vertex3>();

        internal int _elementCount;

        internal ushort[] _weightIndices;
        internal ushort[] _vertexIndices;
        internal ushort[] _normalIndices;
        internal ushort[][] _colorIndices = new ushort[2][];
        internal ushort[][] _uvIndices = new ushort[8][];

        internal UnsafeBuffer _precVertices;
        internal UnsafeBuffer _precNormals;
        internal UnsafeBuffer _precColors;
        internal UnsafeBuffer[] _precUVs = new UnsafeBuffer[8];

        ~Primitive() { Dispose(); }

        internal unsafe void PreparePointers(GLContext ctx)
        {
            if (_precVertices == null)
                return;

            ctx.glVertexPointer(3, GLDataType.Float, 0, _precVertices.Address);

            if (_precNormals != null)
                ctx.glNormalPointer(GLDataType.Float, 0, _precNormals.Address);

            if (_precColors != null)
                ctx.glColorPointer(4, GLDataType.Byte, 0, _precColors.Address);
        }

        internal unsafe void Render(GLContext ctx, int uvIndex)
        {
            if (uvIndex >= 0)
            {
                if (_precUVs[uvIndex] == null)
                    return;

                ctx.glTexCoordPointer(2, GLDataType.Float, 0, _precUVs[uvIndex].Address);
            }
            else
            {

            }

            ctx.glDrawArrays(_type, 0, _elementCount);
        }

        internal unsafe void Precalc(MDL0PolygonNode parent, IMatrixNode[] nodes)
        {
            //If already calculated, and no weights, skip?
            bool hasNodes = parent.Model._linker.NodeCache.Length > 0;
            if ((_precVertices != null) && hasNodes)
                return;

            Vector3[] verts, norms = null;
            Vector3* vPtr = null, nPtr = null;
            RGBAPixel[][] colors = new RGBAPixel[2][];
            Vector2[][] uvs = new Vector2[8][];
            uint* ptrCache = stackalloc uint[10];

            //Points
            verts = parent._vertexNode.Vertices;
            if (_precVertices == null)
                _precVertices = new UnsafeBuffer(_elementCount * 12);
            vPtr = (Vector3*)_precVertices.Address;

            //Normals
            if (parent._normalNode != null)
            {
                norms = parent._normalNode.Normals;
                if(_precNormals == null)
                    _precNormals = new UnsafeBuffer(_elementCount * 12);
                nPtr = (Vector3*)_precNormals.Address;
            }
            else if (_precNormals != null)
            {
                _precNormals.Dispose();
                _precNormals = null;
            }

            ////Colors
            //for (int i = 0; i < 1; i++)
            //{
            //    if (parent._colorSet[i] != null)
            //    {
            //        colors[i] = parent._colorSet[i].Colors;
            //        if (_precColors == null)
            //            _precColors = new UnsafeBuffer(_elementCount * 4);
            //        ptrCache[i] = (uint)_precColors.Address;
            //    }
            //    else if (_precColors != null)
            //    {
            //        _precColors.Dispose();
            //        _precColors = null;
            //    }
            //}

            //UVs
            for (int i = 0; i < 8; i++)
            {
                if (parent._uvSet[i] != null)
                {
                    uvs[i] = parent._uvSet[i].Points;
                    if (_precUVs[i] == null)
                        _precUVs[i] = new UnsafeBuffer(_elementCount * 8);
                    ptrCache[i + 2] = (uint)_precUVs[i].Address;
                }
                else if (_precUVs[i] != null)
                {
                    _precUVs[i].Dispose();
                    _precUVs[i] = null;
                }
            }

            int count = _vertices.Count;
            for(int x = 0 ; x < count ; x++)
            {
                Vertex3 vert = _vertices[x];

                ////Vertices
                //if (hasNodes)
                //    *vPtr++ = nodes[vert.Influence.NodeIndex].Matrix.Multiply(verts[vert.Position]);
                //else
                //    *vPtr++ = verts[vert.Position];

                ////Normals
                //if (nPtr != null)
                //{
                //    if (hasNodes)
                //        *nPtr++ = nodes[vert.Influence.NodeIndex].Matrix.Multiply(norms[vert.Normal]);
                //    else
                //        *nPtr++ = norms[vert.Normal.Length];
                //}

                ////Colors
                //for (int i = 0; i < 1; i++)
                //{
                //    RGBAPixel* cPtr = (RGBAPixel*)ptrCache[i];
                //    if (cPtr != null)
                //        cPtr[x] = colors[i][vert.Color[i]];
                //}

                ////UVs
                //for (int i = 0; i < 8; i++)
                //{
                //    Vector2* uPtr = (Vector2*)ptrCache[i + 2];
                //    if (uPtr != null)
                //        uPtr[x] = uvs[i][vert.UV[i]];
                //}
            }
        }

        public void Dispose()
        {
            if (_precVertices != null) { _precVertices.Dispose(); _precVertices = null; }
            if (_precNormals != null) { _precNormals.Dispose(); _precNormals = null; }
            if (_precColors != null) { _precColors.Dispose(); _precColors = null; }
            for (int i = 0; i < 8; i++)
                if (_precUVs[i] != null) { _precUVs[i].Dispose(); _precUVs[i] = null; }
        }
    }
}
