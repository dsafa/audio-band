using System;
using System.Collections.Generic;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A placeholder text that has values based on the current song.
    /// </summary>
    public abstract class TextPlaceholder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextPlaceholder"/> class.
        /// </summary>
        /// <param name="parameters">The parameters passed to the text format.</param>
        protected TextPlaceholder(IEnumerable<TextPlaceholderParameter> parameters)
        {
            // TODO parameters
        }

        /// <summary>
        /// Occurs when the placeholders text has changed.
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// Gets the current text value for the placeholder.
        /// </summary>
        /// <returns>The value.</returns>
        public abstract string GetText();

        /// <summary>
        /// Raises the <see cref="TextChanged"/> event.
        /// </summary>
        protected void RaiseTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the parameter from the name.
        /// </summary>
        /// <param name="parameterName">The parameter name.</param>
        /// <returns>The value of the parameter or null if not passed in.</returns>
        protected string GetParameter(string parameterName)
        {
            return null;
        }
    }
}
