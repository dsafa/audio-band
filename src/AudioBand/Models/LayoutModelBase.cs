namespace AudioBand.Models
{
    /// <summary>
    /// Base model that contains support for layout.
    /// </summary>
    public class LayoutModelBase : ModelBase
    {
        private bool _isVisible = true;
        private double _width;
        private double _height;
        private double _xPosition;
        private double _yPosition;
        private PositionAnchor _positionAnchor = PositionAnchor.TopLeft;

        /// <summary>
        /// Gets or sets a value indicating whether it is visible.
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        /// <summary>
        /// Gets or sets the x position.
        /// </summary>
        public double XPosition
        {
            get => _xPosition;
            set => SetProperty(ref _xPosition, value);
        }

        /// <summary>
        /// Gets or sets the y position.
        /// </summary>
        public double YPosition
        {
            get => _yPosition;
            set => SetProperty(ref _yPosition, value);
        }

        /// <summary>
        /// Gets or sets the positioning.
        /// </summary>
        public PositionAnchor Anchor
        {
            get => _positionAnchor;
            set => SetProperty(ref _positionAnchor, value);
        }
    }
}
