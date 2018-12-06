using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioBand.Views.Wpf.ValueConverters
{
    /// <summary>
    /// Converts a string to a bool.
    /// </summary>
    internal class EmptyStringToBoolConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // True if string not empty or null.
            if (value is string s)
            {
                return !string.IsNullOrEmpty(s);
            }

            return false;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
