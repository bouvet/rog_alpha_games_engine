using GamesEngine.Patterns.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesEngine.Service.Communication.Commands
{
    public interface IRotateGameObjectCommand : ICommand
    {
        public float MousePositionX { get; set; }
        public float MousePositionY { get; set; }
    }
    public class RotateGameObjectCommand : IRotateGameObjectCommand
    {
        public string Type { get; set; } = "RotateGameObject";
        public string? ConnectionId { get; set; }
        public float MousePositionX { get; set; }
        public float MousePositionY { get; set; }

        public RotateGameObjectCommand()
        {
        }
    }
}
