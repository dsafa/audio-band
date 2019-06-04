using System.Collections.Generic;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A <see cref="TextPlaceholder"/> with the value based on the song artist.
    /// </summary>
    public class SongArtistPlaceholder : TextPlaceholder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SongArtistPlaceholder"/> class.
        /// </summary>
        /// <param name="parameters">The placeholder parameter.</param>
        public SongArtistPlaceholder(IEnumerable<TextPlaceholderParameter> parameters)
            : base(parameters)
        {
        }

        /// <inheritdoc />
        public override string GetText()
        {
            return "artist";
        }
    }
}
