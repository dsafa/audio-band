using System;
using System.Drawing;

namespace AudioBand.Connector
{
    public class AlbumArtChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Album art image. If null, a placeholder will be used
        /// </summary>
        public Image AlbumArt { get; set; }
    }
}
