using System.Numerics;
using g3;

namespace GamesEngine.Math
{
    using Line = Tuple<IVector, IVector>;

    public interface IBounds
    {
        IVector Position { get; }
        IVector Rotation { get; }
        public float Width { get; }
        public float Height { get; }
        public float Depth { get; }
        public bool Intersects(IBounds? bounds);
        public IVector GetIntersection(IBounds bounds);
        public bool Contains(IBounds bounds);
        public bool Contains(float x, float y, float z);
    }

    public class OrientedBounds : IBounds
    {
        public IVector Position { get; }
        public IVector Rotation { get; }
        private IVector EndPosition { get; }

        public float Width { get; }
        public float Height { get; }
        public float Depth { get; }

        private Line[] Edges;

        public OrientedBounds(IMatrix position, float width, float height, float depth)
        {
            Position = position.GetPosition();
            Rotation = position.GetRotation();
            Width = width;
            Height = height;
            Depth = depth;

            float rotX = Rotation.GetX();
            float rotY = Rotation.GetY();
            float rotZ = Rotation.GetZ();

            IVector direction = Vector.GetDirectionVector(rotX, rotY);
            EndPosition = new Vector(Position.GetX() + direction.GetX() * Width, Position.GetY() + direction.GetY() * Height, Position.GetZ() + direction.GetZ() * Depth);

            Width = System.Math.Abs((EndPosition - Position).GetX());
            Height = System.Math.Abs((EndPosition - Position).GetY());
            Depth = System.Math.Abs((EndPosition - Position).GetZ());

            Edges = GetEdges();
        }

        private Line[] GetEdges()
        {
            Vector[] vertices = {
                new (Position.GetX(), Position.GetY(), Position.GetZ()),
                new (Position.GetX(), Position.GetY(), EndPosition.GetZ()),
                new (Position.GetX(), EndPosition.GetY(), Position.GetZ()),
                new (Position.GetX(), EndPosition.GetY(), EndPosition.GetZ()),
                new (EndPosition.GetX(), Position.GetY(), Position.GetZ()),
                new (EndPosition.GetX(), Position.GetY(), EndPosition.GetZ()),
                new (EndPosition.GetX(), EndPosition.GetY(), Position.GetZ()),
                new (EndPosition.GetX(), EndPosition.GetY(), EndPosition.GetZ())
            };

            Line[] edges = {
                new (vertices[0], vertices[1]),
                new (vertices[0], vertices[2]),
                new (vertices[0], vertices[4]),
                new (vertices[1], vertices[3]),
                new (vertices[1], vertices[5]),
                new (vertices[2], vertices[3]),
                new (vertices[2], vertices[6]),
                new (vertices[3], vertices[7]),
                new (vertices[4], vertices[5]),
                new (vertices[4], vertices[6]),
                new (vertices[5], vertices[7]),
                new (vertices[6], vertices[7])
            };

            return edges;
        }

        public IVector GetIntersection(IBounds bounds)
        {
            if (bounds is OrientedBounds bs)
            {
                Line[] edges1 = Edges;
                Line[] edges2 = bs.Edges;

                foreach (var edge1 in edges1)
                {
                    foreach (var edge2 in edges2)
                    {
                        if (LineIntersects(edge1, edge2))
                        {
                            return IntersectionPoint(edge1, edge2);
                        }
                    }
                }
            }
            return null;
        }

        public static IVector? IntersectionPoint(Line line1, Line line2)
        {
            return null;
        }

        private bool LineIntersects(Line line1, Line line2)
        {
            IVector v1 = line1.Item2 - line1.Item1;
            IVector v2 = line2.Item2 - line2.Item1;
            IVector v3 = line2.Item1 - line1.Item1;

            IVector cross = IVector.Cross(v1, v2);

            if (cross.GetX() == 0 && cross.GetY() == 0 && cross.GetZ() == 0)
                return false; // Lines are parallel

            float dot = IVector.Dot(v3, cross);
            if (System.Math.Abs(dot) > float.Epsilon)
                return false; // Lines are skew and do not intersect

            IVector cross1 = IVector.Cross(v3, v1);
            IVector cross2 = IVector.Cross(v3, v2);

            float dotCross = IVector.Dot(cross1, cross2);
            if (dotCross < 0)
                return false; // Lines do not intersect

            return true; // Lines do intersect
        }

