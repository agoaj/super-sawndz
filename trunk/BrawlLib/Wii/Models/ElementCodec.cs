using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Graphics;
using System.Runtime.InteropServices;
using BrawlLib.Modeling;
using System.Windows.Forms;
using BrawlLib.Imaging;

namespace BrawlLib.Wii.Models
{
    public unsafe delegate void ElementDecoder(ref byte* pIn, ref byte* pOut, float scale);
    public unsafe class ElementCodec
    {
        [Flags]
        public enum CodecType : int
        {
            S = 0,
            ST = 5,
            XY = 10,
            XYZ = 15
        }

        #region Decoders

        public static ElementDecoder[] Decoders = new ElementDecoder[] 
        {
            //Element_Input_Output
            Element_Byte_Float2, //S
            Element_SByte_Float2,
            Element_wUShort_Float2,
            Element_wShort_Float2,
            Element_wFloat_Float2,
            Element_Byte2_Float2, //ST
            Element_SByte2_Float2,
            Element_wUShort2_Float2,
            Element_wShort2_Float2,
            Element_wFloat2_Float2,
            Element_Byte2_Float3, //XY
            Element_SByte2_Float3,
            Element_wUShort2_Float3,
            Element_wShort2_Float3,
            Element_wFloat2_Float3,
            Element_Byte3_Float3, //XYZ
            Element_SByte3_Float3,
            Element_wUShort3_Float3,
            Element_wShort3_Float3,
            Element_wFloat3_Float3
        };

