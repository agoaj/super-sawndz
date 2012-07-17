using System;
using System.Collections.Generic;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using BrawlLib.Modeling;
using System.Collections;
using BrawlLib.Imaging;
using System.Windows.Forms;
using BrawlLib.Wii.Graphics;

namespace BrawlLib.Wii.Models
{
    public static unsafe class ModelEncoder
    {
        //This assumes influence list has already been cleaned
        public static void AssignNodeIndices(ModelLinker linker)
        {
            MDL0Node model = linker.Model;
            int index = 0;

            int count = model._influences._influences.Count + linker.BoneCache.Length;

            linker._nodeCount = count;
            linker.Model._numNodes = count;
            linker.NodeCache = new IMatrixNode[count];

            //Add referenced primaries
            foreach (MDL0BoneNode bone in linker.BoneCache)
            {
                if (bone.ReferenceCount > 0 || bone._infPolys.Count > 0)
                    linker.NodeCache[bone._nodeIndex = index++] = bone;
                else
                    bone._nodeIndex = -1;
                bone._weightCount = 0;
            }

            //Add weight groups
            foreach (Influence i in model._influences._influences)
            {
                linker.NodeCache[i._index = index++] = i;
                foreach (BoneWeight b in i._weights)
                    b.Bone._weightCount++;
            }

            //Add remaining bones
            foreach (MDL0BoneNode bone in linker.BoneCache)
                if (bone._nodeIndex == -1)
                    linker.NodeCache[bone._nodeIndex = index++] = bone;
        }

