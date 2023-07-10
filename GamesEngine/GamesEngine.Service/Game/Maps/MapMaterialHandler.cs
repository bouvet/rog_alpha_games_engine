using GamesEngine.Service.Game.Object;
using Newtonsoft.Json;

namespace GamesEngine.Service.Game.Maps;

public interface IMapMaterialHandler
{
    public IGameObject? GetGameObject(IMapObject mapObject);
}

public class MapMaterialHandler : IMapMaterialHandler
{
    private List<IMapMaterial> MapMaterials = new();
    private Dictionary<string, Type> GameObjectTypes = new();

    public MapMaterialHandler()
    {
        var files = Directory.GetFiles("./GameData/Materials", "*.json");
        foreach (var file in files)
        {
            var material = JsonConvert.DeserializeObject<MapMaterial>(File.ReadAllText(file));
            if (material != null) MapMaterials.Add(material);
        }

        var types = FindTypes(typeof(IGameObject));
        foreach (var type in types)
        {
            if (Activator.CreateInstance(type) is IGameObject gameObject) GameObjectTypes.Add(gameObject.Type, type);
        }
    }

    private static List<Type> FindTypes(Type checkType)
    {
        var types = new List<Type>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        foreach (var type in assembly.GetTypes())
        {
            if (type is not { IsAbstract: false, IsInterface: false }) continue;

            if (type.GetInterfaces().Any(iface => iface == checkType))
            {
                if (type.GetConstructor(Type.EmptyTypes) != null)
                {
                    types.Add(type);
                }
            }
        }

        return types;
    }

    public IGameObject? GetGameObject(IMapObject mapObject)
    {
        IGameObject? gameObject = null;

        if (mapObject.Type == "Custom")
        {
            mapObject.Properties.TryGetValue("Material", out var materialName);

            if (materialName != null)
            {
                var material = MapMaterials.FirstOrDefault(x => x.Name == materialName);

                if (material != null)
                {
                    gameObject = mapObject.Static
                        ? new CustomStaticGameObject(material)
                        : new CustomDynamicGameObject(material);
                }
            }
        }
        else
        {
            if (GameObjectTypes.TryGetValue(mapObject.Type, out var type))
            {
                gameObject = Activator.CreateInstance(type) as IGameObject;
            }
        }

        return gameObject;
    }
}