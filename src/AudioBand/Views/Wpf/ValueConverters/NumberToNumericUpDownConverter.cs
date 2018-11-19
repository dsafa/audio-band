using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioBand.Views.Wpf.ValueConverters
{
    // There are issues binding a number to the value property of a numericupdown control (double?).
    // Example message: Cannot convert from type 'UInt32' to type 'System.Nullable`1[System.Double]
    internal class NumberToNumericUpDownConverter : IValueConverter
    {
        // Number to double?
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType == null) return 0;
            if (value == null) return 0;
            if (!value.GetType().IsPrimitive) return 0;

            return System.Convert.ChangeType(value, underlyingType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
