using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.Imaging;

namespace BrawlLib.OpenGL
{
    public class GLPrimitive
    {
        internal ushort[] _nodeIndices;
        internal ushort[] _vertexIndices;
        internal ushort[] _normalIndices;
        internal ushort[][] _colorIndices = new ushort[2][];
        internal ushort[][] _uvIndices = new ushort[8][];

        internal Vector3[] _vertices, _normals;

        internal GLPolygon _parent;
        internal GLPrimitiveType _type;
        internal int _elements;

        public bool _enabled = true;

        internal unsafe void Rebuild()
        {
            if (_vertices == null)
                _vertices = new Vector3[_elements];
            if (_normals == null)
                _normals = new Vector3[_elements];

            Vector3* sPtr = (Vector3*)_parent._vertices.Address;
            Vector3* nPtr = _parent._normals != null ? (Vector3*)_parent._normals.Address : null;


            Matrix43 m = _parent._node != null ? _parent._node._matrix : Matrix43.Identity;
            for (int i = 0; i < _elements; i++)
            {
                if (_nodeIndices != null)
                    m = _parent._model._nodes[_nodeIndices[i]]._matrix;

                _vertices[i] = m.Multiply(sPtr[_vertexIndices[i]]);

                if (nPtr != null)
                    _normals[i] = m.Multiply(nPtr[_normalIndices[i]]);
            }
        }

        internal unsafe void Render(GLContext context, uint[] texIds)
        {
            if (!_enabled)
                return;

            //context.glEnable(GLEnableCap.Texture2D);

            if (_parent._uvData[0] == null)
                return;

            //Vector3* vPtr = (Vector3*)_parent._vertices.Address;
            //Vector3* nPtr = _parent._normals != null ? (Vector3*)_parent._normals.Address : null;
            ARGBPixel* c1Ptr = _parent._colors1 != null ? (ARGBPixel*)_parent._colors1.Address : null;
            ARGBPixel* c2Ptr = _parent._colors2 != null ? (ARGBPixel*)_parent._colors2.Address : null;
            //Vector2* uvPtr = _parent._uvData[0] != null ? (Vector2*)_parent._uvData[0].Address : null;

            int numUV = 0;
            Vector2*[] uPtrs = new Vector2*[8];
            while (_parent._uvData[numUV] != null)
            {
                uPtrs[numUV] = (Vector2*)_parent._uvData[numUV].Address;
                numUV++;
            }

            Vector3 v, n;
            Vector2 u;
            uint id;

            fixed (Vector3* vPtr = _vertices)
            fixed (Vector3* nPtr = _normals)
            {

                for (int t = 0; t < 1; t++)
                {
                    if ((id = texIds[t]) == 0)
                        continue;

                    context.glBindTexture(GLTextureTarget.Texture2D, id);
                    context.glBegin(_type);
                    for (int i = 0; i < _elements; i++)
                    {
                        if (c1Ptr != null)
                            context.glColor4((byte*)&c1Ptr[_colorIndices[0][i]]);
                        //if(c2Ptr != null)
                        //    context.glColor4((byte*)&c2Ptr[_colorIndices[1][i]]);

                        context.glNormal((float*)&nPtr[i]);

                        context.glTexCoord2((float*)&uPtrs[0][_uvIndices[0][i]]);
                        //u = uPtrs[t][_uvIndices[t][i]];
                        //context.glTexCoord2((float*)&u);

                        context.glVertex3v((float*)&vPtr[i]);
                    }
                    context.glEnd();
                }
            }
            context.CheckErrors();
        }
    }
}
