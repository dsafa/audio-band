using System;

namespace AudioBand.Connector
{
    public class TrackInfoChangedEventArgs : EventArgs
    {
        public string TrackName { get; set; }
        public string Artist { get; set; }
    }
}
