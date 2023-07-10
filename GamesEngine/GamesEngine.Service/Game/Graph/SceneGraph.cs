using GamesEngine.Service.Camera;
using GamesEngine.Service.Game.Object;
namespace GamesEngine.Service.Game.Graph
{
    public interface ISceneGraph
    {
        public IGraphBinTree<int, IDynamicGameObject?> DynamicGameObject { get; set; }
        public IGraphBinTree<int, IStaticGameObject?> StaticGameObject { get; set; }
        public IOctoTree<IGameObject> OctoTree { get; set; }
    }

    public class SceneGraph : ISceneGraph
    {
        public IGraphBinTree<int, IDynamicGameObject?> DynamicGameObject { get; set; } = new GraphBinTree<int, IDynamicGameObject?>();
        public IGraphBinTree<int, IStaticGameObject?> StaticGameObject { get; set; } = new GraphBinTree<int, IStaticGameObject?>();
        public IOctoTree<IGameObject> OctoTree { get; set; } = new OctoTree<IGameObject>();
    }
}