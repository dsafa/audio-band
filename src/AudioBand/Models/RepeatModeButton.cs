using System.Windows.Media;

namespace AudioBand.Models
{
    /// <summary>
    /// Model for the repeat mode button.
    /// </summary>
    public class RepeatModeButton : ButtonModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatModeButton"/> class.
        /// </summary>
        public RepeatModeButton()
        {
            Width = 40;
            Height = 15;
            XPosition = 450;
            YPosition = 3;
        }

        /// <summary>
        /// Gets or sets the repeat off content.
        /// </summary>
        public ButtonContent RepeatOffContent { get; set; } = new ButtonContent
        {
            Text = "",
            TextColor = Colors.DimGray,
            HoveredTextColor = Colors.White,
            ClickedTextColor = Colors.Gray,
        };

        /// <summary>
        /// Gets or sets the repeat context content.
        /// </summary>
        public ButtonContent RepeatContextContent { get; set; } = new ButtonContent
        {
            Text = "",
            TextColor = Colors.DodgerBlue,
            HoveredTextColor = Colors.CornflowerBlue,
            ClickedTextColor = Colors.RoyalBlue,
        };

        /// <summary>
        /// Gets or sets the repeat track content.
        /// </summary>
        public ButtonContent RepeatTrackContent { get; set; } = new ButtonContent
        {
            Text = "",
            TextColor = Colors.DodgerBlue,
            HoveredTextColor = Colors.CornflowerBlue,
            ClickedTextColor = Colors.RoyalBlue,
        };
    }
}
