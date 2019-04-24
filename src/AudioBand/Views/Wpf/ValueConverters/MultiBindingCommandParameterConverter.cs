using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioBand.Views.Wpf.ValueConverters
{
    /// <summary>
    /// Converts multi binding for command parameter
    /// </summary>
    [ValueConversion(typeof(object[]), typeof(object[]))]
    public class MultiBindingCommandParameterConverter : IMultiValueConverter
    {
        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
