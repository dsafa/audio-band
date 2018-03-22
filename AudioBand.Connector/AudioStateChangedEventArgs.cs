using System;

namespace AudioBand.Connector
{
    public class AudioStateChangedEventArgs : EventArgs
    {
        public AudioState State { get; set; }
    }
}
