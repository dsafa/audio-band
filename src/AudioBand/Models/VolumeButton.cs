using System.Windows.Media;

namespace AudioBand.Models
{
    /// <summary>
    /// Model for the volume button.
    /// </summary>
    public class VolumeButton : ButtonModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeButton"/> class.
        /// </summary>
        public VolumeButton()
        {
            XPosition = 251;
            YPosition = 6;

            Width = 18;
            Height = 18;

            PopupWidth = 36;
            PopupHeight = 160;

            XPopupOffset = -1;
            YPopupOffset = -11;
        }

        /// <summary>
        /// Gets or sets the Popup's width.
        /// </summary>
        public double PopupWidth { get; set; }

        /// <summary>
        /// Gets or sets the Popup's height.
        /// </summary>
        public double PopupHeight { get; set; }

        /// <summary>
        /// Gets or sets the Popup's X offset.
        /// </summary>
        public double XPopupOffset { get; set; }

        /// <summary>
        /// Gets or sets the Popup's Y offset.
        /// </summary>
        public double YPopupOffset { get; set; }

        /// <summary>
        /// Gets or sets the button with no volume.
        /// </summary>
        public ButtonContent NoVolumeContent { get; set; } = new ButtonContent
        {
            Text = "",
        };

        /// <summary>
        /// Gets or sets the button with low volume.
        /// </summary>
        public ButtonContent LowVolumeContent { get; set; } = new ButtonContent
        {
            Text = "",
        };

        /// <summary>
        /// Gets or sets the button with mid volume.
        /// </summary>
        public ButtonContent MidVolumeContent { get; set; } = new ButtonContent
        {
            Text = "",
        };

        /// <summary>
        /// Gets or sets the button with high volume.
        /// </summary>
        public ButtonContent HighVolumeContent { get; set; } = new ButtonContent
        {
            Text = "",
        };

        /// <summary>
        /// Gets or sets the ForegroundColor.
        /// </summary>
        public Color VolumeBarForegroundColor { get;  set; } = Colors.DodgerBlue;

        /// <summary>
        /// Gets or sets the BackgroundColor.
        /// </summary>
        public Color VolumeBarBackgroundColor { get; set; } = Colors.DimGray;
    }
}
