using GamesEngine.Service.GameLoop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GamesEngine.Math;
using GamesEngine.Service.Client;
using Newtonsoft.Json;
using Vector = GamesEngine.Math.Vector;

namespace GamesEngine.Service.Game.Object
{
    public interface IPlayerGameObject : IDynamicGameObject
    {
        [JsonIgnore]
        public IClient Client { get; }
        public IVector Motion { get; set; }
    }
    public class PlayerGameObject : DynamicGameObject, IPlayerGameObject
    {
        public string Type => "Player";

        [JsonIgnore]
        public IClient Client { get; }

        public IVector Motion { get; set; } = new Vector(0,0,0);
        public IMatrix WorldMatrix { get; set; } = new Matrix();
        public IMatrix LocalMatrix { get; set; } = new Matrix();
        public IGameObject? Parent { get; set; }
        public List<IGameObject> Children { get; set; }

        public PlayerGameObject(IClient client)
        {
            Client = client;
        }

        public override void Render()
        {
            throw new NotImplementedException();
        }

        public override void Update(IInterval deltaTime, ITime time)
        {
        }

        public IBounds? GetBounds()
        {
            return MakeBounds(WorldMatrix);
        }

        private IBounds? MakeBounds(IMatrix matrix)
        {
            IMatrix boundsMatrix = matrix.Copy();
            IVector direction = Vector.GetDirectionVector(boundsMatrix);
            //boundsMatrix.SetPosition(boundsMatrix.GetPosition() + (direction * new Vector(boundsMatrix.GetScale().GetX() / 2, 0, 0))); //Shift the bounding box to the center along the X axis
            return new Bounds(WorldMatrix, WorldMatrix.GetScale().GetX(),WorldMatrix.GetScale().GetY(),WorldMatrix.GetScale().GetZ());
        }

        private static readonly bool COLLISION = false;
        public override void UpdateMovement(IInterval deltaTime, ITime time)
        {
            float multiplier = deltaTime.GetInterval() / 100f;
            IVector moved = Motion * multiplier;
            IVector curPos = WorldMatrix.GetPosition();

            if (COLLISION)
            {
                var matrix = new Matrix();
                matrix.SetPosition(curPos + moved);
                matrix.SetRotation(WorldMatrix.GetRotation());
                IBounds? bounds = MakeBounds(matrix);

                IGameObject? collision = CollisionCheck(GameHandler.GetGame(Client.ConnectionId), this, bounds, false);

                if (collision != null)
                {
                    IVector collisionDir = collision.WorldMatrix.GetPosition() - WorldMatrix.GetPosition();
                    collisionDir = collisionDir.Normalize();

                    // Reduce the motion of cubeB in the direction of the collision
                    float collisionSpeed = IVector.Dot(collisionDir, moved);
                    moved -= collisionDir * collisionSpeed;
                }
            }

            var newMotion = Motion - moved;
            Motion = newMotion;
            WorldMatrix.SetPosition(curPos + moved);
        }

        public void Collision(IGameObject? otherGameObject)
        {
        }
    }
}
