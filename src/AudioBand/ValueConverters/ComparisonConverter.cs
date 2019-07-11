using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Takes two objects and converts them to a bool if they are equal.
    /// </summary>
    public class ComparisonConverter : IMultiValueConverter
    {
        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values[0] == values[1];
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