        public static int CalcSize(ModelLinker linker) { return CalcSize(null, linker); }
        public static int CalcSize(Collada form, ModelLinker linker)
        {
            MDL0Node model = linker.Model;

            int headerLen, 
                groupLen = 0, 
                tableLen = 0, 
                texLen = 0, 
                boneLen = 0, 
                dataLen = 0, 
                defLen = 0, 
                assetLen = 0,
                treeLen = 0, 
                mixLen = 0, 
                opaLen = 0, 
                xluLen = 0;

            int aInd, aLen;

            //Get header length
            switch (linker.Version)
            {
                case 0x08:
                case 0x09: headerLen = 0x80; break;
                case 0x0A: headerLen = 0x88; break;
                case 0x0B: headerLen = 0x8C; break;
                default: headerLen = 0x80;
                    //Unsupported version. Change to 9 as default.
                    linker.Version = 9; break;
            }

            //Assign node indices
            AssignNodeIndices(linker);

            //Get table length
            tableLen = (linker._nodeCount + 1) << 2;

            //Get group/data length
            List<MDLResourceType> iList = ModelLinker.IndexBank[linker.Version];
            foreach (MDLResourceType resType in iList)
            {
                IEnumerable entryList = null;
                int entries = 0;

                switch (resType)
                {
                    case MDLResourceType.Definitions:

                        //NodeTree
                        treeLen = linker.BoneCache.Length * 5;

                        //NodeMix
                        foreach (Influence i in model._influences._influences)
                        {
                            mixLen += 4;
                            foreach (BoneWeight w in i._weights)
                                mixLen += 6;
                        }
                        foreach (MDL0BoneNode b in linker.BoneCache)
                            if (b._weightCount > 0)
                                mixLen += 5;

                        //DrawOpa and DrawXlu
                        //Get assigned materials and categorize
                        if (model._matList != null)
                        for (int i = 0; i < model._matList.Count; i++)
                        {
                            //Entries are ordered by material, not by polygon.
                            MDL0MaterialNode mat = model._matList[i] as MDL0MaterialNode;
                            if (!mat.isMetal)
                            for (int l = 0; l < mat._polygons.Count; l++)
                                if (!mat.XLUMaterial)
                                    opaLen += 8;
                                else
                                    xluLen += 8;
                        }

                        //Add terminate byte and set model def flags
                        if (model._hasTree = (treeLen > 0))
                        { treeLen++; entries++; }
                        if (model._hasMix = (mixLen > 0))
                        { mixLen++; entries++; }
                        if (model._hasOpa = (opaLen > 0))
                        { opaLen++; entries++; }
                        if (model._hasXlu = (xluLen > 0))
                        { xluLen++; entries++; }

                        //Align data
                        defLen += (treeLen + mixLen + opaLen + xluLen).Align(4);

                        break;

                    case MDLResourceType.Vertices:
                        if (model._vertList != null)
                        {
                            entryList = model._vertList;
                            break;
                        }
                        else
                        {
                            aInd = 0; //Set the ID
                            aLen = 1; //Offset count
                        }

                    EvalAssets:

                        List<ResourceNode> polyList = model._polyList;
                        if (polyList == null)
                            break;

                        string str = "";

                        //Create asset lists
                        IList aList;
                        switch (aInd) //Switch by the set ID
                        {
                            case 0: aList = linker._vertices = new List<VertexCodec>(polyList.Count); str = "Vertices "; break;
                            case 1: aList = linker._normals = new List<VertexCodec>(polyList.Count); str = "Normals "; break;
                            case 2: aList = linker._colors = new List<ColorCodec>(polyList.Count); str = "Colors "; break;
                            default: aList = linker._uvs = new List<VertexCodec>(polyList.Count); str = "UVs "; break;
                        }

                        aLen += aInd;
                        for (int i = 0; i < polyList.Count; i++)
                        {
                            MDL0PolygonNode p = polyList[i] as MDL0PolygonNode;
                            for (int x = aInd; x < aLen; x++)
                                if (p._manager._faceData[x] != null)
                                {
                                    if (model._importOptions._rmpClrs && model._importOptions._addClrs)
                                    {
                                        if (i > 0 && x == 2 && model._noColors == true)
                                        { p._elementIndices[x] = 0; continue; }
                                        else if (i >= 0 && x == 3 && model._noColors == true)
                                        { p._elementIndices[x] = -1; break; }
                                    }

                                    p._elementIndices[x] = (short)aList.Count;

                                    if (form != null)
                                        form.Say("Encoding " + str + (x - aInd) + " for Object " + i + ": " + p.Name);
                                    
                                    switch (aInd)
                                    {
                                        case 0:
                                            VertexCodec vert;
                                            aList.Add(vert = new VertexCodec(p._manager.RawVertices, false, model._importOptions._fltVerts));
                                            assetLen += vert._dataLen.Align(0x20) + 0x40;
                                            break;
                                        case 1:
                                            aList.Add(vert = new VertexCodec(p._manager.RawNormals, false, model._importOptions._fltNrms));
                                            assetLen += vert._dataLen.Align(0x20) + 0x20;
                                            break;
                                        case 2:
                                            ColorCodec col;
                                            aList.Add(col = new ColorCodec(p._manager.Colors(x - 2)));
                                            assetLen += col._dataLen.Align(0x20) + 0x20;
                                            break;
                                        default:
                                            aList.Add(vert = new VertexCodec(p._manager.UVs(x - 4), model._importOptions._fltUVs));
                                            assetLen += vert._dataLen.Align(0x20) + 0x40;
                                            break;
                                    }
                                }
                                else
                                    p._elementIndices[x] = -1;
                        }
                        entries = aList.Count;
                        break;
                    case MDLResourceType.Normals:
                        if (model._normList != null)
                            entryList = model._normList;
                        else
                        {
                            aInd = 1; //Set the ID
                            aLen = 1; //Offset count
                            goto EvalAssets;
                        }
                        break;
                    case MDLResourceType.Colors:
                        if (model._colorList != null)
                            entryList = model._colorList;
                        else
                        {
                            aInd = 2; //Set the ID
                            aLen = 2; //Offset count
                            goto EvalAssets;
                        }
                        break;
                    case MDLResourceType.UVs:
                        if (model._uvList != null)
                            entryList = model._uvList;
                        else
                        {
                            aInd = 4; //Set the ID
                            aLen = 8; //Offset count
                            goto EvalAssets;
                        }
                        break;

                    case MDLResourceType.Bones:
                        int index = 0;
                        foreach (MDL0BoneNode b in linker.BoneCache)
                        {
                            if (form != null)
                                form.Say("Calculating the size of the Bones - " + b.Name);

                            b._entryIndex = index++;
                            boneLen += b.CalculateSize(true);
                        }
                        entries = linker.BoneCache.Length;
                        break;

                    case MDLResourceType.Materials: 
                        if (model._matList != null)
                            entries = model._matList.Count; 
                        break;

                    case MDLResourceType.Objects:
                        if (model._polyList != null)
                            entryList = model._polyList; 
                        break;

                    case MDLResourceType.Shaders:
                        if ((entryList = model.GetUsedShaders()) != null && model._matList != null)
                            entries = model._matList.Count;
                        break;

                    case MDLResourceType.Textures:
                        if (model._texList != null)
                        {
                            foreach (MDL0TextureNode tex in model._texList)
                                texLen += (tex._references.Count * 8) + 4;

                            linker._texCount = entries = model._texList.Count;
                        }
                        break;

                    case MDLResourceType.Palettes:
                        if (model._pltList != null)
                        {
                            foreach (MDL0TextureNode pal in model._pltList)
                                texLen += (pal._references.Count * 8) + 4;

                            linker._palCount = entries = model._pltList.Count;
                        }
                        break;
                }

                if (entryList != null)
                {
                    int index = 0;
                    foreach (MDL0EntryNode e in entryList)
                    {
                        if (form != null)
                            if (resType == MDLResourceType.Objects)
                                form.Say("Encoding the " + resType.ToString() + " - " + e.Name);
                            else
                                form.Say("Calculating the size of the " + resType.ToString() + " - " + e.Name);

                        e._entryIndex = index++;
                        dataLen += e.CalculateSize(true);
                    }
                    if (entries == 0)
                        entries = index;
                }

                if (entries > 0)
                    groupLen += (entries * 0x10) + 0x18;
            }

            //Align the materials perfectly using the data length
            int temp = 0;
            if (model._matList != null && iList.IndexOf(MDLResourceType.Materials) != -1)
            {
                int index = 0;
                MDL0MaterialNode prev = null;
                foreach (MDL0MaterialNode e in model._matList)
                {
                    if (form != null)
                        form.Say("Calculating the size of the Materials - " + e.Name);

                    e._entryIndex = index++;

                    if (index == 1)
                    {
                        if ((temp = (e._mdlOffset = headerLen + tableLen + groupLen + texLen + defLen + boneLen).Align(0x10)) != e._mdlOffset)
                            e._dataAlign = temp - e._mdlOffset;
                    }
                    else
                        e._mdlOffset = (prev = ((MDL0MaterialNode)model._matList[index - 1]))._mdlOffset + prev._calcSize;

                    dataLen += e.CalculateSize(true);
                }
            }

            return
            (linker._headerLen = headerLen) +
            (linker._tableLen = tableLen) +
            (linker._groupLen = groupLen) +
            (linker._texLen = texLen) +
            (linker._defLen = defLen) +
            (linker._boneLen = boneLen) +
            (linker._assetLen = assetLen) +
            (linker._dataLen = dataLen) +
            (model._part2Entries.Count > 0 ? 0x1C + model._part2Entries.Count * 0x2C : 0);
        }

