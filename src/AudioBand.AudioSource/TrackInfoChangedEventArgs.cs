using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Event arguments for the <see cref="IAudioSource.TrackInfoChanged"/> event.
    /// </summary>
    [Serializable]
    public sealed class TrackInfoChangedEventArgs : EventArgs, ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackInfoChangedEventArgs"/> class.
        /// </summary>
        public TrackInfoChangedEventArgs()
        {
            // Empty
        }

        private TrackInfoChangedEventArgs(SerializationInfo info, StreamingContext context)
        {
            TrackName = info.GetString(nameof(TrackName));
            Artist = info.GetString(nameof(Artist));
            AlbumArt = ByteArrayToImage((byte[])info.GetValue(nameof(AlbumArt), typeof(byte[])));
            Album = info.GetString(nameof(Album));
            TrackLength = (TimeSpan)info.GetValue(nameof(TrackLength), typeof(TimeSpan));
        }

        /// <summary>
        /// Gets or sets the track Name
        /// </summary>
        /// <value>The name of the track.</value>
        public string TrackName { get; set; }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        /// <value>The name of the artist.</value>
        public string Artist { get; set; }

        /// <summary>
        /// Gets or sets the album art image. If <see langword="null"/>, a placeholder will be used.
        /// </summary>
        /// <value>The album art.</value>
        public Image AlbumArt { get; set; }

        /// <summary>
        /// Gets or sets the album name.
        /// </summary>
        /// <value>The name of the album.</value>
        public string Album { get; set; }

        /// <summary>
        /// Gets or sets the length of the track
        /// </summary>
        /// <value>The length of the track.</value>
        public TimeSpan TrackLength { get; set; }

        /// <inheritdoc/>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(TrackName), TrackName);
            info.AddValue(nameof(Artist), Artist);
            info.AddValue(nameof(AlbumArt), ImageToByteArray(AlbumArt), typeof(byte[]));
            info.AddValue(nameof(Album), Album);
            info.AddValue(nameof(TrackLength), TrackLength, typeof(TimeSpan));
        }

        private static byte[] ImageToByteArray(Image image)
        {
            if (image == null)
            {
                return null;
            }

            var copy = new Bitmap(image);
            return new ImageConverter().ConvertTo(copy, typeof(byte[])) as byte[];
        }

        private static Image ByteArrayToImage(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            return new ImageConverter().ConvertFrom(data) as Image;
        }
    }
}
