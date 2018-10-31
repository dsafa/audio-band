using System;
using System.Drawing;

namespace AudioBand.AudioSource
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

        /// <summary>
        /// Album name
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// Length of the track
        /// </summary>
        public TimeSpan TrackLength { get; set; }
    }
}
