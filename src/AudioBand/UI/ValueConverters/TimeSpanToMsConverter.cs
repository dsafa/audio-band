using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioBand.UI
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
            if (!ValueConverterHelper.IsValid<TimeSpan>(value))
            {
                return 0.0;
            }

            return ((TimeSpan)value).TotalMilliseconds;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ValueConverterHelper.IsValid<double>(value))
            {
                return TimeSpan.Zero;
            }

            return TimeSpan.FromMilliseconds((double)value);
        }
    }
}
