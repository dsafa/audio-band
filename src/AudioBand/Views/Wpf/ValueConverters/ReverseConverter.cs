using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace AudioBand.Views.Wpf.ValueConverters
{
    //https://stackoverflow.com/a/19916654
    [ContentProperty("Converter")]
    public class ReverseConverter : IValueConverter
    {
        public IValueConverter Converter { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converter.ConvertBack(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converter.Convert(value, targetType, parameter, culture);
        }
    }
}