        internal static unsafe void Build(ModelLinker linker, MDL0Header* header, int length, bool force) { Build(null, linker, header, length, force); }
        internal static unsafe void Build(Collada form, ModelLinker linker, MDL0Header* header, int length, bool force)
        {
            byte* groupAddr = (byte*)header + linker._headerLen + linker._tableLen;
            byte* dataAddr = groupAddr + linker._groupLen + linker._texLen; //Definitions start here
            byte* assetAddr = dataAddr + linker._defLen + linker._boneLen + linker._dataLen;

            linker.Header = header;

            if (form != null)
                form.Say("Writing header...");

            //Create new model header
            *header = new MDL0Header(length, linker.Version);
            MDL0Props* props = header->Properties;

            if (form != null)
                form.Say("Writing node table...");

            //Write node table, assign node ids
            WriteNodeTable(linker);

            if (form != null)
                form.Say("Writing definitions...");

            //Write def table
            WriteDefs(linker, ref groupAddr, ref dataAddr);

            //Set format list for each polygon's UVAT groups
            SetFormatLists(linker);

            //Write assets first, but only if the model is an import
            if (linker.Model._isImport)
                WriteAssets(form, linker, ref assetAddr);

            //Write groups
            linker.Write(form, ref groupAddr, ref dataAddr, force);

            //Write Part2 Entries
            if (linker.Model._part2Entries.Count > 0 && linker.Version != 9)
            {
                header->_part2Offset = (int)dataAddr - (int)header;
                Part2Data* part2 = header->Part2;
                if (part2 != null)
                {
                    part2->_totalLen = 0x1C + linker.Model._part2Entries.Count * 0x2C;
                    ResourceGroup* pGroup = part2->Group;
                    *pGroup = new ResourceGroup(linker.Model._part2Entries.Count);
                    ResourceEntry* pEntry = &pGroup->_first + 1;
                    byte* pData = (byte*)pGroup + pGroup->_totalSize;
                    foreach (string s in linker.Model._part2Entries)
                    {
                        (pEntry++)->_dataOffset = (int)pData - (int)pGroup;
                        Part2DataEntry* p = (Part2DataEntry*)pData;
                        *p = new Part2DataEntry(1);
                        pData += 0x1C;
                    }
                }
            }
            else
                header->_part2Offset = 0;

            //Write textures
            WriteTextures(linker, ref groupAddr);

            //Set box min and box max
            if (linker.Model._isImport) 
                SetBox(linker);

            //Store group offsets
            linker.Finish();

            //Set new properties
            *props = new MDL0Props(linker.Version, linker.Model._numVertices, linker.Model._numFaces, linker.Model._numNodes, linker.Model._unk1, linker.Model._unk2, linker.Model._unk3, linker.Model._unk4, linker.Model._unk5, linker.Model._unk6, linker.Model.BoxMin, linker.Model.BoxMax);
        }

