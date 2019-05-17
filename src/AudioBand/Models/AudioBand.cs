namespace AudioBand.Models
{
    /// <summary>
    /// The model for the toolbar.
    /// </summary>
    public class AudioBand : ModelBase
    {
        private double _width = 500;
        private double _height = 30;

        /// <summary>
        /// Gets or sets the width of the toolbar.
        /// </summary>
        public double Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        /// <summary>
        /// Gets or sets the height of the toolbar.
        /// </summary>
        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }
    }
}
