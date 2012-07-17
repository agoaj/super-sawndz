using System;
using System.Runtime.InteropServices;
using BrawlLib.Wii.Models;
using System.Collections.Generic;
using BrawlLib.Imaging;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.Modeling
{
    public class Facepoint
    {
        public int id = -1;

        public int VertexIndex;
        public Vertex3 Vertex;

        public int NormalIndex;

        public int[] ColorIndex = new int[2];

        public int[] UVIndex = new int[8];

        public int NodeId { get { return Vertex != null && Vertex.Inf != null ? Vertex.Inf.NodeIndex : Node != null ? Node.NodeIndex : -1; } }
        
        public IMatrixNode Node = null;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PrimitiveHeader
    {
        public WiiPrimitiveType Type;
        public bushort Entries;

        public PrimitiveHeader(WiiPrimitiveType type, int entries)
        {
            Type = type;
            Entries = (ushort)entries;
        }

        internal VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr Data { get { return Address + 3; } }
    }

    public class PrimitiveGroup
    {
        //This is the main group of primitives, all using a group of node ids.

        [Browsable(true)]
        public int[] NodeIds { get { return nodeIds.ToArray(); } }

        [Browsable(true)]
        public PrimitiveHeader[] Headers { get { return _headers.ToArray(); } }

        public List<int> nodeIds = new List<int>();

        public override string ToString()
        {
            return "";
        }

        //For imports
        public List<Tristrip> Tristrips = new List<Tristrip>();
        public List<Triangle> Triangles = new List<Triangle>();

        //For existing models
        public List<PrimitiveHeader> _headers = new List<PrimitiveHeader>();
        public List<List<Facepoint>> _points = new List<List<Facepoint>>();
        public List<IMatrixNode> _nodes = new List<IMatrixNode>();
        
        public void AddTriangle(Triangle t)
        {
            Triangles.Add(t);
            if (!nodeIds.Contains(t.x.NodeId))
                nodeIds.Add(t.x.NodeId);
            if (!nodeIds.Contains(t.y.NodeId))
                nodeIds.Add(t.y.NodeId);
            if (!nodeIds.Contains(t.z.NodeId))
                nodeIds.Add(t.z.NodeId);
            t.grouped = true;
        }

        public void AddTristrip(Tristrip t)
        {
            Tristrips.Add(t);
            foreach (Facepoint f in t.points)
                nodeIds.Add(f.NodeId);
            t.grouped = true;
        }

        public bool CanAdd(Triangle t)
        {
            int count = 0;
            if (!nodeIds.Contains(t.x.NodeId))
                count++;
            if (!nodeIds.Contains(t.y.NodeId))
                count++;
            if (!nodeIds.Contains(t.z.NodeId))
                count++;
            if (count + nodeIds.Count <= 10)
            {
                AddTriangle(t);
                return true;
            }
            return false;
        }

        public Tristrip CanAdd(Tristrip t)
        {
            HashSet<int> tempIds = new HashSet<int>();
            Tristrip notAdded = new Tristrip();
            Tristrip added = new Tristrip();
            int range = 0;

            foreach (int x in nodeIds)
                tempIds.Add(x);

            foreach (Facepoint f in t.points)
            {
                tempIds.Add(f.NodeId);
                range++;
                if (tempIds.Count <= 10)
                    continue;
                else goto Next;
            }
        Next:
            //Align to range divisible by 3
            int remainder = 0;
            if ((remainder = range % 3) != 0)
                range -= remainder;

            if (range < 3)
                return null;

            //Seperate tristrip
            for (int i = 0; i < t.points.Count; i++)
                if (i <= range)
                    added.points.Add(t.points[i]);
                else
                    notAdded.points.Add(t.points[i]);

            if (added.points.Count >= 3)
                AddTristrip(added);

            if (notAdded.points.Count >= 3)
                return notAdded;

            return null;
        }

        public bool tryMerge(PrimitiveGroup g2)
        {
            HashSet<int> tempIds = new HashSet<int>();

            foreach (int i in nodeIds)
                tempIds.Add(i);

            foreach (int i in g2.nodeIds)
                tempIds.Add(i);

            if (tempIds.Count <= 10)
            {
                for (int i = 0; i < g2.Triangles.Count; i++)
                    AddTriangle(g2.Triangles[i]);
                for (int i = 0; i < g2.Tristrips.Count; i++)
                    AddTristrip(g2.Tristrips[i]);
                return true;
            }

            return false;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class TriangleGroup
    {
        public PrimitiveHeader Header { get { return new PrimitiveHeader(WiiPrimitiveType.Triangles, Triangles.Count * 3); } }
        public List<Triangle> Triangles = new List<Triangle>();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class TristripGroup
    {
        public PrimitiveHeader Header { get { return new PrimitiveHeader(WiiPrimitiveType.TriangleStrip, Count); } }
        public int Count 
        { 
            get 
            {
                int count = 0;
                foreach (Tristrip t in Tristrips)
                    count += t.points.Count;
                return count;
            } 
        }
        public List<Tristrip> Tristrips;
    }

    public class Tristrip
    {
        public List<Facepoint> points = new List<Facepoint>();
        public bool grouped;
    }

    public class Triangle
    {
        public Facepoint x;
        public int xIndex { get { return x.VertexIndex; } }
        public Facepoint y;
        public int yIndex { get { return y.VertexIndex; } }
        public Facepoint z;
        public int zIndex { get { return z.VertexIndex; } }

        public Facepoint[] values { get { return new Facepoint[] { x, y, z }; } }

        public bool grouped = false;

        public int p1 = -1;
        public int p2 = -1;
        public int remaining = -1;

        public int index1 = -1;
        public int index2 = -1;
        public int remainingIndex = -1;

        public bool TwoPointsMatch(Triangle ext)
        {
            List<int> thisT = new List<int>() { xIndex, yIndex, zIndex };
            List<int> thatT = new List<int>() { ext.xIndex, ext.yIndex, ext.zIndex };

            for (int i = 0; i < 3; i++)
                if ((ext.index1 = thatT.IndexOf(thisT[i])) != -1)
                {
                    //First point match found
                    p1 = i;
                    break;
                }

            for (int i = 0; i < 3; i++)
                if (i != p1 && (ext.index2 = thatT.IndexOf(thisT[i])) != -1)
                {
                    //Second point match found
                    p2 = i;
                    break;
                }

            remaining = 3 - (p1 + p2);
            ext.remainingIndex = 3 - (ext.index1 + ext.index2);

            if (p2 != -1 && p1 != -1 && thisT[p2] < thisT[p1])
            {
                int temp = p1;
                p1 = p2;
                p2 = temp;
            }

            if (ext.index2 != -1 && ext.index1 != -1 && thatT[ext.index2] < thatT[ext.index1])
            {
                int temp = ext.index1;
                ext.index1 = ext.index2;
                ext.index2 = temp;
            }

            return p1 != -1 && p2 != -1 && ext.index1 != -1 && ext.index2 != -1;
        }
    }
}