        private static void WriteNodeTable(ModelLinker linker)
        {
            bint* ptr = (bint*)((byte*)linker.Header + linker._headerLen);
            int len = linker._nodeCount;
            int i = 0;

            //Set length
            *ptr++ = len;

            //Write indices
            while (i < len)
            {
                IMatrixNode n = linker.NodeCache[i++];
                if (n.IsPrimaryNode)
                    *ptr++ = ((MDL0BoneNode)n)._entryIndex;
                else
                    *ptr++ = -1;
            }
        }

        private static void WriteDefs(ModelLinker linker, ref byte* pGroup, ref byte* pData)
        {
            MDL0Node mdl = linker.Model;

            //This should never happen!
            if (!mdl._hasMix && !mdl._hasOpa && !mdl._hasTree && !mdl._hasXlu)
                return;

            IList polyList = mdl._polyList;
            IList matList = mdl._matList;
            MDL0PolygonNode poly;
            MDL0MaterialNode mat;
            int polyCount = 0;
            if (mdl._polyList != null) 
                polyCount = polyList.Count;
            int entryCount = 0;
            byte* floor = pData;
            int dataLen;

            ResourceGroup* group = linker.Defs = (ResourceGroup*)pGroup;
            ResourceEntry* entry = &group->_first + 1;

            //NodeTree
            if (mdl._hasTree)
            {
                //Write group entry
                entry[entryCount++]._dataOffset = (int)(pData - pGroup);

                int bCount = linker.BoneCache.Length;
                for (int i = 0; i < bCount; i++)
                {
                    MDL0BoneNode bone = linker.BoneCache[i] as MDL0BoneNode;

                    *pData = 2; //Entry tag
                    *(bushort*)(pData + 1) = (ushort)bone._entryIndex;
                    *(bushort*)(pData + 3) = (ushort)(bone._parent is MDL0BoneNode ? ((MDL0BoneNode)bone._parent)._nodeIndex : 0);
                    pData += 5; //Advance
                }

                *pData++ = 1; //Terminate
            }

            //NodeMix
            //Only weight references go here.
            //First list bones used by weight groups, in bone order
            //Then list weight groups that use bones. Ordered by entry count.
            if (mdl._hasMix)
            {
                //Write group entry
                entry[entryCount++]._dataOffset = (int)(pData - pGroup);

                //Add bones first (using flat bone list)
                foreach (MDL0BoneNode b in linker.BoneCache)
                    if (b._weightCount > 0)
                    {
                        *pData = 5; //Tag
                        *(bushort*)(pData + 1) = (ushort)b._nodeIndex;
                        *(bushort*)(pData + 3) = (ushort)b._entryIndex;
                        pData += 5; //Advance
                    }

                //Add weight groups (using sorted influence list)
                foreach (Influence i in mdl._influences._influences)
                {
                    *pData = 3; //Tag
                    *(bushort*)&pData[1] = (ushort)i._index;
                    pData[3] = (byte)i._weights.Length;
                    pData += 4; //Advance
                    foreach (BoneWeight w in i._weights)
                    {
                        *(bushort*)pData = (ushort)w.Bone._nodeIndex;
                        *(bfloat*)(pData + 2) = w.Weight;
                        pData += 6; //Advance
                    }
                }

                *pData++ = 1; //Terminate
            }

            //DrawOpa
            if (mdl._hasOpa)
            {
                //Write group entry
                entry[entryCount++]._dataOffset = (int)(pData - pGroup);

                for (int i = 0; i < matList.Count; i++)
                {
                    //Entries are ordered by material, not by polygon.
                    mat = matList[i] as MDL0MaterialNode;
                    if (!mat.isMetal)
                    for (int l = 0; l < mat._polygons.Count; l++)
                        if (!mat.XLUMaterial)
                        {
                            *pData = 4; //Tag
                            *(bushort*)(pData + 1) = (ushort)mat._entryIndex;
                            *(bushort*)(pData + 3) = (poly = mat._polygons[l]) != null ? (ushort)poly._entryIndex : (ushort)0;
                            *(bushort*)(pData + 5) = (ushort)(poly.BoneNode != null ? poly.BoneNode.BoneIndex : 0);
                            pData[7] = 0;
                            pData += 8; //Advance
                        }
                }

                *pData++ = 1; //Terminate
            }

            //DrawXlu
            if (mdl._hasXlu)
            {
                //Write group entry
                entry[entryCount++]._dataOffset = (int)(pData - pGroup);

                for (int i = 0; i < matList.Count; i++)
                {
                    //Entries are ordered by material, not by polygon.
                    mat = matList[i] as MDL0MaterialNode;
                    if (!mat.isMetal)
                    for (int l = 0; l < mat._polygons.Count; l++)
                        if (mat.XLUMaterial)
                        {
                            *pData = 4; //Tag
                            *(bushort*)(pData + 1) = (ushort)mat._entryIndex;
                            *(bushort*)(pData + 3) = (poly = mat._polygons[l]) != null ? (ushort)poly._entryIndex : (ushort)0;
                            *(bushort*)(pData + 5) = (ushort)(poly.BoneNode != null ? poly.BoneNode.BoneIndex : 0);
                            pData[7] = 0;
                            pData += 8; //Advance
                        }
                }

                *pData++ = 1; //Terminate
            }

            //Align data
            dataLen = (int)(pData - floor);
            while ((dataLen++ & 3) != 0)
                *pData++ = 0;

            //Set header
            *group = new ResourceGroup(entryCount);

            //Advance group poiner
            pGroup += group->_totalSize;
        }

