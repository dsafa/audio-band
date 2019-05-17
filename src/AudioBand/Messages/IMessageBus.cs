using System;
using System.Runtime.CompilerServices;

namespace AudioBand.Messages
{
    /// <summary>
    /// A class to send and receive system wide messages.
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        /// Subscribes to a message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The handler for the message.</param>
        void Subscribe<TMessage>(Action<TMessage> handler);

        /// <summary>
        /// Unsubscribes to a message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The handler for the message.</param>
        void Unsubscribe<TMessage>(Action<TMessage> handler);

        /// <summary>
        /// Publishes a message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message to publish.</param>
        /// <param name="caller">The caller.</param>
        void Publish<TMessage>(TMessage message, [CallerMemberName] string caller = "");
    }
}
