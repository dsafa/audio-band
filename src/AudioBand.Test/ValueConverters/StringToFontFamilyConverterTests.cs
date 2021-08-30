using AudioBand.UI;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Xunit;

namespace AudioBand.Test
{
    public class StringToFontFamilyConverterTests
    {
        private readonly StringToFontFamilyConverter _converter = new StringToFontFamilyConverter();

        [Fact]
        public void ConvertString_ReturnsFontFamily()
        {
            var result = _converter.Convert("Arial", typeof(FontFamily), null, CultureInfo.CurrentCulture);

            Assert.IsType<FontFamily>(result);
            Assert.Equal("Arial", ((FontFamily)result).Source);
        }

        [Fact]
        public void ConvertNull_ReturnsUnsetValue()
        {
            Assert.Equal(DependencyProperty.UnsetValue, _converter.Convert(null, typeof(FontFamily), null, CultureInfo.CurrentCulture));
        }

        [Fact]
        public void ConvertBackFontFamily_ReturnsStringName()
        {
            var ff = new FontFamily("Arial");

            var result = _converter.ConvertBack(ff, typeof(string), null, CultureInfo.CurrentCulture);

            Assert.IsType<string>(result);
            Assert.Equal("Arial", (string)result);
        }

        [Fact]
        public void ConvertBackNull_ReturnsUnsetValue()
        {
            Assert.Equal(DependencyProperty.UnsetValue, _converter.ConvertBack(null, typeof(string), null, CultureInfo.CurrentCulture));

        }
    }
}
