using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AudioBand.ValueConverters;
using Xunit;

namespace AudioBand.Test
{
    public class FlagToBoolConverterTests
    {
        private readonly FlagToBoolConverter _converter = new FlagToBoolConverter();

        [Flags]
        private enum TestEnum
        {
            None = 0,
            Flag1 = 1,
            Flag2 = 2,
        }

        [Fact]
        public void ConvertNoFlag_ReturnsFalse()
        {
            var result = _converter.Convert(TestEnum.None, typeof(bool), TestEnum.Flag2, CultureInfo.CurrentCulture);

            Assert.IsType<bool>(result);
            Assert.False((bool)result);
        }

        [Fact]
        public void ConvertHasFlag_ReturnsTrue()
        {
            var value = TestEnum.Flag1 | TestEnum.Flag2;
            var result = _converter.Convert(value, typeof(bool), TestEnum.Flag2, CultureInfo.CurrentCulture);

            Assert.IsType<bool>(result);
            Assert.True((bool)result);
        }

        [Fact]
        public void ConvertNull_ReturnsFalse()
        {
            Assert.False((bool)_converter.Convert(null, typeof(bool), null, CultureInfo.CurrentCulture));
        }

        [Fact]
        public void ConvertBack_ReturnsUnsetValue()
        {
            Assert.Equal(DependencyProperty.UnsetValue, _converter.ConvertBack(null, typeof(Enum), null, CultureInfo.CurrentCulture));
        }
    }
}
