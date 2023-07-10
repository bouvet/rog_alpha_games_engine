using GamesEngine.Patterns.Command;
using GamesEngine.Patterns.Query;
using GamesEngine.Service.Communication.Commands;
using GamesEngine.Service.Game.Object;
using System.Numerics;
using GamesEngine.Math;

namespace GamesEngine.Service.Communication.CommandHandlers
{
    public class RotateGameObjectCommandHandler : ICommandHandler<RotateGameObjectCommand, ICommandCallback<string>>
    {
        public void Handle(RotateGameObjectCommand command, ICommandCallback<string> callback)
        {
            IGameObject? gameObject = GameHandler.GetGame(command.ConnectionId).FindGameObject(GameHandler.GetClient(command.ConnectionId).PlayerGameObject.Id);

            if (gameObject != null)
            {
                var rot = CalculateRotation(gameObject.WorldMatrix.GetPosition(), command.MousePositionX, command.MousePositionY);
                IVector rotationVector = new Math.Vector(90, rot, 0);
                gameObject.WorldMatrix.SetRotation(rotationVector);
                callback.OnSuccess("success");
                return;
            }

            callback.OnFailure();
        }

        private static float CalculateRotation(IVector vector, float targetX, float targetY)
        {
            Vector3 targetPosition = new Vector3(targetX, targetY, 0);
            Vector3 currentPosition = new Vector3(vector.GetX(), vector.GetY(), 0);
            Vector3 direction = Vector3.Normalize(targetPosition - currentPosition);
            // Calculate the rotation angle in radians
            float rotationAngleRadians = MathF.Atan2(direction.Y, direction.X);

            // Convert the rotation angle to degrees
            float rotationAngleDegrees = rotationAngleRadians * (180.0f / MathF.PI);
            return rotationAngleDegrees + 90;
        }
    }
}
