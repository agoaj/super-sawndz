using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BrawlLib.Wii.Models;
using BrawlLib.Imaging;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.OpenGL;

namespace BrawlLib.Modeling
{
    public unsafe partial class Collada
    {
        static PrimitiveManager DecodePrimitivesWeighted(GeometryEntry geo, SkinEntry skin, SceneEntry scene, InfluenceManager infManager, ref string Error)
        {
            PrimitiveManager manager = DecodePrimitives(geo);

            MDL0BoneNode[] boneList;
            MDL0BoneNode bone = null;
            int boneCount;
            string[] jointStrings = null;
            byte* pCmd = stackalloc byte[4];
            int cmdCount = skin._weightInputs.Count;
            float weight = 0;
            float* pWeights = null;
            Vector3* pVert = null, pNorms = null;
            ushort* pVInd = (ushort*)manager._indices.Address;
            List<Vertex3> vertList = new List<Vertex3>(skin._weightCount);
            Matrix* pMatrix = null;

            UnsafeBuffer remap = new UnsafeBuffer(skin._weightCount * 2);
            ushort* pRemap = (ushort*)remap.Address;

            pNorms = (Vector3*)manager._faceData[1].Address;
            //List<int> FixedIndices = new List<int>();

            manager._vertices = vertList;

            //Find vertex source
            foreach (SourceEntry s in geo._sources)
                if (s._id == geo._verticesInput._source)
                {
                    pVert = (Vector3*)((UnsafeBuffer)s._arrayData).Address;
                    break;
                }

            //Find joint source
            foreach (InputEntry inp in skin._jointInputs)
                if (inp._semantic == SemanticType.JOINT)
                {
                    foreach (SourceEntry src in skin._sources)
                        if (src._id == inp._source)
                        {
                            jointStrings = src._arrayData as string[];
                            break;
                        }
                }
                else if (inp._semantic == SemanticType.INV_BIND_MATRIX)
                {
                    foreach (SourceEntry src in skin._sources)
                        if (src._id == inp._source)
                        {
                            pMatrix = (Matrix*)((UnsafeBuffer)src._arrayData).Address;
                            break;
                        }
                }

            Error = "There was a problem creating the list of bones for geometry entry " + geo._name;

            //Populate bone list
            boneCount = jointStrings.Length;
            boneList = new MDL0BoneNode[boneCount];
            for (int i = 0; i < boneCount; i++)
                boneList[i] = scene.FindNode(jointStrings[i])._node as MDL0BoneNode;

            //Build command list
            foreach (InputEntry inp in skin._weightInputs)
            {
                switch (inp._semantic)
                {
                    case SemanticType.JOINT:
                        pCmd[inp._offset] = 1;
                        break;

                    case SemanticType.WEIGHT:
                        pCmd[inp._offset] = 2;

                        //Get weight source
                        foreach (SourceEntry src in skin._sources)
                            if (src._id == inp._source)
                            {
                                pWeights = (float*)((UnsafeBuffer)src._arrayData).Address;
                                break;
                            }

                        break;

                    default:
                        pCmd[inp._offset] = 0;
                        break;
                }
            }

            Error = "There was a problem creating vertex influences for geometry entry " + geo._name;

            //Build vertex list and remap table
            for (int i = 0; i < skin._weightCount; i++)
            {
                //Create influence
                int iCount = skin._weights[i].Length / cmdCount;
                Influence inf = new Influence(iCount);
                fixed (int* p = skin._weights[i])
                {
                    int* iPtr = p;
                    for (int x = 0; x < iCount; x++)
                    {
                        for (int z = 0; z < cmdCount; z++, iPtr++)
                            if (pCmd[z] == 1)
                                bone = boneList[*iPtr];
                            else if (pCmd[z] == 2)
                                weight = pWeights[*iPtr];
                        //if (bone != null)
                        //    if (bone.Name == "TopN" || bone.Name == "XRotN" || bone.Name == "YRotN" || bone.Name == "TransN" || bone.Name == "ThrowN" || bone.Name == "FacePattern")
                        //        Console.WriteLine(bone.Name);
                        //    else if (bone.Parent != null)
                        //        if (bone.Parent.Name == "FacePattern")
                        //            Console.WriteLine(bone.Name);
                        inf._weights[x] = new BoneWeight(bone, weight);
                    }
                }

                inf.CalcMatrix();

                Error = "There was a problem creating a vertex from the geometry entry " + geo._name + ".\nMake sure that all the vertices are weighted properly.";

                Vertex3 v;
                if (inf._weights.Length > 1)
                {
                    //Match with manager
                    inf = infManager.AddOrCreate(inf);
                    v = new Vertex3(skin._bindMatrix * pVert[i], inf); //World position
                }
                else
                {
                    bone = inf._weights[0].Bone;
                    v = new Vertex3(bone._inverseBindMatrix * skin._bindMatrix * pVert[i], bone); //Local position
                }

                ////Create Vertex, set to world position.
                //v = new Vertex3(skin._bindMatrix * pVert[i], inf);
                ////Fix single-bind vertices
                //v.Position = inf._weights[0].Bone._inverseBindMatrix * v.Position;

                ushort index = 0;
                while (index < vertList.Count)
                {
                    if (v.Equals(vertList[index])) break;
                    index++;
                }
                if (index == vertList.Count)
                    vertList.Add(v);

                pRemap[i] = index;
            }

            Error = "There was a problem fixing normal rotations for geometry entry " + geo._name;

            //Remap vertex indices and fix normals
            for (int i = 0; i < manager._pointCount; i++, pVInd++)
            {
                *pVInd = pRemap[*pVInd];
                Vertex3 v = null;
                if (*pVInd < vertList.Count)
                    v = vertList[*pVInd];
                if (v != null && v._influence != null)
                    if (v._influence.Weights.Length > 1)
                        pNorms[i] = skin._bindMatrix.GetRotationMatrix() * pNorms[i];
                    else
                        pNorms[i] = skin._bindMatrix.GetRotationMatrix() * v._influence.Weights[0].Bone._inverseBindMatrix.GetRotationMatrix() * pNorms[i];
            }

            remap.Dispose();

            //manager.MergeTempData();
            return manager;
        }
        static PrimitiveManager DecodePrimitivesUnweighted(GeometryEntry geo)
        {
            PrimitiveManager manager = DecodePrimitives(geo);

            Vector3* pVert = null;
            ushort* pVInd = (ushort*)manager._indices.Address;
            int vCount = 0;
            List<Vertex3> vertList = new List<Vertex3>(manager._pointCount);

            manager._vertices = vertList;

            //Find vertex source
            foreach (SourceEntry s in geo._sources)
                if (s._id == geo._verticesInput._source)
                {
                    UnsafeBuffer b = s._arrayData as UnsafeBuffer;
                    pVert = (Vector3*)b.Address;
                    vCount = b.Length / 12;
                    break;
                }

            UnsafeBuffer remap = new UnsafeBuffer(vCount * 2);
            ushort* pRemap = (ushort*)remap.Address;

            //Create remap table
            for (int i = 0; i < vCount; i++)
            {
                //Create Vertex and look for match
                Vertex3 v = new Vertex3(pVert[i]);

                int index = 0;
                while (index < vertList.Count)
                {
                    if (v.Equals(vertList[index]))
                        break;
                    index++;
                }
                if (index == vertList.Count)
                    vertList.Add(v);

                pRemap[i] = (ushort)index;
            }

            //Remap vertex indices
            for (int i = 0; i < manager._pointCount; i++, pVInd++)
                *pVInd = pRemap[*pVInd];

            remap.Dispose();

            //manager.MergeTempData();
            return manager;
        }

