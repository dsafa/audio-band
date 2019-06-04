using System.Collections.Generic;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A <see cref="TextPlaceholder"/> with a value based on the current song name.
    /// </summary>
    public class SongNamePlaceholder : TextPlaceholder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SongNamePlaceholder"/> class.
        /// </summary>
        /// <param name="parameters">The placeholder parameters.</param>
        public SongNamePlaceholder(IEnumerable<TextPlaceholderParameter> parameters)
            : base(parameters)
        {
        }

        /// <inheritdoc />
        public override string GetText()
        {
            return "song name";
        }
    }
}
