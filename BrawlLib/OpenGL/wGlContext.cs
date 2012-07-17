using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using BrawlLib.OpenGL.etc;

namespace BrawlLib.OpenGL
{
    internal unsafe class wGlContext : GLContext
    {
        VoidPtr _hwnd, _hdc, _hglrc;

        public override void Share(GLContext ctx)
        {
            if (!wGL.wglShareLists(_hglrc, ((wGlContext)ctx)._hglrc))
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public override void Dispose()
        {
            if (_hglrc)
            {
                wGL.wglMakeCurrent(null, null);
                if (!wGL.wglDeleteContext(_hglrc))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                _hglrc = null;
            }
            if (_hdc)
            {
                //Win32.ReleaseDC(_hwnd, _hdc);
                _hdc = null;
            }
            base.Dispose();
        }

        public wGlContext(Control target)
        {
            wGL.wglMakeCurrent(null, null);

            _hwnd = target.Handle;
            if (!(_hdc = Win32.GetDC(_hwnd)))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            PixelFormatDescriptor pfd = new PixelFormatDescriptor(32, 32);

            int format = wGL.ChoosePixelFormat(_hdc, &pfd);
            if (format == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (wGL.DescribePixelFormat(_hdc, format, pfd.nSize, &pfd) == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (!wGL.SetPixelFormat(_hdc, format, &pfd))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (!(_hglrc = wGL.wglCreateContext(_hdc)))
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public override void Capture()
        {
            if ((_hdc) && (_hglrc))
            {
                if (!wGL.wglMakeCurrent(_hdc, _hglrc))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
        public override void Swap()
        {
            if ((_hdc) && (_hglrc))
                wGL.wglSwapBuffers(_hdc);
        }
        public override void Release()
        {
            wGL.wglMakeCurrent(null, null);
        }

        internal override void glAccum(GLAccumOp op, float value) { wGL.glAccum(op, value); }

        private delegate void ActiveTextureDelegate(GLMultiTextureTarget texture);
        private ActiveTextureDelegate _pActiveTexture;
        internal override void glActiveTexture(GLMultiTextureTarget texture)
        {
            if (_pActiveTexture == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glActiveTexture");
                if (ptr == null)
                    ptr = wGL.wglGetProcAddress("glActiveTextureARB");
                _pActiveTexture = (ActiveTextureDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(ActiveTextureDelegate));
            }
            _pActiveTexture(texture);
        }
        internal override void glAlphaFunc(GLAlphaFunc func, float refValue) { wGL.glAlphaFunc(func, refValue); }
        internal override bool glAreTexturesResident(int num, uint* textures, bool* residences) { return wGL.glAreTexturesResident(num, textures, residences); }
        internal override void glArrayElement(int index) { wGL.glArrayElement(index); }
        internal override void glBegin(GLPrimitiveType mode) { wGL.glBegin(mode); }
        internal override void glBindTexture(GLTextureTarget target, uint texture) { wGL.glBindTexture(target, texture); }
        internal override void glBitmap(int width, int height, float xorig, float yorig, float xmove, float ymove, byte* bitmap) { wGL.glBitmap(width, height, xorig, yorig, xmove, ymove, bitmap); }
        internal override void glBlendFunc(GLBlendFactor sfactor, GLBlendFactor dfactor) { wGL.glBlendFunc(sfactor, dfactor); }
        internal override void glBindBuffer(BufferTarget target, Int32 buffer) { wGL.glBindBuffer(target, buffer); }

        internal override void glCallList(uint list) { wGL.glCallList(list); }
        internal override void glCallLists(int n, uint type, void* lists) { wGL.glCallLists(n, type, lists); }

        internal override void glClear(GLClearMask mask) { wGL.glClear(mask); }
        internal override void glClearAccum(float red, float green, float blue, float alpha) { wGL.glClearAccum(red, green, blue, alpha); }
        internal override void glClearColor(float red, float green, float blue, float alpha) { wGL.glClearColor(red, green, blue, alpha); }
        internal override void glClearDepth(double depth) { wGL.glClearDepth(depth); }
        internal override void glClearIndex(float c) { wGL.glClearIndex(c); }
        internal override void glClearStencil(int s) { wGL.glClearStencil(s); }
        internal override void glClipPlane(uint plane, double* equation) { wGL.glClipPlane(plane, equation); }

        internal override void glColor(sbyte red, sbyte green, sbyte blue) { wGL.glColor3b(red, green, blue); }
        internal override void glColor(double red, double green, double blue) { wGL.glColor3d(red, green, blue); }
        internal override void glColor(float red, float green, float blue) { wGL.glColor3f(red, green, blue); }
        internal override void glColor(int red, int green, int blue) { wGL.glColor3i(red, green, blue); }
        internal override void glColor(short red, short green, short blue) { wGL.glColor3s(red, green, blue); }
        internal override void glColor(byte red, byte green, byte blue) { wGL.glColor3ub(red, green, blue); }
        internal override void glColor(uint red, uint green, uint blue) { wGL.glColor3ui(red, green, blue); }
        internal override void glColor(ushort red, ushort green, ushort blue) { wGL.glColor3us(red, green, blue); }
        internal override void glColor(sbyte red, sbyte green, sbyte blue, sbyte alpha) { wGL.glColor4b(red, green, blue, alpha); }
        internal override void glColor(double red, double green, double blue, double alpha) { wGL.glColor4d(red, green, blue, alpha); }
        internal override void glColor(float red, float green, float blue, float alpha) { wGL.glColor4f(red, green, blue, alpha); }
        internal override void glColor(int red, int green, int blue, int alpha) { wGL.glColor4i(red, green, blue, alpha); }
        internal override void glColor(short red, short green, short blue, short alpha) { wGL.glColor4s(red, green, blue, alpha); }
        internal override void glColor(byte red, byte green, byte blue, byte alpha) { wGL.glColor4ub(red, green, blue, alpha); }
        internal override void glColor(uint red, uint green, uint blue, uint alpha) { wGL.glColor4ui(red, green, blue, alpha); }
        internal override void glColor(ushort red, ushort green, ushort blue, ushort alpha) { wGL.glColor4us(red, green, blue, alpha); }
        internal override void glColor3(sbyte* v) { wGL.glColor3bv(v); }
        internal override void glColor3(double* v) { wGL.glColor3dv(v); }
        internal override void glColor3(float* v) { wGL.glColor3fv(v); }
        internal override void glColor3(int* v) { wGL.glColor3iv(v); }
        internal override void glColor3(short* v) { wGL.glColor3sv(v); }
        internal override void glColor3(byte* v) { wGL.glColor3ubv(v); }
        internal override void glColor3(uint* v) { wGL.glColor3uiv(v); }
        internal override void glColor3(ushort* v) { wGL.glColor3usv(v); }
        internal override void glColor4(sbyte* v) { wGL.glColor4bv(v); }
        internal override void glColor4(double* v) { wGL.glColor4dv(v); }
        internal override void glColor4(float* v) { wGL.glColor4fv(v); }
        internal override void glColor4(int* v) { wGL.glColor4iv(v); }
        internal override void glColor4(short* v) { wGL.glColor4sv(v); }
        internal override void glColor4(byte* v) { wGL.glColor4ubv(v); }
        internal override void glColor4(uint* v) { wGL.glColor4uiv(v); }
        internal override void glColor4(ushort* v) { wGL.glColor4usv(v); }

        internal override void glColorMask(bool red, bool green, bool blue, bool alpha) { wGL.glColorMask(red, green, blue, alpha); }
        internal override void glColorMaterial(GLFace face, GLMaterialParameter mode) { wGL.glColorMaterial(face, mode); }
        internal override void glColorPointer(int size, GLDataType type, int stride, void* pointer) { wGL.glColorPointer(size, type, stride, pointer); }

        internal override void glCopyPixels(int x, int y, int width, int height, uint type) { wGL.glCopyPixels(x, y, width, height, type); }
        internal override void glCopyTexImage1D(GLTextureTarget target, int level, GLInternalPixelFormat internalFormat, int x, int y, int width, int border) { wGL.glCopyTexImage1D(target, level, internalFormat, x, y, width, border); }
        internal override void glCopyTexImage2D(GLTextureTarget target, int level, GLInternalPixelFormat internalFormat, int x, int y, int width, int height, int border) { wGL.glCopyTexImage2D(target, level, internalFormat, x, y, width, height, border); }
        internal override void glCopyTexSubImage1D(GLTextureTarget target, int level, int xOffset, int x, int y, int width) { wGL.glCopyTexSubImage1D(target, level, xOffset, x, y, width); }
        internal override void glCopyTexSubImage2D(GLTextureTarget target, int level, int xOffset, int yOffset, int x, int y, int width, int height) { wGL.glCopyTexSubImage2D(target, level, xOffset, yOffset, x, y, width, height); }

        #region Shaders

        //Pretty unorganized right now...

        //private delegate int GetShaderivDelegate(uint shader, uint pname, int* @params_ptr);
        //private GetShaderivDelegate _pGetShaderiv;
        //internal override int glGetShaderiv(uint shader, ShaderParameter pname)
        //{
        //    if (_pGetShaderiv == null)
        //    {
        //        VoidPtr ptr = wGL.wglGetProcAddress("glGetShaderiv");
        //        if (ptr == null)
        //            Console.WriteLine("wtf");
        //        _pGetShaderiv = (GetShaderivDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(GetShaderivDelegate));
        //    }
        //    return _pGetShaderiv(shader, (uint)pname, @params_ptr);
            
        //}

        private delegate int UseProgramObjectARBDelegate(uint programObj);
        private UseProgramObjectARBDelegate _pUseProgramObjectARB;
        internal override int glUseProgramObjectARB(uint programObj)
        {
            if (_pUseProgramObjectARB == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glUseProgramObjectARB");
                if (ptr == null)
                    Console.WriteLine("wtf");
                _pUseProgramObjectARB = (UseProgramObjectARBDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(UseProgramObjectARBDelegate));
            }
            return _pUseProgramObjectARB(programObj);
        }

        private delegate int CreateShaderObjectARBDelegate(uint shaderType);
        private CreateShaderObjectARBDelegate _pCreateShaderObjectARB;
        internal override int glCreateShaderObjectARB(ArbShaderObjects shaderType)
        {
            if (_pCreateShaderObjectARB == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glCreateShaderObjectARB");
                if (ptr == null)
                    Console.WriteLine("wtf");
                _pCreateShaderObjectARB = (CreateShaderObjectARBDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(CreateShaderObjectARBDelegate));
            }
            return _pCreateShaderObjectARB((uint)shaderType);
        }

        private delegate void ProgramStringARBDelegate(uint target, uint format, int len, IntPtr @string);
        private ProgramStringARBDelegate _pProgramStringARB;
        internal override void glProgramStringARB(AssemblyProgramTargetArb target, ArbVertexProgram format, int len, string @string)
        {
            GCHandle @string_ptr = GCHandle.Alloc(@string, GCHandleType.Pinned);
            if (_pProgramStringARB == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glProgramStringARB");
                if (ptr == null)
                    Console.WriteLine("wtf");
                _pProgramStringARB = (ProgramStringARBDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(ProgramStringARBDelegate));
            }
            _pProgramStringARB((uint)target, (uint)format, len, (IntPtr)@string_ptr.AddrOfPinnedObject());
        }

        private delegate void Uniform1Delegate(int location, int v0);
        private Uniform1Delegate _pUniform1;
        internal override void glUniform1(int location, int v0)
        {
            if (_pUniform1 == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glUniform1iARB");
                if (ptr == null)
                    Console.WriteLine("wtf");
                _pUniform1 = (Uniform1Delegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(Uniform1Delegate));
            }
            _pUniform1(location, v0);
        }

        private delegate int GetUniformLocationDelegate(uint program, string name);
        private GetUniformLocationDelegate _pGetUniformLocation;
        internal override int glGetUniformLocation(uint program, string name)
        {
            if (_pGetUniformLocation == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glGetUniformLocation");
                if (ptr == null)
                    Console.WriteLine("wtf");
                _pGetUniformLocation = (GetUniformLocationDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(GetUniformLocationDelegate));
            }
            return _pGetUniformLocation(program, name);
        }

        private delegate int LinkProgramDelegate(uint program);
        private LinkProgramDelegate _pLinkProgram;
        internal override void glLinkProgram(uint program)
        {
            if (_pLinkProgram == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glLinkProgramARB");
                if (ptr == null)
                    Console.WriteLine("wtf");
                _pLinkProgram = (LinkProgramDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(LinkProgramDelegate));
            }
            _pLinkProgram(program);
        }

        private delegate int UseProgramDelegate(uint program);
        private UseProgramDelegate _pUseProgram;
        internal override void glUseProgram(uint program)
        {
            if (_pUseProgram == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glUseProgramObjectARB");
                if (ptr == null)
                    Console.WriteLine("wtf");
                _pUseProgram = (UseProgramDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(UseProgramDelegate));
            }
            _pUseProgram(program);
        }

        private delegate int AttachShaderDelegate(uint program, uint shader);
        private AttachShaderDelegate _pAttachShader;
        internal override void glAttachShader(uint program, uint shader)
        {
            if (_pAttachShader == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glAttachObjectARB");
                if (ptr == null)
                    Console.WriteLine("wtf");
                _pAttachShader = (AttachShaderDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(AttachShaderDelegate));
            }
            _pAttachShader(program, shader);
        }

        private delegate void ShaderSourceDelegate(uint shader, int count, string[] @string, int* length);
        private ShaderSourceDelegate _pShaderSource;
        internal override void glShaderSource(uint shader, int count, string[] @string, int* length)
        {
            if (_pShaderSource == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glShaderSourceARB");
                if (ptr == null)
                    Console.WriteLine("wtf");
                _pShaderSource = (ShaderSourceDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(ShaderSourceDelegate));
            }
            _pShaderSource(shader, count, @string, length);
        }

        private delegate int CompileShaderDelegate(uint shader);
        private CompileShaderDelegate _pCompileShader;
        internal override void glCompileShader(uint shader)
        {
            if (_pCompileShader == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glCompileShader");
                if (ptr == null)
                    Console.WriteLine("wtf");
                _pCompileShader = (CompileShaderDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(CompileShaderDelegate));
            }
            _pCompileShader(shader);
        }

        private delegate void GenProgramsARBDelegate(int n, uint* programs);
        private GenProgramsARBDelegate _pGenProgramsARB;
        internal override void glGenProgramsARB(int n, uint* programs)
        {
            if (_pGenProgramsARB == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glGenProgramsARB");
                if (ptr == null)
                    Console.WriteLine("wtf");
                _pGenProgramsARB = (GenProgramsARBDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(GenProgramsARBDelegate));
            }
            _pGenProgramsARB(n, programs);
        }

        private delegate void BindProgramARBDelegate(uint target, int program);
        private BindProgramARBDelegate _pBindProgramARB;
        internal override void glBindProgramARB(AssemblyProgramTargetArb target, int program)
        {
            if (_pBindProgramARB == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glBindProgramARB");
                if (ptr == null)
                    Console.WriteLine("wtf");
                _pBindProgramARB = (BindProgramARBDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(BindProgramARBDelegate));
            }
            _pBindProgramARB((uint)target, program);
        }

        private delegate int CreateProgramDelegate();
        private CreateProgramDelegate _pCreateProgram;
        internal override int glCreateProgram()
        {
            if (_pCreateProgram == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glCreateProgram");
                if (ptr == null)
                    Console.WriteLine("wtf");
                _pCreateProgram = (CreateProgramDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(CreateProgramDelegate));
            }
            return _pCreateProgram();
        }

        private delegate int CreateShaderDelegate(uint type);
        private CreateShaderDelegate _pCreateShader;
        internal override int glCreateShader(ShaderType type)
        {
            if (_pCreateShader == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glCreateShader");
                if (ptr == null)
                    Console.WriteLine("wtf");
                _pCreateShader = (CreateShaderDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(CreateShaderDelegate));
            }
            return _pCreateShader((uint)type);
        }
        #endregion

        internal override void glCullFace(GLFace mode) { wGL.glCullFace(mode); }

        internal override void glDeleteLists(uint list, int range) { wGL.glDeleteLists(list, range); }

        internal override void glDeleteTextures(int num, uint* textures) { wGL.glDeleteTextures(num, textures); }

        internal override void glDepthFunc(GLFunction func) { wGL.glDepthFunc(func); }

        internal override void glDepthMask(bool flag)
        {
            throw new NotImplementedException();
        }

        internal override void glDepthRange(double near, double far)
        {
            throw new NotImplementedException();
        }

        internal override void glDisable(uint cap) { wGL.glDisable(cap); }
        internal override void glDisableClientState(GLArrayType cap) { wGL.glDisableClientState(cap); }

        internal override void glDrawArrays(GLPrimitiveType mode, int first, int count) { wGL.glDrawArrays(mode, first, count); }

        internal override void glDrawBuffer(uint mode)
        {
            throw new NotImplementedException();
        }

        internal override void glDrawElements(GLPrimitiveType mode, int count, GLElementType type, void* indices) { wGL.glDrawElements(mode, count, type, indices); }

        internal override void glDrawPixels(int width, int height, GLPixelDataFormat format, GLPixelDataType type, void* pixels) { wGL.glDrawPixels(width, height, format, type, pixels); }

        internal override void glEdgeFlag(bool flag)
        {
            throw new NotImplementedException();
        }

        internal override void glEdgeFlagPointer(int stride, bool* pointer)
        {
            throw new NotImplementedException();
        }

        internal override void glEdgeFlagv(bool* flag)
        {
            throw new NotImplementedException();
        }

        internal override void glEnable(GLEnableCap cap) { wGL.glEnable(cap); }
        internal override void glEnableClientState(GLArrayType cap) { wGL.glEnableClientState(cap); }

        internal override void glEnd() { wGL.glEnd(); }

        internal override void glEndList() { wGL.glEndList(); }

        internal override void glEvalCoord(double u)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord(float u)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord(double u, double v)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord(float u, float v)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord1(double* u)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord1(float* u)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord2(double* u)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord2(float* u)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalMesh(uint mode, int i1, int i2)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalMesh(uint mode, int i1, int i2, int j1, int j2)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalPoint(int i)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalPoint(int i, int j)
        {
            throw new NotImplementedException();
        }

        internal override void glFeedbackBuffer(int size, uint type, out float* buffer)
        {
            throw new NotImplementedException();
        }

        internal override void glFinish() { wGL.glFinish(); }

        internal override void glFlush() { wGL.glFlush(); }

        internal override void glFog(FogParameter pname, float param)
        {
            wGL.glFogf((uint)pname, param);
        }

        internal override void glFog(FogParameter pname, int param)
        {
            wGL.glFogi((uint)pname, param);
        }

        internal override void glFog(FogParameter pname, float* param)
        {
            wGL.glFogfv((uint)pname, param);
        }

        internal override void glFog(FogParameter pname, int* param)
        {
            wGL.glFogiv((uint)pname, param);
        }

        internal override void glFrontFace(GLFrontFaceDirection mode) { wGL.glFrontFace(mode); }

        internal override void glFrustum(double left, double right, double bottom, double top, double near, double far)
        {
            throw new NotImplementedException();
        }

        internal override uint glGenLists(int range) { return wGL.glGenLists(range); }

        internal override void glGenTextures(int num, uint* textures) { wGL.glGenTextures(num, textures); }

        internal override void glGet(GLGetMode pname, bool* param) { wGL.glGetBooleanv(pname, param); }
        internal override void glGet(GLGetMode pname, double* param) { wGL.glGetDoublev(pname, param); }
        internal override void glGet(GLGetMode pname, float* param) { wGL.glGetFloatv(pname, param); }
        internal override void glGet(GLGetMode pname, int* param) { wGL.glGetIntegerv(pname, param); }

        internal override void glGetClipPlane(uint plane, double* equation)
        {
            throw new NotImplementedException();
        }

        internal override GLErrorCode glGetError() { return wGL.glGetError(); }

        internal override void glGetLight(uint light, uint pname, float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetLight(uint light, uint pname, int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetMap(uint target, uint query, double* v)
        {
            throw new NotImplementedException();
        }

        internal override void glGetMap(uint target, uint query, float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glGetMap(uint target, uint query, int* v)
        {
            throw new NotImplementedException();
        }

        internal override void glGetMaterial(uint face, uint pname, float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetMaterial(uint face, uint pname, int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetPixelMap(uint map, float* values)
        {
            throw new NotImplementedException();
        }

        internal override void glGetPixelMap(uint map, uint* values)
        {
            throw new NotImplementedException();
        }

        internal override void glGetPixelMap(uint map, ushort* values)
        {
            throw new NotImplementedException();
        }

        internal override void glGetPointer(uint name, void* values)
        {
            throw new NotImplementedException();
        }

        internal override void glGetPolygonStipple(out byte* mask)
        {
            throw new NotImplementedException();
        }

        internal override byte* glGetString(uint name)
        {
            return wGL.glGetString(name);
        }

        internal override void glGetTexEnv(uint target, uint pname, out float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexEnv(uint target, uint pname, out int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexGen(uint coord, uint pname, out double* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexGen(uint coord, uint pname, out float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexGen(uint coord, uint pname, out int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexImage(uint target, uint format, uint type, out void* pixels)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexLevelParameter(uint target, int level, uint pname, out float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexLevelParameter(uint target, int level, uint pname, out int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexParameter(uint target, uint pname, out float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexParameter(uint target, uint pname, out int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glHint(GLHintTarget target, GLHintMode mode) { wGL.glHint(target, mode); }

        internal override void glIndex(double c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(float c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(int c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(short c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(double* c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(float* c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(int* c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(short* c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndexMask(uint mask)
        {
            throw new NotImplementedException();
        }

        internal override void glIndexPointer(uint type, int stride, void* ptr)
        {
            throw new NotImplementedException();
        }

        internal override void glInitNames()
        {
            throw new NotImplementedException();
        }

        internal override void glInterleavedArrays(uint format, int stride, void* pointer)
        {
            throw new NotImplementedException();
        }

        internal override bool glIsEnabled(uint cap)
        {
            throw new NotImplementedException();
        }

        internal override bool glIsList(uint list)
        {
            throw new NotImplementedException();
        }

        internal override bool glIsTexture(uint texture)
        {
            throw new NotImplementedException();
        }

        internal override bool glLight(GLLightTarget light, GLLightParameter pname, float param) { return wGL.glLightf(light, pname, param); }
        internal override bool glLight(GLLightTarget light, GLLightParameter pname, int param) { return wGL.glLighti(light, pname, param); }
        internal override bool glLight(GLLightTarget light, GLLightParameter pname, float* param) { return wGL.glLightfv(light, pname, param); }
        internal override bool glLight(GLLightTarget light, GLLightParameter pname, int* param) { return wGL.glLightiv(light, pname, param); }

        internal override void glLightModel(uint pname, float param)
        {
            throw new NotImplementedException();
        }

        internal override void glLightModel(uint pname, int param)
        {
            throw new NotImplementedException();
        }

        internal override void glLightModel(uint pname, float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glLightModel(uint pname, int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glLineStipple(int factor, ushort pattern) { wGL.glLineStipple(factor, pattern); }

        internal override void glLineWidth(float width) { wGL.glLineWidth(width); }

        internal override void glListBase(uint b) { wGL.glListBase(b); }

        internal override void glLoadIdentity() { wGL.glLoadIdentity(); }

        internal override void glLoadMatrix(double* m) { wGL.glLoadMatrixd(m); }

        internal override void glLoadMatrix(float* m) { wGL.glLoadMatrixf(m); }

        internal override void glLoadName(uint name) { wGL.glLoadName(name); }

        internal override void glLogicOp(uint opcode)
        {
            throw new NotImplementedException();
        }

        internal override void glMap(uint target, double u1, double u2, int stride, int order, double* points)
        {
            throw new NotImplementedException();
        }

        internal override void glMap(uint target, float u1, float u2, int stride, int order, float* points)
        {
            throw new NotImplementedException();
        }

        internal override void glMap(uint target, double u1, double u2, int ustride, int uorder, double v1, double v2, int vstride, int vorder, double* points)
        {
            throw new NotImplementedException();
        }

        internal override void glMap(uint target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder, float* points)
        {
            throw new NotImplementedException();
        }

        internal override void glMapGrid(int un, double u1, double u2)
        {
            throw new NotImplementedException();
        }

        internal override void glMapGrid(int un, float u1, float u2)
        {
            throw new NotImplementedException();
        }

        internal override void glMapGrid(int un, double u1, double u2, int vn, double v1, double v2)
        {
            throw new NotImplementedException();
        }

        internal override void glMapGrid(int un, float u1, float u2, int vn, float v1, float v2)
        {
            throw new NotImplementedException();
        }

        internal override void glMaterial(GLFace face, GLMaterialParameter pname, float param) { wGL.glMaterialf(face, pname, param); }
        internal override void glMaterial(GLFace face, GLMaterialParameter pname, int param) { wGL.glMateriali(face, pname, param); }
        internal override void glMaterial(GLFace face, GLMaterialParameter pname, float* param) { wGL.glMaterialfv(face, pname, param); }
        internal override void glMaterial(GLFace face, GLMaterialParameter pname, int* param) { wGL.glMaterialiv(face, pname, param); }

        internal override void glMatrixMode(GLMatrixMode mode) { wGL.glMatrixMode(mode); }

        private delegate void MultiTexCoord1sDelegate(GLMultiTextureTarget target, short s);
        private delegate void MultiTexCoord1iDelegate(GLMultiTextureTarget target, int s);
        private delegate void MultiTexCoord2fvDelegate(GLMultiTextureTarget target, float* v);

        private MultiTexCoord2fvDelegate _mtc2fv;

        //internal override void glMultiTexCoord(GLMultiTextureTarget target, short s) { wGL.glMultiTexCoord1s(target, s); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, int s){ wGL.glMultiTexCoord1i(target, s); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, float s){ wGL.glMultiTexCoord1f(target, s); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, double s){ wGL.glMultiTexCoord1d(target, s); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, short s, short t) { wGL.glMultiTexCoord2s(target, s, t); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, int s, int t) { wGL.glMultiTexCoord2i(target, s, t); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, float s, float t) { wGL.glMultiTexCoord2f(target, s, t); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, double s, double t) { wGL.glMultiTexCoord2d(target, s, t); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, short s, short t, short r) { wGL.glMultiTexCoord3s(target, s, t, r); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, int s, int t, int r) { wGL.glMultiTexCoord3i(target, s, t, r); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, float s, float t, float r) { wGL.glMultiTexCoord3f(target, s, t, r); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, double s, double t, double r) { wGL.glMultiTexCoord3d(target, s, t, r); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, short s, short t, short r, short q) { wGL.glMultiTexCoord4s(target, s, t, r, q); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, int s, int t, int r, int q) { wGL.glMultiTexCoord4i(target, s, t, r, q); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, float s, float t, float r, float q) { wGL.glMultiTexCoord4f(target, s, t, r, q); }
        //internal override void glMultiTexCoord(GLMultiTextureTarget target, double s, double t, double r, double q) { wGL.glMultiTexCoord4d(target, s, t, r, q); }
        //internal override void glMultiTexCoord1(GLMultiTextureTarget target, short* v) { wGL.glMultiTexCoord1sv(target, v); }
        //internal override void glMultiTexCoord1(GLMultiTextureTarget target, int* v) { wGL.glMultiTexCoord1iv(target, v); }
        //internal override void glMultiTexCoord1(GLMultiTextureTarget target, float* v) { wGL.glMultiTexCoord1fv(target, v); }
        //internal override void glMultiTexCoord1(GLMultiTextureTarget target, double* v) { wGL.glMultiTexCoord1dv(target, v); }
        //internal override void glMultiTexCoord2(GLMultiTextureTarget target, short* v) { wGL.glMultiTexCoord2sv(target, v); }
        //internal override void glMultiTexCoord2(GLMultiTextureTarget target, int* v) { wGL.glMultiTexCoord2iv(target, v); }
        internal override void glMultiTexCoord2(GLMultiTextureTarget target, float* v)
        {
            if (_mtc2fv == null)
            {
                VoidPtr ptr = wGL.wglGetProcAddress("glMultiTexCoord2fv");
                if (ptr == null)
                    ptr = wGL.wglGetProcAddress("glMultiTexCoord2fvARB");
                _mtc2fv = (MultiTexCoord2fvDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(MultiTexCoord2fvDelegate));
            }
            _mtc2fv(target, v);
        }
        //internal override void glMultiTexCoord2(GLMultiTextureTarget target, double* v) { wGL.glMultiTexCoord2dv(target, v); }
        //internal override void glMultiTexCoord3(GLMultiTextureTarget target, short* v) { wGL.glMultiTexCoord3sv(target, v); }
        //internal override void glMultiTexCoord3(GLMultiTextureTarget target, int* v) { wGL.glMultiTexCoord3iv(target, v); }
        //internal override void glMultiTexCoord3(GLMultiTextureTarget target, float* v) { wGL.glMultiTexCoord3fv(target, v); }
        //internal override void glMultiTexCoord3(GLMultiTextureTarget target, double* v) { wGL.glMultiTexCoord3dv(target, v); }
        //internal override void glMultiTexCoord4(GLMultiTextureTarget target, short* v) { wGL.glMultiTexCoord4sv(target, v); }
        //internal override void glMultiTexCoord4(GLMultiTextureTarget target, int* v) { wGL.glMultiTexCoord4iv(target, v); }
        //internal override void glMultiTexCoord4(GLMultiTextureTarget target, float* v) { wGL.glMultiTexCoord4fv(target, v); }
        //internal override void glMultiTexCoord4(GLMultiTextureTarget target, double* v) { wGL.glMultiTexCoord4dv(target, v); }

        internal override void glMultMatrix(double* m) { wGL.glMultMatrixd(m); }

        internal override void glMultMatrix(float* m) { wGL.glMultMatrixf(m); }

        internal override void glNewList(uint list, GLListMode mode) { wGL.glNewList(list, mode); }

        internal override void glNormal(sbyte nx, sbyte ny, sbyte nz) { wGL.glNormal3b(nx, ny, nz); }
        internal override void glNormal(double nx, double ny, double nz) { wGL.glNormal3d(nx, ny, nz); }
        internal override void glNormal(float nx, float ny, float nz) { wGL.glNormal3f(nx, ny, nz); }
        internal override void glNormal(int nx, int ny, int nz) { wGL.glNormal3i(nx, ny, nz); }
        internal override void glNormal(short nx, short ny, short nz) { wGL.glNormal3s(nx, ny, nz); }
        internal override void glNormal(sbyte* v) { wGL.glNormal3bv(v); }
        internal override void glNormal(double* v) { wGL.glNormal3dv(v); }
        internal override void glNormal(float* v) { wGL.glNormal3fv(v); }
        internal override void glNormal(int* v) { wGL.glNormal3iv(v); }
        internal override void glNormal(short* v) { wGL.glNormal3sv(v); }

        internal override void glNormalPointer(GLDataType type, int stride, void* pointer) { wGL.glNormalPointer(type, stride, pointer); }

        internal override void glOrtho(double left, double right, double bottom, double top, double near, double far) { wGL.glOrtho(left, right, bottom, top, near, far); }

        internal override void glPassThrough(float token)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelMap(uint map, int mapsize, float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelMap(uint map, int mapsize, uint* v)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelMap(uint map, int mapsize, ushort* v)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelStore(uint pname, float param)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelStore(uint pname, int param)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelTransfer(uint pname, float param)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelTransfer(uint pname, int param)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelZoom(float xfactor, float yfactor)
        {
            throw new NotImplementedException();
        }

        internal override void glPointSize(float size) { wGL.glPointSize(size); }

        internal override void glPolygonMode(GLFace face, GLPolygonMode mode) { wGL.glPolygonMode(face, mode); }

        internal override void glPolygonOffset(float factor, float units)
        {
            throw new NotImplementedException();
        }

        internal override void glPolygonStipple(byte* mask)
        {
            throw new NotImplementedException();
        }

        internal override void glPopAttrib(uint mask)
        {
            throw new NotImplementedException();
        }

        internal override void glPopClientAttrib(uint mask)
        {
            throw new NotImplementedException();
        }

        internal override void glPopMatrix() { wGL.glPopMatrix(); }

        internal override void glPopName()
        {
            throw new NotImplementedException();
        }

        internal override void glPrioritizeTextures(int num, uint* textures, float* priorities)
        {
            throw new NotImplementedException();
        }

        internal override void glPushAttrib(uint mask)
        {
            throw new NotImplementedException();
        }

        internal override void glPushClientAttrib(uint mask)
        {
            throw new NotImplementedException();
        }

        internal override void glPushMatrix() { wGL.glPushMatrix(); }

        internal override void glPushName(uint name)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(double x, double y)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(float x, float y)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(int x, int y)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(short x, short y)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(double x, double y, double z)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(int x, int y, int z)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(short x, short y, short z)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(double x, double y, double z, double w)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(float x, float y, float z, float w)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(int x, int y, int z, int w)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(short x, short y, short z, short w)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos2(double* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos2(float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos2(int* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos2(short* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos3(double* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos3(float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos3(int* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos3(short* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos4(double* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos4(float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos4(int* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos4(short* v)
        {
            throw new NotImplementedException();
        }

        internal override void glReadBuffer(uint mode)
        {
            throw new NotImplementedException();
        }

        internal override void glReadPixels(int x, int y, int width, int height, GLPixelDataFormat format, GLPixelDataType type, void* pixels) { wGL.glReadPixels(x, y, width, height, format, type, pixels); }

        internal override void glRect(double x1, double y1, double x2, double y2)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(float x1, float y1, float x2, float y2)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(int x1, int y1, int x2, int y2)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(short x1, short y1, short x2, short y2)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(double* v1, double* v2)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(float* v1, float* v2)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(int* v1, int* v2)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(short* v1, short* v2)
        {
            throw new NotImplementedException();
        }

        internal override int glRenderMode(uint mode)
        {
            throw new NotImplementedException();
        }

        internal override void glRotate(double angle, double x, double y, double z) { wGL.glRotated(angle, x, y, z); }

        internal override void glRotate(float angle, float x, float y, float z) { wGL.glRotatef(angle, x, y, z); }

        internal override void glScale(double x, double y, double z) { wGL.glScaled(x, y, z); }
        internal override void glScale(float x, float y, float z) { wGL.glScalef(x, y, z); }

        internal override void glScissor(int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        internal override void glSelectBuffer(int size, out uint* buffer)
        {
            throw new NotImplementedException();
        }

        internal override void glShadeModel(GLShadingModel mode) { wGL.glShadeModel(mode); }

        internal override void glStencilFunc(uint func, int refval, uint mask)
        {
            wGL.glStencilFunc(func, refval, mask);
        }

        internal override void glStencilMask(uint mask)
        {
            throw new NotImplementedException();
        }

        internal override void glStencilOp(uint fail, uint zfail, uint zpass)
        {
            wGL.glStencilOp(fail, zfail, zpass);
        }

        internal override void glTexCoord(double s) { wGL.glTexCoord1d(s); }
        internal override void glTexCoord(float s) { wGL.glTexCoord1f(s); }
        internal override void glTexCoord(int s) { wGL.glTexCoord1i(s); }
        internal override void glTexCoord(short s) { wGL.glTexCoord1s(s); }
        internal override void glTexCoord(double s, double t) { wGL.glTexCoord2d(s, t); }
        internal override void glTexCoord(float s, float t) { wGL.glTexCoord2f(s, t); }
        internal override void glTexCoord(int s, int t) { wGL.glTexCoord2i(s, t); }
        internal override void glTexCoord(short s, short t) { wGL.glTexCoord2s(s, t); }
        internal override void glTexCoord(double s, double t, double r) { wGL.glTexCoord3d(s, t, r); }
        internal override void glTexCoord(float s, float t, float r) { wGL.glTexCoord3f(s, t, r); }
        internal override void glTexCoord(int s, int t, int r) { wGL.glTexCoord3i(s, t, r); }
        internal override void glTexCoord(short s, short t, short r) { wGL.glTexCoord3s(s, t, r); }
        internal override void glTexCoord(double s, double t, double r, double q) { wGL.glTexCoord4d(s, t, r, q); }
        internal override void glTexCoord(float s, float t, float r, float q) { wGL.glTexCoord4f(s, t, r, q); }
        internal override void glTexCoord(int s, int t, int r, int q) { wGL.glTexCoord4i(s, t, r, q); }
        internal override void glTexCoord(short s, short t, short r, short q) { wGL.glTexCoord4s(s, t, r, q); }
        internal override void glTexCoord1(double* v) { wGL.glTexCoord1dv(v); }
        internal override void glTexCoord1(float* v) { wGL.glTexCoord1fv(v); }
        internal override void glTexCoord1(int* v) { wGL.glTexCoord1iv(v); }
        internal override void glTexCoord1(short* v) { wGL.glTexCoord1sv(v); }
        internal override void glTexCoord2(double* v) { wGL.glTexCoord2dv(v); }
        internal override void glTexCoord2(float* v) { wGL.glTexCoord2fv(v); }
        internal override void glTexCoord2(int* v) { wGL.glTexCoord2iv(v); }
        internal override void glTexCoord2(short* v) { wGL.glTexCoord2sv(v); }
        internal override void glTexCoord3(double* v) { wGL.glTexCoord3dv(v); }
        internal override void glTexCoord3(float* v) { wGL.glTexCoord3fv(v); }
        internal override void glTexCoord3(int* v) { wGL.glTexCoord3iv(v); }
        internal override void glTexCoord3(short* v) { wGL.glTexCoord3sv(v); }
        internal override void glTexCoord4(double* v) { wGL.glTexCoord4dv(v); }
        internal override void glTexCoord4(float* v) { wGL.glTexCoord4fv(v); }
        internal override void glTexCoord4(int* v) { wGL.glTexCoord4iv(v); }
        internal override void glTexCoord4(short* v) { wGL.glTexCoord4sv(v); }

        internal override void glTexCoordPointer(int size, GLDataType type, int stride, void* pointer) { wGL.glTexCoordPointer(size, type, stride, pointer); }

        internal override void glTexEnv(GLTexEnvTarget target, GLTexEnvParam pname, float param) { wGL.glTexEnvf(target, pname, param); }
        internal override void glTexEnv(GLTexEnvTarget target, GLTexEnvParam pname, int param) { wGL.glTexEnvi(target, pname, param); }
        internal override void glTexEnv(GLTexEnvTarget target, GLTexEnvParam pname, float* param) { wGL.glTexEnvfv(target, pname, param); }
        internal override void glTexEnv(GLTexEnvTarget target, GLTexEnvParam pname, int* param) { wGL.glTexEnviv(target, pname, param); }

        internal override void glTexGen(TextureCoordName coord, TextureGenParameter pname, double param) { wGL.glTexGend(coord, pname, param); }
        internal override void glTexGen(TextureCoordName coord, TextureGenParameter pname, float param) { wGL.glTexGenf(coord, pname, param); }
        internal override void glTexGen(TextureCoordName coord, TextureGenParameter pname, int param) { wGL.glTexGeni(coord, pname, param); }
        internal override void glTexGen(TextureCoordName coord, TextureGenParameter pname, double* param) { wGL.glTexGendv(coord, pname, param); }
        internal override void glTexGen(TextureCoordName coord, TextureGenParameter pname, float* param) { wGL.glTexGenfv(coord, pname, param); }
        internal override void glTexGen(TextureCoordName coord, TextureGenParameter pname, int* param) { wGL.glTexGeniv(coord, pname, param); }

        internal override void glTexImage1D(GLTexImageTarget target, int level, GLInternalPixelFormat internalFormat, int width, int border, GLPixelDataFormat format, GLPixelDataType type, void* pixels) { wGL.glTexImage1D(target, level, internalFormat, width, border, format, type, pixels); }

        internal override void glTexImage2D(GLTexImageTarget target, int level, GLInternalPixelFormat internalFormat, int width, int height, int border, GLPixelDataFormat format, GLPixelDataType type, void* pixels) { wGL.glTexImage2D(target, level, internalFormat, width, height, border, format, type, pixels); }

        internal override void glTexParameter(GLTextureTarget target, GLTextureParameter pname, float param) { wGL.glTexParameterf(target, pname, param); }
        internal override void glTexParameter(GLTextureTarget target, GLTextureParameter pname, int param) { wGL.glTexParameteri(target, pname, param); }
        internal override void glTexParameter(GLTextureTarget target, GLTextureParameter pname, float* param) { wGL.glTexParameterfv(target, pname, param); }
        internal override void glTexParameter(GLTextureTarget target, GLTextureParameter pname, int* param) { wGL.glTexParameteriv(target, pname, param); }

        internal override void glTexSubImage1D(GLTexImageTarget target, int level, int xOffset, int width, GLPixelDataFormat format, GLPixelDataType type, void* pixels)
        {
            wGL.glTexSubImage1D(target, level, xOffset, width, format, type, pixels);
        }

        internal override void glTexSubImage2D(GLTexImageTarget target, int level, int xOffset, int yOffset, int width, int height, GLPixelDataFormat format, GLPixelDataType type, void* pixels)
        {
            wGL.glTexSubImage2D(target, level, xOffset, yOffset, width, height, format, type, pixels);
        }

        internal override void glTranslate(double x, double y, double z) { wGL.glTranslated(x, y, z); }
        internal override void glTranslate(float x, float y, float z) { wGL.glTranslatef(x, y, z); }

        internal override void glVertex(double x, double y) { wGL.glVertex2d(x, y); }
        internal override void glVertex(float x, float y) { wGL.glVertex2f(x, y); }
        internal override void glVertex(int x, int y) { wGL.glVertex2i(x, y); }
        internal override void glVertex(short x, short y) { wGL.glVertex2s(x, y); }
        internal override void glVertex(double x, double y, double z) { wGL.glVertex3d(x, y, z); }
        internal override void glVertex(float x, float y, float z) { wGL.glVertex3f(x, y, z); }
        internal override void glVertex(int x, int y, int z) { wGL.glVertex3i(x, y, z); }
        internal override void glVertex(short x, short y, short z) { wGL.glVertex3s(x, y, z); }
        internal override void glVertex(double x, double y, double z, double w) { wGL.glVertex4d(x, y, z, w); }
        internal override void glVertex(float x, float y, float z, float w) { wGL.glVertex4f(x, y, z, w); }
        internal override void glVertex(int x, int y, int z, int w) { wGL.glVertex4i(x, y, z, w); }
        internal override void glVertex(short x, short y, short z, short w) { wGL.glVertex4s(x, y, z, w); }
        internal override void glVertex2v(double* v) { wGL.glVertex2dv(v); }
        internal override void glVertex2v(float* v) { wGL.glVertex2fv(v); }
        internal override void glVertex2v(int* v) { wGL.glVertex2iv(v); }
        internal override void glVertex2v(short* v) { wGL.glVertex2sv(v); }
        internal override void glVertex3v(double* v) { wGL.glVertex3dv(v); }
        internal override void glVertex3v(float* v) { wGL.glVertex3fv(v); }
        internal override void glVertex3v(int* v) { wGL.glVertex3iv(v); }
        internal override void glVertex3v(short* v) { wGL.glVertex3sv(v); }
        internal override void glVertex4v(double* v) { wGL.glVertex4dv(v); }
        internal override void glVertex4v(float* v) { wGL.glVertex4fv(v); }
        internal override void glVertex4v(int* v) { wGL.glVertex4iv(v); }
        internal override void glVertex4v(short* v) { wGL.glVertex4sv(v); }

        internal override void glVertexPointer(int size, GLDataType type, int stride, void* pointer) { wGL.glVertexPointer(size, type, stride, pointer); }

        internal override void glViewport(int x, int y, int width, int height) { wGL.glViewport(x, y, width, height); }

        internal override int gluBuild2DMipmaps(GLTextureTarget target, GLInternalPixelFormat internalFormat, int width, int height, GLPixelDataFormat format, GLPixelDataType type, void* data) { return wGL.gluBuild2DMipmaps(target, internalFormat, width, height, format, type, data); }
        internal override void gluDeleteQuadric(int quad) { wGL.gluDeleteQuadric(quad); }
        internal override int gluNewQuadric() { return wGL.gluNewQuadric(); }

        internal override void gluPerspective(double fovy, double aspect, double zNear, double zFar) { wGL.gluPerspective(fovy, aspect, zNear, zFar); }

        internal override void gluSphere(int quad, double radius, int slices, int stacks) { wGL.gluSphere(quad, radius, slices, stacks); }

        internal override void gluQuadricDrawStyle(int quad, GLUQuadricDrawStyle draw) { wGL.gluQuadricDrawStyle(quad, draw); }
        internal override void gluQuadricOrientation(int quad, GLUQuadricOrientation orientation) { wGL.gluQuadricOrientation(quad, orientation); }

        internal override void gluLookAt(double eyeX, double eyeY, double eyeZ, double centerX, double centerY, double centerZ, double upX, double upY, double upZ) { wGL.gluLookAt(eyeX, eyeY, eyeZ, centerX, centerY, centerZ, upX, upY, upZ); }
        internal override void gluUnProject(double winX, double winY, double winZ, double* model, double* proj, int* view, double* objX, double* objY, double* objZ) { wGL.gluUnProject(winX, winY, winZ, model, proj, view, objX, objY, objZ); }

    }
}
