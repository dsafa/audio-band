using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Converts timespan to milliseconds.
    /// </summary>
    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class TimeSpanToMsConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((TimeSpan)value).TotalMilliseconds;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeSpan.FromMilliseconds((double)value);
        }
    }
}
