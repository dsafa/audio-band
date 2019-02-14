using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AudioBand.Views.Wpf.ValueConverters
{
    /// <summary>
    /// Converter from <see langword="bool"/> to <see cref="Visibility"/>.
    /// </summary>
    internal class BoolToVisibilityConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
