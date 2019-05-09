using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Converter from a number to a value for the numericupdown.
    /// </summary>
    /// <remarks>
    /// There are issues binding a number to the value property of a numericupdown control (double?).
    /// Example message: Cannot convert from type 'UInt32' to type 'System.Nullable`1[System.Double].
    /// </remarks>
    public class CoerceNumberConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Number to double?
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType == null)
            {
                return 0;
            }

            if (value == null)
            {
                return 0;
            }

            if (!value.GetType().IsPrimitive)
            {
                return 0;
            }

            return System.Convert.ChangeType(value, underlyingType);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
