using System;
using System.Globalization;
using System.Windows;
using AudioBand.UI;
using Xunit;

namespace AudioBand.Test
{
    public class ObjectToTypeConverterTests
    {
        private readonly ObjectToTypeConverter _converter = new ObjectToTypeConverter();

        [Fact]
        public void ConvertNull_ReturnsUnsetValue()
        {
            Assert.Equal(DependencyProperty.UnsetValue, _converter.Convert(null, typeof(Type), null, CultureInfo.CurrentCulture));
        }

        [Fact]
        public void ConvertObject_ReturnsTypeOfObject()
        {
            var result = _converter.Convert("string", typeof(Type), null, CultureInfo.CurrentCulture);

            Assert.Equal(typeof(string), result);
        }

        [Fact]
        public void ConvertBack_NotSupported_ReturnsUnsetValue()
        {
            Assert.Equal(DependencyProperty.UnsetValue, _converter.ConvertBack(null, typeof(object), null, CultureInfo.CurrentCulture));
        }
    }
}
