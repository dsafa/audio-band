using System.Windows.Media;

namespace AudioBand.Models
{
    /// <summary>
    /// Base model for buttons.
    /// </summary>
    public class ButtonModelBase : LayoutModelBase
    {
        private Color _backgroundColor = Colors.Transparent;
        private Color _hoveredBackgroundColor = Color.FromArgb(25, 255, 255, 255);
        private Color _clickedBackgroundColor = Color.FromArgb(15, 255, 255, 255);

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }

        /// <summary>
        /// Gets or sets the background color when hovered.
        /// </summary>
        public Color HoveredBackgroundColor
        {
            get => _hoveredBackgroundColor;
            set => SetProperty(ref _hoveredBackgroundColor, value);
        }

        /// <summary>
        /// Gets or sets the background color when clicked.
        /// </summary>
        public Color ClickedBackgroundColor
        {
            get => _clickedBackgroundColor;
            set => SetProperty(ref _clickedBackgroundColor, value);
        }
    }
}