        //Write assets will only be used for model imports.
        private static void WriteAssets(Collada form, ModelLinker linker, ref byte* pData)
        {
            int index;
            MDL0Node model = linker.Model;

            if (linker._vertices != null && linker._vertices.Count != 0)
            {
                model.LinkGroup(new MDL0GroupNode(MDLResourceType.Vertices));
                model._vertGroup._parent = model;

                index = 0;
                foreach (VertexCodec c in linker._vertices)
                {
                    MDL0VertexNode node = new MDL0VertexNode();

                    node._name = model.Name + "_" + model._polyList[index]._name;
                    if (((MDL0PolygonNode)model._polyList[index])._material != null)
                        node._name += "_" + ((MDL0PolygonNode)model._polyList[index])._material._name;

                    if (form != null)
                        form.Say("Writing Vertices - " + node.Name);

                    MDL0VertexData* header = (MDL0VertexData*)pData;
                    header->_dataLen = c._dataLen.Align(0x20) + 0x40;
                    header->_dataOffset = 0x40;
                    header->_index = index++;
                    header->_isXYZ = c._hasZ ? 1 : 0;
                    header->_type = (int)c._type;
                    header->_divisor = (byte)c._scale;
                    header->_entryStride = (byte)c._dstStride;
                    header->_numVertices = (short)c._dstCount;
                    header->_eMin = c._min;
                    header->_eMax = c._max;
                    header->_pad1 = header->_pad2 = 0;

                    c.Write(pData + 0x40);

                    node._replSrc = node._replUncompSrc = new DataSource(header, header->_dataLen);
                    model._vertGroup.AddChild(node, false);

                    pData += header->_dataLen;
                }
            }

            if (linker._normals != null && linker._normals.Count != 0)
            {
                model.LinkGroup(new MDL0GroupNode(MDLResourceType.Normals));
                model._normGroup._parent = model;

                index = 0;
                foreach (VertexCodec c in linker._normals)
                {
                    MDL0NormalNode node = new MDL0NormalNode();

                    node._name = model.Name + "_" + model._polyList[index]._name;
                    if (((MDL0PolygonNode)model._polyList[index])._material != null)
                        node._name += "_" + ((MDL0PolygonNode)model._polyList[index])._material._name;

                    if (form != null)
                        form.Say("Writing Normals - " + node.Name);

                    MDL0NormalData* header = (MDL0NormalData*)pData;
                    header->_dataLen = c._dataLen.Align(0x20) + 0x20;
                    header->_dataOffset = 0x20;
                    header->_index = index++;
                    header->_isNBT = 0;
                    header->_type = (int)c._type;
                    header->_divisor = (byte)c._scale;
                    header->_entryStride = (byte)c._dstStride;
                    header->_numVertices = (ushort)c._dstCount;

                    c.Write(pData + 0x20);

                    node._replSrc = node._replUncompSrc = new DataSource(header, header->_dataLen);
                    model._normGroup.AddChild(node, false);

                    pData += header->_dataLen;
                }
            }

            if (linker._colors != null && linker._colors.Count != 0)
            {
                model.LinkGroup(new MDL0GroupNode(MDLResourceType.Colors));
                model._colorGroup._parent = model;

                index = 0;
                foreach (ColorCodec c in linker._colors)
                {
                    MDL0ColorNode node = new MDL0ColorNode();

                    node._name = model.Name + "_" + model._polyList[index]._name;
                    if (((MDL0PolygonNode)model._polyList[index])._material != null)
                        node._name += "_" + ((MDL0PolygonNode)model._polyList[index])._material._name;

                    if (form != null)
                        form.Say("Writing Colors - " + node.Name);

                    MDL0ColorData* header = (MDL0ColorData*)pData;
                    header->_dataLen = c._dataLen.Align(0x20) + 0x20;
                    header->_dataOffset = 0x20;
                    header->_index = index++;
                    header->_isRGBA = c._hasAlpha ? 1 : 0;
                    header->_format = (int)c._outType;
                    header->_entryStride = (byte)c._dstStride;
                    header->_scale = 0;
                    header->_numEntries = (ushort)c._dstCount;

                    c.Write(pData + 0x20);

                    node._replSrc = node._replUncompSrc = new DataSource(header, header->_dataLen);
                    model._colorGroup.AddChild(node, false);

                    pData += header->_dataLen;
                }
            }

            if (linker._uvs != null && linker._uvs.Count != 0)
            {
                model.LinkGroup(new MDL0GroupNode(MDLResourceType.UVs));
                model._uvGroup._parent = model;

                index = 0;
                foreach (VertexCodec c in linker._uvs)
                {
                    MDL0UVNode node = new MDL0UVNode() { _name = "#" + index };

                    if (form != null)
                        form.Say("Writing UVs - " + node.Name);

                    MDL0UVData* header = (MDL0UVData*)pData;
                    header->_dataLen = c._dataLen.Align(0x20) + 0x40;
                    header->_dataOffset = 0x40;
                    header->_index = index++;
                    header->_format = (int)c._type;
                    header->_divisor = (byte)c._scale;
                    header->_isST = 1;
                    header->_entryStride = (byte)c._dstStride;
                    header->_numEntries = (ushort)c._dstCount;
                    header->_min = (Vector2)c._min;
                    header->_max = (Vector2)c._max;
                    header->_pad1 = header->_pad2 = header->_pad3 = header->_pad4 = 0;

                    c.Write(pData + 0x40);

                    node._replSrc = node._replUncompSrc = new DataSource(header, header->_dataLen);
                    model._uvGroup.AddChild(node, false);

                    pData += header->_dataLen;
                }
            }

            //Clean groups
            if (model._vertList != null && model._vertList.Count > 0)
            {
                model._children.Add(model._vertGroup);
                linker.Groups[(int)(MDLResourceType)Enum.Parse(typeof(MDLResourceType), model._vertGroup.Name)] = model._vertGroup;
            }
            else
                model.UnlinkGroup(model._vertGroup);

            if (model._normList != null && model._normList.Count > 0)
            {
                model._children.Add(model._normGroup);
                linker.Groups[(int)(MDLResourceType)Enum.Parse(typeof(MDLResourceType), model._normGroup.Name)] = model._normGroup;
            }
            else
                model.UnlinkGroup(model._normGroup);

            if (model._uvList != null && model._uvList.Count > 0)
            {
                model._children.Add(model._uvGroup);
                linker.Groups[(int)(MDLResourceType)Enum.Parse(typeof(MDLResourceType), model._uvGroup.Name)] = model._uvGroup;
            }
            else
                model.UnlinkGroup(model._uvGroup);

            if (model._colorList != null && model._colorList.Count > 0)
            {
                model._children.Add(model._colorGroup);
                linker.Groups[(int)(MDLResourceType)Enum.Parse(typeof(MDLResourceType), model._colorGroup.Name)] = model._colorGroup;
            }
            else
                model.UnlinkGroup(model._colorGroup);

            //Link sets
            if (model._polyList != null)
            foreach (MDL0PolygonNode poly in model._polyList)
            {
                if (poly._elementIndices[0] != -1)
                    poly._vertexNode = (MDL0VertexNode)model._vertGroup._children[poly._elementIndices[0]];
                if (poly._elementIndices[1] != -1)
                    poly._normalNode = (MDL0NormalNode)model._normGroup._children[poly._elementIndices[1]];
                for (int i = 2; i < 4; i++)
                    if (poly._elementIndices[i] != -1)
                        poly._colorSet[i - 2] = (MDL0ColorNode)model._colorGroup._children[poly._elementIndices[i]];
                for (int i = 4; i < 12; i++)
                    if (poly._elementIndices[i] != -1)
                        poly._uvSet[i - 4] = (MDL0UVNode)model._uvGroup._children[poly._elementIndices[i]];
            }
        }

