using System;

namespace AudioSourceHost
{
    public class MarshaledEventHandler<T> : MarshalByRefObject
    {
        private readonly Action<T> _handler;

        public MarshaledEventHandler(Action<T> handler)
        {
            _handler = handler;
        }

        public void Handler(object sender, T args)
        {
            _handler(args);
        }
    }
}
