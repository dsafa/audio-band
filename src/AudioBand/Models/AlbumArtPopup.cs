namespace AudioBand.Models
{
    /// <summary>
    /// Model for the album art popup.
    /// </summary>
    public class AlbumArtPopup
    {
        /// <summary>
        /// Gets or sets a value indicating whether the album art popup is visible.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Gets or sets the width of the album art popup.
        /// </summary>
        public double Width { get; set; } = 250;

        /// <summary>
        /// Gets or sets the height of the album art popup.
        /// </summary>
        public double Height { get; set; } = 250;

        /// <summary>
        /// Gets or sets the x position of the album art popup.
        /// </summary>
        public double XPosition { get; set; } = -110;

        /// <summary>
        /// Gets or sets the margin between the album art popup and the taskbar.
        /// </summary>
        public double Margin { get; set; } = 4;
    }
}
