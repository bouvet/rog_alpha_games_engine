using GamesEngine.Math;

namespace GamesEngine.Service.Game.Object.StaticGameObjects;

public class FloorGameObject : IStaticGameObject
{
    public string Type => "Floor";
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
        return null;
    }
}