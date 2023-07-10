using System.Text.Json;
using GamesEngine.Patterns.Query;
using GamesEngine.Service.Game;
using GamesEngine.Service.Game.Object;

namespace GamesEngine.Communication.Queries.Handlers
{

    public class FindGameObjectQueryHandler : IQueryHandler<FindGameObjectQuery, IQueryCallback<string>>
    {
        private readonly IGame _game;

        public FindGameObjectQueryHandler(IGame game)
        {
            _game = game;
        }

        public void Handle(FindGameObjectQuery query, IQueryCallback<string> callback)
        {
            IGameObject? gameObject = _game.FindGameObject(query.GameObjectId);

            if (gameObject != null)
            {
                callback.OnSuccess(JsonSerializer.Serialize(gameObject));
            }
            else
            {
                callback.OnFailure();
            }
        }
    }

}
