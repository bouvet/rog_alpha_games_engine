using GamesEngine.Service.Client;
using GamesEngine.Service.Game;
using GamesEngine.Service.Game.Graph;
using GamesEngine.Service.Game.Maps;
using GamesEngine.Service.Game.Object;
using GamesEngine.Service.GameLoop;

namespace GamesEngine.Tests.Fakes;

public class MockGame : Game
{
    public MockGame() { }

    public MockGame(IGameMap gameMap) : base(gameMap) { }
}