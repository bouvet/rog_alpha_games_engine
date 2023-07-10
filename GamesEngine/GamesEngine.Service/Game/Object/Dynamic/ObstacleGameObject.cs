using GamesEngine.Math;
using GamesEngine.Service.GameLoop;

namespace GamesEngine.Service.Game.Object.StaticGameObjects;

public class ObstacleGameObject : IDynamicGameObject
{
    public string Type => "Obstacle";
    public int Id { get; set; }
    public IMatrix WorldMatrix { get; set; } = new Matrix();
    public IMatrix LocalMatrix { get; set; } = new Matrix();
    public IGameObject? Parent { get; set; }
    public List<IGameObject> Children { get; set; }

    public bool Colliding { get; set; }

    public void Render()
    {
        throw new NotImplementedException();
    }

    public void Collision(IGameObject? otherGameObject)
    {
        if(otherGameObject is PlayerGameObject)
        Colliding = true;
    }

    public IBounds? GetBounds()
    {
        return new Bounds(WorldMatrix, WorldMatrix.GetScale().GetX(), WorldMatrix.GetScale().GetY(), WorldMatrix.GetScale().GetZ());
    }

    public IVector Motion { get; set; }
    public void Update(IInterval deltaTime, ITime time)
    {
        Colliding = false;
    }

    public void UpdateMovement(IInterval deltaTime, ITime time)
    {
        throw new NotImplementedException();
    }
}