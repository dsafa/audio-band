using System.Drawing;

namespace AudioBand.Connector
{
    public class TrackChangedEventArgs
    {
        public string TrackName { get; set; }
        public string Artist { get; set; }
        public Image AlbumArt { get; set; }
    }
}
