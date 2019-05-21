using System.ComponentModel;

namespace AudioBand.Models
{
    /// <summary>
    /// Configures the fade effect for text.
    /// </summary>
    public enum TextFadeEffect
    {
        /// <summary>
        /// Never use the fade effect.
        /// </summary>
        [Description("Never")]
        Never,

        /// <summary>
        /// Always use the fade effect.
        /// </summary>
        [Description("Always")]
        Always,

        /// <summary>
        /// Only fade if the text is scrolling.
        /// </summary>
        [Description("Only when scrolling")]
        OnlyWhenScrolling,
    }
}
