using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AudioBand.Logging;
using NLog;

namespace AudioBand.Messages
{
    /// <summary>
    /// Implementation for the message bus.
    /// </summary>
    public class MessageBus : IMessageBus
    {
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<MessageBus>();
        private readonly Dictionary<Type, List<object>> _handlers = new Dictionary<Type, List<object>>();

        /// <inheritdoc />
        public void Subscribe<TMessage>(Action<TMessage> handler)
        {
            Logger.Debug("{class} registered new handler for {message}", handler.Target.GetType(), typeof(TMessage));

            if (_handlers.ContainsKey(typeof(TMessage)))
            {
                _handlers[typeof(TMessage)].Add(handler);
            }
            else
            {
                _handlers.Add(typeof(TMessage), new List<object> { handler });
            }
        }

        /// <inheritdoc />
        public void Unsubscribe<TMessage>(Action<TMessage> handler)
        {
            Logger.Debug("{class} unsubscribed for {message}", handler.Target.GetType(), typeof(TMessage));

            if (_handlers.TryGetValue(typeof(TMessage), out var handlers))
            {
                handlers.Remove(handler);
            }
        }

        /// <inheritdoc />
        public void Publish<TMessage>(TMessage message, [CallerMemberName] string caller = "")
        {
            Logger.Debug("Publishing message {message}", message);

            if (!_handlers.TryGetValue(typeof(TMessage), out var handlers))
            {
                return;
            }

            Logger.Debug("{publisher} -> {@handlers}", caller, handlers);

            foreach (var handler in handlers)
            {
                ((Action<TMessage>)handler)(message);
            }
        }
    }
}
