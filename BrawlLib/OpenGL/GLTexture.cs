using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using System.Drawing.Imaging;

namespace BrawlLib.OpenGL
{
    public class GLTexture
    {
        public string _name;
        public uint _texId;

        private bool _remake = true;
        private Bitmap[] _textures;

        //public unsafe GLTexture(GLModel gLModel, MDL0TextureNode tex)
        //{
        //    _name = tex.Name;
        //}

        public unsafe uint Initialize(GLContext context)
        {
            if (_remake)
            {
                ClearTexture(context);

                uint id = 0;
                context.glGenTextures(1, &id);
                _texId = id;

                context.glBindTexture(GLTextureTarget.Texture2D, id);

                context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MagFilter, (int)GLTextureFilter.LINEAR);
                context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MinFilter, (int)GLTextureFilter.NEAREST_MIPMAP_LINEAR);
                context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.BaseLevel, 0);
                context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MaxLevel, _textures.Length - 1);

                for (int i = 0; i < _textures.Length; i++)
                {
                    Bitmap bmp = _textures[i];
                    BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    context.glTexImage2D(GLTexImageTarget.Texture2D, i, (GLInternalPixelFormat)4, data.Width, data.Height, 0, GLPixelDataFormat.BGRA, GLPixelDataType.UNSIGNED_BYTE, (void*)data.Scan0);
                    bmp.UnlockBits(data);
                }

                _remake = false;
                ClearImages();
            }
            return _texId;
        }

        private void ClearImages()
        {
            if (_textures != null)
            {
                foreach (Bitmap bmp in _textures)
                    bmp.Dispose();
                _textures = null;
            }
        }
        private unsafe void ClearTexture(GLContext context)
        {
            if (_texId != 0)
            {
                uint id = _texId;
                context.glDeleteTextures(1, &id);
                _texId = 0;
            }
        }

        public void Unbind(GLContext context)
        {
            ClearImages();
            ClearTexture(context);
        }

        //internal void Attach(Bitmap bmp, string name)
        //{
        //    if (name.Equals(_name))
        //    {
        //        _bmp = bmp;
        //        _remake = true;
        //    }
        //}

        internal unsafe void Attach(TEX0Node tex)
        {
            ClearImages();

            _textures = new Bitmap[tex.LevelOfDetail];
            for (int i = 0; i < tex.LevelOfDetail; i++)
                _textures[i] = tex.GetImage(i);

            _remake = true;
        }
        internal GLContext _context;
        internal uint _id;
        internal int _width, _height;

        public uint Id { get { return _id; } }
        public int Width { get { return _width; } }
        public int Height { get { return _height; } }

        //public GLTexture() { }
        public unsafe GLTexture(GLContext ctx, int width, int height)
        {
            uint id;
            ctx.glGenTextures(1, &id);
            if ((_id = id) == 0)
                ctx.CheckErrors();
            _context = ctx;
            _width = width;
            _height = height;
        }

        public void Bind() { Bind(-1, -1); }
        public void Bind(int index, int program)
        {
            if (_context != null)
            {
                //if (program != -1)
                //{
                //    _context.glUniform1(_context.glGetUniformLocation((uint)program, "samp" + index), index);
                //    _context.glActiveTexture((GLMultiTextureTarget)((int)GLMultiTextureTarget.TEXTURE0 + index));
                //}
                _context.glBindTexture(GLTextureTarget.Texture2D, _id);
            }
        }

        public unsafe void Delete()
        {
            if ((_context != null) && (_id != 0))
            {
                uint id = _id;
                _context.glDeleteTextures(1, &id);
                _id = 0;
                _context = null;
            }
        }
    }
}
