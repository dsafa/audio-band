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
        /// Displays the artist
        /// </summary>
        Artist = 1,

        /// <summary>
        /// Displays the song.
        /// </summary>
        Song = 2,

        /// <summary>
        /// Displays the album
        /// </summary>
        Album = 4,

        /// <summary>
        /// Displays the current time.
        /// </summary>
        CurrentTime = 8,

        /// <summary>
        /// Displays the song length.
        /// </summary>
        SongLength = 16,

        /// <summary>
        /// Text is colored.
        /// </summary>
        Colored = 32,

        /// <summary>
        /// Text is bolded.
        /// </summary>
        Bold = 64,

        /// <summary>
        /// Text is italicised.
        /// </summary>
        Italic = 128,

        /// <summary>
        /// Text is underlined.
        /// </summary>
        Underline = 256,
    }
}
