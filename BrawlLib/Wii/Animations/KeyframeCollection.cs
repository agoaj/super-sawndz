using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using BrawlLib.SSBBTypes;

namespace BrawlLib.Wii.Animations
{
    public enum KeyFrameMode
    {
        ScaleX = 0x10,
        ScaleY = 0x11,
        ScaleZ = 0x12,
        RotX = 0x13,
        RotY = 0x14,
        RotZ = 0x15,
        TransX = 0x16,
        TransY = 0x17,
        TransZ = 0x18,
        ScaleXYZ = 0x30,
        RotXYZ = 0x33,
        TransXYZ = 0x36,
        All = 0x90
    }

    public unsafe class KeyframeCollection
    {
        internal KeyframeEntry[] _keyRoots = new KeyframeEntry[9]{
        //Scale
        new KeyframeEntry(-1, 1.0f), //X - Used by all (SHP0 == %)
        new KeyframeEntry(-1, 1.0f), //Y - Not used by SHP0
        new KeyframeEntry(-1, 1.0f), //Z - CHR0s only
        //Rotation
        new KeyframeEntry(-1, 0.0f), //X - Not used by SHP0
        new KeyframeEntry(-1, 0.0f), //Y - CHR0s only
        new KeyframeEntry(-1, 0.0f), //Z - CHR0s only
        //Translation
        new KeyframeEntry(-1, 0.0f), //X - Not used by SHP0
        new KeyframeEntry(-1, 0.0f), //Y - Not used by SHP0
        new KeyframeEntry(-1, 0.0f)};//Z - CHR0s only

        internal int[] _keyCounts = new int[9];

        internal AnimationCode _evalCode;
        internal SRT0Code _texEvalCode;

        public int this[KeyFrameMode mode] { get { return _keyCounts[(int)mode]; } }

        internal int _frameLimit;
        public int FrameLimit
        {
            get { return _frameLimit; }
            set
            {
                _frameLimit = value;
                for (int i = 0; i < 9; i++)
                {
                    KeyframeEntry root = _keyRoots[i];
                    while (root._prev._index >= value)
                    {
                        root._prev.Remove();
                        _keyCounts[i]--;
                    }
                }
            }
        }
        
        internal bool _linearRot;
        public bool LinearRotation { get { return _linearRot; } set { _linearRot = value; } }

        public float this[KeyFrameMode mode, int index]
        {
            get { return GetFrameValue(mode, index); }
            set { SetFrameValue(mode, index, value); }
        }

        public KeyframeCollection(int limit) { _frameLimit = limit; }

        private const float _cleanDistance = 0.00001f;
        public void Clean()
        {
            int flag, res;
            KeyframeEntry entry, root;
            for (int i = 0; i < 9; i++)
            {
                root = _keyRoots[i];

                //Eliminate redundant values
                for (entry = root._next._next; entry != root; entry = entry._next)
                {
                    flag = res = 0;

                    if (entry._prev == root)
                    {
                        if (entry._next != root)
                            flag = 1;
                    }
                    else if (entry._next == root)
                        flag = 2;
                    else
                        flag = 3;

                    if((flag & 1) != 0)
                        res |= (Math.Abs(entry._next._value - entry._value) <= _cleanDistance) ? 1 : 0;
                    
                    if((flag & 2) != 0)
                        res |= (Math.Abs(entry._prev._value - entry._value) <= _cleanDistance) ? 2 : 0;

                    if ((flag == res) && (res != 0))
                    {
                        entry = entry._prev;
                        entry._next.Remove();
                        _keyCounts[i]--;
                    }
                }
            }
        }

        public KeyframeEntry GetKeyframe(KeyFrameMode mode, int index)
        {
            KeyframeEntry entry, root = _keyRoots[(int)mode & 0xF];
            for (entry = root._next; (entry != root) && (entry._index < index); entry = entry._next) ;
            if (entry._index == index)
                return entry;
            return null;
        }
        public float GetFrameValue(KeyFrameMode mode, int index)
        {
            KeyframeEntry entry, root = _keyRoots[(int)mode & 0xF];

            if (index >= root._prev._index)
                return root._prev._value;
            if (index <= root._next._index)
                return root._next._value;

            //Find the entry just before the specified index
            for (entry = root._next; //Get the first entry
                (entry != root) && //Make sure it's not the root
                (entry._index < index);  //Its index must be less than the current index
                entry = entry._next) //Get the next entry
                if (entry._index == index) //The index is a keyframe
                    return entry._value; //Return the value of the keyframe.

            //There was no keyframe... interpolate!
            return entry._prev.Interpolate(index - entry._prev._index, _linearRot);
        }
        public KeyframeEntry SetFrameValue(KeyFrameMode mode, int index, float value)
        {
            KeyframeEntry entry = null, root;
            for (int x = (int)mode & 0xF, y = x + ((int)mode >> 4); x < y; x++)
            {
                root = _keyRoots[x];

                if ((root._prev == root) || (root._prev._index < index))
                    entry = root;
                else
                    for (entry = root._next; (entry != root) && (entry._index <= index); entry = entry._next) ;

                entry = entry._prev;
                if (entry._index != index)
                {
                    _keyCounts[x]++;
                    entry.InsertAfter(entry = new KeyframeEntry(index, value));
                }
                else
                    entry._value = value;
            }
            return entry;
        }

