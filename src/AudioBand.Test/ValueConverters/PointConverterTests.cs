using System.Globalization;
using System.Windows;
using Xunit;
using PointConverter = AudioBand.UI.PointConverter;

namespace AudioBand.Test
{
    public class PointConverterTests
    {
        private readonly PointConverter _converter = new PointConverter();

        [Fact]
        public void ConvertXAndY_ReturnsPointWithXAndY()
        {
            var x = 1;
            var y = 2;
            var result = _converter.Convert(new object[] { x, y }, typeof(Point), null, CultureInfo.CurrentCulture);

            Assert.IsType<Point>(result);
            var point = (Point)result;
            Assert.Equal(x, point.X);
            Assert.Equal(y, point.Y);
        }

        [Fact]
        public void ConvertBack_Unsupported_ReturnsNull()
        {
            Assert.Null(_converter.ConvertBack(null, null, null, CultureInfo.CurrentCulture));
        }
    }
}
