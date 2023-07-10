using GamesEngine.Patterns;
using GamesEngine.Patterns.Command;
using GamesEngine.Patterns.Query;

namespace GamesEngine.Service.Communication
{
    /// <summary>
    /// Defines the interface for a communication service that can send and receive messages to and from clients.
    /// </summary>
    public interface ICommunication
    {
        /// <summary>
        /// Gets the communication strategy used by this instance of Communication.
        /// </summary>
        ICommunicationStrategy CommunicationStrategy { get; }

        /// <summary>
        /// Gets the communication dispatcher used by this instance of Communication.
        /// </summary>
        ICommunicationDispatcher CommunicationDispatcher { get; }

        /// <summary>
        /// Sends a message to a specific client.
        /// </summary>
        /// <param name="targetId">The ID of the target client.</param>
        /// <param name="message">The message to send.</param>
        void SendToClient(string targetId, IMessage message);

        /// <summary>
        /// Sends a message to all connected clients.
        /// </summary>
        /// <param name="message">The message to send.</param>
        void SendToAllClients(IMessage message);

        /// <summary>
        /// This method is called when a message is received from a client.
        /// It processes the message and sends a response back to the client if necessary.
        /// </summary>
        /// <param name="senderId">The ID of the client that sent the message.</param>
        /// <param name="message">The message received from the client.</param>
        void OnMessage(string senderId, IMessage message);
    }

    public class Communication : ICommunication
    {
        /// <summary>
        /// Gets the communication strategy used by this instance of Communication.
        /// </summary>
        public ICommunicationStrategy CommunicationStrategy { get; private set; }

        /// <summary>
        /// Gets the communication dispatcher used by this instance of Communication.
        /// </summary>
        public ICommunicationDispatcher CommunicationDispatcher { get; private set; }

        public Communication(ICommunicationStrategy communicationStrategy,
                             ICommunicationDispatcher communicationDispatcher)
        {
            CommunicationStrategy = communicationStrategy;
            CommunicationDispatcher = communicationDispatcher;
        }

        /// <summary>
        /// This method is called when a message is received from a client.
        /// It processes the message and sends a response back to the client if necessary.
        /// </summary>
        /// <param name="senderId">The ID of the client that sent the message.</param>
        /// <param name="message">The message received from the client.</param>
        public void OnMessage(string senderId, IMessage message)
        {
            message.ConnectionId = senderId;
            switch (message)
            {
                case IQuery query:
                    CommunicationDispatcher.ResolveQuery(query,
                    
                    (response) =>
                    {
                        SendToClient(senderId, new Response(query.Type, response));
                    },
                    () =>
                    {
                        //TODO Failure
                    });
                    break;

                case ICommand command:
                    CommunicationDispatcher.ResolveCommand(command,
                    (response) =>
                    {
                        SendToClient(senderId, new Response(command.Type, response));
                    },
                    () =>
                    {
                        //TODO Failure
                    });
                    break;
            }
        }

        /// <summary>
        /// This method sends a message to a specific client.
        /// </summary>
        /// <param name="targetId">The ID of the target client.</param>
        /// <param name="message">The message to send.</param>
        public void SendToClient(string targetId, IMessage message)
        {
            CommunicationStrategy.SendToClient(targetId, message);
        }

        /// <summary>
        /// This method sends a message to all connected clients.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void SendToAllClients(IMessage message)
        {
            CommunicationStrategy.SendToAllClients(message);
        }
    }
}

