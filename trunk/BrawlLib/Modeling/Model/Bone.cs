using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;

namespace BrawlLib.Modeling
{
    public unsafe class Bone : IEnumerable<Bone>
    {
        internal Model _model;

        internal int _index;

        public Matrix FrameMatrix;

        internal Bone _parent, _prev, _next;
        internal Bone _firstChild, _lastChild;
        internal int _childCount;

        public Bone(MDL0Bone* pBone)
        {

        }

        public int ChildTotal
        {
            get
            {
                int total = _childCount;
                if (total > 0)
                    foreach (Bone b in this)
                        total += b.ChildTotal;
                return total;
            }
        }

        public void Remove()
        {
            if (_prev != null)
                _prev._next = _next;
            if (_next != null)
                _next._prev = _prev;

            _prev = _next = null;

            if (_parent != null)
            {
                _parent._childCount--;
                _parent = null;
            }

            if (_model != null)
            {
                List<Bone> list = _model._bones;
                list.RemoveAt(_index);
                for (int i = _index; i < list.Count; i++)
                {
                    list[i]._index -= 1;
                }
            }
        }
        public void InsertBefore(Bone bone)
        {
            bone.Remove();

            if (_parent != null)
            {
                _parent._childCount++;
            }

            if (_model != null)
            {
                List<Bone> list = _model._bones;

                list.Insert(_index, bone);
                for (int i = _index; i < list.Count; i++)
                    list[i]._index = i;
            }
        }
        public void InsertAfter(Bone bone)
        {

        }

        private struct BoneEnumerator : IEnumerator<Bone>
        {
            private Bone _parent, _current;

            public BoneEnumerator(Bone parent) { _parent = parent; _current = null; }

            public Bone Current { get { return _current; } }

            public void Dispose() { _parent = _current = null; }

            object System.Collections.IEnumerator.Current { get { return _current; } }

            public bool MoveNext()
            {
                if (_current == null)
                    if (_parent._firstChild == null)
                        return false;
                    else
                        _current = _parent._firstChild;
                else if (_current._next == null)
                    return false;
                else
                    _current = _current._next;

                return true;
            }

            public void Reset() { _current = null; }

        }

        public IEnumerator<Bone> GetEnumerator() { return new BoneEnumerator(this); }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return new BoneEnumerator(this); }
    }
}
