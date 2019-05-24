using System;
using System.Globalization;
using System.Windows;
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
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return 0;
            }

            return ((TimeSpan)value).TotalMilliseconds;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return TimeSpan.Zero;
            }

            return TimeSpan.FromMilliseconds((double)value);
        }
    }
}
