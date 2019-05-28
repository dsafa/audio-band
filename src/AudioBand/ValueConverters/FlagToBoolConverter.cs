using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Converts enum flag to bool.
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class FlagToBoolConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue || parameter == null)
            {
                return false;
            }

            return ((Enum)value).HasFlag((Enum)parameter);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
