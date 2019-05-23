namespace AudioBand.Models
{
    /// <summary>
    /// Model for the play/pause button.
    /// </summary>
    public class PlayPauseButton : ButtonModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayPauseButton"/> class.
        /// </summary>
        public PlayPauseButton()
        {
            XPosition = 370;
            YPosition = 3;
            Width = 40;
            Height = 15;
        }

        /// <summary>
        /// Gets or sets the button content for the play state.
        /// </summary>
        public ButtonContent PlayContent { get; set; } = new ButtonContent
        {
            Text = "",
        };

        /// <summary>
        /// Gets or sets the button content for the pause state.
        /// </summary>
        public ButtonContent PauseContent { get; set; } = new ButtonContent
        {
            Text = "",
        };
    }
}
