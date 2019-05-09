using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Converts string using the string format provided.
    /// </summary>
    [ValueConversion(typeof(object), typeof(string), ParameterType = typeof(string))]
    public class StringFormatConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return null;
            }

            return string.Format(culture, (string)parameter, value);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
