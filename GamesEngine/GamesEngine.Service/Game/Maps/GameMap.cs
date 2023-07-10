namespace GamesEngine.Service.Game.Maps;

public interface IGameMap
{
    public List<MapObject> Objects { get; set; }
    public string MapName { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class GameMap : IGameMap
{
    public List<MapObject> Objects { get; set; }
    public string MapName { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}