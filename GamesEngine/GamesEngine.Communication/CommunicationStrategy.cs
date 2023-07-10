using GamesEngine.Patterns;

public delegate void MessageCallback(string senderId, IMessage message);

/// <summary>
/// Defines methods for a communication strategy.
/// </summary>
public interface ICommunicationStrategy
{
    /// <summary>
    /// Occurs when a message is received.
    /// </summary>
    MessageCallback OnMessage { get; }

    /// <summary>
    /// Sends a message to a specific client.
    /// </summary>
    /// <param name="targetId">The ID of the target client.</param>
    /// <param name="message">The message to send.</param>
    void SendToClient(string targetId, IMessage message);

    /// <summary>
    /// Sends a message to all clients.
    /// </summary>
    /// <param name="message">The message to send.</param>
    void SendToAllClients(IMessage message);
}