using GamesEngine.Patterns.Command;
using GamesEngine.Patterns.Query;
using GamesEngine.Service.Communication.Commands;
using GamesEngine.Service.Game.Object;
using GamesEngine.Service.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesEngine.Service.Communication.CommandHandlers
{

    public class CreateBulletCommandHandler : ICommandHandler<CreateBulletCommand, ICommandCallback<string>>
    {
        public void Handle(CreateBulletCommand command, ICommandCallback<string> callback)
        {
            IGameObject? gameObject = GameHandler.GetGame(command.ConnectionId).FindGameObject(command.GameObjectId);

            IGameObject? bulletGameObject = new BulletGameObject();
            bulletGameObject.WorldMatrix = gameObject.WorldMatrix;
            bulletGameObject.Parent = gameObject;
            GameHandler.GetGame(command.ConnectionId).AddGameObject(bulletGameObject);

            if (bulletGameObject != null)
            {
                callback.OnSuccess("success");
            }
            else
            {
                callback.OnFailure();
            }
        }
    }
}
