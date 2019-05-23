using System.Windows.Media;

namespace AudioBand.Models
{
    /// <summary>
    /// Button model for the shuffle button.
    /// </summary>
    public class ShuffleModeButton : ButtonModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShuffleModeButton"/> class.
        /// </summary>
        public ShuffleModeButton()
        {
            Width = 40;
            Height = 15;
            XPosition = 290;
            YPosition = 3;
        }

        /// <summary>
        /// Gets or sets the shuffle off content.
        /// </summary>
        public ButtonContent ShuffleOffContent { get; set; } = new ButtonContent
        {
            Text = "",
            TextColor = Colors.DimGray,
            HoveredTextColor = Colors.White,
            ClickedTextColor = Colors.Gray,
        };

        /// <summary>
        /// Gets or sets the shuffle on content.
        /// </summary>
        public ButtonContent ShuffleOnContent { get; set; } = new ButtonContent
        {
            Text = "",
            TextColor = Colors.DodgerBlue,
            HoveredTextColor = Colors.CornflowerBlue,
            ClickedTextColor = Colors.RoyalBlue,
        };
    }
}