        static PrimitiveManager DecodePrimitives(GeometryEntry geo)
        {
            ushort* pTri = null, pLin = null;
            long* pInDataList = stackalloc long[12];
            long* pOutDataList = stackalloc long[12];
            int* pData = stackalloc int[16];
            int faces = 0, lines = 0, points = 0;
            ushort fIndex = 0, lIndex = 0, temp;

            PrimitiveDecodeCommand* pCmd = (PrimitiveDecodeCommand*)pData;
            byte** pInData = (byte**)pInDataList;
            byte** pOutData = (byte**)pOutDataList;

            //_geo = geo;
            //_sources = new UnsafeBuffer[12];
            //_remapTable = new List<int>(64);
            PrimitiveManager manager = new PrimitiveManager();

            //Assign vertex source
            foreach (SourceEntry s in geo._sources)
                if (s._id == geo._verticesInput._source)
                {
                    pInData[0] = (byte*)((UnsafeBuffer)s._arrayData).Address;
                    break;
                }

            foreach (PrimitiveEntry prim in geo._primitives)
            {
                //Get face/line count
                if (prim._type == PrimitiveType.lines || prim._type == PrimitiveType.linestrips)
                    lines += prim._faceCount;
                else
                    faces += prim._faceCount;

                //Get point total
                points += prim._pointCount;

                //Signal storage buffers and set type offsets
                foreach (InputEntry inp in prim._inputs)
                {
                    int offset = -1;

                    switch (inp._semantic)
                    {
                        case SemanticType.VERTEX: offset = 0; break;
                        case SemanticType.NORMAL: offset = 1; break;
                        case SemanticType.COLOR: if (inp._set < 2) offset = 2 + inp._set; break;
                        case SemanticType.TEXCOORD: if (inp._set < 8) offset = 4 + inp._set; break;
                    }

                    if (offset != -1)
                        manager._dirty[offset] = true;

                    inp._outputOffset = offset;
                }
            }
            manager._pointCount = points;

            //Create primitives
            if (faces > 0)
            {
                manager._triangles = new NewPrimitive(faces * 3, OpenGL.GLPrimitiveType.Triangles);
                pTri = (ushort*)manager._triangles._indices.Address;
            }
            if (lines > 0)
            {
                manager._lines = new NewPrimitive(lines * 2, OpenGL.GLPrimitiveType.Lines);
                pLin = (ushort*)manager._lines._indices.Address;
            }

            manager._indices = new UnsafeBuffer(points * 2);
            //Create face buffers and assign output pointers
            for (int i = 0; i < 12; i++)
                if (manager._dirty[i])
                {
                    int stride;
                    if (i == 0) stride = 2;
                    else if (i == 1) stride = 12;
                    else if (i < 4) stride = 4;
                    else stride = 8;
                    manager._faceData[i] = new UnsafeBuffer(points * stride);
                    if (i == 0)
                        pOutData[i] = (byte*)manager._indices.Address;
                    else
                        pOutData[i] = (byte*)manager._faceData[i].Address;
                }

            //Decode primitives
            foreach (PrimitiveEntry prim in geo._primitives)
            {
                int count = prim._inputs.Count;
                //Map inputs to command sequence
                foreach (InputEntry inp in prim._inputs)
                {
                    if (inp._outputOffset == -1)
                        pCmd[inp._offset].Cmd = 0;
                    else
                    {
                        pCmd[inp._offset].Cmd = (byte)inp._semantic;
                        pCmd[inp._offset].Index = (byte)inp._outputOffset;

                        //Assign input buffer
                        foreach (SourceEntry src in geo._sources)
                            if (src._id == inp._source)
                            {
                                pInData[inp._outputOffset] = (byte*)((UnsafeBuffer)src._arrayData).Address;
                                break;
                            }
                    }
                }

                //Decode face data using command list
                foreach (PrimitiveFace f in prim._faces)
                    fixed (ushort* p = f._pointIndices)
                        RunPrimitiveCmd(pInData, pOutData, pCmd, count, p, f._pointCount);

                //Process point indices
                switch (prim._type)
                {
                    case PrimitiveType.triangles:
                        count = prim._faceCount * 3;
                        while (count-- > 0)
                            *pTri++ = fIndex++;
                        break;
                    case PrimitiveType.trifans:
                    case PrimitiveType.polygons:
                    case PrimitiveType.polylist:
                        foreach (PrimitiveFace f in prim._faces)
                        {
                            count = f._pointCount - 2;
                            temp = fIndex;
                            fIndex += 2;
                            while (count-- > 0)
                            {
                                *pTri++ = temp;
                                *pTri++ = (ushort)(fIndex - 1);
                                *pTri++ = fIndex++;
                            }
                        }
                        break;
                    case PrimitiveType.tristrips:
                        foreach (PrimitiveFace f in prim._faces)
                        {
                            count = f._pointCount;
                            fIndex += 2;
                            for (int i = 2; i < count; i++)
                            {
                                if ((i & 1) == 0)
                                {
                                    *pTri++ = (ushort)(fIndex - 2);
                                    *pTri++ = (ushort)(fIndex - 1);
                                    *pTri++ = fIndex++;
                                }
                                else
                                {
                                    *pTri++ = (ushort)(fIndex - 2);
                                    *pTri++ = fIndex;
                                    *pTri++ = (ushort)(fIndex++ - 1);
                                }
                            }
                        }
                        break;

                    case PrimitiveType.linestrips:
                        foreach (PrimitiveFace f in prim._faces)
                        {
                            count = f._pointCount - 1;
                            lIndex++;
                            while (count-- > 0)
                            {
                                *pLin++ = (ushort)(lIndex - 1);
                                *pLin++ = lIndex++;
                            }
                        }
                        break;

                    case PrimitiveType.lines:
                        foreach (PrimitiveFace f in prim._faces)
                        {
                            count = f._pointCount * 2;
                            while (count-- > 0)
                                *pLin++ = lIndex++;
                        }
                        break;
                }
            }
            return manager;
        }

