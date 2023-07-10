using FluentAssertions;
using GamesEngine.Math;
using GamesEngine.Service.Game;
using GamesEngine.Service.Game.Object;
using GamesEngine.Service.GameLoop;
using GamesEngine.Tests.Fakes;
using GamesEngine.Tests.Fakes.GameObjects;

namespace GamesEngine.Tests;

[TestFixture]
public class GameBoundsTests
{
    [Test]
    public void ShouldCollideContains()
    {
        //Arrange
        var mockMovingObject = new MockMovingObject(new Vector(0, 0, 0));
        var mockStaticObject = new MockStaticObject(new Vector(0, 0, 0));
        var mockGame = new Game();
        mockGame.AddGameObject(mockMovingObject);
        mockGame.AddGameObject(mockStaticObject);

        //Act
        IGameObject? gameObject = GameObject.CollisionCheck(mockGame, mockMovingObject);

        //Assert
        gameObject.Should().NotBeNull();
        gameObject.Id.Should().Be(mockStaticObject.Id);
    }

    [Test]
    public void ShouldCollideIntersects()
    {
        //Arrange
        var mockMovingObject = new MockMovingObject(new Vector(0, 0, 0));
        var mockStaticObject = new MockStaticObject(new Vector(0.9f, 0, 0));
        var mockGame = new Game();
        mockGame.AddGameObject(mockMovingObject);
        mockGame.AddGameObject(mockStaticObject);

        //Act
        IGameObject? gameObject = GameObject.CollisionCheck(mockGame, mockMovingObject);

        //Assert
        gameObject.Should().NotBeNull();
        gameObject.Id.Should().Be(mockStaticObject.Id);
    }

    [Test]
    public void ShouldNotCollide()
    {
        //Arrange
        var mockMovingObject = new MockMovingObject(new Vector(0, 0, 0));
        var mockStaticObject = new MockStaticObject(new Vector(10, 10, 10));
        var mockGame = new Game();
        mockGame.AddGameObject(mockMovingObject);
        mockGame.AddGameObject(mockStaticObject);

        //Act
        IGameObject? gameObject = GameObject.CollisionCheck(mockGame, mockMovingObject);

        //Assert
        gameObject.Should().BeNull();
    }

    [Test]
    public void ShouldCollideWithMultipleObjectsAndReturnFirst()
    {
        //Arrange
        var mockMovingObject = new MockMovingObject(new Vector(0, 0, 0));
        var mockStaticObject = new MockStaticObject(new Vector(0, 0, 0));
        var mockStaticObject2 = new MockStaticObject(new Vector(0, 0, 0));
        var mockGame = new Game();
        mockGame.AddGameObject(mockMovingObject);
        mockGame.AddGameObject(mockStaticObject);
        mockGame.AddGameObject(mockStaticObject2);

        //Act
        IGameObject? gameObject = GameObject.CollisionCheck(mockGame, mockMovingObject);

        //Assert
        gameObject.Should().NotBeNull();
        gameObject.Id.Should().Be(mockStaticObject.Id);
    }

    [Test]
    public void ShouldCollideWithCorrect()
    {
        //Arrange
        var mockMovingObject = new MockMovingObject(new Vector(0, 0, 0));
        var mockStaticObject = new MockStaticObject(new Vector(10, 0, 0));
        var mockStaticObject2 = new MockStaticObject(new Vector(0, 0, 0));

        var mockGame = new Game();
        mockGame.AddGameObject(mockMovingObject);
        mockGame.AddGameObject(mockStaticObject);
        mockGame.AddGameObject(mockStaticObject2);

        //Act
        IGameObject? gameObject = GameObject.CollisionCheck(mockGame, mockMovingObject);

        //Assert
        gameObject.Should().NotBeNull();
        gameObject.Id.Should().Be(mockStaticObject2.Id);
    }

    [Test]
    public void ShouldMoveAndCollide()
    {
        //Arrange
        var mockMovingObject = new MockMovingObject(new Vector(1, 0, 0));
        var mockStaticObject = new MockStaticObject(new Vector(1, 0, 0));
        var mockStaticObject2 = new MockStaticObject(new Vector(3, 0, 0));

        var mockGame = new Game();
        var mockGameLoop = new MockGameLoop(mockGame, new MockTime(1000));
        mockGame.GameLoop = mockGameLoop;

        mockGame.AddGameObject(mockMovingObject);
        mockGame.AddGameObject(mockStaticObject);
        mockGame.AddGameObject(mockStaticObject2);

        IGameObject? firstCollide = GameObject.CollisionCheck(mockGame, mockMovingObject);
        firstCollide.Should().BeNull();

        mockGame.GameLoop.Update();
        mockMovingObject.WorldMatrix.GetPosition().GetX().Should().Be(1);

        IGameObject? secondCollide = GameObject.CollisionCheck(mockGame, mockMovingObject);
        secondCollide.Should().NotBeNull();
        secondCollide.Id.Should().Be(mockStaticObject.Id);

        mockGame.GameLoop.Update();
        mockGame.GameLoop.Update();
        mockMovingObject.WorldMatrix.GetPosition().GetX().Should().Be(3);

        IGameObject? thirdCollide = GameObject.CollisionCheck(mockGame, mockMovingObject);
        thirdCollide.Should().NotBeNull();
        thirdCollide.Id.Should().Be(mockStaticObject2.Id);
    }
}