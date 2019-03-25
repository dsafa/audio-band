using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WinColor = System.Drawing.Color;
using WpfColor = System.Windows.Media.Color;

namespace AudioBand.Views.Wpf.ValueConverters
{
    /// <summary>
    /// Converts a winforms color to a wpf color.
    /// </summary>
    internal class WinColorToWpfColorConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return default(WpfColor);
            }

            if (value is WpfColor)
            {
                return value;
            }

            var winColor = (WinColor)value;
            return WpfColor.FromArgb(winColor.A, winColor.R, winColor.G, winColor.B);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return default(WinColor);
            }

            if (value is WinColor)
            {
                return value;
            }

            var wpfColor = (WpfColor)value;
            return WinColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
        }
    }
}
