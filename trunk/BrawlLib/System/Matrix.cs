using System;
using System.Runtime.InteropServices;
using BrawlLib.Modeling;

namespace System
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Matrix
    {
        public static readonly Matrix Identity = ScaleMatrix(1.0f, 1.0f, 1.0f);

        fixed float _values[16];

        public float* Data { get { fixed (float* ptr = _values)return ptr; } }
        
        public float this[int x, int y]
        {
            get { return Data[(y << 2) + x]; }
            set { Data[(y << 2) + x] = value; }
        }
        public float this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        public Matrix(float[] values)
        {
            Matrix m = this;
            float* p = (float*)&m;
            for (int i = 0; i < 16; i++)
                p[i] = values[i];
        }

        public Matrix Reverse()
        {
            Matrix m;
            float* pOut = (float*)&m;
            fixed (float* p = _values)
                for (int y = 0; y < 4; y++)
                    for (int x = 0; x < 4; x++)
                        *pOut++ = p[(x << 2) + y];
            return m;
        }

        public Vector3 GetPoint()
        {
            fixed (float* p = _values)
                return new Vector3(p[12], p[13], p[14]);
        }

        public static Matrix ScaleMatrix(float x, float y, float z)
        {
            Matrix m = new Matrix();
            float* p = (float*)&m;
            p[0] = x;
            p[5] = y;
            p[10] = z;
            p[15] = 1.0f;
            return m;
        }
        public static Matrix TranslationMatrix(float x, float y, float z)
        {
            Matrix m = Identity;
            float* p = (float*)&m;
            p[12] = x;
            p[13] = y;
            p[14] = z;
            return m;
        }
        public static Matrix RotationMatrix(Vector3 angles) { return RotationMatrix(angles._x, angles._y, angles._z); }
        public static Matrix RotationMatrix(float x, float y, float z)
        {
            float cosx = (float)Math.Cos(x * Maths._deg2radf);
            float sinx = (float)Math.Sin(x * Maths._deg2radf);
            float cosy = (float)Math.Cos(y * Maths._deg2radf);
            float siny = (float)Math.Sin(y * Maths._deg2radf);
            float cosz = (float)Math.Cos(z * Maths._deg2radf);
            float sinz = (float)Math.Sin(z * Maths._deg2radf);

            Matrix m = Identity;
            float* p = (float*)&m;

            m[0] = cosy * cosz;
            m[1] = sinz * cosy;
            m[2] = -siny;
            m[4] = (sinx * cosz * siny - cosx * sinz);
            m[5] = (sinx * sinz * siny + cosz * cosx);
            m[6] = sinx * cosy;
            m[8] = (sinx * sinz + cosx * cosz * siny);
            m[9] = (cosx * sinz * siny - sinx * cosz);
            m[10] = cosx * cosy;

            return m;
        }

        public void Translate(float x, float y, float z)
        {
            fixed (float* p = _values)
            {
                p[12] += (p[0] * x) + (p[4] * y) + (p[8] * z);
                p[13] += (p[1] * x) + (p[5] * y) + (p[9] * z);
                p[14] += (p[2] * x) + (p[6] * y) + (p[10] * z);
                p[15] += (p[3] * x) + (p[7] * y) + (p[11] * z);
            }
        }

        public void Multiply(Matrix* m)
        {
            Matrix m2 = this;

            float* s1 = (float*)m, s2 = (float*)&m2;

            fixed (float* dPtr = _values)
            {
                int index = 0;
                float val;
                for (int b = 0; b < 16; b += 4)
                    for (int a = 0; a < 4; a++)
                    {
                        val = 0.0f;
                        for (int x = b, y = a; y < 16; y += 4)
                            val += s1[x++] * s2[y];
                        dPtr[index++] = val;
                    }
            }
        }

        public Vector3 Multiply(Vector3 v)
        {
            Vector3 nv = new Vector3();
            fixed (float* p = _values)
            {
                nv._x = (p[0] * v._x) + (p[4] * v._y) + (p[8] * v._z) + p[12];
                nv._y = (p[1] * v._x) + (p[5] * v._y) + (p[9] * v._z) + p[13];
                nv._z = (p[2] * v._x) + (p[6] * v._y) + (p[10] * v._z) + p[14];
            }
            return nv;
        }

        public Vector3 MultiplyInverse(Vector3 v)
        {
            Vector3 nv = new Vector3();
            fixed (float* p = _values)
            {
                nv._x = (p[0] * v._x) + (p[1] * v._y) + (p[2] * v._z);
                nv._y = (p[4] * v._x) + (p[5] * v._y) + (p[6] * v._z);
                nv._z = (p[8] * v._x) + (p[9] * v._y) + (p[10] * v._z);
            }
            return nv;
        }

        public Vector3 Divide(Vector3 v, Vector3 nv)
        {
            //Thanks to VILE for this function!
            fixed (float* p = _values)
            {
                v._x = (nv._x - (p[4] * v._y) - (p[8] * v._z) - p[12]) / p[0];
                v._y = (nv._y - (p[1] * v._x) - (p[9] * v._z) - p[13]) / p[5];
                v._z = (nv._z - (p[2] * v._x) - (p[6] * v._y) - p[14]) / p[10];
            }
            return v;
        }

        public static Vector2 operator *(Matrix m, Vector2 v)
        {
            Vector2 nv;
            float* p = (float*)&m;
            nv._x = (p[0] * v._x) + (p[4] * v._y) + p[8] + p[12];
            nv._y = (p[1] * v._x) + (p[5] * v._y) + p[9] + p[13];
            //nv._x = (p[0] * v._x) + (p[4] * v._y) + (p[8] * v._z) + p[12];
            //nv._y = (p[1] * v._x) + (p[5] * v._y) + (p[9] * v._z) + p[13];
            //nv._z = (p[2] * v._x) + (p[6] * v._y) + (p[10] * v._z) + p[14];
            return nv;
        }

        public static Vector3 operator *(Matrix m, Vector3 v)
        {
            Vector3 nv;
            float* p = (float*)&m;
            nv._x = (p[0] * v._x) + (p[4] * v._y) + (p[8] * v._z) + p[12];
            nv._y = (p[1] * v._x) + (p[5] * v._y) + (p[9] * v._z) + p[13];
            nv._z = (p[2] * v._x) + (p[6] * v._y) + (p[10] * v._z) + p[14];
            return nv;
        }

        public static Vector4 operator *(Matrix m, Vector4 v)
        {
            Vector4 nv;
            float* dPtr = (float*)&nv;
            float* p0 = (float*)&m, p1 = p0 + 4, p2 = p0 + 8, p3 = p0 + 12;
            for (int i = 0; i < 4; i++)
                dPtr[i] = (p0[i] * v._x) + (p1[i] * v._y) + (p2[i] * v._z) + (p3[i] * v._w);
            return nv;
        }

        internal void Multiply(float p)
        {
            fixed (float* dPtr = _values)
            {
                for (int i = 0; i < 16; i++)
                    dPtr[i] *= p;
            }
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix dm;
            float* s1 = (float*)&m2, s2 = (float*)&m1, d = (float*)&dm;

            int index = 0;
            float val;
            for (int b = 0; b < 16; b += 4)
                for (int a = 0; a < 4; a++)
                {
                    val = 0.0f;
                    for (int x = b, y = a; y < 16; y += 4)
                        val += s1[x++] * s2[y];
                    d[index++] = val;
                }

            return dm;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            float* dPtr = (float*)&m1;
            float* sPtr = (float*)&m2;
            for (int i = 0; i < 16; i++)
                *dPtr++ += *sPtr++;
            return m1;
        }
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            float* dPtr = (float*)&m1;
            float* sPtr = (float*)&m2;
            for (int i = 0; i < 16; i++)
                *dPtr++ -= *sPtr++;
            return m1;
        }
        public static Matrix operator /(Matrix m1, Matrix m2)
        {
            float* dPtr = (float*)&m1;
            float* sPtr = (float*)&m2;
            for (int i = 0; i < 16; i++)
                *dPtr++ /= *sPtr++;
            return m1;
        }
        public static Matrix operator *(Matrix m, float f)
        {
            float* p = (float*)&m;
            for (int i = 0; i < 16; i++)
                *p++ *= f;
            return m;
        }
        public static Matrix operator /(Matrix m, float f)
        {
            float* p = (float*)&m;
            for (int i = 0; i < 16; i++)
                *p++ /= f;
            return m;
        }
        public static bool operator ==(Matrix m1, Matrix m2) 
        {
            float* p1 = (float*)&m1;
            float* p2 = (float*)&m2;

            for (int i = 0; i < 16; i++)
                if (*p1++ != *p2++)
                    return false;

            return true;
        }
        public static bool operator !=(Matrix m1, Matrix m2) 
        {
            float* p1 = (float*)&m1;
            float* p2 = (float*)&m2;

            for (int i = 0; i < 16; i++)
                if (*p1++ != *p2++)
                    return true;

            return false;
        }
        public static Matrix operator -(Matrix m)
        {
            float* p = (float*)&m;
            int i = 0;
            while (i++ < 16) *p = -*p++;
            return m;
        }

        public Matrix Inverse()
        {
            Matrix m = this;

            float* p = (float*)&m;
            int i = 0;
            while (i < 16)
            {
                if (i == 0 || i == 5 || i == 10 || i == 15)
                {
                    p++;
                }
                else
                    *p = -*p++;
                i++;
            }

            return m;
        }

        public override string ToString()
        {
            return String.Format("({0},{1},{2},{3})({4},{5},{6},{7})({8},{9},{10},{11})({12},{13},{14},{15})", this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7], this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15]);
        }

        public void RotateX(float x)
        {
            float var1, var2;
            float cosx = (float)Math.Cos(x / 180.0f * Math.PI);
            float sinx = (float)Math.Sin(x / 180.0f * Math.PI);

            fixed (float* p = _values)
            {
                var1 = p[4]; var2 = p[8];
                p[4] = (var1 * cosx) + (var2 * sinx);
                p[8] = (var1 * -sinx) + (var2 * cosx);

                var1 = p[5]; var2 = p[9];
                p[5] = (var1 * cosx) + (var2 * sinx);
                p[9] = (var1 * -sinx) + (var2 * cosx);

                var1 = p[6]; var2 = p[10];
                p[6] = (var1 * cosx) + (var2 * sinx);
                p[10] = (var1 * -sinx) + (var2 * cosx);
            }
        }
        public void RotateY(float y)
        {
            float var1, var2;
            float cosy = (float)Math.Cos(y / 180.0f * Math.PI);
            float siny = (float)Math.Sin(y / 180.0f * Math.PI);

            fixed (float* p = _values)
            {
                var1 = p[0]; var2 = p[8];
                p[0] = (var1 * cosy) + (var2 * -siny);
                p[8] = (var1 * siny) + (var2 * cosy);

                var1 = p[1]; var2 = p[9];
                p[1] = (var1 * cosy) + (var2 * -siny);
                p[9] = (var1 * siny) + (var2 * cosy);

                var1 = p[2]; var2 = p[10];
                p[2] = (var1 * cosy) + (var2 * -siny);
                p[10] = (var1 * siny) + (var2 * cosy);
            }
        }
        public void RotateZ(float z)
        {
            float var1, var2;
            float cosz = (float)Math.Cos(z / 180.0f * Math.PI);
            float sinz = (float)Math.Sin(z / 180.0f * Math.PI);

            fixed (float* p = _values)
            {
                var1 = p[0]; var2 = p[4];
                p[0] = (var1 * cosz) + (var2 * sinz);
                p[4] = (var1 * -sinz) + (var2 * cosz);

                var1 = p[1]; var2 = p[5];
                p[1] = (var1 * cosz) + (var2 * sinz);
                p[5] = (var1 * -sinz) + (var2 * cosz);

                var1 = p[2]; var2 = p[6];
                p[2] = (var1 * cosz) + (var2 * sinz);
                p[6] = (var1 * -sinz) + (var2 * cosz);
            }
        }

        public Matrix GetRotationMatrix()
        {
            Matrix m = Identity;
            float* p = (float*)&m;
            fixed (float* src = _values)
            {
                m[0] = src[0];
                m[1] = src[1];
                m[2] = src[2];

                m[4] = src[4];
                m[5] = src[5];
                m[6] = src[6];

                m[8] = src[8];
                m[9] = src[9];
                m[10] = src[10];
            }
            return m;
        }

        public Quaternion QuaternionFromMatrix()
        {
            Matrix m = this;
            Quaternion q = new Quaternion();

            q._w = (float)Math.Sqrt(Math.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2;
            q._x = (float)Math.Sqrt(Math.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2;
            q._y = (float)Math.Sqrt(Math.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2;
            q._z = (float)Math.Sqrt(Math.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2;
            q._x *= Math.Sign(q._x * (m[2, 1] - m[1, 2]));
            q._y *= Math.Sign(q._y * (m[0, 2] - m[2, 0]));
            q._z *= Math.Sign(q._z * (m[1, 0] - m[0, 1]));
            return q;
        }

        public Vector4 toQuaternion()
        {
            Vector4 q = new Vector4();

            Matrix m = this;
            float* p = (float*)&m;

            double trace = p[0] + p[5] + p[10] + 1.0;

            if (trace > 1e-7)
            {
                double s = 0.5 / Math.Sqrt(trace);
                q[0] = (float)((p[9] - p[6]) * s);
                q[1] = (float)((p[2] - p[8]) * s);
                q[2] = (float)((p[4] - p[1]) * s);
                q[3] = (float)(0.25 / s);
            }
            else
            {
                if (p[0] > p[5] && p[0] > p[10])
                {
                    double s = 2.0 * Math.Sqrt(1.0 + p[0] - p[5] - p[10]);
                    q[0] = (float)(0.25 * s);
                    q[1] = (float)((p[1] + p[4]) / s);
                    q[2] = (float)((p[2] + p[8]) / s);
                    q[3] = (float)((p[6] - p[9]) / s);
                }
                else if (p[5] > p[10])
                {
                    double s = 2.0 * Math.Sqrt(1.0 + p[5] - p[0] - p[10]);
                    q[0] = (float)((p[1] + p[4]) / s);
                    q[1] = (float)(0.25 * s);
                    q[2] = (float)((p[6] + p[9]) / s);
                    q[3] = (float)((p[2] - p[8]) / s);
                }
                else
                {
                    double s = 2.0 * Math.Sqrt(1.0 + p[10] - p[0] - p[5]);
                    q[0] = (float)((p[2] + p[8]) / s);
                    q[1] = (float)((p[6] + p[9]) / s);
                    q[2] = (float)(0.25 * s);
                    q[3] = (float)((p[1] - p[4]) / s);
                }
            }

            q.Normalize();

            return q * Maths._rad2degf;
        }

        public void fromQuaternion(Vector4 q)
        {
            Matrix m = this;
            float* p = (float*)&m;

            double X = q[0];
            double Y = q[1];
            double Z = q[2];
            double W = q[3];

            double xx = X * X;
            double xy = X * Y;
            double xz = X * Z;
            double xw = X * W;
            double yy = Y * Y;
            double yz = Y * Z;
            double yw = Y * W;
            double zz = Z * Z;
            double zw = Z * W;

            this = Identity;

            p[0] = (float)(1 - 2 * (yy + zz));
            p[1] = (float)(2 * (xy - zw));
            p[2] = (float)(2 * (xz + yw));
            p[4] = (float)(2 * (xy + zw));
            p[5] = (float)(1 - 2 * (xx + zz));
            p[6] = (float)(2 * (yz - xw));
            p[8] = (float)(2 * (xz - yw));
            p[9] = (float)(2 * (yz + xw));
            p[10] = (float)(1 - 2 * (xx + yy));
        }

        //Derive Euler angles from matrix, simply by reversing the transformation process.
        //Vulnerable to gimbal lock, quaternions may be a better solution
        public Vector3 GetAngles()
        {
            float x, y, z, c;
            fixed (float* p = _values)
            {
                y = (float)Math.Asin(-p[2]);
                if ((Maths._halfPif - (float)Math.Abs(y)) < 0.0001f)
                {
                    //Gimbal lock, occurs when the y rotation falls on pi/2 or -pi/2
                    z = 0.0f;
                    if (y > 0)
                        x = (float)Math.Atan2(p[4], p[8]);
                    else
                        x = (float)Math.Atan2(p[4], -p[8]);
                }
                else
                {
                    c = (float)Math.Cos(y);
                    x = (float)Math.Atan2(p[6] / c, p[10] / c);
                    z = (float)Math.Atan2(p[1] / c, p[0] / c);

                    //180 z/x inverts y, use second option
                    if (Maths._pif - Math.Abs(z) < 0.05f)
                    {
                        y = Maths._pif - y;
                        c = (float)Math.Cos(y);
                        x = (float)Math.Atan2(p[6] / c, p[10] / c);
                        z = (float)Math.Atan2(p[1] / c, p[0] / c);
                    }
                }
            }
            return new Vector3(x, y, z) * Maths._rad2degf;
        }

        public FrameState Derive()
        {
            FrameState state = new FrameState();

            fixed (float* p = _values)
            {
                //Translation is easy!
                state._translate = *(Vector3*)&p[12];

                //Scale, use sqrt of rotation columns
                state._scale._x = (float)Math.Round(Math.Sqrt(p[0] * p[0] + p[1] * p[1] + p[2] * p[2]), 4);
                state._scale._y = (float)Math.Round(Math.Sqrt(p[4] * p[4] + p[5] * p[5] + p[6] * p[6]), 4);
                state._scale._z = (float)Math.Round(Math.Sqrt(p[8] * p[8] + p[9] * p[9] + p[10] * p[10]), 4);

                state._rotate = GetAngles();
            }

            state.CalcTransforms();
            return state;
        }

        internal void Scale(float x, float y, float z)
        {
            Matrix m = ScaleMatrix(x, y, z);
            this.Multiply(&m);
        }

        public static explicit operator Matrix(Matrix43 m)
        {
            Matrix m1;
            float* sPtr = (float*)&m;
            float* dPtr = (float*)&m1;

            dPtr[0] = sPtr[0];
            dPtr[1] = sPtr[4];
            dPtr[2] = sPtr[8];
            dPtr[3] = 0.0f;
            dPtr[4] = sPtr[1];
            dPtr[5] = sPtr[5];
            dPtr[6] = sPtr[9];
            dPtr[7] = 0.0f;
            dPtr[8] = sPtr[2];
            dPtr[9] = sPtr[6];
            dPtr[10] = sPtr[10];
            dPtr[11] = 0.0f;
            dPtr[12] = sPtr[3];
            dPtr[13] = sPtr[7];
            dPtr[14] = sPtr[11];
            dPtr[15] = 1.0f;

            return m1;
        }
        public static explicit operator Matrix43(Matrix m)
        {
            Matrix43 m1;
            float* sPtr = (float*)&m;
            float* dPtr = (float*)&m1;

            dPtr[0] = sPtr[0];
            dPtr[1] = sPtr[4];
            dPtr[2] = sPtr[8];
            dPtr[3] = sPtr[12];
            dPtr[4] = sPtr[1];
            dPtr[5] = sPtr[5];
            dPtr[6] = sPtr[9];
            dPtr[7] = sPtr[13];
            dPtr[8] = sPtr[2];
            dPtr[9] = sPtr[6];
            dPtr[10] = sPtr[10];
            dPtr[11] = sPtr[14];

            return m1;
        }

        public static Matrix ProjectionMatrix(float fovY, float aspect, float nearZ, float farZ)
        {
            Matrix m;

            float* p = (float*)&m;

            float cotan = (float)(1.0 / Math.Tan(fovY / 2 * Math.PI / 180.0));

            p[0] = cotan / aspect;
            p[5] = cotan;
            p[10] = (farZ + nearZ) / (nearZ - farZ);
            p[11] = -1.0f;
            p[14] = (2.0f * farZ * nearZ) / (nearZ - farZ);

            p[1] = p[2] = p[3] = p[4] = p[6] = p[7] = p[8] = p[9] = p[12] = p[13] = p[15] = 0.0f;

            return m;
        }
        public static Matrix ReverseProjectionMatrix(float fovY, float aspect, float nearZ, float farZ)
        {
            Matrix m;

            float* p = (float*)&m;

            float cotan = (float)(1.0 / Math.Tan(fovY / 2 * Math.PI / 180.0));
            float val = (2.0f * farZ * nearZ) / (nearZ - farZ);

            p[0] = aspect / cotan;
            p[5] = 1.0f / cotan;
            p[11] = 1.0f / val;
            p[14] = -1.0f;
            p[15] = (farZ + nearZ) / (nearZ - farZ) / val;

            p[1] = p[2] = p[3] = p[4] = p[6] = p[7] = p[8] = p[9] = p[10] = p[12] = p[13] = 0.0f;

            return m;
        }

        public static Matrix TransformMatrix(Vector3 scale, Vector3 rotate, Vector3 translate)
        {
            Matrix m;
            float* d = (float*)&m;

            float cosx = (float)Math.Cos(rotate._x * Maths._deg2radf);
            float sinx = (float)Math.Sin(rotate._x * Maths._deg2radf);
            float cosy = (float)Math.Cos(rotate._y * Maths._deg2radf);
            float siny = (float)Math.Sin(rotate._y * Maths._deg2radf);
            float cosz = (float)Math.Cos(rotate._z * Maths._deg2radf);
            float sinz = (float)Math.Sin(rotate._z * Maths._deg2radf);

            d[0] = scale._x * cosy * cosz;
            d[1] = scale._x * sinz * cosy;
            d[2] = -scale._x * siny;
            d[3] = 0.0f;
            d[4] = scale._y * (sinx * cosz * siny - cosx * sinz);
            d[5] = scale._y * (sinx * sinz * siny + cosz * cosx);
            d[6] = scale._y * sinx * cosy;
            d[7] = 0.0f;
            d[8] = scale._z * (sinx * sinz + cosx * cosz * siny);
            d[9] = scale._z * (cosx * sinz * siny - sinx * cosz);
            d[10] = scale._z * cosx * cosy;
            d[11] = 0.0f;
            d[12] = translate._x;
            d[13] = translate._y;
            d[14] = translate._z;
            d[15] = 1.0f;

            return m;
        }

        public static Matrix ReverseTransformMatrix(Vector3 scale, Vector3 rotation, Vector3 translation)
        {
            float cosx = (float)Math.Cos(rotation._x * Maths._deg2radf);
            float sinx = (float)Math.Sin(rotation._x * Maths._deg2radf);
            float cosy = (float)Math.Cos(rotation._y * Maths._deg2radf);
            float siny = (float)Math.Sin(rotation._y * Maths._deg2radf);
            float cosz = (float)Math.Cos(rotation._z * Maths._deg2radf);
            float sinz = (float)Math.Sin(rotation._z * Maths._deg2radf);

            scale._x = 1 / scale._x;
            scale._y = 1 / scale._y;
            scale._z = 1 / scale._z;
            translation._x = -translation._x;
            translation._y = -translation._y;
            translation._z = -translation._z;

            Matrix m;
            float* p = (float*)&m;

            p[0] = scale._x * cosy * cosz;
            p[1] = scale._y * (sinx * siny * cosz - cosx * sinz);
            p[2] = scale._z * (cosx * siny * cosz + sinx * sinz);
            p[3] = 0.0f;

            p[4] = scale._x * cosy * sinz;
            p[5] = scale._y * (sinx * siny * sinz + cosx * cosz);
            p[6] = scale._z * (cosx * siny * sinz - sinx * cosz);
            p[7] = 0.0f;

            p[8] = -scale._x * siny;
            p[9] = scale._y * sinx * cosy;
            p[10] = scale._z * cosx * cosy;
            p[11] = 0.0f;

            p[12] = (translation._x * p[0]) + (translation._y * p[4]) + (translation._z * p[8]);
            p[13] = (translation._x * p[1]) + (translation._y * p[5]) + (translation._z * p[9]);
            p[14] = (translation._x * p[2]) + (translation._y * p[6]) + (translation._z * p[10]);
            p[15] = 1.0f;

            return m;
        }

        public static Matrix QuaternionTransformMatrix(Vector3 scale, Quaternion rotate, Vector3 translate)
        {
            Matrix m;
            float* p = (float*)&m;

            float xx = rotate._x * rotate._x;
            float yy = rotate._y * rotate._y;
            float zz = rotate._z * rotate._z;
            float xy = rotate._x * rotate._y;
            float zw = rotate._z * rotate._w;
            float zx = rotate._z * rotate._x;
            float yw = rotate._y * rotate._w;
            float yz = rotate._y * rotate._z;
            float xw = rotate._x * rotate._w;

            p[0] = scale._x * (1.0f - (2.0f * (yy + zz)));
            p[1] = scale._x * (2.0f * (xy + zw));
            p[2] = scale._x * (2.0f * (zx - yw));
            p[3] = 0.0f;

            p[4] = scale._y * (2.0f * (xy - zw));
            p[5] = scale._y * (1.0f - (2.0f * (zz + xx)));
            p[6] = scale._y * (2.0f * (yz + xw));
            p[7] = 0.0f;

            p[8] = scale._z * (2.0f * (zx + yw));
            p[9] = scale._z * (2.0f * (yz - xw));
            p[10] = scale._z * (1.0f - (2.0f * (yy + xx)));
            p[11] = 0.0f;

            p[12] = translate._x;
            p[13] = translate._y;
            p[14] = translate._z;
            p[15] = 1.0f;

            return m;
        }

        public static Matrix ReverseQuaternionTransformMatrix(Vector3 scale, Quaternion rotate, Vector3 translate)
        {
            Matrix m;
            float* p = (float*)&m;

            float xx = rotate._x * rotate._x;
            float yy = rotate._y * rotate._y;
            float zz = rotate._z * rotate._z;
            float xy = rotate._x * rotate._y;
            float zw = rotate._z * rotate._w;
            float zx = rotate._z * rotate._x;
            float yw = rotate._y * rotate._w;
            float yz = rotate._y * rotate._z;
            float xw = rotate._x * rotate._w;

            scale._x = 1 / scale._x;
            scale._y = 1 / scale._y;
            scale._z = 1 / scale._z;
            translate._x = -translate._x;
            translate._y = -translate._y;
            translate._z = -translate._z;

            p[0] = scale._x * (1.0f - (2.0f * (yy + zz)));
            p[1] = scale._x * (2.0f * (xy + zw));
            p[2] = scale._x * (2.0f * (zx - yw));
            p[3] = 0.0f;

            p[4] = scale._y * (2.0f * (xy - zw));
            p[5] = scale._y * (1.0f - (2.0f * (zz + xx)));
            p[6] = scale._y * (2.0f * (yz + xw));
            p[7] = 0.0f;

            p[8] = scale._z * (2.0f * (zx + yw));
            p[9] = scale._z * (2.0f * (yz - xw));
            p[10] = scale._z * (1.0f - (2.0f * (yy + xx)));
            p[11] = 0.0f;

            p[12] = (translate._x * p[0]) + (translate._y * p[4]) + (translate._z * p[8]);
            p[13] = (translate._x * p[1]) + (translate._y * p[5]) + (translate._z * p[9]);
            p[14] = (translate._x * p[2]) + (translate._y * p[6]) + (translate._z * p[10]);
            p[15] = 1.0f;

            return m;
        }

        public FrameState QuatDerive()
        {
            FrameState state = new FrameState();

            fixed (float* p = _values)
            {
                //Translation is easy!
                state._translate = *(Vector3*)&p[12];

                //Scale, use sqrt of rotation columns
                state._scale._x = (float)Math.Round(Math.Sqrt(p[0] * p[0] + p[1] * p[1] + p[2] * p[2]), 4);
                state._scale._y = (float)Math.Round(Math.Sqrt(p[4] * p[4] + p[5] * p[5] + p[6] * p[6]), 4);
                state._scale._z = (float)Math.Round(Math.Sqrt(p[8] * p[8] + p[9] * p[9] + p[10] * p[10]), 4);

                Matrix m = new Matrix();
                float* d = (float*)&m;

                d[0] = p[0] / state._scale._x;
                d[1] = p[1] / state._scale._x;
                d[2] = p[2] / state._scale._x;

                d[4] = p[4] / state._scale._y;
                d[5] = p[5] / state._scale._y;
                d[6] = p[6] / state._scale._y;

                d[8] = p[8] / state._scale._z;
                d[9] = p[9] / state._scale._z;
                d[10] = p[10] / state._scale._z;

                d[15] = 1;

                state._quaternion = m.ToQuaternion();
            }

            state.CalcQuatTransforms();
            return state;
        }

        public static Matrix AxisAngleMatrix(Vector3 point1, Vector3 point2)
        {
            Matrix m = Matrix.Identity;
            //Equal points will cause a corrupt matrix
            if (point1 != point2)
            {
                float* pOut = (float*)&m;

                point1 = point1.Normalize();
                point2 = point2.Normalize();

                Vector3 vSin = point1.Cross(point2);
                Vector3 axis = vSin.Normalize();

                float cos = point1.Dot(point2);
                Vector3 vt = axis * (1.0f - cos);

                pOut[0] = vt._x * axis._x + cos;
                pOut[5] = vt._y * axis._y + cos;
                pOut[10] = vt._z * axis._z + cos;

                vt._x *= axis._y;
                vt._z *= axis._x;
                vt._y *= axis._z;

                pOut[1] = vt._x + vSin._z;
                pOut[2] = vt._z - vSin._y;
                pOut[4] = vt._x - vSin._z;
                pOut[6] = vt._y + vSin._x;
                pOut[8] = vt._z + vSin._y;
                pOut[9] = vt._y - vSin._x;
            }
            return m;
        }

        public Matrix QuaternionRotMatrix(Quaternion quaternion)
        {
            Matrix result;

            float xx = quaternion._x * quaternion._x;
            float yy = quaternion._y * quaternion._y;
            float zz = quaternion._z * quaternion._z;
            float xy = quaternion._x * quaternion._y;
            float zw = quaternion._z * quaternion._w;
            float zx = quaternion._z * quaternion._x;
            float yw = quaternion._y * quaternion._w;
            float yz = quaternion._y * quaternion._z;
            float xw = quaternion._x * quaternion._w;

            result[0, 0] = 1.0f - (2.0f * (yy + zz));
            result[0, 1] = 2.0f * (xy + zw);
            result[0, 2] = 2.0f * (zx - yw);
            result[0, 3] = 0.0f;

            result[1, 0] = 2.0f * (xy - zw);
            result[1, 1] = 1.0f - (2.0f * (zz + xx));
            result[1, 2] = 2.0f * (yz + xw);
            result[1, 3] = 0.0f;

            result[2, 0] = 2.0f * (zx + yw);
            result[2, 1] = 2.0f * (yz - xw);
            result[2, 2] = 1.0f - (2.0f * (yy + xx));
            result[2, 3] = 0.0f;

            result[3, 0] = 0.0f;
            result[3, 1] = 0.0f;
            result[3, 2] = 0.0f;
            result[3, 3] = 1.0f;

            return result;
        }
        public Matrix RotationAxis(Vector3 axis, float angle)
        {
            if (axis.Dot() != 1.0f)
                axis.Normalize();

            Matrix result;
            float x = axis._x;
            float y = axis._y;
            float z = axis._z;
            float cos = (float)(Math.Cos((double)(angle)));
            float sin = (float)(Math.Sin((double)(angle)));
            float xx = x * x;
            float yy = y * y;
            float zz = z * z;
            float xy = x * y;
            float xz = x * z;
            float yz = y * z;

            result[0, 0] = xx + (cos * (1.0f - xx));
            result[0, 1] = (xy - (cos * xy)) + (sin * z);
            result[0, 2] = (xz - (cos * xz)) - (sin * y);
            result[0, 3] = 0.0f;
            result[1, 0] = (xy - (cos * xy)) - (sin * z);
            result[1, 1] = yy + (cos * (1.0f - yy));
            result[1, 2] = (yz - (cos * yz)) + (sin * x);
            result[1, 3] = 0.0f;
            result[2, 0] = (xz - (cos * xz)) + (sin * y);
            result[2, 1] = (yz - (cos * yz)) - (sin * x);
            result[2, 2] = zz + (cos * (1.0f - zz));
            result[2, 3] = 0.0f;
            result[3, 0] = 0.0f;
            result[3, 1] = 0.0f;
            result[3, 2] = 0.0f;
            result[3, 3] = 1.0f;

            return result;
        }

        //public MatrixStruct RotationYawPitchRoll(float yaw, float pitch, float roll)
        //{
        //    Quaternion quaternion = Quaternion.FromAxisAngle(yaw, pitch, roll);
        //    return QuaternionRotMatrix(quaternion);
        //}
        
        public Quaternion ToQuaternion()
        {
            Matrix m = this;
            float* p = (float*)&m;
            
            Quaternion result = new Quaternion();

            float scale = m[0, 0] + m[1, 1] + m[2, 2];
            float half, sqrt;

            if (scale > 0.0f)
            {
                sqrt = (float)(Math.Sqrt((double)(scale + 1.0f)));

                result._w = sqrt * 0.5f;
                sqrt = 0.5f / sqrt;

                result._x = (m[1, 2] - m[2, 1]) * sqrt;
                result._y = (m[2, 0] - m[0, 2]) * sqrt;
                result._z = (m[0, 1] - m[1, 0]) * sqrt;

                return result;
            }

            if ((m[0, 0] >= m[1, 1]) && (m[0, 0] >= m[2, 2]))
            {
                sqrt = (float)(Math.Sqrt((double)(1.0f + m[0, 0] - m[1, 1] - m[2, 2])));
                half = 0.5f / sqrt;

                result._x = 0.5f * sqrt;
                result._x = (m[0, 1] + m[1, 0]) * half;
                result._z = (m[0, 2] + m[2, 0]) * half;
                result._w = (m[1, 2] - m[2, 1]) * half;

                return result;
            }

            if (m[1, 1] > m[2, 2])
            {
                sqrt = (float)(Math.Sqrt((double)(1.0f + m[1, 1] - m[0, 0] - m[2, 2])));
                half = 0.5f / sqrt;

                result._x = (m[1, 0] + m[0, 1]) * half;
                result._y = 0.5f * sqrt;
                result._x = (m[2, 1] + m[1, 2]) * half;
                result._w = (m[2, 0] - m[0, 2]) * half;

                return result;
            }

            sqrt = (float)(Math.Sqrt((double)(1.0f + m[2, 2] - m[0, 0] - m[1, 1])));
            half = 0.5f / sqrt;

            result._x = (m[2, 0] + m[0, 2]) * half;
            result._y = (m[2, 1] + m[1, 2]) * half;
            result._z = 0.5f * sqrt;
            result._w = (m[0, 1] - m[1, 0]) * half;

            return result;
        }

        public static void slerp(Quaternion Q0, Quaternion Q1, double T, Quaternion Result) 
        {
            double CosTheta = Q0[3] * Q1[3] - (Q0[0] * Q1[0] + Q0[1] * Q1[1] + Q0[2] * Q1[2]);
	        double Theta = Math.Acos(CosTheta);
            double SinTheta = Math.Sqrt(1.0 - CosTheta * CosTheta);

            if(Math.Abs(SinTheta) < 1e-5)
            {
                for(int i = 0; i < 4; i++)
                    Result[i] = Q0[i];
                    
                return;
            }

            double Sin_T_Theta = Math.Sin(T * Theta) / SinTheta;
            double Sin_OneMinusT_Theta = Math.Sin((1.0 - T) * Theta) / SinTheta;

	        for(int i = 0; i < 4; i++)
                Result[i] = (float)(Q0[i] * Sin_OneMinusT_Theta + Q1[i] * Sin_T_Theta);

            Result.Normalize();
        }


//public class Matrix {
//    /** The elements of the matrix. */
//    public double p[0], p[4], p[8], p[12];
//    public double p[1], p[5], p[9], p[13];
//    public double p[2], p[6], p[10], p[14];
//    public double p[3], p[7], p[11], p[15];

//    /** Default constructor. */
//    public Matrix(){
//    setIdentity();
//    }
    
//    /** Default constructor. */
//    public Matrix(Matrix in){
//    copy(in);
//    }
    
//    /** Set the matrix to the identity matrix. */
//    public void setIdentity(){
//    p[0] = 1.0; p[4] = 0.0; p[8] = 0.0; p[12] = 0.0;
//    p[1] = 0.0; p[5] = 1.0; p[9] = 0.0; p[13] = 0.0;
//    p[2] = 0.0; p[6] = 0.0; p[10] = 1.0; p[14] = 0.0;
//    p[3] = 0.0; p[7] = 0.0; p[11] = 0.0; p[15] = 1.0;
//    }
    
//    /** Set the matrix to the zero matrix. */
//    public void zero(){
//    p[0] = 0.0; p[4] = 0.0; p[8] = 0.0; p[12] = 0.0;
//    p[1] = 0.0; p[5] = 0.0; p[9] = 0.0; p[13] = 0.0;
//    p[2] = 0.0; p[6] = 0.0; p[10] = 0.0; p[14] = 0.0;
//    p[3] = 0.0; p[7] = 0.0; p[11] = 0.0; p[15] = 0.0;
//    }

//    /** Set matrix from another. */
//    public void set(Matrix m){
//    p[0] = m.p[0]; p[4] = m.p[4]; p[8] = m.p[8]; p[12] = m.p[12];
//    p[1] = m.p[1]; p[5] = m.p[5]; p[9] = m.p[9]; p[13] = m.p[13];
//    p[2] = m.p[2]; p[6] = m.p[6]; p[10] = m.p[10]; p[14] = m.p[14];
//    p[3] = m.p[3]; p[7] = m.p[7]; p[11] = m.p[11]; p[15] = m.p[15];
//    }
    
//    public void set(int i, int j, double val){
//    if(i < 0 || i > 3 || j < 0 || j > 3){
//        Log.error("trying to set element " + i + "," + j + " to %g", val);
//        return;
//    }
//    if(i == 0){
//        if(j == 0)      p[0] = val;
//        else if(j == 1) p[4] = val;
//        else if(j == 2) p[8] = val;
//        else if(j == 3) p[12] = val;
//    }else if(i == 1){
//        if(j == 0)      p[1] = val;
//        else if(j == 1) p[5] = val;
//        else if(j == 2) p[9] = val;
//        else if(j == 3) p[13] = val;
//    }else if(i == 2){
//        if(j == 0)      p[2] = val;
//        else if(j == 1) p[6] = val;
//        else if(j == 2) p[10] = val;
//        else if(j == 3) p[14] = val;
//    }else if(i == 3){
//        if(j == 0)      p[3] = val;
//        else if(j == 1) p[7] = val;
//        else if(j == 2) p[11] = val;
//        else if(j == 3) p[15] = val;
//    }else{
//        Log.error("trying to set row %d", i);
//    }
//    }

//    /** Scale the transformation matrix. */
//    public void scale(double s){
//    scale(s, s, s);
//    }
    
//    /** Apply non uniform scale. */
//    public void scale(double sx, double sy, double sz){ 
//    p[0] *= sx; p[4] *= sy; p[8] *= sz;
//    p[1] *= sx; p[5] *= sy; p[9] *= sz;
//    p[2] *= sx; p[6] *= sy; p[10] *= sz;
//    p[3] *= sx; p[7] *= sy; p[11] *= sz;
//    }
    
//    /** Translate the transformation matrix. */
//    public void translate(double tx, double ty, double tz){
//    p[0] += p[12]*tx; p[4] += p[12]*ty; p[8] += p[12]*tz;
//    p[1] += p[13]*tx; p[5] += p[13]*ty; p[9] += p[13]*tz;
//    p[2] += p[14]*tx; p[6] += p[14]*ty; p[10] += p[14]*tz;
//    p[3] += p[15]*tx; p[7] += p[15]*ty; p[11] += p[15]*tz;
//    }

//    private static Matrix workMatrix = new Matrix();

//    /** Translate the transformation matrix the other way. */
//    public void pretranslate(double tx, double ty, double tz){
//    p[3] = tx*p[0] + ty*p[1] + tz*p[2];
//    p[7] = tx*p[4] + ty*p[5] + tz*p[6];
//    p[11] = tx*p[8] + ty*p[9] + tz*p[10];
//    }
    
//    /** Rotate around x in degrees. */
//    public void rotateXdegrees(double d){
//    double r = d*Math.PI / 180.0;
//    double c = Math.cos(r);
//    double s = Math.sin(r);

//    double t = 0.0;
//    t = p[4]; p[4] = t*c - p[8]*s; p[8] = t*s + p[8]*c; 
//    t = p[5]; p[5] = t*c - p[9]*s; p[9] = t*s + p[9]*c; 
//    t = p[6]; p[6] = t*c - p[10]*s; p[10] = t*s + p[10]*c; 
//    t = p[7]; p[7] = t*c - p[11]*s; p[11] = t*s + p[11]*c; 
//    }

//    /** Rotate around y in degrees. */
//    public void rotateYdegrees(double d){
//    double r = d*Math.PI / 180.0;
//    double c = Math.cos(r);
//    double s = Math.sin(r);

//    double t = 0.0;
//    t = p[0]; p[0] = t*c + p[8]*s; p[8] = p[8]*c - t*s;
//    t = p[1]; p[1] = t*c + p[9]*s; p[9] = p[9]*c - t*s;
//    t = p[2]; p[2] = t*c + p[10]*s; p[10] = p[10]*c - t*s;
//    t = p[3]; p[3] = t*c + p[11]*s; p[11] = p[11]*c - t*s;
//    }

//    /** Rotate around Z in degrees. */
//    public void rotateZdegrees(double d){
//    double r = d*Math.PI / 180.0;
//    double c = Math.cos(r);
//    double s = Math.sin(r);

//    Matrix m = new Matrix();
//    m.rotateAroundVector(0., 0., 1., r);
//    transform(m);
	
//    // this is wrong...
//    //double t = 0.0;
//    //t = p[0]; p[0] = t*c + p[4]*s; p[4] = t*s - p[4]*c;
//    //t = p[1]; p[1] = t*c + p[5]*s; p[5] = t*s - p[5]*c;
//    //t = p[2]; p[2] = t*c + p[6]*s; p[6] = t*s - p[6]*c;
//    //t = p[3]; p[3] = t*c + p[7]*s; p[7] = t*s - p[7]*c;
//    }

//    /** Transform by another matrix. */
//    public void transform(Matrix m){
//    double xp[0] = p[0], xp[4] = p[4], xp[8] = p[8], xp[12] = p[12];
//    double xp[1] = p[1], xp[5] = p[5], xp[9] = p[9], xp[13] = p[13];
//    double xp[2] = p[2], xp[6] = p[6], xp[10] = p[10], xp[14] = p[14];
//    double xp[3] = p[3], xp[7] = p[7], xp[11] = p[11], xp[15] = p[15];
	
//    p[0] = xp[0]*m.p[0] + xp[4]*m.p[1] + xp[8]*m.p[2] + xp[12]*m.p[3];
//    p[4] = xp[0]*m.p[4] + xp[4]*m.p[5] + xp[8]*m.p[6] + xp[12]*m.p[7];
//    p[8] = xp[0]*m.p[8] + xp[4]*m.p[9] + xp[8]*m.p[10] + xp[12]*m.p[11];
//    p[12] = xp[0]*m.p[12] + xp[4]*m.p[13] + xp[8]*m.p[14] + xp[12]*m.p[15];
	
//    p[1] = xp[1]*m.p[0] + xp[5]*m.p[1] + xp[9]*m.p[2] + xp[13]*m.p[3];
//    p[5] = xp[1]*m.p[4] + xp[5]*m.p[5] + xp[9]*m.p[6] + xp[13]*m.p[7];
//    p[9] = xp[1]*m.p[8] + xp[5]*m.p[9] + xp[9]*m.p[10] + xp[13]*m.p[11];
//    p[13] = xp[1]*m.p[12] + xp[5]*m.p[13] + xp[9]*m.p[14] + xp[13]*m.p[15];
	
//    p[2] = xp[2]*m.p[0] + xp[6]*m.p[1] + xp[10]*m.p[2] + xp[14]*m.p[3];
//    p[6] = xp[2]*m.p[4] + xp[6]*m.p[5] + xp[10]*m.p[6] + xp[14]*m.p[7];
//    p[10] = xp[2]*m.p[8] + xp[6]*m.p[9] + xp[10]*m.p[10] + xp[14]*m.p[11];
//    p[14] = xp[2]*m.p[12] + xp[6]*m.p[13] + xp[10]*m.p[14] + xp[14]*m.p[15];
	
//    p[3] = xp[3]*m.p[0] + xp[7]*m.p[1] + xp[11]*m.p[2] + xp[15]*m.p[3];
//    p[7] = xp[3]*m.p[4] + xp[7]*m.p[5] + xp[11]*m.p[6] + xp[15]*m.p[7];
//    p[11] = xp[3]*m.p[8] + xp[7]*m.p[9] + xp[11]*m.p[10] + xp[15]*m.p[11];
//    p[15] = xp[3]*m.p[12] + xp[7]*m.p[13] + xp[11]*m.p[14] + xp[15]*m.p[15];
//    }
    
//    /** Transform a point by the current matrix. */
//    public void transform(Point3d p){
//    double x = p.x, y = p.y, z = p.z;
//    p.x = x*p[0] + y*p[1] + z*p[2] + p[3];
//    p.y = x*p[4] + y*p[5] + z*p[6] + p[7];
//    p.z = x*p[8] + y*p[9] + z*p[10] + p[11];
//    }

//    /** Transform a point by the inverse matrix (assumes rotation matrix) */
//    public void transformByInverse(Point3d p){
//    double x = p.x, y = p.y, z = p.z;
//    // don't need translation part here.
//    p.x = x*p[0] + y*p[4] + z*p[8];
//    p.y = x*p[1] + y*p[5] + z*p[9];
//    p.z = x*p[2] + y*p[6] + z*p[10];
//    }
    
//    /** Rotate around a line. */
//    public void rotateAroundVector(Point3d p, double theta){
//    rotateAroundVector(p.x, p.y, p.z, theta);
//    }
    
//    /** Rotate around a line. */
//    public void rotateAroundVector(double x, double y, double z,
//                   double theta){
//    double d = x*x + y*y + z*z;

//    if(d > 1.e-3){
//        d = Math.sqrt(d);
//        x /= d;
//        y /= d;
//        z /= d;
//    }else{
//        System.out.println("rotateAroundVector: direction is zero length");
//        return;
//    }

//    double s = Math.sin(theta);
//    double c = Math.cos(theta);
//    double t = 1.0 - c;

//    setIdentity();
	
//    p[0] = t * x * x + c;	/* leading diagonal */
//    p[5] = t * y * y + c;
//    p[10] = t * z * z + c;
	
//    p[1] = t * x * y + s * z;	/* off diagonal elements */
//    p[2] = t * x * z - s * y;
	
//    p[4] = t * x * y - s * z;
//    p[6] = t * y * z + s * x;
	
//    p[8] = t * x * z + s * y;
//    p[9] = t * y * z - s * x;
//    }
    
//    /** A format object for printing matrices. */
//    private static Format f6 = new Format("%11.6f");
    
//    /** Print a default message with the matrix. */
//    public void print(){
//    print("-----------------");
//    }
    
//    /** Print the matrix. */
//    public void print(String message){
//    System.out.println(message);
//    System.out.println("" + f6.format(p[0]) + " " + f6.format(p[4]) +
//               " " + f6.format(p[8]) + " " + f6.format(p[12]));
//    System.out.println("" + f6.format(p[1]) + " " + f6.format(p[5]) +
//               " " + f6.format(p[9]) + " " + f6.format(p[13]));
//    System.out.println("" + f6.format(p[2]) + " " + f6.format(p[6]) +
//               " " + f6.format(p[10]) + " " + f6.format(p[14]));
//    System.out.println("" + f6.format(p[3]) + " " + f6.format(p[7]) + 
//               " " + f6.format(p[11]) + " " + f6.format(p[15]));
//    }

//    public String returnScript(){
//    String command = "matrix ";
//    command += FILE.sprint(" %g", p[0]) + FILE.sprint(" %g", p[4]) + FILE.sprint(" %g", p[8]) + FILE.sprint(" %g", p[12]);
//    command += FILE.sprint(" %g", p[1]) + FILE.sprint(" %g", p[5]) + FILE.sprint(" %g", p[9]) + FILE.sprint(" %g", p[13]);
//    command += FILE.sprint(" %g", p[2]) + FILE.sprint(" %g", p[6]) + FILE.sprint(" %g", p[10]) + FILE.sprint(" %g", p[14]);
//    command += FILE.sprint(" %g", p[3]) + FILE.sprint(" %g", p[7]) + FILE.sprint(" %g", p[11]) + FILE.sprint(" %g", p[15]);
//    command += ";";

//    return command;
//    }

//    /** Small number for matrix equivalence. */
//    private static final double TOL = 1.e-5;

//    /** Does this matrix equal another matrix. */
//    public boolean equals(Matrix m){
//    if(Math.abs(p[0] - m.p[0]) > TOL) return false;
//    if(Math.abs(p[4] - m.p[4]) > TOL) return false;
//    if(Math.abs(p[8] - m.p[8]) > TOL) return false;
//    if(Math.abs(p[12] - m.p[12]) > TOL) return false;
//    if(Math.abs(p[1] - m.p[1]) > TOL) return false;
//    if(Math.abs(p[5] - m.p[5]) > TOL) return false;
//    if(Math.abs(p[9] - m.p[9]) > TOL) return false;
//    if(Math.abs(p[13] - m.p[13]) > TOL) return false;
//    if(Math.abs(p[2] - m.p[2]) > TOL) return false;
//    if(Math.abs(p[6] - m.p[6]) > TOL) return false;
//    if(Math.abs(p[10] - m.p[10]) > TOL) return false;
//    if(Math.abs(p[14] - m.p[14]) > TOL) return false;
//    if(Math.abs(p[3] - m.p[3]) > TOL) return false;
//    if(Math.abs(p[7] - m.p[7]) > TOL) return false;
//    if(Math.abs(p[11] - m.p[11]) > TOL) return false;
//    if(Math.abs(p[15] - m.p[15]) > TOL) return false;

//    return true;
//    }

//    /** Does this matrix equal another matrix. */
//    public boolean isIdentity(){
//    return isIdentity(TOL);
//    }

//    public boolean isIdentity(double tol){
//    if(Math.abs(p[0] - 1.0) > tol) return false;
//    if(Math.abs(p[4])       > tol) return false;
//    if(Math.abs(p[8])       > tol) return false;
//    if(Math.abs(p[12])       > tol) return false;
//    if(Math.abs(p[1])       > tol) return false;
//    if(Math.abs(p[5] - 1.0) > tol) return false;
//    if(Math.abs(p[9])       > tol) return false;
//    if(Math.abs(p[13])       > tol) return false;
//    if(Math.abs(p[2])       > tol) return false;
//    if(Math.abs(p[6])       > tol) return false;
//    if(Math.abs(p[10] - 1.0) > tol) return false;
//    if(Math.abs(p[14])       > tol) return false;
//    if(Math.abs(p[3])       > tol) return false;
//    if(Math.abs(p[7])       > tol) return false;
//    if(Math.abs(p[11])       > tol) return false;
//    if(Math.abs(p[15] - 1.0) > tol) return false;

//    return true;
//    }

//    /** Copy m into this matrix. */
//    public void copy(Matrix m){
//    p[0] = m.p[0]; p[4] = m.p[4]; p[8] = m.p[8]; p[12] = m.p[12];
//    p[1] = m.p[1]; p[5] = m.p[5]; p[9] = m.p[9]; p[13] = m.p[13];
//    p[2] = m.p[2]; p[6] = m.p[6]; p[10] = m.p[10]; p[14] = m.p[14];
//    p[3] = m.p[3]; p[7] = m.p[7]; p[11] = m.p[11]; p[15] = m.p[15];
//    }

//    /** Transpose the matrix. */
//    public void transpose(){
//    double tmp;

//    // remember only transpose once
//    tmp = p[4]; p[4] = p[1]; p[1] = tmp;
//    tmp = p[8]; p[8] = p[2]; p[2] = tmp;
//    tmp = p[12]; p[12] = p[3]; p[3] = tmp;

//    tmp = p[9]; p[9] = p[6]; p[6] = tmp;
//    tmp = p[13]; p[13] = p[7]; p[7] = tmp;

//    tmp = p[14]; p[14] = p[11]; p[11] = tmp;
//    }

//    /*
//     * Matrix Inversion
//     * by Richard Carling
//     * from "Graphics Gems", Academic Press, 1990
//     */

//    /** A small number. */
//    private static final double SMALL_NUMBER = 1.e-8;

//    /**
//     *   invert( original_matrix, inverse_matrix )
//     * 
//     *    calculate the inverse of a 4x4 matrix
//     *
//     *     -1     
//     *     A  = ___1__ adjoint A
//     *         det A
//     */
//    public static void invert(Matrix in, Matrix out ){
//    /* calculate the adjoint matrix */
//    adjoint(in, out);

//    /*  calculate the 4x4 determinant
//     *  if the determinant is zero, 
//     *  then the inverse matrix is not unique.
//     */
//    double det = det4x4(in);

//    if(Math.abs(det) < SMALL_NUMBER){
//        System.err.println("Matrix.invert: Non-singular matrix, " +
//                   "no inverse");
//        return;
//    }

//    /* scale the adjoint matrix to get the inverse */
//    out.p[0] /= det; out.p[4] /= det; out.p[8] /= det; out.p[12] /= det;
//    out.p[1] /= det; out.p[5] /= det; out.p[9] /= det; out.p[13] /= det;
//    out.p[2] /= det; out.p[6] /= det; out.p[10] /= det; out.p[14] /= det;
//    out.p[3] /= det; out.p[7] /= det; out.p[11] /= det; out.p[15] /= det;
//    }

//    /**
//     *   adjoint( original_matrix, inverse_matrix )
//     * 
//     *     calculate the adjoint of a 4x4 matrix
//     *
//     *      Let  a   denote the minor determinant of matrix A obtained by
//     *            ij
//     *
//     *      deleting the ith row and jth column from A.
//     *
//     *                    i+j
//     *     Let  b   = (-1)    a
//     *           ij            ji
//     *
//     *    The matrix B = (b  ) is the adjoint of A
//     *                     ij
//     */
//    public static void adjoint(Matrix in, Matrix out){
//    double a1, a2, a3, a4, b1, b2, b3, b4;
//    double c1, c2, c3, c4, d1, d2, d3, d4;

//    /* assign to individual variable names to aid  */
//    /* selecting correct values  */

//    a1 = in.p[0]; b1 = in.p[4]; 
//    c1 = in.p[8]; d1 = in.p[12];

//    a2 = in.p[1]; b2 = in.p[5]; 
//    c2 = in.p[9]; d2 = in.p[13];

//    a3 = in.p[2]; b3 = in.p[6];
//    c3 = in.p[10]; d3 = in.p[14];

//    a4 = in.p[3]; b4 = in.p[7]; 
//    c4 = in.p[11]; d4 = in.p[15];


//    /* row column labeling reversed since we transpose rows & columns */

//    out.p[0] =   det3x3(b2, b3, b4, c2, c3, c4, d2, d3, d4);
//    out.p[1] = - det3x3(a2, a3, a4, c2, c3, c4, d2, d3, d4);
//    out.p[2] =   det3x3(a2, a3, a4, b2, b3, b4, d2, d3, d4);
//    out.p[3] = - det3x3(a2, a3, a4, b2, b3, b4, c2, c3, c4);
        
//    out.p[4] = - det3x3(b1, b3, b4, c1, c3, c4, d1, d3, d4);
//    out.p[5] =   det3x3(a1, a3, a4, c1, c3, c4, d1, d3, d4);
//    out.p[6] = - det3x3(a1, a3, a4, b1, b3, b4, d1, d3, d4);
//    out.p[7] =   det3x3(a1, a3, a4, b1, b3, b4, c1, c3, c4);
        
//    out.p[8] =   det3x3(b1, b2, b4, c1, c2, c4, d1, d2, d4);
//    out.p[9] = - det3x3(a1, a2, a4, c1, c2, c4, d1, d2, d4);
//    out.p[10] =   det3x3(a1, a2, a4, b1, b2, b4, d1, d2, d4);
//    out.p[11] = - det3x3(a1, a2, a4, b1, b2, b4, c1, c2, c4);
        
//    out.p[12] = - det3x3(b1, b2, b3, c1, c2, c3, d1, d2, d3);
//    out.p[13] =   det3x3(a1, a2, a3, c1, c2, c3, d1, d2, d3);
//    out.p[14] = - det3x3(a1, a2, a3, b1, b2, b3, d1, d2, d3);
//    out.p[15] =   det3x3(a1, a2, a3, b1, b2, b3, c1, c2, c3);
//    }

//    /**
//     * double = det4x4( matrix )
//     * 
//     * calculate the determinant of a 4x4 matrix.
//     */
//    private static double det4x4(Matrix m){
//    double a1, a2, a3, a4, b1, b2, b3, b4, c1, c2, c3, c4, d1, d2, d3, d4;

//    /* assign to individual variable names to aid selecting */
//    /*  correct elements */

//    a1 = m.p[0]; b1 = m.p[4]; 
//    c1 = m.p[8]; d1 = m.p[12];

//    a2 = m.p[1]; b2 = m.p[5]; 
//    c2 = m.p[9]; d2 = m.p[13];

//    a3 = m.p[2]; b3 = m.p[6]; 
//    c3 = m.p[10]; d3 = m.p[14];

//    a4 = m.p[3]; b4 = m.p[7]; 
//    c4 = m.p[11]; d4 = m.p[15];

//    double ans;

//    ans = a1 * det3x3(b2, b3, b4, c2, c3, c4, d2, d3, d4)
//        - b1 * det3x3(a2, a3, a4, c2, c3, c4, d2, d3, d4)
//        + c1 * det3x3(a2, a3, a4, b2, b3, b4, d2, d3, d4)
//        - d1 * det3x3(a2, a3, a4, b2, b3, b4, c2, c3, c4);
//    return ans;
//    }

//    /**
//     * double = det3x3(  a1, a2, a3, b1, b2, b3, c1, c2, c3 )
//     * 
//     * calculate the determinant of a 3x3 matrix
//     * in the form
//     *
//     *     | a1,  b1,  c1 |
//     *     | a2,  b2,  c2 |
//     *     | a3,  b3,  c3 |
//     */
//    private static double det3x3(double a1, double a2, double a3,
//                 double b1, double b2, double b3,
//                 double c1, double c2, double c3){
//    double ans;

//    ans = a1 * det2x2(b2, b3, c2, c3)
//        - b1 * det2x2(a2, a3, c2, c3)
//        + c1 * det2x2(a2, a3, b2, b3);
//    return ans;
//    }

//    /**
//     * double = det2x2( double a, double b, double c, double d )
//     * 
//     * calculate the determinant of a 2x2 matrix.
//     */
//    private static double det2x2(double a, double b, double c, double d){
//    double ans = a * d - b * c;
//    return ans;
//    }

//    /** Interpolate a new matrix. */
//    public static Matrix interpolate(Matrix MS, Matrix MF, double frac){
//    Matrix MI = new Matrix();

//    interpolate(MS, MF, frac, MI);

//    return MI;
//    }

//    /** Interpolate a new matrix. */
//    public static void interpolate(Matrix MS, Matrix MF, double frac, Matrix MI){
//    double qS[] = new double[4];
//    double qF[] = new double[4];
//    double qI[] = new double[4];

//    //MS.print("start");

//    MS.toQuaternion(qS);
//    MF.toQuaternion(qF);

//    slerp(qS, qF, frac, qI);

//    MI.fromQuaternion(qI);

//    //System.out.println("frac " + frac);
//    //MI.print("interpolated");
//    //MF.print("final");
//    }

//    /** Convert a matrix to a quaternion. */
//    public void toQuaternion(double q[]){
//    double trace = p[0] + p[5] + p[10] + 1.0;

//    if( trace > 1.e-7 ) {
//        double s = 0.5 / Math.sqrt(trace);
//        q[0] = ( p[6] - p[9] ) * s;
//        q[1] = ( p[8] - p[2] ) * s;
//        q[2] = ( p[1] - p[4] ) * s;
//        q[3] = 0.25 / s;
//    } else {
//        if ( p[0] > p[5] && p[0] > p[10] ) {
//        double s = 2.0 * Math.sqrt( 1.0 + p[0] - p[5] - p[10]);
//        q[0] = 0.25 * s;
//        q[1] = (p[4] + p[1] ) / s;
//        q[2] = (p[8] + p[2] ) / s;
//        q[3] = (p[9] - p[6] ) / s;
//        } else if (p[5] > p[10]) {
//        double s = 2.0 * Math.sqrt( 1.0 + p[5] - p[0] - p[10]);
//        q[0] = (p[4] + p[1] ) / s;
//        q[1] = 0.25 * s;
//        q[2] = (p[9] + p[6] ) / s;
//        q[3] = (p[8] - p[2] ) / s;    
//        } else {
//        double s = 2.0 * Math.sqrt( 1.0 + p[10] - p[0] - p[5] );
//        q[0] = (p[8] + p[2] ) / s;
//        q[1] = (p[9] + p[6] ) / s;
//        q[2] = 0.25 * s;
//        q[3] = (p[4] - p[1] ) / s;
//        }
//    }

//    double len = q[0]*q[0] +q[1]*q[1] +q[2]*q[2] +q[3]*q[3];

//    len = Math.sqrt(len);

//    for(int i = 0; i < 4; i++){
//        q[i] /= len;
//    }
//    }

//    /** Generate rotation matrix from quaternion. */
//    public void fromQuaternion(double q[]){
//    //fromQuaternion(q[3], q[0], q[1], q[2]);
//    double X = q[0];
//    double Y = q[1];
//    double Z = q[2];
//    double W = q[3];

//    double xx = X * X;
//    double xy = X * Y;
//    double xz = X * Z;
//    double xw = X * W;
//    double yy = Y * Y;
//    double yz = Y * Z;
//    double yw = Y * W;
//    double zz = Z * Z;
//    double zw = Z * W;

//    setIdentity();

//    p[0]  = 1 - 2 * ( yy + zz );
//    p[4]  =     2 * ( xy - zw );
//    p[8]  =     2 * ( xz + yw );
//    p[1]  =     2 * ( xy + zw );
//    p[5]  = 1 - 2 * ( xx + zz );
//    p[9]  =     2 * ( yz - xw );
//    p[2]  =     2 * ( xz - yw );
//    p[6]  =     2 * ( yz + xw );
//    p[10]  = 1 - 2 * ( xx + yy );
//    //mat[3]  = mat[7] = mat[11] = mat[12] = mat[13] = mat[14] = 0;
//    //mat[15] = 1;
//    /*
//    mat[0]  = 1 - 2 * ( yy + zz );
//    mat[1]  =     2 * ( xy - zw );
//    mat[2]  =     2 * ( xz + yw );
//    mat[4]  =     2 * ( xy + zw );
//    mat[5]  = 1 - 2 * ( xx + zz );
//    mat[6]  =     2 * ( yz - xw );
//    mat[8]  =     2 * ( xz - yw );
//    mat[9]  =     2 * ( yz + xw );
//    mat[10] = 1 - 2 * ( xx + yy );
//    mat[3]  = mat[7] = mat[11] = mat[12] = mat[13] = mat[14] = 0;
//    mat[15] = 1;
//    */
//    }

//    /**
//     * Generate rotation matrix from quaternion.
//     * Used by the fitting routine
//     */
//    public void fromQuaternion(double q1, double q2, double q3, double q4){
//    p[0] = q1*q1 + q2*q2 - q3*q3 - q4*q4;
//    p[1] = 2. * (q2*q3 - q1*q4);
//    p[2] = 2. * (q2*q4 + q1*q3);
//    p[3] = 0.0;

//    p[4] = 2. * (q2*q3 + q1*q4);
//    p[5] = q1*q1 - q2*q2 + q3*q3 - q4*q4;
//    p[6] = 2. * (q3*q4 - q1*q2);
//    p[7] = 0.0;

//    p[8] = 2. * (q2*q4 - q1*q3);
//    p[9] = 2. * (q3*q4 + q1*q2);
//    p[10] = q1*q1 - q2*q2 - q3*q3 + q4*q4;
//    p[11] = 0.0;

//    p[12] = p[13] = p[14] = 0.0;
//    p[15] = 1.0;
//    }

//    /** The famous quaternion slerp. */
//    public static void slerp(double Q0[], double Q1[], double T, double Result[]) {
//    //double CosTheta = Q0.DotProd(Q1);
//    double CosTheta = Q0[3]*Q1[3] - (Q0[0]*Q1[0] + Q0[1]*Q1[1] + Q0[2]*Q1[2]);
//    double Theta = Math.acos(CosTheta);
//    double SinTheta = Math.sqrt(1.0-CosTheta*CosTheta);
	
//        if(Math.abs(SinTheta) < 1.e-5){
//            for(int i = 0; i < 4; i++){
//                Result[i] = Q0[i];
//            }
//            return;
//        }

//    double Sin_T_Theta = Math.sin(T*Theta)/SinTheta;
//    double Sin_OneMinusT_Theta = Math.sin((1.0-T)*Theta)/SinTheta;

//    //Result = Q0*Sin_OneMinusT_Theta;
//    //Result += (Q1*Sin_T_Theta);
//    for(int i = 0; i < 4; i++){
//        Result[i] = Q0[i]*Sin_OneMinusT_Theta + Q1[i]*Sin_T_Theta;
//    }

//    double len =
//        Result[0]*Result[0] +
//        Result[1]*Result[1] +
//        Result[2]*Result[2] +
//        Result[3]*Result[3];
//    len = Math.sqrt(len);

//    for(int i = 0; i < 4; i++){
//        Result[i] /= len;
//    }

//    }
//}

    }
}
