using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Multi value converter to convert an x and y into a point.
    /// </summary>
    public class PointConverter : IMultiValueConverter
    {
        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                return default(Point);
            }

            if (values.Any(v => v == DependencyProperty.UnsetValue))
            {
                return default(Point);
            }

            var x = System.Convert.ToDouble(values[0]);
            var y = System.Convert.ToDouble(values[1]);
            return new Point(x, y);
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
