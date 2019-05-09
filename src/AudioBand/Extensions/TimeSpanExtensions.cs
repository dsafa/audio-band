using System;

namespace AudioBand.Extensions
{
    /// <summary>
    /// Extensions for time span.
    /// </summary>
    public static class TimeSpanExtensions
    {
        private const string TimeFormat = @"m\:ss";
        private const string TimeFormatHour = @"h\:mm\:ss";

        /// <summary>
        /// Gets the time as a string.
        /// </summary>
        /// <param name="time">The time to format.</param>
        /// <returns>The string.</returns>
        public static string Format(this TimeSpan time)
        {
            if (time.Hours > 0)
            {
                return time.ToString(TimeFormatHour);
            }
            else
            {
                return time.ToString(TimeFormat);
            }
        }
    }
}
