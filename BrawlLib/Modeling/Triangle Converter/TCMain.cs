using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.Modeling
{
    public static unsafe partial class TriangleConverter
    {
        public static int actcTrianglesToPrimitives(ACTCData tc, int triangleCount, uint[][] triangles, ACTC_var[] primTypes, int[] primLengths, uint[] vertices, int maxBatchSize)
        {
            ACTC_var r;
            int curTriangle;
            int curPrimitive;
            uint curVertex;
            ACTC_var prim;
            int v1 = -1, v2 = -1, v3 = -1;
            int lastPrim;
            int passesWithoutPrims;
            int trisSoFar;

            if (tc.IsInputting != 0)
            {
                //(int)ACTC_.DEBUG(fprintf(stderr, "actcTrianglesToPrimitives : within BeginInput/"
                //    "EndInput\n");)
                return (int)(tc.Error = ACTC_var.DURING_INPUT);
            }
            if (tc.IsOutputting != 0)
            {
                //(int)ACTC_.DEBUG(fprintf(stderr, "actcTrianglesToPrimitives : within"
                //    "BeginOutput/EndOutput\n");)
                return (int)(tc.Error = ACTC_var.DURING_OUTPUT);
            }

            curTriangle = 0;
            curPrimitive = 0;
            curVertex = 0;
            passesWithoutPrims = 0;

            actcMakeEmpty(ref tc);

            ACTC_CHECK(actcBeginInput(ref tc));
            trisSoFar = 0;

            while (curTriangle < triangleCount)
            {
                r = actcAddTriangle(ref tc, triangles[curTriangle][0],
                    triangles[curTriangle][1], triangles[curTriangle][2]);

                trisSoFar++;
                curTriangle++;

                if ((trisSoFar >= maxBatchSize) ||
                    (r == ACTC_var.ALLOC_FAILED && curTriangle != triangleCount) ||
                    (r == ACTC_var.NO_ERROR && curTriangle == triangleCount))
                {
                    /* drain what we got */
                    trisSoFar = 0;

                    ACTC_CHECK(actcEndInput(ref tc));
                    ACTC_CHECK(actcBeginOutput(ref tc));

                    lastPrim = curPrimitive;

                    while ((prim = actcStartNextPrim(ref tc, out v1, out v2)) != ACTC_var.DATABASE_EMPTY)
                    {
                        ACTC_CHECK(prim);

                        primTypes[curPrimitive] = prim;
                        primLengths[curPrimitive] = 2;

                        vertices[curVertex++] = (uint)v1;
                        vertices[curVertex++] = (uint)v2;

                        while ((r = actcGetNextVert(ref tc, out v3)) != ACTC_var.PRIM_COMPLETE)
                        {
                            ACTC_CHECK(r);
                            vertices[curVertex++] = (uint)v3;
                            primLengths[curPrimitive]++;
                        }

                        curPrimitive++;
                    }
                    ACTC_CHECK(actcEndOutput(ref tc));

                    if (r == ACTC_var.ALLOC_FAILED && curPrimitive == lastPrim)
                    {
                        if (passesWithoutPrims == 0)
                        {
                            //not enough memory to add a triangle and 
                            //nothing in the database, better free everything 
                            //and try again
                            actcMakeEmpty(ref tc);
                        }
                        else
                        {
                            //cleaned up and STILL couldn't get a triangle in; 
                            //give up
                            return (int)(tc.Error = ACTC_var.ALLOC_FAILED);
                        }
                        passesWithoutPrims++;
                    }
                    ACTC_CHECK(actcBeginInput(ref tc));
                }
                else
                    ACTC_CHECK(r);

                if (r == ACTC_var.ALLOC_FAILED)
                    curTriangle--;
            }

            ACTC_CHECK(actcEndInput(ref tc));

            actcMakeEmpty(ref tc);

            return curPrimitive;
        }

        #region Triangle Mapping
        static ACTC_var mapEdgeTriangle(ref ACTCEdge edge, ACTCVertex v3)
        {
            ACTCTriangle tmp = new ACTCTriangle();
            tmp.FinalVert = v3;

            if (edge.Triangles == null)
                edge.Triangles = new ACTCTriangle[0];

            Array.Resize<ACTCTriangle>(ref edge.Triangles, edge.Triangles.Length + 1);
            edge.Triangles[edge.Triangles.Length - 1] = tmp;

            return ACTC_var.NO_ERROR;
        }

        static ACTC_var unmapEdgeTriangle(ref ACTCData tc, ref ACTCEdge edge, ACTCVertex v3)
        {
            int i;

            for (i = 0; i < edge.Triangles.Length; i++)
	            if (edge.Triangles[i].FinalVert == v3)
	                break;

            if (i == edge.Triangles.Length) 
            {
	            //(int)ACTC_.DEBUG(
	            //    fprintf(stderr, "ACTC::unmapEdgeTriangle : Couldn't find third vertex"
	            //        " from edge in order to delete it?!?\n");
	            //    abortWithOptionalDump(tc);
	            //)
	            return tc.Error = ACTC_var.DATABASE_CORRUPT;
            }

            edge.Triangles[i] = edge.Triangles[edge.Triangles.Length - 1];
            //Array.Resize<ACTCTriangle>(ref edge.Triangles, edge.Triangles.Length - 1);
            //edge.TriangleCount--;

            return ACTC_var.NO_ERROR;
        }
        #endregion

        #region Edge Mapping
        static ACTC_var mapVertexEdge(ACTCVertex v1, ACTCVertex v2, out ACTCEdge edge)
        {
            uint i;
            ACTCEdge tmp = new ACTCEdge();

            for (i = 0; i < v1.Edges.Length; i++)
                if (v1.Edges[i].V2 == v2) 
                {
	                v1.Edges[i].Count++;
	                break;
	            }

            if (i == v1.Edges.Length) 
            {
	            tmp.V2 = v2;
	            tmp.Count = 1;
                tmp.Triangles = new ACTCTriangle[0];

                //Add the edge to the end of the first vertex edge array.
                if (v1.Edges == null)
                    v1.Edges = new ACTCEdge[0];

                Array.Resize<ACTCEdge>(ref v1.Edges, v1.Edges.Length + 1);
                v1.Edges[v1.Edges.Length - 1] = tmp;
            }

            edge = v1.Edges[i];

            return ACTC_var.NO_ERROR;
        }

        static ACTC_var unmapVertexEdge(ref ACTCData tc, ref ACTCVertex v1, ACTCVertex v2)
        {
            int i;

            for (i = 0; i < v1.Edges.Length; i++)
	            if (v1.Edges[i].V2 == v2)
	                break;

            if (i == v1.Edges.Length) 
            {
	            //(int)ACTC_.DEBUG(
	            //    fprintf(stderr, "ACTC::unmapVertexEdge : Couldn't find edge %d,%d"
	            //        " from vertex in order to unmap it?!?\n", v1.V, v2.V);
	            //    abortWithOptionalDump(tc);
	            //)
	            return tc.Error = ACTC_var.DATABASE_CORRUPT;
            }

            v1.Edges[i].Count--;
            if (v1.Edges[i].Count == 0) 
            {
                //if (v1.Edges[i].Triangles != null)
                //v1.Edges[i].Triangles = new ACTCTriangle[0];
                //free(v1.Edges[i].Triangles);
	            v1.Edges[i] = v1.Edges[v1.Edges.Length - 1];
	            //v1.Edges.Length--;
            }

            return ACTC_var.NO_ERROR;
        }
        #endregion

        static ACTC_var actcAddTriangle(ref ACTCData tc, uint v1, uint v2, uint v3)
        {
            ACTCVertex vertexRec1;
            ACTCVertex vertexRec2;
            ACTCVertex vertexRec3;

            ACTCEdge edge12;
            ACTCEdge edge23;
            ACTCEdge edge31;

            if (tc.IsOutputting != 0) 
            {
	            //(int)ACTC_.DEBUG(fprintf(stderr, "actcAddTriangle : inside "
	            //    "BeginOutput/EndOutput\n");)
	            return tc.Error = ACTC_var.IDLE;
            }
            if (tc.IsInputting == 0) 
            {
	            //(int)ACTC_.DEBUG(fprintf(stderr, "actcAddTriangle : outside "
	            //    "BeginInput/EndInput\n");)
	            return tc.Error = ACTC_var.DURING_INPUT;
            }

            if (incVertexValence(ref tc, v1, out vertexRec1) != ACTC_var.NO_ERROR) goto returnError1;
            if (incVertexValence(ref tc, v2, out vertexRec2) != ACTC_var.NO_ERROR) goto free1;
            if (incVertexValence(ref tc, v3, out vertexRec3) != ACTC_var.NO_ERROR) goto free2;

            if (mapVertexEdge(vertexRec1, vertexRec2, out edge12) != ACTC_var.NO_ERROR) goto free3;
            if (mapVertexEdge(vertexRec2, vertexRec3, out edge23) != ACTC_var.NO_ERROR) goto free4;
            if (mapVertexEdge(vertexRec3, vertexRec1, out edge31) != ACTC_var.NO_ERROR) goto free5;

            if (mapEdgeTriangle(ref edge12, vertexRec3) != ACTC_var.NO_ERROR) goto free6;
            if (mapEdgeTriangle(ref edge23, vertexRec1) != ACTC_var.NO_ERROR) goto free7;
            if (mapEdgeTriangle(ref edge31, vertexRec2) != ACTC_var.NO_ERROR) goto free8;

            return ACTC_var.NO_ERROR;

            /*
             * XXX Unfortunately, while backing out during the following
             * statements, we might encounter errors in the database which
             * will not get returned properly to the caller; I take heart in
             * the fact that if such an error occurs, TC is just a moment from
             * core dumping anyway. XXX grantham 20000615
             */

            free8: unmapEdgeTriangle(ref tc, ref edge23, vertexRec1);
            free7: unmapEdgeTriangle(ref tc, ref edge12, vertexRec3);
            free6: unmapVertexEdge(ref tc, ref vertexRec3, vertexRec1);
            free5: unmapVertexEdge(ref tc, ref vertexRec2, vertexRec3);
            free4: unmapVertexEdge(ref tc, ref vertexRec1, vertexRec2);
            free3: decVertexValence(ref tc, ref vertexRec3);
            free2: decVertexValence(ref tc, ref vertexRec2);
            free1: decVertexValence(ref tc, ref vertexRec1);
            returnError1: return tc.Error;
        }

        static ACTC_var actcStartNextPrim(ref ACTCData tc, out int v1Return, out int v2Return)
        {
            ACTCVertex v1 = null;
            ACTCVertex v2 = null;
            ACTC_var findResult;
            v1Return = v2Return = -1;

            if (tc.IsInputting != 0) 
            {
	            //(int)ACTC_.DEBUG(fprintf(stderr, "actcStartNextPrim : within "
	            //    "BeginInput/EndInput\n");)
	            return tc.Error = ACTC_var.DURING_INPUT;
            }
            if (tc.IsOutputting == 0) 
            {
	            //(int)ACTC_.DEBUG(fprintf(stderr, "actcStartNextPrim : outside "
	            //    "BeginOutput/EndOutput\n");)
	            return tc.Error = ACTC_var.IDLE;
            }

            findResult = findNextFanVertex(ref tc, ref v1);
            if (findResult == ACTC_var.NO_ERROR)
	            tc.PrimType = ACTC_var.PRIM_FAN;
            else if (findResult != ACTC_var.NO_MATCHING_VERT) 
            {
	            //(int)ACTC_.DEBUG(fprintf(stderr, "actcStartNextPrim : internal "
	            //    "error finding next appropriate vertex\n");)
	            return tc.Error = findResult;
            } 
            else 
            {
	            findResult = findNextStripVertex(ref tc, ref v1);
	            if (findResult != ACTC_var.NO_ERROR && findResult != ACTC_var.NO_MATCHING_VERT) 
                {
	                //(int)ACTC_.DEBUG(fprintf(stderr, "actcStartNextPrim : internal "
		            //"error finding next appropriate vertex\n");)
	                return tc.Error = findResult;
	            }
	            tc.PrimType = ACTC_var.PRIM_STRIP;
            }

            if (findResult == ACTC_var.NO_MATCHING_VERT) 
            {
	            v1Return = -1;
	            v2Return = -1;
	            return tc.Error = ACTC_var.DATABASE_EMPTY;
            }

            v2 = v1.Edges[0].V2;

            tc.CurWindOrder = ACTC_var.FWD_ORDER;
            tc.VerticesSoFar = 2;

            tc.V1 = v1;
            tc.V2 = v2;

            v1Return = (int)v1.V;
            v2Return = (int)v2.V;

            //(int)ACTC_.INFO(printf("starting with edge %u, %u\n", tc.V1.V, tc.V2.V);)

            return tc.PrimType;
        }

        static ACTC_var findEdge(ACTCVertex v1, ACTCVertex v2, out ACTCEdge edge)
        {
            int i;

            for (i = 0; i < v1.Edges.Length; i++)
                if (v1.Edges[i].V2 == v2) 
                {
	                edge = v1.Edges[i];
                    return ACTC_var.TRUE;
	            }
            edge = null;
            return ACTC_var.FALSE;
        }

        static ACTC_var unmapEdgeTriangleByVerts(ref ACTCData tc, ACTCVertex v1, ACTCVertex v2, ACTCVertex v3)
        {
            ACTCEdge e;

            ACTC_CHECK(findEdge(v1, v2, out e));
            if (e != null)
            unmapEdgeTriangle(ref tc, ref e, v3);
            return ACTC_var.NO_ERROR;
        }

        static ACTC_var actcGetNextVert(ref ACTCData tc, out int vertReturn)
        {
            ACTCEdge edge;
            int wasEdgeFound = 0;
            ACTCVertex thirdVertex;
            int wasFoundReversed;
            vertReturn = -1;

            if (tc.IsInputting != 0) 
            {
	            //(int)ACTC_.DEBUG(fprintf(stderr, "actcGetNextVert : within BeginInput/"
	            //    "EndInput\n");)
	            return tc.Error = ACTC_var.DURING_INPUT;
            }
            if (tc.IsOutputting == 0) 
            {
	            //(int)ACTC_.DEBUG(fprintf(stderr, "actcGetNextVert : outside BeginOutput/"
	            //    "EndOutput\n");)
	            return tc.Error = ACTC_var.IDLE;
            }
            if (tc.PrimType == ACTC_var.NULL) 
            {
	            //(int)ACTC_.DEBUG(fprintf(stderr, "actcGetNextVert : Asked for next vertex "
	            //    "without a primitive (got last\n    vertex already?)\n");)
	            return tc.Error = ACTC_var.INVALID_VALUE;
            }

            if (tc.VerticesSoFar >= tc.MaxPrimVerts) 
            {
                tc.PrimType = ACTC_var.NULL;
	            return tc.Error = ACTC_var.PRIM_COMPLETE;
            }

            if (tc.V1 == null || tc.V2 == null) 
            {
                tc.PrimType = ACTC_var.NULL;
	            return tc.Error = ACTC_var.PRIM_COMPLETE;
            }

            //(int)ACTC_.INFO(printf("looking for edge %u, %u\n", tc.V1.V, tc.V2.V);)

            wasFoundReversed = 0;

            if (findEdge(tc.V1, tc.V2, out edge) != 0) 
	            wasEdgeFound = 1;
            else if (tc.HonorWinding == 0) 
            {
	            wasFoundReversed = 1;
	            if (findEdge(tc.V2, tc.V1, out edge) != 0)
	                wasEdgeFound = 1;
            }

            if (wasEdgeFound == 0) 
            {
                tc.PrimType = ACTC_var.NULL;
	            return tc.Error = ACTC_var.PRIM_COMPLETE;
            }

            thirdVertex = edge.Triangles[edge.Triangles.Length - 1].FinalVert;

            //(int)ACTC_.INFO(printf("third vertex = %u\n", thirdVertex.V);)
            vertReturn = (int)thirdVertex.V;

            if (wasFoundReversed != 0) 
            {
	            ACTC_CHECK(unmapEdgeTriangle(ref tc, ref edge, thirdVertex));
	            ACTC_CHECK(unmapEdgeTriangleByVerts(ref tc, tc.V1, thirdVertex, tc.V2));
	            ACTC_CHECK(unmapEdgeTriangleByVerts(ref  tc, thirdVertex, tc.V2, tc.V1));
	            ACTC_CHECK(unmapVertexEdge(ref tc, ref tc.V2, tc.V1));
	            ACTC_CHECK(unmapVertexEdge(ref tc, ref tc.V1, thirdVertex));
	            ACTC_CHECK(unmapVertexEdge(ref tc, ref thirdVertex, tc.V2));
            }
            else
            {
	            ACTC_CHECK(unmapEdgeTriangle(ref tc, ref edge, thirdVertex));
	            ACTC_CHECK(unmapEdgeTriangleByVerts(ref tc, tc.V2, thirdVertex, tc.V1));
	            ACTC_CHECK(unmapEdgeTriangleByVerts(ref tc, thirdVertex, tc.V1, tc.V2));
	            ACTC_CHECK(unmapVertexEdge(ref tc, ref tc.V1, tc.V2));
	            ACTC_CHECK(unmapVertexEdge(ref tc, ref tc.V2, thirdVertex));
	            ACTC_CHECK(unmapVertexEdge(ref tc, ref thirdVertex, tc.V1));
            }
            ACTC_CHECK(decVertexValence(ref tc, ref tc.V1));
            ACTC_CHECK(decVertexValence(ref tc, ref tc.V2));
            ACTC_CHECK(decVertexValence(ref tc, ref thirdVertex));

            if (tc.PrimType == ACTC_var.PRIM_FAN) 
                tc.V2 = thirdVertex;
            else /* PRIM_STRIP */ 
            {
	            if (tc.CurWindOrder == ACTC_var.FWD_ORDER)
	                tc.V1 = thirdVertex;
	            else
	                tc.V2 = thirdVertex;
                tc.CurWindOrder = 1 - tc.CurWindOrder;
            }

            tc.VerticesSoFar++;
            return ACTC_var.NO_ERROR;
        }
    }
}