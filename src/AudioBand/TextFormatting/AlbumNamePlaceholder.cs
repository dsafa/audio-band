using System.Collections.Generic;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A <see cref="TextPlaceholder"/> with the value based on the album name.
    /// </summary>
    public class AlbumNamePlaceholder : TextPlaceholder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumNamePlaceholder"/> class.
        /// </summary>
        /// <param name="parameters">The placeholder parameters.</param>
        public AlbumNamePlaceholder(IEnumerable<TextPlaceholderParameter> parameters)
            : base(parameters)
        {
        }

        /// <inheritdoc />
        public override string GetText()
        {
            return "album name";
        }
    }
}
