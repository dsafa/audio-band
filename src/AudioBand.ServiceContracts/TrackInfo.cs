using AudioBand.AudioSource;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;

namespace AudioBand.ServiceContracts
{
    /// <summary>
    /// Serializable class for <see cref="TrackInfoChangedEventArgs"/>.
    /// </summary>
    [DataContract]
    public class TrackInfo
    {
        /// <summary>
        /// Matching data member for <see cref="TrackInfoChangedEventArgs.TrackName"/>.
        /// </summary>
        [DataMember]
        public string TrackName { get; set; }

        /// <summary>
        /// Matching data member for <see cref="TrackInfoChangedEventArgs.Artist"/>.
        /// </summary>
        [DataMember]
        public string Artist { get; set; }

        /// <summary>
        /// Matching data member for <see cref="TrackInfoChangedEventArgs.AlbumArt"/> exposed as a byte[].
        /// </summary>
        [DataMember]
        public byte[] AlbumArt { get; set; }

        /// <summary>
        /// Matching data member for <see cref="TrackInfoChangedEventArgs.Album"/>.
        /// </summary>
        [DataMember]
        public string Album { get; set; }

        /// <summary>
        /// Matching data member for <see cref="TrackInfoChangedEventArgs.TrackLength"/>.
        /// </summary>
        [DataMember]
        public TimeSpan TrackLength { get; set; }

        public static explicit operator TrackInfo(TrackInfoChangedEventArgs e)
        {
            return new TrackInfo
            {
                TrackName = e.TrackName,
                Artist = e.Artist,
                AlbumArt = ImageToByteArray(e.AlbumArt),
                Album = e.Album,
                TrackLength = e.TrackLength
            };
        }

        public static explicit operator TrackInfoChangedEventArgs(TrackInfo trackInfo)
        {
            return new TrackInfoChangedEventArgs
            {
                TrackName = trackInfo.TrackName,
                Artist = trackInfo.Artist,
                AlbumArt = ByteArrayToImage(trackInfo.AlbumArt),
                Album = trackInfo.Album,
                TrackLength = trackInfo.TrackLength
            };
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
