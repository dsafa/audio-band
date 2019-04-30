using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioBand.Views.Wpf.ValueConverters
{
    /// <summary>
    /// Converts and object to a type.
    /// </summary>
    [ValueConversion(typeof(object), typeof(Type))]
    public class ObjectToTypeConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.GetType();
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
