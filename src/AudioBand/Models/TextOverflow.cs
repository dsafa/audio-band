using System.ComponentModel;

namespace AudioBand.Models
{
    /// <summary>
    /// Describes the behavior of text when overflowing.
    /// </summary>
    public enum TextOverflow
    {
        /// <summary>
        /// Do nothing, the text will be truncated.
        /// </summary>
        [Description("Truncate")]
        Truncate,

        /// <summary>
        /// Scroll the text.
        /// </summary>
        [Description("Scroll the text")]
        Scroll,
    }
}
