namespace AudioBand.Models
{
    /// <summary>
    /// Model for the previous button.
    /// </summary>
    public class PreviousButton : PlaybackButtonBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreviousButton"/> class.
        /// </summary>
        public PreviousButton()
        {
            ImagePath = "";
            IsVisible = true;
            Width = 73;
            Height = 12;
            XPosition = 30;
            YPosition = 15;
            Text = "";
        }
    }
}
