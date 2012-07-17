using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace System
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Vector3
    {
        public float _x, _y, _z;

        public float X { get { return _x; } set { _x = value; } }
        public float Y { get { return _y; } set { _y = value; } }
        public float Z { get { return _z; } set { _z = value; } }

        public Vector3(string s)
        {
            Vector3 v = new Vector3();
            char[] delims = new char[] { ',', '(', ')', ' ' };
            string[] arr = s.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 3)
            {
                float.TryParse(arr[0], out v._x);
                float.TryParse(arr[1], out v._y);
                float.TryParse(arr[2], out v._z);
            }
            _x = v._x;
            _y = v._y;
            _z = v._z;
        }
        public Vector3(float x, float y, float z) { _x = x; _y = y; _z = z; }
        public Vector3(float s) { _x = s; _y = s; _z = s; }

        public static explicit operator Vector3(Vector4 v) { return new Vector3(v._x / v._w, v._y / v._w, v._z / v._w); }
        //public static explicit operator Vector4(Vector3 v) { return new Vector4(v._x, v._y, v._z, 1.0f); }

        private const float _colorFactor = 1.0f / 255.0f;
        public static explicit operator Vector3(Color c) { return new Vector3(c.R * _colorFactor, c.G * _colorFactor, c.B * _colorFactor); }
        public static explicit operator Color(Vector3 v) { return Color.FromArgb((int)(v._x / _colorFactor), (int)(v._y / _colorFactor), (int)(v._z / _colorFactor)); }

        public static Vector3 operator -(Vector3 v) { return new Vector3(-v._x, -v._y, -v._z); }
        public static Vector3 operator +(Vector3 v1, Vector3 v2) { return new Vector3(v1._x + v2._x, v1._y + v2._y, v1._z + v2._z); }
        public static Vector3 operator +(Vector3 v1, float f) { return new Vector3(v1._x + f, v1._y + f, v1._z + f); }
        public static Vector3 operator +(float f, Vector3 v1) { return new Vector3(v1._x + f, v1._y + f, v1._z + f); }
        public static Vector3 operator -(Vector3 v1, Vector3 v2) { return new Vector3(v1._x - v2._x, v1._y - v2._y, v1._z - v2._z); }
        public static Vector3 operator -(Vector3 v1, float f) { return new Vector3(v1._x - f, v1._y - f, v1._z - f); }
        public static Vector3 operator *(Vector3 v1, Vector3 v2) { return new Vector3(v1._x * v2._x, v1._y * v2._y, v1._z * v2._z); }
        public static Vector3 operator *(Vector3 v1, float s) { return new Vector3(v1._x * s, v1._y * s, v1._z * s); }
        public static Vector3 operator *(float s, Vector3 v1) { return new Vector3(v1._x * s, v1._y * s, v1._z * s); }
        public static Vector3 operator /(Vector3 v1, Vector3 v2) { return new Vector3(v1._x / v2._x, v1._y / v2._y, v1._z / v2._z); }
        public static Vector3 operator /(Vector3 v1, float s) { return new Vector3(v1._x / s, v1._y / s, v1._z / s); }

        public static bool operator ==(Vector3 v1, Vector3 v2) { return (v1._x == v2._x) && (v1._y == v2._y) && (v1._z == v2._z); }
        public static bool operator !=(Vector3 v1, Vector3 v2) { return (v1._x != v2._x) || (v1._y != v2._y) || (v1._z != v2._z); }

        public void Add(Vector3* v) { _x += v->_x; _y += v->_y; _z += v->_z; }
        public void Add(float v) { _x += v; _y += v; _z += v; }
        public void Sub(Vector3* v) { _x -= v->_x; _y -= v->_y; _z -= v->_z; }
        public void Sub(float v) { _x -= v; _y -= v; _z -= v; }
        public void Multiply(Vector3* v) { _x *= v->_x; _y *= v->_y; _z *= v->_z; }
        public void Multiply(float v) { _x *= v; _y *= v; _z *= v; }

        public static float* Mult(float* v1, float* v2) { v1[0] = v1[0] * v2[0]; v1[1] = v1[1] * v2[1]; v1[2] = v1[2] * v2[2]; return v1; }
        public static float* Mult(float* v1, float v2) { v1[0] = v1[0] * v2; v1[1] = v1[1] * v2; v1[2] = v1[2] * v2; return v1; }
        public static float* Add(float* v1, float* v2) { v1[0] = v1[0] + v2[0]; v1[1] = v1[1] + v2[1]; v1[2] = v1[2] + v2[2]; return v1; }
        public static float* Add(float* v1, float v2) { v1[0] = v1[0] + v2; v1[1] = v1[1] + v2; v1[2] = v1[2] + v2; return v1; }
        public static float* Sub(float* v1, float* v2) { v1[0] = v1[0] - v2[0]; v1[1] = v1[1] - v2[1]; v1[2] = v1[2] - v2[2]; return v1; }
        public static float* Sub(float* v1, float v2) { v1[0] = v1[0] - v2; v1[1] = v1[1] - v2; v1[2] = v1[2] - v2; return v1; }

        //public static float* Mult(float* v1, float* v2) { v1[0] = v1[0] * v2[0]; v1[1] = v1[1] * v2[1]; v1[2] = v1[2] * v2[2]; return v1; }
        //public static float* Mult(float* v1, float v2) { v1[0] *= v2; v1[1] *= v2; v1[2] *= v2; return v1; }
        //public static float* Add(float* v1, float* v2) { v1[0] += v2[0]; v1[1] += v2[1]; v1[2] += v2[2]; return v1; }
        //public static float* Add(float* v1, float v2) { v1[0] += v2; v1[1] += v2; v1[2] += v2; return v1; }
        //public static float* Sub(float* v1, float* v2) { v1[0] -= v2[0]; v1[1] -= v2[1]; v1[2] -= v2[2]; return v1; }
        //public static float* Sub(float* v1, float v2) { v1[0] -= v2; v1[1] -= v2; v1[2] -= v2; return v1; }

        public static float Dot(Vector3 v1, Vector3 v2) { return (v1._x * v2._x) + (v1._y * v2._y) + (v1._z * v2._z); }
        public float Dot(Vector3 v) { return (_x * v._x) + (_y * v._y) + (_z * v._z); }
        public float Dot(Vector3* v) { return (_x * v->_x) + (_y * v->_y) + (_z * v->_z); }
        public float Dot() { return (_x * _x) + (_y * _y) + (_z * _z); }

        public static Vector3 Clamp(Vector3 v1, float min, float max) { v1.Clamp(min, max); return v1; }
        public void Clamp(float min, float max) { this.Max(min); this.Min(max); }

        public static Vector3 Min(Vector3 v1, Vector3 v2) { return new Vector3(Math.Min(v1._x, v2._x), Math.Min(v1._y, v2._y), Math.Min(v1._z, v2._z)); }
        public static Vector3 Min(Vector3 v1, float f) { return new Vector3(Math.Min(v1._x, f), Math.Min(v1._y, f), Math.Min(v1._z, f)); }
        public void Min(Vector3 v) { _x = Math.Min(_x, v._x); _y = Math.Min(_y, v._y); _z = Math.Min(_z, v._z); }
        public void Min(Vector3* v) { if (v->_x < _x) _x = v->_x; if (v->_y < _y) _y = v->_y; if (v->_z < _z) _z = v->_z; }
        public void Min(float f) { _x = Math.Min(_x, f); _y = Math.Min(_y, f); _z = Math.Min(_z, f); }

        public static Vector3 Max(Vector3 v1, Vector3 v2) { return new Vector3(Math.Max(v1._x, v2._x), Math.Max(v1._y, v2._y), Math.Max(v1._z, v2._z)); }
        public static Vector3 Max(Vector3 v1, Vector3* v2) { return new Vector3(Math.Max(v1._x, v2->_x), Math.Max(v1._y, v2->_y), Math.Max(v1._z, v2->_z)); }
        public static Vector3 Max(Vector3 v1, float f) { return new Vector3(Math.Max(v1._x, f), Math.Max(v1._y, f), Math.Max(v1._z, f)); }
        public void Max(Vector3 v) { _x = Math.Max(_x, v._x); _y = Math.Max(_y, v._y); _z = Math.Max(_z, v._z); }
        public void Max(Vector3* v) { if (v->_x > _x) _x = v->_x; if (v->_y > _y) _y = v->_y; if (v->_z > _z) _z = v->_z; }
        public void Max(float f) { _x = Math.Max(_x, f); _y = Math.Max(_y, f); _z = Math.Max(_z, f); }

        public float DistanceTo(Vector3 v) { return (v - this).Dot(); }
        public static Vector3 Lerp(Vector3 v1, Vector3 v2, float median) { return (v1 * (1.0f - median)) + (v2 * median); }
        public static Vector3 Floor(Vector3 v) { return new Vector3((int)v._x, (int)v._y, (int)v._z); }

        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);
        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);
        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 One = new Vector3(1, 1, 1);

        public Vector3 Cross(Vector3 v) { return new Vector3(_y * v._z - v._y * _z, _z * v._x - v._z * _x, _x * v._y - v._x * _y); }
        public static Vector3 Cross(Vector3 v1, Vector3 v2) { return new Vector3(v1._y * v2._z - v2._y * v1._z, v1._z * v2._x - v2._z * v1._x, v1._x * v2._y - v2._x * v1._y); }

        public static Vector3 Truncate(Vector3 v)
        {
            return new Vector3(
                v._x > 0.0f ? (float)Math.Floor(v._x) : (float)Math.Ceiling(v._x),
                v._y > 0.0f ? (float)Math.Floor(v._y) : (float)Math.Ceiling(v._z),
                v._z > 0.0f ? (float)Math.Floor(v._z) : (float)Math.Ceiling(v._z));
        }

        public override string ToString() { return String.Format("({0},{1},{2})", _x, _y, _z); }

        public bool Contained(Vector3 start, Vector3 end, float expansion) { return Contained(this, start, end, expansion); }
        public static unsafe bool Contained(Vector3 point, Vector3 start, Vector3 end, float expansion)
        {
            float* sPtr = (float*)&point;
            float* s1 = (float*)&start, s2 = (float*)&end;
            float* temp;
            for (int i = 0; i < 3; i++)
            {
                if (s1[i] > s2[i])
                { temp = s1; s1 = s2; s2 = temp; }

                if ((sPtr[i] < (s1[i] - expansion)) || (sPtr[i] > (s2[i] + expansion)))
                    return false;
            }
            return true;
        }

        public static Vector3 IntersectZ(Vector3 ray1, Vector3 ray2, float z)
        {
            float a = ray2._z - ray1._z;

            float tanX = (ray1._y - ray2._y) / a;
            float tanY = (ray2._x - ray1._x) / a;

            a = z - ray1._z;
            return new Vector3(tanY * a + ray1._x, -tanX * a + ray1._y, z);
        }

        public float TrueDistance(Vector3 p) { return (float)Math.Sqrt((p - this).Dot()); }
        public float TrueDistance() { return (float)Math.Sqrt(Dot()); }

        public Vector3 Normalize() { return this / TrueDistance(); }
        public Vector3 Normalize(Vector3 origin) { return (this - origin).Normalize(); }

        public Vector3 GetAngles() { return new Vector3(AngleX(), AngleY(), AngleZ()); }
        public Vector3 GetAngles(Vector3 origin) { return (this - origin).GetAngles(); }

        public Vector3 LookatAngles() { return new Vector3((float)Math.Atan2(_y, Math.Sqrt(_x * _x + _z * _z)), (float)Math.Atan2(-_x, -_z), 0.0f); }
        public Vector3 LookatAngles(Vector3 origin) { return (this - origin).LookatAngles(); }

        public float AngleX() { return (float)Math.Atan2(_y, -_z); }
        public float AngleY() { return (float)Math.Atan2(-_z, _x); }
        public float AngleZ() { return (float)Math.Atan2(_y, _x); }

        public override int GetHashCode()
        {
            fixed (Vector3* p = &this)
            {
                int* p2 = (int*)p;
                return p2[0] ^ p2[1] ^ p2[2];
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is Vector3)
                return this == (Vector3)obj;
            return false;
        }

        public unsafe float this[int index]
        {
            get { fixed (Vector3* p = &this) return ((float*)p)[index]; }
            set { fixed (Vector3* p = &this) ((float*)p)[index] = value; }
        }

        public void Morph(Vector3 to, float percent) 
        {
            _x += ((to._x - _x) * percent);
            _y += ((to._y - _y) * percent);
            _z += ((to._z - _z) * percent);
        }

        public Vector3 Interpolate(Vector3 _nextValue, int offset, bool linear, float _tangent, float _nextTangent, int _nextIndex, int _index)
        {
            if (offset == 0)
                return this;

            int span = _nextIndex - _index;
            if (offset == span)
                return _nextValue;

            Vector3 diff = _nextValue - this;
            if (linear)
                return this + (diff / span * offset);

            float time = (float)offset / span;
            float inv = time - 1.0f;

            return (offset * inv * ((inv * _tangent) + (time * _nextTangent)))
                + ((time * time) * (3.0f - 2.0f * time) * diff)
                + this;
        }

        public void FromQuaternion(Quaternion pvec4Quat)
        {
            double x2 = pvec4Quat._x + pvec4Quat._x;
            double y2 = pvec4Quat._y + pvec4Quat._y;
            double z2 = pvec4Quat._z + pvec4Quat._z;
            double xz2 = pvec4Quat._x * z2;
            double wy2 = pvec4Quat._w * y2;
            double temp = -(xz2 - wy2);

            if (temp >= 1.0)
                temp = 1.0;
            else if (temp <= -1.0)
                temp = -1.0;

            double yRadian = Math.Sin(temp);

            double xx2 = pvec4Quat._x * x2;
            double xy2 = pvec4Quat._x * y2;
            double zz2 = pvec4Quat._z * z2;
            double wz2 = pvec4Quat._w * z2;

            if (yRadian < Maths._halfPif)
                if (yRadian > -Maths._halfPif)
                {
                    double yz2 = pvec4Quat._y * z2;
                    double wx2 = pvec4Quat._w * x2;
                    double yy2 = pvec4Quat._y * y2;
                    this._x = (float)Math.Atan2(yz2 + wx2, (1.0f - (xx2 + yy2)));
                    this._y = (float)yRadian;
                    this._z = (float)Math.Atan2((xy2 + wz2), (1.0f - (yy2 + zz2)));
                }
                else
                {
                    this._x = (float)-Math.Atan2((xy2 - wz2), (1.0f - (xx2 + zz2)));
                    this._y = (float)yRadian;
                    this._z = 0.0f;
                }
            else
            {
                this._x = (float)Math.Atan2((xy2 - wz2), (1.0f - (xx2 + zz2)));
                this._y = (float)yRadian;
                this._z = 0.0f;
            }
        }
    }
}
