using GamesEngine.Math;
using GamesEngine.Service.Game.Object;
using GamesEngine.Service.GameLoop;

namespace GamesEngine.Tests.Fakes.GameObjects;

public class MockDynamicObject : IDynamicGameObject
{
    public int Id { get; set; } = 1;
    public IMatrix WorldMatrix { get; set; } = new Matrix();
    public IMatrix LocalMatrix { get; set; } = new Matrix();
    public IGameObject? Parent { get; set; } = null;
    public List<IGameObject> Children { get; set; } = new List<IGameObject>();
    public void Render()
    {
        throw new NotImplementedException();
    }

    public void Collision(IGameObject? otherGameObject)
    {
        throw new NotImplementedException();
    }

    public IBounds? GetBounds()
    {
        throw new NotImplementedException();
    }

    public IVector Motion { get; set; }

    public void Update(IInterval deltaTime, ITime time)
    {
        throw new NotImplementedException();
    }

    public void UpdateMovement(IInterval deltaTime, ITime time)
    {
        throw new NotImplementedException();
    }
}