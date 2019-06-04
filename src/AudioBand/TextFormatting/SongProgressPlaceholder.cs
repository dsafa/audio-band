using System.Collections.Generic;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A <see cref="TextPlaceholder"/> with a value based on the current song progress.
    /// </summary>
    public class SongProgressPlaceholder : TextPlaceholder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SongProgressPlaceholder"/> class.
        /// </summary>
        /// <param name="parameters">The placeholder parameters.</param>
        public SongProgressPlaceholder(IEnumerable<TextPlaceholderParameter> parameters)
            : base(parameters)
        {
        }

        /// <inheritdoc />
        public override string GetText()
        {
            return "time";
        }
    }
}
