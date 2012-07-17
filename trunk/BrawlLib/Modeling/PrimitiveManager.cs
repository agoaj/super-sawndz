using System;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Models;
using BrawlLib.Imaging;
using BrawlLib.Wii.Graphics;
using BrawlLib.OpenGL;
using System.Drawing;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.Modeling
{
    public unsafe class NewPrimitive : IDisposable
    {
        internal GLPrimitiveType _type;
        internal int _elementCount;
        internal UnsafeBuffer _indices;

        public NewPrimitive(int elements, GLPrimitiveType type)
        {
            _elementCount = elements;
            _type = type;
            _indices = new UnsafeBuffer(_elementCount * 2);
        }

        ~NewPrimitive() { Dispose(); }
        public void Dispose()
        {
            if (_indices != null)
            {
                _indices.Dispose();
                _indices = null;
            }
        }

        internal unsafe void Render(GLContext ctx)
        {
            ctx.glDrawElements(_type, _elementCount, GLElementType.UNSIGNED_SHORT, _indices.Address);
        }
    }

    unsafe class PrimitiveManager : IDisposable
    {
        public List<Vertex3> _vertices;
        internal UnsafeBuffer _indices;

        public ElementDescriptor _desc;
        internal int _pointCount, _faceCount, _stride;
        public MDL0PolygonNode _polygon;

        //The primitives indices match up to these values as an index.
        internal UnsafeBuffer[] _faceData = new UnsafeBuffer[12];
        internal bool[] _dirty = new bool[12];

        //0 is Vertices
        //1 is Normals
        //2-3 is Colors
        //4-12 is UVs

        internal Dictionary<int, IMatrixNode> Nodes;

        public UnsafeBuffer _graphicsBuffer;
        
        //Graphics buffer is a combination of the _faceData streams.
        //Vertex (Vertex3 - 12 bytes), Normal (Vertex3 - 12 bytes),
        //Color1 & Color2 (Float - 4 bytes each), UVs 0 - 7 (Vertex2 - 8 bytes each),
        //Repeat. It is read as a pointer. 

        internal NewPrimitive _triangles, _tristrips, _trifans, _lines, _points;
        internal List<Primitive2> Primitives = new List<Primitive2>();

        #region Asset Lists
        private Vector3[] _rawVertices;
        public Vector3[] RawVertices
        {
            get
            {
                if (_rawVertices != null && _rawVertices.Length != 0) 
                    return _rawVertices;

                int i = 0;
                _rawVertices = new Vector3[_vertices.Count];
                foreach (Vertex3 v in _vertices)
                    _rawVertices[i++] = v.Position;
                return _rawVertices;
            }
        }
        private Vector3[] _rawNormals;
        public Vector3[] RawNormals
        {
            get
            {
                if (_rawNormals != null && _rawNormals.Length != 0) 
                    return _rawNormals;

                HashSet<Vector3> list = new HashSet<Vector3>();
                Vector3* pIn = (Vector3*)_faceData[1].Address;
                for (int i = 0; i < _pointCount; i++)
                    list.Add(*pIn++);

                _rawNormals = new Vector3[list.Count];
                list.CopyTo(_rawNormals);
                
                return _rawNormals;
            }
        }
        private Vector2[][] _uvs = new Vector2[8][];
        public Vector2[] UVs(int index)
        {
            if (_uvs[index] != null && _uvs[index].Length != 0) 
                return _uvs[index];

            HashSet<Vector2> list = new HashSet<Vector2>();
            Vector2* pIn = (Vector2*)_faceData[index + 4].Address;
            for (int i = 0; i < _pointCount; i++)
                list.Add(*pIn++);

            _uvs[index] = new Vector2[list.Count];
            list.CopyTo(_uvs[index]);

            return _uvs[index];
        }
        private RGBAPixel[][] _colors = new RGBAPixel[2][];
        public RGBAPixel[] Colors(int index)
        {
            if (_colors[index] != null && _colors[index].Length != 0) 
                return _colors[index];

            HashSet<RGBAPixel> list = new HashSet<RGBAPixel>();
            RGBAPixel* pIn = (RGBAPixel*)_faceData[index + 2].Address;
            for (int i = 0; i < _pointCount; i++)
                list.Add(*pIn++);

            _colors[index] = new RGBAPixel[list.Count];
            list.CopyTo(_colors[index]);

            return _colors[index];
        }
        #endregion

        public PrimitiveManager() { }
        public PrimitiveManager(MDL0Polygon* polygon, AssetStorage assets, IMatrixNode[] nodes, MDL0PolygonNode p)
        {
            _polygon = p;
            Nodes = new Dictionary<int, IMatrixNode>();

            byte*[] pAssetList = new byte*[12];
            byte*[] pOutList = new byte*[12];
            int id;

            //This relies on the header being accurate!
            _indices = new UnsafeBuffer(2 * (_pointCount = polygon->_numVertices));
            _faceCount = polygon->_numFaces;

            //Grab asset lists in sequential order.
            if ((id = polygon->_vertexId) >= 0)
            {
                pAssetList[0] = (byte*)assets.Assets[0][id].Address;
                pOutList[0] = (byte*)(_faceData[0] = new UnsafeBuffer(12 * _pointCount)).Address;
            }
            if ((id = polygon->_normalId) >= 0)
            {
                pAssetList[1] = (byte*)assets.Assets[1][id].Address;
                pOutList[1] = (byte*)(_faceData[1] = new UnsafeBuffer(12 * _pointCount)).Address;
            }
            for (int i = 0, x = 2; i < 2; i++, x++)
                if ((id = ((bshort*)polygon->_colorIds)[i]) >= 0)
                {
                    pAssetList[x] = (byte*)assets.Assets[2][id].Address;
                    pOutList[x] = (byte*)(_faceData[x] = new UnsafeBuffer(4 * _pointCount)).Address;
                }
            for (int i = 0, x = 4; i < 8; i++, x++)
                if ((id = ((bshort*)polygon->_uids)[i]) >= 0)
                {
                    pAssetList[x] = (byte*)assets.Assets[3][id].Address;
                    pOutList[x] = (byte*)(_faceData[x] = new UnsafeBuffer(8 * _pointCount)).Address;
                }

            //Compile decode script by reading the polygon def list
            //This sets how to read the facepoints
            _desc = new ElementDescriptor(polygon);

            //Extract primitives, using our descriptor and asset lists
            fixed (byte** pOut = pOutList)
            fixed (byte** pAssets = pAssetList)
                ExtractPrimitives(polygon, ref _desc, pOut, pAssets);

            //Compile merged vertex list
            _vertices = _desc.Finish((Vector3*)pAssetList[0], nodes);
        }
        ~PrimitiveManager() { Dispose(); }
        public void Dispose()
        {
            if (_graphicsBuffer != null)
            { _graphicsBuffer.Dispose(); _graphicsBuffer = null; }
        }

        internal void WritePrimitives(MDL0PolygonNode poly, MDL0Polygon* header)
        {
            _polygon = poly;

            int stride = poly.fpStride;
            VoidPtr address = header->PrimitiveData;
            VoidPtr start = header->PrimitiveData;
            GXVtxDescList[] desc = poly._descList;
            int[] nodeIds = poly._nodeCache;
            _desc.Addresses = new List<uint>();
            _desc.NodeIds = new List<ushort>();
            ushort node;

            foreach (PrimitiveGroup g in poly.groups)
            {
                if (!poly.Model._isImport)
                {
                    g.nodeIds.Clear();
                    for (int i = 0; i < g._headers.Count; i++)
                    {
                        //Re-assign node ids, just in case the nodes were moved. The count won't change
                        foreach (Facepoint point in g._points[i])
                            if (!g.nodeIds.Contains(point.NodeId))
                                g.nodeIds.Add(point.NodeId);
                    }
                }

                int index = 0;
                int id = 0;
                if (poly.Weighted)
                {
                    if (poly.TexMtx)
                    {
                        //Texture Matrices
                        for (int i = 0; i < g.nodeIds.Count; i++)
                        {
                            *(byte*)address++ = 0x30;
                            *(bushort*)address = node = (ushort)g.nodeIds[id++];

                            _desc.Addresses.Add((uint)address - (uint)start);
                            _desc.NodeIds.Add(node);

                            address += 2;
                            *(byte*)address++ = 0xB0;
                            *(byte*)address++ = (byte)(0x78 + (12 * index++));
                        }
                    }

                    index = 0;
                    id = 0;

                    //Position Matrices
                    for (int i = 0; i < g.nodeIds.Count; i++)
                    {
                        *(byte*)address++ = 0x20;
                        *(bushort*)address = node = (ushort)g.nodeIds[id++];

                        _desc.Addresses.Add((uint)address - (uint)start);
                        _desc.NodeIds.Add(node);

                        address += 2;
                        *(byte*)address++ = 0xB0;
                        *(byte*)address++ = (byte)(12 * index++);
                    }

                    index = 0;
                    id = 0;

                    //Normal Matrices
                    for (int i = 0; i < g.nodeIds.Count; i++)
                    {
                        *(byte*)address++ = 0x28;
                        *(bushort*)address = node = (ushort)g.nodeIds[id++];

                        _desc.Addresses.Add((uint)address - (uint)start);
                        _desc.NodeIds.Add(node);

                        address += 2;
                        *(byte*)address++ = 0x84;
                        *(byte*)address++ = (byte)(9 * index++);
                    }
                }

                if (poly.Model._isImport)
                {
                    if (g.Tristrips.Count != 0)
                    {
                        foreach (Tristrip tri in g.Tristrips)
                        {
                            *(PrimitiveHeader*)address = new PrimitiveHeader() { Type = WiiPrimitiveType.TriangleStrip, Entries = (ushort)tri.points.Count }; address += 3;
                            foreach (Facepoint f in tri.points)
                                WriteFacepoint(f, g, desc, ref address);
                        }
                    }
                    if (g.Triangles.Count != 0)
                    {
                        *(PrimitiveHeader*)address = new PrimitiveHeader() { Type = WiiPrimitiveType.Triangles, Entries = (ushort)(g.Triangles.Count * 3) }; address += 3;
                        foreach (Triangle tri in g.Triangles)
                        {
                            WriteFacepoint(tri.x, g, desc, ref address);
                            WriteFacepoint(tri.y, g, desc, ref address);
                            WriteFacepoint(tri.z, g, desc, ref address);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < g._headers.Count; i++)
                    {
                        *(PrimitiveHeader*)address = g._headers[i]; address += 3;
                        foreach (Facepoint point in g._points[i])
                            WriteFacepoint(point, g, desc, ref address);
                    }
                }
            }
        }

        internal void WriteFacepoint(Facepoint f, PrimitiveGroup g, GXVtxDescList[] desc, ref VoidPtr address)
        {
            foreach (GXVtxDescList d in desc)
                switch (d.attr)
                {
                    case GXAttr.GX_VA_PNMTXIDX:
                        if (d.type == XFDataFormat.Direct)
                            *(byte*)address++ = (byte)(3 * g.nodeIds.IndexOf(f.NodeId));
                        break;
                    case GXAttr.GX_VA_TEX0MTXIDX:
                    case GXAttr.GX_VA_TEX1MTXIDX:
                    case GXAttr.GX_VA_TEX2MTXIDX:
                    case GXAttr.GX_VA_TEX3MTXIDX:
                    case GXAttr.GX_VA_TEX4MTXIDX:
                    case GXAttr.GX_VA_TEX5MTXIDX:
                    case GXAttr.GX_VA_TEX6MTXIDX:
                    case GXAttr.GX_VA_TEX7MTXIDX:
                        if (d.type == XFDataFormat.Direct)
                            *(byte*)address++ = (byte)(30 + (3 * g.nodeIds.IndexOf(f.NodeId)));
                        break;
                    case GXAttr.GX_VA_POS:
                        switch (d.type)
                        {
                            case XFDataFormat.Direct:
                                //*(BVec3*)address = f.Vertex.Position;
                                address += 12;
                                break;
                            case XFDataFormat.Index8:
                                *(byte*)address++ = (byte)f.VertexIndex;
                                break;
                            case XFDataFormat.Index16:
                                *(bushort*)address = (ushort)f.VertexIndex;
                                address += 2;
                                break;
                        }
                        break;
                    case GXAttr.GX_VA_NRM:
                        switch (d.type)
                        {
                            case XFDataFormat.Direct:
                                //*(BVec3*)address = f.Normal;
                                address += 12;
                                break;
                            case XFDataFormat.Index8:
                                *(byte*)address++ = (byte)f.NormalIndex;
                                break;
                            case XFDataFormat.Index16:
                                *(bushort*)address = (ushort)f.NormalIndex;
                                address += 2;
                                break;
                        }
                        break;
                    case GXAttr.GX_VA_CLR0:
                    case GXAttr.GX_VA_CLR1:
                        switch (d.type)
                        {
                            case XFDataFormat.Direct:
                                //*(byte*)address++ = (byte)f.Color[(int)d.attr - 11].R;
                                //*(byte*)address++ = (byte)f.Color[(int)d.attr - 11].G;
                                //*(byte*)address++ = (byte)f.Color[(int)d.attr - 11].B;
                                //*(byte*)address++ = (byte)f.Color[(int)d.attr - 11].A;
                                break;
                            case XFDataFormat.Index8:
                                if ((_polygon._c0Changed && d.attr == GXAttr.GX_VA_CLR0) ||
                                    (_polygon._c1Changed && d.attr == GXAttr.GX_VA_CLR1))
                                    *(byte*)address++ = 0;
                                else
                                    *(byte*)address++ = (byte)f.ColorIndex[(int)d.attr - 11];
                                break;
                            case XFDataFormat.Index16:
                                if ((_polygon._c0Changed && d.attr == GXAttr.GX_VA_CLR0) ||
                                    (_polygon._c1Changed && d.attr == GXAttr.GX_VA_CLR1))
                                    *(bushort*)address = 0;
                                else
                                    *(bushort*)address = (ushort)f.ColorIndex[(int)d.attr - 11];
                                address += 2;
                                break;
                        }
                        break;
                    case GXAttr.GX_VA_TEX0:
                    case GXAttr.GX_VA_TEX1:
                    case GXAttr.GX_VA_TEX2:
                    case GXAttr.GX_VA_TEX3:
                    case GXAttr.GX_VA_TEX4:
                    case GXAttr.GX_VA_TEX5:
                    case GXAttr.GX_VA_TEX6:
                    case GXAttr.GX_VA_TEX7:
                        switch (d.type)
                        {
                            case XFDataFormat.Direct:
                                //*(BVec2*)address = f.UV[(int)d.attr - 11];
                                address += 8;
                                break;
                            case XFDataFormat.Index8:
                                *(byte*)address++ = (byte)f.UVIndex[(int)d.attr - 13];
                                break;
                            case XFDataFormat.Index16:
                                *(bushort*)address = (ushort)f.UVIndex[(int)d.attr - 13];
                                address += 2;
                                break;
                        }
                        break;
                }
        }

        internal void ExtractPrimitives(MDL0Polygon* header, ref ElementDescriptor desc, byte** pOut, byte** pAssets)
        {
            int count;
            ushort index = 0, temp;
            byte* pData = (byte*)header->PrimitiveData;
            byte* pTemp = (byte*)pData;
            ushort* indices = (ushort*)_indices.Address;

            //Get element count for each primitive type
            int d5 = 0, d4 = 0, d3 = 0, d2 = 0, d1 = 0;
            ushort* p5, p4, p3, p2, p1;

            bool newGroup = true;
            PrimitiveGroup group = new PrimitiveGroup();
            ushort id;

            //Get counts for each primitive type, and assign face points
        NextPrimitive:
            //Switch by primitive type and increment as well so we can read the count.
            switch ((GXListCommand)(*pTemp++))
            {
                //Fill weight cache
                case GXListCommand.LoadIndexA: //Positions
                    if (newGroup == false)
                    {
                        _polygon.groups.Add(group);
                        group = new PrimitiveGroup();
                        newGroup = true;
                    }
                    if (!group.nodeIds.Contains(id = *(bushort*)pTemp) && id != ushort.MaxValue)
                    {
                        group.nodeIds.Add(id);
                        group._nodes.Add(_polygon.Model._linker.NodeCache[id]);

                        if (!Nodes.ContainsKey(id))
                            Nodes.Add(id, _polygon.Model._linker.NodeCache[id]);
                    }

                    //Set weight node for facepoint extraction
                    desc.SetNode(ref pTemp, (byte*)pData);
                    goto NextPrimitive;

                //Not sure what to do here...
                case GXListCommand.LoadIndexB: //Normals
                case GXListCommand.LoadIndexC: //UVs
                    if (newGroup == false)
                    {
                        _polygon.groups.Add(group);
                        group = new PrimitiveGroup();
                        newGroup = true;
                    }
                    if (!group.nodeIds.Contains(id = *(bushort*)pTemp) && id != ushort.MaxValue)
                    {
                        group.nodeIds.Add(id);
                        group._nodes.Add(_polygon.Model._linker.NodeCache[id]);

                        if (!Nodes.ContainsKey(id))
                            Nodes.Add(id, _polygon.Model._linker.NodeCache[id]);
                    }

                    desc.AddAddr(ref pTemp, (byte*)pData);
                    goto NextPrimitive;

                case GXListCommand.LoadIndexD: //Lights
                    if (newGroup == false)
                    {
                        _polygon.groups.Add(group);
                        group = new PrimitiveGroup();
                        newGroup = true;
                    }
                    if (!group.nodeIds.Contains(id = *(bushort*)pTemp) && id != ushort.MaxValue)
                    {
                        group.nodeIds.Add(id);
                        group._nodes.Add(_polygon.Model._linker.NodeCache[id]);

                        if (!Nodes.ContainsKey(id))
                            Nodes.Add(id, _polygon.Model._linker.NodeCache[id]);
                    }

                    pTemp += 4; //Skip
                    goto NextPrimitive;

                case GXListCommand.DrawQuads:

                    if (newGroup == true) newGroup = false;
                    d3 += (count = *(bushort*)pTemp) / 2 * 3;
                    group._headers.Add(new PrimitiveHeader() { Type = WiiPrimitiveType.Quads, Entries = (ushort)count });
                    
                    break;

                case GXListCommand.DrawTriangles:

                    if (newGroup == true) newGroup = false;
                    d3 += (count = *(bushort*)pTemp);
                    group._headers.Add(new PrimitiveHeader() { Type = WiiPrimitiveType.Triangles, Entries = (ushort)count });
                    
                    break;

                case GXListCommand.DrawTriangleFan:

                    if (newGroup == true) newGroup = false;
                    d3 += ((count = *(bushort*)pTemp) - 2) * 3;
                    //d4 += (count = *(bushort*)pTemp);
                    group._headers.Add(new PrimitiveHeader() { Type = WiiPrimitiveType.TriangleFan, Entries = (ushort)count });
                    
                    break;

                case GXListCommand.DrawTriangleStrip:

                    if (newGroup == true) newGroup = false;
                    d3 += ((count = *(bushort*)pTemp) - 2) * 3;
                    //d5 += (count = *(bushort*)pTemp);
                    group._headers.Add(new PrimitiveHeader() { Type = WiiPrimitiveType.TriangleStrip, Entries = (ushort)count });
                    
                    break;

                case GXListCommand.DrawLines:

                    if (newGroup == true) newGroup = false;
                    d2 += (count = *(bushort*)pTemp);
                    group._headers.Add(new PrimitiveHeader() { Type = WiiPrimitiveType.Lines, Entries = (ushort)count });
                    
                    break;

                case GXListCommand.DrawLineStrip:

                    if (newGroup == true) newGroup = false;
                    d2 += ((count = *(bushort*)pTemp) - 1) * 2;
                    group._headers.Add(new PrimitiveHeader() { Type = WiiPrimitiveType.LineStrip, Entries = (ushort)count });
                    
                    break;

                case GXListCommand.DrawPoints:

                    if (newGroup == true) newGroup = false;
                    d1 += (count = *(bushort*)pTemp);
                    group._headers.Add(new PrimitiveHeader() { Type = WiiPrimitiveType.Points, Entries = (ushort)count });
                    
                    break;

                default:
                    _polygon.groups.Add(group);
                    goto Next; //No more primitives.
            }

            pTemp += 2;

            //Extract facepoints here!
            desc.Run(ref pTemp, pAssets, pOut, count, group, ref indices, _polygon.Model._linker.NodeCache);

            goto NextPrimitive;

        Next: //Create primitives
            //if (d5 > 0)
            //{ _tristrips = new NewPrimitive(d5, GLPrimitiveType.TriangleStrip); p5 = (ushort*)_tristrips._indices.Address; }
            //else
            //{ _tristrips = null; p5 = null; }

            //if (d4 > 0)
            //{ _trifans = new NewPrimitive(d4, GLPrimitiveType.TriangleFan); p4 = (ushort*)_trifans._indices.Address; }
            //else
            //{ _trifans = null; p4 = null; }

            if (d3 > 0)
            { _triangles = new NewPrimitive(d3, GLPrimitiveType.Triangles); p3 = (ushort*)_triangles._indices.Address; }
            else
            { _triangles = null; p3 = null; }

            if (d2 > 0)
            { _lines = new NewPrimitive(d2, GLPrimitiveType.Lines); p2 = (ushort*)_lines._indices.Address; }
            else
            { _lines = null; p2 = null; }

            if (d1 > 0)
            { _points = new NewPrimitive(d1, GLPrimitiveType.Points); p1 = (ushort*)_points._indices.Address; }
            else
            { _points = null; p1 = null; }

            //Extract indices in reverse order, this way we get CCW winding.
        Top:
            switch ((GXListCommand)(*pData++))
            {
                case GXListCommand.LoadIndexA:
                case GXListCommand.LoadIndexB:
                case GXListCommand.LoadIndexC:
                case GXListCommand.LoadIndexD:
                    pData += 4; //Skip
                    goto Top;

                case GXListCommand.DrawQuads:
                    count = *(bushort*)pData;
                    for (int i = 0; i < count; i += 4)
                    {
                        *p3++ = index;
                        *p3++ = (ushort)(index + 2);
                        *p3++ = (ushort)(index + 1);
                        *p3++ = index;
                        *p3++ = (ushort)(index + 3);
                        *p3++ = (ushort)(index + 2);

                        //if ((i & 3) == 2)
                        //{
                        //    *p3++ = index;
                        //    *p3++ = index;
                        //    *p3++ = (ushort)(index++ - 1);
                        //}
                        //*p3++ = index++;
                        index += 4;
                    }
                    break;
                case GXListCommand.DrawTriangles:
                    count = *(bushort*)pData;
                    for (int i = 0; i < count; i += 3)
                    {
                        *p3++ = (ushort)(index + 2);
                        *p3++ = (ushort)(index + 1);
                        *p3++ = index;
                        index += 3;
                    }
                    break;
                case GXListCommand.DrawTriangleFan:
                    count = *(bushort*)pData;
                    temp = index++;
                    for (int i = 2; i < count; i++)
                    {
                        *p3++ = temp;
                        *p3++ = (ushort)(index + 1);
                        *p3++ = index++;
                    }
                    index++;
                    break;
                case GXListCommand.DrawTriangleStrip:
                    count = *(bushort*)pData;
                    //temp = index;
                    index += 2;
                    for (int i = 2; i < count; i++)
                    {
                        if ((i & 1) == 0)
                        {
                            *p3++ = (ushort)(index - 2);
                            *p3++ = index;
                            *p3++ = (ushort)(index - 1);
                            index++;
                            //*p3++ = temp++;
                            //*p3++ = temp++;
                            //*p3++ = index++;
                        }
                        else
                        {
                            *p3++ = (ushort)(index - 1);
                            *p3++ = index;
                            *p3++ = (ushort)(index - 2);
                            index++;
                            //*p3++ = temp--;
                            //*p3++ = temp++;
                            //*p3++ = index++;
                        }
                    }
                    break;
                case GXListCommand.DrawLines:
                    count = *(bushort*)pData;
                    for (int i = 0; i < count; i++)
                        *p2++ = index++;
                    break;
                case GXListCommand.DrawLineStrip:
                    count = *(bushort*)pData;
                    for (int i = 1; i < count; i++)
                    {
                        *p2++ = index++;
                        *p2++ = index;
                    }
                    index++;
                    break;
                case GXListCommand.DrawPoints:
                    count = *(bushort*)pData;
                    for (int i = 0; i < count; i++)
                        *p1++ = index++;
                    break;
                default: return;
            }
            pData += 2 + count * desc.Stride;
            goto Top;
        }

        private void CalcStride()
        {
            _stride = 0;
            for (int i = 0; i < 2; i++)
                if (_faceData[i] != null)
                    _stride += 12;
            for (int i = 2; i < 4; i++)
                if (_faceData[i] != null)
                    _stride += 4;
            for (int i = 4; i < 12; i++)
                if (_faceData[i] != null)
                    _stride += 8;
        }

        internal Facepoint[] MergeData(MDL0PolygonNode poly)
        {
            Facepoint[] _facepoints = new Facepoint[_pointCount];

            ushort* pIndex = (ushort*)_indices.Address;
            for (int x = 0; x < 12; x++)
            {
                if (_faceData[x] == null && x != 0)
                    continue;

                switch (x)
                {
                    case 0:
                        for (int i = 0; i < _pointCount; i++)
                            if (_vertices.Count != 0)
                            {
                                Facepoint f = _facepoints[i] = new Facepoint();
                                f.VertexIndex = *pIndex++;
                                if (f.VertexIndex < _vertices.Count && f.VertexIndex >= 0)
                                    f.Vertex = _vertices[f.VertexIndex];
                            }
                        break;
                    case 1:
                        Vector3* pIn1 = (Vector3*)_faceData[x].Address;
                        for (int i = 0; i < _pointCount; i++)
                            _facepoints[i].NormalIndex = Array.IndexOf(RawNormals, *pIn1++); 
                        break;
                    case 2:
                    case 3:
                        RGBAPixel* pIn2 = (RGBAPixel*)_faceData[x].Address;
                        for (int i = 0; i < _pointCount; i++)
                            _facepoints[i].ColorIndex[x - 2] = Array.IndexOf(Colors(x - 2), *pIn2++); 
                        break;
                    default:
                        Vector2* pIn3 = (Vector2*)_faceData[x].Address;
                        for (int i = 0; i < _pointCount; i++)
                            _facepoints[i].UVIndex[x - 4] = Array.IndexOf(UVs(x - 4), *pIn3++); 
                        break;
                }
            }
            return _facepoints;
        }

        internal void MergeFaceData()
        {
            for (int i = 1; i < 12; i++)
            {
                if (_faceData[i] == null)
                    continue;

                ushort* pIndex = (ushort*)_indices.Address;
                byte* pIn = (byte*)_faceData[i].Address;
                switch (i)
                {
                    case 0:
                        for (int x = 0; x < _pointCount; x++, pIn += 12)
                            _vertices[*pIndex++].Position = *(Vector3*)pIn;
                        break;
                    case 1:
                        for (int x = 0; x < _pointCount; x++, pIn += 12)
                            _vertices[*pIndex++].Normal = *(Vector3*)pIn;
                        break;
                    case 2:
                    case 3:
                        for (int x = 0; x < _pointCount; x++, pIn += 4)
                            _vertices[*pIndex++].Color[i - 2] = *(RGBAPixel*)pIn;
                        break;
                    default:
                        for (int x = 0; x < _pointCount; x++, pIn += 8)
                            _vertices[*pIndex++].UV[i - 4] = *(Vector2*)pIn;
                        break;
                }
            }
        }

        internal void UpdateStream(int index)
        {
            _dirty[index] = false;

            if (_faceData[index] == null || _vertices.Count == 0)
                return;

            //Set starting address
            byte* pOut = (byte*)_graphicsBuffer.Address;
            for (int i = 0; i < index; i++)
                if (_faceData[i] != null)
                {
                    if (i < 2)
                        pOut += 12;
                    else if (i < 4)
                        pOut += 4;
                    else
                        pOut += 8;
                }
            
            ushort* pIndex = (ushort*)_indices.Address;
            if (index == 0) //Vertices
            {
                int v;
                for (int i = 0; i < _pointCount; i++, pOut += _stride)
                    if ((v = *pIndex++) < _vertices.Count && v >= 0)
                        *(Vector3*)pOut = _vertices[v].WeightedPosition;
            }
            else if (index == 1) //Normals
            {
                Vector3* pIn = (Vector3*)_faceData[index].Address;
                for (int i = 0; i < _pointCount; i++, pOut += _stride)
                    if (*pIndex < _vertices.Count && _vertices[*pIndex]._influence != null)
                        *(Vector3*)pOut = _vertices[*pIndex++]._influence.Matrix.GetRotationMatrix() * *pIn++;
                    else
                        *(Vector3*)pOut = *pIn++;
            }
            else if (index < 4) //Colors
            {
                RGBAPixel* pIn = (RGBAPixel*)_faceData[index].Address;
                for (int i = 0; i < _pointCount; i++, pOut += _stride)
                    *(RGBAPixel*)pOut = *pIn++;
            }
            else //UVs
            {
                Vector2* pIn = (Vector2*)_faceData[index].Address;
                for (int i = 0; i < _pointCount; i++, pOut += _stride)
                    *(Vector2*)pOut = *pIn++;
            }
        }

        internal unsafe void PrepareStream(GLContext ctx)
        {
            CalcStride();
            int bufferSize = _stride * _pointCount;

            //Dispose of buffer if size doesn't match
            if ((_graphicsBuffer != null) && (_graphicsBuffer.Length != bufferSize))
            {
                _graphicsBuffer.Dispose();
                _graphicsBuffer = null;
            }

            //Create data buffer
            if (_graphicsBuffer == null)
            {
                _graphicsBuffer = new UnsafeBuffer(bufferSize);
                for (int i = 0; i < 12; i++)
                    _dirty[i] = true;
            }

            byte* pData = (byte*)_graphicsBuffer.Address;
            for (int i = 0; i < 12; i++)
            {
                //Write stream if dirty
                if (_dirty[i]) UpdateStream(i);

                if (_faceData[i] == null)
                    continue;

                switch (i)
                {
                    case 0:
                        ctx.glEnableClientState(GLArrayType.VERTEX_ARRAY);
                        ctx.glVertexPointer(3, GLDataType.Float, _stride, pData);
                        pData += 12;
                        break;

                    case 1:
                        ctx.glEnableClientState(GLArrayType.NORMAL_ARRAY);
                        ctx.glNormalPointer(GLDataType.Float, _stride, pData);
                        pData += 12;
                        break;

                    case 2:
                        ctx.glEnableClientState(GLArrayType.COLOR_ARRAY);
                        ctx.glColorPointer(4, GLDataType.Byte, _stride, pData);
                        pData += 4;
                        break;

                    case 3:
                        //ctx.glEnableClientState(GLArrayType.COLOR_ARRAY);
                        //ctx.glColorPointer(4, GLDataType.Byte, _stride, pData);
                        pData += 4;
                        break;

                    default:
                        //ctx.glEnable(GLEnableCap.Texture2D);
                        //ctx.glEnableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
                        //ctx.glTexCoordPointer(2, GLDataType.Float, _stride, pData);
                        pData += 8;
                        break;
                }
            }
        }

        internal unsafe void DetachStreams(GLContext ctx)
        {
            ctx.glDisableClientState(GLArrayType.COLOR_ARRAY);
            ctx.glDisableClientState(GLArrayType.NORMAL_ARRAY);
            ctx.glDisableClientState(GLArrayType.VERTEX_ARRAY);
            ctx.glDisableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
            ctx.glDisable((uint)GLEnableCap.Texture2D);
        }

        internal void RenderTexture(GLContext ctx, MDL0MaterialRefNode texgen)
        {
            if (texgen != null)
            {
                int texId = texgen.TextureCoordId;
                //texId = texId < 0 ? 0 : texId;
                if ((texId >= 0) && (_faceData[texId += 4] != null))
                {
                    byte* pData = (byte*)_graphicsBuffer.Address;
                    for (int i = 0; i < texId; i++)
                        if (_faceData[i] != null)
                        {
                            if (i < 2)
                                pData += 12;
                            else if (i < 4)
                                pData += 4;
                            else
                                pData += 8;
                        }

                    ctx.glDisable((uint)GLEnableCap.TEXTURE_GEN_S);
                    ctx.glDisable((uint)GLEnableCap.TEXTURE_GEN_T);
                    ctx.glDisable((uint)GLEnableCap.TEXTURE_GEN_Q);
                    ctx.glEnable(GLEnableCap.Texture2D);
                    ctx.glEnableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
                    //ctx.glBindBuffer(BrawlLib.OpenGL.etc.BufferTarget.ArrayBuffer, vertex_buffer_object);
                    ctx.glTexCoordPointer(2, GLDataType.Float, _stride, pData);
                }
                else
                {
                    if (texId < 0)
                    {
                        switch (texId)
                        {
                            case -1: //Vertex coords
                                
                                break;
                            case -2: //Normal coords
                                ctx.glEnable(GLEnableCap.TEXTURE_GEN_S);
                                ctx.glTexGen(TextureCoordName.S, TextureGenParameter.TEXTURE_GEN_MODE, (int)TextureGenMode.SPHERE_MAP);
                                ctx.glEnable(GLEnableCap.TEXTURE_GEN_T);
                                ctx.glTexGen(TextureCoordName.T, TextureGenParameter.TEXTURE_GEN_MODE, (int)TextureGenMode.SPHERE_MAP);
                                ctx.glEnable(GLEnableCap.TEXTURE_GEN_Q);
                                ctx.glTexGen(TextureCoordName.Q, TextureGenParameter.TEXTURE_GEN_MODE, (int)TextureGenMode.SPHERE_MAP);
                                break;
                            case -3: //Color coords

                                break;
                            case -4: //Binormal B coords

                                break;
                            case -5: //Binormal T coords

                                break;
                            default:
                                ctx.glDisable((uint)GLEnableCap.Texture2D);
                                ctx.glDisableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
                                break;
                        }
                    }
                    else
                    {
                        ctx.glDisable((uint)GLEnableCap.Texture2D);
                        ctx.glDisableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
                    }
                }
            }
            else
            {
                ctx.glDisable((uint)GLEnableCap.Texture2D);
                ctx.glDisableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
            }

            if (_tristrips != null)
                _tristrips.Render(ctx);
            if (_trifans != null)
                _trifans.Render(ctx);
            if (_triangles != null)
                _triangles.Render(ctx);
            if (_lines != null)
                _lines.Render(ctx);
            if (_points != null)
                _points.Render(ctx);
        }

        internal void Weight()
        {
            foreach (Vertex3 v in _vertices)
                v.Weight();
            _dirty[0] = true;
        }

        internal void UnWeight()
        {
            foreach (Vertex3 v in _vertices)
                v.UnWeight();
        }

        #region Flags

        public GXVtxDescList[] setDescList(MDL0PolygonNode polygon)
        {
            //Everything is set in the order the facepoint is written!

            short[] indices = polygon._elementIndices;
            int textures = 0, colors = 0;

            //Create new command list
            List<GXVtxDescList> list = new List<GXVtxDescList>();

            if (polygon.Weighted)
            {
                polygon._arrayFlags.HasPosMatrix = true;
                polygon._vertexFormat.HasPosMatrix = true;

                list.Add(new GXVtxDescList() { attr = GXAttr.GX_VA_PNMTXIDX, type = XFDataFormat.Direct });

                polygon.fpStride++;

                //There are no texture matrices without a position/normal matrix also
                for (int i = 0; i < 8; i++)
                    if (polygon._vertexFormat.GetHasTexMatrix(i))
                    {
                        list.Add(new GXVtxDescList() { attr = (GXAttr)(i + 1), type = XFDataFormat.Direct });
                        polygon.fpStride++;
                    }
            }
            else
            {
                polygon._vertexFormat.HasPosMatrix = false;
                polygon._arrayFlags.HasPosMatrix = false;
            }
            if (indices[0] > -1 && _faceData[0] != null) //Positions
            {
                polygon._arrayFlags.HasPositions = true;
                polygon._vertexFormat.PosFormat = (XFDataFormat)(RawVertices.Length > byte.MaxValue ? 3 : 2);

                list.Add(new GXVtxDescList() { attr = GXAttr.GX_VA_POS, type = (XFDataFormat)(RawVertices.Length > byte.MaxValue ? 3 : 2) });
                polygon.fpStride += RawVertices.Length > byte.MaxValue ? 2 : 1;
            }
            else
                polygon._arrayFlags.HasPositions = false;
            if (indices[1] > -1 && _faceData[1] != null) //Normals
            {
                polygon._arrayFlags.HasNormals = true;
                polygon._vertexFormat.NormalFormat = (XFDataFormat)(RawNormals.Length > byte.MaxValue ? 3 : 2);

                list.Add(new GXVtxDescList() { attr = GXAttr.GX_VA_NRM, type = (XFDataFormat)(RawNormals.Length > byte.MaxValue ? 3 : 2) });
                polygon.fpStride += RawNormals.Length > byte.MaxValue ? 2 : 1;
            }
            else
                polygon._arrayFlags.HasNormals = false;
            for (int i = 2; i < 4; i++)
                if (indices[i] > -1 && _faceData[i] != null) //Colors
                {
                    colors++;
                    polygon._arrayFlags.SetHasColor(i - 2, true);
                    polygon._vertexFormat.SetColorFormat(i - 2, (XFDataFormat)(Colors(i - 2).Length > byte.MaxValue ? 3 : 2));

                    list.Add(new GXVtxDescList() { attr = (GXAttr)(i + 9), type = (XFDataFormat)(Colors(i - 2).Length > byte.MaxValue ? 3 : 2) });
                    polygon.fpStride += Colors(i - 2).Length > byte.MaxValue ? 2 : 1;
                }
                else
                    polygon._arrayFlags.SetHasColor(i - 2, false);
            for (int i = 4; i < 12; i++)
                if (indices[i] > -1 && _faceData[i] != null) //UVs
                {
                    textures++;
                    polygon._arrayFlags.SetHasUVs(i - 4, true);
                    polygon._vertexFormat.SetUVFormat(i - 4, (XFDataFormat)(UVs(i - 4).Length > byte.MaxValue ? 3 : 2));

                    list.Add(new GXVtxDescList() { attr = (GXAttr)(i + 9), type = (XFDataFormat)(UVs(i - 4).Length > byte.MaxValue ? 3 : 2) });
                    polygon.fpStride += UVs(i - 4).Length > byte.MaxValue ? 2 : 1;
                }
                else
                    polygon._arrayFlags.SetHasUVs(i - 4, false);

            list.Add(new GXVtxDescList() { attr = GXAttr.GX_VA_NULL });

            polygon._vertexSpecs = new XFVertexSpecs(colors, textures, XFNormalFormat.XYZ);

            return list.ToArray();
        }

        public void SetVtxDescriptor(GXVtxDescList* attrPtr, MDL0PolygonNode polygon)
        {
            //This sets up how to read the facepoints.

            uint nnorms = 0;
            uint ncols = 0;
            uint ntexs = 0;

            uint pnMtxIdx = (int)XFDataFormat.None;
            uint txMtxIdxMask = 0;
            uint posn = (int)XFDataFormat.None;
            uint norm = (int)XFDataFormat.None;
            uint col0 = (int)XFDataFormat.None;
            uint col1 = (int)XFDataFormat.None;
            uint tex0 = (int)XFDataFormat.None;
            uint tex1 = (int)XFDataFormat.None;
            uint tex2 = (int)XFDataFormat.None;
            uint tex3 = (int)XFDataFormat.None;
            uint tex4 = (int)XFDataFormat.None;
            uint tex5 = (int)XFDataFormat.None;
            uint tex6 = (int)XFDataFormat.None;
            uint tex7 = (int)XFDataFormat.None;

            if (attrPtr != null)
            {
                while (attrPtr->attr != GXAttr.GX_VA_NULL)
                {
                    if (!((attrPtr->attr >= GXAttr.GX_VA_PNMTXIDX) && (attrPtr->attr <= GXAttr.GX_VA_MAX_ATTR)))
                        Console.WriteLine("Invalid attribute!");

                    if (!((attrPtr->type >= XFDataFormat.None) && (attrPtr->type <= XFDataFormat.Index16)))
                        Console.WriteLine("Invalid type!");

                    if ((attrPtr->attr >= GXAttr.GX_VA_PNMTXIDX) && (attrPtr->attr <= GXAttr.GX_VA_TEX7MTXIDX))
                        if (!((attrPtr->type == (int)XFDataFormat.None) || (attrPtr->type == XFDataFormat.Direct)))
                            Console.WriteLine("Invalid type for given attribute!");

                    switch (attrPtr->attr)
                    {
                        case GXAttr.GX_VA_PNMTXIDX:
                            pnMtxIdx = (uint)attrPtr->type;
                            break;

                        case GXAttr.GX_VA_TEX0MTXIDX:
                            txMtxIdxMask = (uint)(txMtxIdxMask & ~1) | ((uint)attrPtr->type << 0);
                            break;
                        case GXAttr.GX_VA_TEX1MTXIDX:
                            txMtxIdxMask = (uint)(txMtxIdxMask & ~2) | ((uint)attrPtr->type << 1);
                            break;
                        case GXAttr.GX_VA_TEX2MTXIDX:
                            txMtxIdxMask = (uint)(txMtxIdxMask & ~4) | ((uint)attrPtr->type << 2);
                            break;
                        case GXAttr.GX_VA_TEX3MTXIDX:
                            txMtxIdxMask = (uint)(txMtxIdxMask & ~8) | ((uint)attrPtr->type << 3);
                            break;
                        case GXAttr.GX_VA_TEX4MTXIDX:
                            txMtxIdxMask = (uint)(txMtxIdxMask & ~16) | ((uint)attrPtr->type << 4);
                            break;
                        case GXAttr.GX_VA_TEX5MTXIDX:
                            txMtxIdxMask = (uint)(txMtxIdxMask & ~32) | ((uint)attrPtr->type << 5);
                            break;
                        case GXAttr.GX_VA_TEX6MTXIDX:
                            txMtxIdxMask = (uint)(txMtxIdxMask & ~64) | ((uint)attrPtr->type << 6);
                            break;
                        case GXAttr.GX_VA_TEX7MTXIDX:
                            txMtxIdxMask = (uint)(txMtxIdxMask & ~128) | ((uint)attrPtr->type << 7);
                            break;

                        case GXAttr.GX_VA_POS:
                            posn = (uint)attrPtr->type;
                            break;

                        case GXAttr.GX_VA_NRM:
                            if (attrPtr->type != XFDataFormat.None)
                            { norm = (uint)attrPtr->type; nnorms = 1; } break;

                        case GXAttr.GX_VA_NBT:
                            if (attrPtr->type != XFDataFormat.None)
                            { norm = (uint)attrPtr->type; nnorms = 2; } break;

                        case GXAttr.GX_VA_CLR0: col0 = (uint)attrPtr->type; ncols += (uint)(col0 != 0 ? 1 : 0); break;
                        case GXAttr.GX_VA_CLR1: col1 = (uint)attrPtr->type; ncols += (uint)(col1 != 0 ? 1 : 0); break;

                        case GXAttr.GX_VA_TEX0: tex0 = (uint)attrPtr->type; ntexs += (uint)(tex0 != 0 ? 1 : 0); break;
                        case GXAttr.GX_VA_TEX1: tex1 = (uint)attrPtr->type; ntexs += (uint)(tex1 != 0 ? 1 : 0); break;
                        case GXAttr.GX_VA_TEX2: tex2 = (uint)attrPtr->type; ntexs += (uint)(tex2 != 0 ? 1 : 0); break;
                        case GXAttr.GX_VA_TEX3: tex3 = (uint)attrPtr->type; ntexs += (uint)(tex3 != 0 ? 1 : 0); break;
                        case GXAttr.GX_VA_TEX4: tex4 = (uint)attrPtr->type; ntexs += (uint)(tex4 != 0 ? 1 : 0); break;
                        case GXAttr.GX_VA_TEX5: tex5 = (uint)attrPtr->type; ntexs += (uint)(tex5 != 0 ? 1 : 0); break;
                        case GXAttr.GX_VA_TEX6: tex6 = (uint)attrPtr->type; ntexs += (uint)(tex6 != 0 ? 1 : 0); break;
                        case GXAttr.GX_VA_TEX7: tex7 = (uint)attrPtr->type; ntexs += (uint)(tex7 != 0 ? 1 : 0); break;
                        default: break;
                    }
                    attrPtr++;
                }

                polygon._vertexFormat._lo = ShiftVtxLo(pnMtxIdx, txMtxIdxMask, posn, norm, col0, col1);
                polygon._vertexFormat._hi = ShiftVtxHi(tex0, tex1, tex2, tex3, tex4, tex5, tex6, tex7);
                polygon._vertexSpecs = new XFVertexSpecs((int)ncols, (int)ntexs, (XFNormalFormat)nnorms);
            }
        }

        public GXVtxAttrFmtList[] setFmtList(MDL0PolygonNode polygon, ModelLinker linker)
        {
            List<GXVtxAttrFmtList> list = new List<GXVtxAttrFmtList>();
            VertexCodec vert = null;
            ColorCodec col = null;

            for (int i = 0; i < 12; i++)
            {
                if (polygon._manager._faceData[i] != null)
                    switch (i)
                    {
                        case 0: //Positions
                            if (linker._vertices != null && linker._vertices.Count != 0 && polygon._elementIndices[0] != -1)
                                if ((vert = linker._vertices[polygon._elementIndices[0]]) != null)
                                    list.Add(new GXVtxAttrFmtList()
                                    {
                                        attr = GXAttr.GX_VA_POS,
                                        type = (GXCompType)vert._type,
                                        cnt = (GXCompCnt)(vert._hasZ ? 1 : 0),
                                        frac = (byte)vert._scale
                                    });
                            break;
                        case 1: //Normals
                            vert = null;
                            if (linker._normals != null && linker._normals.Count != 0 && polygon._elementIndices[1] != -1)
                                if ((vert = linker._normals[polygon._elementIndices[1]]) != null)
                                    list.Add(new GXVtxAttrFmtList()
                                    {
                                        attr = GXAttr.GX_VA_NRM,
                                        type = (GXCompType)vert._type,
                                        cnt = (GXCompCnt)0,
                                        frac = (byte)vert._scale
                                    });
                            break;
                        case 2: //Color 1
                        case 3: //Color 2
                            col = null;
                            if (linker._colors != null && linker._colors.Count != 0 && polygon._elementIndices[i] != -1)
                                if ((col = linker._colors[polygon._elementIndices[i]]) != null)
                                    list.Add(new GXVtxAttrFmtList()
                                    {
                                        attr = (GXAttr)((int)GXAttr.GX_VA_CLR0 + (i - 2)),
                                        type = (GXCompType)col._outType,
                                        cnt = (GXCompCnt)(col._hasAlpha ? 1 : 0),
                                        frac = 0
                                    });
                            break;
                        case 4: //Tex 1
                        case 5: //Tex 2
                        case 6: //Tex 3
                        case 7: //Tex 4
                        case 8: //Tex 5
                        case 9: //Tex 6
                        case 10: //Tex 7
                        case 11: //Tex 8
                            vert = null;
                            if (linker._uvs != null && linker._uvs.Count != 0 && polygon._elementIndices[i] != -1)
                                if ((vert = linker._uvs[polygon._elementIndices[i]]) != null)
                                    list.Add(new GXVtxAttrFmtList()
                                    {
                                        attr = (GXAttr)((int)GXAttr.GX_VA_TEX0 + (i - 4)),
                                        type = (GXCompType)vert._type,
                                        cnt = GXCompCnt.GX_TEX_ST,
                                        frac = (byte)vert._scale
                                    });
                            break;
                    }
            }
            list.Add(new GXVtxAttrFmtList() { attr = GXAttr.GX_VA_NULL });
            return list.ToArray();
        }

        public void SetUVATGroups(GXVtxFmt vtxfmt, GXVtxAttrFmtList* list, MDL0Polygon* polygon)
        {
            //These are default values.

            uint posCnt = (int)GXCompCnt.GX_POS_XYZ;
            uint posType = (int)GXCompType.Float;
            uint posFrac = 0;

            uint nrmCnt = (int)GXCompCnt.GX_NRM_XYZ;
            uint nrmType = (int)GXCompType.Float;
            uint nrmIdx3 = 0;

            uint c0Cnt = (int)GXCompCnt.GX_CLR_RGBA;
            uint c0Type = (int)GXCompType.RGBA8;
            uint c1Cnt = (int)GXCompCnt.GX_CLR_RGBA;
            uint c1Type = (int)GXCompType.RGBA8;

            uint tx0Cnt = (int)GXCompCnt.GX_TEX_ST;
            uint tx0Type = (int)GXCompType.Float;
            uint tx0Frac = 0;
            uint tx1Cnt = (int)GXCompCnt.GX_TEX_ST;
            uint tx1Type = (int)GXCompType.Float;
            uint tx1Frac = 0;
            uint tx2Cnt = (int)GXCompCnt.GX_TEX_ST;
            uint tx2Type = (int)GXCompType.Float;
            uint tx2Frac = 0;
            uint tx3Cnt = (int)GXCompCnt.GX_TEX_ST;
            uint tx3Type = (int)GXCompType.Float;
            uint tx3Frac = 0;
            uint tx4Cnt = (int)GXCompCnt.GX_TEX_ST;
            uint tx4Type = (int)GXCompType.Float;
            uint tx4Frac = 0;
            uint tx5Cnt = (int)GXCompCnt.GX_TEX_ST;
            uint tx5Type = (int)GXCompType.Float;
            uint tx5Frac = 0;
            uint tx6Cnt = (int)GXCompCnt.GX_TEX_ST;
            uint tx6Type = (int)GXCompType.Float;
            uint tx6Frac = 0;
            uint tx7Cnt = (int)GXCompCnt.GX_TEX_ST;
            uint tx7Type = (int)GXCompType.Float;
            uint tx7Frac = 0;

            if (!(vtxfmt < GXVtxFmt.GX_MAX_VTXFMT))
                Console.WriteLine("GDSetVtxAttrFmtv: invalid vtx fmt");

            if (list != null)
            {
                while (list->attr != GXAttr.GX_VA_NULL)
                {
                    if (!((list->attr >= GXAttr.GX_VA_POS) && (list->attr <= GXAttr.GX_VA_TEX7)))
                        Console.WriteLine("GDSetVtxAttrFmtv: invalid attribute");
                    if (!(list->frac < 32))
                        Console.WriteLine("GDSetVtxAttrFmtv: invalid frac value");

                    switch (list->attr)
                    {
                        case GXAttr.GX_VA_POS:
                            posCnt = (uint)list->cnt;
                            posType = (uint)list->type;
                            posFrac = list->frac;
                            break;
                        case GXAttr.GX_VA_NRM:
                        case GXAttr.GX_VA_NBT:
                            nrmType = (uint)list->type;
                            if (list->cnt == GXCompCnt.GX_NRM_NBT3)
                            {
                                nrmCnt = (uint)GXCompCnt.GX_NRM_NBT;
                                nrmIdx3 = 1;
                            }
                            else
                            {
                                nrmCnt = (uint)list->cnt;
                                nrmIdx3 = 0;
                            }
                            break;
                        case GXAttr.GX_VA_CLR0:
                            c0Cnt = (uint)list->cnt;
                            c0Type = (uint)list->type;
                            break;
                        case GXAttr.GX_VA_CLR1:
                            c1Cnt = (uint)list->cnt;
                            c1Type = (uint)list->type;
                            break;
                        case GXAttr.GX_VA_TEX0:
                            tx0Cnt = (uint)list->cnt;
                            tx0Type = (uint)list->type;
                            tx0Frac = list->frac;
                            break;
                        case GXAttr.GX_VA_TEX1:
                            tx1Cnt = (uint)list->cnt;
                            tx1Type = (uint)list->type;
                            tx1Frac = list->frac;
                            break;
                        case GXAttr.GX_VA_TEX2:
                            tx2Cnt = (uint)list->cnt;
                            tx2Type = (uint)list->type;
                            tx2Frac = list->frac;
                            break;
                        case GXAttr.GX_VA_TEX3:
                            tx3Cnt = (uint)list->cnt;
                            tx3Type = (uint)list->type;
                            tx3Frac = list->frac;
                            break;
                        case GXAttr.GX_VA_TEX4:
                            tx4Cnt = (uint)list->cnt;
                            tx4Type = (uint)list->type;
                            tx4Frac = list->frac;
                            break;
                        case GXAttr.GX_VA_TEX5:
                            tx5Cnt = (uint)list->cnt;
                            tx5Type = (uint)list->type;
                            tx5Frac = list->frac;
                            break;
                        case GXAttr.GX_VA_TEX6:
                            tx6Cnt = (uint)list->cnt;
                            tx6Type = (uint)list->type;
                            tx6Frac = list->frac;
                            break;
                        case GXAttr.GX_VA_TEX7:
                            tx7Cnt = (uint)list->cnt;
                            tx7Type = (uint)list->type;
                            tx7Frac = list->frac;
                            break;
                        default:
                            break;
                    }
                    list++;
                }

                MDL0PolygonDefs* Defs = (MDL0PolygonDefs*)polygon->DefList;
                Defs->UVATA = (uint)ShiftUVATA(posCnt, posType, posFrac, nrmCnt, nrmType, c0Cnt, c0Type, c1Cnt, c1Type, tx0Cnt, tx0Type, tx0Frac, nrmIdx3);
                Defs->UVATB = (uint)ShiftUVATB(tx1Cnt, tx1Type, tx1Frac, tx2Cnt, tx2Type, tx2Frac, tx3Cnt, tx3Type, tx3Frac, tx4Cnt, tx4Type);
                Defs->UVATC = (uint)ShiftUVATC(tx4Frac, tx5Cnt, tx5Type, tx5Frac, tx6Cnt, tx6Type, tx6Frac, tx7Cnt, tx7Type, tx7Frac);
            }
        }

        #endregion

        #region Shifts

        //Vertex Format Lo Shift
        public uint ShiftVtxLo(uint pmidx, uint t76543210midx, uint pos, uint nrm, uint col0, uint col1)
        {
            return ((((uint)(pmidx)) << 0) |
                    (((uint)(t76543210midx)) << 1) |
                    (((uint)(pos)) << 9) |
                    (((uint)(nrm)) << 11) |
                    (((uint)(col0)) << 13) |
                    (((uint)(col1)) << 15));
        }

        //Vertex Format Hi Shift
        public uint ShiftVtxHi(uint tex0, uint tex1, uint tex2, uint tex3, uint tex4, uint tex5, uint tex6, uint tex7)
        {
            return ((((uint)(tex0)) << 0) |
                    (((uint)(tex1)) << 2) |
                    (((uint)(tex2)) << 4) |
                    (((uint)(tex3)) << 6) |
                    (((uint)(tex4)) << 8) |
                    (((uint)(tex5)) << 10) |
                    (((uint)(tex6)) << 12) |
                    (((uint)(tex7)) << 14));
        }

        //XF Specs Shift
        public uint ShiftXFSpecs(uint host_colors, uint host_normal, uint host_textures)
        {
            return ((((uint)(host_colors)) << 0) |
                    (((uint)(host_normal)) << 2) |
                    (((uint)(host_textures)) << 4));
        }

        //UVAT Group A Shift
        public uint ShiftUVATA(uint posCnt, uint posFmt, uint posShft, uint nrmCnt, uint nrmFmt, uint Col0Cnt, uint Col0Fmt, uint Col1Cnt, uint Col1Fmt, uint tex0Cnt, uint tex0Fmt, uint tex0Shft, uint normalIndex3)
        {
            return ((((uint)(posCnt)) << 0) |
                    (((uint)(posFmt)) << 1) |
                    (((uint)(posShft)) << 4) |
                    (((uint)(nrmCnt)) << 9) |
                    (((uint)(nrmFmt)) << 10) |
                    (((uint)(Col0Cnt)) << 13) |
                    (((uint)(Col0Fmt)) << 14) |
                    (((uint)(Col1Cnt)) << 17) |
                    (((uint)(Col1Fmt)) << 18) |
                    (((uint)(tex0Cnt)) << 21) |
                    (((uint)(tex0Fmt)) << 22) |
                    (((uint)(tex0Shft)) << 25) |
                    (((uint)(1)) << 30) | //Should always be 1
                    (((uint)(normalIndex3)) << 31));
        }

        //UVAT Group B Shift
        public uint ShiftUVATB(uint tex1Cnt, uint tex1Fmt, uint tex1Shft, uint tex2Cnt, uint tex2Fmt, uint tex2Shft, uint tex3Cnt, uint tex3Fmt, uint tex3Shft, uint tex4Cnt, uint tex4Fmt)
        {
            return ((((uint)(tex1Cnt)) << 0) |
                    (((uint)(tex1Fmt)) << 1) |
                    (((uint)(tex1Shft)) << 4) |
                    (((uint)(tex2Cnt)) << 9) |
                    (((uint)(tex2Fmt)) << 10) |
                    (((uint)(tex2Shft)) << 13) |
                    (((uint)(tex3Cnt)) << 18) |
                    (((uint)(tex3Fmt)) << 19) |
                    (((uint)(tex3Shft)) << 22) |
                    (((uint)(tex4Cnt)) << 27) |
                    (((uint)(tex4Fmt)) << 28) |
                    (((uint)(1)) << 31)); //Should always be 1
        }

        //UVAT Group C Shift
        public uint ShiftUVATC(uint tex4Shft, uint tex5Cnt, uint tex5Fmt, uint tex5Shft, uint tex6Cnt, uint tex6Fmt, uint tex6Shft, uint tex7Cnt, uint tex7Fmt, uint tex7Shft)
        {
            return ((((uint)(tex4Shft)) << 0) |
                    (((uint)(tex5Cnt)) << 5) |
                    (((uint)(tex5Fmt)) << 6) |
                    (((uint)(tex5Shft)) << 9) |
                    (((uint)(tex6Cnt)) << 14) |
                    (((uint)(tex6Fmt)) << 15) |
                    (((uint)(tex6Shft)) << 18) |
                    (((uint)(tex7Cnt)) << 23) |
                    (((uint)(tex7Fmt)) << 24) |
                    (((uint)(tex7Shft)) << 27));
        }
        #endregion

        #region Rendering

        public static Color DefaultVertColor = Color.FromArgb(0, 128, 0);
        public static Color DefaultNodeColor = Color.FromArgb(100, 0, 255);

        internal Color _vertColor = Color.Transparent;
        internal Color _nodeColor = Color.Transparent;

        public const float _nodeRadius = 0.05f;
        const float _nodeAdj = 0.01f;

        public bool _render = true;
        internal unsafe void RenderVerts(GLContext ctx, IMatrixNode _singleBind)
        {
            if (!_render)
                return;

            //GLDisplayList vertex = new GLDisplayList(ctx);
            //ctx.glEnable(GLEnableCap.POINT_SMOOTH);
            //vertex.Begin();
            //ctx.glColor(0.0f, 1.0f, 0.0f);
            //ctx.glPointSize(10);
            //vertex.End();

            foreach (Vertex3 v in _vertices)
            {
                if (_vertColor != Color.Transparent)
                    ctx.glColor(_vertColor.R, _vertColor.G, _vertColor.B, _vertColor.A);
                else
                    ctx.glColor(DefaultVertColor.R, DefaultVertColor.G, DefaultVertColor.B, DefaultVertColor.A);

                if (_singleBind != null)
                {
                    ctx.glPushMatrix();

                    Matrix m = _singleBind.Matrix;
                    ctx.glMultMatrix((float*)&m);

                    ctx.glTranslate(v.WeightedPosition._x, v.WeightedPosition._y, v.WeightedPosition._z);
                }
                else
                {
                    //ctx.glBegin(GLPrimitiveType.Points);
                    //ctx.glVertex(v.WeightedPosition._x, v.WeightedPosition._y, v.WeightedPosition._z);
                    //ctx.glEnd();

                    ctx.glPushMatrix();

                    ctx.glTranslate(v.WeightedPosition._x, v.WeightedPosition._y, v.WeightedPosition._z);
                }

                if (_nodeColor != Color.Transparent)
                    ctx.glColor(_nodeColor.R, _nodeColor.G, _nodeColor.B, _nodeColor.A);
                else
                    ctx.glColor(DefaultNodeColor.R, DefaultNodeColor.G, DefaultNodeColor.B, DefaultNodeColor.A);

                DrawNodeOrients(ctx);

                ctx.glPopMatrix();
            }
        }

        private static void DrawNodeOrients(GLContext ctx)
        {
            ctx.glBegin(GLPrimitiveType.Lines);

            ctx.glColor(1.0f, 0.0f, 0.0f, 1.0f);
            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(_nodeRadius * 2, 0.0f, 0.0f);

            ctx.glColor(0.0f, 1.0f, 0.0f, 1.0f);
            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(0.0f, _nodeRadius * 2, 0.0f);

            ctx.glColor(0.0f, 0.0f, 1.0f, 1.0f);
            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(0.0f, 0.0f, _nodeRadius * 2);

            ctx.glEnd();
        }
        #endregion

        internal unsafe PrimitiveManager Clone()
        {
            return this.MemberwiseClone() as PrimitiveManager;
        }
    }
}
