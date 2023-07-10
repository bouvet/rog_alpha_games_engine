using GamesEngine.Service.Game.Maps;

namespace GamesEngine.Tests.Fakes;

public class MockMapsHandler : IMapsHandler
{
    public IMapMaterialHandler MapMaterialHandler { get; set; }
    private List<IGameMap> Maps { get; set; }

    public MockMapsHandler(IMapMaterialHandler mapMaterialHandler, List<IGameMap> maps)
    {
        MapMaterialHandler = mapMaterialHandler;
        Maps = maps;
    }

    public List<IGameMap> GetMaps()
    {
        return Maps;
    }

    public IGameMap GetMap(string mapName)
    {
        return Maps.FirstOrDefault(map => map.MapName == mapName);
    }

    public void AddMap(IGameMap map)
    {
        Maps.Add(map);
    }

    public void RemoveMap(string mapName)
    {
        Maps.Remove(Maps.FirstOrDefault(map => map.MapName == mapName));
    }
}