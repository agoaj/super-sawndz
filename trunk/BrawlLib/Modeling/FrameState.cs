using System;
using System.Runtime.InteropServices;
using BrawlLib.Wii.Animations;

namespace BrawlLib.Modeling
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FrameState
    {
        public static readonly FrameState Neutral = new FrameState(new Vector3(1.0f), new Vector3(), new Vector3());

        internal Vector3 _scale;
        internal Vector3 _rotate;
        internal Vector3 _translate;

        internal float _unk;
        internal Quaternion _quaternion;
        
        public Vector3 Translate
        {
            get { return _translate; }
            set { _translate = value; CalcTransforms(); }
        }
        public Vector3 Rotate
        {
            get { return _rotate; }
            set { _rotate = value; CalcTransforms(); }
        }
        public Quaternion Quaternion
        {
            get { return _quaternion; }
            set { _quaternion = value; CalcQuatTransforms(); }
        }
        public Vector3 Scale
        {
            get { return _scale; }
            set { _scale = value; CalcTransforms(); }
        }

        internal Matrix _transform, _iTransform;
        internal Matrix _quatTransform, _quatiTransform;

        public FrameState(AnimationFrame frame)
        {
            _scale = frame.Scale;
            _rotate = frame.Rotation;
            _translate = frame.Translation;
            _quaternion = new Quaternion();
            _unk = 1;

            CalcTransforms();
        }
        public FrameState(Vector3 scale, Vector3 rotation, Vector3 translation)
        {
            _scale = scale;
            _rotate = rotation;
            _translate = translation;
            _quaternion = new Quaternion();
            _unk = 1;

            CalcTransforms();
        }
        public FrameState(Vector3 scale, Quaternion rotation, Vector3 translation)
        {
            _scale = scale;
            _translate = translation;
            _quaternion = rotation;
            _unk = 1;
            _rotate = new Vector3();
            
            CalcQuatTransforms();
        }
        public FrameState(Vector3 scale, Vector3 rotation, Quaternion quaternion, Vector3 translation)
        {
            _scale = scale;
            _rotate = rotation;
            _translate = translation;
            _quaternion = quaternion;
            _unk = 1;

            CalcTransforms();
            CalcQuatTransforms();
        }
        internal void CalcTransforms()
        {
            _transform = Matrix.TransformMatrix(_scale, _rotate, _translate);
            _iTransform = Matrix.ReverseTransformMatrix(_scale, _rotate, _translate);
        }

        internal void CalcQuatTransforms()
        {
            _quatTransform = Matrix.QuaternionTransformMatrix(_scale, _quaternion, _translate);
            _quatiTransform = Matrix.ReverseQuaternionTransformMatrix(_scale, _quaternion, _translate);
        }

        public static explicit operator AnimationFrame(FrameState state) { return new AnimationFrame(state._scale, state._rotate, state._translate); }
    }
}
