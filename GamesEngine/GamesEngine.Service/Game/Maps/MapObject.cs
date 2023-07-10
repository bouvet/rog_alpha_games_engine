using GamesEngine.Math;

namespace GamesEngine.Service.Game.Maps;

public interface IMapObject
{
    public string Type { get; set; }
    public Vector Position { get; set; }
    public Vector? Rotation { get; set; }
    public Vector? Scale { get; set; }
    public Dictionary<string, dynamic>? Properties { get; set; }
    public bool Static => true;
}

public class MapObject : IMapObject
{
    public string Type { get; set; }
    public Vector Position { get; set; }
    public Vector? Rotation { get; set; } = new (0, 0, 0);
    public Vector? Scale { get; set; } = new(1, 1, 1);
    public Dictionary<string, dynamic>? Properties { get; set; }
}