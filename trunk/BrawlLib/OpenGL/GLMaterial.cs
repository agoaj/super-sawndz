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
    public unsafe class GLMaterial
    {
        public List<GLTextureRef> _textureRefs = new List<GLTextureRef>();
        public GLModel _model;

        public GLMaterial(GLModel model, MDL0MaterialNode mat)
        {
            _model = model;

            foreach (MDL0MaterialRefNode r in mat.Children)
                _textureRefs.Add(new GLTextureRef(this, r));
        }

        public void Bind(GLContext context, uint[] texIds)
        {
            for (int i = 0; i < _textureRefs.Count; i++)
            {
                texIds[i] = _textureRefs[i].Initialize(context);
            }
        }
    }

    //public unsafe class GLTexture
    //{
        
    //}

    public unsafe class GLTextureRef
    {
        public GLTexture _tex;
        public string _name;

        public GLTextureRef(GLMaterial mat, MDL0MaterialRefNode texRef)
        {
            _name = texRef.Name;
            foreach (GLTexture tex in mat._model._textures)
            {
                if (tex._name.Equals(_name))
                {
                    _tex = tex;
                    break;
                }
            }
        }

        public uint Initialize(GLContext context)
        {
            return _tex != null ? _tex.Initialize(context) : 0;
        }
    }
}
