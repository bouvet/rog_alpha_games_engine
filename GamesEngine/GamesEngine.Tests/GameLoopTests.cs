using FluentAssertions;
using GamesEngine.Math;
using GamesEngine.Service;
using GamesEngine.Service.Client;
using GamesEngine.Service.Game;
using GamesEngine.Service.Game.Object;
using GamesEngine.Service.GameLoop;
using GamesEngine.Tests.Fakes;
using GamesEngine.Tests.Fakes.GameObjects;

namespace GamesEngine.Tests;

[TestFixture]
public class GameLoopTests
{
    [Test]
    public void ShouldBeAbleToCreateGameLoop()
    {
        // Arrange
        IGameLoop gameLoopNotNull = new GameLoop(new MockGame());

        // Assert
        gameLoopNotNull.Should().NotBeNull();
    }

    [Test]
    public void ShouldMoveObject()
    {
        // Arrange
        IGame game = new Game();
        IGameLoop gameLoop = new MockGameLoop(game, new MockTime(1000));
        game.GameLoop = gameLoop;

        IMatrix position = new Matrix();
        IDynamicGameObject? gameObject = new MockMovingObject(new Vector(1, 1, 1));
        gameObject.WorldMatrix = position;
        game.AddGameObject(gameObject);

        // Act
        gameLoop.Update();

        // Assert
        gameLoop.Should().NotBeNull();
        game.FindGameObject(gameObject.Id).WorldMatrix.GetPosition().GetX().Should().Be(1);
        game.FindGameObject(gameObject.Id).WorldMatrix.GetPosition().GetY().Should().Be(1);
        game.FindGameObject(gameObject.Id).WorldMatrix.GetPosition().GetZ().Should().Be(1);
    }

    [Test]
    public void ShouldBeAbleToUpdateGameLoop()
    {
        // Arrange
        IGameLoop gameLoop = new GameLoop(new MockGame());

        // Act
        gameLoop.Update();

        // Assert
        gameLoop.Should().NotBeNull();
    }

    [Test]
    public void ShouldBeAbleToRenderGameLoop()
    {
        // Arrange
        IGameLoop gameLoop = new GameLoop(new MockGame());

        // Act
        gameLoop.Render();

        // Assert
        gameLoop.Should().NotBeNull();
    }

    [Test]
    public void ShouldBeAbleToProcessInputGameLoop()
    {
        // Arrange
        IGameLoop gameLoop = new GameLoop(new MockGame());

        // Act
        gameLoop.ProcessInput();

        // Assert
        gameLoop.Should().NotBeNull();
    }
}