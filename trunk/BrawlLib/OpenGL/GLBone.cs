using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Models;

namespace BrawlLib.OpenGL
{
    public class GLBone
    {
        public GLBone _parent;
        public List<GLBone> _children = new List<GLBone>();
        public List<GLPolygon> _polygons = new List<GLPolygon>();

        public int _nodeId;
        public int _index;

        public Vector3 _rotation, _translation, _scale;
        public bool _enabled = true;
        public Matrix43 _nodeMatrix, _inverseMatrix, _finalMatrix, _inverseFinalMatrix;

        public Matrix43 _transformMatrix, _transformInverted;

        //public unsafe virtual void Render(GLContext ctx)
        //{
        //    ctx.glPushMatrix();

        //    ctx.glTranslate(_translation._x, _translation._y, _translation._z);

        //    ctx.glRotate(_rotation._x, 1.0f, 0.0f, 0.0f);
        //    ctx.glRotate(_rotation._y, 0.0f, 1.0f, 0.0f);
        //    ctx.glRotate(_rotation._z, 0.0f, 0.0f, 1.0f);

        //    ctx.glScale(_scale._x, _scale._y, _scale._z);

        //    Matrix43 m;
        //    ctx.glGet(GLGetMode.MODELVIEW_MATRIX, (float*)&m);
        //    _matrix = m;

        //    foreach (GLBone b in _children)
        //        b.Render(ctx);

        //    ctx.glPopMatrix();

        //    //ctx.glBegin(GLPrimitiveType.Lines);

        //    //Vector3 v = new Vector3();
        //    //Vector3 v2 = _translation;
        //    //ctx.glVertex3v((float*)&v);
        //    //ctx.glVertex3v((float*)&v2);

        //    //ctx.glEnd();

        //    //ctx.glPushMatrix();

        //    //Matrix m = _transformMatrix;
        //    //ctx.glMultMatrix(m.Data);

        //    //ctx.glTranslate(_translation._x, _translation._y, _translation._z);

        //    //ctx.glRotate(_rotation._x, 1.0f, 0.0f, 0.0f);
        //    //ctx.glRotate(_rotation._y, 0.0f, 1.0f, 0.0f);
        //    //ctx.glRotate(_rotation._z, 0.0f, 0.0f, 1.0f);

        //    //ctx.glScale(_scale._x, _scale._y, _scale._z);

        //    //Matrix m = _transformInverted;
        //    //ctx.glGet(0x0BA6, m.Data);

        //    //ctx.glLoadMatrix(m.Data);

        //    //foreach (GLPolygon poly in _polygons)
        //    //    poly.Render(ctx, _transformInverted);

        //    //ctx.glScale(_scale._x, _scale._y, _scale._z);
        //    //ctx.glTranslate(_translation._x, _translation._y, _translation._z);
        //    //ctx.glRotate(_rotAngle, _rotation._x, _rotation._y, _rotation._z);


        //    //foreach (GLBone b in _children)
        //    //    b.Render(ctx);

        //    //ctx.glPopMatrix();

        //}


        public unsafe GLBone(MDL0BoneNode node)
        {
            _translation = node.Translation;
            _rotation = node.Rotation;
            _scale = node.Scale;
            _nodeId = node.NodeId;
            _index = node.BoneIndex;

            _transformMatrix = (Matrix43)node._frameMatrix;
            _transformInverted = (Matrix43)node._inverseFrameMatrix;
        }

        internal unsafe void Rebuild()
        {
            Matrix43 final, inverseFinal, node, inverse;

            if (_parent == null)
            {
                final = inverseFinal = Matrix43.Identity;
            }
            else
            {
                final = _parent._finalMatrix;
                inverseFinal = _parent._inverseFinalMatrix;
            }

            node = Matrix43.TransformationMatrix(_scale, _rotation, _translation);
            inverse = Matrix43.ReverseTransformMatrix(_scale, _rotation, _translation);

            final.Multiply(&node);
            inverseFinal.Multiply(&inverse);

            _nodeMatrix = node;
            _finalMatrix = final;
            _inverseMatrix = inverse;
            _inverseFinalMatrix = inverseFinal;

            foreach (GLBone bone in _children)
                bone.Rebuild();

        }
    }
}
