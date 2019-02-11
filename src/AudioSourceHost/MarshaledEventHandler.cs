using System;

namespace AudioSourceHost
{
    /// <summary>
    /// Event handler wrapper for cross app domain callbacks.
    /// </summary>
    public class MarshaledEventHandler : MarshalByRefObject
    {
        private readonly Action _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarshaledEventHandler"/> class
        /// with the action to run when the event is invoked.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public MarshaledEventHandler(Action handler)
        {
            _handler = handler;
        }

        /// <summary>
        /// The event handler compatible delegate.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        public void Handler(object sender, EventArgs e)
        {
            _handler();
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
