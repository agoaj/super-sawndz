using System;
using System.Windows.Forms;
using System.Threading;

namespace BrawlLib.OpenGL
{
    public abstract unsafe class GLPanel : UserControl
    {
        internal protected GLContext _context;

        public bool _projectionChanged = true;
        private int _updateCounter;
        internal GLCamera _camera;

        public GLPanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            SetStyle(ControlStyles.ResizeRedraw, false);
        }
        protected override void Dispose(bool disposing)
        {
            DisposeContext();
            base.Dispose(disposing);
        }
        private void DisposeContext()
        {
            if (_context != null)
            {
                _context.Unbind();
                _context.Dispose();
                _context = null;
            }
        }

        public void BeginUpdate() { _updateCounter++; }
        public void EndUpdate() { if ((_updateCounter = Math.Max(_updateCounter - 1, 0)) == 0) Invalidate(); }

        protected override void OnLoad(EventArgs e)
        {
            _context = GLContext.Attach(this);

            _context.Capture();
            OnInit();
            _context.Release();

            base.OnLoad(e);
        }

        protected override void DestroyHandle()
        {
            DisposeContext();
            base.DestroyHandle();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            DisposeContext();
            base.OnHandleDestroyed(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e) { }

        public virtual float GetDepth(int x, int y)
        {
            float val;
            _context.Capture();
            _context.glReadPixels(x, Height - y, 1, 1, GLPixelDataFormat.DEPTH_COMPONENT, GLPixelDataType.FLOAT, &val);
            return val;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_updateCounter > 0)
                return;

            if (_context == null)
                base.OnPaint(e);
            else if (Monitor.TryEnter(_context))
            {
                try
                {
                    _context.Capture();

                    //Set projection
                    if (_projectionChanged)
                    {
                        OnResized();
                        _projectionChanged = false;
                    }

                    if (_camera != null)
                    {
                        fixed (Matrix* p = &_camera._matrix)
                        {
                            _context.glMatrixMode(GLMatrixMode.ModelView);
                            _context.glLoadMatrix((float*)p);
                        }
                    }

                    OnRender();
                    _context.glFinish();
                    _context.Swap();
                    _context.Release();
                }
                finally { Monitor.Exit(_context); }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            _projectionChanged = true;
            Invalidate();
        }

        //protected override void OnHandleDestroyed(EventArgs e)
        //{
        //    DisposeContext();
        //    base.OnHandleDestroyed(e);
        //}

        internal protected virtual void OnInit()
        {
            _context.glClearColor(1.0f, 1.0f, 1.0f, 0.0f);
            _context.glClearDepth(1.0f);
        }

        public float _fovY = 45.0f, _nearZ = 1.0f, _farZ = 20000.0f, _aspect;

        internal Matrix _projectionMatrix;
        internal Matrix _projectionInverse;

        public Vector3 UnProject(Vector3 point) { return UnProject(point._x, point._y, point._z); }
        public Vector3 UnProject(float x, float y, float z)
        {
            if (_camera == null)
                return new Vector3();

            Vector4 v;
            v._x = 2 * x / Width - 1;
            v._y = 2 * (Height - y) / Height - 1;
            v._z = 2 * z - 1;
            v._w = 1.0f;

            return (Vector3)(_camera._matrixInverse * _projectionInverse * v);
        }

        public Vector3 TraceZ(Vector3 point, float z)
        {
            if (_camera == null)
                return new Vector3();

            double a = point._z - z;
            //Perform trig functions, using camera for angles

            //Get angle, truncating to MOD 180
            //double angleX = _camera._rotation._y - ((int)(_camera._rotation._y / 180.0) * 180);

            double angleX = Math.IEEERemainder(-_camera._rotation._y, 180.0);
            if (angleX < -90.0f)
                angleX = -180.0f - angleX;
            if (angleX > 90.0f)
                angleX = 180.0f - angleX;

            double angleY = Math.IEEERemainder(_camera._rotation._x, 180.0);
            if (angleY < -90.0f)
                angleY = -180.0f - angleY;
            if (angleY > 90.0f)
                angleY = 180.0f - angleY;

            float lenX = (float)(Math.Tan(angleX * Math.PI / 180.0) * a);
            float lenY = (float)(Math.Tan(angleY * Math.PI / 180.0) * a);

            return new Vector3(point._x + lenX, point._y + lenY, z);
        }

        //Projects a ray at 'screenPoint' through sphere at 'center' with 'radius'.
        //If point does not intersect
        public Vector3 ProjectCameraSphere(Vector2 screenPoint, Vector3 center, float radius, bool clamp)
        {
            if (_camera == null)
                return new Vector3();

            Vector3 point;

            //Get ray points
            Vector4 v = new Vector4(2 * screenPoint._x / Width - 1, 2 * (Height - screenPoint._y) / Height - 1, -1.0f, 1.0f);
            Vector3 ray1 = (Vector3)(_camera._matrixInverse * _projectionInverse * v);
            v._z = 1.0f;
            Vector3 ray2 = (Vector3)(_camera._matrixInverse * _projectionInverse * v);

            if (!Maths.LineSphereIntersect(ray1, ray2, center, radius, out point))
            {
                //If no intersect is found, project the ray through the plane perpendicular to the camera.
                Maths.LinePlaneIntersect(ray1, ray2, center, _camera.GetPoint().Normalize(center), out point);

                //Clamp the point to edge of the sphere
                if (clamp)
                    point = Maths.PointAtLineDistance(center, point, radius);
            }

            return point;
        }


        protected void CalculateProjection()
        {
            _projectionMatrix = Matrix.ProjectionMatrix(_fovY, _aspect, _nearZ, _farZ);
            _projectionInverse = Matrix.ReverseProjectionMatrix(_fovY, _aspect, _nearZ, _farZ);
        }

        internal protected virtual void OnResized()
        {
            _aspect = (float)Width / Height;
            CalculateProjection();

            _context.glViewport(0, 0, Width, Height);
            _context.glMatrixMode(GLMatrixMode.Projection);
            fixed (Matrix* p = &_projectionMatrix)
                _context.glLoadMatrix((float*)p);
        }

        internal protected virtual void OnRender()
        {
            _context.glClear(GLClearMask.ColorBuffer | GLClearMask.DepthBuffer);
        }
    }
}
