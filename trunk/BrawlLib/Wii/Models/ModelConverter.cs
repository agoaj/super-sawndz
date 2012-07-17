using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Textures;
using BrawlLib.Imaging;
using System.Runtime.InteropServices;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Graphics;

namespace BrawlLib.Wii.Models
{
    public static unsafe class ModelConverter
    {
    //    public static GLPolygon ExtractPolygon(GLModel model, MDL0PolygonNode polygon)
    //    {
    //        return new GLPolygon(model, polygon);
    //    }

        private static float ReadValue(ref VoidPtr addr, WiiVertexComponentType type, float divisor)
        {
            switch (type)
            {
                case WiiVertexComponentType.UInt8: addr += 1; return ((byte*)addr)[-1] / divisor;
                case WiiVertexComponentType.Int8: addr += 1; return ((sbyte*)addr)[-1] / divisor;
                case WiiVertexComponentType.UInt16: addr += 2; return ((bushort*)addr)[-1] / divisor;
                case WiiVertexComponentType.Int16: addr += 2; return ((bshort*)addr)[-1] / divisor;
                case WiiVertexComponentType.Float: addr += 4; return ((bfloat*)addr)[-1];
            }
            return 0.0f;
        }

        private delegate ushort IndexParser(VoidPtr addr);
        private static IndexParser ByteParser = x => *(byte*)x;
        private static IndexParser UShortParser = x => *(bushort*)x;
        //public static GLPrimitive ExtractPrimitive(ref VoidPtr address, EntrySize entryInfo, GLPolygon parent, ushort[] nodeBuffer, ref int nodeIndex)
        //{
        //Top:
        //    PrimitiveHeader* header = (PrimitiveHeader*)address;
        //    GLPrimitiveType type;
        //    switch (header->Type)
        //    {
        //        case WiiPrimitiveType.PosMtx: //0x20
        //            {
        //                if (*(bushort*)header->Data == 0xB000)
        //                    nodeIndex = 0;
        //                nodeBuffer[nodeIndex++] = header->Entries;

        //                //nodeBuffer[(*(bushort*)header->Data - 0xB000) / 0x0C] = header->Entries;
        //                address += 5;
        //                goto Top;
        //            }
        //        case WiiPrimitiveType.NorMtx: //0x28
        //        case WiiPrimitiveType.TexMtx: //0x30
        //        case WiiPrimitiveType.LightMtx: //0x38
        //            { address += 5; goto Top; }
        //        case WiiPrimitiveType.Lines: { type = GLPrimitiveType.Lines; break; } //0xA8
        //        case WiiPrimitiveType.LineStrip: { type = GLPrimitiveType.LineStrip; break; } //0xB0
        //        case WiiPrimitiveType.Points: { type = GLPrimitiveType.Points; break; } //0xB8
        //        case WiiPrimitiveType.Quads: { type = GLPrimitiveType.Quads; break; } //0x80
        //        case WiiPrimitiveType.TriangleFan: { type = GLPrimitiveType.TriangleFan; break; } //0xA0
        //        case WiiPrimitiveType.Triangles: { type = GLPrimitiveType.Triangles; break; } //0x90
        //        case WiiPrimitiveType.TriangleStrip: { type = GLPrimitiveType.TriangleStrip; break; } //0x98
        //        default:
        //            return null;
        //    }

        //    GLPrimitive primitive = new GLPrimitive();
        //    primitive._type = type;
        //    primitive._elements = header->Entries;
        //    primitive._parent = parent;

        //    int entries = primitive._elements;
        //    int stride = entryInfo._totalLen;
        //    VoidPtr data = header->Data;

        //    //Weight indices
        //    primitive._nodeIndices = ParseWeights(data, entries, entryInfo._extraLen, stride, nodeBuffer);
        //    data += entryInfo._extraLen;

        //    //Vertex Data
        //    primitive._vertexIndices = ParseIndices(data, entries, entryInfo._vertexLen, stride);
        //    data += entryInfo._vertexLen;

        //    //Normal Data
        //    primitive._normalIndices = ParseIndices(data, entries, entryInfo._normalLen, stride);
        //    data += entryInfo._normalLen;

        //    //Color Data
        //    for (int i = 0; i < 2; data += entryInfo._colorLen[i++])
        //        primitive._colorIndices[i] = ParseIndices(data, entries, entryInfo._colorLen[i++], stride);

        //    //UV Data
        //    for (int i = 0; i < 8; data += entryInfo._uvLen[i++])
        //        primitive._uvIndices[i] = ParseIndices(data, entries, entryInfo._uvLen[i], stride);

        //    address += (entryInfo._totalLen * entries) + 3;

        //    return primitive;
        //}

