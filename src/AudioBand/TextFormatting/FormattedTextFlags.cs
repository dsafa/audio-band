using System;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// Flags for what the text contains.
    /// </summary>
    [Flags]
    public enum FormattedTextFlags
    {
        /// <summary>
        /// Normal text
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Text is bolded.
        /// </summary>
        Bold = 1,

        /// <summary>
        /// Text is italicised.
        /// </summary>
        Italic = 1 << 1,

        /// <summary>
        /// Text is underlined.
        /// </summary>
        Underline = 1 << 2,
    }
}
