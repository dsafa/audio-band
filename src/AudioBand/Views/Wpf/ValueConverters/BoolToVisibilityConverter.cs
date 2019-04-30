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
        /// <summary>
        /// Gets or sets a value indicating whether to collapse when hidden.
        /// </summary>
        public bool Collapse { get; set; } = true;

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : (Collapse ? Visibility.Collapsed : Visibility.Hidden);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