        private static Primitive ExtractPrimitive(ref VoidPtr address, EntrySize entryInfo, ushort[] nodeBuffer, ref int nodeIndex)
        {
        Top:
            PrimitiveHeader* header = (PrimitiveHeader*)address;
            GLPrimitiveType type;
            switch (header->Type)
            {
                case WiiPrimitiveType.PosMtx: //0x20
                    {
                        if (*(bushort*)header->Data == 0xB000)
                            nodeIndex = 0;
                        nodeBuffer[nodeIndex++] = header->Entries;

                        //nodeBuffer[(*(bushort*)header->Data - 0xB000) / 0x0C] = header->Entries;
                        address += 5;
                        goto Top;
                    }
                case WiiPrimitiveType.NorMtx: //0x28
                case WiiPrimitiveType.TexMtx: //0x30
                case WiiPrimitiveType.LightMtx: //0x38
                    { address += 5; goto Top; }
                case WiiPrimitiveType.Lines: { type = GLPrimitiveType.Lines; break; } //0xA8
                case WiiPrimitiveType.LineStrip: { type = GLPrimitiveType.LineStrip; break; } //0xB0
                case WiiPrimitiveType.Points: { type = GLPrimitiveType.Points; break; } //0xB8
                case WiiPrimitiveType.Quads: { type = GLPrimitiveType.Quads; break; } //0x80
                case WiiPrimitiveType.TriangleFan: { type = GLPrimitiveType.TriangleFan; break; } //0xA0
                case WiiPrimitiveType.Triangles: { type = GLPrimitiveType.Triangles; break; } //0x90
                case WiiPrimitiveType.TriangleStrip: { type = GLPrimitiveType.TriangleStrip; break; } //0x98
                default:
                    return null;
            }

            Primitive primitive = new Primitive();
            primitive._type = type;

            int entries = primitive._elementCount = header->Entries;
            int stride = entryInfo._totalLen;
            VoidPtr data = header->Data;

            //Weight indices
            primitive._weightIndices = ParseWeights(data, entries, entryInfo._extraLen, stride, nodeBuffer);
            data += entryInfo._extraLen;

            //Vertex Data
            primitive._vertexIndices = ParseIndices(data, entries, entryInfo._vertexLen, stride);
            data += entryInfo._vertexLen;

            //Normal Data
            primitive._normalIndices = ParseIndices(data, entries, entryInfo._normalLen, stride);
            data += entryInfo._normalLen;

            //Color Data
            for (int i = 0; i < 2; data += entryInfo._colorLen[i++])
                primitive._colorIndices[i] = ParseIndices(data, entries, entryInfo._colorLen[i], stride);

            //UV Data
            for (int i = 0; i < 8; data += entryInfo._uvLen[i++])
                primitive._uvIndices[i] = ParseIndices(data, entries, entryInfo._uvLen[i], stride);

            address += (entryInfo._totalLen * entries) + 3;

            return primitive;
        }

        private static ushort[] ParseWeights(VoidPtr address, int elementCount, int elementLen, int stride, ushort[] nodeBuffer)
        {
            if (elementLen == 0)
                return null;

            ushort[] indices = new ushort[elementCount];
            byte* ptr = (byte*)address;

            for (int i = 0; i < elementCount; ptr += stride )
                indices[i++] = nodeBuffer[*ptr / 3];

            return indices;
        }

        private static ushort[] ParseIndices(VoidPtr address, int elementCount, int elementLen, int stride)
        {
            if (elementLen == 0)
                return null;

            ushort[] indices = new ushort[elementCount];
            byte* ptr = (byte*)address;

            if (elementLen == 1)
                for (int i = 0; i < elementCount; ptr += stride)
                    indices[i++] = *ptr;
            else
                for (int i = 0; i < elementCount; ptr += stride)
                    indices[i++] = *(bushort*)ptr;

            return indices;
        }

        public static List<Primitive> ExtractPrimitives(MDL0Polygon* polygon)
        {
            List<Primitive> list = new List<Primitive>();

            VoidPtr dataAddr = polygon->PrimitiveData;
            //ElementFlags e = new ElementFlags(polygon->_elemFlags, polygon->_texFlags);
            EntrySize e = new EntrySize(polygon->_vertexFormat);

            int nodeIndex = 0;
            ushort[] nodeBuffer = new ushort[16];
            Primitive p;

            while ((p = ExtractPrimitive(ref dataAddr, e, nodeBuffer, ref nodeIndex)) != null)
                list.Add(p);

            return list;
        }

        private static ushort[] ParseWeights(byte* pData, int elementCount, int stride, ushort[] nodeBuffer)
        {
            ushort[] indices = new ushort[elementCount];

            for (int i = 0; i < elementCount; pData += stride )
                indices[i++] = nodeBuffer[*pData / 3];

            return indices;
        }

        private static ushort[] ParseElement(ref byte* pData, XFDataFormat fmt, int count, int stride)
        {
            if (fmt == XFDataFormat.None)
                return null;

            ushort[] indices = new ushort[count];

            byte* tPtr = pData;
            if (fmt == XFDataFormat.Index8)
            {
                for (int i = 0; i < count; tPtr += stride)
                    indices[i++] = *tPtr;
                pData++;
            }
            else if (fmt == XFDataFormat.Index16)
            {
                for (int i = 0; i < count; tPtr += stride)
                    indices[i++] = *(bushort*)tPtr;
                pData += 2;
            }

            return indices;
        }

        private static ushort[] Parse8(byte* pData, int elements, int stride)
        {
            ushort[] indices = new ushort[elements];

            for (int i = 0; i < elements; pData += stride)
                indices[i++] = *pData;

            return indices;
        }
    }
}
