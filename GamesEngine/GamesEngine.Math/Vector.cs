using System.Numerics;
using Newtonsoft.Json;

namespace GamesEngine.Math
{
    public interface IVector
    {
        float GetAbsolute();
        float GetX();
        float GetY();
        float GetZ();

        IVector Add(IVector vector);
        IVector Subtract(IVector vector);
        IVector Multiply(IVector vector);
        IVector MultiplyWithScalar(int scalar);
        IVector Transform(Quaternion rotation);
        IVector Copy();
    }
    public class Vector : IVector
    {
        public IVector Transform(Quaternion rotation)
        {
            Vector3 vector3 = new Vector3(GetX(), GetY(), GetZ());
            Vector3 transformedVector = Vector3.Transform(vector3, rotation);
            IVector rotatedVector = new Vector(transformedVector.X, transformedVector.Y, transformedVector.Z);
            return rotatedVector;
        }

        public IVector Copy()
        {
            return new Vector(GetX(), GetY(), GetZ());
        }

        [JsonProperty]
        public float X
        {
            get => _vector.X;
            set => _vector.X = value;
        }

        [JsonProperty]
        public float Y
        {
            get => _vector.Y;
            set => _vector.Y = value;
        }

        [JsonProperty]
        public float Z
        {
            get => _vector.Z;
            set => _vector.Z = value;
        }

        private Vector3 _vector;
        public Vector(float x, float y, float z)
        {
            _vector = new Vector3(x, y, z);
        }

        public Vector()
        {
            _vector = new Vector3();
        }
        public float GetAbsolute()
        {
            return _vector.Length();
        }
        public float GetX()
        {
            return _vector.X;
        }
        public float GetY()
        {
            return _vector.Y;
        }
        public float GetZ()
        {
            return _vector.Z;
        }

        public IVector Add(IVector vector)
        {
            _vector.X += vector.GetX();
            _vector.Y += vector.GetY();
            _vector.Z += vector.GetZ();
            return this;
        }

        public IVector Subtract(IVector vector)
        {
            _vector.X -= vector.GetX();
            _vector.Y -= vector.GetY();
            _vector.Z -= vector.GetZ();
            return this;
        }

        public IVector Multiply(IVector vector)
        {
            _vector.X *= vector.GetX();
            _vector.Y *= vector.GetY();
            _vector.Z *= vector.GetZ();
            return this;
        }

        public IVector MultiplyWithScalar(int scalar)
        {
            return new Vector(GetX() * scalar, GetY() * scalar, GetZ() * scalar);
        }
    }
}