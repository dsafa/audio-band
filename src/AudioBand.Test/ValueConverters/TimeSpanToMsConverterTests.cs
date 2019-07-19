using System;
using System.Globalization;
using AudioBand.UI;
using Xunit;

namespace AudioBand.Test
{
    public class TimeSpanToMsConverterTests
    {
        private readonly TimeSpanToMsConverter _converter = new TimeSpanToMsConverter();

        [Fact]
        public void ConvertTimeSpan_ReturnsMs()
        {
            var time = TimeSpan.FromMilliseconds(500);
            var result = _converter.Convert(time, typeof(double), null, CultureInfo.CurrentCulture);

            Assert.IsType<double>(result);
            Assert.Equal(time.TotalMilliseconds, (double)result);
        }

        [Fact]
        public void ConvertTimeSpanInvalid_Returns0()
        {
            var result = _converter.Convert(null, typeof(double), null, CultureInfo.CurrentCulture);

            Assert.IsType<double>(result);
            Assert.Equal(0.0, (double)result);
        }

        [Fact]
        public void ConvertBackMs_ReturnsTimespan()
        {
            var ms = 1000.0;
            var result = _converter.ConvertBack(ms, typeof(TimeSpan), null, CultureInfo.CurrentCulture);

            Assert.IsType<TimeSpan>(result);
            Assert.Equal(TimeSpan.FromMilliseconds(ms), (TimeSpan)result);
        }

        [Fact]
        public void ConvertBackInvalid_ReturnsTimespan0()
        {
            var result = _converter.ConvertBack(null, typeof(TimeSpan), null, CultureInfo.CurrentCulture);

            Assert.IsType<TimeSpan>(result);
            Assert.Equal(TimeSpan.Zero, (TimeSpan)result);
        }
    }
}
