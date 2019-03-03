using NLog.LayoutRenderers;
using System;
using System.Diagnostics;
using System.Text;

namespace AudioBand.Logging
{
    public class AudioBandExceptionLayoutRenderer : ExceptionLayoutRenderer
    {
        protected override void AppendToString(StringBuilder sb, Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            sb.AppendLine();
            sb.AppendLine("---START OF EXCEPTION---");
            sb.AppendLine(ex.ToStringDemystified());
            sb.AppendLine("---END OF EXCEPTION---");
        }
    }
}