        public bool Intersects(IBounds? bounds)
        {
            if (bounds is OrientedBounds bs)
            {
                Line[] edges1 = Edges;
                Line[] edges2 = bs.Edges;

                foreach (var edge1 in edges1)
                {
                    foreach (var edge2 in edges2)
                    {
                        if (LineIntersects(edge1, edge2))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;

        }

        public bool Contains(IBounds bounds)
        {
            return false;
        }

        public bool Contains(float x, float y, float z)
        {
            return false;
        }
    }

    public class Bounds : IBounds
    {
        public IVector Position { get; }
        public IVector Rotation { get; }
        private IVector EndPosition { get; }

        private AxisAlignedBox3d box;

        public float Width { get; }
        public float Height { get; }
        public float Depth { get; }

        public Bounds(IMatrix position, float width, float height, float depth)
        {
            Position = position.GetPosition();
            Rotation = position.GetRotation();
            Width = width;
            Height = height;
            Depth = depth;

            Vector3 rotation = new Vector3(
                DegreesToRadians(Rotation.GetX()),
                DegreesToRadians(Rotation.GetY()),
                DegreesToRadians(Rotation.GetZ())
            );

            Vector3 dimensions = new Vector3(width, height, depth);
            Vector3 pivot = new Vector3(dimensions.X / 2, dimensions.Y/2, dimensions.Z / 2);

            Vector3[] corners = {
                new (0, 0, 0),
                new (dimensions.X, 0, 0),
                new (0, dimensions.Y, 0),
                new (0, 0, dimensions.Z),
                new (dimensions.X, dimensions.Y, 0),
                new (dimensions.X, 0, dimensions.Z),
                new (0, dimensions.Y, dimensions.Z),
                new (dimensions.X, dimensions.Y, dimensions.Z)
            };

            corners = Array.ConvertAll(corners, corner => Vector3.Subtract(corner, pivot));

            Vector3 start = new Vector3(Position.GetX(), Position.GetY(), Position.GetZ());
            Matrix4x4 matrix = MakeRotationFromEuler(rotation);
            Vector3 endRelative = Vector3.Transform(dimensions, matrix);
            Vector3 end = Vector3.Add(start, endRelative);
            Vector3[] worldCorners = Array.ConvertAll(corners, corner => Vector3.Add(Vector3.Transform(corner, matrix), start));
            Vector3 min = worldCorners[0];
            Vector3 max = worldCorners[0];

            foreach (Vector3 corner in worldCorners)
            {
                min = Vector3.Min(min, corner);
                max = Vector3.Max(max, corner);
            }

            start = min;
            end = max;

            box = new AxisAlignedBox3d(new Vector3d(start.X, start.Y, start.Z), new Vector3d(end.X, end.Y, end.Z));
        }

        public static float DegreesToRadians(float degrees)
        {
            return degrees * (System.MathF.PI / 180);
        }


        public static Matrix4x4 MakeRotationFromEuler(Vector3 euler)
        {
            float cosPitch = (float)System.Math.Cos(euler.X);
            float sinPitch = (float)System.Math.Sin(euler.X);
            float cosYaw = (float)System.Math.Cos(euler.Y);
            float sinYaw = (float)System.Math.Sin(euler.Y);
            float cosRoll = (float)System.Math.Cos(euler.Z);
            float sinRoll = (float)System.Math.Sin(euler.Z);

            Vector3 xaxis = new Vector3(
                cosYaw * cosRoll,
                sinPitch * sinYaw * cosRoll + cosPitch * sinRoll,
                -cosPitch * sinYaw * cosRoll + sinPitch * sinRoll
            );

            Vector3 yaxis = new Vector3(
                -cosYaw * sinRoll,
                -sinPitch * sinYaw * sinRoll + cosPitch * cosRoll,
                cosPitch * sinYaw * sinRoll + sinPitch * cosRoll
            );

            Vector3 zaxis = new Vector3(
                sinYaw,
                -sinPitch * cosYaw,
                cosPitch * cosYaw
            );

            Matrix4x4 rotation = new Matrix4x4(
                xaxis.X, yaxis.X, zaxis.X, 0,
                xaxis.Y, yaxis.Y, zaxis.Y, 0,
                xaxis.Z, yaxis.Z, zaxis.Z, 0,
                0, 0, 0, 1
            );

            return rotation;
        }


        public IVector GetIntersection(IBounds bounds)
        {
            return null;
        }

        public bool Intersects(IBounds? bounds)
        {
            if (bounds is Bounds bs)
            {
                return bs.box.Intersects(box);
            }

            return false;
        }

        public bool Contains(IBounds bounds)
        {
            if (bounds is Bounds bs)
            {
                return bs.box.Contains(box);
            }

            return false;
        }

        public bool Contains(float x, float y, float z)
        {
            return box.Contains(new Vector3d(x,y,z));
        }
    }
}