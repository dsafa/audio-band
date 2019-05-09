using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Converts a double to a corner radius.
    /// </summary>
    [ValueConversion(typeof(double), typeof(CornerRadius))]
    public class DoubleToCornerRadiusConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new CornerRadius(System.Convert.ToDouble(value) / 2);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
