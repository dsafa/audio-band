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
        /// <summary>
        /// Uses a double as the width of a corner radius.
        /// </summary>
        /// <param name="value">The double to convert.</param>
        /// <param name="targetType">The type of <see cref="CornerRadius"/>.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>A uniform corner radius with each side equal to value / 2.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new CornerRadius(System.Convert.ToDouble(value) / 2);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
