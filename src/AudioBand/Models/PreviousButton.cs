namespace AudioBand.Models
{
    /// <summary>
    /// Model for the previous button.
    /// </summary>
    public class PreviousButton : ButtonModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreviousButton"/> class.
        /// </summary>
        public PreviousButton()
        {
            IsVisible = true;
            Width = 40;
            Height = 15;
            XPosition = 330;
            YPosition = 3;
        }

        /// <summary>
        /// Gets or sets the content for the button.
        /// </summary>
        public ButtonContent Content { get; set; } = new ButtonContent
        {
            Text = "",
        };
    }
}
