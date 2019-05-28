using System;

namespace AudioBand.Models
{
    /// <summary>
    /// Specifies the anchor behavior of the position of controls.
    /// </summary>
    [Flags]
    public enum PositionAnchor
    {
        /// <summary>
        /// Anchor to the top.
        /// </summary>
        Top = 1,

        /// <summary>
        /// Anchor to the right.
        /// </summary>
        Right = 1 << 1,

        /// <summary>
        /// Anchor to the bottom.
        /// </summary>
        Bottom = 1 << 2,

        /// <summary>
        /// Anchor to the left.
        /// </summary>
        Left = 1 << 3,

        /// <summary>
        /// Anchor to top left.
        /// </summary>
        TopLeft = Top | Left,

        /// <summary>
        /// Anchor to top right.
        /// </summary>
        TopRight = Top | Right,

        /// <summary>
        /// Anchor to bottom right.
        /// </summary>
        BottomRight = Bottom | Right,

        /// <summary>
        /// Anchor to bottom left.
        /// </summary>
        BottomLeft = Bottom | Left,
    }
}
