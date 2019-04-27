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
            Width = 73;
            Height = 12;
            XPosition = 176;
            YPosition = 15;
        }
    }
}
