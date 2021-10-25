using AudioBand.Extensions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AudioBand.UI
{
    /// <summary>
    /// Convert a <see cref="Color"/> to a string representation.
    /// </summary>
    [ValueConversion(typeof(Color), typeof(string))]
    public class ColorToNameConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ValueConverterHelper.IsValid<Color>(value))
            {
                return DependencyProperty.UnsetValue;
            }

            return ((Color)value).GetColorName();
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ValueConverterHelper.IsValid<string>(value))
            {
                return DependencyProperty.UnsetValue;
            }

            return ColorConverter.ConvertFromString((string)value);
        }
    }
}
