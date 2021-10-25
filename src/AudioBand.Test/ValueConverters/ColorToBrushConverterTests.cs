using AudioBand.UI;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Xunit;

namespace AudioBand.Test
{
    public class ColorToBrushConverterTests
    {
        private readonly ColorToBrushConverter _converter = new ColorToBrushConverter();

        [Fact]
        public void ConvertColor_ReturnsSolidColorBrushWithSameColor()
        {
            var color = Colors.Red;
            var result = _converter.Convert(color, typeof(SolidColorBrush), null, CultureInfo.CurrentCulture);

            Assert.IsType<SolidColorBrush>(result);
            Assert.Equal(color, ((SolidColorBrush)result).Color);
        }

        [Fact]
        public void ConvertNull_ReturnsUnsetValue()
        {
            Assert.Equal(DependencyProperty.UnsetValue, _converter.Convert(null, typeof(SolidColorBrush), null, CultureInfo.CurrentCulture));
        }

        [Fact]
        public void ConvertBackSolidColorBrush_ReturnsColorWithSameValue()
        {
            var brush = Brushes.Blue;
            var result = _converter.ConvertBack(brush, typeof(Color), null, CultureInfo.CurrentCulture);

            Assert.IsType<Color>(result);
            Assert.Equal(brush.Color, (Color)result);
        }

        [Fact]
        public void ConvertBackNull_ReturnsUnsetValue()
        {
            Assert.Equal(DependencyProperty.UnsetValue, _converter.ConvertBack(null, typeof(Color), null, CultureInfo.CurrentCulture));
        }
    }
}
