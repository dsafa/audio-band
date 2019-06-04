using System.Collections.Generic;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A <see cref="TextPlaceholder"/> with the value based on the songs total time.
    /// </summary>
    public class SongLengthPlaceholder : TextPlaceholder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SongLengthPlaceholder"/> class.
        /// </summary>
        /// <param name="parameters">The placeholder parameters.</param>
        public SongLengthPlaceholder(IEnumerable<TextPlaceholderParameter> parameters)
            : base(parameters)
        {
        }

        /// <inheritdoc />
        public override string GetText()
        {
            return "length";
        }
    }
}
