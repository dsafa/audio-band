using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AudioBand.UI
{
    /// <summary>
    /// Converter from <see cref="Color"/> to <see cref="Brush"/>.
    /// </summary>
    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    public class ColorToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="Color"/> to a <see cref="SolidColorBrush"/>.
        /// </summary>
        /// <param name="value">The color.</param>
        /// <param name="targetType">The type of <see cref="Brush"/>.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The current culture.</param>
        /// <returns>A <see cref="SolidColorBrush"/> representing the color.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ValueConverterHelper.IsValid<Color>(value))
            {
                return DependencyProperty.UnsetValue;
            }

            return new SolidColorBrush((Color)value);
        }

        /// <summary>
        /// Converts a <see cref="SolidColorBrush"/> to a <see cref="Color"/>.
        /// </summary>
        /// <param name="value">The brush.</param>
        /// <param name="targetType">The type of <see cref="Color"/>.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The current culture.</param>
        /// <returns>The color component of a <see cref="SolidColorBrush"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ValueConverterHelper.IsValid<SolidColorBrush>(value))
            {
                return DependencyProperty.UnsetValue;
            }

            return ((SolidColorBrush)value).Color;
        }
    }
}
