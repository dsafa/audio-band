using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioBand.Views.Wpf.ValueConverters
{
    internal class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                return !string.IsNullOrEmpty(s);
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
