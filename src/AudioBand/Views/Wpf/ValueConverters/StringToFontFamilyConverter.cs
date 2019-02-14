using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AudioBand.Views.Wpf.ValueConverters
{
    /// <summary>
    /// Converts a string to a <see cref="FontFamily"/>.
    /// </summary>
    internal class StringToFontFamilyConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var fontFamilyString = (string)value;
            return new FontFamily(fontFamilyString);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var fontFamily = (FontFamily)value;
            return fontFamily.Source;
        }
    }
}
