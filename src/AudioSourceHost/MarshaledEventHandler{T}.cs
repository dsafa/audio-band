using System;

namespace AudioSourceHost
{
    /// <summary>
    /// Event handler wrapper for cross app domain callbacks.
    /// </summary>
    /// <typeparam name="T">Type of the event args</typeparam>
    public class MarshaledEventHandler<T> : MarshalByRefObject
    {
        private readonly Action<T> _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarshaledEventHandler{T}"/> class
        /// with the action to run when the event is invoked.
        /// </summary>
        /// <param name="handler">Action to run.</param>
        public MarshaledEventHandler(Action<T> handler)
        {
            _handler = handler;
        }

        /// <summary>
        /// The event handler compatible delegate.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The event args.</param>
        public void Handler(object sender, T args)
        {
            _handler(args);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
