using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioBand.UI
{
    /// <summary>
    /// Inverses a boolean.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBoolConverter : IValueConverter
    {
        /// <summary>
        /// Returns the inverse of the provided boolean.
        /// </summary>
        /// <param name="value">The value of the bool.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">Ignorable.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The inverse of the boolean.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
