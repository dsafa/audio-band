using AudioBand.UI;
using System.Globalization;
using System.Windows;
using Xunit;
using Visibility = System.Windows.Visibility;

namespace AudioBand.Test
{
    public class BoolToVisibilityConverterTests
    {
        private readonly BoolToVisibilityConverter _converter = new BoolToVisibilityConverter();

        [Fact]
        public void ConvertTrue_ReturnsVisible()
        {
            var result = ConvertToVisibility(true, null);
            Assert.Equal(Visibility.Visible, result);
        }

        [Fact]
        public void ConvertFalse_ParameterNull_ReturnsCollapsed()
        {
            var result = ConvertToVisibility(false, null);
            Assert.Equal(Visibility.Collapsed, result);
        }

        [Fact]
        public void ConvertFalse_ParameterTrue_ReturnsCollapsed()
        {
            var result = ConvertToVisibility(false, true);
            Assert.Equal(Visibility.Collapsed, result);
        }

        [Fact]
        public void ConvertFalse_ParameterFalse_ReturnsHidden()
        {
            var result = ConvertToVisibility(false, false);
            Assert.Equal(Visibility.Hidden, result);
        }

        [Fact]
        public void ConvertNull_ReturnsUnsetValue()
        {
            Assert.Equal(DependencyProperty.UnsetValue, _converter.Convert(null, typeof(Visibility), null, CultureInfo.CurrentCulture));
        }

        [Fact]
        public void ConvertNonBool_ReturnsUnsetValue()
        {
            Assert.Equal(DependencyProperty.UnsetValue, _converter.Convert(null, typeof(Visibility), null, CultureInfo.CurrentCulture));
        }

        [Fact]
        public void ConvertBackVisible_ReturnsTrue()
        {
            Assert.True(ConvertToBool(Visibility.Visible));
        }

        [Fact]
        public void ConvertBackCollapsed_ReturnsFalse()
        {
            Assert.False(ConvertToBool(Visibility.Collapsed));
        }

        [Fact]
        public void ConvertBackHidden_ReturnsFalse()
        {
            Assert.False(ConvertToBool(Visibility.Hidden));
        }

        [Fact]
        public void ConvertBackNull_ReturnsUnsetValue()
        {
            Assert.Equal(DependencyProperty.UnsetValue, _converter.ConvertBack(null, typeof(bool), null, CultureInfo.CurrentCulture));
        }

        [Fact]
        public void ConvertBackNonVisibility_ReturnsUnsetValue()
        {
            Assert.Equal(DependencyProperty.UnsetValue, _converter.ConvertBack("invalid", typeof(bool), null, CultureInfo.CurrentCulture));
        }

        private Visibility ConvertToVisibility(bool value, object parameter)
        {
            var result = _converter.Convert(value, typeof(Visibility), parameter, CultureInfo.CurrentCulture);

            Assert.IsType<Visibility>(result);

            return (Visibility)result;
        }

        private bool ConvertToBool(Visibility visibility)
        {
            var result = _converter.ConvertBack(visibility, typeof(bool), null, CultureInfo.CurrentCulture);

            Assert.IsType<bool>(result);

            return (bool)result;
        }
    }
}
