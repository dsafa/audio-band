using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AudioBand.UI
{
    /// <summary>
    /// Converts an object to its type.
    /// </summary>
    [ValueConversion(typeof(object), typeof(Type))]
    public class ObjectToTypeConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            return value.GetType();
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
