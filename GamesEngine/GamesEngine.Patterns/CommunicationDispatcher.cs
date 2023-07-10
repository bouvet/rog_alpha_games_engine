using System.Reflection;
using GamesEngine.Patterns.Command;
using GamesEngine.Patterns.Query;

namespace GamesEngine.Patterns;

public delegate void QueryCallback(string response);
public delegate void CommandCallback(string response);
public delegate void FailureCallback();

public interface ICommunicationDispatcher
{

    /// <summary>
    /// Resolves the given command by finding the appropriate command handler and invoking it with the given command.
    /// </summary>
    /// <param name="command">The command to resolve.</param>
    /// <param name="callback">The callback to invoke when the command has been successfully resolved.</param>
    /// <param name="failureCallback">The callback to invoke when the command resolution has failed.</param>
    void ResolveCommand(ICommand command, CommandCallback callback, FailureCallback failureCallback);

    /// <summary>
    /// Resolves the given query by finding the appropriate query handler and invoking it with the given query.
    /// </summary>
    /// <param name="query">The query to resolve.</param>
    /// <param name="callback">The callback to invoke when the query has been successfully resolved.</param>
    /// <param name="failureCallback">The callback to invoke when the query resolution has failed.</param>
    void ResolveQuery(IQuery query, QueryCallback callback, FailureCallback failureCallback);
}

/// <summary>
/// Defines the interface for a class that provides access to the types of command and query handlers used by a communication dispatcher.
/// </summary>
public interface IDispatcherTypes
{
    List<Type> QueryHandlers();
    List<Type> CommandHandlers();
}

public class DispatcherTypes : IDispatcherTypes
{
    /// <summary>
    /// This method is used to get all the query handlers in the system.
    /// </summary>
    /// <returns>A list of query handlers.</returns>
    public List<Type> QueryHandlers()
    {
        return FindTypes(typeof(IQueryHandler<,>));
    }

    // Find all types that implement ICommandHandler<,>.
    // This is done by finding all types in all assemblies that are not abstract or interfaces,
    // and then check if any of the types they implement are ICommandHandler<,>.
    // If they are, add them to a list of types and return it.
    public List<Type> CommandHandlers()
    {
        return FindTypes(typeof(ICommandHandler<,>));
    }

    /// <summary>
    /// Finds all types in all assemblies that implement the specified interface type.
    /// </summary>
    /// <param name="checkType">The interface type to check for.</param>
    /// <returns>A list of types that implement the specified interface type.</returns>
    private static List<Type> FindTypes(Type checkType)
    {
        var types = new List<Type>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        foreach (var type in assembly.GetTypes())
        {
            if (type is not { IsAbstract: false, IsInterface: false }) continue;

            if (type.GetInterfaces().Any(iface =>
                    iface.IsGenericType && iface.GetGenericTypeDefinition() == checkType))
                types.Add(type);
        }

        return types;
    }
}

public class CommunicationDispatcher : ICommunicationDispatcher
{
    /// <summary>
    /// Gets or sets the dispatcher types used to resolve commands and queries.
    /// </summary>
    protected IDispatcherTypes DispatcherTypes { get; init; } = new DispatcherTypes();

    /// <summary>
    /// Resolves the given command by finding the appropriate command handler and invoking it with the given command.
    /// </summary>
    /// <param name="command">The command to resolve.</param>
    /// <param name="callback">The callback to invoke when the command has been successfully resolved.</param>
    /// <param name="failureCallback">The callback to invoke when the command resolution has failed.</param>
    public void ResolveCommand(ICommand command, CommandCallback callback, FailureCallback failureCallback)
    {
        var method = typeof(CommunicationDispatcher).GetMethod(nameof(InvokeCommandHandler),
            BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var type in DispatcherTypes.CommandHandlers())
            if (type.GetInterfaces().Select(iface => iface.GetGenericArguments()).Any(genericArgs => genericArgs.Length > 0 && genericArgs[0] == command.GetType() && !type.IsAbstract))
            {
                var commandType = command.GetType();
                var instance = Activator.CreateInstance(type);
                var genericMethod = method?.MakeGenericMethod(commandType, typeof(ICommandCallback<string>));
                genericMethod?.Invoke(this, new[]
                {
                    instance, command, new CommandCallback<string>(
                        response => { callback(response); },
                        () => { failureCallback(); }
                    )
                });
            }
    }

    /// <summary>
    /// Resolves the given query by finding the appropriate query handler and invoking it with the given query.
    /// </summary>
    /// <param name="query">The query to resolve.</param>
    /// <param name="callback">The callback to invoke when the query has been successfully resolved.</param>
    /// <param name="failureCallback">The callback to invoke when the query resolution has failed.</param>
    public void ResolveQuery(IQuery query, QueryCallback callback, FailureCallback failureCallback)
    {
        var method = typeof(CommunicationDispatcher).GetMethod(nameof(InvokeQueryHandler),
            BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var type in DispatcherTypes.QueryHandlers())
            if (type.GetInterfaces().Select(iface => iface.GetGenericArguments()).Any(genericArgs =>
                    genericArgs[0] == query.GetType() &&
                    typeof(IQueryCallback<string>).IsAssignableFrom(genericArgs[1]) && !type.IsAbstract))
            {
                var queryType = query.GetType();
                var instance = Activator.CreateInstance(type);
                var genericMethod = method?.MakeGenericMethod(queryType, typeof(IQueryCallback<string>));
                genericMethod?.Invoke(this, new[]
                {
                    instance, query, new QueryCallback<string>(
                        response => { callback(response); },
                        () => { failureCallback(); }
                    )
                });
            }
    }

    /// <summary>
    /// Invokes the given command handler with the specified command and callback.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to handle.</typeparam>
    /// <typeparam name="TCallback">The type of callback to use.</typeparam>
    /// <param name="handler">The command handler to invoke.</param>
    /// <param name="command">The command to handle.</param>
    /// <param name="callback">The callback to use.</param>
    /// <exception cref="ArgumentNullException">Thrown when the handler, command, or callback is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command or callback is of the wrong type.</exception>
    private void InvokeCommandHandler<TCommand, TCallback>(ICommandHandler<TCommand, TCallback> handler,
        TCommand command, TCallback callback)
        where TCommand : ICommand
        where TCallback : ICommandCallback<string>
    {
        handler.Handle(command, callback);
    }

    /// <summary>
    /// Invokes the given query handler with the specified query and callback.
    /// </summary>
    /// <typeparam name="TQuery">The type of query to handle.</typeparam>
    /// <typeparam name="TCallback">The type of callback to use.</typeparam>
    /// <param name="handler">The query handler to invoke.</param>
    /// <param name="query">The query to handle.</param>
    /// <param name="callback">The callback to use.</param>
    /// <exception cref="ArgumentNullException">Thrown when the handler, query, or callback is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the query or callback is of the wrong type.</exception>
    private void InvokeQueryHandler<TQuery, TCallback>(IQueryHandler<TQuery, TCallback> handler, TQuery query,
        TCallback callback)
        where TQuery : IQuery
        where TCallback : IQueryCallback<string>
    {
        handler.Handle(query, callback);
    }
}