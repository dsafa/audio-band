namespace AudioBand.Models
{
    /// <summary>
    /// Model for the album art.
    /// </summary>
    public class AlbumArt : LayoutModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumArt"/> class.
        /// </summary>
        public AlbumArt()
        {
            Width = 30;
            Height = 30;
            XPosition = 245;
            YPosition = 0;
        }

        /// <summary>
        /// Gets or sets the path of the placeholder image.
        /// </summary>
        public string PlaceholderPath { get; set; } = "";
    }
}
