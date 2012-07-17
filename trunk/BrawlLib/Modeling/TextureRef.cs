using System;
using System.Collections.Generic;
using BrawlLib.OpenGL;
using System.Drawing;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using BrawlLib.Imaging;
using System.Drawing.Imaging;

namespace BrawlLib.Modeling
{
    public class TextureRef
    {
        public string Name;
        public GLTexture Texture;
        public bool Reset;
        public bool Selected;
        public bool Enabled = true;
        public object Source;

        private GLContext _context;

        internal List<MDL0MaterialRefNode> _texRefs = new List<MDL0MaterialRefNode>();
        internal List<MDL0MaterialRefNode> _decRefs = new List<MDL0MaterialRefNode>();

        public bool IsTexture { get { return _texRefs.Count > 0; } }
        public bool IsDecal { get { return _decRefs.Count > 0; } }

        public TextureRef() { }
        public TextureRef(string name) 
        {
            Name = name;
            if (Name == "TShadow1")
                Enabled = false;
        }

        public override string ToString() { return Name; }

        internal unsafe void Prepare(GLContext ctx)
        {
            if (_context == null)
                _context = ctx;


            if (Texture != null)
                Texture.Bind();
            else
                Load();

            float* p = stackalloc float[4];
            p[0] = p[1] = p[2] = p[3] = 1.0f;
            if (Selected)
                p[0] = -1.0f;

            ctx.glLight(GLLightTarget.Light0, GLLightParameter.SPECULAR, p);
            ctx.glLight(GLLightTarget.Light0, GLLightParameter.DIFFUSE, p);
        }

        public void Reload()
        {
            if (_context == null)
                return;

            _context.Capture();
            Load();
        }

        private unsafe void Load()
        {
            if (_context == null)
                return;

            Source = null;

            if (Texture != null)
                Texture.Delete();
            Texture = new GLTexture(_context, 0, 0);
            Texture.Bind();

            //ctx._states[String.Format("{0}_TexRef", Name)] = Texture;

            Bitmap bmp = null;
            TEX0Node tNode = null;

            if (_context._states.ContainsKey("_Node_Refs"))
            {
                List<ResourceNode> nodes = _context._states["_Node_Refs"] as List<ResourceNode>;
                List<ResourceNode> searched = new List<ResourceNode>(nodes.Count);

                foreach (ResourceNode n in nodes)
                {
                    ResourceNode node = n.RootNode;
                    if (searched.Contains(node))
                        continue;
                    searched.Add(node);

                    //Search node itself first
                    if ((tNode = node.FindChild("Textures(NW4R)/" + Name, true) as TEX0Node) != null)
                    {
                        Source = tNode;
                        bmp = tNode.GetImage(0);
                    }
                    else
                    {
                        //Then search node directory
                        string path = node._origPath;
                        if (path != null)
                        {
                            DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(path));
                            foreach (FileInfo file in dir.GetFiles(Name + ".*"))
                            {
                                if (file.Name.EndsWith(".tga"))
                                {
                                    Source = file.FullName;
                                    bmp = TGA.FromFile(file.FullName);
                                    break;
                                }
                                else if (file.Name.EndsWith(".png") || file.Name.EndsWith(".tiff") || file.Name.EndsWith(".tif"))
                                {
                                    Source = file.FullName;
                                    bmp = (Bitmap)Bitmap.FromFile(file.FullName);
                                    break;
                                }
                            }
                        }
                    }
                    if (bmp != null)
                        break;
                }
                searched.Clear();

                if (bmp != null)
                {
                    int w = bmp.Width, h = bmp.Height, size = w * h;

                    Texture._width = w;
                    Texture._height = h;
                    _context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MagFilter, (int)GLTextureFilter.LINEAR);
                    _context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MinFilter, (int)GLTextureFilter.NEAREST_MIPMAP_LINEAR);
                    //_context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.BaseLevel, 0);

                    //if (tNode != null)
                    //    _context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MaxLevel, tNode.LevelOfDetail);
                    //else
                    //    _context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MaxLevel, 0);

                    BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    try
                    {
                        using (UnsafeBuffer buffer = new UnsafeBuffer(size << 2))
                        {
                            ARGBPixel* sPtr = (ARGBPixel*)data.Scan0;
                            ABGRPixel* dPtr = (ABGRPixel*)buffer.Address;

                            for (int i = 0; i < size; i++)
                                *dPtr++ = (ABGRPixel)(*sPtr++);

                            int res = _context.gluBuild2DMipmaps(GLTextureTarget.Texture2D, GLInternalPixelFormat._4, w, h, GLPixelDataFormat.RGBA, GLPixelDataType.UNSIGNED_BYTE, buffer.Address);
                            if (res != 0)
                            {
                            }
                        }
                    }
                    finally
                    {
                        bmp.UnlockBits(data);
                        bmp.Dispose();
                    }
                }
            }
        }

        public static int Compare(TextureRef t1, TextureRef t2)
        {
            return String.Compare(t1.Name, t2.Name, false);
        }

        internal void Bind(GLContext ctx)
        {
            Unbind();

            _context = ctx;

            Selected = false;
            Enabled = true;
        }
        internal void Unbind()
        {
            if (Texture != null) { Texture.Delete(); Texture = null; }
            _context = null;
        }
    }
}
