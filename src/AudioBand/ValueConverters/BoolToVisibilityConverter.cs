using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Converter from <see langword="bool"/> to <see cref="Visibility"/>.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility), ParameterType = typeof(bool))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="bool"/> to a <see cref="Visibility"/>.
        /// </summary>
        /// <param name="value">The value of the bool to convert.</param>
        /// <param name="targetType">Type is <see cref="Visibility"/>.</param>
        /// <param name="parameter">A bool indicating whether the visibility should be collapsed.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns><see cref="Visibility.Visible"/> if <paramref name="value"/> is true.
        /// Otherwise returns <see cref="Visibility.Collapsed"/> except when <paramref name="parameter"/> is false in which case
        /// <see cref="Visibility.Hidden"/> is returned.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ValueConverterHelper.IsValid<bool>(value))
            {
                return DependencyProperty.UnsetValue;
            }

            var collapse = parameter == null ? true : System.Convert.ToBoolean(parameter);
            var isVisible = (bool)value;
            if (isVisible)
            {
                return Visibility.Visible;
            }
            else
            {
                return collapse ? Visibility.Collapsed : Visibility.Hidden;
            }
        }

        /// <summary>
        /// Converts a <see cref="Visibility"/> to a <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Visibility"/>.</param>
        /// <param name="targetType">The type of <see cref="bool"/>.</param>
        /// <param name="parameter">Parameter is ignored.</param>
        /// <param name="culture">The current culture.</param>
        /// <returns>True if <paramref name="value"/> is <see cref="Visibility.Visible"/>. False otherwise.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ValueConverterHelper.IsValid<Visibility>(value))
            {
                return DependencyProperty.UnsetValue;
            }

            var visibility = (Visibility)value;
            return visibility == Visibility.Visible;
        }
    }
}
