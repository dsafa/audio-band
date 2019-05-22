using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;

namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Converts a path to an image source. Multivalue version allows the fallback to be bound.
    /// </summary>
    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class PathToImageSourceConverter : IValueConverter, IMultiValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return null;
            }

            var path = value as string;
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            if (path.EndsWith(".svg"))
            {
                try
                {
                    var svgDrawing = new FileSvgReader(new WpfDrawingSettings()).Read(path);
                    svgDrawing.Freeze();
                    return new DrawingImage(svgDrawing);
                }
                catch
                {
                    return null;
                }
            }

            return new BitmapImage(new Uri(path));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(values[0], targetType, parameter, culture) ?? values[1];
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
