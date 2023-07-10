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

        void SetX(float x);
        void SetY(float y);
        void SetZ(float z);

        IVector Add(IVector vector);
        IVector Subtract(IVector vector);

        IVector Multiply(IVector vector);
        IVector MultiplyWithScalar(float scalar);

        IVector Divide(IVector vector);
        IVector DivideWithScalar(float scalar);

        IVector Normalize();

        double Distance(IVector vector);

        public static IVector operator +(IVector a, IVector b) => a.Add(b);
        public static IVector operator -(IVector a, IVector b) => a.Subtract(b);

        public static IVector operator *(IVector a, IVector b) => a.Multiply(b);
        public static IVector operator *(IVector a, int b) => a.MultiplyWithScalar(b);
        public static IVector operator *(IVector a, float b) => a.MultiplyWithScalar(b);
        public static IVector operator *(IVector a, double b) => a.MultiplyWithScalar((float)b);

        public static IVector operator /(IVector a, IVector b) => a.Divide(b);
        public static IVector operator /(IVector a, int b) => a.DivideWithScalar(b);
        public static IVector operator /(IVector a, float b) => a.DivideWithScalar(b);
        public static IVector operator /(IVector a, double b) => a.DivideWithScalar((float)b);

        public static IVector Cross(IVector a, IVector b)
        {
            return new Vector(
                a.GetY() * b.GetZ() - a.GetZ() * b.GetY(),
                a.GetZ() * b.GetX() - a.GetX() * b.GetZ(),
                a.GetX() * b.GetY() - a.GetY() * b.GetX());
        }

        public static float Dot(IVector a, IVector b)
        {
            return a.GetX() * b.GetX() + a.GetY() * b.GetY() + a.GetZ() * b.GetZ();
        }
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

        public IVector Normalize()
        {
            _vector = Vector3.Normalize(_vector);
            return this;
        }

        public double Distance(IVector vector)
        {
            return System.Math.Sqrt(System.Math.Pow(GetX() - vector.GetX(), 2) + System.Math.Pow(GetY() - vector.GetY(), 2) + System.Math.Pow(GetZ() - vector.GetZ(), 2));
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

        public void SetX(float x)
        {
            _vector.X = x;
        }

        public void SetY(float y)
        {
            _vector.Y = y;
        }

        public void SetZ(float z)
        {
            _vector.Z = z;
        }

        public IVector Add(IVector vector)
        {
            return new Vector(GetX() + vector.GetX(), GetY() + vector.GetY(), GetZ() + vector.GetZ());
        }

        public IVector Subtract(IVector vector)
        {
            return new Vector(GetX() - vector.GetX(), GetY() - vector.GetY(), GetZ() - vector.GetZ());
        }

        public IVector Multiply(IVector vector)
        {
            return new Vector(GetX() * vector.GetX(), GetY() * vector.GetY(), GetZ() * vector.GetZ());
        }

        public IVector Divide(IVector vector)
        {
            return new Vector(GetX() * vector.GetX(), GetY() * vector.GetY(), GetZ() * vector.GetZ());
        }

        public IVector MultiplyWithScalar(float scalar)
        {
            return new Vector(GetX() * scalar, GetY() * scalar, GetZ() * scalar);
        }

        public IVector DivideWithScalar(float scalar)
        {
            return new Vector(GetX() / scalar, GetY() / scalar, GetZ() / scalar);
        }

        public override string ToString()
        {
            return _vector.ToString();
        }

        public static IVector GetDirectionVector(IMatrix matrix)
        {
            return GetDirectionVector(matrix.GetRotation().GetX(), matrix.GetRotation().GetY());
        }

        public static IVector GetDirectionVector(float yaw, float pitch)
        {
            // Convert angles to radians
            float yawRad = yaw * (MathF.PI / 180.0f);
            float pitchRad = pitch * (MathF.PI / 180.0f);

            float X = MathF.Cos(yawRad) * MathF.Cos(pitchRad);
            float Y = MathF.Sin(pitchRad);
            float Z = MathF.Sin(yawRad) * MathF.Cos(pitchRad);

            // Normalize the direction vector.
            float length = MathF.Sqrt(X * X + Y * Y + Z * Z);
            X /= length;
            Y /= length;
            Z /= length;

            return new Vector(X, Y, Z);
        }
    }
}