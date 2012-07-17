using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.OpenGL
{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public class GLDisplayList
    {
        public uint _id;
        private GLContext _context;

        //public GLDisplayList(uint id) { _id = id; }
        public GLDisplayList(GLContext ctx)
        {
            _id = ctx.glGenLists(1);
            _context = ctx;
        }

        public void Begin() { _context.glNewList(_id, GLListMode.COMPILE); }
        public void Begin(GLListMode mode) { _context.glNewList(_id, mode); }
        public void End() { _context.glEndList(); }
        public void Call() { _context.glCallList(_id); }

        public void Delete()
        {
            if (_context != null)
            {
                _context.glDeleteLists(_id, 1);
                _id = 0;
                _context = null;
            }
        }
    }
}
