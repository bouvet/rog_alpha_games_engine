using GamesEngine.Service.Client;
using GamesEngine.Service.GameLoop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesEngine.Service.Game.Graph;
using GamesEngine.Communication.Queries;
using GamesEngine.Math;
using GamesEngine.Service.Communication;
using GamesEngine.Service.Game.Maps;
using GamesEngine.Service.Game.Object;
using GamesEngine.Service.Game.Object.StaticGameObjects;
using GamesEngine.Service.Server;
using IUser = GamesEngine.Users.IUser;

namespace GamesEngine.Service.Game
{
    public interface IGame
    {
        public List<IClient> Clients { get; set; }
        public IGameLoop GameLoop { get; set; }
        public ISceneGraph SceneGraph { get; set; }

        public void AddGameObject(IGameObject? gameObject);
        public void RemoveGameObject(int id);
        public IGameObject? FindGameObject(int id);

        public IClient OnConnect(string connectionId);
        public void OnDisconnect(IClient client);
    }

    public class Game : IGame
    {
        public List<IClient> Clients { get; set; } = new();
        public IGameLoop GameLoop { get; set; }
        public ISceneGraph SceneGraph { get; set; } = new SceneGraph();

        public Game()
        {
            GameLoop = new GameLoop.GameLoop(this);
        }

        public Game(IGameMap gameMap)
        {
            GameLoop = new GameLoop.GameLoop(this);

            // Create a game world
            for (var x = 0; x < gameMap.Width; x++)
            {
                for (var y = 0; y < gameMap.Height; y++)
                {
                    if(x == 0 || x == gameMap.Width - 1 || y == 0 || y == gameMap.Height - 1)
                    {
                        WallGameObject? wallGameObject = new WallGameObject();
                        float height = (x + y) % 2 == 0 ? 1 : 1.5f;

                        wallGameObject.WorldMatrix.SetPosition(new Vector(x - (gameMap.Width/ 2f), y - (gameMap.Height / 2f), 0));
                        wallGameObject.WorldMatrix.SetScale(new Vector(1, 1, height));
                        AddGameObject(wallGameObject);
                    }

                    FloorGameObject? floorGameObject = new FloorGameObject();
                    floorGameObject.WorldMatrix.SetPosition(new Vector(x - (gameMap.Width / 2f), y - (gameMap.Height / 2f), -1));
                    AddGameObject(floorGameObject);
                }
            }


            foreach (var mapObject in gameMap.Objects)
            {
                IGameObject? gameObject = GameHandler.MapsHandler.MapMaterialHandler.GetGameObject(mapObject);

                if (gameObject != null)
                {
                    gameObject.WorldMatrix.SetPosition(mapObject.Position);
                    if(mapObject.Rotation != null) gameObject.WorldMatrix.SetRotation(mapObject.Rotation);
                    if(mapObject.Scale != null) gameObject.WorldMatrix.SetScale(mapObject.Scale);
                    AddGameObject(gameObject);
                }
            }
        }

        public IGameObject? FindGameObject(int id)
        {
            if (SceneGraph.DynamicGameObject.ContainsKey(id))
            {
                return SceneGraph.DynamicGameObject.Get(id);
            }

            if (SceneGraph.StaticGameObject.ContainsKey(id))
            {
                return SceneGraph.StaticGameObject.Get(id);
            }
            return null;
        }

        public void RemoveGameObject(int id)
        {
            IGameObject? gameObject = FindGameObject(id);

            if (gameObject is IDynamicGameObject)
            {
                SceneGraph.DynamicGameObject.Remove(id);
            }
            else if (gameObject is IStaticGameObject)
            {
                SceneGraph.StaticGameObject.Remove(id);
            }
        }

        public void AddGameObject(IGameObject? gameObject)
        {
            var highestDynamicID = SceneGraph.DynamicGameObject.GetKeys().Count > 0 ? SceneGraph.DynamicGameObject.GetKeys().Max() : 0;
            var highestStaticID = SceneGraph.StaticGameObject.GetKeys().Count > 0 ? SceneGraph.StaticGameObject.GetKeys().Max() : 0;

            var newId = System.Math.Max(highestDynamicID, highestStaticID) + 1;
            gameObject.Id = newId;

            if (gameObject is IDynamicGameObject dynamicGameObject)
            {
                SceneGraph.DynamicGameObject.Add(newId, dynamicGameObject);
            }
            else if (gameObject is IStaticGameObject staticGameObject)
            {
                SceneGraph.StaticGameObject.Add(newId, staticGameObject);
            }
        }

        public IClient OnConnect(string connectionId)
        {
            IClient client = new Client.Client();
            client.ConnectionId = connectionId;

            //Replace with actual user id logic
            client.UserId = Clients.Count;

            Clients.Add(client);

            PlayerGameObject? playerGameObject = new PlayerGameObject(client);
            playerGameObject.WorldMatrix.SetPosition(new Vector(0, 0, 0));
            playerGameObject.WorldMatrix.SetScale(new Vector(0.75f, 0.75f, 1.5f));
            AddGameObject(playerGameObject);

            client.PlayerGameObject = playerGameObject;

            var user = GameHandler.UserHandler.GetUser(client.UserId);
            if(user != null) Console.WriteLine($"\"{user.Name}\" Connected with ID: {client.UserId} and ConnectionID: {client.ConnectionId}");

            return client;
        }

        public void OnDisconnect(IClient client)
        {
            Clients.Remove(client);
            SceneGraph.DynamicGameObject.Remove(client.PlayerGameObject.Id);

            var user = GameHandler.UserHandler.GetUser(client.UserId);

            if(user != null) Console.WriteLine($"\"{user.Name}\" Disconnected with ID: {client.UserId} and ConnectionID: {client.ConnectionId}");
        }
    }
}
