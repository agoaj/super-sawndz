using System;

namespace BrawlLib.OpenGL
{
    public unsafe class GLCamera
    {
        public Matrix _matrix;
        public Matrix _matrixInverse;

        public Vector3 _rotation;

        public GLCamera()
        {
            _matrix = _matrixInverse = Matrix.Identity;
        }

        public Vector3 GetPoint() { return _matrixInverse.Multiply(new Vector3()); }

        public void Translate(float x, float y, float z)
        {
            _matrix = Matrix.TranslationMatrix(-x, -y, -z) * _matrix;
            _matrixInverse.Translate(x, y, z);
        }
        public void Rotate(float x, float y)
        {
            //Grab vertex from matrix
            Vector3 point = _matrixInverse.Multiply(new Vector3());

            //Increment rotations
            _rotation._x += x;
            _rotation._y += y;

            //Reset matrices, using new rotations
            _matrix = Matrix.ReverseTransformMatrix(new Vector3(1.0f), _rotation, point);
            _matrixInverse = Matrix.TransformMatrix(new Vector3(1.0f), _rotation, point);
        }

        public void Pivot(float radius, float x, float y)
        {
            Translate(0, 0, -radius);
            Rotate(x, y);
            Translate(0, 0, radius);
        }

        public void Reset()
        {
            _matrix = _matrixInverse = Matrix.Identity;
            _rotation = new Vector3();
        }
    }
}
