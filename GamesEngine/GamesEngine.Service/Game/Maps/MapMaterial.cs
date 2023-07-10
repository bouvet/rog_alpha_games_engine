using System.Drawing;
using GamesEngine.Math;

namespace GamesEngine.Service.Game.Maps;

public interface IMapMaterial
{
    public string Name { get; set; }
    public MaterialColor? Color { get; set; }
    public string? Type { get; set; }
    public MaterialBounds? Bounds { get; set; }
}

public class MapMaterial : IMapMaterial
{
    public string Name { get; set; }
    public MaterialColor? Color { get; set; }
    public string? Type { get; set; }
    public MaterialBounds? Bounds { get; set; }
}

public class MaterialBounds
{
    public Vector? Position { get; set; } = new();
    public Vector? Size { get; set; } = new();
}

public class MaterialColor
{
    public int R { get; set; } = 0;
    public int G { get; set; } = 0;
    public int B { get; set; } = 0;
    public int A { get; set; } = 255;
}