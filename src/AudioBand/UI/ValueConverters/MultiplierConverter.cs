using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace AudioBand.UI
{
    /// <summary>
    /// Multiplies all values together.
    /// </summary>
    [ValueConversion(typeof(double), typeof(double), ParameterType = typeof(double))]
    public class MultiplierConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts a list of numeric values into a single value equal to multiplying them together.
        /// </summary>
        /// <param name="values">The list of values to multiply together.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>A single value as the result of multiplying all the values together. Returns 1 if no values.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                return 1.0;
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
            return null;
        }
    }
}
