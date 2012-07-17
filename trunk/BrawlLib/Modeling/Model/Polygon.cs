using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BrawlLib.Imaging;
using BrawlLib.OpenGL;
using BrawlLib.Wii.Models;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Graphics;
using System.ComponentModel;

namespace BrawlLib.Modeling
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FacePoint
    {
        public ushort VertexId;
        public Vector3 Normal; //Must be multiplied by vertex matrix
        public fixed uint Color[2];
        public fixed float UVs[16]; //Multiplied with pre-set texture matrix (set by material)
    }

    public unsafe class Primitive2 : IDisposable
    {
        internal GLPrimitiveType _type;
        internal int _elements;
        internal int[] _indices;
        internal UnsafeBuffer _data;

        ~Primitive2() { Dispose(); }
        public void Dispose()
        {
            if (_data != null)
            {
                _data.Dispose();
                _data = null;
            }
        }

        public void Precalc(Polygon poly)
        {
            //Manage data buffer size and adjust accordingly
            int dataSize = _elements * poly._script.Stride;

            if (_data != null)
                if (dataSize != _data.Length)
                    _data.Dispose();
                else
                    goto Next;

            _data = new UnsafeBuffer(dataSize);

        Next:
            //Fill buffer with raw data
            byte* pOut = (byte*)_data.Address;
            FacePoint* pFacePoint = (FacePoint*)poly._facePoints.Address;

            for (int i = 0; i < _elements; i++)
                poly._script.Run(poly, &pFacePoint[_indices[i]], ref pOut);
        }

        byte* pRenderAddr;
        int iRenderStride;
        public void PreparePointers(ElementDefinition def, GLContext ctx)
        {
            iRenderStride = def.Stride;
            pRenderAddr = (byte*)_data.Address;

            ctx.glVertexPointer(3, GLDataType.Float, iRenderStride, pRenderAddr);
            pRenderAddr += 12;

            if (def.Normals)
            {
                ctx.glNormalPointer(GLDataType.Float, iRenderStride, pRenderAddr);
                pRenderAddr += 12;
            }
            if (def.Colors[0])
            {
                ctx.glColorPointer(4, GLDataType.Byte, iRenderStride, pRenderAddr);
                pRenderAddr += 4;
            }
            if (def.Colors[1])
            {
                //ctx.glColorPointer(4, GLDataType.Byte, iRenderStride, pRenderAddr);
                pRenderAddr += 4;
            }
            //if (def.UVs[0])
            //{
            //    //ctx.glColorPointer(4, GLDataType.Byte, iRenderStride, pRenderAddr);
            //    //pRenderAddr += 8;
            //}
        }

        public void Render(GLContext ctx, int index)
        {
            if (index >= 0)
                ctx.glTexCoordPointer(2, GLDataType.Float, iRenderStride, pRenderAddr);

            ctx.glDrawArrays(_type, 0, _elements);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PrimitiveScript
    {
        public int Stride;
        private byte _count;
        private fixed byte _commands[31];
        public fixed ushort Nodes[16];

        public PrimitiveScript(ElementDefinition def)
        {
            def.CalcStride();
            Stride = def.Stride;
            _count = 0;
            fixed (byte* c = _commands)
            {
                c[_count++] = (byte)ScriptCommand.WeightedPosition;

                if (def.Normals)
                    if (def.Weighted)
                        c[_count++] = (byte)ScriptCommand.WeightedNormal;
                    else
                        c[_count++] = (byte)ScriptCommand.Normal;

                for (int i = 0; i < 2; i++)
                    if (def.Colors[i])
                        c[_count++] = (byte)((int)ScriptCommand.Color0 + i);

                for (int i = 0; i < 8; i++)
                    if (def.UVs[i])
                        c[_count++] = (byte)((int)ScriptCommand.UV0 + i);

                c[_count++] = 0;
            }
        }

        public void Run(Polygon poly, FacePoint* point, ref byte* pOut)
        {
            Vertex3 v = poly._vertices[point->VertexId];
            ScriptCommand o;
            int index;
            fixed (byte* c = _commands)
            {
                ScriptCommand* cmd = (ScriptCommand*)c;
            Top:
                switch (o = *cmd++)
                {
                    case ScriptCommand.None: break;

                    case ScriptCommand.Position:
                        *(Vector3*)pOut = v.Position;
                        pOut += 12;
                        goto Top;

                    case ScriptCommand.WeightedPosition:
                        *(Vector3*)pOut = v.WeightedPosition;
                        pOut += 12;
                        goto Top;

                    case ScriptCommand.Normal:
                        *(Vector3*)pOut = point->Normal;
                        pOut += 12;
                        goto Top;

                    case ScriptCommand.WeightedNormal:
                        *(Vector3*)pOut = v.Inf != null ? v.Inf.Matrix * point->Normal : point->Normal;
                        pOut += 12;
                        goto Top;

                    case ScriptCommand.Color0:
                    case ScriptCommand.Color1:
                        index = (int)(o - ScriptCommand.Color0);
                        *(RGBAPixel*)pOut = ((RGBAPixel*)point->Color)[index];
                        pOut += 4;
                        goto Top;

                    //The rest are UVs
                    default:
                        index = (int)(o - ScriptCommand.UV0);
                        *(Vector2*)pOut = ((Vector2*)point->UVs)[index];
                        pOut += 8;
                        goto Top;
                }
            }
        }
    }

    public enum ScriptCommand : byte
    {
        None,
        Position,
        WeightedPosition,
        Normal,
        WeightedNormal,
        WeightPos,
        Color0,
        Color1,
        UV0,
        UV1,
        UV2,
        UV3,
        UV4,
        UV5,
        UV6,
        UV7
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe class ElementDefinition
    {
        public int Stride;
        public bool Weighted;

        public XFDataFormat PositionFmt;

        public bool Normals;
        public XFDataFormat NormalFmt;

        public bool[] Colors = new bool[2];
        public XFDataFormat[] ColorFmt = new XFDataFormat[2];

        public bool[] UVs = new bool[8];
        public XFDataFormat[] UVFmt = new XFDataFormat[8];
        
        public bool[] TexMatrices = new bool[8];

        public void CalcStride()
        {
            Stride = 12;
            if (Normals)
                Stride += 12;

            for (int i = 0; i < 2; i++)
                if (Colors[i])
                    Stride += 4;

            for (int i = 0; i < 8; i++)
                if (UVs[i])
                    Stride += 8;
        }
    }

    public unsafe class Polygon : MDL0EntryNode
    {
        internal bool _render = true;

        internal IMatrixNode _singleBind;
        internal MDL0MaterialNode _material;

        internal PrimitiveScript _script;
        internal ElementDefinition _elemDef;

        internal List<Vertex3> _vertices = new List<Vertex3>();

        internal int _facePointCount;
        internal UnsafeBuffer _facePoints;

        internal List<Primitive2> _primitives = new List<Primitive2>();

        [Category("Vertices")]
        public List<Vertex3> Vertices { get { return _vertices; } }

        #region Primitive Management
        public void GetPrimitives(MDL0Polygon* header, MDL0PolygonNode polygon)
        {
            AssetStorage assets = Model._assets;
            IMatrixNode[] influences = Model._linker.NodeCache;
            byte*[] pAssetList = new byte*[12]; int id;

            //Sync Data
            if ((id = header->_vertexId) >= 0)
                pAssetList[0] = (byte*)assets.Assets[0][id].Address;

            if ((id = header->_normalId) >= 0)
                pAssetList[1] = (byte*)assets.Assets[1][id].Address;
            
            for (int i = 0, x = 2; i < 2; i++, x++)
                if ((id = ((bshort*)header->_colorIds)[i]) >= 0)
                    pAssetList[x] = (byte*)assets.Assets[2][id].Address;

            for (int i = 0, x = 4; i < 8; i++, x++)
                if ((id = ((bshort*)header->_uids)[i]) >= 0)
                    pAssetList[x] = (byte*)assets.Assets[3][id].Address;

            //Set definition
            _elemDef = new ElementDefinition();
            _elemDef.Weighted = polygon.VertexFormat.HasPosMatrix;
            _elemDef.PositionFmt = polygon.VertexFormat.PosFormat;
            _elemDef.Normals = (_elemDef.NormalFmt = polygon.VertexFormat.NormalFormat) != XFDataFormat.None;
            for (int i = 0; i < 2; i++)
                _elemDef.Colors[i] = (_elemDef.ColorFmt[i] = polygon.VertexFormat.GetColorFormat(i)) != XFDataFormat.None;
            for (int i = 0; i < 8; i++)
                _elemDef.UVs[i] = (_elemDef.UVFmt[i] = polygon.VertexFormat.GetUVFormat(i)) != XFDataFormat.None;
            for (int i = 0; i < 8; i++)
                _elemDef.TexMatrices[i] = polygon.VertexFormat.GetHasTexMatrix(i);

            List<FacePoint> facePoints = new List<FacePoint>();

            //Create script using definition for decoding and rendering
            _script = new PrimitiveScript(_elemDef);

            //Create remap table for vertex weights
            RemapTable = new UnsafeBuffer(header->_numVertices * 4);
            RemapSize = 0;

            //Extract primitives using script
            byte* pData = (byte*)header->PrimitiveData;
            int count; GLPrimitiveType type;
            int ind = 0;
            _facePoints = new UnsafeBuffer((_facePointCount = header->_numVertices) * 86);
            FacePoint* points = (FacePoint*)_facePoints.Address;

        Top:  
            switch ((WiiPrimitiveType)(*pData++))
            {
                //Fill weight cache
                case WiiPrimitiveType.PosMtx:
                    //Get node ID
                    ushort node = *(bushort*)pData;
                    //Get cache index
                    int index = (*(bushort*)(pData + 2) & 0xFFF) / 12;
                    //Assign node ID to cache, using index
                    fixed (ushort* n = _script.Nodes)
                        n[index] = node;
                    
                    pData += 4;
                    goto Top;

                case WiiPrimitiveType.NorMtx: //Same as PosMtx
                case WiiPrimitiveType.TexMtx:
                case WiiPrimitiveType.LightMtx:
                    pData += 4; //Skip
                    goto Top;

                case WiiPrimitiveType.Quads: type = GLPrimitiveType.Quads; break;
                case WiiPrimitiveType.Triangles: type = GLPrimitiveType.Triangles; break;
                case WiiPrimitiveType.TriangleFan: type = GLPrimitiveType.TriangleFan; break;
                case WiiPrimitiveType.TriangleStrip: type = GLPrimitiveType.TriangleStrip; break;
                case WiiPrimitiveType.Lines: type = GLPrimitiveType.Lines; break;
                case WiiPrimitiveType.LineStrip: type = GLPrimitiveType.LineStrip; break;
                case WiiPrimitiveType.Points: type = GLPrimitiveType.Points; break;
                default: goto Next; //No more primitives.
            }
            
            count = *(bushort*)pData; pData += 2;
            Primitive2 prim = new Primitive2() { _type = type, _elements = count };
            prim._indices = new int[count];

            //Extract facepoints
            fixed (byte** pAssets = pAssetList)
                for (int i = 0; i < count; i++)
                {
                    points[ind] = ExtractFacepoint(ref pData, pAssets);
                    prim._indices[i] = (ushort)ind++;
                }
            _primitives.Add(prim);

            goto Top; //Move on to next primitive

        Next: _vertices = Finish((Vector3*)pAssetList[0], influences);
        }

        public UnsafeBuffer RemapTable;
        public int RemapSize;

        private FacePoint ExtractFacepoint(ref byte* addr, byte** pAssets)
        {
            int weight = 0;
            int index = 0;
            FacePoint fp = new FacePoint();

            fixed (ushort* pNode = _script.Nodes)
            {
                if (_elemDef.Weighted)
                {
                    weight = pNode[*addr++ / 3];
                }
                for (int i = 0; i < 8; i++)
                    if (_elemDef.TexMatrices[i])
                        addr++;

                if (_elemDef.PositionFmt != XFDataFormat.None) //Vertices
                    if (_elemDef.PositionFmt != XFDataFormat.Direct)
                    {
                        if (_elemDef.PositionFmt == XFDataFormat.Index16)
                        {
                            index = *(bushort*)addr;
                            addr += 2;
                        }
                        else
                        {
                            index = *(byte*)addr++;
                        }
                        //Match weight and index with remap table
                        int mapEntry = (weight << 16) | index;
                        int* pTmp = (int*)RemapTable.Address;

                        //Find matching index, starting at end of list
                        index = RemapSize;
                        while ((--index >= 0) && (pTmp[index] != mapEntry));

                        //No match, create new entry
                        //Will be processed into vertices at the end!
                        if (index < 0)
                            pTmp[index = RemapSize++] = mapEntry;

                        //Write index
                        fp.VertexId = (ushort)index;
                    }
                    else //Direct
                    {

                    }
                if (_elemDef.Normals)
                    if (_elemDef.NormalFmt != XFDataFormat.Direct)
                    {
                        if (_elemDef.NormalFmt == XFDataFormat.Index16)
                        {
                            index = *(bushort*)addr;
                            addr += 2;
                        }
                        else
                        {
                            index = *(byte*)addr++;
                        }
                        SetValue(pAssets, fp, index, 1);
                    }
                    else //Direct
                    {

                    }
                for (int i = 0; i < 2; i++) //Colors
                    if (_elemDef.Colors[i])
                        if (_elemDef.ColorFmt[i] != XFDataFormat.Direct)
                        {
                            if (_elemDef.ColorFmt[i] == XFDataFormat.Index16)
                            {
                                index = *(bushort*)addr;
                                addr += 2;
                            }
                            else
                            {
                                index = *(byte*)addr++;
                            }
                            SetValue(pAssets, fp, index, i + 2);
                        }
                        else //Direct
                        {

                        }
                for (int i = 0; i < 8; i++) //UVs
                    if (_elemDef.UVs[i])
                        if (_elemDef.UVFmt[i] != XFDataFormat.Direct)
                        {
                            if (_elemDef.UVFmt[i] == XFDataFormat.Index16)
                            {
                                index = *(bushort*)addr;
                                addr += 2;
                            }
                            else
                            {
                                index = *(byte*)addr++;
                            }
                            SetValue(pAssets, fp, index, i + 4);
                        }
                        else //Direct
                        {

                        }
            }
            return fp;
        }

        private void SetValue(byte** pAssets, FacePoint point, int index, int type)
        {
            //Input data from asset cache
            switch(type)
            {
                case 1:
                    Vector3* tIn1 = (Vector3*)pAssets[type] + index;
                    point.Normal = *tIn1;
                    break;
                case 2:
                case 3:
                    uint* tIn2 = (uint*)pAssets[type] + index;
                    point.Color[type - 2] = *tIn2++;
                    break;
                default:
                    float* tIn3 = (float*)pAssets[type] + index;
                    point.UVs[type - 4] = (*tIn3++);
                    point.UVs[type - 3] = (*tIn3++);
                    break;
            }
        }

        internal unsafe List<Vertex3> Finish(Vector3* pVert, IMatrixNode[] nodeTable)
        {
            //Create vertex list from remap table
            List<Vertex3> list = new List<Vertex3>(RemapSize);

            if (_elemDef.Weighted)
            {
                ushort* pMap = (ushort*)RemapTable.Address;
                for (int i = 0; i < RemapSize; i++)
                {
                    //Create new vertex, assigning the value + influence from the remap table
                    Vertex3 v = new Vertex3(pVert[*pMap++], nodeTable[*pMap++]);
                    //Add vertex to list
                    list.Add(v);
                }
            }
            else
            {
                //Add vertex to list using raw value.
                int* pMap = (int*)RemapTable.Address;
                for (int i = 0; i < RemapSize; i++)
                    list.Add(new Vertex3(pVert[*pMap++]));
            }

            //Clean up
            RemapTable.Dispose();
            RemapTable = null;

            return list;
        }

        public void SetPrimitives(MDL0Polygon* polygon)
        {

        }
        #endregion

        public void Precalc()
        {
            foreach (Vertex3 v in _vertices)
                v.Weight();

            foreach (Primitive2 p in _primitives)
                p.Precalc(this);
        }

        internal void Render(GLContext ctx)
        {
            if (!_render)
                return;

            //Set single bind matrix
            if (_singleBind != null)
            {
                ctx.glPushMatrix();
                Matrix m = _singleBind.Matrix;
                ctx.glMultMatrix((float*)&m);
            }

            //Enable arrays
            ctx.glEnableClientState(GLArrayType.VERTEX_ARRAY);

            if (_elemDef.Normals)
                ctx.glEnableClientState(GLArrayType.NORMAL_ARRAY);

            if (_elemDef.Colors[0])
                ctx.glEnableClientState(GLArrayType.COLOR_ARRAY);

            if ((_material != null) && (_material._children.Count > 0))
            {
                ctx.glEnableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
                ctx.glEnable(GLEnableCap.Texture2D);
                foreach (MDL0MaterialRefNode mr in _material.Children)
                {
                    if (mr._texture.Enabled)
                        mr.Bind(ctx);
                }
                foreach (Primitive2 prim in _primitives)
                {
                    prim.PreparePointers(_elemDef, ctx);
                    prim.Render(ctx, 0);
                }
                ctx.glDisable((uint)GLEnableCap.Texture2D);
                ctx.glDisableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
            }
            else
            {
                ctx.glDisable((uint)GLEnableCap.Texture2D);
                foreach (Primitive2 prim in _primitives)
                {
                    prim.PreparePointers(_elemDef, ctx);
                    prim.Render(ctx, -1);
                }
            }

            //Disable arrays
            if (_elemDef.Normals)
                ctx.glDisableClientState(GLArrayType.NORMAL_ARRAY);

            if (_elemDef.Colors[0])
                ctx.glDisableClientState(GLArrayType.COLOR_ARRAY);

            ctx.glDisableClientState(GLArrayType.VERTEX_ARRAY);

            //Pop matrix
            if (_singleBind != null)
                ctx.glPopMatrix();
        }
    }
}
