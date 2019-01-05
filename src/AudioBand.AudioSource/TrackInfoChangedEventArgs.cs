using System;
using System.Drawing;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Event arguments for the <see cref="IAudioSource.TrackInfoChanged"/> event.
    /// </summary>
    public class TrackInfoChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the track Name
        /// </summary>
        public string TrackName { get; set; }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Gets or sets the album art image. If <see langword="null"/>, a placeholder will be used.
        /// </summary>
        public Image AlbumArt { get; set; }

        /// <summary>
        /// Gets or sets the album name.
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// Gets or sets the length of the track
        /// </summary>
        public TimeSpan TrackLength { get; set; }
    }
}
