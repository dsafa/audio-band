namespace AudioBand.Models
{
    /// <summary>
    /// Model for the next button.
    /// </summary>
    public class NextButton : PlaybackButtonBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NextButton"/> class.
        /// </summary>
        public NextButton()
        {
            ImagePath = "";
            IsVisible = true;
            Width = 40;
            Height = 15;
            XPosition = 410;
            YPosition = 3;
            Text = "";
        }
    }
}
