using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AudioBand.Views.Wpf.ValueConverters
{
    internal class WinColorToWpfColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new Color();
            }

            var winColor = (System.Drawing.Color)value;
            return Color.FromArgb(winColor.A, winColor.R, winColor.G, winColor.B);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new System.Drawing.Color();
            }

            var wpfColor = (Color) value;
            return System.Drawing.Color.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
        }
    }
}
