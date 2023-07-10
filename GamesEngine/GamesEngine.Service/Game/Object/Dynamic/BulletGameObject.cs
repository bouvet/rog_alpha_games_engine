using GamesEngine.Service.GameLoop;
using GamesEngine.Math;

namespace GamesEngine.Service.Game.Object
{

    public interface IBulletGameObject : IDynamicGameObject
    {
        public int Speed { get; set; }
    }

    public class BulletGameObject : DynamicGameObject, IBulletGameObject
    {
        public int Speed { get; set; } = 10;

        public override void Update(IInterval deltaTime, ITime time)
        {
            IVector updatePosition = WorldMatrix.GetRotation().MultiplyWithScalar((int)deltaTime.GetInterval() * Speed);
            WorldMatrix.SetPosition(WorldMatrix.GetPosition().Add(updatePosition));
        }

        public override void UpdateMovement(IInterval deltaTime, ITime time)
        {

        }
    }
}
