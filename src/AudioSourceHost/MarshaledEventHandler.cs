using System;

namespace AudioSourceHost
{
    public class MarshaledEventHandler : MarshalByRefObject
    {
        private readonly Action _handler;

        public MarshaledEventHandler(Action handler)
        {
            _handler = handler;
        }

        public void Handler(object sender, EventArgs e)
        {
            _handler();
        }
    }
}