        //Materials must already be written. Do this last!
        private static void WriteTextures(ModelLinker linker, ref byte* pGroup)
        {
            MDL0GroupNode texGrp = linker.Groups[(int)MDLResourceType.Textures];
            MDL0GroupNode palGrp = linker.Groups[(int)MDLResourceType.Palettes];

            if (texGrp == null) return;

            ResourceGroup* pTexGroup = null;
            ResourceEntry* pTexEntry = null;
            if (linker._texCount > 0)
            {
                linker.Textures = pTexGroup = (ResourceGroup*)pGroup;
                *pTexGroup = new ResourceGroup(linker._texCount);

                pTexEntry = &pTexGroup->_first + 1;
                pGroup += pTexGroup->_totalSize;
            }

            ResourceGroup* pDecGroup = null;
            ResourceEntry* pDecEntry = null;
            if (linker._palCount > 0)
            {
                linker.Palettes = pDecGroup = (ResourceGroup*)pGroup;
                *pDecGroup = new ResourceGroup(linker._palCount);
                pDecEntry = &pDecGroup->_first + 1;
                pGroup += pDecGroup->_totalSize;
            }

            bint* pData = (bint*)pGroup;
            int offset;

            //Textures
            if (pTexGroup != null)
                foreach (MDL0TextureNode t in texGrp._children)
                    if (t._references.Count > 0)
                    {
                        offset = (int)pData;
                        (pTexEntry++)->_dataOffset = offset - (int)pTexGroup;
                        *pData++ = t._references.Count;
                        foreach (MDL0MaterialRefNode mat in t._references)
                        {
                            *pData++ = (int)mat.Material.WorkingUncompressed.Address - offset;
                            *pData++ = (int)mat.WorkingUncompressed.Address - offset;
                        }
                    }

            //Palettes
            if (pDecGroup != null)
                foreach (MDL0TextureNode t in palGrp._children)
                    if (t._references.Count > 0)
                    {
                        offset = (int)pData;
                        (pDecEntry++)->_dataOffset = offset - (int)pDecGroup;
                        *pData++ = t._references.Count;
                        foreach (MDL0MaterialRefNode mat in t._references)
                        {
                            *pData++ = (int)mat.Material.WorkingUncompressed.Address - offset;
                            *pData++ = (int)mat.WorkingUncompressed.Address - offset;
                        }
                    }
            
        }

