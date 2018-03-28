using System;
using System.Drawing;

namespace AudioBand.Connector
{
    public class TrackInfoChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Track Name
        /// </summary>
        public string TrackName { get; set; }

        /// <summary>
        /// Artist
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Album art image. If null, a placeholder will be used
        /// </summary>
        public Image AlbumArt { get; set; }
    }
}
