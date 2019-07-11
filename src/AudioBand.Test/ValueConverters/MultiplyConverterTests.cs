using System.Globalization;
using AudioBand.ValueConverters;
using Xunit;

namespace AudioBand.Test
{
    public class MultiplyConverterTests
    {
        private readonly MultiplierConverter _converter = new MultiplierConverter();

        [Fact]
        public void ConvertSingleValue_ReturnsValue()
        {
            var result = _converter.Convert(new object[] {5.0}, typeof(double), null, CultureInfo.CurrentCulture);

            Assert.IsType<double>(result);
            Assert.Equal(5.0, (double)result, 4);
        }

        [Fact]
        public void ConvertMultipleValues_ReturnsMultipliedValue()
        {
            var values = new object[] {1.0, 2, 3};
            var result = _converter.Convert(values, typeof(double), null, CultureInfo.CurrentCulture);

            Assert.IsType<double>(result);
            Assert.Equal(6.0, (double)result, 4);
        }

        [Fact]
        public void ConvertNone_Returns1()
        {
            var result = _converter.Convert(null, typeof(double), null, CultureInfo.CurrentCulture);

            Assert.IsType<double>(result);
            Assert.Equal(1.0, (double)result);
        }

        [Fact]
        public void ConvertBack_NotSupported_ReturnsNull()
        {
            var result = _converter.ConvertBack(new object[]{"a", 1}, new[] { typeof(string), typeof(int)}, null, CultureInfo.CurrentCulture);
            Assert.Null(result);
        }
    }
}
