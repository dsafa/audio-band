using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AudioBand.UI
{
    /// <summary>
    /// Converts enum flag to bool.
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(bool), ParameterType = typeof(Enum))]
    public class FlagToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Converts an enum flag to a bool value.
        /// </summary>
        /// <param name="value">The value of the flag.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The flag to check if set.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>True if the flag specified by <paramref name="parameter"/> is set.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return false;
            }

            return ((Enum)value).HasFlag((Enum)parameter);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