        public AnimationFrame GetFullFrame(int index)
        {
            AnimationFrame frame;
            float* dPtr = (float*)&frame;
            for (int x = 0x10; x < 0x19; x++)
                *dPtr++ = GetFrameValue((KeyFrameMode)x, index);
            return frame;
        }

        public KeyframeEntry Remove(KeyFrameMode mode, int index)
        {
            KeyframeEntry entry = null, root;
            for (int x = (int)mode & 0xF, y = x + ((int)mode >> 4); x < y; x++)
            {
                root = _keyRoots[x];

                for (entry = root._next; (entry != root) && (entry._index < index); entry = entry._next) ;

                if (entry._index == index)
                {
                    entry.Remove();
                    _keyCounts[x]--;
                }
                else
                    entry = null;
            }
            return entry;
        }

        public void Insert(KeyFrameMode mode, int index)
        {
            KeyframeEntry entry = null, root;
            for (int x = (int)mode & 0xF, y = x + ((int)mode >> 4); x < y; x++)
            {
                root = _keyRoots[x];
                for (entry = root._prev; (entry != root) && (entry._index >= index); entry = entry._prev)
                    if (++entry._index >= _frameLimit)
                    {
                        entry = entry._next;
                        entry._prev.Remove();
                        _keyCounts[x]--;
                    }
            }
        }

        public void Delete(KeyFrameMode mode, int index)
        {
            KeyframeEntry entry = null, root;
            for (int x = (int)mode & 0xF, y = x + ((int)mode >> 4); x < y; x++)
            {
                root = _keyRoots[x];
                for (entry = root._prev; (entry != root) && (entry._index >= index); entry = entry._prev)
                    if ((entry._index == index) || (--entry._index < 0))
                    {
                        entry = entry._next;
                        entry._prev.Remove();
                        _keyCounts[x]--;
                    }
            }
        }
    }

    public class KeyframeEntry
    {
        public int _index;
        public KeyframeEntry _prev, _next;

        public float _value;
        public float _tangent;

        public KeyframeEntry(int index, float value)
        {
            _index = index;
            _prev = _next = this;
            _value = value;
        }

        public void InsertBefore(KeyframeEntry entry)
        {
            _prev._next = entry;
            entry._prev = _prev;
            entry._next = this;
            _prev = entry;
        }
        public void InsertAfter(KeyframeEntry entry)
        {
            _next._prev = entry;
            entry._next = _next;
            entry._prev = this;
            _next = entry;
        }
        public void Remove()
        {
            _next._prev = _prev;
            _prev._next = _next;
            //_prev = _next = this;
        }

        public float Interpolate(int offset, bool linear)
        {
            if (offset == 0)
                return _value;

            int span = _next._index - _index;
            if (offset == span)
                return _next._value;

            float diff = _next._value - _value;
            if (linear)
                return _value + (diff / span * offset);

            float time = (float)offset / span;
            float inv = time - 1.0f;

            return (offset * inv * ((inv * _tangent) + (time * _next._tangent)))
                + ((time * time) * (3.0f - 2.0f * time) * diff)
                + _value;
        }

        public float Interpolate2(int offset, bool linear)
        {
            if (offset == 0)
                return _value; //The offset is this keyframe

            int span = _next._index - _index;
            if (offset == span)
                return _next._value; //The offset is the next keyframe

            float t = ((float)offset / span);

            float diff = _next._value - _value;
            if (linear)
                return _value + (diff / span * offset);
                //return _value + t * (_next._value - _value);

            return Maths.Bezier(_value, _tangent, _next._tangent, _next._value, t);

        }

        public float Hermite(float t)
        {
            float h1 = (float)(2 * (t * t * t) - 3 * (t * t) + 1);
            float h2 = (float)(-2 * (t * t * t) + 3 * (t * t));
            float h3 = (float)((t * t * t) - 2 * (t * t) + t);
            float h4 = (float)((t * t * t) - (t * t));

            return
                h1 * _value +
                h2 * _next._value +
                h3 * _tangent +
                h4 * _next._tangent;
        }

        public float GenerateTangent()
        {
            float tan = 0.0f;
            if (_prev._index != -1)
                tan += (_value - _prev._value) / (_index - _prev._index);
            if (_next._index != -1)
            {
                tan += (_next._value - _value) / (_next._index - _index);
                if (_prev._index != -1)
                    tan *= 0.5f;
            }

            return _tangent = tan;
        }

        public override string ToString()
        {
            return String.Format("Prev={0}, Next={1}, Value={2}", _prev, _next, _value);
        }
    }
}
