namespace System
{
    public struct Quaternion
    {
        public float _x, _y, _z, _w;

        public Quaternion(float x, float y, float z, float w) { this._x = x; this._y = y; this._z = z; this._w = w; }
        public Quaternion(float s) { _x = s; _y = s; _z = s; _w = 1; }

        public unsafe float this[int index]
        {
            get { fixed (Quaternion* p = &this) return ((float*)p)[index]; }
            set { fixed (Quaternion* p = &this) ((float*)p)[index] = value; }
        }

        public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);

        public static Quaternion operator *(Quaternion v, float f) { return new Quaternion(v._x * f, v._y * f, v._z * f, v._w * f); }
        public static Quaternion operator /(Quaternion v, float f) { return new Quaternion(v._x / f, v._y / f, v._z / f, v._w / f); }
        public static Quaternion operator -(Quaternion v1, Vector4 v2) { return new Quaternion(v1._x - v2._x, v1._y - v2._y, v1._z - v2._z, v1._w - v2._w); }
        public static Quaternion operator +(Quaternion v1, Vector4 v2) { return new Quaternion(v1._x + v2._x, v1._y + v2._y, v1._z + v2._z, v1._w + v2._w); }

        public static bool operator ==(Quaternion v1, Quaternion v2) { return (v1._x == v2._x) && (v1._y == v2._y) && (v1._z == v2._z) && (v1._w == v2._w); }
        public static bool operator !=(Quaternion v1, Quaternion v2) { return (v1._x != v2._x) || (v1._y != v2._y) || (v1._z != v2._z) || (v1._w != v2._w); }

        public float Length() { return (float)Math.Sqrt(Dot()); }
        public float Dot() { return _x * _x + _y * _y + _z * _z + _w * _w; }
        public float Dot(Quaternion v) { return _x * v._x + _y * v._y + _z * v._z + _w * v._w; }
        public Quaternion Normalize() { return this * (1.0f / Length()); }

        public float Dot3() { return _x * _x + _y * _y + _z * _z; }
        public float Dot3(Quaternion v) { return _x * v._x + _y * v._y + _z * v._z; }
        public float Length3() { return (float)Math.Sqrt(Dot3()); }
        public Quaternion Normalize3()
        {
            float scale = 1.0f / Length3();
            return new Quaternion(_x * scale, _y * scale, _z * scale, _w);
        }

        public override bool Equals(object obj)
        {
            if (obj is Quaternion)
                return this == (Quaternion)obj;
            return false;
        }

        public unsafe override int GetHashCode()
        {
            fixed (Quaternion* p = &this)
            {
                int* p2 = (int*)p;
                return p2[0] ^ p2[1] ^ p2[2] ^ p2[3];
            }
        }

        public void ToAxisAngle(out Vector3 axis, out float angle)
        {
            Vector4 result = ToAxisAngle();
            axis = new Vector3(result._x, result._y, result._z);
            angle = result._w;
        }

        public Vector4 ToAxisAngle()
        {
            Quaternion q = this;
            if (q._w > 1.0f)
                q.Normalize();

            Vector4 result = new Vector4();

            result._w = 2.0f * (float)System.Math.Acos(q._w);
            float den = (float)System.Math.Sqrt(1.0 - q._w * q._w);
            if (den > 0.0001f)
            {
                result._x = q._x / den;
                result._y = q._y / den;
                result._z = q._z / den;
            }
            else
                result._x = 1;

            return result;
        }

        public static Quaternion FromAxisAngle(Vector3 axis, float angle)
        {
            if (axis.Dot() == 0.0f)
                return Identity;

            Quaternion result = Identity;

            angle *= 0.5f;
            axis.Normalize();
            result._x = axis.X * (float)System.Math.Sin(angle);
            result._y = axis.Y * (float)System.Math.Sin(angle);
            result._z = axis.Z * (float)System.Math.Sin(angle);
            result._w = (float)System.Math.Cos(angle);

            return result.Normalize();
        }

        //public static Quaternion Slerp(Quaternion q1, Quaternion q2, float blend)
        //{
        //    // if either input is zero, return the other.
        //    if (q1.Dot() == 0.0f)
        //        if (q2.Dot() == 0.0f)
        //            return Identity;
        //        else
        //            return q2;
        //    else if (q2.Dot() == 0.0f)
        //        return q1;

        //    float cosHalfAngle = q1._w * q2._w + Vector3.Dot(new Vector3(q1._x, q1._y, q1._z), new Vector3(q2._x, q2._y, q2._z));

        //    if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f)
        //    {
        //        // angle = 0.0f, so just return one input.
        //        return q1;
        //    }
        //    else if (cosHalfAngle < 0.0f)
        //    {
        //        q2._x = -q2._x;
        //        q2._y = -q2._y;
        //        q2._z = -q2._z;
        //        q2._w = -q2._w;
        //        cosHalfAngle = -cosHalfAngle;
        //    }

        //    float blendA;
        //    float blendB;
        //    if (cosHalfAngle < 0.99f)
        //    {
        //        // do proper slerp for big angles
        //        float halfAngle = (float)System.Math.Acos(cosHalfAngle);
        //        float sinHalfAngle = (float)System.Math.Sin(halfAngle);
        //        float oneOverSinHalfAngle = 1.0f / sinHalfAngle;
        //        blendA = (float)System.Math.Sin(halfAngle * (1.0f - blend)) * oneOverSinHalfAngle;
        //        blendB = (float)System.Math.Sin(halfAngle * blend) * oneOverSinHalfAngle;
        //    }
        //    else
        //    {
        //        // do lerp if angle is really small.
        //        blendA = 1.0f - blend;
        //        blendB = blend;
        //    }

        //    Quaternion result = new Quaternion(blendA * new Vector3(q1._x, q1._y, q1._z) + blendB * new Vector3(q2._x, q2._y, q2._z), blendA * q1._w + blendB * q2._w);
        //    if (result.Dot() > 0.0f)
        //        return result.Normalize();
        //    else
        //        return Identity;
        //}
        
        public void FromEuler(Vector3 pvec3EulerAngle)
        {
            double xRadian = pvec3EulerAngle._x * 0.5;
            double yRadian = pvec3EulerAngle._y * 0.5;
            double zRadian = pvec3EulerAngle._z * 0.5;
            double sinX = Math.Sin(xRadian);
            double cosX = Math.Cos(xRadian);
            double sinY = Math.Sin(yRadian);
            double cosY = Math.Cos(yRadian);
            double sinZ = Math.Sin(zRadian);
            double cosZ = Math.Cos(zRadian);

            //XYZ
            this._x = (float)(sinX * cosY * cosZ - cosX * sinY * sinZ);
            this._y = (float)(cosX * sinY * cosZ + sinX * cosY * sinZ);
            this._z = (float)(cosX * cosY * sinZ - sinX * sinY * cosZ);
            this._w = (float)(cosX * cosY * cosZ + sinX * sinY * sinZ);
        }
    }
}
