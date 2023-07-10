using GamesEngine.Communication.Queries;
using GamesEngine.Patterns.Query;
using GamesEngine.Service.Game.Object;
using GamesEngine.Service.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesEngine.Patterns.Command;
using GamesEngine.Service.Communication.Commands;
using GamesEngine.Patterns;
using System.Collections;
using System.Numerics;
using GamesEngine.Math;
using GamesEngine.Service.Client;
using Vector = GamesEngine.Math.Vector;

namespace GamesEngine.Service.Communication.CommandHandlers
{
    public class MovePlayerCommandHandler : ICommandHandler<MovePlayerCommand, ICommandCallback<string>>
    {

        public void Handle(MovePlayerCommand command, ICommandCallback<string> callback)
        {
            IClient client = GameHandler.GetClient(command.ConnectionId);
            IPlayerGameObject gameObject = client.PlayerGameObject;

            if (gameObject != null)
            {
                float speed = 0.3f;

                IVector updatePosition = new Vector(0f, 0f, 0f);
                IVector direction = new Vector(command.x, command.y, command.z);
                updatePosition = direction * speed;

                gameObject.Motion += updatePosition;

                float maxSpeed = 10f;

                float xMotion = updatePosition.GetX();
                float yMotion = updatePosition.GetY();
                float zMotion = updatePosition.GetZ();

                xMotion = MathF.Max(MathF.Min(xMotion, maxSpeed), -maxSpeed);
                yMotion = MathF.Max(MathF.Min(yMotion, maxSpeed), -maxSpeed);
                zMotion = MathF.Max(MathF.Min(zMotion, maxSpeed), -maxSpeed);

                gameObject.Motion = new Vector(xMotion, yMotion, zMotion);


                if (updatePosition.GetX() != 0 || updatePosition.GetY() != 0 || updatePosition.GetZ() != 0)
                {
                    callback.OnSuccess("success");
                }
                else
                {
                    callback.OnFailure();
                }
            }
            else
            {
                callback.OnFailure();
            }
        }
    }
}
