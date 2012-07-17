using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace BrawlLib.OpenGL
{
    public static unsafe class wGL
    {
        //Platform  
        [DllImport("opengl32.dll")]
        public static extern bool wglCopyContext(VoidPtr hglrcSrc, VoidPtr hglrcDst, uint mask);
        [DllImport("opengl32.dll", SetLastError = true)]
        public static extern VoidPtr wglCreateContext(VoidPtr hdc);
        [DllImport("opengl32.dll")]
        public static extern VoidPtr wglCreateLayerContext(VoidPtr hdc, int layerPlane);
        [DllImport("opengl32.dll", SetLastError = true)]
        public static extern bool wglDeleteContext(VoidPtr hglrc);
        [DllImport("opengl32.dll")]
        public static extern bool wglDescribeLayerPlane(VoidPtr hdc, int iPixelFormat, int iLayerPlane, uint nBytes, VoidPtr layerPlaneDescriptor);
        [DllImport("opengl32.dll")]
        public static extern VoidPtr wglGetCurrentContext();
        [DllImport("opengl32.dll")]
        public static extern VoidPtr wglGetCurrentDC();
        /*
        [DllImport("opengl32.dll")]
        public static extern ?? wglGetDefaultProcAddress(??);
         * */
        [DllImport("opengl32.dll")]
        public static extern int wglGetLayerPaletteEntries(VoidPtr hdc, int iLayerPlane, int iStart, int cEntries, VoidPtr colorRefArray);
        [DllImport("opengl32.dll")]
        public static extern int wglGetPixelFormat(VoidPtr hdc);
        [DllImport("opengl32.dll", EntryPoint = "wglGetProcAddress", CharSet = CharSet.Ansi)]
        public static extern VoidPtr wglGetProcAddress(string lpszProc);
        [DllImport("opengl32.dll", SetLastError = true)]
        public static extern bool wglMakeCurrent(VoidPtr hdc, VoidPtr hglrc);
        [DllImport("opengl32.dll")]
        public static extern bool wglRealizeLayerPalette(VoidPtr hdc, int iLayerPlane, bool bRealize);
        [DllImport("opengl32.dll")]
        public static extern int wglSetLayerPaletteEntries(VoidPtr hdc, int iLayerPlane, int iStart, int cEntries, VoidPtr colorRefArray);
        [DllImport("opengl32.dll")]
        public static extern bool wglShareLists(VoidPtr hglrc1, VoidPtr hglrc2);
        [DllImport("opengl32.dll", SetLastError = true)]
        public static extern bool wglSwapBuffers(VoidPtr hdc);
        [DllImport("opengl32.dll")]
        public static extern bool wglSwapLayerBuffers(VoidPtr hdc, uint fuPlanes);
        [DllImport("opengl32.dll")]
        public static extern ulong wglSwapMultipleBuffers(uint num, VoidPtr swapStruct);
        [DllImport("opengl32.dll")]
        public static extern bool wglUseFontBitmapsA(VoidPtr hdc, ulong first, ulong count, ulong listBase);
        [DllImport("opengl32.dll")]
        public static extern bool wglUseFontBitmapsW(VoidPtr hdc, ulong first, ulong count, ulong listBase);
        [DllImport("opengl32.dll")]
        public static extern bool wglUseFontOutlinesA(VoidPtr hdc, ulong first, ulong count, ulong listBase, float deviation, float extrusion, int format, VoidPtr glyphMetricsFloatArr);
        [DllImport("opengl32.dll")]
        public static extern bool wglUseFontOutlinesW(VoidPtr hdc, ulong first, ulong count, ulong listBase, float deviation, float extrusion, int format, VoidPtr glyphMetricsFloatArr);

        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern int ChoosePixelFormat(VoidPtr hdc, PixelFormatDescriptor* pfd);
        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern int DescribePixelFormat(VoidPtr hdc, int iPixelFormat, ushort nBytes, PixelFormatDescriptor* pfd);
        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern bool SetPixelFormat(VoidPtr hdc, int iPixelFormat, PixelFormatDescriptor* pfd);

        //OpenGL
        [DllImport("opengl32.dll")]
        public static extern void glAccum(GLAccumOp op, float value);
        [DllImport("opengl32.dll")]
        public static extern void glAlphaFunc(GLAlphaFunc func, float refValue);
        [DllImport("opengl32.dll")]
        public static extern bool glAreTexturesResident(int num, uint* textures, bool* residences);
        [DllImport("opengl32.dll")]
        public static extern void glArrayElement(int index);
        [DllImport("opengl32.dll")]
        public static extern void glBegin(GLPrimitiveType mode);
        [DllImport("opengl32.dll")]
        public static extern void glBindTexture(GLTextureTarget target, uint texture);
        [DllImport("opengl32.dll")]
        public static extern void glBitmap(int width, int height, float xorig, float yorig, float xmove, float ymove, byte* bitmap);
        [DllImport("opengl32.dll")]
        public static extern void glBlendFunc(GLBlendFactor sfactor, GLBlendFactor dfactor);
        [DllImport("opengl32.dll")]
        public static extern void glCallList(uint list);
        [DllImport("opengl32.dll")]
        public static extern void glCallLists(int n, uint type, void* lists);
        [DllImport("opengl32.dll")]
        public static extern void glClear(GLClearMask mask);
        [DllImport("opengl32.dll")]
        public static extern void glClearAccum(float red, float green, float blue, float alpha);
        [DllImport("opengl32.dll")]
        public static extern void glClearColor(float red, float green, float blue, float alpha);
        [DllImport("opengl32.dll")]
        public static extern void glClearDepth(double depth);
        [DllImport("opengl32.dll")]
        public static extern void glClearIndex(float c);
        [DllImport("opengl32.dll")]
        public static extern void glClearStencil(int s);
        [DllImport("opengl32.dll")]
        public static extern void glClipPlane(uint plane, double* equation);

        #region glColor

        [DllImport("opengl32.dll")]
        public static extern void glColor3b(sbyte red, sbyte green, sbyte blue);
        [DllImport("opengl32.dll")]
        public static extern void glColor3d(double red, double green, double blue);
        [DllImport("opengl32.dll")]
        public static extern void glColor3f(float red, float green, float blue);
        [DllImport("opengl32.dll")]
        public static extern void glColor3i(int red, int green, int blue);
        [DllImport("opengl32.dll")]
        public static extern void glColor3s(short red, short green, short blue);
        [DllImport("opengl32.dll")]
        public static extern void glColor3ub(byte red, byte green, byte blue);
        [DllImport("opengl32.dll")]
        public static extern void glColor3ui(uint red, uint green, uint blue);
        [DllImport("opengl32.dll")]
        public static extern void glColor3us(ushort red, ushort green, ushort blue);

        [DllImport("opengl32.dll")]
        public static extern void glColor4b(sbyte red, sbyte green, sbyte blue, sbyte alpha);
        [DllImport("opengl32.dll")]
        public static extern void glColor4d(double red, double green, double blue, double alpha);
        [DllImport("opengl32.dll")]
        public static extern void glColor4f(float red, float green, float blue, float alpha);
        [DllImport("opengl32.dll")]
        public static extern void glColor4i(int red, int green, int blue, int alpha);
        [DllImport("opengl32.dll")]
        public static extern void glColor4s(short red, short green, short blue, short alpha);
        [DllImport("opengl32.dll")]
        public static extern void glColor4ub(byte red, byte green, byte blue, byte alpha);
        [DllImport("opengl32.dll")]
        public static extern void glColor4ui(uint red, uint green, uint blue, uint alpha);
        [DllImport("opengl32.dll")]
        public static extern void glColor4us(ushort red, ushort green, ushort blue, ushort alpha);

        [DllImport("opengl32.dll")]
        public static extern void glColor3bv(sbyte* v);
        [DllImport("opengl32.dll")]
        public static extern void glColor3dv(double* v);
        [DllImport("opengl32.dll")]
        public static extern void glColor3fv(float* v);
        [DllImport("opengl32.dll")]
        public static extern void glColor3iv(int* v);
        [DllImport("opengl32.dll")]
        public static extern void glColor3sv(short* v);
        [DllImport("opengl32.dll")]
        public static extern void glColor3ubv(byte* v);
        [DllImport("opengl32.dll")]
        public static extern void glColor3uiv(uint* v);
        [DllImport("opengl32.dll")]
        public static extern void glColor3usv(ushort* v);

        [DllImport("opengl32.dll")]
        public static extern void glColor4bv(sbyte* v);
        [DllImport("opengl32.dll")]
        public static extern void glColor4dv(double* v);
        [DllImport("opengl32.dll")]
        public static extern void glColor4fv(float* v);
        [DllImport("opengl32.dll")]
        public static extern void glColor4iv(int* v);
        [DllImport("opengl32.dll")]
        public static extern void glColor4sv(short* v);
        [DllImport("opengl32.dll")]
        public static extern void glColor4ubv(byte* v);
        [DllImport("opengl32.dll")]
        public static extern void glColor4uiv(uint* v);
        [DllImport("opengl32.dll")]
        public static extern void glColor4usv(ushort* v);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern void glColorMask(bool red, bool green, bool blue, bool alpha);
        [DllImport("opengl32.dll")]
        public static extern void glColorMaterial(GLFace face, GLMaterialParameter mode);
        [DllImport("opengl32.dll")]
        public static extern void glColorPointer(int size, GLDataType type, int stride, void* pointer);
        [DllImport("opengl32.dll")]
        public static extern void glCopyPixels(int x, int y, int width, int height, uint type);

        #region CopyTex

        [DllImport("opengl32.dll")]
        public static extern void glCopyTexImage1D(GLTextureTarget target, int level, GLInternalPixelFormat publicFormat, int x, int y, int width, int border);
        [DllImport("opengl32.dll")]
        public static extern void glCopyTexImage2D(GLTextureTarget target, int level, GLInternalPixelFormat publicFormat, int x, int y, int width, int height, int border);
        [DllImport("opengl32.dll")]
        public static extern void glCopyTexSubImage1D(GLTextureTarget target, int level, int xOffset, int x, int y, int width);
        [DllImport("opengl32.dll")]
        public static extern void glCopyTexSubImage2D(GLTextureTarget target, int level, int xOffset, int yOffset, int x, int y, int width, int height);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern void glCreateProgram();
        [DllImport("opengl32.dll")]
        public static extern void glCreateShader(ShaderType type);
        [DllImport("opengl32.dll")]
        public static extern void glCullFace(GLFace mode);
        /*
        [DllImport("opengl32.dll")]
        public static extern ?? glDebugEntry(??);
         * */
        [DllImport("opengl32.dll")]
        public static extern void glDeleteLists(uint list, int range);
        [DllImport("opengl32.dll")]
        public static extern void glDeleteTextures(int num, uint* textures);
        [DllImport("opengl32.dll")]
        public static extern void glDepthFunc(GLFunction func);
        [DllImport("opengl32.dll")]
        public static extern void glDepthMask(bool flag);
        [DllImport("opengl32.dll")]
        public static extern void glDepthRange(double near, double far);
        [DllImport("opengl32.dll")]
        public static extern void glDisable(uint cap);
        [DllImport("opengl32.dll")]
        public static extern void glDisableClientState(GLArrayType cap);
        [DllImport("opengl32.dll")]
        public static extern void glDrawArrays(GLPrimitiveType mode, int first, int count);
        [DllImport("opengl32.dll")]
        public static extern void glDrawBuffer(uint mode);
        [DllImport("opengl32.dll")]
        public static extern void glDrawElements(GLPrimitiveType mode, int count, GLElementType type, void* indices);
        [DllImport("opengl32.dll")]
        public static extern void glDrawPixels(int width, int height, GLPixelDataFormat format, GLPixelDataType type, void* pixels);
        [DllImport("opengl32.dll")]
        public static extern void glEdgeFlag(bool flag);
        [DllImport("opengl32.dll")]
        public static extern void glEdgeFlagPointer(int stride, bool* pointer);
        [DllImport("opengl32.dll")]
        public static extern void glEdgeFlagv(bool* flag);
        [DllImport("opengl32.dll")]
        public static extern void glEnable(GLEnableCap cap);
        [DllImport("opengl32.dll")]
        public static extern void glEnableClientState(GLArrayType cap);
        [DllImport("opengl32.dll")]
        public static extern void glEnd();
        [DllImport("opengl32.dll")]
        public static extern void glEndList();

        #region glEvalCoord

        [DllImport("opengl32.dll")]
        public static extern void glEvalCoord1d(double u);
        [DllImport("opengl32.dll")]
        public static extern void glEvalCoord1f(float u);
        [DllImport("opengl32.dll")]
        public static extern void glEvalCoord2d(double u, double v);
        [DllImport("opengl32.dll")]
        public static extern void glEvalCoord2f(float u, float v);
        [DllImport("opengl32.dll")]
        public static extern void glEvalCoord1dv(double* u);
        [DllImport("opengl32.dll")]
        public static extern void glEvalCoord1fv(float* u);
        [DllImport("opengl32.dll")]
        public static extern void glEvalCoord2dv(double* u);
        [DllImport("opengl32.dll")]
        public static extern void glEvalCoord2fv(float* u);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern void glEvalMesh1(uint mode, int i1, int i2);
        [DllImport("opengl32.dll")]
        public static extern void glEvalMesh2(uint mode, int i1, int i2, int j1, int j2);

        [DllImport("opengl32.dll")]
        public static extern void glEvalPoint1(int i);
        [DllImport("opengl32.dll")]
        public static extern void glEvalPoint2(int i, int j);

        [DllImport("opengl32.dll")]
        public static extern void glFeedbackBuffer(int size, uint type, out float* buffer);
        [DllImport("opengl32.dll")]
        public static extern void glFinish();
        [DllImport("opengl32.dll")]
        public static extern void glFlush();

        #region glFog

        [DllImport("opengl32.dll")]
        public static extern void glFogf(uint pname, float param);
        [DllImport("opengl32.dll")]
        public static extern void glFogi(uint pname, int param);
        [DllImport("opengl32.dll")]
        public static extern void glFogfv(uint pname, float* param);
        [DllImport("opengl32.dll")]
        public static extern void glFogiv(uint pname, int* param);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern void glFrontFace(GLFrontFaceDirection mode);
        [DllImport("opengl32.dll")]
        public static extern void glFrustum(double left, double right, double bottom, double top, double near, double far);
        [DllImport("opengl32.dll")]
        public static extern uint glGenLists(int range);
        [DllImport("opengl32.dll")]
        public static extern void glGenTextures(int num, uint* textures);

        #region glGet

        [DllImport("opengl32.dll")]
        public static extern void glGetBooleanv(GLGetMode pname, bool* param);
        [DllImport("opengl32.dll")]
        public static extern void glGetDoublev(GLGetMode pname, double* param);
        [DllImport("opengl32.dll")]
        public static extern void glGetFloatv(GLGetMode pname, float* param);
        [DllImport("opengl32.dll")]
        public static extern void glGetIntegerv(GLGetMode pname, int* param);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern void glGetClipPlane(uint plane, double* equation);
        [DllImport("opengl32.dll")]
        public static extern GLErrorCode glGetError();

        [DllImport("opengl32.dll")]
        public static extern void glGetLightfv(uint light, uint pname, float* param);
        [DllImport("opengl32.dll")]
        public static extern void glGetLightiv(uint light, uint pname, int* param);

        [DllImport("opengl32.dll")]
        public static extern void glGetMapdv(uint target, uint query, double* v);
        [DllImport("opengl32.dll")]
        public static extern void glGetMapfv(uint target, uint query, float* v);
        [DllImport("opengl32.dll")]
        public static extern void glGetMapiv(uint target, uint query, int* v);

        [DllImport("opengl32.dll")]
        public static extern void glGetMaterialfv(uint face, uint pname, float* param);
        [DllImport("opengl32.dll")]
        public static extern void glGetMaterialiv(uint face, uint pname, int* param);

        [DllImport("opengl32.dll")]
        public static extern void glGetPixelMapfv(uint map, float* values);
        [DllImport("opengl32.dll")]
        public static extern void glGetPixelMapuiv(uint map, uint* values);
        [DllImport("opengl32.dll")]
        public static extern void glGetPixelMapusv(uint map, ushort* values);

        [DllImport("opengl32.dll")]
        public static extern void glGetPointerv(uint name, void* values);
        [DllImport("opengl32.dll")]
        public static extern void glGetPolygonStipple(out byte* mask);
        [DllImport("opengl32.dll")]
        public static extern byte* glGetString(uint name);

        [DllImport("opengl32.dll")]
        public static extern void glGetTexEnvfv(uint target, uint pname, out float* param);
        [DllImport("opengl32.dll")]
        public static extern void glGetTexEnviv(uint target, uint pname, out int* param);

        [DllImport("opengl32.dll")]
        public static extern void glGetTexGendv(uint coord, uint pname, out double* param);
        [DllImport("opengl32.dll")]
        public static extern void glGetTexGenfv(uint coord, uint pname, out float* param);
        [DllImport("opengl32.dll")]
        public static extern void glGetTexGeniv(uint coord, uint pname, out int* param);

        [DllImport("opengl32.dll")]
        public static extern void glGetTexImage(uint target, uint format, uint type, out void* pixels);

        [DllImport("opengl32.dll")]
        public static extern void glGetTexLevelParameterfv(uint target, int level, uint pname, out float* param);
        [DllImport("opengl32.dll")]
        public static extern void glGetTexLevelParameteriv(uint target, int level, uint pname, out int* param);

        [DllImport("opengl32.dll")]
        public static extern void glGetTexParameterfv(uint target, uint pname, out float* param);
        [DllImport("opengl32.dll")]
        public static extern void glGetTexParameteriv(uint target, uint pname, out int* param);

        [DllImport("opengl32.dll")]
        public static extern void glHint(GLHintTarget target, GLHintMode mode);

        #region glIndex

        [DllImport("opengl32.dll")]
        public static extern void glIndexd(double c);
        [DllImport("opengl32.dll")]
        public static extern void glIndexf(float c);
        [DllImport("opengl32.dll")]
        public static extern void glIndexi(int c);
        [DllImport("opengl32.dll")]
        public static extern void glIndexs(short c);

        [DllImport("opengl32.dll")]
        public static extern void glIndexdv(double* c);
        [DllImport("opengl32.dll")]
        public static extern void glIndexfv(float* c);
        [DllImport("opengl32.dll")]
        public static extern void glIndexiv(int* c);
        [DllImport("opengl32.dll")]
        public static extern void glIndexsv(short* c);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern void glIndexMask(uint mask);
        [DllImport("opengl32.dll")]
        public static extern void glIndexPointer(uint type, int stride, void* ptr);
        [DllImport("opengl32.dll")]
        public static extern void glInitNames();
        [DllImport("opengl32.dll")]
        public static extern void glInterleavedArrays(uint format, int stride, void* pointer);
        [DllImport("opengl32.dll")]
        public static extern bool glIsEnabled(uint cap);
        [DllImport("opengl32.dll")]
        public static extern bool glIsList(uint list);
        [DllImport("opengl32.dll")]
        public static extern bool glIsTexture(uint texture);

        #region glLight

        [DllImport("opengl32.dll")]
        public static extern bool glLightf(GLLightTarget light, GLLightParameter pname, float param);
        [DllImport("opengl32.dll")]
        public static extern bool glLighti(GLLightTarget light, GLLightParameter pname, int param);
        [DllImport("opengl32.dll")]
        public static extern bool glLightfv(GLLightTarget light, GLLightParameter pname, float* param);
        [DllImport("opengl32.dll")]
        public static extern bool glLightiv(GLLightTarget light, GLLightParameter pname, int* param);

        #endregion

        #region glLightModel

        [DllImport("opengl32.dll")]
        public static extern void glLightModelf(uint pname, float param);
        [DllImport("opengl32.dll")]
        public static extern void glLightModeli(uint pname, int param);
        [DllImport("opengl32.dll")]
        public static extern void glLightModelfv(uint pname, float* param);
        [DllImport("opengl32.dll")]
        public static extern void glLightModeliv(uint pname, int* param);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern void glLineStipple(int factor, ushort pattern);
        [DllImport("opengl32.dll")]
        public static extern void glLineWidth(float width);
        [DllImport("opengl32.dll")]
        public static extern void glListBase(uint b);
        [DllImport("opengl32.dll")]
        public static extern void glLoadIdentity();
        [DllImport("opengl32.dll")]
        public static extern void glLoadMatrixd(double* m);
        [DllImport("opengl32.dll")]
        public static extern void glLoadMatrixf(float* m);
        [DllImport("opengl32.dll")]
        public static extern void glLoadName(uint name);
        [DllImport("opengl32.dll")]
        public static extern void glLogicOp(uint opcode);

        #region glMap

        [DllImport("opengl32.dll")]
        public static extern void glMap1d(uint target, double u1, double u2, int stride, int order, double* points);
        [DllImport("opengl32.dll")]
        public static extern void glMap1f(uint target, float u1, float u2, int stride, int order, float* points);
        [DllImport("opengl32.dll")]
        public static extern void glMap2d(uint target, double u1, double u2, int ustride, int uorder, double v1, double v2, int vstride, int vorder, double* points);
        [DllImport("opengl32.dll")]
        public static extern void glMap2f(uint target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder, float* points);

        #endregion

        #region glMapGrid

        [DllImport("opengl32.dll")]
        public static extern void glMapGrid1d(int un, double u1, double u2);
        [DllImport("opengl32.dll")]
        public static extern void glMapGrid1f(int un, float u1, float u2);
        [DllImport("opengl32.dll")]
        public static extern void glMapGrid2d(int un, double u1, double u2, int vn, double v1, double v2);
        [DllImport("opengl32.dll")]
        public static extern void glMapGrid2f(int un, float u1, float u2, int vn, float v1, float v2);

        #endregion

        #region glMaterial

        [DllImport("opengl32.dll")]
        public static extern void glMaterialf(GLFace face, GLMaterialParameter pname, float param);
        [DllImport("opengl32.dll")]
        public static extern void glMateriali(GLFace face, GLMaterialParameter pname, int param);
        [DllImport("opengl32.dll")]
        public static extern void glMaterialfv(GLFace face, GLMaterialParameter pname, float* param);
        [DllImport("opengl32.dll")]
        public static extern void glMaterialiv(GLFace face, GLMaterialParameter pname, int* param);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern void glMatrixMode(GLMatrixMode mode);

        [DllImport("opengl32.dll")]
        public static extern void glMultMatrixd(double* m);
        [DllImport("opengl32.dll")]
        public static extern void glMultMatrixf(float* m);

        [DllImport("opengl32.dll")]
        public static extern void glNewList(uint list, GLListMode mode);

        #region glNormal

        [DllImport("opengl32.dll")]
        public static extern void glNormal3b(sbyte nx, sbyte ny, sbyte nz);
        [DllImport("opengl32.dll")]
        public static extern void glNormal3d(double nx, double ny, double nz);
        [DllImport("opengl32.dll")]
        public static extern void glNormal3f(float nx, float ny, float nz);
        [DllImport("opengl32.dll")]
        public static extern void glNormal3i(int nx, int ny, int nz);
        [DllImport("opengl32.dll")]
        public static extern void glNormal3s(short nx, short ny, short nz);

        [DllImport("opengl32.dll")]
        public static extern void glNormal3bv(sbyte* v);
        [DllImport("opengl32.dll")]
        public static extern void glNormal3dv(double* v);
        [DllImport("opengl32.dll")]
        public static extern void glNormal3fv(float* v);
        [DllImport("opengl32.dll")]
        public static extern void glNormal3iv(int* v);
        [DllImport("opengl32.dll")]
        public static extern void glNormal3sv(short* v);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern void glNormalPointer(GLDataType type, int stride, void* pointer);

        [DllImport("opengl32.dll")]
        public static extern void glOrtho(double left, double right, double bottom, double top, double near, double far);
        [DllImport("opengl32.dll")]
        public static extern void glPassThrough(float token);

        [DllImport("opengl32.dll")]
        public static extern void glPixelMapfv(uint map, int mapsize, float* v);
        [DllImport("opengl32.dll")]
        public static extern void glPixelMapuiv(uint map, int mapsize, uint* v);
        [DllImport("opengl32.dll")]
        public static extern void glPixelMapusv(uint map, int mapsize, ushort* v);

        [DllImport("opengl32.dll")]
        public static extern void glPixelStoref(uint pname, float param);
        [DllImport("opengl32.dll")]
        public static extern void glPixelStorei(uint pname, int param);

        [DllImport("opengl32.dll")]
        public static extern void glPixelTransferf(uint pname, float param);
        [DllImport("opengl32.dll")]
        public static extern void glPixelTransferi(uint pname, int param);

        [DllImport("opengl32.dll")]
        public static extern void glPixelZoom(float xfactor, float yfactor);
        [DllImport("opengl32.dll")]
        public static extern void glPointSize(float size);
        [DllImport("opengl32.dll")]
        public static extern void glPolygonMode(GLFace face, GLPolygonMode mode);
        [DllImport("opengl32.dll")]
        public static extern void glPolygonOffset(float factor, float units);
        [DllImport("opengl32.dll")]
        public static extern void glPolygonStipple(byte* mask);

        [DllImport("opengl32.dll")]
        public static extern void glPopAttrib(uint mask);
        [DllImport("opengl32.dll")]
        public static extern void glPopClientAttrib(uint mask);
        [DllImport("opengl32.dll")]
        public static extern void glPopMatrix();
        [DllImport("opengl32.dll")]
        public static extern void glPopName();

        [DllImport("opengl32.dll")]
        public static extern void glPrioritizeTextures(int num, uint* textures, float* priorities);

        [DllImport("opengl32.dll")]
        public static extern void glPushAttrib(uint mask);
        [DllImport("opengl32.dll")]
        public static extern void glPushClientAttrib(uint mask);
        [DllImport("opengl32.dll")]
        public static extern void glPushMatrix();
        [DllImport("opengl32.dll")]
        public static extern void glPushName(uint name);

        #region glRasterPos

        [DllImport("opengl32.dll")]
        public static extern void glRasterPos2d(double x, double y);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos2f(float x, float y);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos2i(int x, int y);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos2s(short x, short y);

        [DllImport("opengl32.dll")]
        public static extern void glRasterPos3d(double x, double y, double z);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos3f(float x, float y, float z);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos3i(int x, int y, int z);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos3s(short x, short y, short z);

        [DllImport("opengl32.dll")]
        public static extern void glRasterPos4d(double x, double y, double z, double w);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos4f(float x, float y, float z, float w);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos4i(int x, int y, int z, int w);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos4s(short x, short y, short z, short w);

        [DllImport("opengl32.dll")]
        public static extern void glRasterPos2dv(double* v);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos2fv(float* v);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos2iv(int* v);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos2sv(short* v);

        [DllImport("opengl32.dll")]
        public static extern void glRasterPos3dv(double* v);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos3fv(float* v);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos3iv(int* v);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos3sv(short* v);

        [DllImport("opengl32.dll")]
        public static extern void glRasterPos4dv(double* v);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos4fv(float* v);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos4iv(int* v);
        [DllImport("opengl32.dll")]
        public static extern void glRasterPos4sv(short* v);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern void glReadBuffer(uint mode);
        [DllImport("opengl32.dll")]
        public static extern void glReadPixels(int x, int y, int width, int height, GLPixelDataFormat format, GLPixelDataType type, void* pixels);

        #region glRect

        [DllImport("opengl32.dll")]
        public static extern void glRectd(double x1, double y1, double x2, double y2);
        [DllImport("opengl32.dll")]
        public static extern void glRectf(float x1, float y1, float x2, float y2);
        [DllImport("opengl32.dll")]
        public static extern void glRecti(int x1, int y1, int x2, int y2);
        [DllImport("opengl32.dll")]
        public static extern void glRects(short x1, short y1, short x2, short y2);

        [DllImport("opengl32.dll")]
        public static extern void glRectdv(double* v1, double* v2);
        [DllImport("opengl32.dll")]
        public static extern void glRectfv(float* v1, float* v2);
        [DllImport("opengl32.dll")]
        public static extern void glRectiv(int* v1, int* v2);
        [DllImport("opengl32.dll")]
        public static extern void glRectsv(short* v1, short* v2);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern int glRenderMode(uint mode);

        [DllImport("opengl32.dll")]
        public static extern void glRotated(double angle, double x, double y, double z);
        [DllImport("opengl32.dll")]
        public static extern void glRotatef(float angle, float x, float y, float z);

        [DllImport("opengl32.dll")]
        public static extern void glScaled(double x, double y, double z);
        [DllImport("opengl32.dll")]
        public static extern void glScalef(float x, float y, float z);

        [DllImport("opengl32.dll")]
        public static extern void glScissor(int x, int y, int width, int height);
        [DllImport("opengl32.dll")]
        public static extern void glSelectBuffer(int size, out uint* buffer);
        [DllImport("opengl32.dll")]
        public static extern void glShadeModel(GLShadingModel mode);
        [DllImport("opengl32.dll")]
        public static extern void glStencilFunc(uint func, int refval, uint mask);
        [DllImport("opengl32.dll")]
        public static extern void glStencilMask(uint mask);
        [DllImport("opengl32.dll")]
        public static extern void glStencilOp(uint fail, uint zfail, uint zpass);

        #region glTexCoord

        [DllImport("opengl32.dll")]
        public static extern void glTexCoord1d(double s);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord1f(float s);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord1i(int s);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord1s(short s);

        [DllImport("opengl32.dll")]
        public static extern void glTexCoord2d(double s, double t);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord2f(float s, float t);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord2i(int s, int t);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord2s(short s, short t);

        [DllImport("opengl32.dll")]
        public static extern void glTexCoord3d(double s, double t, double r);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord3f(float s, float t, float r);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord3i(int s, int t, int r);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord3s(short s, short t, short r);

        [DllImport("opengl32.dll")]
        public static extern void glTexCoord4d(double s, double t, double r, double q);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord4f(float s, float t, float r, float q);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord4i(int s, int t, int r, int q);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord4s(short s, short t, short r, short q);

        [DllImport("opengl32.dll")]
        public static extern void glTexCoord1dv(double* v);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord1fv(float* v);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord1iv(int* v);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord1sv(short* v);

        [DllImport("opengl32.dll")]
        public static extern void glTexCoord2dv(double* v);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord2fv(float* v);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord2iv(int* v);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord2sv(short* v);

        [DllImport("opengl32.dll")]
        public static extern void glTexCoord3dv(double* v);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord3fv(float* v);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord3iv(int* v);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord3sv(short* v);

        [DllImport("opengl32.dll")]
        public static extern void glTexCoord4dv(double* v);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord4fv(float* v);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord4iv(int* v);
        [DllImport("opengl32.dll")]
        public static extern void glTexCoord4sv(short* v);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern void glTexCoordPointer(int size, GLDataType type, int stride, void* pointer);

        [DllImport("opengl32.dll")]
        public static extern void glTexEnvf(GLTexEnvTarget target, GLTexEnvParam pname, float param);
        [DllImport("opengl32.dll")]
        public static extern void glTexEnvi(GLTexEnvTarget target, GLTexEnvParam pname, int param);
        [DllImport("opengl32.dll")]
        public static extern void glTexEnvfv(GLTexEnvTarget target, GLTexEnvParam pname, float* param);
        [DllImport("opengl32.dll")]
        public static extern void glTexEnviv(GLTexEnvTarget target, GLTexEnvParam pname, int* param);

        #region glTexGen

        [DllImport("opengl32.dll")]
        public static extern void glTexGend(TextureCoordName coord, TextureGenParameter pname, double param);
        [DllImport("opengl32.dll")]
        public static extern void glTexGenf(TextureCoordName coord, TextureGenParameter pname, float param);
        [DllImport("opengl32.dll")]
        public static extern void glTexGeni(TextureCoordName coord, TextureGenParameter pname, int param);

        [DllImport("opengl32.dll")]
        public static extern void glTexGendv(TextureCoordName coord, TextureGenParameter pname, double* param);
        [DllImport("opengl32.dll")]
        public static extern void glTexGenfv(TextureCoordName coord, TextureGenParameter pname, float* param);
        [DllImport("opengl32.dll")]
        public static extern void glTexGeniv(TextureCoordName coord, TextureGenParameter pname, int* param);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern void glTexImage1D(GLTexImageTarget target, int level, GLInternalPixelFormat publicFormat, int width, int border, GLPixelDataFormat format, GLPixelDataType type, void* pixels);
        [DllImport("opengl32.dll")]
        public static extern void glTexImage2D(GLTexImageTarget target, int level, GLInternalPixelFormat publicFormat, int width, int height, int border, GLPixelDataFormat format, GLPixelDataType type, void* pixels);

        #region glTexParameter

        [DllImport("opengl32.dll")]
        public static extern void glTexParameterf(GLTextureTarget target, GLTextureParameter pname, float param);
        [DllImport("opengl32.dll")]
        public static extern void glTexParameteri(GLTextureTarget target, GLTextureParameter pname, int param);
        [DllImport("opengl32.dll")]
        public static extern void glTexParameterfv(GLTextureTarget target, GLTextureParameter pname, float* param);
        [DllImport("opengl32.dll")]
        public static extern void glTexParameteriv(GLTextureTarget target, GLTextureParameter pname, int* param);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern void glTexSubImage1D(GLTexImageTarget target, int level, int xOffset, int width, GLPixelDataFormat format, GLPixelDataType type, void* pixels);
        [DllImport("opengl32.dll")]
        public static extern void glTexSubImage2D(GLTexImageTarget target, int level, int xOffset, int yOffset, int width, int height, GLPixelDataFormat format, GLPixelDataType type, void* pixels);

        [DllImport("opengl32.dll")]
        public static extern void glTranslated(double x, double y, double z);
        [DllImport("opengl32.dll")]
        public static extern void glTranslatef(float x, float y, float z);

        #region glVertex

        [DllImport("opengl32.dll")]
        public static extern void glVertex2d(double x, double y);
        [DllImport("opengl32.dll")]
        public static extern void glVertex2f(float x, float y);
        [DllImport("opengl32.dll")]
        public static extern void glVertex2i(int x, int y);
        [DllImport("opengl32.dll")]
        public static extern void glVertex2s(short x, short y);

        [DllImport("opengl32.dll")]
        public static extern void glVertex3d(double x, double y, double z);
        [DllImport("opengl32.dll")]
        public static extern void glVertex3f(float x, float y, float z);
        [DllImport("opengl32.dll")]
        public static extern void glVertex3i(int x, int y, int z);
        [DllImport("opengl32.dll")]
        public static extern void glVertex3s(short x, short y, short z);

        [DllImport("opengl32.dll")]
        public static extern void glVertex4d(double x, double y, double z, double w);
        [DllImport("opengl32.dll")]
        public static extern void glVertex4f(float x, float y, float z, float w);
        [DllImport("opengl32.dll")]
        public static extern void glVertex4i(int x, int y, int z, int w);
        [DllImport("opengl32.dll")]
        public static extern void glVertex4s(short x, short y, short z, short w);

        [DllImport("opengl32.dll")]
        public static extern void glVertex2dv(double* v);
        [DllImport("opengl32.dll")]
        public static extern void glVertex2fv(float* v);
        [DllImport("opengl32.dll")]
        public static extern void glVertex2iv(int* v);
        [DllImport("opengl32.dll")]
        public static extern void glVertex2sv(short* v);

        [DllImport("opengl32.dll")]
        public static extern void glVertex3dv(double* v);
        [DllImport("opengl32.dll")]
        public static extern void glVertex3fv(float* v);
        [DllImport("opengl32.dll")]
        public static extern void glVertex3iv(int* v);
        [DllImport("opengl32.dll")]
        public static extern void glVertex3sv(short* v);

        [DllImport("opengl32.dll")]
        public static extern void glVertex4dv(double* v);
        [DllImport("opengl32.dll")]
        public static extern void glVertex4fv(float* v);
        [DllImport("opengl32.dll")]
        public static extern void glVertex4iv(int* v);
        [DllImport("opengl32.dll")]
        public static extern void glVertex4sv(short* v);

        #endregion

        [DllImport("opengl32.dll")]
        public static extern void glVertexPointer(int size, GLDataType type, int stride, void* pointer);
        [DllImport("opengl32.dll")]
        public static extern void glViewport(int x, int y, int width, int height);

        [DllImport("Glu32.dll")]
        public static extern int gluBuild2DMipmaps(GLTextureTarget target, GLInternalPixelFormat publicFormat, int width, int height, GLPixelDataFormat format, GLPixelDataType type, void* data);

        [DllImport("Glu32.dll")]
        public static extern void gluDeleteQuadric(int quad);

        [DllImport("Glu32.dll")]
        public static extern int gluNewQuadric();

        [DllImport("Glu32.dll")]
        public static extern void gluPerspective(double fovy, double aspect, double zNear, double zFar);

        [DllImport("Glu32.dll")]
        public static extern void gluSphere(int quad, double radius, int slices, int stacks);

        [DllImport("Glu32.dll")]
        public static extern void gluQuadricDrawStyle(int quad, GLUQuadricDrawStyle draw);

        [DllImport("Glu32.dll")]
        public static extern void gluQuadricOrientation(int quad, GLUQuadricOrientation orientation);

        [DllImport("Glu32.dll")]
        public static extern void gluLookAt(double eyeX, double eyeY, double eyeZ, double centerX, double centerY, double centerZ, double upX, double upY, double upZ);

        [DllImport("Glu32.dll")]
        public static extern void gluUnProject(double winX, double winY, double winZ, double* model, double* proj, int* view, double* objX, double* objY, double* objZ);

        [DllImport("opengl32.dll")]
        public static extern void glBindBuffer(etc.BufferTarget target, int buffer);

        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glNewList", ExactSpelling = true)]
        public static extern void NewList(UInt32 list, int mode);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEndList", ExactSpelling = true)]
        public static extern void EndList();
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCallList", ExactSpelling = true)]
        public static extern void CallList(UInt32 list);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCallLists", ExactSpelling = true)]
        public static extern void CallLists(Int32 n, int type, IntPtr lists);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDeleteLists", ExactSpelling = true)]
        public static extern void DeleteLists(UInt32 list, Int32 range);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGenLists", ExactSpelling = true)]
        public static extern Int32 GenLists(Int32 range);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glListBase", ExactSpelling = true)]
        public static extern void ListBase(UInt32 @base);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glBegin", ExactSpelling = true)]
        public static extern void Begin(int mode);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glBitmap", ExactSpelling = true)]
        public static extern unsafe void Bitmap(Int32 width, Int32 height, Single xorig, Single yorig, Single xmove, Single ymove, Byte* bitmap);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3b", ExactSpelling = true)]
        public static extern void Color3b(SByte red, SByte green, SByte blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3bv", ExactSpelling = true)]
        public static extern unsafe void Color3bv(SByte* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3d", ExactSpelling = true)]
        public static extern void Color3d(Double red, Double green, Double blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3dv", ExactSpelling = true)]
        public static extern unsafe void Color3dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3f", ExactSpelling = true)]
        public static extern void Color3f(Single red, Single green, Single blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3fv", ExactSpelling = true)]
        public static extern unsafe void Color3fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3i", ExactSpelling = true)]
        public static extern void Color3i(Int32 red, Int32 green, Int32 blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3iv", ExactSpelling = true)]
        public static extern unsafe void Color3iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3s", ExactSpelling = true)]
        public static extern void Color3s(Int16 red, Int16 green, Int16 blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3sv", ExactSpelling = true)]
        public static extern unsafe void Color3sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3ub", ExactSpelling = true)]
        public static extern void Color3ub(Byte red, Byte green, Byte blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3ubv", ExactSpelling = true)]
        public static extern unsafe void Color3ubv(Byte* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3ui", ExactSpelling = true)]
        public static extern void Color3ui(UInt32 red, UInt32 green, UInt32 blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3uiv", ExactSpelling = true)]
        public static extern unsafe void Color3uiv(UInt32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3us", ExactSpelling = true)]
        public static extern void Color3us(UInt16 red, UInt16 green, UInt16 blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor3usv", ExactSpelling = true)]
        public static extern unsafe void Color3usv(UInt16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4b", ExactSpelling = true)]
        public static extern void Color4b(SByte red, SByte green, SByte blue, SByte alpha);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4bv", ExactSpelling = true)]
        public static extern unsafe void Color4bv(SByte* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4d", ExactSpelling = true)]
        public static extern void Color4d(Double red, Double green, Double blue, Double alpha);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4dv", ExactSpelling = true)]
        public static extern unsafe void Color4dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4f", ExactSpelling = true)]
        public static extern void Color4f(Single red, Single green, Single blue, Single alpha);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4fv", ExactSpelling = true)]
        public static extern unsafe void Color4fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4i", ExactSpelling = true)]
        public static extern void Color4i(Int32 red, Int32 green, Int32 blue, Int32 alpha);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4iv", ExactSpelling = true)]
        public static extern unsafe void Color4iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4s", ExactSpelling = true)]
        public static extern void Color4s(Int16 red, Int16 green, Int16 blue, Int16 alpha);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4sv", ExactSpelling = true)]
        public static extern unsafe void Color4sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4ub", ExactSpelling = true)]
        public static extern void Color4ub(Byte red, Byte green, Byte blue, Byte alpha);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4ubv", ExactSpelling = true)]
        public static extern unsafe void Color4ubv(Byte* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4ui", ExactSpelling = true)]
        public static extern void Color4ui(UInt32 red, UInt32 green, UInt32 blue, UInt32 alpha);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4uiv", ExactSpelling = true)]
        public static extern unsafe void Color4uiv(UInt32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4us", ExactSpelling = true)]
        public static extern void Color4us(UInt16 red, UInt16 green, UInt16 blue, UInt16 alpha);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColor4usv", ExactSpelling = true)]
        public static extern unsafe void Color4usv(UInt16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEdgeFlag", ExactSpelling = true)]
        public static extern void EdgeFlag(Int32 flag);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEdgeFlagv", ExactSpelling = true)]
        public static extern unsafe void EdgeFlagv(Int32* flag);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEnd", ExactSpelling = true)]
        public static extern void End();
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIndexd", ExactSpelling = true)]
        public static extern void Indexd(Double c);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIndexdv", ExactSpelling = true)]
        public static extern unsafe void Indexdv(Double* c);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIndexf", ExactSpelling = true)]
        public static extern void Indexf(Single c);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIndexfv", ExactSpelling = true)]
        public static extern unsafe void Indexfv(Single* c);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIndexi", ExactSpelling = true)]
        public static extern void Indexi(Int32 c);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIndexiv", ExactSpelling = true)]
        public static extern unsafe void Indexiv(Int32* c);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIndexs", ExactSpelling = true)]
        public static extern void Indexs(Int16 c);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIndexsv", ExactSpelling = true)]
        public static extern unsafe void Indexsv(Int16* c);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glNormal3b", ExactSpelling = true)]
        public static extern void Normal3b(SByte nx, SByte ny, SByte nz);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glNormal3bv", ExactSpelling = true)]
        public static extern unsafe void Normal3bv(SByte* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glNormal3d", ExactSpelling = true)]
        public static extern void Normal3d(Double nx, Double ny, Double nz);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glNormal3dv", ExactSpelling = true)]
        public static extern unsafe void Normal3dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glNormal3f", ExactSpelling = true)]
        public static extern void Normal3f(Single nx, Single ny, Single nz);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glNormal3fv", ExactSpelling = true)]
        public static extern unsafe void Normal3fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glNormal3i", ExactSpelling = true)]
        public static extern void Normal3i(Int32 nx, Int32 ny, Int32 nz);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glNormal3iv", ExactSpelling = true)]
        public static extern unsafe void Normal3iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glNormal3s", ExactSpelling = true)]
        public static extern void Normal3s(Int16 nx, Int16 ny, Int16 nz);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glNormal3sv", ExactSpelling = true)]
        public static extern unsafe void Normal3sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos2d", ExactSpelling = true)]
        public static extern void RasterPos2d(Double x, Double y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos2dv", ExactSpelling = true)]
        public static extern unsafe void RasterPos2dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos2f", ExactSpelling = true)]
        public static extern void RasterPos2f(Single x, Single y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos2fv", ExactSpelling = true)]
        public static extern unsafe void RasterPos2fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos2i", ExactSpelling = true)]
        public static extern void RasterPos2i(Int32 x, Int32 y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos2iv", ExactSpelling = true)]
        public static extern unsafe void RasterPos2iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos2s", ExactSpelling = true)]
        public static extern void RasterPos2s(Int16 x, Int16 y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos2sv", ExactSpelling = true)]
        public static extern unsafe void RasterPos2sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos3d", ExactSpelling = true)]
        public static extern void RasterPos3d(Double x, Double y, Double z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos3dv", ExactSpelling = true)]
        public static extern unsafe void RasterPos3dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos3f", ExactSpelling = true)]
        public static extern void RasterPos3f(Single x, Single y, Single z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos3fv", ExactSpelling = true)]
        public static extern unsafe void RasterPos3fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos3i", ExactSpelling = true)]
        public static extern void RasterPos3i(Int32 x, Int32 y, Int32 z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos3iv", ExactSpelling = true)]
        public static extern unsafe void RasterPos3iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos3s", ExactSpelling = true)]
        public static extern void RasterPos3s(Int16 x, Int16 y, Int16 z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos3sv", ExactSpelling = true)]
        public static extern unsafe void RasterPos3sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos4d", ExactSpelling = true)]
        public static extern void RasterPos4d(Double x, Double y, Double z, Double w);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos4dv", ExactSpelling = true)]
        public static extern unsafe void RasterPos4dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos4f", ExactSpelling = true)]
        public static extern void RasterPos4f(Single x, Single y, Single z, Single w);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos4fv", ExactSpelling = true)]
        public static extern unsafe void RasterPos4fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos4i", ExactSpelling = true)]
        public static extern void RasterPos4i(Int32 x, Int32 y, Int32 z, Int32 w);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos4iv", ExactSpelling = true)]
        public static extern unsafe void RasterPos4iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos4s", ExactSpelling = true)]
        public static extern void RasterPos4s(Int16 x, Int16 y, Int16 z, Int16 w);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRasterPos4sv", ExactSpelling = true)]
        public static extern unsafe void RasterPos4sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRectd", ExactSpelling = true)]
        public static extern void Rectd(Double x1, Double y1, Double x2, Double y2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRectdv", ExactSpelling = true)]
        public static extern unsafe void Rectdv(Double* v1, Double* v2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRectf", ExactSpelling = true)]
        public static extern void Rectf(Single x1, Single y1, Single x2, Single y2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRectfv", ExactSpelling = true)]
        public static extern unsafe void Rectfv(Single* v1, Single* v2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRecti", ExactSpelling = true)]
        public static extern void Recti(Int32 x1, Int32 y1, Int32 x2, Int32 y2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRectiv", ExactSpelling = true)]
        public static extern unsafe void Rectiv(Int32* v1, Int32* v2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRects", ExactSpelling = true)]
        public static extern void Rects(Int16 x1, Int16 y1, Int16 x2, Int16 y2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRectsv", ExactSpelling = true)]
        public static extern unsafe void Rectsv(Int16* v1, Int16* v2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord1d", ExactSpelling = true)]
        public static extern void TexCoord1d(Double s);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord1dv", ExactSpelling = true)]
        public static extern unsafe void TexCoord1dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord1f", ExactSpelling = true)]
        public static extern void TexCoord1f(Single s);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord1fv", ExactSpelling = true)]
        public static extern unsafe void TexCoord1fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord1i", ExactSpelling = true)]
        public static extern void TexCoord1i(Int32 s);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord1iv", ExactSpelling = true)]
        public static extern unsafe void TexCoord1iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord1s", ExactSpelling = true)]
        public static extern void TexCoord1s(Int16 s);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord1sv", ExactSpelling = true)]
        public static extern unsafe void TexCoord1sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord2d", ExactSpelling = true)]
        public static extern void TexCoord2d(Double s, Double t);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord2dv", ExactSpelling = true)]
        public static extern unsafe void TexCoord2dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord2f", ExactSpelling = true)]
        public static extern void TexCoord2f(Single s, Single t);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord2fv", ExactSpelling = true)]
        public static extern unsafe void TexCoord2fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord2i", ExactSpelling = true)]
        public static extern void TexCoord2i(Int32 s, Int32 t);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord2iv", ExactSpelling = true)]
        public static extern unsafe void TexCoord2iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord2s", ExactSpelling = true)]
        public static extern void TexCoord2s(Int16 s, Int16 t);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord2sv", ExactSpelling = true)]
        public static extern unsafe void TexCoord2sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord3d", ExactSpelling = true)]
        public static extern void TexCoord3d(Double s, Double t, Double r);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord3dv", ExactSpelling = true)]
        public static extern unsafe void TexCoord3dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord3f", ExactSpelling = true)]
        public static extern void TexCoord3f(Single s, Single t, Single r);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord3fv", ExactSpelling = true)]
        public static extern unsafe void TexCoord3fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord3i", ExactSpelling = true)]
        public static extern void TexCoord3i(Int32 s, Int32 t, Int32 r);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord3iv", ExactSpelling = true)]
        public static extern unsafe void TexCoord3iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord3s", ExactSpelling = true)]
        public static extern void TexCoord3s(Int16 s, Int16 t, Int16 r);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord3sv", ExactSpelling = true)]
        public static extern unsafe void TexCoord3sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord4d", ExactSpelling = true)]
        public static extern void TexCoord4d(Double s, Double t, Double r, Double q);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord4dv", ExactSpelling = true)]
        public static extern unsafe void TexCoord4dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord4f", ExactSpelling = true)]
        public static extern void TexCoord4f(Single s, Single t, Single r, Single q);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord4fv", ExactSpelling = true)]
        public static extern unsafe void TexCoord4fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord4i", ExactSpelling = true)]
        public static extern void TexCoord4i(Int32 s, Int32 t, Int32 r, Int32 q);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord4iv", ExactSpelling = true)]
        public static extern unsafe void TexCoord4iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord4s", ExactSpelling = true)]
        public static extern void TexCoord4s(Int16 s, Int16 t, Int16 r, Int16 q);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord4sv", ExactSpelling = true)]
        public static extern unsafe void TexCoord4sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex2d", ExactSpelling = true)]
        public static extern void Vertex2d(Double x, Double y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex2dv", ExactSpelling = true)]
        public static extern unsafe void Vertex2dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex2f", ExactSpelling = true)]
        public static extern void Vertex2f(Single x, Single y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex2fv", ExactSpelling = true)]
        public static extern unsafe void Vertex2fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex2i", ExactSpelling = true)]
        public static extern void Vertex2i(Int32 x, Int32 y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex2iv", ExactSpelling = true)]
        public static extern unsafe void Vertex2iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex2s", ExactSpelling = true)]
        public static extern void Vertex2s(Int16 x, Int16 y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex2sv", ExactSpelling = true)]
        public static extern unsafe void Vertex2sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex3d", ExactSpelling = true)]
        public static extern void Vertex3d(Double x, Double y, Double z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex3dv", ExactSpelling = true)]
        public static extern unsafe void Vertex3dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex3f", ExactSpelling = true)]
        public static extern void Vertex3f(Single x, Single y, Single z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex3fv", ExactSpelling = true)]
        public static extern unsafe void Vertex3fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex3i", ExactSpelling = true)]
        public static extern void Vertex3i(Int32 x, Int32 y, Int32 z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex3iv", ExactSpelling = true)]
        public static extern unsafe void Vertex3iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex3s", ExactSpelling = true)]
        public static extern void Vertex3s(Int16 x, Int16 y, Int16 z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex3sv", ExactSpelling = true)]
        public static extern unsafe void Vertex3sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex4d", ExactSpelling = true)]
        public static extern void Vertex4d(Double x, Double y, Double z, Double w);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex4dv", ExactSpelling = true)]
        public static extern unsafe void Vertex4dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex4f", ExactSpelling = true)]
        public static extern void Vertex4f(Single x, Single y, Single z, Single w);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex4fv", ExactSpelling = true)]
        public static extern unsafe void Vertex4fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex4i", ExactSpelling = true)]
        public static extern void Vertex4i(Int32 x, Int32 y, Int32 z, Int32 w);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex4iv", ExactSpelling = true)]
        public static extern unsafe void Vertex4iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex4s", ExactSpelling = true)]
        public static extern void Vertex4s(Int16 x, Int16 y, Int16 z, Int16 w);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertex4sv", ExactSpelling = true)]
        public static extern unsafe void Vertex4sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glClipPlane", ExactSpelling = true)]
        public static extern unsafe void ClipPlane(int plane, Double* equation);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColorMaterial", ExactSpelling = true)]
        public static extern void ColorMaterial(int face, int mode);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCullFace", ExactSpelling = true)]
        public static extern void CullFace(int mode);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glFogf", ExactSpelling = true)]
        public static extern void Fogf(int pname, Single param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glFogfv", ExactSpelling = true)]
        public static extern unsafe void Fogfv(int pname, Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glFogi", ExactSpelling = true)]
        public static extern void Fogi(int pname, Int32 param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glFogiv", ExactSpelling = true)]
        public static extern unsafe void Fogiv(int pname, Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glFrontFace", ExactSpelling = true)]
        public static extern void FrontFace(int mode);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glHint", ExactSpelling = true)]
        public static extern void Hint(int target, int mode);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLightf", ExactSpelling = true)]
        public static extern void Lightf(int light, int pname, Single param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLightfv", ExactSpelling = true)]
        public static extern unsafe void Lightfv(int light, int pname, Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLighti", ExactSpelling = true)]
        public static extern void Lighti(int light, int pname, Int32 param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLightiv", ExactSpelling = true)]
        public static extern unsafe void Lightiv(int light, int pname, Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLightModelf", ExactSpelling = true)]
        public static extern void LightModelf(int pname, Single param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLightModelfv", ExactSpelling = true)]
        public static extern unsafe void LightModelfv(int pname, Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLightModeli", ExactSpelling = true)]
        public static extern void LightModeli(int pname, Int32 param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLightModeliv", ExactSpelling = true)]
        public static extern unsafe void LightModeliv(int pname, Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLineStipple", ExactSpelling = true)]
        public static extern void LineStipple(Int32 factor, UInt16 pattern);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLineWidth", ExactSpelling = true)]
        public static extern void LineWidth(Single width);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMaterialf", ExactSpelling = true)]
        public static extern void Materialf(int face, int pname, Single param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMaterialfv", ExactSpelling = true)]
        public static extern unsafe void Materialfv(int face, int pname, Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMateriali", ExactSpelling = true)]
        public static extern void Materiali(int face, int pname, Int32 param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMaterialiv", ExactSpelling = true)]
        public static extern unsafe void Materialiv(int face, int pname, Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPointSize", ExactSpelling = true)]
        public static extern void PointSize(Single size);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPolygonMode", ExactSpelling = true)]
        public static extern void PolygonMode(int face, int mode);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPolygonStipple", ExactSpelling = true)]
        public static extern unsafe void PolygonStipple(Byte* mask);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glScissor", ExactSpelling = true)]
        public static extern void Scissor(Int32 x, Int32 y, Int32 width, Int32 height);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glShadeModel", ExactSpelling = true)]
        public static extern void ShadeModel(int mode);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexParameterf", ExactSpelling = true)]
        public static extern void TexParameterf(int target, int pname, Single param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexParameterfv", ExactSpelling = true)]
        public static extern unsafe void TexParameterfv(int target, int pname, Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexParameteri", ExactSpelling = true)]
        public static extern void TexParameteri(int target, int pname, Int32 param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexParameteriv", ExactSpelling = true)]
        public static extern unsafe void TexParameteriv(int target, int pname, Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexImage1D", ExactSpelling = true)]
        public static extern void TexImage1D(int target, Int32 level, int publicformat, Int32 width, Int32 border, int format, int type, IntPtr pixels);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexImage2D", ExactSpelling = true)]
        public static extern void TexImage2D(int target, Int32 level, int publicformat, Int32 width, Int32 height, Int32 border, int format, int type, IntPtr pixels);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexEnvf", ExactSpelling = true)]
        public static extern void TexEnvf(int target, int pname, Single param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexEnvfv", ExactSpelling = true)]
        public static extern unsafe void TexEnvfv(int target, int pname, Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexEnvi", ExactSpelling = true)]
        public static extern void TexEnvi(int target, int pname, Int32 param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexEnviv", ExactSpelling = true)]
        public static extern unsafe void TexEnviv(int target, int pname, Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexGend", ExactSpelling = true)]
        public static extern void TexGend(int coord, int pname, Double param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexGendv", ExactSpelling = true)]
        public static extern unsafe void TexGendv(int coord, int pname, Double* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexGenf", ExactSpelling = true)]
        public static extern void TexGenf(int coord, int pname, Single param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexGenfv", ExactSpelling = true)]
        public static extern unsafe void TexGenfv(int coord, int pname, Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexGeni", ExactSpelling = true)]
        public static extern void TexGeni(int coord, int pname, Int32 param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexGeniv", ExactSpelling = true)]
        public static extern unsafe void TexGeniv(int coord, int pname, Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glFeedbackBuffer", ExactSpelling = true)]
        public static extern unsafe void FeedbackBuffer(Int32 size, int type, [Out] Single* buffer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSelectBuffer", ExactSpelling = true)]
        public static extern unsafe void SelectBuffer(Int32 size, [Out] UInt32* buffer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRenderMode", ExactSpelling = true)]
        public static extern Int32 RenderMode(int mode);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glInitNames", ExactSpelling = true)]
        public static extern void InitNames();
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLoadName", ExactSpelling = true)]
        public static extern void LoadName(UInt32 name);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPassThrough", ExactSpelling = true)]
        public static extern void PassThrough(Single token);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPopName", ExactSpelling = true)]
        public static extern void PopName();
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPushName", ExactSpelling = true)]
        public static extern void PushName(UInt32 name);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDrawBuffer", ExactSpelling = true)]
        public static extern void DrawBuffer(int mode);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glClear", ExactSpelling = true)]
        public static extern void Clear(int mask);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glClearAccum", ExactSpelling = true)]
        public static extern void ClearAccum(Single red, Single green, Single blue, Single alpha);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glClearIndex", ExactSpelling = true)]
        public static extern void ClearIndex(Single c);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glClearColor", ExactSpelling = true)]
        public static extern void ClearColor(Single red, Single green, Single blue, Single alpha);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glClearStencil", ExactSpelling = true)]
        public static extern void ClearStencil(Int32 s);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glClearDepth", ExactSpelling = true)]
        public static extern void ClearDepth(Double depth);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glStencilMask", ExactSpelling = true)]
        public static extern void StencilMask(UInt32 mask);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColorMask", ExactSpelling = true)]
        public static extern void ColorMask(Int32 red, Int32 green, Int32 blue, Int32 alpha);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDepthMask", ExactSpelling = true)]
        public static extern void DepthMask(Int32 flag);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIndexMask", ExactSpelling = true)]
        public static extern void IndexMask(UInt32 mask);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glAccum", ExactSpelling = true)]
        public static extern void Accum(int op, Single value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDisable", ExactSpelling = true)]
        public static extern void Disable(int cap);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEnable", ExactSpelling = true)]
        public static extern void Enable(int cap);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glFinish", ExactSpelling = true)]
        public static extern void Finish();
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glFlush", ExactSpelling = true)]
        public static extern void Flush();
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPopAttrib", ExactSpelling = true)]
        public static extern void PopAttrib();
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPushAttrib", ExactSpelling = true)]
        public static extern void PushAttrib(int mask);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMap1d", ExactSpelling = true)]
        public static extern unsafe void Map1d(int target, Double u1, Double u2, Int32 stride, Int32 order, Double* points);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMap1f", ExactSpelling = true)]
        public static extern unsafe void Map1f(int target, Single u1, Single u2, Int32 stride, Int32 order, Single* points);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMap2d", ExactSpelling = true)]
        public static extern unsafe void Map2d(int target, Double u1, Double u2, Int32 ustride, Int32 uorder, Double v1, Double v2, Int32 vstride, Int32 vorder, Double* points);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMap2f", ExactSpelling = true)]
        public static extern unsafe void Map2f(int target, Single u1, Single u2, Int32 ustride, Int32 uorder, Single v1, Single v2, Int32 vstride, Int32 vorder, Single* points);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMapGrid1d", ExactSpelling = true)]
        public static extern void MapGrid1d(Int32 un, Double u1, Double u2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMapGrid1f", ExactSpelling = true)]
        public static extern void MapGrid1f(Int32 un, Single u1, Single u2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMapGrid2d", ExactSpelling = true)]
        public static extern void MapGrid2d(Int32 un, Double u1, Double u2, Int32 vn, Double v1, Double v2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMapGrid2f", ExactSpelling = true)]
        public static extern void MapGrid2f(Int32 un, Single u1, Single u2, Int32 vn, Single v1, Single v2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEvalCoord1d", ExactSpelling = true)]
        public static extern void EvalCoord1d(Double u);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEvalCoord1dv", ExactSpelling = true)]
        public static extern unsafe void EvalCoord1dv(Double* u);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEvalCoord1f", ExactSpelling = true)]
        public static extern void EvalCoord1f(Single u);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEvalCoord1fv", ExactSpelling = true)]
        public static extern unsafe void EvalCoord1fv(Single* u);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEvalCoord2d", ExactSpelling = true)]
        public static extern void EvalCoord2d(Double u, Double v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEvalCoord2dv", ExactSpelling = true)]
        public static extern unsafe void EvalCoord2dv(Double* u);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEvalCoord2f", ExactSpelling = true)]
        public static extern void EvalCoord2f(Single u, Single v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEvalCoord2fv", ExactSpelling = true)]
        public static extern unsafe void EvalCoord2fv(Single* u);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEvalMesh1", ExactSpelling = true)]
        public static extern void EvalMesh1(int mode, Int32 i1, Int32 i2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEvalPoint1", ExactSpelling = true)]
        public static extern void EvalPoint1(Int32 i);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEvalMesh2", ExactSpelling = true)]
        public static extern void EvalMesh2(int mode, Int32 i1, Int32 i2, Int32 j1, Int32 j2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEvalPoint2", ExactSpelling = true)]
        public static extern void EvalPoint2(Int32 i, Int32 j);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glAlphaFunc", ExactSpelling = true)]
        public static extern void AlphaFunc(int func, Single @ref);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glBlendFunc", ExactSpelling = true)]
        public static extern void BlendFunc(int sfactor, int dfactor);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLogicOp", ExactSpelling = true)]
        public static extern void LogicOp(int opcode);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glStencilFunc", ExactSpelling = true)]
        public static extern void StencilFunc(int func, Int32 @ref, UInt32 mask);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glStencilOp", ExactSpelling = true)]
        public static extern void StencilOp(int fail, int zfail, int zpass);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDepthFunc", ExactSpelling = true)]
        public static extern void DepthFunc(int func);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPixelZoom", ExactSpelling = true)]
        public static extern void PixelZoom(Single xfactor, Single yfactor);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPixelTransferf", ExactSpelling = true)]
        public static extern void PixelTransferf(int pname, Single param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPixelTransferi", ExactSpelling = true)]
        public static extern void PixelTransferi(int pname, Int32 param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPixelStoref", ExactSpelling = true)]
        public static extern void PixelStoref(int pname, Single param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPixelStorei", ExactSpelling = true)]
        public static extern void PixelStorei(int pname, Int32 param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPixelMapfv", ExactSpelling = true)]
        public static extern unsafe void PixelMapfv(int map, Int32 mapsize, Single* values);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPixelMapuiv", ExactSpelling = true)]
        public static extern unsafe void PixelMapuiv(int map, Int32 mapsize, UInt32* values);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPixelMapusv", ExactSpelling = true)]
        public static extern unsafe void PixelMapusv(int map, Int32 mapsize, UInt16* values);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glReadBuffer", ExactSpelling = true)]
        public static extern void ReadBuffer(int mode);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCopyPixels", ExactSpelling = true)]
        public static extern void CopyPixels(Int32 x, Int32 y, Int32 width, Int32 height, int type);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glReadPixels", ExactSpelling = true)]
        public static extern void ReadPixels(Int32 x, Int32 y, Int32 width, Int32 height, int format, int type, [Out] IntPtr pixels);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDrawPixels", ExactSpelling = true)]
        public static extern void DrawPixels(Int32 width, Int32 height, int format, int type, IntPtr pixels);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetBooleanv", ExactSpelling = true)]
        public static extern unsafe void GetBooleanv(int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetClipPlane", ExactSpelling = true)]
        public static extern unsafe void GetClipPlane(int plane, [Out] Double* equation);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetDoublev", ExactSpelling = true)]
        public static extern unsafe void GetDoublev(int pname, [Out] Double* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetError", ExactSpelling = true)]
        public static extern int GetError();
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetFloatv", ExactSpelling = true)]
        public static extern unsafe void GetFloatv(int pname, [Out] Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetIntegerv", ExactSpelling = true)]
        public static extern unsafe void GetIntegerv(int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetLightfv", ExactSpelling = true)]
        public static extern unsafe void GetLightfv(int light, int pname, [Out] Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetLightiv", ExactSpelling = true)]
        public static extern unsafe void GetLightiv(int light, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetMapdv", ExactSpelling = true)]
        public static extern unsafe void GetMapdv(int target, int query, [Out] Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetMapfv", ExactSpelling = true)]
        public static extern unsafe void GetMapfv(int target, int query, [Out] Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetMapiv", ExactSpelling = true)]
        public static extern unsafe void GetMapiv(int target, int query, [Out] Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetMaterialfv", ExactSpelling = true)]
        public static extern unsafe void GetMaterialfv(int face, int pname, [Out] Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetMaterialiv", ExactSpelling = true)]
        public static extern unsafe void GetMaterialiv(int face, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetPixelMapfv", ExactSpelling = true)]
        public static extern unsafe void GetPixelMapfv(int map, [Out] Single* values);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetPixelMapuiv", ExactSpelling = true)]
        public static extern unsafe void GetPixelMapuiv(int map, [Out] UInt32* values);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetPixelMapusv", ExactSpelling = true)]
        public static extern unsafe void GetPixelMapusv(int map, [Out] UInt16* values);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetPolygonStipple", ExactSpelling = true)]
        public static extern unsafe void GetPolygonStipple([Out] Byte* mask);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetString", ExactSpelling = true)]
        public static extern IntPtr GetString(int name);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetTexEnvfv", ExactSpelling = true)]
        public static extern unsafe void GetTexEnvfv(int target, int pname, [Out] Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetTexEnviv", ExactSpelling = true)]
        public static extern unsafe void GetTexEnviv(int target, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetTexGendv", ExactSpelling = true)]
        public static extern unsafe void GetTexGendv(int coord, int pname, [Out] Double* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetTexGenfv", ExactSpelling = true)]
        public static extern unsafe void GetTexGenfv(int coord, int pname, [Out] Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetTexGeniv", ExactSpelling = true)]
        public static extern unsafe void GetTexGeniv(int coord, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetTexImage", ExactSpelling = true)]
        public static extern void GetTexImage(int target, Int32 level, int format, int type, [Out] IntPtr pixels);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetTexParameterfv", ExactSpelling = true)]
        public static extern unsafe void GetTexParameterfv(int target, int pname, [Out] Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetTexParameteriv", ExactSpelling = true)]
        public static extern unsafe void GetTexParameteriv(int target, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetTexLevelParameterfv", ExactSpelling = true)]
        public static extern unsafe void GetTexLevelParameterfv(int target, Int32 level, int pname, [Out] Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetTexLevelParameteriv", ExactSpelling = true)]
        public static extern unsafe void GetTexLevelParameteriv(int target, Int32 level, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIsEnabled", ExactSpelling = true)]
        public static extern Int32 IsEnabled(int cap);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIsList", ExactSpelling = true)]
        public static extern Int32 IsList(UInt32 list);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDepthRange", ExactSpelling = true)]
        public static extern void DepthRange(Double near, Double far);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glFrustum", ExactSpelling = true)]
        public static extern void Frustum(Double left, Double right, Double bottom, Double top, Double zNear, Double zFar);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLoadIdentity", ExactSpelling = true)]
        public static extern void LoadIdentity();
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLoadMatrixf", ExactSpelling = true)]
        public static extern unsafe void LoadMatrixf(Single* m);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLoadMatrixd", ExactSpelling = true)]
        public static extern unsafe void LoadMatrixd(Double* m);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMatrixMode", ExactSpelling = true)]
        public static extern void MatrixMode(int mode);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultMatrixf", ExactSpelling = true)]
        public static extern unsafe void MultMatrixf(Single* m);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultMatrixd", ExactSpelling = true)]
        public static extern unsafe void MultMatrixd(Double* m);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glOrtho", ExactSpelling = true)]
        public static extern void Ortho(Double left, Double right, Double bottom, Double top, Double zNear, Double zFar);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPopMatrix", ExactSpelling = true)]
        public static extern void PopMatrix();
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPushMatrix", ExactSpelling = true)]
        public static extern void PushMatrix();
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRotated", ExactSpelling = true)]
        public static extern void Rotated(Double angle, Double x, Double y, Double z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glRotatef", ExactSpelling = true)]
        public static extern void Rotatef(Single angle, Single x, Single y, Single z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glScaled", ExactSpelling = true)]
        public static extern void Scaled(Double x, Double y, Double z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glScalef", ExactSpelling = true)]
        public static extern void Scalef(Single x, Single y, Single z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTranslated", ExactSpelling = true)]
        public static extern void Translated(Double x, Double y, Double z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTranslatef", ExactSpelling = true)]
        public static extern void Translatef(Single x, Single y, Single z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glViewport", ExactSpelling = true)]
        public static extern void Viewport(Int32 x, Int32 y, Int32 width, Int32 height);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glArrayElement", ExactSpelling = true)]
        public static extern void ArrayElement(Int32 i);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColorPointer", ExactSpelling = true)]
        public static extern void ColorPointer(Int32 size, int type, Int32 stride, IntPtr pointer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDisableClientState", ExactSpelling = true)]
        public static extern void DisableClientState(int array);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDrawArrays", ExactSpelling = true)]
        public static extern void DrawArrays(int mode, Int32 first, Int32 count);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDrawElements", ExactSpelling = true)]
        public static extern void DrawElements(int mode, Int32 count, int type, IntPtr indices);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEdgeFlagPointer", ExactSpelling = true)]
        public static extern void EdgeFlagPointer(Int32 stride, IntPtr pointer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEnableClientState", ExactSpelling = true)]
        public static extern void EnableClientState(int array);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetPointerv", ExactSpelling = true)]
        public static extern void GetPointerv(int pname, [Out] IntPtr @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIndexPointer", ExactSpelling = true)]
        public static extern void IndexPointer(int type, Int32 stride, IntPtr pointer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glInterleavedArrays", ExactSpelling = true)]
        public static extern void InterleavedArrays(int format, Int32 stride, IntPtr pointer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glNormalPointer", ExactSpelling = true)]
        public static extern void NormalPointer(int type, Int32 stride, IntPtr pointer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexCoordPointer", ExactSpelling = true)]
        public static extern void TexCoordPointer(Int32 size, int type, Int32 stride, IntPtr pointer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexPointer", ExactSpelling = true)]
        public static extern void VertexPointer(Int32 size, int type, Int32 stride, IntPtr pointer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPolygonOffset", ExactSpelling = true)]
        public static extern void PolygonOffset(Single factor, Single units);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCopyTexImage1D", ExactSpelling = true)]
        public static extern void CopyTexImage1D(int target, Int32 level, int publicformat, Int32 x, Int32 y, Int32 width, Int32 border);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCopyTexImage2D", ExactSpelling = true)]
        public static extern void CopyTexImage2D(int target, Int32 level, int publicformat, Int32 x, Int32 y, Int32 width, Int32 height, Int32 border);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCopyTexSubImage1D", ExactSpelling = true)]
        public static extern void CopyTexSubImage1D(int target, Int32 level, Int32 xoffset, Int32 x, Int32 y, Int32 width);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCopyTexSubImage2D", ExactSpelling = true)]
        public static extern void CopyTexSubImage2D(int target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 x, Int32 y, Int32 width, Int32 height);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexSubImage1D", ExactSpelling = true)]
        public static extern void TexSubImage1D(int target, Int32 level, Int32 xoffset, Int32 width, int format, int type, IntPtr pixels);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexSubImage2D", ExactSpelling = true)]
        public static extern void TexSubImage2D(int target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, int format, int type, IntPtr pixels);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glAreTexturesResident", ExactSpelling = true)]
        public static extern unsafe Int32 AreTexturesResident(Int32 n, UInt32* textures, [Out] Int32* residences);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glBindTexture", ExactSpelling = true)]
        public static extern void BindTexture(int target, UInt32 texture);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDeleteTextures", ExactSpelling = true)]
        public static extern unsafe void DeleteTextures(Int32 n, UInt32* textures);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGenTextures", ExactSpelling = true)]
        public static extern unsafe void GenTextures(Int32 n, [Out] UInt32* textures);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIsTexture", ExactSpelling = true)]
        public static extern Int32 IsTexture(UInt32 texture);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPrioritizeTextures", ExactSpelling = true)]
        public static extern unsafe void PrioritizeTextures(Int32 n, UInt32* textures, Single* priorities);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIndexub", ExactSpelling = true)]
        public static extern void Indexub(Byte c);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIndexubv", ExactSpelling = true)]
        public static extern unsafe void Indexubv(Byte* c);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPopClientAttrib", ExactSpelling = true)]
        public static extern void PopClientAttrib();
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPushClientAttrib", ExactSpelling = true)]
        public static extern void PushClientAttrib(int mask);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glBlendColor", ExactSpelling = true)]
        public static extern void BlendColor(Single red, Single green, Single blue, Single alpha);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glBlendEquation", ExactSpelling = true)]
        public static extern void BlendEquation(int mode);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDrawRangeElements", ExactSpelling = true)]
        public static extern void DrawRangeElements(int mode, UInt32 start, UInt32 end, Int32 count, int type, IntPtr indices);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColorTable", ExactSpelling = true)]
        public static extern void ColorTable(int target, int publicformat, Int32 width, int format, int type, IntPtr table);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColorTableParameterfv", ExactSpelling = true)]
        public static extern unsafe void ColorTableParameterfv(int target, int pname, Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColorTableParameteriv", ExactSpelling = true)]
        public static extern unsafe void ColorTableParameteriv(int target, int pname, Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCopyColorTable", ExactSpelling = true)]
        public static extern void CopyColorTable(int target, int publicformat, Int32 x, Int32 y, Int32 width);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetColorTable", ExactSpelling = true)]
        public static extern void GetColorTable(int target, int format, int type, [Out] IntPtr table);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetColorTableParameterfv", ExactSpelling = true)]
        public static extern unsafe void GetColorTableParameterfv(int target, int pname, [Out] Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetColorTableParameteriv", ExactSpelling = true)]
        public static extern unsafe void GetColorTableParameteriv(int target, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glColorSubTable", ExactSpelling = true)]
        public static extern void ColorSubTable(int target, Int32 start, Int32 count, int format, int type, IntPtr data);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCopyColorSubTable", ExactSpelling = true)]
        public static extern void CopyColorSubTable(int target, Int32 start, Int32 x, Int32 y, Int32 width);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glConvolutionFilter1D", ExactSpelling = true)]
        public static extern void ConvolutionFilter1D(int target, int publicformat, Int32 width, int format, int type, IntPtr image);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glConvolutionFilter2D", ExactSpelling = true)]
        public static extern void ConvolutionFilter2D(int target, int publicformat, Int32 width, Int32 height, int format, int type, IntPtr image);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glConvolutionParameterf", ExactSpelling = true)]
        public static extern void ConvolutionParameterf(int target, int pname, Single @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glConvolutionParameterfv", ExactSpelling = true)]
        public static extern unsafe void ConvolutionParameterfv(int target, int pname, Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glConvolutionParameteri", ExactSpelling = true)]
        public static extern void ConvolutionParameteri(int target, int pname, Int32 @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glConvolutionParameteriv", ExactSpelling = true)]
        public static extern unsafe void ConvolutionParameteriv(int target, int pname, Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCopyConvolutionFilter1D", ExactSpelling = true)]
        public static extern void CopyConvolutionFilter1D(int target, int publicformat, Int32 x, Int32 y, Int32 width);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCopyConvolutionFilter2D", ExactSpelling = true)]
        public static extern void CopyConvolutionFilter2D(int target, int publicformat, Int32 x, Int32 y, Int32 width, Int32 height);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetConvolutionFilter", ExactSpelling = true)]
        public static extern void GetConvolutionFilter(int target, int format, int type, [Out] IntPtr image);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetConvolutionParameterfv", ExactSpelling = true)]
        public static extern unsafe void GetConvolutionParameterfv(int target, int pname, [Out] Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetConvolutionParameteriv", ExactSpelling = true)]
        public static extern unsafe void GetConvolutionParameteriv(int target, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetSeparableFilter", ExactSpelling = true)]
        public static extern void GetSeparableFilter(int target, int format, int type, [Out] IntPtr row, [Out] IntPtr column, [Out] IntPtr span);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSeparableFilter2D", ExactSpelling = true)]
        public static extern void SeparableFilter2D(int target, int publicformat, Int32 width, Int32 height, int format, int type, IntPtr row, IntPtr column);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetHistogram", ExactSpelling = true)]
        public static extern void GetHistogram(int target, Int32 reset, int format, int type, [Out] IntPtr values);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetHistogramParameterfv", ExactSpelling = true)]
        public static extern unsafe void GetHistogramParameterfv(int target, int pname, [Out] Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetHistogramParameteriv", ExactSpelling = true)]
        public static extern unsafe void GetHistogramParameteriv(int target, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetMinmax", ExactSpelling = true)]
        public static extern void GetMinmax(int target, Int32 reset, int format, int type, [Out] IntPtr values);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetMinmaxParameterfv", ExactSpelling = true)]
        public static extern unsafe void GetMinmaxParameterfv(int target, int pname, [Out] Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetMinmaxParameteriv", ExactSpelling = true)]
        public static extern unsafe void GetMinmaxParameteriv(int target, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glHistogram", ExactSpelling = true)]
        public static extern void Histogram(int target, Int32 width, int publicformat, Int32 sink);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMinmax", ExactSpelling = true)]
        public static extern void Minmax(int target, int publicformat, Int32 sink);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glResetHistogram", ExactSpelling = true)]
        public static extern void ResetHistogram(int target);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glResetMinmax", ExactSpelling = true)]
        public static extern void ResetMinmax(int target);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexImage3D", ExactSpelling = true)]
        public static extern void TexImage3D(int target, Int32 level, int publicformat, Int32 width, Int32 height, Int32 depth, Int32 border, int format, int type, IntPtr pixels);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glTexSubImage3D", ExactSpelling = true)]
        public static extern void TexSubImage3D(int target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, int format, int type, IntPtr pixels);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCopyTexSubImage3D", ExactSpelling = true)]
        public static extern void CopyTexSubImage3D(int target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 x, Int32 y, Int32 width, Int32 height);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glActiveTexture", ExactSpelling = true)]
        public static extern void ActiveTexture(int texture);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glClientActiveTexture", ExactSpelling = true)]
        public static extern void ClientActiveTexture(int texture);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord1d", ExactSpelling = true)]
        public static extern void MultiTexCoord1d(int target, Double s);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord1dv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord1dv(int target, Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord1f", ExactSpelling = true)]
        public static extern void MultiTexCoord1f(int target, Single s);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord1fv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord1fv(int target, Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord1i", ExactSpelling = true)]
        public static extern void MultiTexCoord1i(int target, Int32 s);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord1iv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord1iv(int target, Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord1s", ExactSpelling = true)]
        public static extern void MultiTexCoord1s(int target, Int16 s);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord1sv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord1sv(int target, Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord2d", ExactSpelling = true)]
        public static extern void MultiTexCoord2d(int target, Double s, Double t);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord2dv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord2dv(int target, Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord2f", ExactSpelling = true)]
        public static extern void MultiTexCoord2f(int target, Single s, Single t);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord2fv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord2fv(int target, Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord2i", ExactSpelling = true)]
        public static extern void MultiTexCoord2i(int target, Int32 s, Int32 t);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord2iv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord2iv(int target, Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord2s", ExactSpelling = true)]
        public static extern void MultiTexCoord2s(int target, Int16 s, Int16 t);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord2sv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord2sv(int target, Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord3d", ExactSpelling = true)]
        public static extern void MultiTexCoord3d(int target, Double s, Double t, Double r);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord3dv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord3dv(int target, Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord3f", ExactSpelling = true)]
        public static extern void MultiTexCoord3f(int target, Single s, Single t, Single r);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord3fv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord3fv(int target, Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord3i", ExactSpelling = true)]
        public static extern void MultiTexCoord3i(int target, Int32 s, Int32 t, Int32 r);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord3iv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord3iv(int target, Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord3s", ExactSpelling = true)]
        public static extern void MultiTexCoord3s(int target, Int16 s, Int16 t, Int16 r);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord3sv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord3sv(int target, Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord4d", ExactSpelling = true)]
        public static extern void MultiTexCoord4d(int target, Double s, Double t, Double r, Double q);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord4dv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord4dv(int target, Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord4f", ExactSpelling = true)]
        public static extern void MultiTexCoord4f(int target, Single s, Single t, Single r, Single q);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord4fv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord4fv(int target, Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord4i", ExactSpelling = true)]
        public static extern void MultiTexCoord4i(int target, Int32 s, Int32 t, Int32 r, Int32 q);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord4iv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord4iv(int target, Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord4s", ExactSpelling = true)]
        public static extern void MultiTexCoord4s(int target, Int16 s, Int16 t, Int16 r, Int16 q);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiTexCoord4sv", ExactSpelling = true)]
        public static extern unsafe void MultiTexCoord4sv(int target, Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLoadTransposeMatrixf", ExactSpelling = true)]
        public static extern unsafe void LoadTransposeMatrixf(Single* m);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLoadTransposeMatrixd", ExactSpelling = true)]
        public static extern unsafe void LoadTransposeMatrixd(Double* m);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultTransposeMatrixf", ExactSpelling = true)]
        public static extern unsafe void MultTransposeMatrixf(Single* m);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultTransposeMatrixd", ExactSpelling = true)]
        public static extern unsafe void MultTransposeMatrixd(Double* m);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSampleCoverage", ExactSpelling = true)]
        public static extern void SampleCoverage(Single value, Int32 invert);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCompressedTexImage3D", ExactSpelling = true)]
        public static extern void CompressedTexImage3D(int target, Int32 level, int publicformat, Int32 width, Int32 height, Int32 depth, Int32 border, Int32 imageSize, IntPtr data);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCompressedTexImage2D", ExactSpelling = true)]
        public static extern void CompressedTexImage2D(int target, Int32 level, int publicformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, IntPtr data);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCompressedTexImage1D", ExactSpelling = true)]
        public static extern void CompressedTexImage1D(int target, Int32 level, int publicformat, Int32 width, Int32 border, Int32 imageSize, IntPtr data);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCompressedTexSubImage3D", ExactSpelling = true)]
        public static extern void CompressedTexSubImage3D(int target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, int format, Int32 imageSize, IntPtr data);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCompressedTexSubImage2D", ExactSpelling = true)]
        public static extern void CompressedTexSubImage2D(int target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, int format, Int32 imageSize, IntPtr data);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCompressedTexSubImage1D", ExactSpelling = true)]
        public static extern void CompressedTexSubImage1D(int target, Int32 level, Int32 xoffset, Int32 width, int format, Int32 imageSize, IntPtr data);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetCompressedTexImage", ExactSpelling = true)]
        public static extern void GetCompressedTexImage(int target, Int32 level, [Out] IntPtr img);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glBlendFuncSeparate", ExactSpelling = true)]
        public static extern void BlendFuncSeparate(int sfactorRGB, int dfactorRGB, int sfactorAlpha, int dfactorAlpha);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glFogCoordf", ExactSpelling = true)]
        public static extern void FogCoordf(Single coord);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glFogCoordfv", ExactSpelling = true)]
        public static extern unsafe void FogCoordfv(Single* coord);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glFogCoordd", ExactSpelling = true)]
        public static extern void FogCoordd(Double coord);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glFogCoorddv", ExactSpelling = true)]
        public static extern unsafe void FogCoorddv(Double* coord);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glFogCoordPointer", ExactSpelling = true)]
        public static extern void FogCoordPointer(int type, Int32 stride, IntPtr pointer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiDrawArrays", ExactSpelling = true)]
        public static extern unsafe void MultiDrawArrays(int mode, [Out] Int32* first, [Out] Int32* count, Int32 primcount);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMultiDrawElements", ExactSpelling = true)]
        public static extern unsafe void MultiDrawElements(int mode, Int32* count, int type, IntPtr indices, Int32 primcount);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPointParameterf", ExactSpelling = true)]
        public static extern void PointParameterf(int pname, Single param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPointParameterfv", ExactSpelling = true)]
        public static extern unsafe void PointParameterfv(int pname, Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPointParameteri", ExactSpelling = true)]
        public static extern void PointParameteri(int pname, Int32 param);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glPointParameteriv", ExactSpelling = true)]
        public static extern unsafe void PointParameteriv(int pname, Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3b", ExactSpelling = true)]
        public static extern void SecondaryColor3b(SByte red, SByte green, SByte blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3bv", ExactSpelling = true)]
        public static extern unsafe void SecondaryColor3bv(SByte* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3d", ExactSpelling = true)]
        public static extern void SecondaryColor3d(Double red, Double green, Double blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3dv", ExactSpelling = true)]
        public static extern unsafe void SecondaryColor3dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3f", ExactSpelling = true)]
        public static extern void SecondaryColor3f(Single red, Single green, Single blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3fv", ExactSpelling = true)]
        public static extern unsafe void SecondaryColor3fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3i", ExactSpelling = true)]
        public static extern void SecondaryColor3i(Int32 red, Int32 green, Int32 blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3iv", ExactSpelling = true)]
        public static extern unsafe void SecondaryColor3iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3s", ExactSpelling = true)]
        public static extern void SecondaryColor3s(Int16 red, Int16 green, Int16 blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3sv", ExactSpelling = true)]
        public static extern unsafe void SecondaryColor3sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3ub", ExactSpelling = true)]
        public static extern void SecondaryColor3ub(Byte red, Byte green, Byte blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3ubv", ExactSpelling = true)]
        public static extern unsafe void SecondaryColor3ubv(Byte* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3ui", ExactSpelling = true)]
        public static extern void SecondaryColor3ui(UInt32 red, UInt32 green, UInt32 blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3uiv", ExactSpelling = true)]
        public static extern unsafe void SecondaryColor3uiv(UInt32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3us", ExactSpelling = true)]
        public static extern void SecondaryColor3us(UInt16 red, UInt16 green, UInt16 blue);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColor3usv", ExactSpelling = true)]
        public static extern unsafe void SecondaryColor3usv(UInt16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glSecondaryColorPointer", ExactSpelling = true)]
        public static extern void SecondaryColorPointer(Int32 size, int type, Int32 stride, IntPtr pointer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos2d", ExactSpelling = true)]
        public static extern void WindowPos2d(Double x, Double y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos2dv", ExactSpelling = true)]
        public static extern unsafe void WindowPos2dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos2f", ExactSpelling = true)]
        public static extern void WindowPos2f(Single x, Single y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos2fv", ExactSpelling = true)]
        public static extern unsafe void WindowPos2fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos2i", ExactSpelling = true)]
        public static extern void WindowPos2i(Int32 x, Int32 y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos2iv", ExactSpelling = true)]
        public static extern unsafe void WindowPos2iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos2s", ExactSpelling = true)]
        public static extern void WindowPos2s(Int16 x, Int16 y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos2sv", ExactSpelling = true)]
        public static extern unsafe void WindowPos2sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos3d", ExactSpelling = true)]
        public static extern void WindowPos3d(Double x, Double y, Double z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos3dv", ExactSpelling = true)]
        public static extern unsafe void WindowPos3dv(Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos3f", ExactSpelling = true)]
        public static extern void WindowPos3f(Single x, Single y, Single z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos3fv", ExactSpelling = true)]
        public static extern unsafe void WindowPos3fv(Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos3i", ExactSpelling = true)]
        public static extern void WindowPos3i(Int32 x, Int32 y, Int32 z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos3iv", ExactSpelling = true)]
        public static extern unsafe void WindowPos3iv(Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos3s", ExactSpelling = true)]
        public static extern void WindowPos3s(Int16 x, Int16 y, Int16 z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glWindowPos3sv", ExactSpelling = true)]
        public static extern unsafe void WindowPos3sv(Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGenQueries", ExactSpelling = true)]
        public static extern unsafe void GenQueries(Int32 n, [Out] UInt32* ids);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDeleteQueries", ExactSpelling = true)]
        public static extern unsafe void DeleteQueries(Int32 n, UInt32* ids);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIsQuery", ExactSpelling = true)]
        public static extern Int32 IsQuery(UInt32 id);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glBeginQuery", ExactSpelling = true)]
        public static extern void BeginQuery(int target, UInt32 id);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEndQuery", ExactSpelling = true)]
        public static extern void EndQuery(int target);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetQueryiv", ExactSpelling = true)]
        public static extern unsafe void GetQueryiv(int target, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetQueryObjectiv", ExactSpelling = true)]
        public static extern unsafe void GetQueryObjectiv(UInt32 id, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetQueryObjectuiv", ExactSpelling = true)]
        public static extern unsafe void GetQueryObjectuiv(UInt32 id, int pname, [Out] UInt32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glBindBuffer", ExactSpelling = true)]
        public static extern void BindBuffer(int target, UInt32 buffer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDeleteBuffers", ExactSpelling = true)]
        public static extern unsafe void DeleteBuffers(Int32 n, UInt32* buffers);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGenBuffers", ExactSpelling = true)]
        public static extern unsafe void GenBuffers(Int32 n, [Out] UInt32* buffers);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIsBuffer", ExactSpelling = true)]
        public static extern Int32 IsBuffer(UInt32 buffer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glBufferData", ExactSpelling = true)]
        public static extern void BufferData(int target, IntPtr size, IntPtr data, int usage);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glBufferSubData", ExactSpelling = true)]
        public static extern void BufferSubData(int target, IntPtr offset, IntPtr size, IntPtr data);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetBufferSubData", ExactSpelling = true)]
        public static extern void GetBufferSubData(int target, IntPtr offset, IntPtr size, [Out] IntPtr data);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glMapBuffer", ExactSpelling = true)]
        public static extern unsafe IntPtr MapBuffer(int target, int access);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUnmapBuffer", ExactSpelling = true)]
        public static extern Int32 UnmapBuffer(int target);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetBufferParameteriv", ExactSpelling = true)]
        public static extern unsafe void GetBufferParameteriv(int target, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetBufferPointerv", ExactSpelling = true)]
        public static extern void GetBufferPointerv(int target, int pname, [Out] IntPtr @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glBlendEquationSeparate", ExactSpelling = true)]
        public static extern void BlendEquationSeparate(int modeRGB, int modeAlpha);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDrawBuffers", ExactSpelling = true)]
        public static extern unsafe void DrawBuffers(Int32 n, int* bufs);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glStencilOpSeparate", ExactSpelling = true)]
        public static extern void StencilOpSeparate(int face, int sfail, int dpfail, int dppass);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glStencilFuncSeparate", ExactSpelling = true)]
        public static extern void StencilFuncSeparate(int frontfunc, int backfunc, Int32 @ref, UInt32 mask);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glStencilMaskSeparate", ExactSpelling = true)]
        public static extern void StencilMaskSeparate(int face, UInt32 mask);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glAttachShader", ExactSpelling = true)]
        public static extern void AttachShader(UInt32 program, UInt32 shader);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glBindAttribLocation", ExactSpelling = true)]
        public static extern void BindAttribLocation(UInt32 program, UInt32 index, System.String name);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCompileShader", ExactSpelling = true)]
        public static extern void CompileShader(UInt32 shader);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCreateProgram", ExactSpelling = true)]
        public static extern Int32 CreateProgram();
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glCreateShader", ExactSpelling = true)]
        public static extern Int32 CreateShader(int type);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDeleteProgram", ExactSpelling = true)]
        public static extern void DeleteProgram(UInt32 program);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDeleteShader", ExactSpelling = true)]
        public static extern void DeleteShader(UInt32 shader);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDetachShader", ExactSpelling = true)]
        public static extern void DetachShader(UInt32 program, UInt32 shader);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glDisableVertexAttribArray", ExactSpelling = true)]
        public static extern void DisableVertexAttribArray(UInt32 index);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glEnableVertexAttribArray", ExactSpelling = true)]
        public static extern void EnableVertexAttribArray(UInt32 index);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetActiveAttrib", ExactSpelling = true)]
        public static extern unsafe void GetActiveAttrib(UInt32 program, UInt32 index, Int32 bufSize, [Out] Int32* length, [Out] Int32* size, [Out] int* type, [Out] System.Text.StringBuilder name);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetActiveUniform", ExactSpelling = true)]
        public static extern unsafe void GetActiveUniform(UInt32 program, UInt32 index, Int32 bufSize, [Out] Int32* length, [Out] Int32* size, [Out] int* type, [Out] System.Text.StringBuilder name);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetAttachedShaders", ExactSpelling = true)]
        public static extern unsafe void GetAttachedShaders(UInt32 program, Int32 maxCount, [Out] Int32* count, [Out] UInt32* obj);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetAttribLocation", ExactSpelling = true)]
        public static extern Int32 GetAttribLocation(UInt32 program, System.String name);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetProgramiv", ExactSpelling = true)]
        public static extern unsafe void GetProgramiv(UInt32 program, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetProgramInfoLog", ExactSpelling = true)]
        public static extern unsafe void GetProgramInfoLog(UInt32 program, Int32 bufSize, [Out] Int32* length, [Out] System.Text.StringBuilder infoLog);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetShaderiv", ExactSpelling = true)]
        public static extern unsafe void GetShaderiv(UInt32 shader, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetShaderInfoLog", ExactSpelling = true)]
        public static extern unsafe void GetShaderInfoLog(UInt32 shader, Int32 bufSize, [Out] Int32* length, [Out] System.Text.StringBuilder infoLog);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetShaderSource", ExactSpelling = true)]
        public static extern unsafe void GetShaderSource(UInt32 shader, Int32 bufSize, [Out] Int32* length, [Out] System.Text.StringBuilder[] source);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetUniformLocation", ExactSpelling = true)]
        public static extern Int32 GetUniformLocation(UInt32 program, System.String name);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetUniformfv", ExactSpelling = true)]
        public static extern unsafe void GetUniformfv(UInt32 program, Int32 location, [Out] Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetUniformiv", ExactSpelling = true)]
        public static extern unsafe void GetUniformiv(UInt32 program, Int32 location, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetVertexAttribdv", ExactSpelling = true)]
        public static extern unsafe void GetVertexAttribdv(UInt32 index, int pname, [Out] Double* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetVertexAttribfv", ExactSpelling = true)]
        public static extern unsafe void GetVertexAttribfv(UInt32 index, int pname, [Out] Single* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetVertexAttribiv", ExactSpelling = true)]
        public static extern unsafe void GetVertexAttribiv(UInt32 index, int pname, [Out] Int32* @params);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glGetVertexAttribPointerv", ExactSpelling = true)]
        public static extern void GetVertexAttribPointerv(UInt32 index, int pname, [Out] IntPtr pointer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIsProgram", ExactSpelling = true)]
        public static extern Int32 IsProgram(UInt32 program);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glIsShader", ExactSpelling = true)]
        public static extern Int32 IsShader(UInt32 shader);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glLinkProgram", ExactSpelling = true)]
        public static extern void LinkProgram(UInt32 program);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glShaderSource", ExactSpelling = true)]
        public static extern unsafe void ShaderSource(UInt32 shader, Int32 count, System.String[] @string, Int32* length);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUseProgram", ExactSpelling = true)]
        public static extern void UseProgram(UInt32 program);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform1f", ExactSpelling = true)]
        public static extern void Uniform1f(Int32 location, Single v0);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform2f", ExactSpelling = true)]
        public static extern void Uniform2f(Int32 location, Single v0, Single v1);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform3f", ExactSpelling = true)]
        public static extern void Uniform3f(Int32 location, Single v0, Single v1, Single v2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform4f", ExactSpelling = true)]
        public static extern void Uniform4f(Int32 location, Single v0, Single v1, Single v2, Single v3);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform1i", ExactSpelling = true)]
        public static extern void Uniform1i(Int32 location, Int32 v0);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform2i", ExactSpelling = true)]
        public static extern void Uniform2i(Int32 location, Int32 v0, Int32 v1);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform3i", ExactSpelling = true)]
        public static extern void Uniform3i(Int32 location, Int32 v0, Int32 v1, Int32 v2);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform4i", ExactSpelling = true)]
        public static extern void Uniform4i(Int32 location, Int32 v0, Int32 v1, Int32 v2, Int32 v3);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform1fv", ExactSpelling = true)]
        public static extern unsafe void Uniform1fv(Int32 location, Int32 count, Single* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform2fv", ExactSpelling = true)]
        public static extern unsafe void Uniform2fv(Int32 location, Int32 count, Single* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform3fv", ExactSpelling = true)]
        public static extern unsafe void Uniform3fv(Int32 location, Int32 count, Single* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform4fv", ExactSpelling = true)]
        public static extern unsafe void Uniform4fv(Int32 location, Int32 count, Single* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform1iv", ExactSpelling = true)]
        public static extern unsafe void Uniform1iv(Int32 location, Int32 count, Int32* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform2iv", ExactSpelling = true)]
        public static extern unsafe void Uniform2iv(Int32 location, Int32 count, Int32* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform3iv", ExactSpelling = true)]
        public static extern unsafe void Uniform3iv(Int32 location, Int32 count, Int32* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniform4iv", ExactSpelling = true)]
        public static extern unsafe void Uniform4iv(Int32 location, Int32 count, Int32* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniformMatrix2fv", ExactSpelling = true)]
        public static extern unsafe void UniformMatrix2fv(Int32 location, Int32 count, Int32 transpose, Single* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniformMatrix3fv", ExactSpelling = true)]
        public static extern unsafe void UniformMatrix3fv(Int32 location, Int32 count, Int32 transpose, Single* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniformMatrix4fv", ExactSpelling = true)]
        public static extern unsafe void UniformMatrix4fv(Int32 location, Int32 count, Int32 transpose, Single* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glValidateProgram", ExactSpelling = true)]
        public static extern void ValidateProgram(UInt32 program);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib1d", ExactSpelling = true)]
        public static extern void VertexAttrib1d(UInt32 index, Double x);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib1dv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib1dv(UInt32 index, Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib1f", ExactSpelling = true)]
        public static extern void VertexAttrib1f(UInt32 index, Single x);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib1fv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib1fv(UInt32 index, Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib1s", ExactSpelling = true)]
        public static extern void VertexAttrib1s(UInt32 index, Int16 x);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib1sv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib1sv(UInt32 index, Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib2d", ExactSpelling = true)]
        public static extern void VertexAttrib2d(UInt32 index, Double x, Double y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib2dv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib2dv(UInt32 index, Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib2f", ExactSpelling = true)]
        public static extern void VertexAttrib2f(UInt32 index, Single x, Single y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib2fv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib2fv(UInt32 index, Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib2s", ExactSpelling = true)]
        public static extern void VertexAttrib2s(UInt32 index, Int16 x, Int16 y);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib2sv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib2sv(UInt32 index, Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib3d", ExactSpelling = true)]
        public static extern void VertexAttrib3d(UInt32 index, Double x, Double y, Double z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib3dv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib3dv(UInt32 index, Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib3f", ExactSpelling = true)]
        public static extern void VertexAttrib3f(UInt32 index, Single x, Single y, Single z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib3fv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib3fv(UInt32 index, Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib3s", ExactSpelling = true)]
        public static extern void VertexAttrib3s(UInt32 index, Int16 x, Int16 y, Int16 z);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib3sv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib3sv(UInt32 index, Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4Nbv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib4Nbv(UInt32 index, SByte* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4Niv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib4Niv(UInt32 index, Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4Nsv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib4Nsv(UInt32 index, Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4Nub", ExactSpelling = true)]
        public static extern void VertexAttrib4Nub(UInt32 index, Byte x, Byte y, Byte z, Byte w);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4Nubv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib4Nubv(UInt32 index, Byte* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4Nuiv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib4Nuiv(UInt32 index, UInt32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4Nusv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib4Nusv(UInt32 index, UInt16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4bv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib4bv(UInt32 index, SByte* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4d", ExactSpelling = true)]
        public static extern void VertexAttrib4d(UInt32 index, Double x, Double y, Double z, Double w);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4dv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib4dv(UInt32 index, Double* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4f", ExactSpelling = true)]
        public static extern void VertexAttrib4f(UInt32 index, Single x, Single y, Single z, Single w);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4fv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib4fv(UInt32 index, Single* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4iv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib4iv(UInt32 index, Int32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4s", ExactSpelling = true)]
        public static extern void VertexAttrib4s(UInt32 index, Int16 x, Int16 y, Int16 z, Int16 w);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4sv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib4sv(UInt32 index, Int16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4ubv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib4ubv(UInt32 index, Byte* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4uiv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib4uiv(UInt32 index, UInt32* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttrib4usv", ExactSpelling = true)]
        public static extern unsafe void VertexAttrib4usv(UInt32 index, UInt16* v);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glVertexAttribPointer", ExactSpelling = true)]
        public static extern void VertexAttribPointer(UInt32 index, Int32 size, int type, Int32 normalized, Int32 stride, IntPtr pointer);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniformMatrix2x3fv", ExactSpelling = true)]
        public static extern unsafe void UniformMatrix2x3fv(Int32 location, Int32 count, Int32 transpose, Single* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniformMatrix3x2fv", ExactSpelling = true)]
        public static extern unsafe void UniformMatrix3x2fv(Int32 location, Int32 count, Int32 transpose, Single* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniformMatrix2x4fv", ExactSpelling = true)]
        public static extern unsafe void UniformMatrix2x4fv(Int32 location, Int32 count, Int32 transpose, Single* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniformMatrix4x2fv", ExactSpelling = true)]
        public static extern unsafe void UniformMatrix4x2fv(Int32 location, Int32 count, Int32 transpose, Single* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniformMatrix3x4fv", ExactSpelling = true)]
        public static extern unsafe void UniformMatrix3x4fv(Int32 location, Int32 count, Int32 transpose, Single* value);
        [System.Security.SuppressUnmanagedCodeSecurity()]
        [DllImport("opengl32.dll", EntryPoint = "glUniformMatrix4x3fv", ExactSpelling = true)]
        public static extern unsafe void UniformMatrix4x3fv(Int32 location, Int32 count, Int32 transpose, Single* value);
        
    }
}
