using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AudioBand.UI
{
    /// <summary>
    /// Converts a string to a <see cref="FontFamily"/>.
    /// </summary>
    [ValueConversion(typeof(string), typeof(FontFamily))]
    public class StringToFontFamilyConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ValueConverterHelper.IsValid<string>(value))
            {
                return DependencyProperty.UnsetValue;
            }

            return new FontFamily((string)value);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ValueConverterHelper.IsValid<FontFamily>(value))
            {
                return DependencyProperty.UnsetValue;
            }

            var fontFamily = (FontFamily)value;
            return fontFamily.Source;
        }
    }
}
