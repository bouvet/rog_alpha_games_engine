using GamesEngine.Service.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesEngine.Service.Game.Object;

namespace GamesEngine.Service.Client
{
    public interface IClient
    {
        public int UserId { get; set; }
        public PlayerGameObject? PlayerGameObject { get; set;  }
        public string? ConnectionId { get; set; }
    }
    public class Client : IClient
    {
        public int UserId { get; set; }
        public PlayerGameObject? PlayerGameObject { get; set; }
        public string? ConnectionId { get; set; }
    }
}
