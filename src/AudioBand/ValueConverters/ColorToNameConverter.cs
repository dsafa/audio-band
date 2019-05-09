using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using AudioBand.Extensions;

namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Convert a string representation to a <see cref="Color"/>.
    /// </summary>
    [ValueConversion(typeof(Color), typeof(string))]
    public class ColorToNameConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Color)value).GetColorName();
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ColorConverter.ConvertFromString((string)value);
        }
    }
}
