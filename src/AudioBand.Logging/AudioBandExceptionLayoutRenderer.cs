using System;
using System.Text;
using NLog.LayoutRenderers;

namespace AudioBand.Logging
{
    /// <summary>
    /// Layout renderer for exceptions that call demystify.
    /// </summary>
    public class AudioBandExceptionLayoutRenderer : ExceptionLayoutRenderer
    {
        /// <summary>
        /// Appends the result of calling ToString() on an Exception to the specified <see cref="T:System.Text.StringBuilder" />.
        /// </summary>
        /// <param name="sb">The <see cref="T:System.Text.StringBuilder" /> to append the rendered data to.</param>
        /// <param name="ex">The Exception whose call to ToString() should be appended.</param>
        protected override void AppendToString(StringBuilder sb, Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            sb.AppendLine("---START OF EXCEPTION---");
            sb.AppendLine(ex.ToString());
            sb.AppendLine("---END OF EXCEPTION");
        }
    }
}