        public static void Element_Byte_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(*pIn++) * scale;
            ((float*)pOut)[1] = 0.0f;
            pOut += 8;
        }
        public static void Element_SByte_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(*(sbyte*)pIn++) * scale;
            ((float*)pOut)[1] = 0.0f;
            pOut += 8;
        }
        public static void Element_wUShort_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(ushort)((*pIn++ << 8) | *pIn++) * scale;
            ((float*)pOut)[1] = 0.0f;
            pOut += 8;
        }
        public static void Element_wShort_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(short)((*pIn++ << 8) | *pIn++) * scale;
            ((float*)pOut)[1] = 0.0f;
            pOut += 8;
        }
        public static void Element_wFloat_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            float val;
            byte* p = (byte*)&val;
            p[3] = *pIn++;
            p[2] = *pIn++;
            p[1] = *pIn++;
            p[0] = *pIn++;

            ((float*)pOut)[0] = val * scale;
            ((float*)pOut)[1] = 0.0f;
            pOut += 8;
        }

        public static void Element_Byte2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(*pIn++) * scale;
            ((float*)pOut)[1] = (float)(*pIn++) * scale;
            pOut += 8;
        }
        public static void Element_SByte2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(*(sbyte*)pIn++) * scale;
            ((float*)pOut)[1] = (float)(*(sbyte*)pIn++) * scale;
            pOut += 8;
        }
        public static void Element_wUShort2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(ushort)((*pIn++ << 8) | *pIn++) * scale;
            ((float*)pOut)[1] = (float)(ushort)((*pIn++ << 8) | *pIn++) * scale;
            pOut += 8;
        }
        public static void Element_wShort2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(short)((*pIn++ << 8) | *pIn++) * scale;
            ((float*)pOut)[1] = (float)(short)((*pIn++ << 8) | *pIn++) * scale;
            pOut += 8;
        }
        public static void Element_wFloat2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            float val;
            byte* p = (byte*)&val;

            for (int i = 0; i < 2; i++)
            {
                p[3] = *pIn++;
                p[2] = *pIn++;
                p[1] = *pIn++;
                p[0] = *pIn++;
                ((float*)pOut)[i] = val * scale;
            }
            pOut += 8;
        }

        public static void Element_wShort2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            float* f = (float*)pOut;

            *f++ = (float)(short)((*pIn++ << 8) | *pIn++) * scale;
            *f++ = (float)(short)((*pIn++ << 8) | *pIn++) * scale;
            *f = 0.0f;

            pOut += 12;
        }

        public static void Element_wShort3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            short temp;
            byte* p = (byte*)&temp;
            for (int i = 0; i < 3; i++)
            {
                p[1] = *pIn++;
                p[0] = *pIn++;
                *(float*)pOut = (float)temp * scale;
                pOut += 4;
            }
        }
        public static void Element_wUShort2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            ushort temp;
            byte* p = (byte*)&temp;
            for (int i = 0; i < 3; i++)
            {
                if (i == 2)
                    *(float*)pOut = 0.0f;
                else
                {
                    p[1] = *pIn++;
                    p[0] = *pIn++;
                    *(float*)pOut = (float)temp * scale;
                }
                pOut += 4;
            }
        }
        public static void Element_wUShort3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            ushort temp;
            byte* p = (byte*)&temp;
            for (int i = 0; i < 3; i++)
            {
                p[1] = *pIn++;
                p[0] = *pIn++;
                *(float*)pOut = (float)temp * scale;
                pOut += 4;
            }
        }
        public static void Element_Byte2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            for (int i = 0; i < 3; i++)
            {
                *(float*)pOut = (i == 2) ? 0.0f : (float)(*pIn++) * scale;
                pOut += 4;
            }
        }
        public static void Element_Byte3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            for (int i = 0; i < 3; i++)
            {
                *(float*)pOut = (float)(*pIn++) * scale;
                pOut += 4;
            }
        }
        public static void Element_SByte2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            for (int i = 0; i < 3; i++)
            {
                *(float*)pOut = (i == 2) ? 0.0f : (float)(*(sbyte*)pIn++) * scale;
                pOut += 4;
            }
        }
        public static void Element_SByte3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            for (int i = 0; i < 3; i++)
            {
                *(float*)pOut = (float)(*(sbyte*)pIn++) * scale;
                pOut += 4;
            }
        }
        public static void Element_wFloat2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            float temp;
            byte* p = (byte*)&temp;
            for (int i = 0; i < 3; i++)
            {
                if (i == 2)
                    *(float*)pOut = 0.0f;
                else
                {
                    p[3] = *pIn++;
                    p[2] = *pIn++;
                    p[1] = *pIn++;
                    p[0] = *pIn++;
                    *(float*)pOut = temp;
                }
                pOut += 4;
            }
        }
        public static void Element_wFloat3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            float val;
            byte* p = (byte*)&val;
            for (int i = 0; i < 3; i++)
            {
                p[3] = *pIn++;
                p[2] = *pIn++;
                p[1] = *pIn++;
                p[0] = *pIn++;
                *(float*)pOut = val;
                pOut += 4;
            }
        }

        #endregion

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ElementDescriptor
    {
        public int Stride;
        public bool Weighted;

        public fixed byte Commands[31];
        public fixed int Defs[12];

        public UnsafeBuffer RemapTable;
        public int RemapSize;

        private fixed ushort Nodes[16];

        public ElementDescriptor(MDL0Polygon* polygon)
        {
            byte* pData = (byte*)polygon->DefList;
            byte* pCom;
            ElementDef* pDef;
            
            CPElementSpec UVATGroups;
            int format; //1 for direct, 2 for byte, 3 for short

            //Create remap table for vertex weights
            RemapTable = new UnsafeBuffer(polygon->_numVertices * 4);
            RemapSize = 0;
            Stride = 0;
            
            NodeIds = new List<ushort>();
            Addresses = new List<uint>();

            //Read element descriptor from polygon display list
            MDL0PolygonDefs* Definitons = (MDL0PolygonDefs*)polygon->DefList;

            int fmtLo = (int)Definitons->VtxFmtLo;
            int fmtHi = (int)Definitons->VtxFmtHi;

            UVATGroups = new CPElementSpec(
                (uint)Definitons->UVATA,
                (uint)Definitons->UVATB,
                (uint)Definitons->UVATC);

            //Build extract script.
            //What we're doing is assigning extract commands for elements in the polygon, in true order.
            //This allows us to process the polygon blindly, assuming that the definition is accurate.
            //Theoretically, this should offer a significant speed bonus.
            fixed (int* pDefData = Defs)
            fixed (byte* pComData = Commands)
            {
                pCom = pComData;
                pDef = (ElementDef*)pDefData;

                //Pos/Norm weight
                if (Weighted = (fmtLo & 1) != 0)
                {
                    //Set the first command as the weight
                    *pCom++ = (byte)DecodeOp.PosWeight;
                    Stride++; //Increment stride by a byte (the length of the facepoints)
                }

                //Tex matrix
                for (int i = 0; i < 8; i++)
                    if (((fmtLo >> (i + 1)) & 1) != 0)
                    {
                        //Set the command for each texture matrix
                        *pCom++ = (byte)(DecodeOp.TexMtx0 + i);
                        Stride++; //Increment stride by a byte (the length of the facepoints)
                    }

                //Positions
                format = ((fmtLo >> 9) & 3) - 1;
                if (format >= 0)
                {
                    //Set the definitions input
                    pDef->Format = (byte)format;
                    //Set the type to Positions
                    pDef->Type = 0;
                    if (format == 0)
                    {
                        pDef->Scale = (byte)UVATGroups.PositionDef.Scale;
                        //pDef->Output = 
                        *pCom++ = (byte)DecodeOp.ElementDirect;
                    }
                    else
                    {
                        Stride += format; //Add to stride (the length of the facepoints)
                        pDef->Output = 12; //Set the output
                        *pCom++ = (byte)DecodeOp.ElementIndexed;
                    }
                    pDef++;
                }

                //Normals
                format = ((fmtLo >> 11) & 3) - 1;
                if (format >= 0)
                {
                    //Set the definitions input
                    pDef->Format = (byte)format;
                    //Set the type to Normals
                    pDef->Type = 1;
                    if (format == 0)
                    {
                        pDef->Scale = (byte)UVATGroups.NormalDef.Scale;
                        //pDef->Output = 
                        *pCom++ = (byte)DecodeOp.ElementDirect;
                    }
                    else
                    {
                        Stride += format; //Add to stride (the length of the facepoints)
                        pDef->Output = 12; //Set the output
                        *pCom++ = (byte)DecodeOp.ElementIndexed;
                    }
                    pDef++;
                }

                //Colors
                for (int i = 0; i < 2; i++)
                {
                    format = ((fmtLo >> (i * 2 + 13)) & 3) - 1;
                    if (format >= 0) 
                    {
                        //Set the definitions input
                        pDef->Format = (byte)format;
                        //Set the type to Colors
                        pDef->Type = (byte)(i + 2);
                        if (format == 0)
                        {
                            //pDef->Output = 
                            pDef->Scale = 0;
                            *pCom++ = (byte)DecodeOp.ElementDirect;
                        }
                        else
                        {
                            Stride += format; //Add to stride (the length of the facepoints)
                            pDef->Output = 4; //Set the output
                            *pCom++ = (byte)DecodeOp.ElementIndexed;
                        }
                        pDef++;
                    }
                }

                //UVs
                for (int i = 0; i < 8; i++)
                {
                    format = ((fmtHi >> (i * 2)) & 3) - 1;
                    if (format >= 0)
                    {
                        //Set the definitions input
                        pDef->Format = (byte)format;
                        //Set the type to UVs
                        pDef->Type = (byte)(i + 4);
                        if (format == 0)
                        {
                            //pDef->Output = 
                            pDef->Scale = (byte)UVATGroups.GetUVDef(i).Scale;
                            *pCom++ = (byte)DecodeOp.ElementDirect;
                        }
                        else
                        {
                            Stride += format; //Add to stride (the length of the facepoints)
                            pDef->Output = 8; //Set the output
                            *pCom++ = (byte)DecodeOp.ElementIndexed;
                        }
                        pDef++;
                    }
                }
                *pCom = 0;
            }
        }

        public List<ushort> NodeIds;
        public List<uint> Addresses;
        
        //Set node ID/Index using specified command block
        public void SetNode(ref byte* pIn, byte* start)
        {
            //Get node ID
            ushort node = *(bushort*)pIn;

            NodeIds.Add(node);
            Addresses.Add((uint)pIn - (uint)start);

            //Get cache index.
            //Wii memory assigns data using offsets of 4-byte values.
            //In this case, each matrix takes up 12 floats (4 bytes each)

            //Divide by 12, the number of float values per 4x3 matrix, to get the actual index
            int index = (*(bushort*)(pIn + 2) & 0xFFF) / 12;
            //Assign node ID to cache, using index
            fixed (ushort* n = Nodes)
                n[index] = node;

            //Increment pointer
            pIn += 4;
        }

        public void AddAddr(ref byte* pIn, byte* start)
        {
            ushort node = *(bushort*)pIn;

            NodeIds.Add(node);
            Addresses.Add((uint)pIn - (uint)start);

            //Increment pointer
            pIn += 4;
        }

        //Decode a single primitive using command list
        public void Run(ref byte* pIn, byte** pAssets, byte** pOut, int count, PrimitiveGroup group, ref ushort* indices, IMatrixNode[] nodeTable)
        {
            int weight = 0;

            //Vector3 Position = new Vector3();
            //Vector3 Normal = new Vector3();
            //Vector2[] UV = new Vector2[8];
            //RGBAPixel[] Color = new RGBAPixel[2];

            int index = 0, outSize;
            DecodeOp o;
            ElementDef* pDef;
            byte* p;
            //byte[] pTexMtx = new byte[8];

            byte* tIn, tOut;

            group._points.Add(new List<Facepoint>());

            //Iterate commands in list
            fixed(ushort* pNode = Nodes)
            fixed (int* pDefData = Defs)
            fixed (byte* pCmd = Commands)
            {
                for (int i = 0; i < count; i++)
                {
                    pDef = (ElementDef*)pDefData;
                    p = pCmd;

                    Facepoint f = new Facepoint();

                Top:
                    o = (DecodeOp)(*p++);
                    switch (o)
                    {
                        //Process weight using cache
                        case DecodeOp.PosWeight:
                            weight = pNode[*pIn++ / 3];
                            if (weight < nodeTable.Length)
                                f.Node = nodeTable[weight];
                            goto Top;

                        case DecodeOp.TexMtx0:
                        case DecodeOp.TexMtx1:
                        case DecodeOp.TexMtx2:
                        case DecodeOp.TexMtx3:
                        case DecodeOp.TexMtx4:
                        case DecodeOp.TexMtx5:
                        case DecodeOp.TexMtx6:
                        case DecodeOp.TexMtx7:
                            //index = (int)o - (int)DecodeOp.TexMtx0;
                            if (*pIn++ == 0x60) //Identity matrix...
                                Console.WriteLine("wtf");
                            //pTexMtx[index] = (byte)(*pIn++ / 3);
                            goto Top;

                        case DecodeOp.ElementDirect:
                            ElementCodec.Decoders[pDef->Output]
                                (ref pIn, ref pOut[pDef->Type], VQuant.DeQuantTable[pDef->Scale]);
                            goto Top;

                        case DecodeOp.ElementIndexed:

                            //Get asset index
                            if (pDef->Format == 2)
                            { index = *(bushort*)pIn; pIn += 2; }
                            else index = *pIn++;

                            switch (pDef->Type)
                            {
                                case 0:
                                    f.VertexIndex = index;
                                    break;
                                case 1:
                                    f.NormalIndex = index;
                                    break;
                                case 2:
                                case 3:
                                    f.ColorIndex[pDef->Type - 2] = index;
                                    break;
                                default:
                                    f.UVIndex[pDef->Type - 4] = index;
                                    break;
                            }

                            if (pDef->Type == 0) //Special processing for vertices
                            {
                                //Match weight and index with remap table
                                int mapEntry = (weight << 16) | index;
                                int* pTmp = (int*)RemapTable.Address;

                                //Find matching index, starting at end of list
                                //Do nothing while the index lowers
                                index = RemapSize;
                                while ((--index >= 0) && (pTmp[index] != mapEntry)) ;

                                //No match, create new entry
                                //Will be processed into vertices at the end!
                                if (index < 0) pTmp[index = RemapSize++] = mapEntry;

                                //Write index
                                //*(ushort*)pOut[pDef->Type] = (ushort)index;
                                //pOut[pDef->Type] += 2;
                                *indices++ = (ushort)index;
                            }
                            else
                            {
                                //Copy data from buffer
                                outSize = pDef->Output;

                                //Input data from asset cache
                                tIn = pAssets[pDef->Type] + (index * outSize);
                                tOut = pOut[pDef->Type];

                                //Copy data to output
                                while (outSize-- > 0)
                                    *tOut++ = *tIn++;

                                //Increment element output pointer
                                pOut[pDef->Type] = tOut;
                            }

                            pDef++;
                            goto Top;

                        //Can't use this until it works faster. Merges all data into vertices.
                        //    if (pDef->Type == 0)
                        //    {
                        //        tOut = (byte*)&Position;
                        //        goto Apply;
                        //    }
                        //    else if (pDef->Type == 1)
                        //    {
                        //        tOut = (byte*)&Normal;
                        //        goto Apply;
                        //    }
                        //    else if (pDef->Type < 4)
                        //    {
                        //        fixed (RGBAPixel* color = &Color[pDef->Type - 2])
                        //        tOut = (byte*)color;
                        //        goto Apply;
                        //    }
                        //    else
                        //    {
                        //        fixed (Vector2* uv = &UV[pDef->Type - 4])
                        //        tOut = (byte*)uv;
                        //        goto Apply;
                        //    }

                        //Apply:
                        //    //Copy data from buffer
                        //    outSize = pDef->Output;

                        //    //Input data from asset cache
                        //    tIn = pAssets[pDef->Type] + (index * outSize);

                        //    //Copy data to output
                        //    while (outSize-- > 0)
                        //        *tOut++ = *tIn++;

                        //    pDef++;
                        //    goto Top;

                        default: break; //End
                    }
                    ////Remap as whole vertex

                    ////Match vertex with remap table
                    //VertData mapEntry = new VertData(Position, weight, Normal, Color, UV);
                    //VertData* pTmp = (VertData*)RemapTable.Address;

                    ////Find matching index, starting at end of list
                    //index = RemapSize;
                    //while ((--index >= 0) && (pTmp[index] != mapEntry));

                    ////No match, create new entry
                    //if (index < 0)
                    //    pTmp[index = RemapSize++] = mapEntry;

                    ////Write index
                    //*pOut++ = (ushort)index;

                    group._points[group._points.Count - 1].Add(f);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ElementDef
        {
            public byte Format; //Input format
            public byte Output; //Output size/decoder
            public byte Type;
            public byte Scale;
        }

        public enum DecodeOp : int
        {
            End = 0,
            PosWeight,
            TexMtx0,
            TexMtx1,
            TexMtx2,
            TexMtx3,
            TexMtx4,
            TexMtx5,
            TexMtx6,
            TexMtx7,
            ElementDirect, 
            ElementIndexed
        }

        internal unsafe List<Vertex3> Finish(Vector3* pVert, IMatrixNode[] nodeTable)
        {
            //Create vertex list from remap table
            List<Vertex3> list = new List<Vertex3>(RemapSize);

            if (Weighted)
            {
                if (nodeTable != null)
                {
                    ushort* pMap = (ushort*)RemapTable.Address;
                    for (int i = 0; i < RemapSize; i++)
                    {
                        //Create new vertex, assigning the value + influence from the remap table
                        Vertex3 v = new Vertex3(pVert[*pMap++], *pMap < nodeTable.Length ? nodeTable[*pMap] : null); pMap++;
                        //Add vertex to list
                        list.Add(v);
                    }
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
            //RemapTable.Dispose();
            //RemapTable = null;

            return list;
        }

        //internal unsafe List<Vertex3> Finish(IMatrixNode[] nodeTable)
        //{
        //    //Create vertex list from remap table
        //    List<Vertex3> list = new List<Vertex3>();

        //    VertData* pMap = (VertData*)RemapTable.Address;
        //    for (int i = 0; i < RemapSize; i++, pMap++)
        //    {
        //        list.Add(new Vertex3(
        //            pMap->_position,
        //            Weighted ? nodeTable[pMap->_weight] : null,
        //            pMap->_normal,
        //            new RGBAPixel[] { pMap->_color1, pMap->_color2 },
        //            new Vector2[] { pMap->_uv1, pMap->_uv2, pMap->_uv3, pMap->_uv4, pMap->_uv5, pMap->_uv6, pMap->_uv7, pMap->_uv8 })
        //        );
        //    }

        //    //Clean up
        //    RemapTable.Dispose();
        //    RemapTable = null;

        //    return list;
        //}
    }
}
