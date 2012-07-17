using System;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;

namespace BrawlLib.OpenGL
{
    public class GLNode
    {
        //public Dictionary<GLBone, float> _boneRefs = new Dictionary<GLBone,float>();
        internal Matrix43 _matrix;
        //public int _index;

        //internal GLModel _model;
        internal GLBone _bone;
        internal Dictionary<GLBone, float> _nodeLinks = new Dictionary<GLBone, float>();


        //public GLNode(GLModel model)
        //{
        //    _model = model;
        //}

        //public unsafe GLNode(GLModel model, MDL0Node3Class def)
        //{
        //    _model = model;
        //    foreach (MDL0NodeType3Entry e in def._entries)
        //        _nodeLinks[model._nodes[e._id]] = e._value;
        //}


        internal unsafe void Rebuild()
        {
            if (_bone != null)
            {
                _matrix = _bone._transformMatrix;
            }
            else
            {
                _matrix = Matrix43.Identity;
                //_matrix = new Matrix43();
                //foreach (KeyValuePair<GLBone, float> pair in _nodeLinks)
                //{
                //    Matrix43 ifm = pair.Key._inverseFinalMatrix;
                //    Matrix43 fm = pair.Key._finalMatrix;
                //    fm.Multiply(&ifm);
                //    fm.Multiply(pair.Value);

                //    _matrix.Add(&fm);
                //}
            }
        }

        internal unsafe void Link(GLModel model, MDL0Node3Class def)
        {
            foreach (MDL0NodeType3Entry e in def._entries)
                _nodeLinks[model._nodes[e._id]._bone] = e._value;
        }
    }
}
