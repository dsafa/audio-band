using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AudioBand.Views.Wpf.ValueConverters
{
    /// <summary>
    /// Converter from <see cref="Color"/> to <see cref="Brush"/>.
    /// </summary>
    internal class ColorToBrushConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((SolidColorBrush)value).Color;
        }
    }
}
