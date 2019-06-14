namespace AudioBand.Models
{
    /// <summary>
    /// Base model that contains support for layout.
    /// </summary>
    /// <remarks>This should probably be a has-a relationship instead of inheriting from this class.</remarks>
    public class LayoutModelBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether it is visible.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the x position.
        /// </summary>
        public double XPosition { get; set; }

        /// <summary>
        /// Gets or sets the y position.
        /// </summary>
        public double YPosition { get; set; }

        /// <summary>
        /// Gets or sets the positioning.
        /// </summary>
        public PositionAnchor Anchor { get; set; } = PositionAnchor.TopLeft;
    }
}
