using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace AudioBand.Views.Wpf.ValueConverters
{
    /// <summary>
    /// Reverses the direction of a <see cref="IValueConverter"/>.
    /// </summary>
    /// <remarks>https://stackoverflow.com/a/19916654</remarks>
    [ContentProperty("Converter")]
    public class ReverseConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets he converter to revrse.
        /// </summary>
        public IValueConverter Converter { get; set; }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converter.ConvertBack(value, targetType, parameter, culture);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converter.Convert(value, targetType, parameter, culture);
        }
    }
}
