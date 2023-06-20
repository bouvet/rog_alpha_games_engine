using System.Numerics;
using Newtonsoft.Json;

namespace GamesEngine.Math
{
    public interface IMatrix
    {
        IVector GetRotation();
        IVector GetScale();
        IVector GetPosition();
        void SetRotation(IVector rotation);
        void SetScale(IVector scale);
        void SetPosition(IVector position);
    }
    public class Matrix : IMatrix
    {
        Matrix4x4 _matrix = new();

        public Matrix()
        {
            SetScale(new Vector(1,1,1));
            SetPosition(new Vector(0,0,0));
            SetRotation(new Vector(0,0,0));
        }

        [JsonProperty]
        public float M11
        {
            get => _matrix.M11;
            set => _matrix.M11 = value;
        }

        [JsonProperty]
        public float M12
        {
            get => _matrix.M12;
            set => _matrix.M12 = value;
        }

        [JsonProperty]
        public float M13
        {
            get => _matrix.M13;
            set => _matrix.M13 = value;
        }

        [JsonProperty]
        public float M14
        {
            get => _matrix.M14;
            set => _matrix.M14 = value;
        }

        [JsonProperty]
        public float M21
        {
            get => _matrix.M21;
            set => _matrix.M21 = value;
        }

        [JsonProperty]
        public float M22
        {
            get => _matrix.M22;
            set => _matrix.M22 = value;
        }

        [JsonProperty]
        public float M23
        {
            get => _matrix.M23;
            set => _matrix.M23 = value;
        }

        [JsonProperty]
        public float M24
        {
            get => _matrix.M24;
            set => _matrix.M24 = value;
        }

        [JsonProperty]
        public float M31
        {
            get => _matrix.M31;
            set => _matrix.M31 = value;
        }

        [JsonProperty]
        public float M32
        {
            get => _matrix.M32;
            set => _matrix.M32 = value;
        }

        [JsonProperty]
        public float M33
        {
            get => _matrix.M33;
            set => _matrix.M33 = value;
        }

        [JsonProperty]
        public float M34
        {
            get => _matrix.M34;
            set => _matrix.M34 = value;
        }

        [JsonProperty]
        public float M41
        {
            get => _matrix.M41;
            set => _matrix.M41 = value;
        }

        [JsonProperty]
        public float M42
        {
            get => _matrix.M42;
            set => _matrix.M42 = value;
        }

        [JsonProperty]
        public float M43
        {
            get => _matrix.M43;
            set => _matrix.M43 = value;
        }

        [JsonProperty]
        public float M44
        {
            get => _matrix.M44;
            set => _matrix.M44 = value;
        }

        public IVector GetRotation()
        {
            return new Vector(_matrix.M11, _matrix.M12, _matrix.M13);
        }
        public IVector GetScale()
        {
            return new Vector(_matrix.M21, _matrix.M22, _matrix.M23);
        }
        public IVector GetPosition()
        {
            return new Vector(_matrix.M41, _matrix.M42, _matrix.M43);
        }
        public void SetRotation(IVector rotation)
        {
            Matrix4x4 tempMatrix = _matrix;
            tempMatrix.M11 = rotation.GetX();
            tempMatrix.M12 = rotation.GetY();
            tempMatrix.M13 = rotation.GetZ();
            _matrix = tempMatrix;
        }
        public void SetScale(IVector scale)
        {
            Matrix4x4 tempMatrix = _matrix;
            tempMatrix.M21 = scale.GetX();
            tempMatrix.M22 = scale.GetY();
            tempMatrix.M23 = scale.GetZ();
            _matrix = tempMatrix;
        }
        public void SetPosition(IVector position)
        {
            Matrix4x4 tempMatrix = _matrix;
            tempMatrix.M41 = position.GetX();
            tempMatrix.M42 = position.GetY();
            tempMatrix.M43 = position.GetZ();
            _matrix = tempMatrix;
        }
    }
}