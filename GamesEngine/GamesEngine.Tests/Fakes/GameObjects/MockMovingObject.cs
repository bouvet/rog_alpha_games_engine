using GamesEngine.Math;
using GamesEngine.Service.Game.Object;
using GamesEngine.Service.GameLoop;

namespace GamesEngine.Tests.Fakes.GameObjects;

public class MockMovingObject : IDynamicGameObject
{
    public MockMovingObject(Vector motion)
    {
        Motion = motion;
    }

    public int Id { get; set; }
    public IMatrix WorldMatrix { get; set; } = new Matrix();
    public IMatrix LocalMatrix { get; set; } = new Matrix();
    public IGameObject? Parent { get; set; }
    public List<IGameObject> Children { get; set; }
    public void Render()
    {
        throw new NotImplementedException();
    }

    public void Collision(IGameObject? otherGameObject) { }

    public IBounds? GetBounds()
    {
        return new Bounds(WorldMatrix, 1, 1, 1);
    }

    public IVector Motion { get; set; }

    public void Update(IInterval deltaTime, ITime time) { }

    public void UpdateMovement(IInterval deltaTime, ITime time)
    {
        float multiplier = deltaTime.GetInterval() / 1000f; //TODO Replace 1000f with update frequency (1000 = 1s)
        WorldMatrix.SetPosition(WorldMatrix.GetPosition() + Motion * multiplier);
    }
}