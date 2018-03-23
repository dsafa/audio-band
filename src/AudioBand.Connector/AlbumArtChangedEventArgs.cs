using System;
using System.Drawing;

namespace AudioBand.Connector
{
    public class AlbumArtChangedEventArgs : EventArgs
    {
        public Image AlbumArt { get; set; }
    }
}