        private static void SetBox(ModelLinker linker)
        {
            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);
            if (linker.Model._vertList != null)
            {
                foreach (MDL0VertexNode v in linker.Model._vertList)
                {
                    if (v.EMin._x < min._x)
                        min._x = v.EMin._x;
                    if (v.EMin._y < min._y)
                        min._y = v.EMin._y;
                    if (v.EMin._z < min._z)
                        min._z = v.EMin._z;

                    if (v.EMax._x > max._x)
                        max._x = v.EMax._x;
                    if (v.EMax._y > max._y)
                        max._y = v.EMax._y;
                    if (v.EMax._z > max._z)
                        max._z = v.EMax._z;
                }
            }
            else
                min = max = new Vector3(0);

            linker.Model._min = min;
            linker.Model._max = max;

            if (linker.Model._polyList != null)
            {
                linker.Model._numVertices = 0;
                linker.Model._numFaces = 0;
                foreach (MDL0PolygonNode n in linker.Model._polyList)
                {
                    linker.Model._numVertices += n._numVertices;
                    linker.Model._numFaces += n._numFaces;
                }
            }
        }

        private static void SetFormatLists(ModelLinker linker)
        {
            if (linker.Model._polyList != null)
            for (int i = 0; i < linker.Model._polyList.Count; i++)
            {
                MDL0PolygonNode poly = (MDL0PolygonNode)linker.Model._polyList[i];
                poly._fmtList = poly._manager.setFmtList(poly, linker);
            }
        }
    }
}
