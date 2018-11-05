using System;
using System.Drawing;

namespace AudioBand.Models
{
    internal class Track : ModelBase
    {
        public bool IsPlaying { get; set; }
        public TimeSpan TrackProgress { get; set; }
        public TimeSpan TrackLength { get; set; }
        public string TrackName { get; set; }
        public string Artist { get; set; }
        public string AlbumName { get; set; }
        public Image AlbumArt { get; set; }
        public Image PlaceholderImage { get; set; }
    }
}
