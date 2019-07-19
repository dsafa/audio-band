using System.Windows.Media;

namespace AudioBand.Models
{
    /// <summary>
    /// The model for the toolbar.
    /// </summary>
    public class GeneralSettings
    {
        /// <summary>
        /// Gets or sets the width of the toolbar.
        /// </summary>
        public double Width { get; set; } = 500;

        /// <summary>
        /// Gets or sets the height of the toolbar.
        /// </summary>
        public double Height { get; set; } = 30;

        /// <summary>
        /// Gets or sets the background color of the toolbar.
        /// </summary>
        public Color BackgroundColor { get; set; } = Colors.Transparent;
    }
}
