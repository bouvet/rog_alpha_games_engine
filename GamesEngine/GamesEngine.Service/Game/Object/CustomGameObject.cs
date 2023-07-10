using GamesEngine.Math;
using GamesEngine.Service.Game.Maps;
using GamesEngine.Service.GameLoop;

namespace GamesEngine.Service.Game.Object;

public interface ICustomGameObject : IGameObject
{
    public IMapMaterial MapMaterial { get; set; }
}

public abstract class CustomGameObject : ICustomGameObject, IGameObject
{
    public string Type => MapMaterial.Type ?? "Custom";

    public IMapMaterial MapMaterial { get; set; }
    public int Id { get; set; }
    public IMatrix WorldMatrix { get; set; } = new Matrix();
    public IMatrix LocalMatrix { get; set; } = new Matrix();
    public IGameObject? Parent { get; set; }
    public List<IGameObject> Children { get; set; }
    public void Render()
    {
        // throw new NotImplementedException();
    }

    public void Collision(IGameObject? otherGameObject)
    {
        // throw new NotImplementedException();
    }

    public IBounds? GetBounds()
    {
        if (MapMaterial.Bounds != null)
        {
            IMatrix matrix = WorldMatrix.Copy();
            if(MapMaterial.Bounds.Position != null) matrix.SetPosition(matrix.GetPosition() + MapMaterial.Bounds.Position);

            if (MapMaterial.Bounds.Size == null)
            {
                return new Bounds(matrix, WorldMatrix.GetScale().GetX(), WorldMatrix.GetScale().GetY(), WorldMatrix.GetScale().GetZ());
            }

            return new Bounds(matrix, MapMaterial.Bounds.Size.X * WorldMatrix.GetScale().GetX(), MapMaterial.Bounds.Size.Y * WorldMatrix.GetScale().GetY(), MapMaterial.Bounds.Size.Z * WorldMatrix.GetScale().GetZ());
        }
        return new Bounds(WorldMatrix, WorldMatrix.GetScale().GetX(), WorldMatrix.GetScale().GetY(), WorldMatrix.GetScale().GetZ());
    }

}

public class CustomStaticGameObject : CustomGameObject, IStaticGameObject
{
    public CustomStaticGameObject(IMapMaterial mapMaterial)
    {
        MapMaterial = mapMaterial;
    }
}

public class CustomDynamicGameObject : CustomGameObject, IDynamicGameObject
{
    public CustomDynamicGameObject(IMapMaterial mapMaterial)
    {
        MapMaterial = mapMaterial;
    }

    public IVector Motion { get; set; }
    public void Update(IInterval deltaTime, ITime time){ }
    public void UpdateMovement(IInterval deltaTime, ITime time) { }
}