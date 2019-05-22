namespace AudioBand.Models
{
    /// <summary>
    /// Model for the next button.
    /// </summary>
    public class NextButton : ButtonModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NextButton"/> class.
        /// </summary>
        public NextButton()
        {
            IsVisible = true;
            Width = 40;
            Height = 15;
            XPosition = 410;
            YPosition = 3;
        }

        /// <summary>
        /// Gets or sets the content for the button.
        /// </summary>
        public ButtonContent Content { get; set; } = new ButtonContent
        {
            Text = "",
        };
    }
}
