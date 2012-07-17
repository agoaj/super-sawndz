using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BrawlLib.Modeling
{
    public static unsafe partial class TriangleConverter
    {
        public enum ACTC_var : int
        {
            NO_ERROR = 0,

            ALLOC_FAILED = -0x2000,
            DURING_INPUT = -0x2001,
            DURING_OUTPUT = -0x2002,
            IDLE = -0x2003,
            INVALID_VALUE = -0x2004,
            DATABASE_EMPTY = -0x2005,
            DATABASE_CORRUPT = -0x2006,
            PRIM_COMPLETE = -0x2007,

            OUT_MIN_FAN_VERTS = 0x1000,
            OUT_HONOR_WINDING = 0x1001,
            OUT_MAX_PRIM_VERTS = 0x1004,
            IN_MIN_VERT = 0x1005,
            IN_MAX_VERT = 0x1006,
            IN_MAX_VERT_SHARING = 0x1007,
            IN_MAX_EDGE_SHARING = 0x1008,
            MINOR_VERSION = 0x1009,
            MAJOR_VERSION = 0x1010,

            PRIM_FAN = 0x2000,
            PRIM_STRIP = 0x2001,

            NULL = -1,

            TRUE = 1,
            FALSE = 0,

            NO_MATCHING_VERT = -0x3000,
            FWD_ORDER = 0,
            REV_ORDER = 1,
        }

        public const int LEVEL1COUNT = 16384;
        public const int LEVEL2COUNT = 1024;
        public const int LEVEL3COUNT = 256;

        public class TableLevel3
        {
            public int EntryCount;
            public ACTCVertex[] Table = new ACTCVertex[256]; //256
            public bool[] IsSet = new bool[256];
        }

        public class TableLevel2
        {
            public int EntryCount;
            public TableLevel3[] Table = new TableLevel3[1024]; //1024
        }

        public class TableRoot
        {
            public uint EntryCount;
            public uint TotalEntryCount;
            public uint TotalAllocatedBytes;
            public int EmptyEntryCount;
            public TableLevel2[] Table = new TableLevel2[16384]; //16384
        }

        public class TableIterator
        {
            public int i1, i2, i3;
            public uint i;
            public TableLevel3 CurLevel3;
            public int CheckLevel1, CheckLevel2;
        }

        public static int tableRetrieve(uint a, TableRoot table, out ACTCVertex refe)
        {
            int i1 = (int)a / (LEVEL2COUNT * LEVEL3COUNT);
            int i2 = ((int)a / LEVEL3COUNT) % LEVEL2COUNT;
            int i3 = (int)a % LEVEL3COUNT;
            refe = null;

            if (table.Table[i1] == null)
                return 0;

            if (table.Table[i1].Table[i2] == null)
                return 0;

            if (table.Table[i1].Table[i2].IsSet[i3] == false)
                return 0;

            refe = table.Table[i1].Table[i2].Table[i3];
            return 1;
        }

        public static int tableInsert(uint a, ref TableRoot table, ACTCVertex refe)
        {
            int i1 = (int)a / (LEVEL2COUNT * LEVEL3COUNT);
            int i2 = ((int)a / LEVEL3COUNT) % LEVEL2COUNT;
            int i3 = (int)a % LEVEL3COUNT;

            if (table.Table[i1] == null)
            {
                ////chartedSetLabel("table level 2");
                table.Table[i1] = new TableLevel2();
                //table.TotalAllocatedBytes += (uint)sizeof(TableLevel2);
                if (table.Table[i1] == null)
                    return 0;
            }

            if (table.Table[i1].Table[i2] == null)
            {
                ////chartedSetLabel("table level 3");
                table.Table[i1].Table[i2] = new TableLevel3();
                //table.TotalAllocatedBytes += (uint)sizeof(TableLevel3);
                if (table.Table[i1].Table[i2] == null)
                    return 0;

                table.Table[i1].EntryCount++;
                table.TotalEntryCount += LEVEL3COUNT;
                table.EmptyEntryCount += LEVEL3COUNT;
            }

            if (table.Table[i1].Table[i2].IsSet[i3] == false)
            {
                table.Table[i1].Table[i2].EntryCount++;
                table.EmptyEntryCount--;
                table.Table[i1].Table[i2].IsSet[i3] = true;
            }

            table.Table[i1].Table[i2].Table[i3] = refe;
            return 1;
        }

        public static int tableRemove(uint a, ref TableRoot table, ref ACTCVertex wasref)
        {
            int i1 = (int)a / (LEVEL2COUNT * LEVEL3COUNT);
            int i2 = ((int)a / LEVEL3COUNT) % LEVEL2COUNT;
            int i3 = (int)a % LEVEL3COUNT;

            if (table.Table[i1] == null)
                return 0;

            if (table.Table[i1].Table[i2] == null)
                return 0;

            if (table.Table[i1].Table[i2].IsSet[i3] == false)
                return 0;

            if (wasref != null)
                wasref = table.Table[i1].Table[i2].Table[i3];

            table.Table[i1].Table[i2].IsSet[i3] = false;
            table.EmptyEntryCount++;

            if (--table.Table[i1].Table[i2].EntryCount == 0)
            {
                table.EmptyEntryCount -= LEVEL3COUNT;
                table.TotalEntryCount -= LEVEL3COUNT;

                //free(table.Table[i1].Table[i2]);

                //table.TotalAllocatedBytes -= (uint)sizeof(TableLevel3);
                table.Table[i1].Table[i2] = null;

                if (--table.Table[i1].EntryCount == 0)
                {
                    //table.TotalAllocatedBytes -= (uint)sizeof(TableLevel2);
                    //free(table.Table[i1]);
                    table.Table[i1] = null;
                }
            }
            return 1;
        }

        public static void tableResetIterator(ref TableIterator ti)
        {
            ti.i1 = 0;
            ti.i2 = 0;
            ti.i3 = 0;
            ti.i = 0;
            ti.CheckLevel1 = 1;
            ti.CheckLevel2 = 1;
        }

        public static int tableIterate(TableRoot table, ref TableIterator ti, ref uint* i, ref ACTCVertex refe)
        {
            int done;

            done = 0;
            while (ti.i1 < LEVEL1COUNT)
            {
                if (ti.CheckLevel1 == 1 && table.Table[ti.i1] == null)
                {
                    ti.i += LEVEL2COUNT * LEVEL3COUNT;
                    ti.i1++;
                    continue;
                }
                else
                    ti.CheckLevel1 = 0;

                if (ti.CheckLevel2 == 1 && table.Table[ti.i1].Table[ti.i2] == null)
                {
                    ti.i += LEVEL3COUNT;
                    if (++ti.i2 >= LEVEL2COUNT)
                    {
                        ti.i2 = 0;
                        ti.i1++;
                        ti.CheckLevel1 = 1;
                    }
                    continue;
                }
                else
                    ti.CheckLevel2 = 0;

                if (ti.i3 == 0)
                    ti.CurLevel3 = table.Table[ti.i1].Table[ti.i2];

                if (ti.CurLevel3.IsSet[ti.i3] != false)
                {
                    if (refe != null)
                        refe = ti.CurLevel3.Table[ti.i3];
                    if (i != null)
                        *i = ti.i;
                    done = 1;
                }
                ti.i++;
                if (++ti.i3 >= LEVEL3COUNT)
                {
                    ti.i3 = 0;
                    ti.CheckLevel2 = 1;
                    if (++ti.i2 >= LEVEL2COUNT)
                    {
                        ti.i2 = 0;
                        ti.i1++;
                        ti.CheckLevel1 = 1;
                    }
                }
                if (done == 1)
                    return 1;
            }
            return 0;
        }

        public static void tableDelete(TableRoot table)
        {
            int i1, i2, i3;

            for (i1 = 0; i1 < LEVEL1COUNT; i1++)
            {
                if (table.Table[i1] != null)
                {
                    for (i2 = 0; i2 < LEVEL2COUNT; i2++)
                    {
                        if (table.Table[i1].Table[i2] != null)
                        {
                            for (i3 = 0; i3 < LEVEL3COUNT; i3++)
                            {
                                if (table.Table[i1].Table[i2].IsSet[i3] != false)
                                {
                                    table.Table[i1].Table[i2].IsSet[i3] = false;
                                    //table.Table[i1].Table[i2].Table[i3] = null;
                                }
                            }
                            //free(table.Table[i1].Table[i2]);
                        }
                    }
                    //free(table.Table[i1]);
                    table.Table[i1] = null;
                }
            }
            table.TotalEntryCount = 0;
            table.EmptyEntryCount = 0;
            //table.TotalAllocatedBytes = 0;

            table = new TableRoot();
        }

        //public static void tableGetStats(TableRoot table, int totalBytes, int emptyCount, int totalCount)
        //{
        //    if (emptyCount != -1)
        //        emptyCount = table.EmptyEntryCount;
        //    if (totalCount != -1)
        //        totalCount = (int)table.TotalEntryCount;
        //    if (totalBytes != -1)
        //        totalBytes = (int)table.TotalAllocatedBytes;
        //}

        ///* "table.c" ENDSNIPPET */

        //#if !defined(MEM_CHART)
        //#define //chartedSetLabel(a)
        //#endif

        //#if defined(DEBUG)
        //#define ACTC_DEBUG(a) a
        //#else
        //#define ACTC_DEBUG(a)
        //#endif

        //#if defined(INFO)
        //#define ACTC_INFO(a) a
        //#else
        //#define ACTC_INFO(a)
        //#endif

        public static ACTC_var ACTC_CHECK(ACTC_var a)
        {
            ACTC_var theErrorNow;
            theErrorNow = a;
            if (theErrorNow < 0)
                MessageBox.Show(a.ToString());
            return theErrorNow;
        }

        public class ACTCTriangle
        {
            public ACTCVertex FinalVert;
        }

        public class ACTCEdge
        {
            public ACTCVertex V2;
            public int Count;
            //public int Triangles.Length;
            public ACTCTriangle[] Triangles;
        }

        public class ACTCVertex
        {
            public uint V;
            public int Count;
            public ACTCVertex[] PointsToMe;
            public ACTCVertex Next;
            //public int Edges.Length;
            public ACTCEdge[] Edges;
        }

        /* private tokens */
        public const int ACTC_NO_MATCHING_VERT = -0x3000;
        public const int ACTC_FWD_ORDER = 0;
        public const int ACTC_REV_ORDER = 1;
        public const int MAX_STATIC_VERTS = 10000000; /* buh? */

        public struct ACTCData
        {
            /* vertex and edge database */
            public int VertexCount;
            public TableRoot Vertices;
            public TableIterator VertexIterator;
            public int CurMaxVertValence;
            public int CurMinVertValence;
            public ACTCVertex[] VertexBins;

            /* alternate vertex array if range small enough */
            public ACTCVertex[] StaticVerts;
            public int UsingStaticVerts;
            public uint VertRange;

            /* During consolidation */
            public ACTC_var CurWindOrder;
            public ACTC_var PrimType;
            public ACTCVertex V1;
            public ACTCVertex V2;
            public int VerticesSoFar;

            /* Error and state handling */
            public int IsInputting;
            public int IsOutputting;
            public ACTC_var Error;

            /* actcParam-settable parameters */
            public uint MinInputVert;
            public uint MaxInputVert;
            public int MaxVertShare;
            public int MaxEdgeShare;
            public int MinFanVerts;
            public int MaxPrimVerts;
            public int HonorWinding;
        };
    }
}