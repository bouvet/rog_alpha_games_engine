using GamesEngine.Patterns;
using GamesEngine.Patterns.Command;
using GamesEngine.Patterns.Query;
using GamesEngine.Service.Communication.Commands;
using Newtonsoft.Json;

namespace GamesEngine.Service.Communication.CommandHandlers;

public class PlayerStatusHandler : ICommandHandler<PlayerStatusCommand, ICommandCallback<string>>
{
    public void Handle(PlayerStatusCommand command, ICommandCallback<string> callback)
    {
        if (command.IsJoin)
        {
            GameHandler.OnPlayerConnect(command.ConnectionId, null);

            var playerId = GameHandler.GetClient(command.ConnectionId).PlayerGameObject.Id;
            GameHandler.Communication.SendToClient(command.ConnectionId, new Response("PlayerId", JsonConvert.SerializeObject(new Dictionary<string, object>
            {
                { "id", playerId }
            })));
        }
        else
        {
            GameHandler.OnPlayerDisconnect(command.ConnectionId);
        }
    }
}