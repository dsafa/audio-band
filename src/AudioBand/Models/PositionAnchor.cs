using System;
using System.ComponentModel;
using AudioBand.Extensions;

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
        [DescriptorIgnore]
        Top = 1,

        /// <summary>
        /// Anchor to the right.
        /// </summary>
        [DescriptorIgnore]
        Right = 1 << 1,

        /// <summary>
        /// Anchor to the bottom.
        /// </summary>
        [DescriptorIgnore]
        Bottom = 1 << 2,

        /// <summary>
        /// Anchor to the left.
        /// </summary>
        [DescriptorIgnore]
        Left = 1 << 3,

        /// <summary>
        /// Anchor to top left.
        /// </summary>
        [Description("Relative to top left")]
        TopLeft = Top | Left,

        /// <summary>
        /// Anchor to top right.
        /// </summary>
        [Description("Relative to top right")]
        TopRight = Top | Right,

        /// <summary>
        /// Anchor to bottom right.
        /// </summary>
        [Description("Relative to bottom right")]
        BottomRight = Bottom | Right,

        /// <summary>
        /// Anchor to bottom left.
        /// </summary>
        [Description("Relative to bottom left")]
        BottomLeft = Bottom | Left,
    }
}
