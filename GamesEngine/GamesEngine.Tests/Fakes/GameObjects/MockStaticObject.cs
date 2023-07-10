using GamesEngine.Math;
using GamesEngine.Service.Game.Object;

namespace GamesEngine.Tests.Fakes.GameObjects;

public class MockStaticObject : IStaticGameObject
{
    public int Id { get; set; }
    public IMatrix WorldMatrix { get; set; } = new Matrix();
    public IMatrix LocalMatrix { get; set; } = new Matrix();
    public IGameObject? Parent { get; set; }
    public List<IGameObject> Children { get; set; }
    public bool Colliding { get; set; }

    public MockStaticObject(Vector position)
    {
        WorldMatrix.SetPosition(position);
    }

    public void Render()
    {
        throw new NotImplementedException();
    }

    public void Collision(IGameObject? otherGameObject)
    {
        Colliding = true;
    }

    public IBounds? GetBounds()
    {
        return new Bounds(WorldMatrix, 1, 1, 1);
    }
}