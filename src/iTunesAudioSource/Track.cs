using System;
using System.Drawing;

namespace iTunesAudioSource
{
    public class Track
    {
        public string Name { get; set; }

        public string Album { get; set; }

        public Image Artwork { get; set; }

        public string Artist { get; set; }

        public TimeSpan Length { get; set; }
    }
}
