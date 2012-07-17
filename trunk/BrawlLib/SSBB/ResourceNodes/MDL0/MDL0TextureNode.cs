using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0TextureNode : MDL0EntryNode
    {
        //internal MDL0Texture* Header { get { return (MDL0Texture*)WorkingUncompressed.Address; } }

        [Category("Texture Data")]
        public int NumEntries
        {
            get
            {
                //if (Header != null) return Header->_numEntries;
                //else 
                return _references.Count;
            }
        }
        [Category("Texture Data")]
        public int DataLen
        {
            get
            {
                //if (Header != null) return Header->_numEntries * 8 + 4;
                //else
                return _references.Count * 8 + 4;
            }
        }

        //[Category("Texture Data")]
        //public MDL0TextureEntry[] Entries { get { return _entries; } }
        //internal MDL0TextureEntry[] _entries;

        protected override bool OnInitialize()
        {
            //_entries = new MDL0TextureEntry[NumEntries];
            //for (int i = 0; i < NumEntries; i++)
            //    _entries[i] = Header->Entries[i];

            return false;
        }

        public GLTexture Texture;
        public bool Reset;
        public bool Selected;
        public bool ObjOnly;
        public bool Enabled = true;
        public object Source;
        public bool Rendered = false;

        public GLContext _context;

        internal List<MDL0MaterialRefNode> _references = new List<MDL0MaterialRefNode>();

        public MDL0TextureNode() { }
        public MDL0TextureNode(string name) 
        {
            _name = name;
            if (Name == "TShadow1" || Name == "_env_map_tex_dummy")
                Enabled = false;
        }

        public PLT0Node palette = null;
        internal unsafe void Prepare(GLContext ctx, MDL0MaterialRefNode mRef)
        {
            if (_context == null)
                _context = ctx;

            if (palette == null && mRef.PaletteNode != null)
                palette = mRef.RootNode.FindChild("Palettes(NW4R)/" + mRef.Palette, true) as PLT0Node;

            try
            {
                if (Texture != null)
                    Texture.Bind(mRef.Index, (int)((MDL0MaterialNode)mRef.Parent).ShaderNode.programHandle);
                else
                    Load(mRef.Index, (int)((MDL0MaterialNode)mRef.Parent).ShaderNode.programHandle, palette);
            }
            catch { }

            int filter = 0;
            switch (mRef.MagFilter)
            {
                case MDL0MaterialRefNode.TextureMagFilter.Nearest:
                    filter = (int)GLTextureFilter.NEAREST; break;
                case MDL0MaterialRefNode.TextureMagFilter.Linear:
                    filter = (int)GLTextureFilter.LINEAR; break;
            }
            _context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MagFilter, filter);
            switch (mRef.MinFilter)
            {
                case MDL0MaterialRefNode.TextureMinFilter.Nearest:
                    filter = (int)GLTextureFilter.NEAREST; break;
                case MDL0MaterialRefNode.TextureMinFilter.Linear:
                    filter = (int)GLTextureFilter.LINEAR; break;
                case MDL0MaterialRefNode.TextureMinFilter.Nearest_Mipmap_Nearest:
                    filter = (int)GLTextureFilter.NEAREST_MIPMAP_NEAREST; break;
                case MDL0MaterialRefNode.TextureMinFilter.Nearest_Mipmap_Linear:
                    filter = (int)GLTextureFilter.NEAREST_MIPMAP_LINEAR; break;
                case MDL0MaterialRefNode.TextureMinFilter.Linear_Mipmap_Nearest:
                    filter = (int)GLTextureFilter.LINEAR_MIPMAP_NEAREST; break;
                case MDL0MaterialRefNode.TextureMinFilter.Linear_Mipmap_Linear:
                    filter = (int)GLTextureFilter.LINEAR_MIPMAP_LINEAR; break;
            }
            _context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MinFilter, filter);
            _context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.LODBias, mRef.LODBias);
            
            //_context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.BaseLevel, 0);

            float* p = stackalloc float[4];
            p[0] = p[1] = p[2] = p[3] = 1.0f;
            if (Selected && !ObjOnly)
                p[0] = -1.0f;

            _context.glLight(GLLightTarget.Light0, GLLightParameter.SPECULAR, p);
            _context.glLight(GLLightTarget.Light0, GLLightParameter.DIFFUSE, p);
        }

        public void GetSource()
        {
            Source = BRESNode.FindChild("Textures(NW4R)/" + Name, true) as TEX0Node;
        }

        //public void AddTexture(string name)
        //{
        //    _context.Capture();
        //    if (!PAT0Textures.ContainsKey(name))
        //    {
        //        TEX0Node texture = RootNode.FindChild("Textures(NW4R)/" + name, true) as TEX0Node;
        //        PAT0Textures[name] = new GLTexture(_context, 0, 0);
        //        Bitmap bmp = texture.GetImage(0);
        //        if (bmp != null)
        //        {
        //            int w = bmp.Width, h = bmp.Height, size = w * h;

        //            PAT0Textures[name]._width = w;
        //            PAT0Textures[name]._height = h;
        //            BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        //            try
        //            {
        //                using (UnsafeBuffer buffer = new UnsafeBuffer(size << 2))
        //                {
        //                    ARGBPixel* sPtr = (ARGBPixel*)data.Scan0;
        //                    ABGRPixel* dPtr = (ABGRPixel*)buffer.Address;

        //                    for (int i = 0; i < size; i++)
        //                        *dPtr++ = (ABGRPixel)(*sPtr++);

        //                    int res = _context.gluBuild2DMipmaps(GLTextureTarget.Texture2D, GLInternalPixelFormat._4, w, h, GLPixelDataFormat.RGBA, GLPixelDataType.UNSIGNED_BYTE, buffer.Address);
        //                    if (res != 0)
        //                    {
        //                    }
        //                }
        //            }
        //            finally
        //            {
        //                bmp.UnlockBits(data);
        //                bmp.Dispose();
        //            }
        //        }
        //        PAT0Textures[name].Bind();
        //    }
        //}

        public void Reload()
        {
            if (_context == null)
                return;

            _context.Capture();
            Load();
        }

        private unsafe void Load() { Load(-1, -1, null); }
        private unsafe void Load(int index, int program, PLT0Node palette)
        {
            if (_context == null)
                return;

            Source = null;

            if (Texture != null)
                Texture.Delete();
            Texture = new GLTexture(_context, 0, 0);
            Texture.Bind(index, program);

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
                        if (palette != null)
                            bmp = tNode.GetImage(0, palette);
                        else
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
                    //_context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MagFilter, (int)GLTextureFilter.LINEAR);
                    //_context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MinFilter, (int)GLTextureFilter.NEAREST_MIPMAP_LINEAR);
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

        public static int Compare(MDL0TextureNode t1, MDL0TextureNode t2)
        {
            return String.Compare(t1.Name, t2.Name, false);
        }

        internal override void Bind(GLContext ctx)
        {
            //Unbind(ctx);

            if (Name == "TShadow1" || Name == "_env_map_tex_dummy")
                Enabled = false;

            _context = ctx;

            Selected = false;
            //Enabled = true;
        }
        internal void Unbind()
        {
            if (Texture != null) { Texture.Delete(); Texture = null; }
            _context = null;
            Rendered = false;
        }
    }
}
