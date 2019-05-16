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
            Width = 40;
            Height = 15;
            XPosition = 330;
            YPosition = 3;
            Text = "";
        }
    }
}
