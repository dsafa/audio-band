using AudioBand.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AudioBand.UI
{
    /// <summary>
    /// Converts custom <see cref="CustomLabel.TextAlignment"/>.
    /// </summary>
    [ValueConversion(typeof(CustomLabel.TextAlignment), typeof(TextAlignment))]
    public class TextAlignmentConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ValueConverterHelper.IsValid<CustomLabel.TextAlignment>(value))
            {
                return DependencyProperty.UnsetValue;
            }

            switch ((CustomLabel.TextAlignment)value)
            {
                case CustomLabel.TextAlignment.Center:
                    return TextAlignment.Center;
                case CustomLabel.TextAlignment.Left:
                    return TextAlignment.Left;
                case CustomLabel.TextAlignment.Right:
                    return TextAlignment.Right;
                default:
                    return TextAlignment.Center;
            }
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
