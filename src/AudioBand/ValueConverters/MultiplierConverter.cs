using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Mutliples all values together.
    /// </summary>
    [ValueConversion(typeof(double), typeof(double), ParameterType = typeof(double))]
    public class MultiplierConverter : IMultiValueConverter
    {
        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                return 1;
            }

            double acc = 1;
            foreach (var value in values.Where(v => double.TryParse(v.ToString(), out _)).Select(v => double.Parse(v.ToString())))
            {
                acc *= value;
            }

            return acc;
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
