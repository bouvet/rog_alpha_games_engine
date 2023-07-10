using GamesEngine.Service.Game.Maps;
using GamesEngine.Service.Game.Object;

namespace GamesEngine.Tests.Fakes;

public class MockMapMaterialHandler : IMapMaterialHandler
{
    private IGameObject? _gameObject;

    public MockMapMaterialHandler(IGameObject? gameObject)
    {
        _gameObject = gameObject;
    }

    public IGameObject? GetGameObject(IMapObject mapObject)
    {
        return _gameObject;
    }
}