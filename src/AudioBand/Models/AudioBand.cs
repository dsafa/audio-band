namespace AudioBand.Models
{
    /// <summary>
    /// The model for the toolbar.
    /// </summary>
    public class AudioBand : ModelBase
    {
        private int _width = 250;
        private int _height = 30;

        /// <summary>
        /// Gets or sets the width of the toolbar.
        /// </summary>
        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        /// <summary>
        /// Gets or sets the height of the toolbar.
        /// </summary>
        public int Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }
    }
}