        private static void RunPrimitiveCmd(byte** pIn, byte** pOut, PrimitiveDecodeCommand* pCmd, int cmdCount, ushort* pIndex, int count)
        {
            int buffer;
            while (count-- > 0)
                for (int i = 0; i < cmdCount; i++)
                {
                    buffer = pCmd[i].Index;
                    switch ((SemanticType)pCmd[i].Cmd)
                    {
                        case SemanticType.None:
                            *pIndex += 1;
                            break;

                        case SemanticType.VERTEX:
                            //Can't do remap table because weights haven't been assigned yet!
                            *(ushort*)pOut[buffer] = *pIndex++;
                            pOut[buffer] += 2;
                            break;

                        case SemanticType.NORMAL:
                            *(Vector3*)pOut[buffer] = ((Vector3*)pIn[buffer])[*pIndex++];
                            pOut[buffer] += 12;
                            break;

                        case SemanticType.COLOR:
                            float* p = (float*)(pIn[buffer] + (*pIndex++ * 16));
                            byte* p2 = pOut[buffer];
                            for (int x = 0; x < 4; x++)
                                *p2++ = (byte)(*p++ * 255.0f + 0.5f);
                            pOut[buffer] = p2;
                            break;

                        case SemanticType.TEXCOORD:
                            //Flip y axis so coordinates are bottom-up
                            Vector2 v = ((Vector2*)pIn[buffer])[*pIndex++];
                            v._y = 1.0f - v._y;
                            *(Vector2*)pOut[buffer] = v;
                            pOut[buffer] += 8;
                            break;
                    }
                }
        }

