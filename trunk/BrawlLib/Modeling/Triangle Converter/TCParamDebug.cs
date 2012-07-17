using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.Modeling
{
    public static unsafe partial class TriangleConverter
    {
        //#if Debug

        //static void dumpTriangles(ACTCEdge *e, FILE *fp)
        //{
        //    int i;
        //    int c;
        //    char v[12];

        //    c = fprintf(fp, "      %d triangles: ");
        //    for(i = 0; i < e.Triangles.Length; i++) {
        //    if (c + 1 + sprintf(v, "%u", e.Triangles[i].FinalVert) > 78) {
        //        fputs("\n", fp);
        //        c = fprintf(fp, "        ");
        //    }
        //    c += fprintf(fp, " %s", v);
        //    }
        //    fputs("\n", fp);
        //}

        //static void dumpEdges(ACTCVertex *vert, FILE *fp)
        //{
        //    int i;
        //    int c;
        //    char v[26]; /* two signed ints plus x plus NUL */

        //    for(i = 0; i < vert.Edges.Length; i++) {
        //    fprintf(fp, "    %u.%u (%d times)\n", vert.V, vert.Edges[i].V2.V,
        //        vert.Edges[i].Count);
        //    dumpTriangles(&vert.Edges[i], fp);
        //    }
        //    fputs("\n", fp);
        //}

        //static void dumpVertices(ACTCData *tc, FILE *fp)
        //{
        //    int i;
        //    ACTCVertex *v;

        //    if (!tc.UsingStaticVerts)
        //        tableResetIterator(tc.VertexIterator);

        //    fprintf(fp, "%d vertices in valences list\n", tc.VertexCount);
        //    if (tc.UsingStaticVerts) {
        //        for(i = 0; i < tc.VertRange; i++) {
        //        v = &tc.StaticVerts[i];
        //        if (v.Count > 0) {
        //        fprintf(fp, "  vertex %u, valence %d, %d edges\n", v.V,
        //            v.Count, v.Edges.Length);
        //        dumpEdges(v, fp);
        //        }
        //    }
        //    } else {
        //    for(i = 0; i < tc.VertexCount; i++) {
        //        if (tableIterate(tc.Vertices, tc.VertexIterator, null,
        //        (void **)&v) == 0) {
        //        fprintf(fp, "ACTC::dumpVertices : fewer vertices in the table "
        //            "than we expected!\n");
        //        fprintf(stderr, "ACTC::dumpVertices : fewer vertices in the table "
        //            "than we expected!\n");
        //        }
        //        if (v == null) {
        //        fprintf(fp, "ACTC::dumpVertices : did not expect to get a null"
        //            "Vertex from the table iterator!\n");
        //        fprintf(stderr, "ACTC::dumpVertices : did not expect to get a null"
        //            "Vertex from the table iterator!\n");
        //        }
        //        fprintf(fp, "  vertex %u, valence %d, %d edges\n", v.V, v.Count,
        //        v.Edges.Length);
        //        dumpEdges(v, fp);
        //    }
        //    }
        //}

        //static void dumpVertexBins(ACTCData *tc, FILE *fp)
        //{
        //    ACTCVertex *cur;
        //    int i;
        //    int c;
        //    char v[26]; /* two signed ints plus x plus NUL */

        //    fprintf(fp, "vertex bins:\n");
        //    if (tc.VertexBins == null) {
        //        fprintf(fp, "        empty.\n");
        //    return;
        //    }
        //    for(i = 1; i <= tc.CurMaxVertValence; i++) {
        //        cur = tc.VertexBins[i];
        //    c = fprintf(fp, "        bin %d . ", i);
        //    while(cur != null) {
        //        if (c + 1 + sprintf(v, "%ux%d", cur.V, cur.Count) > 78) {
        //        fputs("\n", fp);
        //        c = fprintf(fp, "          ");
        //        }
        //        c += fprintf(fp, " %s", v);
        //        cur = cur.Next;
        //    }
        //    fputs("\n", fp);
        //    }
        //}

        //void actcDumpState(ACTCData *tc, FILE *fp)
        //{
        //    dumpVertices(tc, fp);
        //    dumpVertexBins(tc, fp);
        //}

        //static int abortWithOptionalDump(ACTCData *tc)
        //{
        //    (int)ACTC_.INFO(actcDumpState(tc, stderr));
        //    abort();
        //}

        //#endif Debug

        static ACTC_var actcGetError(ACTCData tc)
        {
            ACTC_var error = tc.Error;
            tc.Error = ACTC_var.NO_ERROR;
            return error;
        }

        //No need to use pointers.
        //static void *reallocAndAppend(void **ptr, uint *itemCount, uint itemBytes, void *append)
        //{
        //    void *t;

        //    t = (void*)Marshal.ReAllocCoTaskMem(new IntPtr(*ptr), (int)itemBytes * (int)(*itemCount + 1));
        //    if (t == null)
        //        return null;
        //    *ptr = t;

        //    //memcpy((unsigned char *)*ptr + *itemCount * itemBytes, append, itemBytes);
        //    (*itemCount) += 1;

        //    return *ptr;
        //}

        /*
         * Call only during input; changes vertices' valences and does not
         * fix the bins that are ordered by vertex valence.  (It's usually cheaper
         * to traverse the vertex list once after all are added, since that's
         * linear in the NUMBER OF UNIQUE VERTEX INDICES, which is almost always
         * going to be less than the number of vertices.)
         */
        static ACTC_var incVertexValence(ref ACTCData tc, uint v, out ACTCVertex found)
        {
            ACTCVertex vertex;
            found = null;
            if (tc.UsingStaticVerts == 1)
            {
                vertex = tc.StaticVerts[v];
                vertex.Count++;
                if (vertex.Count == 1)
                {
                    vertex.V = v;
                    tc.VertexCount++;
                }
            }
            else
            {
                if (tableRetrieve(v, tc.Vertices, out vertex) == 1)
                {
                    if (vertex.V != v)
                    {
                        //(int)ACTC_.DEBUG(
                        //    fprintf(stderr, "ACTC::incVertexValence : Got vertex %d when "
                        //    "looking for vertex %d?!?\n", vertex.V, v);
                        //    abortWithOptionalDump(tc);
                        //)
                        return tc.Error = ACTC_var.DATABASE_CORRUPT;
                    }
                    vertex.Count++;
                }
                else
                {
                    ////chartedSetLabel("new Vertex");
                    vertex = new ACTCVertex();
                    vertex.V = v;
                    vertex.Count = 1;
                    vertex.Edges = new ACTCEdge[0];
                    //vertex.Edges.Length = 0;
                    if (tableInsert(v, ref tc.Vertices, vertex) == 0)
                    {
                        //(int)ACTC_.DEBUG(fprintf(stderr, "ACTC::incVertexValence : Failed "
                        //    "to insert vertex into table\n");)
                        return tc.Error = ACTC_var.ALLOC_FAILED;
                    }
                    tc.VertexCount++;
                }
            }

            if (vertex.Count > tc.CurMaxVertValence)
                tc.CurMaxVertValence = vertex.Count;

            found = vertex;

            return ACTC_var.NO_ERROR;
        }

        static ACTC_var decVertexValence(ref ACTCData tc, ref ACTCVertex v)
        {
            v.Count--;
            if (v.Count < 0)
            {
                //(int)ACTC_.DEBUG(
                //    fprintf(stderr, "ACTC::decVertexValence : Valence went "
                //    "negative?!?\n");
                //    abortWithOptionalDump(tc);
                //)
                return tc.Error = ACTC_var.DATABASE_CORRUPT;
            }

            if (v.PointsToMe != null)
            {
                v.PointsToMe[0] = v.Next;
                if (v.Next != null)
                    v.Next.PointsToMe = v.PointsToMe;
                v.Next = null;
            }

            if (v.Count == 0)
            {
                tc.VertexCount--;
                if (v.Edges != null)
                {
                    //free(v.Edges);
                    //v.Edges = null;
                }
                if (tc.UsingStaticVerts == 0)
                {
                    ACTCVertex g = null;
                    tableRemove(v.V, ref tc.Vertices, ref g);
                    //free(v);
                }
                //v = null;
            }
            else
            {
                if (tc.VertexBins != null)
                {
                    v.Next = tc.VertexBins[v.Count];
                    v.PointsToMe[0] = tc.VertexBins[v.Count];
                    if (v.Next != null)
                        v.Next.PointsToMe[0] = v.Next;
                    tc.VertexBins[v.Count] = v;
                    if (v.Count < tc.CurMinVertValence)
                        tc.CurMinVertValence = v.Count;
                }
            }

            return ACTC_var.NO_ERROR;
        }

        static ACTC_var findNextFanVertex(ref ACTCData tc, ref ACTCVertex vert)
        {
            if (tc.CurMaxVertValence < tc.MinFanVerts)
                return ACTC_var.NO_MATCHING_VERT;

            while (tc.VertexBins[tc.CurMaxVertValence] == null)
            {
                tc.CurMaxVertValence--;
                if (tc.CurMaxVertValence < tc.CurMinVertValence)
                {
                    if (tc.VertexCount > 0)
                    {
                        //(int)ACTC_.DEBUG(fprintf(stderr, "tc::findNextFanVertex : no more "
                        //    "vertices in bins but VertexCount > 0\n");)
                        return tc.Error = ACTC_var.DATABASE_CORRUPT;
                    }
                    return ACTC_var.NO_MATCHING_VERT;
                }
            }
            vert = tc.VertexBins[tc.CurMaxVertValence];
            return ACTC_var.NO_ERROR;
        }

        static ACTC_var findNextStripVertex(ref ACTCData tc, ref ACTCVertex vert)
        {
            while (tc.VertexBins[tc.CurMinVertValence] == null)
            {
                tc.CurMinVertValence++;
                if (tc.CurMinVertValence > tc.CurMaxVertValence)
                {
                    if (tc.VertexCount > 0)
                    {
                        //(int)ACTC_.DEBUG(fprintf(stderr, "tc::findNextStripVertex : no more "
                        //    "vertices in bins but VertexCount > 0\n");)
                        return tc.Error = ACTC_var.DATABASE_CORRUPT;
                    }
                    return ACTC_var.NO_MATCHING_VERT;
                }
            }
            vert = tc.VertexBins[tc.CurMinVertValence];
            return ACTC_var.NO_ERROR;
        }

        static int actcGetIsDuringInput(ACTCData tc)
        {
            return tc.IsInputting;
        }

        static ACTC_var actcBeginInput(ref ACTCData tc)
        {
            if (tc.IsOutputting != 0)
            {
                //(int)ACTC_.DEBUG(fprintf(stderr, "actcBeginInput : called within "
                //    "BeginOutput/EndOutput\n");)
                return tc.Error = ACTC_var.DURING_INPUT;
            }

            if (tc.IsInputting != 0)
            {
                //(int)ACTC_.DEBUG(fprintf(stderr, "actcBeginInput : called within "
                //    "BeginInput/EndInput\n");)
                return tc.Error = ACTC_var.DURING_INPUT;
            }

            tc.IsInputting = 1;
            tc.CurMaxVertValence = 0;

            if (tc.MaxInputVert < MAX_STATIC_VERTS - 1)
            {
                //uint byteCount;
                tc.UsingStaticVerts = 1;
                tc.VertRange = tc.MaxInputVert + 1;
                //byteCount = (uint)sizeof(ACTCVertex) * tc.VertRange;

                ////chartedSetLabel("static verts");
                //fixed (ACTCVertex* t = new ACTCVertex[tc.VertRange])
                tc.StaticVerts = new ACTCVertex[tc.VertRange];
                if (tc.StaticVerts == null)
                {
                    //(int)ACTC_.INFO(printf("Couldn't allocate static %d vert block of %u "
                    //"bytes\n", tc.VertRange, byteCount);)
                    tc.UsingStaticVerts = 0;
                }
            }
            else
                tc.UsingStaticVerts = 0;

            return ACTC_var.NO_ERROR;
        }

        static ACTC_var actcEndInput(ref ACTCData tc)
        {
            if (tc.IsOutputting != 0)
            {
                //(int)ACTC_.DEBUG(fprintf(stderr, "actcEndInput : called within "
                //    "BeginOutput/EndOutput\n");)
                return tc.Error = ACTC_var.DURING_OUTPUT;
            }

            if (tc.IsInputting == 0)
            {
                //(int)ACTC_.DEBUG(fprintf(stderr, "actcEndInput : called outside "
                //    "BeginInput/EndInput\n");)
                return tc.Error = ACTC_var.IDLE;
            }

            tc.IsInputting = 0;

            return ACTC_var.NO_ERROR;
        }

        static int actcGetIsDuringOutput(ACTCData tc) { return tc.IsOutputting; }

        static ACTC_var actcBeginOutput(ref ACTCData tc)
        {
            ACTCVertex v = new ACTCVertex();
            int i;

            if (tc.IsInputting != 0)
            {
                //(int)ACTC_.DEBUG(fprintf(stderr, "actcBeginOutput : called within "
                //    "BeginInput/EndInput\n");)
                return tc.Error = ACTC_var.DURING_INPUT;
            }

            if (tc.IsOutputting != 0)
            {
                //(int)ACTC_.DEBUG(fprintf(stderr, "actcBeginOutput : called within "
                //    "BeginOutput/EndOutput\n");)
                return tc.Error = ACTC_var.DURING_OUTPUT;
            }

            tc.IsOutputting = 1;

            tc.CurMinVertValence = int.MaxValue;
            //chartedSetLabel("vertex bins");

            tc.VertexBins = new ACTCVertex[tc.CurMaxVertValence + 1];

            //if (tc.VertexBins == null) 
            //{
            //    //(int)ACTC_.DEBUG(fprintf(stderr, "actcBeginOutput : couldn't allocate %d bytes "
            //    //    "for Vertex Bins\n",
            //    //    sizeof(ACTCVertex *) * tc.CurMaxVertValence);)
            //    return tc.Error = (int)ACTC_.ALLOC_FAILED;
            //}

            if (tc.UsingStaticVerts != 0)
            {
                double edgeTotal = 0;
                for (i = 0; i < tc.VertRange; i++)
                {
                    v = tc.StaticVerts[i];
                    if (v.Count > 0)
                    {
                        v.Next = tc.VertexBins[v.Count];
                        v.PointsToMe[0] = tc.VertexBins[v.Count];
                        tc.VertexBins[v.Count] = v;

                        if (v.Next != null)
                            v.Next.PointsToMe[0] = v.Next;

                        if (v.Count < tc.CurMinVertValence)
                            tc.CurMinVertValence = v.Count;

                        edgeTotal += v.Edges.Length;
                    }
                }
            }
            else
            {
                tableResetIterator(ref tc.VertexIterator);
                for (i = 0; i < tc.VertexCount; i++)
                {
                    uint* g = null;
                    if (tableIterate(tc.Vertices, ref tc.VertexIterator, ref g, ref v) == 0)
                    {
                        //(int)ACTC_.DEBUG(fprintf(stderr, "actcBeginOutput : fewer vertices in "
                        //    "the table than we expected!\n");)
                        return tc.Error = ACTC_var.DATABASE_CORRUPT;
                    }

                    v.Next = tc.VertexBins[v.Count];

                    ACTCVertex[] PointsToMe = new ACTCVertex[tc.VertexBins.Length - v.Count];
                    for (int x = v.Count; x < tc.VertexBins.Length; x++)
                        PointsToMe[x - v.Count] = tc.VertexBins[x];

                    v.PointsToMe = PointsToMe;
                    tc.VertexBins[v.Count] = v;

                    if (v.Next != null)
                    {
                        if (v.Next.PointsToMe == null)
                            v.Next.PointsToMe = new ACTCVertex[1];
                        v.Next.PointsToMe[0] = v.Next;
                    }

                    if (v.Count < tc.CurMinVertValence)
                        tc.CurMinVertValence = v.Count;
                }
            }

            return ACTC_var.NO_ERROR;
        }

        static ACTC_var actcEndOutput(ref ACTCData tc)
        {
            if (tc.IsInputting != 0)
            {
                //(int)ACTC_.DEBUG(fprintf(stderr, "actcEndOutput : called within "
                //    "BeginInput/EndInput\n");)
                return tc.Error = ACTC_var.DURING_INPUT;
            }

            if (tc.IsOutputting == 0)
            {
                //(int)ACTC_.DEBUG(fprintf(stderr, "actcEndOutput : called outside "
                //    "BeginOutput/EndOutput\n");)
                return tc.Error = ACTC_var.IDLE;
            }

            tc.IsOutputting = 0;

            if (tc.UsingStaticVerts != 0)
            {
                //free(tc.StaticVerts);
                tc.StaticVerts = null;
                tc.UsingStaticVerts = 0;
            }

            //free(tc.VertexBins);
            tc.VertexBins = null;

            return ACTC_var.NO_ERROR;
        }

        public static ACTCData actcNew()
        {
            ACTCData tc = new ACTCData();
            //#if Debug
            //    static int didPrintVersion = 0;

            //    if (!didPrintVersion) {
            //    int verMinor, verMajor;
            //    didPrintVersion = 1;

            //        actcGetParami(tc, (int)ACTC_.MAJOR_VERSION, &verMajor);
            //        actcGetParami(tc, (int)ACTC_.MINOR_VERSION, &verMinor);
            //    fprintf(stderr, "TC Version %d.%d\n", verMajor, verMinor);
            //    }
            //#endif Debug

            //chartedSetLabel("the tc struct");

            //if (tc == null)
            //{
            //    //(int)ACTC_.DEBUG(fprintf(stderr, "actcNew : couldn't allocate %d bytes "
            //    //    "for new ACTCData\n", sizeof(*tc));)
            //    return null;
            //}

            tc.Vertices = new TableRoot();
            tc.Vertices.Table = new TableLevel2[16384];
            tc.VertexIterator = new TableIterator();
            tc.MinFanVerts = 3;
            tc.MaxPrimVerts = int.MaxValue;
            tc.MaxInputVert = int.MaxValue;
            tc.MaxEdgeShare = int.MaxValue;
            tc.MaxVertShare = int.MaxValue;
            tc.HonorWinding = 1;

            /* seed = 0 handled by calloc */
            /* XXX grantham 20000615 - seed ignored for now */

            return tc;
        }

        //static uint allocatedForTriangles(ACTCEdge e)
        //{
        //    return (uint)sizeof(ACTCTriangle) * (uint)e.Triangles.Length;
        //}

        //static uint allocatedForEdges(ACTCVertex *vert)
        //{
        //    int i;
        //    uint size;

        //    size = (uint)sizeof(ACTCEdge) * (uint)vert.Edges.Length;
        //    for(i = 0; i < vert.Edges.Length; i++) {
        //    size += allocatedForTriangles(&vert.Edges[i]);
        //    }
        //    return size;
        //}

        //static uint allocatedForVertices(ACTCData *tc)
        //{
        //    int i;
        //    int size = 0;
        //    ACTCVertex *v;

        //    if (tc.UsingStaticVerts == 0)
        //        tableResetIterator(tc.VertexIterator);

        //    if (tc.UsingStaticVerts != 0) {
        //        size = (int)tc.VertRange * sizeof(ACTCVertex);
        //        for(i = 0; i < tc.VertRange; i++) {
        //        v = &tc.StaticVerts[i];
        //        if (v.Count > 0)
        //            size += (int)allocatedForEdges(v);
        //    }
        //    } else {
        //    for(i = 0; i < tc.VertexCount; i++) {
        //        tableIterate(tc.Vertices, tc.VertexIterator, null, (void **)&v);
        //        size += (int)allocatedForEdges(v);
        //    }
        //    }
        //    return (uint)size;
        //}

        //int actcGetMemoryAllocation(ACTCData *tc, uint *bytesAllocated)
        //{
        //    int tableBytes = 0;

        //    tableGetStats(*tc.Vertices, -1, -1, tableBytes);
        //    *bytesAllocated = (uint)sizeof(ACTCData);
        //    *bytesAllocated += (uint)tableBytes;
        //    *bytesAllocated += allocatedForVertices(tc); /* recurses */

        //    return (int)ACTC_.NO_ERROR;
        //}

        static void freeVertex(ACTCVertex v)
        {
            v = new ACTCVertex();
        }

        static ACTC_var actcMakeEmpty(ref ACTCData tc)
        {
            tc.VertexCount = 0;
            if (tc.UsingStaticVerts == 0)
                tableDelete(tc.Vertices);
            if (tc.VertexBins != null)
            {
                //tc.VertexBins = null;//free(tc.VertexBins);
                tc.VertexBins = null;
            }
            tc.IsOutputting = 0;
            tc.IsInputting = 0;
            return ACTC_var.NO_ERROR;
        }

        static void actcDelete(ACTCData tc)
        {
            actcMakeEmpty(ref tc);
            tc.VertexIterator = null; //free(tc.VertexIterator);
            tc.Vertices = null; //free(tc.Vertices);
            //tc = new ACTCData(); //free(tc);
        }

        static ACTC_var actcParami(ACTCData tc, ACTC_var param, int value)
        {
            if (tc.IsInputting != 0)
            {
                //(int)ACTC_.DEBUG(fprintf(stderr, "actcParami : within BeginInput/"
                //    "EndInput\n");)
                return tc.Error = ACTC_var.DURING_INPUT;
            }
            if (tc.IsOutputting != 0)
            {
                //(int)ACTC_.DEBUG(fprintf(stderr, "actcParami : within BeginOutput/"
                //    "EndOutput\n");)
                return tc.Error = ACTC_var.DURING_OUTPUT;
            }

            switch (param)
            {
                case ACTC_var.OUT_MIN_FAN_VERTS:
                    tc.MinFanVerts = value;
                    break;

                case ACTC_var.IN_MAX_VERT:
                    if (value < tc.MinInputVert)
                    {
                        //(int)ACTC_.DEBUG(fprintf(stderr, "actcParami : tried to set "
                        //    "MAX_INPUT_VERT to %d, less than MIN_INPUT_VERT (%d)\n",
                        //    value, tc.MinInputVert);)
                        return tc.Error = ACTC_var.INVALID_VALUE;
                    }
                    tc.MaxInputVert = (uint)value;
                    break;

                case ACTC_var.IN_MIN_VERT:
                    if (value > tc.MaxInputVert)
                    {
                        //(int)ACTC_.DEBUG(fprintf(stderr, "actcParami : tried to set "
                        //    "MIN_INPUT_VERT to %d, greater than MAX_INPUT_VERT (%d)\n",
                        //    value, tc.MaxInputVert);)
                        return tc.Error = ACTC_var.INVALID_VALUE;
                    }
                    tc.MinInputVert = (uint)value;
                    break;

                case ACTC_var.IN_MAX_EDGE_SHARING:
                    tc.MaxEdgeShare = value;
                    break;

                case ACTC_var.IN_MAX_VERT_SHARING:
                    tc.MaxVertShare = value;
                    break;

                case ACTC_var.OUT_HONOR_WINDING:
                    tc.HonorWinding = value;
                    break;

                case ACTC_var.OUT_MAX_PRIM_VERTS:
                    if (value < 3)
                    {
                        //(int)ACTC_.DEBUG(fprintf(stderr, "actcParami : tried to set "
                        //    "MAX_PRIM_VERTS to %d (needed to be 3 or more)\n", value);)
                        return tc.Error = ACTC_var.INVALID_VALUE;
                    }
                    tc.MaxPrimVerts = value;
                    break;

            }
            return ACTC_var.NO_ERROR;
        }

        static ACTC_var actcParamu(ACTCData tc, ACTC_var param, uint value)
        {
            /*
             * XXX - yes, this is questionable, but I consulted industry
             * experts and we agreed that most common behavior is to copy the
             * bits directly, which is what I want.
             */
            return actcParami(tc, param, (int)value);
        }

        static ACTC_var actcGetParami(ACTCData tc, ACTC_var param, int* value)
        {
            switch (param)
            {
                case ACTC_var.MAJOR_VERSION:
                    *value = 1;
                    break;

                case ACTC_var.MINOR_VERSION:
                    *value = 1;
                    break;

                case ACTC_var.IN_MAX_VERT:
                    *value = (int)tc.MaxInputVert;
                    break;

                case ACTC_var.IN_MIN_VERT:
                    *value = (int)tc.MinInputVert;
                    break;

                case ACTC_var.IN_MAX_EDGE_SHARING:
                    *value = tc.MaxEdgeShare;
                    break;

                case ACTC_var.IN_MAX_VERT_SHARING:
                    *value = tc.MaxVertShare;
                    break;

                case ACTC_var.OUT_MIN_FAN_VERTS:
                    *value = tc.MinFanVerts;
                    break;

                case ACTC_var.OUT_HONOR_WINDING:
                    *value = tc.HonorWinding;
                    break;

                case ACTC_var.OUT_MAX_PRIM_VERTS:
                    *value = tc.MaxPrimVerts;
                    break;

                default:
                    *value = 0;
                    return tc.Error = ACTC_var.INVALID_VALUE;
            }
            return ACTC_var.NO_ERROR;
        }

        static ACTC_var actcGetParamu(ACTCData tc, ACTC_var param, uint* value)
        {
            return actcGetParami(tc, param, (int*)value);
        }
    }
}