using System.Numerics;
using GamesEngine.Math;

namespace GamesEngine.Service.Game.Object
{
    public interface IGameObject
    {
        string Type => GetType().Name;
        int Id { get; set; }
        IMatrix WorldMatrix { get; set; }
        IMatrix LocalMatrix { get; set; }
        IGameObject? Parent { get; set; }
        List<IGameObject> Children { get; set; }
        public void Render();
        public void Collision(IGameObject? otherGameObject);
        public IBounds? GetBounds();
    }

    public class GameObject : IGameObject
    {
        public int Id { get; set; }
        public IMatrix WorldMatrix { get; set; }
        public IMatrix LocalMatrix { get; set; }
        public IGameObject? Parent { get; set; }
        public List<IGameObject> Children { get; set; }
        public void Collision(IGameObject? otherGameObject) { }

        public IBounds? GetBounds()
        {
            return null;
        }
        public virtual void Render()
        {
            throw new NotImplementedException();
        }

        public static IGameObject? CollisionCheck(IGame game, IGameObject? gameObject)
        {
            IBounds? bounds = gameObject?.GetBounds();
            if (bounds == null)
            {
                return null;
            }

            return CollisionCheck(game, gameObject, bounds);
        }

        public static IGameObject? CollisionCheck(IGame game, IGameObject? gameObject, IBounds? bounds, bool alertObjects = true)
        {
            List<IGameObject?> gameObjects = new List<IGameObject?>();
            gameObjects.AddRange(game.SceneGraph.StaticGameObject.GetValues());
            gameObjects.AddRange(game.SceneGraph.DynamicGameObject.GetValues());

            foreach (var staticOb in gameObjects
                         .Where(e => e.Id != gameObject.Id)
                         .Where(e => gameObject.Parent == null || e.Id != gameObject.Parent.Id)
                         .Where(e => e.GetBounds() != null)
                         .Where(e => e.WorldMatrix.GetPosition().Distance(gameObject.WorldMatrix.GetPosition()) < 10))
            {
                IBounds? staticBounds = staticOb.GetBounds();
                if (bounds.Intersects(staticBounds))
                {
                    if (alertObjects)
                    {
                        gameObject.Collision(staticOb);
                        staticOb.Collision(gameObject);
                    }

                    return staticOb;
                }
            }

            return null;
        }
    }
}