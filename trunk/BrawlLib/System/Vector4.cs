using System;
using System.Runtime.InteropServices;

namespace System
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Vector4
    {
        public float _x, _y, _z, _w;

        public Vector4(float x, float y, float z, float w) { this._x = x; this._y = y; this._z = z; this._w = w; }
        public Vector4(float s) { _x = s; _y = s; _z = s; _w = 1; }

        public float X { get { return _x; } set { _x = value; } }
        public float Y { get { return _y; } set { _y = value; } }
        public float Z { get { return _z; } set { _z = value; } }
        public float W { get { return _w; } set { _w = value; } }

        //public static explicit operator Vector3(Vector4 v) { return new Vector3(v._x / v._w, v._y / v._w, v._z / v._w); }
        public static explicit operator Vector4(Vector3 v) { return new Vector4(v._x, v._y, v._z, 1.0f); }

        public static Vector4 operator *(Vector4 v, float f) { return new Vector4(v._x * f, v._y * f, v._z * f, v._w * f); }
        public static Vector4 operator /(Vector4 v, float f) { return new Vector4(v._x / f, v._y / f, v._z / f, v._w / f); }
        public static Vector4 operator -(Vector4 v1, Vector4 v2) { return new Vector4(v1._x - v2._x, v1._y - v2._y, v1._z - v2._z, v1._w - v2._w); }
        public static Vector4 operator +(Vector4 v1, Vector4 v2) { return new Vector4(v1._x + v2._x, v1._y + v2._y, v1._z + v2._z, v1._w + v2._w); }

        public static bool operator ==(Vector4 v1, Vector4 v2) { return (v1._x == v2._x) && (v1._y == v2._y) && (v1._z == v2._z) && (v1._w == v2._w); }
        public static bool operator !=(Vector4 v1, Vector4 v2) { return (v1._x != v2._x) || (v1._y != v2._y) || (v1._z != v2._z) || (v1._w != v2._w); }

        public float Length() { return (float)Math.Sqrt(Dot()); }
        public float Dot() { return _x * _x + _y * _y + _z * _z + _w * _w; }
        public float Dot(Vector4 v) { return _x * v._x + _y * v._y + _z * v._z + _w * v._w; }
        public Vector4 Normalize() { return this * (1.0f / Length()); }

        public float Dot3() { return _x * _x + _y * _y + _z * _z; }
        public float Dot3(Vector4 v) { return _x * v._x + _y * v._y + _z * v._z; }
        public float Length3() { return (float)Math.Sqrt(Dot3()); }
        public Vector4 Normalize3()
        {
            float scale = 1.0f / Length3();
            return new Vector4(_x * scale, _y * scale, _z * scale, _w);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector4)
                return this == (Vector4)obj;
            return false;
        }

        public override int GetHashCode()
        {
            fixed (Vector4* p = &this)
            {
                int* p2 = (int*)p;
                return p2[0] ^ p2[1] ^ p2[2] ^ p2[3];
            }
        }

        public unsafe float this[int index]
        {
            get { fixed (Vector4* p = &this) return ((float*)p)[index]; }
            set { fixed (Vector4* p = &this) ((float*)p)[index] = value; }
        }

        public static Vector4 UnitX = new Vector4(1, 0, 0, 0);
        public static Vector4 UnitY = new Vector4(0, 1, 0, 0);
        public static Vector4 UnitZ = new Vector4(0, 0, 1, 0);
        public static Vector4 UnitW = new Vector4(0, 0, 0, 1);
        public static Vector4 Zero = new Vector4(0, 0, 0, 0);
        public static readonly Vector4 One = new Vector4(1, 1, 1, 1);
    }
}