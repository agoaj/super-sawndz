using System;
using System.Collections.Generic;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Modeling;
using System.Windows.Forms;

namespace BrawlLib.Wii.Models
{
    /// <summary>
    /// Managed collection of influences. Only influences with references should be used.
    /// It is up to the implementation to properly manage this collection.
    /// </summary>
    public class InfluenceManager
    {
        internal List<Influence> _influences = new List<Influence>();
        public List<Influence> Influences { get { return _influences; } }

        public Influence AddOrCreate(Influence inf)
        {
            //Search for influence in list. If it exists, return it.
            foreach (Influence i in _influences)
                if (i.Equals(inf))
                    return i;

            //Not found, add it to the list.
            _influences.Add(inf);
            return inf;
        }

        public int Count { get { return _influences.Count; } }
        public int CountPrimary
        {
            get
            {
                int count = 0;
                foreach (Influence i in _influences)
                    if (i.IsPrimaryNode)
                        count++;
                return count;
            }
        }
        public int CountWeighted
        {
            get
            {
                int count = 0;
                foreach (Influence i in _influences)
                    if (i.IsWeighted)
                        count++;
                return count;
            }
        }

        //Increases reference count
        public Influence AddOrCreateInf(Influence inf)
        {
            Influence i = AddOrCreate(inf);
            i._refCount++;
            return i;
        }

        public void Remove(Influence inf)
        {
            for (int i = 0; i < _influences.Count; i++)
                if (object.ReferenceEquals(_influences[i], inf))
                {
                    if (inf._refCount-- <= 0)
                        _influences.RemoveAt(i);
                    break;
                }
        }

        //Get all weighted influences
        public Influence[] GetWeighted()
        {
            List<Influence> list = new List<Influence>(_influences.Count);
            foreach (Influence i in _influences)
                if (i.IsWeighted)
                    list.Add(i);

            return list.ToArray();
        }

        //Remove all influences without references
        public void Clean()
        {
            int i = 0;
            while (i < _influences.Count)
            {
                if (_influences[i]._refCount <= 0)
                    _influences.RemoveAt(i);
                else
                    i++;
            }
        }

        //Sorts influences
        public void Sort()
        {
            _influences.Sort(Influence.Compare);
        }
    }

    public class Influence : IMatrixNode
    {
        public override string ToString()
        {
            return "";
        }

        internal int _refCount;
        internal int _index;
        internal int _permanentID;
        internal Matrix _matrix;
        internal Matrix _invMatrix;
        internal BoneWeight[] _weights;

        public BoneWeight[] Weights { get { return _weights; } }

        public int ReferenceCount { get { return _refCount; } set { _refCount = value; } }
        public int NodeIndex { get { return _index; } }
        public int PermanentID { get { return _permanentID; } }

        public Matrix Matrix { get { return _matrix; } }
        public Matrix InverseBindMatrix { get { return _invMatrix; } }

        public bool IsPrimaryNode { get { return false; } }

        public bool IsWeighted { get { return _weights.Length > 1; } }
        public MDL0BoneNode Bone { get { return _weights[0].Bone; } }

        public Influence(int capacity) { _weights = new BoneWeight[capacity]; }
        public Influence(BoneWeight[] weights) { _weights = weights; }
        public Influence(MDL0BoneNode bone) { _weights = new BoneWeight[] { new BoneWeight(bone) }; }

        public Influence Clone()
        {
            Influence i = new Influence(_weights.Length);
            _weights.CopyTo(i._weights, 0);
            return i;
        }

        public Influence Splice(BoneWeight weight)
        {
            Influence i = new Influence(_weights.Length + 1);
            _weights.CopyTo(i._weights, 0);
            i._weights[_weights.Length] = weight;
            return i;
        }

        public void CalcMatrix()
        {
            if (_weights.Length > 1)
            {
                _matrix = new Matrix();
                foreach (BoneWeight w in _weights)
                    if (w.Bone != null)
                        _matrix += (w.Bone.Matrix * w.Bone.InverseBindMatrix) * w.Weight;
                _invMatrix = _matrix.Inverse();
            }
            else if (_weights.Length == 1)
            {
                if (_weights[0].Bone != null)
                {
                    _matrix = _weights[0].Bone.Matrix;
                    _invMatrix = _weights[0].Bone.InverseBindMatrix;
                }
            }
            else
                _matrix = _invMatrix = Matrix.Identity;

        }

        //public override bool Equals(object obj)
        //{
        //    if (obj is Influence)
        //        return Equals(obj as Influence);
        //    return false;
        //}
        public static int Compare(Influence i1, Influence i2)
        {
            if (i1._weights.Length < i2._weights.Length)
                return -1;
            if (i1._weights.Length > i2._weights.Length)
                return 1;

            if (i1._refCount > i2._refCount)
                return -1;
            if (i1._refCount < i2._refCount)
                return 1;

            return 0;
        }

        public bool Equals(Influence inf)
        {
            bool found;

            if (object.ReferenceEquals(this, inf))
                return true;

            if (_weights.Length != inf._weights.Length)
                return false;

            foreach (BoneWeight w1 in _weights)
            {
                found = false;
                foreach (BoneWeight w2 in inf._weights) { if (w1 == w2) { found = true; break; } }
                if (!found)
                    return false;
            }
            return true;
        }

        public void Merge(Influence inf, float weight)
        {
            _weights[_weights.Length + 1] = new BoneWeight(inf._weights[0].Bone, weight);
        }

        public void CalcBase()
        {
            if (_weights.Length == 1)
            {
                _matrix = _weights[0].Bone.Matrix;
                _invMatrix = _weights[0].Bone.InverseBindMatrix;
            }
            else
                _matrix = _invMatrix = Matrix.Identity;
        }
        public void CalcWeighted()
        {
            if (_weights.Length > 1)
            {
                //Multiply the current matrix by the inverse bind matrix and scale
                _matrix = new Matrix();
                foreach (BoneWeight w in _weights)
                    _matrix += (w.Bone.Matrix * w.Bone.InverseBindMatrix) * w.Weight;
            }
            else
                _matrix = _weights[0].Bone.Matrix;
        }
    }

    public struct BoneWeight
    {
        public override string ToString()
        {
            return Bone.Name + " - Weight: " + Weight;
        }

        public MDL0BoneNode Bone;
        public float Weight;

        public BoneWeight(MDL0BoneNode bone) : this(bone, 1.0f) { }
        public BoneWeight(MDL0BoneNode bone, float weight) { Bone = bone; Weight = weight; }

        public static bool operator ==(BoneWeight b1, BoneWeight b2) { return (b1.Bone == b2.Bone) && (b1.Weight - b2.Weight < 0.0001); }
        public static bool operator !=(BoneWeight b1, BoneWeight b2) { return !(b1 == b2); }
        public override bool Equals(object obj)
        {
            if (obj is BoneWeight)
                return this == (BoneWeight)obj;
            return false;
        }
        public override int GetHashCode() { return base.GetHashCode(); }
    }
}
