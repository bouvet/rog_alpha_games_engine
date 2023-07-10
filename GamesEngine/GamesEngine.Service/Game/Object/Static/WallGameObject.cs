using GamesEngine.Math;

namespace GamesEngine.Service.Game.Object.StaticGameObjects;

public class WallGameObject : IStaticGameObject
{
    public virtual string Type => "Wall";
    public int Id { get; set; }
    public IMatrix WorldMatrix { get; set; } = new Matrix();
    public IMatrix LocalMatrix { get; set; } = new Matrix();
    public IGameObject? Parent { get; set; }
    public List<IGameObject> Children { get; set; }
    public void Render()
    {
        throw new NotImplementedException();
    }

    public void Collision(IGameObject? otherGameObject)
    {
    }

    public IBounds? GetBounds()
    {
        return new Bounds(WorldMatrix, WorldMatrix.GetScale().GetX(), WorldMatrix.GetScale().GetY(), WorldMatrix.GetScale().GetZ());
    }
}