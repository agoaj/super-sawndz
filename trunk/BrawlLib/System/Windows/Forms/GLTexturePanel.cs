using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;
using BrawlLib.Imaging;

namespace System.Windows.Forms
{
    public class GLTexturePanel : GLPanel
    {
        private GLTexture _currentTexture;
        public GLTexture Texture
        {
            get { return _currentTexture; }
            set 
            {
                if (_currentTexture == value)
                    return;

                if (((_currentTexture = value) != null) && (_context != null))
                    _currentTexture._context.Share(_context);
            }
        }

        protected unsafe internal override void OnInit()
        {
            //Share lists with original context
            if (_currentTexture != null)
                _currentTexture._context.Share(_context);

            //Set caps
            _context.glEnable(GLEnableCap.Blend);
            _context.glEnable(GLEnableCap.Texture2D);
            _context.glDisable((int)GLEnableCap.DepthTest);
            _context.glBlendFunc(GLBlendFactor.SRC_ALPHA, GLBlendFactor.ONE_MINUS_SRC_ALPHA);

            OnResized();
        }

        protected internal unsafe override void OnRender()
        {
            GLTexture _bgTex = _context.FindOrCreate<GLTexture>("TexBG", CreateBG);
            _bgTex.Bind();

            //Draw BG
            float s = (float)Width / _bgTex.Width, t = (float)Height / _bgTex.Height;

            _context.glBegin(GLPrimitiveType.Quads);

            _context.glTexCoord(0.0f, 0.0f);
            _context.glVertex(0.0f, 0.0f);
            _context.glTexCoord(s, 0.0f);
            _context.glVertex(1.0, 0.0f);
            _context.glTexCoord(s, t);
            _context.glVertex(1.0, 1.0);
            _context.glTexCoord(0, t);
            _context.glVertex(0.0f, 1.0);

            _context.glEnd();

            //Draw texture
            if ((_currentTexture != null) && (_currentTexture._id != 0))
            {
                float tAspect = (float)_currentTexture.Width / _currentTexture.Height;
                float wAspect = (float)Width / Height;
                float* points = stackalloc float[8];

                if (tAspect > wAspect) //Texture is wider, use horizontal fit
                {
                    points[0] = points[6] = 0.0f;
                    points[2] = points[4] = 1.0f;

                    points[1] = points[3] = ((Height - ((float)Width / _currentTexture.Width * _currentTexture.Height))) / Height / 2.0f;
                    points[5] = points[7] = 1.0f - points[1];
                }
                else
                {
                    points[1] = points[3] = 0.0f;
                    points[5] = points[7] = 1.0f;

                    points[0] = points[6] = (Width - ((float)Height / _currentTexture.Height * _currentTexture.Width)) / Width / 2.0f;
                    points[2] = points[4] = 1.0f - points[0];
                }

                _context.glBindTexture(GLTextureTarget.Texture2D, _currentTexture._id);
                _context.glBegin(GLPrimitiveType.Quads);

                _context.glTexCoord(0.0f, 0.0f);
                _context.glVertex2v(&points[0]);
                _context.glTexCoord(1.0f, 0.0f);
                _context.glVertex2v(&points[2]);
                _context.glTexCoord(1.0f, 1.0f);
                _context.glVertex2v(&points[4]);
                _context.glTexCoord(0.0f, 1.0f);
                _context.glVertex2v(&points[6]);

                _context.glEnd();
            }
        }

        internal protected override void OnResized()
        {
            //Set up orthographic projection

            _context.glViewport(0, 0, Width, Height);

            _context.glMatrixMode(GLMatrixMode.Projection);
            _context.glLoadIdentity();
            _context.glOrtho(0.0, 1.0, 1.0, 0.0, -0.1, 1.0);
        }

        internal static unsafe GLTexture CreateBG(GLContext ctx)
        {
            GLTexture tex = new GLTexture(ctx, 16, 16);
            tex.Bind();

            //Create BG texture
            ABGRPixel left = new ABGRPixel(255, 192, 192, 192);
            ABGRPixel right = new ABGRPixel(255, 240, 240, 240);

            int* pixelData = stackalloc int[16 * 16];
            ABGRPixel* p = (ABGRPixel*)pixelData;

            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                    *p++ = ((x & 8) == (y & 8)) ? left : right;

            //ctx.glEnable(GLEnableCap.Texture2D);
            //ctx.glBindTexture(GLTextureTarget.Texture2D, _backTex._id);
            ctx.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.WrapS, (int)GLTextureWrapMode.REPEAT);
            ctx.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.WrapT, (int)GLTextureWrapMode.REPEAT);
            ctx.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MinFilter, (int)GLTextureFilter.NEAREST);
            ctx.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MagFilter, (int)GLTextureFilter.NEAREST);
            ctx.glTexImage2D(GLTexImageTarget.Texture2D, 0, GLInternalPixelFormat._4, 16, 16, 0, GLPixelDataFormat.RGBA, GLPixelDataType.UNSIGNED_BYTE, pixelData);

            return tex;
        }
    }
}
