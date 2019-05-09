using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Converts a string to a <see cref="Visibility"/> value.
    /// </summary>
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class StringToVisibilityConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var collapse = parameter == null ? true : System.Convert.ToBoolean(parameter);
            return string.IsNullOrEmpty((string)value) ? (collapse ? Visibility.Collapsed : Visibility.Hidden) : Visibility.Visible;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
