using System.Windows.Media;

namespace AudioBand.Models
{
    /// <summary>
    /// Model for the progress bar.
    /// </summary>
    public class ProgressBar : LayoutModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class.
        /// </summary>
        public ProgressBar()
        {
            Width = 130;
            Height = 4;
            XPosition = 325;
            YPosition = 22;
        }

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        public Color ForegroundColor { get; set; } = Colors.DodgerBlue;

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public Color BackgroundColor { get; set; } = Colors.DimGray;

        /// <summary>
        /// Gets or sets the hover color.
        /// </summary>
        public Color HoverColor { get; set; } = Colors.DeepSkyBlue;
    }
}
