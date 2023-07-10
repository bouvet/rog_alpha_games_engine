using GamesEngine.Patterns.Query;
using GamesEngine.Service;
using GamesEngine.Service.Game.Object;
using System.Text.Json;
using Newtonsoft.Json;

namespace GamesEngine.Communication.Queries.Handlers;

public class FetchStaticObjectsHandler : IQueryHandler<FetchStaticObjectsQuery, IQueryCallback<string>>
{
    public void Handle(FetchStaticObjectsQuery query, IQueryCallback<string> callBack)
    {
        List<IStaticGameObject?> objects = GameHandler.GetGame(query.ConnectionId).SceneGraph.StaticGameObject.GetValues();
        string jsonString = JsonConvert.SerializeObject(objects, Formatting.Indented, new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()
        });
        callBack.OnSuccess(jsonString);
    }
}