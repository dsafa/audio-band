using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using AudioBand.Extensions;

namespace AudioBand.Views.Wpf.ValueConverters
{
    /// <summary>
    /// Convert a string representation to a <see cref="Color"/>.
    /// </summary>
    internal class StringToColorConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ColorConverter.ConvertFromString((string)value);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Color)value).GetColorName();
        }
    }
}
