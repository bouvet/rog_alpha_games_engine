using FluentAssertions;
using GamesEngine.Service.Client;
using GamesEngine.Service.Game;
using GamesEngine.Service.Game.Object;
using GamesEngine.Tests.Fakes.GameObjects;

namespace GamesEngine.Tests;

[TestFixture]
public class GameTests
{
    [Test]
    public void ShouldBeAbleToCreateGame()
    {
        // Arrange
        Game gameNotNull = new Game();

        // Assert
        gameNotNull.Should().NotBeNull();
    }

    [Test]
    public void ShouldBeAbleToCreateGameWithObject()
    {
        // Arrange
        Game game = new Game();
        MockDynamicObject? gameObject = new MockDynamicObject();

        // Act
        game.AddGameObject(gameObject);

        // Assert
        game.SceneGraph.DynamicGameObject.Should().NotBeNull();
        game.SceneGraph.DynamicGameObject.GetValues().Should().HaveCountGreaterThan(0);
    }

    [Test]
    public void ShouldBeAbleToAddAndGetObject()
    {
        // Arrange
        Game game = new Game();
        MockDynamicObject? gameObject = new MockDynamicObject();

        // Act
        game.AddGameObject(gameObject);
        var id = gameObject.Id;

        // Assert
        game.FindGameObject(id).Should().NotBeNull();
        game.FindGameObject(id).Id.Should().Be(id);
        game.FindGameObject(id).Should().Be(gameObject);
    }

    [Test]
    public void ShouldBeAbleToAddAndRemoveObject()
    {
        // Arrange
        Game game = new Game();
        MockDynamicObject? gameObject = new MockDynamicObject();

        // Act
        game.AddGameObject(gameObject);
        var id = gameObject.Id;
        game.RemoveGameObject(id);

        // Assert
        game.FindGameObject(id).Should().BeNull();
    }

    [Test]
    public void ShouldBeAbleToAddClient()
    {
        // Arrange
        Game game = new Game();

        // Act
        IClient client = game.OnConnect("1");

        // Assert
        game.Clients.Should().NotBeNull();
        game.Clients.Should().HaveCountGreaterThan(0);
        game.Clients.Should().Contain(client);
    }

    [Test]
    public void ShouldBeAbleToAddClientAndDisconnect()
    {
        // Arrange
        Game game = new Game();

        // Act
        IClient client = game.OnConnect("1");
        game.OnDisconnect(client);

        // Assert
        game.Clients.Should().NotBeNull();
        game.Clients.Should().HaveCount(0);
        game.Clients.Should().NotContain(client);
    }

    [Test]
    public void ShouldBeAbleToAddPlayerObject()
    {
        // Arrange
        Game game = new Game();

        // Act
        IClient client = game.OnConnect("1");

        // Assert
        client.PlayerGameObject.Should().NotBeNull();
        game.FindGameObject(client.PlayerGameObject.Id).Should().NotBeNull();
    }
}