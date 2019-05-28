using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Reverses the direction of a <see cref="IValueConverter"/>.
    /// </summary>
    /// <remarks>https://stackoverflow.com/a/19916654.</remarks>
    [ContentProperty("Converter")]
    public class ReverseConverter : IValueConverter
    {
        private readonly IValueConverter _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseConverter"/> class with the converter to reverse.
        /// </summary>
        /// <param name="converter">The converter to reverse.</param>
        public ReverseConverter(IValueConverter converter)
        {
            _converter = converter;
        }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _converter.ConvertBack(value, targetType, parameter, culture);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _converter.Convert(value, targetType, parameter, culture);
        }
    }
}