        static void NullWeight(PrimitiveManager manager, GeometryEntry geo)
        {
            Vector3* pVert = null;
            ushort* pVInd = (ushort*)manager._indices.Address;
            List<Vertex3> vertList = new List<Vertex3>(manager._pointCount);

            //Find vertex source
            foreach (SourceEntry s in geo._sources)
                if (s._id == geo._verticesInput._source)
                {
                    pVert = (Vector3*)((UnsafeBuffer)s._arrayData).Address;
                    break;
                }

            //Construct Vertex from new weight
            for (int i = 0; i < manager._pointCount; i++)
            {
                //Create Vertex and look for match
                Vertex3 v = new Vertex3(pVert[*pVInd]);
                int index = 0;
                while (index < vertList.Count)
                {
                    if (v.Equals(vertList[i]))
                        break;
                    index++;
                }
                if (index == vertList.Count)
                    vertList.Add(v);

                //Assign new index
                *pVInd++ = (ushort)index;
            }

        }
        static void Weight(PrimitiveManager manager, SkinEntry skin, DecoderShell shell, GeometryEntry geo, InfluenceManager iMan)
        {
            MDL0BoneNode[] boneList;
            MDL0BoneNode bone = null;
            int boneCount;
            string[] jointStrings = null;
            byte* pCmd = stackalloc byte[4];
            int cmdCount = skin._weightInputs.Count;
            float weight = 0;
            float* pWeights = null;
            Vector3* pVert = null;
            ushort* pVInd = (ushort*)manager._indices.Address;
            List<Vertex3> vertList = new List<Vertex3>(skin._weightCount);

            manager._vertices = vertList;

            //Find vertex source
            foreach (SourceEntry s in geo._sources)
                if (s._id == geo._verticesInput._source)
                {
                    pVert = (Vector3*)((UnsafeBuffer)s._arrayData).Address;
                    break;
                }

            //Find joint source
            foreach (InputEntry inp in skin._jointInputs)
                if (inp._semantic == SemanticType.JOINT)
                {
                    foreach (SourceEntry src in skin._sources)
                        if (src._id == inp._source)
                        {
                            jointStrings = src._arrayData as string[];
                            break;
                        }
                    break;
                }

            //Populate bone list
            boneCount = jointStrings.Length;
            boneList = new MDL0BoneNode[boneCount];
            for (int i = 0; i < boneCount; i++)
                boneList[i] = shell.FindNode(jointStrings[i])._node as MDL0BoneNode;

            //Build command list
            foreach (InputEntry inp in skin._weightInputs)
            {
                switch (inp._semantic)
                {
                    case SemanticType.JOINT:
                        pCmd[inp._offset] = 1;
                        break;

                    case SemanticType.WEIGHT:
                        pCmd[inp._offset] = 2;

                        //Get weight source
                        foreach (SourceEntry src in skin._sources)
                            if (src._id == inp._source)
                            {
                                pWeights = (float*)((UnsafeBuffer)src._arrayData).Address;
                                break;
                            }

                        break;

                    default:
                        pCmd[inp._offset] = 0;
                        break;
                }
            }

            //Construct Vertex from new weight
            for (int i = 0; i < skin._weightCount; i++)
            {
                //Create influence
                int iCount = skin._weights.Length / cmdCount;
                Influence inf = new Influence(iCount);
                fixed (int* p = skin._weights[i])
                {
                    int* iPtr = p;
                    for (int x = 0; x < iCount; x++)
                    {
                        for (int z = 0; z < cmdCount; z++, iPtr++)
                            if (pCmd[z] == 1)
                                bone = boneList[*iPtr];
                            else if (pCmd[z] == 2)
                                weight = pWeights[*iPtr];

                        inf._weights[x] = new BoneWeight(bone, weight);
                    }
                }

                //Match with manager
                inf = iMan.AddOrCreateInf(inf);

                //Create Vertex and look for match
                Vertex3 v = new Vertex3(pVert[*pVInd], inf);
                int index = 0;
                while (index < vertList.Count)
                {
                    if (v.Equals(vertList[i]))
                        break;
                    index++;
                }
                if (index == vertList.Count)
                    vertList.Add(v);

                //Assign new index
                *pVInd++ = (ushort)index;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct PrimitiveDecodeCommand
        {
            public byte Cmd;
            public byte Index;
            public byte Pad1, Pad2;
        }

    }
}
