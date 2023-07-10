using Newtonsoft.Json;
using FluentAssertions;
using GamesEngine.Communication.Queries;
using GamesEngine.Communication.Queries.Handlers;
using GamesEngine.Math;
using GamesEngine.Patterns;
using GamesEngine.Patterns.Command;
using GamesEngine.Patterns.Query;
using GamesEngine.Service;
using GamesEngine.Service.Communication;
using GamesEngine.Service.Game;
using GamesEngine.Service.Game.Maps;
using GamesEngine.Service.Game.Object;
using GamesEngine.Tests.Fakes;
using GamesEngine.Tests.Fakes.GameObjects;
using static GamesEngine.Service.Communication.Communication;

namespace GamesEngine.Tests;

[TestFixture]
public class CommunicationTests
{
    private ICommunication Communication;
    private ICommunicationStrategy CommunicationStrategy;
    private ICommunicationDispatcher CommunicationDispatcher;
    private IDispatcherTypes MockDispatcherTypes;

    public CommunicationTests()
    {
        MockDispatcherTypes = new MockDispatcherTypes(
            new List<Type> { typeof(MockQueryHandler), typeof(FetchDynamicObjectsHandler) },
            new List<Type> { typeof(MockCommandHandler), typeof(DynamicCommandHandler) });
        CommunicationStrategy = new CommunicationStrategyMock((id, mes) => Communication.OnMessage(id, mes));
        CommunicationDispatcher = new CommunicationDispatcherMock(MockDispatcherTypes);
        Communication = new CommunicationMock(CommunicationStrategy, CommunicationDispatcher);
    }

    [Test]
    public void ShouldFindQueryHandler()
    {
        // Arrange
        IDispatcherTypes dispatcherTypes = new DispatcherTypes();

        // Act
        var queryHandlers = dispatcherTypes.QueryHandlers();

        // Assert
        queryHandlers.Should().NotBeNull();
        queryHandlers.Should().HaveCountGreaterThan(0);
        queryHandlers.Should().Contain(typeof(MockQueryHandler));
    }

    [Test]
    public void ShouldFindCommandHandler()
    {
        // Arrange
        IDispatcherTypes dispatcherTypes = new DispatcherTypes();

        // Act
        var commandHandlers = dispatcherTypes.CommandHandlers();

        // Assert
        commandHandlers.Should().NotBeNull();
        commandHandlers.Should().HaveCountGreaterThan(0);
        commandHandlers.Should().Contain(typeof(MockCommandHandler));
    }

    [Test]
    public void ShouldBeAbleToSendQueryMessage()
    {
        // Arrange
        IMessage message = new QueryMock();

        // Act
        Communication.SendToAllClients(message);

        // Assert
        ((CommunicationMock)Communication).Result.Should().Be("Success");
    }

    [Test]
    public void ShouldBeAbleToSendCommandMessage()
    {
        // Arrange
        IMessage message = new CommandMock();

        // Act
        Communication.SendToAllClients(message);

        // Assert
        ((CommunicationMock)Communication).Result.Should().Be("Success");
    }

    [Test]
    public void ShouldBeAbleToHandleQuery()
    {
        // Arrange
        IMessage message = new QueryMock();
        var result = "";

        // Act
        CommunicationDispatcher.ResolveQuery(message as IQuery,
            (response) =>
            {
                result = "success";
            },
            () =>
            {
                result = "failure";
            });

        // Assert
        result.Should().Be("success");
    }

    [Test]
    public void ShouldBeAbleToHandleCommand()
    {
        // Arrange
        IMessage message = new CommandMock();
        var result = "";

        // Act
        CommunicationDispatcher.ResolveCommand(message as ICommand,
            (response) =>
            {
                result = "success";
            },
            () =>
            {
                result = "failure";
            });

        // Assert
        result.Should().Be("success");
    }

    [Test]
    public void ShouldBeAbleToHandleDynamicCommandFailure()
    {
        // Arrange
        IMessage message = new DynamicCommandMock(false);
        var result = "";

        // Act
        CommunicationDispatcher.ResolveCommand(message as ICommand,
            (response) =>
            {
                result = "success";
            },
            () =>
            {
                result = "failure";
            });

        // Assert
        result.Should().Be("failure");
    }

    [Test]
    public void ShouldBeAbleToHandleDynamicCommandSuccess()
    {
        // Arrange
        IMessage message = new DynamicCommandMock(true);
        var result = "";

        // Act
        CommunicationDispatcher.ResolveCommand(message as ICommand,
            (response) =>
            {
                result = "success";
            },
            () =>
            {
                result = "failure";
            });

        // Assert
        result.Should().Be("success");
    }

    [Test]
    public void ShouldBeAbleToGetObjects()
    {
        //Arrange
        GameHandler.MapsHandler = new MockMapsHandler(new MockMapMaterialHandler(null), new List<IGameMap>());
        GameHandler.UserHandler = new MockUserHandler(null);

        var connectionId = "TEST";
        IGame mockGame = new Game();
        GameHandler.AddGame(0, mockGame);
        GameHandler.AddPlayerId(connectionId, 0);

        mockGame.AddGameObject(new MockMovingObject(new Vector(1,1,1)));
        var result = "";
        IDynamicGameObject gameObject = new MockMovingObject(new Vector(1, 1, 1));
        gameObject.Id = 1;
        List<IDynamicGameObject> list = new List<IDynamicGameObject>();
        list.Add(gameObject);
        var query = new FetchDynamicObjectsQuery();
        query.ConnectionId = connectionId;

        //Act
        CommunicationDispatcher.ResolveQuery(query,
            (response) => { result = response;},
            () => { Assert.Fail(); });

        string jsonString = JsonConvert.SerializeObject(list, Formatting.Indented, new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()
        });

        //Assert
        result.Should().Be(jsonString);
    }
}