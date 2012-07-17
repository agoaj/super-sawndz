using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrawlLib.OpenGL;
using System.Reflection;
using System.Runtime.InteropServices;
using BrawlLib.OpenGL.etc;

namespace BrawlLib.OpenGL
{
    public delegate T GLCreateHandler<T>(GLContext ctx);

    public unsafe abstract partial class GLContext : IDisposable
    {
        internal Dictionary<string, object> _states = new Dictionary<string, object>();
        public T FindOrCreate<T>(string name, GLCreateHandler<T> handler)
        {
            if (_states.ContainsKey(name))
                return (T)_states[name];
            T obj = handler(this);
            _states[name] = obj;
            return obj;
        }

        public void Unbind()
        {
            try
            {
                Capture();
                foreach (object o in _states.Values)
                {
                    if (o is GLDisplayList)
                        (o as GLDisplayList).Delete();
                    else if (o is GLTexture)
                        (o as GLTexture).Delete();
                }
            }
            catch { }
            _states.Clear();
        }

        public virtual void Share(GLContext ctx) { }

        public virtual void Dispose() { }

        public static GLContext Attach(Control target)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT: return new wGlContext(target);
            }
            return null;
        }

        public void CheckErrors()
        {
            GLErrorCode code;
            if ((code = glGetError()) == GLErrorCode.NO_ERROR)
                return;

            //throw new Exception(code.ToString());
        }

        public virtual void Capture() { }
        public virtual void Swap() { }
        public virtual void Release() { }

        public unsafe void DrawBox(Vector3 p1, Vector3 p2)
        {
            glBegin(GLPrimitiveType.QuadStrip);

            glVertex(p1._x, p1._y, p1._z);
            glVertex(p1._x, p2._y, p1._z);
            glVertex(p2._x, p1._y, p1._z);
            glVertex(p2._x, p2._y, p1._z);
            glVertex(p2._x, p1._y, p2._z);
            glVertex(p2._x, p2._y, p2._z);
            glVertex(p1._x, p1._y, p2._z);
            glVertex(p1._x, p2._y, p2._z);
            glVertex(p1._x, p1._y, p1._z);
            glVertex(p1._x, p2._y, p1._z);

            glEnd();

            glBegin(GLPrimitiveType.Quads);

            glVertex(p1._x, p2._y, p1._z);
            glVertex(p1._x, p2._y, p2._z);
            glVertex(p2._x, p2._y, p2._z);
            glVertex(p2._x, p2._y, p1._z);

            glVertex(p1._x, p1._y, p1._z);
            glVertex(p1._x, p1._y, p2._z);
            glVertex(p2._x, p1._y, p2._z);
            glVertex(p2._x, p1._y, p1._z);

            glEnd();
        }

        public unsafe void DrawInvertedBox(Vector3 p1, Vector3 p2)
        {
            glBegin(GLPrimitiveType.QuadStrip);

            glVertex(p1._x, p1._y, p1._z);
            glVertex(p1._x, p2._y, p1._z);
            glVertex(p1._x, p1._y, p2._z);
            glVertex(p1._x, p2._y, p2._z);
            glVertex(p2._x, p1._y, p2._z);
            glVertex(p2._x, p2._y, p2._z);
            glVertex(p2._x, p1._y, p1._z);
            glVertex(p2._x, p2._y, p1._z);
            glVertex(p1._x, p1._y, p1._z);
            glVertex(p1._x, p2._y, p1._z);

            glEnd();

            glBegin(GLPrimitiveType.Quads);

            glVertex(p2._x, p2._y, p1._z);
            glVertex(p2._x, p2._y, p2._z);
            glVertex(p1._x, p2._y, p2._z);
            glVertex(p1._x, p2._y, p1._z);

            glVertex(p1._x, p1._y, p1._z);
            glVertex(p1._x, p1._y, p2._z);
            glVertex(p2._x, p1._y, p2._z);
            glVertex(p2._x, p1._y, p1._z);

            glEnd();
        }

        public void DrawCube(Vector3 p, float radius)
        {
            Vector3 p1 = new Vector3(p._x + radius, p._y + radius, p._z + radius);
            Vector3 p2 = new Vector3(p._x - radius, p._y - radius, p._z - radius);
            DrawBox(p2, p1);
        }

        public void DrawInvertedCube(Vector3 p, float radius)
        {
            Vector3 p1 = new Vector3(p._x + radius, p._y + radius, p._z + radius);
            Vector3 p2 = new Vector3(p._x - radius, p._y - radius, p._z - radius);
            DrawInvertedBox(p2, p1);
        }

        public void DrawRing(float radius)
        {
            glPushMatrix();
            glScale(radius, radius, radius);
            GetRingList().Call();
            glPopMatrix();
        }

        public GLDisplayList GetLine() { return FindOrCreate<GLDisplayList>("Line", CreateLine); }
        private static GLDisplayList CreateLine(GLContext ctx)
        {
            GLDisplayList list = new GLDisplayList(ctx);
            ctx.glBegin(GLPrimitiveType.Lines);

            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(2.0f, 0.0f, 0.0f);

            ctx.glEnd();

            list.End();
            return list;
        }

        public GLDisplayList GetRingList() { return FindOrCreate<GLDisplayList>("Ring", CreateRing); }
        private static GLDisplayList CreateRing(GLContext ctx)
        {
            GLDisplayList list = new GLDisplayList(ctx);
            list.Begin();

            ctx.glBegin(GLPrimitiveType.LineLoop);

            float angle = 0.0f;
            for (int i = 0; i < 360; i++, angle = i * Maths._deg2radf)
                ctx.glVertex(Math.Cos(angle), Math.Sin(angle));

            ctx.glEnd();

            list.End();
            return list;
        }

        public GLDisplayList GetSquareList() { return FindOrCreate<GLDisplayList>("Square", CreateSquare); }
        private static GLDisplayList CreateSquare(GLContext ctx)
        {
            GLDisplayList list = new GLDisplayList(ctx);
            list.Begin();

            ctx.glBegin(GLPrimitiveType.LineLoop);

            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(0.0f, 0.0f, 1.0f);
            ctx.glVertex(0.0f, 1.0f, 1.0f);
            ctx.glVertex(0.0f, 1.0f, 0.0f);
            ctx.glVertex(0.0f, 0.0f, 0.0f);

            ctx.glEnd();

            list.End();
            return list;
        }

        public GLDisplayList GetAxisList() { return FindOrCreate<GLDisplayList>("Axes", CreateAxes); }
        private static GLDisplayList CreateAxes(GLContext ctx)
        {
            GLDisplayList list = new GLDisplayList(ctx);
            list.Begin();

            ctx.glBegin(GLPrimitiveType.Lines);

            ctx.glColor(1.0f, 0.0f, 0.0f, 1.0f);

            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(2.0f, 0.0f, 0.0f);
            ctx.glVertex(1.0f, 0.0f, 0.0f);
            ctx.glVertex(1.0f, 1.0f, 0.0f);
            ctx.glVertex(1.0f, 0.0f, 0.0f);
            ctx.glVertex(1.0f, 0.0f, 1.0f);

            ctx.glColor(0.0f, 1.0f, 0.0f, 1.0f);

            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(0.0f, 2.0f, 0.0f);
            ctx.glVertex(0.0f, 1.0f, 0.0f);
            ctx.glVertex(1.0f, 1.0f, 0.0f);
            ctx.glVertex(0.0f, 1.0f, 0.0f);
            ctx.glVertex(0.0f, 1.0f, 1.0f);

            ctx.glColor(0.0f, 0.0f, 1.0f, 1.0f);

            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(0.0f, 0.0f, 2.0f);
            ctx.glVertex(0.0f, 0.0f, 1.0f);
            ctx.glVertex(1.0f, 0.0f, 1.0f);
            ctx.glVertex(0.0f, 0.0f, 1.0f);
            ctx.glVertex(0.0f, 1.0f, 1.0f);

            ctx.glEnd();

            list.End();
            return list;
        }
        public GLDisplayList GetCubeList() { return FindOrCreate<GLDisplayList>("Cube", CreateCube); }
        private static GLDisplayList CreateCube(GLContext ctx)
        {
            GLDisplayList list = new GLDisplayList(ctx);
            list.Begin();

            ctx.glBegin(GLPrimitiveType.QuadStrip);

            Vector3 p1 = new Vector3(0);
            Vector3 p2 = new Vector3(0.99f);

            ctx.glVertex(p1._x, p1._y, p1._z);
            ctx.glVertex(p1._x, p2._y, p1._z);
            ctx.glVertex(p2._x, p1._y, p1._z);
            ctx.glVertex(p2._x, p2._y, p1._z);
            ctx.glVertex(p2._x, p1._y, p2._z);
            ctx.glVertex(p2._x, p2._y, p2._z);
            ctx.glVertex(p1._x, p1._y, p2._z);
            ctx.glVertex(p1._x, p2._y, p2._z);
            ctx.glVertex(p1._x, p1._y, p1._z);
            ctx.glVertex(p1._x, p2._y, p1._z);

            ctx.glEnd();

            ctx.glBegin(GLPrimitiveType.Quads);

            ctx.glVertex(p1._x, p2._y, p1._z);
            ctx.glVertex(p1._x, p2._y, p2._z);
            ctx.glVertex(p2._x, p2._y, p2._z);
            ctx.glVertex(p2._x, p2._y, p1._z);

            ctx.glVertex(p1._x, p1._y, p1._z);
            ctx.glVertex(p1._x, p1._y, p2._z);
            ctx.glVertex(p2._x, p1._y, p2._z);
            ctx.glVertex(p2._x, p1._y, p1._z);

            ctx.glEnd();

            list.End();
            return list;
        }

        public GLDisplayList GetCircleList() { return FindOrCreate<GLDisplayList>("Circle", CreateCircle); }
        private static GLDisplayList CreateCircle(GLContext ctx)
        {
            GLDisplayList list = new GLDisplayList(ctx);
            list.Begin();

            ctx.glBegin(GLPrimitiveType.TriangleFan);

            ctx.glVertex(0.0f, 0.0f, 0.0f);

            float angle = 0.0f;
            for (int i = 0; i < 361; i++, angle = i * Maths._deg2radf)
                ctx.glVertex(Math.Cos(angle), Math.Sin(angle));

            ctx.glEnd();

            list.End();
            return list;
        }

        public void DrawSphere(float radius)
        {
            glPushMatrix();
            glScale(radius, radius, radius);
            GetSphereList().Call();
            glPopMatrix();
        }
        public GLDisplayList GetSphereList() { return FindOrCreate<GLDisplayList>("Sphere", CreateSphere); }
        private static GLDisplayList CreateSphere(GLContext ctx)
        {
            int quad = ctx.gluNewQuadric();
            ctx.gluQuadricDrawStyle(quad, GLUQuadricDrawStyle.GLU_FILL);
            ctx.gluQuadricOrientation(quad, GLUQuadricOrientation.Outside);

            GLDisplayList dl = new GLDisplayList(ctx);

            dl.Begin();
            ctx.gluSphere(quad, 1.0f, 40, 40);
            dl.End();

            ctx.gluDeleteQuadric(quad);

            return dl;
        }

        internal abstract void glAccum(GLAccumOp op, float value);
        internal abstract void glActiveTexture(GLMultiTextureTarget texture);
        internal abstract void glAlphaFunc(GLAlphaFunc func, float refValue);
        internal abstract bool glAreTexturesResident(int num, uint* textures, bool* residences);
        internal abstract void glArrayElement(int index);
        internal abstract void glBegin(GLPrimitiveType mode);
        internal abstract void glBindTexture(GLTextureTarget target, uint texture);
        internal abstract void glBitmap(int width, int height, float xorig, float yorig, float xmove, float ymove, byte* bitmap);
        internal abstract void glBlendFunc(GLBlendFactor sfactor, GLBlendFactor dfactor);
        internal void glCallList(GLDisplayList list) { glCallList(list._id); }
        internal abstract void glBindBuffer(BufferTarget target, Int32 buffer);
        internal abstract void glCallList(uint list);
        internal abstract void glCallLists(int n, uint type, void* lists);
        internal abstract void glClear(GLClearMask mask);
        internal abstract void glClearAccum(float red, float green, float blue, float alpha);
        internal abstract void glClearColor(float red, float green, float blue, float alpha);
        internal abstract void glClearDepth(double depth);
        internal abstract void glClearIndex(float c);
        internal abstract void glClearStencil(int s);
        internal abstract void glClipPlane(uint plane, double* equation);

        #region glColor

        internal abstract void glColor(sbyte red, sbyte green, sbyte blue);
        internal abstract void glColor(double red, double green, double blue);
        internal abstract void glColor(float red, float green, float blue);
        internal abstract void glColor(int red, int green, int blue);
        internal abstract void glColor(short red, short green, short blue);
        internal abstract void glColor(byte red, byte green, byte blue);
        internal abstract void glColor(uint red, uint green, uint blue);
        internal abstract void glColor(ushort red, ushort green, ushort blue);

        internal abstract void glColor(sbyte red, sbyte green, sbyte blue, sbyte alpha);
        internal abstract void glColor(double red, double green, double blue, double alpha);
        internal abstract void glColor(float red, float green, float blue, float alpha);
        internal abstract void glColor(int red, int green, int blue, int alpha);
        internal abstract void glColor(short red, short green, short blue, short alpha);
        internal abstract void glColor(byte red, byte green, byte blue, byte alpha);
        internal abstract void glColor(uint red, uint green, uint blue, uint alpha);
        internal abstract void glColor(ushort red, ushort green, ushort blue, ushort alpha);

        internal abstract void glColor3(sbyte* v);
        internal abstract void glColor3(double* v);
        internal abstract void glColor3(float* v);
        internal abstract void glColor3(int* v);
        internal abstract void glColor3(short* v);
        internal abstract void glColor3(byte* v);
        internal abstract void glColor3(uint* v);
        internal abstract void glColor3(ushort* v);

        internal abstract void glColor4(sbyte* v);
        internal abstract void glColor4(double* v);
        internal abstract void glColor4(float* v);
        internal abstract void glColor4(int* v);
        internal abstract void glColor4(short* v);
        internal abstract void glColor4(byte* v);
        internal abstract void glColor4(uint* v);
        internal abstract void glColor4(ushort* v);

        #endregion

        internal abstract void glColorMask(bool red, bool green, bool blue, bool alpha);
        internal abstract void glColorMaterial(GLFace face, GLMaterialParameter mode);
        internal abstract void glColorPointer(int size, GLDataType type, int stride, void* pointer);
        internal abstract void glCopyPixels(int x, int y, int width, int height, uint type);

        #region CopyTex

        internal abstract void glCopyTexImage1D(GLTextureTarget target, int level, GLInternalPixelFormat internalFormat, int x, int y, int width, int border);
        internal abstract void glCopyTexImage2D(GLTextureTarget target, int level, GLInternalPixelFormat internalFormat, int x, int y, int width, int height, int border);
        internal abstract void glCopyTexSubImage1D(GLTextureTarget target, int level, int xOffset, int x, int y, int width);
        internal abstract void glCopyTexSubImage2D(GLTextureTarget target, int level, int xOffset, int yOffset, int x, int y, int width, int height);

        #endregion

        internal abstract int glCreateProgram();
        public unsafe void ShaderSource(int shader, string @string)
        {
            int length = @string.Length;
            glShaderSource((uint)shader, 1, new string[] { @string }, &length);
        }
        internal abstract void glShaderSource(uint shader, int count, string[] @string, int* length);
        internal abstract int glCreateShader(ShaderType type);
        internal abstract void glBindProgramARB(AssemblyProgramTargetArb target, int program);
        internal abstract void glGenProgramsARB(int n, uint* programs);
        internal abstract void glCompileShader(uint shader);
        //internal abstract void GetShaderInfoLog
        //internal abstract void GetShader
        internal abstract void glAttachShader(uint program, uint shader);
        internal abstract void glLinkProgram(uint program);
        internal abstract void glUseProgram(uint program);
        internal abstract void glProgramStringARB(AssemblyProgramTargetArb target, ArbVertexProgram format, int len, string @string);
        internal abstract void glUniform1(int location, int v0);
        internal abstract int glGetUniformLocation(uint program, string name);
        internal abstract int glCreateShaderObjectARB(ArbShaderObjects shaderType);
        //internal abstract int glAttachObjectARB(Int32 containerObj, Int32 obj);
        internal abstract int glUseProgramObjectARB(uint programObj);
        //internal abstract int glGetShaderiv(uint shader, ShaderParameter pname);

        internal abstract void glCullFace(GLFace mode);
        /*
        [DllImport("opengl32.dll")]
        internal abstract ?? glDebugEntry(??);
         * */
        public void glDeleteList(GLDisplayList list) { glDeleteLists(list._id, 1); list._id = 0; }
        internal abstract void glDeleteLists(uint list, int range);
        internal void glDeleteTexture(GLTexture texture)
        {
            uint id = texture._id;
            glDeleteTextures(1, &id);
            texture._id = 0;
        }
        internal abstract void glDeleteTextures(int num, uint* textures);
        internal abstract void glDepthFunc(GLFunction func);
        internal abstract void glDepthMask(bool flag);
        internal abstract void glDepthRange(double near, double far);
        internal abstract void glDisable(uint cap);
        internal abstract void glDisableClientState(GLArrayType cap);
        internal abstract void glDrawArrays(GLPrimitiveType mode, int first, int count);
        internal abstract void glDrawBuffer(uint mode);
        internal abstract void glDrawElements(GLPrimitiveType mode, int count, GLElementType type, void* indices);
        internal abstract void glDrawPixels(int width, int height, GLPixelDataFormat format, GLPixelDataType type, void* pixels);
        internal abstract void glEdgeFlag(bool flag);
        internal abstract void glEdgeFlagPointer(int stride, bool* pointer);
        internal abstract void glEdgeFlagv(bool* flag);
        internal abstract void glEnable(GLEnableCap cap);
        internal abstract void glEnableClientState(GLArrayType cap);
        internal abstract void glEnd();
        internal abstract void glEndList();

        #region glEvalCoord

        internal abstract void glEvalCoord(double u);
        internal abstract void glEvalCoord(float u);
        internal abstract void glEvalCoord(double u, double v);
        internal abstract void glEvalCoord(float u, float v);
        internal abstract void glEvalCoord1(double* u);
        internal abstract void glEvalCoord1(float* u);
        internal abstract void glEvalCoord2(double* u);
        internal abstract void glEvalCoord2(float* u);

        #endregion

        internal abstract void glEvalMesh(uint mode, int i1, int i2);
        internal abstract void glEvalMesh(uint mode, int i1, int i2, int j1, int j2);

        internal abstract void glEvalPoint(int i);
        internal abstract void glEvalPoint(int i, int j);

        internal abstract void glFeedbackBuffer(int size, uint type, out float* buffer);
        internal abstract void glFinish();
        internal abstract void glFlush();

        #region glFog

        internal abstract void glFog(FogParameter pname, float param);
        internal abstract void glFog(FogParameter pname, int param);
        internal abstract void glFog(FogParameter pname, float* param);
        internal abstract void glFog(FogParameter pname, int* param);

        #endregion

        internal abstract void glFrontFace(GLFrontFaceDirection mode);
        internal abstract void glFrustum(double left, double right, double bottom, double top, double near, double far);
        internal abstract uint glGenLists(int range);
        internal abstract void glGenTextures(int num, uint* textures);

        #region glGet

        internal abstract void glGet(GLGetMode pname, bool* param);
        internal abstract void glGet(GLGetMode pname, double* param);
        internal abstract void glGet(GLGetMode pname, float* param);
        internal abstract void glGet(GLGetMode pname, int* param);

        #endregion

        internal abstract void glGetClipPlane(uint plane, double* equation);
        internal abstract GLErrorCode glGetError();

        internal abstract void glGetLight(uint light, uint pname, float* param);
        internal abstract void glGetLight(uint light, uint pname, int* param);

        internal abstract void glGetMap(uint target, uint query, double* v);
        internal abstract void glGetMap(uint target, uint query, float* v);
        internal abstract void glGetMap(uint target, uint query, int* v);

        internal abstract void glGetMaterial(uint face, uint pname, float* param);
        internal abstract void glGetMaterial(uint face, uint pname, int* param);

        internal abstract void glGetPixelMap(uint map, float* values);
        internal abstract void glGetPixelMap(uint map, uint* values);
        internal abstract void glGetPixelMap(uint map, ushort* values);

        internal abstract void glGetPointer(uint name, void* values);
        internal abstract void glGetPolygonStipple(out byte* mask);
        internal abstract byte* glGetString(uint name);

        internal abstract void glGetTexEnv(uint target, uint pname, out float* param);
        internal abstract void glGetTexEnv(uint target, uint pname, out int* param);

        internal abstract void glGetTexGen(uint coord, uint pname, out double* param);
        internal abstract void glGetTexGen(uint coord, uint pname, out float* param);
        internal abstract void glGetTexGen(uint coord, uint pname, out int* param);

        internal abstract void glGetTexImage(uint target, uint format, uint type, out void* pixels);

        internal abstract void glGetTexLevelParameter(uint target, int level, uint pname, out float* param);
        internal abstract void glGetTexLevelParameter(uint target, int level, uint pname, out int* param);

        internal abstract void glGetTexParameter(uint target, uint pname, out float* param);
        internal abstract void glGetTexParameter(uint target, uint pname, out int* param);

        internal abstract void glHint(GLHintTarget target, GLHintMode mode);

        #region glIndex

        internal abstract void glIndex(double c);
        internal abstract void glIndex(float c);
        internal abstract void glIndex(int c);
        internal abstract void glIndex(short c);

        internal abstract void glIndex(double* c);
        internal abstract void glIndex(float* c);
        internal abstract void glIndex(int* c);
        internal abstract void glIndex(short* c);

        #endregion

        internal abstract void glIndexMask(uint mask);
        internal abstract void glIndexPointer(uint type, int stride, void* ptr);
        internal abstract void glInitNames();
        internal abstract void glInterleavedArrays(uint format, int stride, void* pointer);
        internal abstract bool glIsEnabled(uint cap);
        internal abstract bool glIsList(uint list);
        internal abstract bool glIsTexture(uint texture);

        #region glLight

        internal abstract bool glLight(GLLightTarget light, GLLightParameter pname, float param);
        internal abstract bool glLight(GLLightTarget light, GLLightParameter pname, int param);
        internal abstract bool glLight(GLLightTarget light, GLLightParameter pname, float* param);
        internal abstract bool glLight(GLLightTarget light, GLLightParameter pname, int* param);

        #endregion

        #region glLightModel

        internal abstract void glLightModel(uint pname, float param);
        internal abstract void glLightModel(uint pname, int param);
        internal abstract void glLightModel(uint pname, float* param);
        internal abstract void glLightModel(uint pname, int* param);

        #endregion

        internal abstract void glLineStipple(int factor, ushort pattern);
        internal abstract void glLineWidth(float width);
        internal abstract void glListBase(uint b);
        internal abstract void glLoadIdentity();
        internal abstract void glLoadMatrix(double* m);
        internal abstract void glLoadMatrix(float* m);
        internal abstract void glLoadName(uint name);
        internal abstract void glLogicOp(uint opcode);

        #region glMap

        internal abstract void glMap(uint target, double u1, double u2, int stride, int order, double* points);
        internal abstract void glMap(uint target, float u1, float u2, int stride, int order, float* points);
        internal abstract void glMap(uint target, double u1, double u2, int ustride, int uorder, double v1, double v2, int vstride, int vorder, double* points);
        internal abstract void glMap(uint target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder, float* points);

        #endregion

        #region glMapGrid

        internal abstract void glMapGrid(int un, double u1, double u2);
        internal abstract void glMapGrid(int un, float u1, float u2);
        internal abstract void glMapGrid(int un, double u1, double u2, int vn, double v1, double v2);
        internal abstract void glMapGrid(int un, float u1, float u2, int vn, float v1, float v2);

        #endregion

        #region glMaterial

        internal abstract void glMaterial(GLFace face, GLMaterialParameter pname, float param);
        internal abstract void glMaterial(GLFace face, GLMaterialParameter pname, int param);
        internal abstract void glMaterial(GLFace face, GLMaterialParameter pname, float* param);
        internal abstract void glMaterial(GLFace face, GLMaterialParameter pname, int* param);

        #endregion

        internal abstract void glMatrixMode(GLMatrixMode mode);

        #region glMultiTexCoord
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, short s);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, int s);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, float s);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, double s);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, short s, short t);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, int s, int t);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, float s, float t);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, double s, double t);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, short s, short t, short r);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, int s, int t, int r);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, float s, float t, float r);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, double s, double t, double r);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, short s, short t, short r, short q);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, int s, int t, int r, int q);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, float s, float t, float r, float q);
        //internal abstract void glMultiTexCoord(GLMultiTextureTarget target, double s, double t, double r, double q);
        //internal abstract void glMultiTexCoord1(GLMultiTextureTarget target, short* v);
        //internal abstract void glMultiTexCoord1(GLMultiTextureTarget target, int* v);
        //internal abstract void glMultiTexCoord1(GLMultiTextureTarget target, float* v);
        //internal abstract void glMultiTexCoord1(GLMultiTextureTarget target, double* v);
        //internal abstract void glMultiTexCoord2(GLMultiTextureTarget target, short* v);
        //internal abstract void glMultiTexCoord2(GLMultiTextureTarget target, int* v);
        internal abstract void glMultiTexCoord2(GLMultiTextureTarget target, float* v);
        //internal abstract void glMultiTexCoord2(GLMultiTextureTarget target, double* v);
        //internal abstract void glMultiTexCoord3(GLMultiTextureTarget target, short* v);
        //internal abstract void glMultiTexCoord3(GLMultiTextureTarget target, int* v);
        //internal abstract void glMultiTexCoord3(GLMultiTextureTarget target, float* v);
        //internal abstract void glMultiTexCoord3(GLMultiTextureTarget target, double* v);
        //internal abstract void glMultiTexCoord4(GLMultiTextureTarget target, short* v);
        //internal abstract void glMultiTexCoord4(GLMultiTextureTarget target, int* v);
        //internal abstract void glMultiTexCoord4(GLMultiTextureTarget target, float* v);
        //internal abstract void glMultiTexCoord4(GLMultiTextureTarget target, double* v);
        #endregion

        internal abstract void glMultMatrix(double* m);
        internal abstract void glMultMatrix(float* m);

        internal abstract void glNewList(uint list, GLListMode mode);

        #region glNormal

        internal abstract void glNormal(sbyte nx, sbyte ny, sbyte nz);
        internal abstract void glNormal(double nx, double ny, double nz);
        internal abstract void glNormal(float nx, float ny, float nz);
        internal abstract void glNormal(int nx, int ny, int nz);
        internal abstract void glNormal(short nx, short ny, short nz);

        internal abstract void glNormal(sbyte* v);
        internal abstract void glNormal(double* v);
        internal abstract void glNormal(float* v);
        internal abstract void glNormal(int* v);
        internal abstract void glNormal(short* v);

        #endregion

        internal abstract void glNormalPointer(GLDataType type, int stride, void* pointer);

        internal abstract void glOrtho(double left, double right, double bottom, double top, double near, double far);
        internal abstract void glPassThrough(float token);

        internal abstract void glPixelMap(uint map, int mapsize, float* v);
        internal abstract void glPixelMap(uint map, int mapsize, uint* v);
        internal abstract void glPixelMap(uint map, int mapsize, ushort* v);

        internal abstract void glPixelStore(uint pname, float param);
        internal abstract void glPixelStore(uint pname, int param);

        internal abstract void glPixelTransfer(uint pname, float param);
        internal abstract void glPixelTransfer(uint pname, int param);

        internal abstract void glPixelZoom(float xfactor, float yfactor);
        internal abstract void glPointSize(float size);
        internal abstract void glPolygonMode(GLFace face, GLPolygonMode mode);
        internal abstract void glPolygonOffset(float factor, float units);
        internal abstract void glPolygonStipple(byte* mask);

        internal abstract void glPopAttrib(uint mask);
        internal abstract void glPopClientAttrib(uint mask);
        internal abstract void glPopMatrix();
        internal abstract void glPopName();

        internal abstract void glPrioritizeTextures(int num, uint* textures, float* priorities);

        internal abstract void glPushAttrib(uint mask);
        internal abstract void glPushClientAttrib(uint mask);
        internal abstract void glPushMatrix();
        internal abstract void glPushName(uint name);

        #region glRasterPos

        internal abstract void glRasterPos(double x, double y);
        internal abstract void glRasterPos(float x, float y);
        internal abstract void glRasterPos(int x, int y);
        internal abstract void glRasterPos(short x, short y);

        internal abstract void glRasterPos(double x, double y, double z);
        internal abstract void glRasterPos(float x, float y, float z);
        internal abstract void glRasterPos(int x, int y, int z);
        internal abstract void glRasterPos(short x, short y, short z);

        internal abstract void glRasterPos(double x, double y, double z, double w);
        internal abstract void glRasterPos(float x, float y, float z, float w);
        internal abstract void glRasterPos(int x, int y, int z, int w);
        internal abstract void glRasterPos(short x, short y, short z, short w);

        internal abstract void glRasterPos2(double* v);
        internal abstract void glRasterPos2(float* v);
        internal abstract void glRasterPos2(int* v);
        internal abstract void glRasterPos2(short* v);

        internal abstract void glRasterPos3(double* v);
        internal abstract void glRasterPos3(float* v);
        internal abstract void glRasterPos3(int* v);
        internal abstract void glRasterPos3(short* v);

        internal abstract void glRasterPos4(double* v);
        internal abstract void glRasterPos4(float* v);
        internal abstract void glRasterPos4(int* v);
        internal abstract void glRasterPos4(short* v);

        #endregion

        internal abstract void glReadBuffer(uint mode);
        internal abstract void glReadPixels(int x, int y, int width, int height, GLPixelDataFormat format, GLPixelDataType type, void* pixels);

        #region glRect

        internal abstract void glRect(double x1, double y1, double x2, double y2);
        internal abstract void glRect(float x1, float y1, float x2, float y2);
        internal abstract void glRect(int x1, int y1, int x2, int y2);
        internal abstract void glRect(short x1, short y1, short x2, short y2);

        internal abstract void glRect(double* v1, double* v2);
        internal abstract void glRect(float* v1, float* v2);
        internal abstract void glRect(int* v1, int* v2);
        internal abstract void glRect(short* v1, short* v2);

        #endregion

        internal abstract int glRenderMode(uint mode);

        internal abstract void glRotate(double angle, double x, double y, double z);
        internal abstract void glRotate(float angle, float x, float y, float z);

        internal abstract void glScale(double x, double y, double z);
        internal abstract void glScale(float x, float y, float z);

        internal abstract void glScissor(int x, int y, int width, int height);
        internal abstract void glSelectBuffer(int size, out uint* buffer);
        internal abstract void glShadeModel(GLShadingModel mode);
        internal abstract void glStencilFunc(uint func, int refval, uint mask);
        internal abstract void glStencilMask(uint mask);
        internal abstract void glStencilOp(uint fail, uint zfail, uint zpass);

        #region glTexCoord

        internal abstract void glTexCoord(double s);
        internal abstract void glTexCoord(float s);
        internal abstract void glTexCoord(int s);
        internal abstract void glTexCoord(short s);

        internal abstract void glTexCoord(double s, double t);
        internal abstract void glTexCoord(float s, float t);
        internal abstract void glTexCoord(int s, int t);
        internal abstract void glTexCoord(short s, short t);

        internal abstract void glTexCoord(double s, double t, double r);
        internal abstract void glTexCoord(float s, float t, float r);
        internal abstract void glTexCoord(int s, int t, int r);
        internal abstract void glTexCoord(short s, short t, short r);

        internal abstract void glTexCoord(double s, double t, double r, double q);
        internal abstract void glTexCoord(float s, float t, float r, float q);
        internal abstract void glTexCoord(int s, int t, int r, int q);
        internal abstract void glTexCoord(short s, short t, short r, short q);

        internal abstract void glTexCoord1(double* v);
        internal abstract void glTexCoord1(float* v);
        internal abstract void glTexCoord1(int* v);
        internal abstract void glTexCoord1(short* v);

        internal abstract void glTexCoord2(double* v);
        internal abstract void glTexCoord2(float* v);
        internal abstract void glTexCoord2(int* v);
        internal abstract void glTexCoord2(short* v);

        internal abstract void glTexCoord3(double* v);
        internal abstract void glTexCoord3(float* v);
        internal abstract void glTexCoord3(int* v);
        internal abstract void glTexCoord3(short* v);

        internal abstract void glTexCoord4(double* v);
        internal abstract void glTexCoord4(float* v);
        internal abstract void glTexCoord4(int* v);
        internal abstract void glTexCoord4(short* v);

        #endregion

        internal abstract void glTexCoordPointer(int size, GLDataType type, int stride, void* pointer);

        internal abstract void glTexEnv(GLTexEnvTarget target, GLTexEnvParam pname, float param);
        internal abstract void glTexEnv(GLTexEnvTarget target, GLTexEnvParam pname, int param);
        internal abstract void glTexEnv(GLTexEnvTarget target, GLTexEnvParam pname, float* param);
        internal abstract void glTexEnv(GLTexEnvTarget target, GLTexEnvParam pname, int* param);

        #region glTexGen

        internal abstract void glTexGen(TextureCoordName coord, TextureGenParameter pname, double param);
        internal abstract void glTexGen(TextureCoordName coord, TextureGenParameter pname, float param);
        internal abstract void glTexGen(TextureCoordName coord, TextureGenParameter pname, int param);
        internal abstract void glTexGen(TextureCoordName coord, TextureGenParameter pname, double* param);
        internal abstract void glTexGen(TextureCoordName coord, TextureGenParameter pname, float* param);
        internal abstract void glTexGen(TextureCoordName coord, TextureGenParameter pname, int* param);

        #endregion

        internal abstract void glTexImage1D(GLTexImageTarget target, int level, GLInternalPixelFormat internalFormat, int width, int border, GLPixelDataFormat format, GLPixelDataType type, void* pixels);
        internal abstract void glTexImage2D(GLTexImageTarget target, int level, GLInternalPixelFormat internalFormat, int width, int height, int border, GLPixelDataFormat format, GLPixelDataType type, void* pixels);

        #region glTexParameter

        internal abstract void glTexParameter(GLTextureTarget target, GLTextureParameter pname, float param);
        internal abstract void glTexParameter(GLTextureTarget target, GLTextureParameter pname, int param);
        internal abstract void glTexParameter(GLTextureTarget target, GLTextureParameter pname, float* param);
        internal abstract void glTexParameter(GLTextureTarget target, GLTextureParameter pname, int* param);

        #endregion

        internal abstract void glTexSubImage1D(GLTexImageTarget target, int level, int xOffset, int width, GLPixelDataFormat format, GLPixelDataType type, void* pixels);
        internal abstract void glTexSubImage2D(GLTexImageTarget target, int level, int xOffset, int yOffset, int width, int height, GLPixelDataFormat format, GLPixelDataType type, void* pixels);

        internal abstract void glTranslate(double x, double y, double z);
        internal abstract void glTranslate(float x, float y, float z);

        #region glVertex

        internal abstract void glVertex(double x, double y);
        internal abstract void glVertex(float x, float y);
        internal abstract void glVertex(int x, int y);
        internal abstract void glVertex(short x, short y);

        internal abstract void glVertex(double x, double y, double z);
        internal abstract void glVertex(float x, float y, float z);
        internal abstract void glVertex(int x, int y, int z);
        internal abstract void glVertex(short x, short y, short z);

        internal abstract void glVertex(double x, double y, double z, double w);
        internal abstract void glVertex(float x, float y, float z, float w);
        internal abstract void glVertex(int x, int y, int z, int w);
        internal abstract void glVertex(short x, short y, short z, short w);

        internal abstract void glVertex2v(double* v);
        internal abstract void glVertex2v(float* v);
        internal abstract void glVertex2v(int* v);
        internal abstract void glVertex2v(short* v);

        internal abstract void glVertex3v(double* v);
        internal abstract void glVertex3v(float* v);
        internal abstract void glVertex3v(int* v);
        internal abstract void glVertex3v(short* v);

        internal abstract void glVertex4v(double* v);
        internal abstract void glVertex4v(float* v);
        internal abstract void glVertex4v(int* v);
        internal abstract void glVertex4v(short* v);

        #endregion

        internal abstract void glVertexPointer(int size, GLDataType type, int stride, void* pointer);
        internal abstract void glViewport(int x, int y, int width, int height);

        internal abstract int gluBuild2DMipmaps(GLTextureTarget target, GLInternalPixelFormat internalFormat, int width, int height, GLPixelDataFormat format, GLPixelDataType type, void* data);
        internal abstract void gluDeleteQuadric(int quad);
        internal abstract int gluNewQuadric();

        internal abstract void gluPerspective(double fovy, double aspect, double zNear, double zFar);

        internal abstract void gluSphere(int quad, double radius, int slices, int stacks);

        internal abstract void gluQuadricDrawStyle(int quad, GLUQuadricDrawStyle draw);
        internal abstract void gluQuadricOrientation(int quad, GLUQuadricOrientation orientation);
        internal abstract void gluLookAt(double eyeX, double eyeY, double eyeZ, double centerX, double centerY, double centerZ, double upX, double upY, double upZ);
        internal abstract void gluUnProject(double winX, double winY, double winZ, double* model, double* proj, int* view, double* objX, double* objY, double* objZ);
    }
}