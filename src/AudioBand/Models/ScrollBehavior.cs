using System.ComponentModel;

namespace AudioBand.Models
{
    /// <summary>
    /// Describes the behavior for scrolling text.
    /// </summary>
    public enum ScrollBehavior
    {
        /// <summary>
        /// The text will always scroll.
        /// </summary>
        [Description("Always scroll")]
        Always,

        /// <summary>
        /// The text will only start scrolling if a track is playing.
        /// </summary>
        [Description("Only when a track is playing")]
        OnlyWhenTrackPlaying,

        /// <summary>
        /// The text will only start scrolling when the mouse is over it.
        /// </summary>
        [Description("Only when mouse is over")]
        OnlyWhenMouseOver,
    }
}
