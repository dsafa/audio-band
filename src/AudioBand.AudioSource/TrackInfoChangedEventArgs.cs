using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Event arguments for the <see cref="IAudioSource.TrackInfoChanged"/> event.
    /// </summary>
    [DataContract]
    public class TrackInfoChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the track Name
        /// </summary>
        [DataMember]
        public string TrackName { get; set; }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        [DataMember]
        public string Artist { get; set; }

        /// <summary>
        /// Gets or sets the album art image. If <see langword="null"/>, a placeholder will be used.
        /// </summary>
        [DataMember]
        public Image AlbumArt { get; set; }

        /// <summary>
        /// Gets or sets the album name.
        /// </summary>
        [DataMember]
        public string Album { get; set; }

        /// <summary>
        /// Gets or sets the length of the track
        /// </summary>
        [DataMember]
        public TimeSpan TrackLength { get; set; }
    }
}
