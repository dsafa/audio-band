using System.Globalization;
using System.IO;
using System.Windows.Media;
using AudioBand.UI;
using Xunit;

namespace AudioBand.Test
{
    public class PathToImageConverterTests
    {
        private PathToImageSourceConverter _converter = new PathToImageSourceConverter();
        private string _sampleImagePath = Path.GetFullPath("Assets/imgsource.png");

        [Fact]
        void Convert_EmptyPath_ReturnsNull()
        {
            Assert.Null(_converter.Convert("", typeof(ImageSource), null, CultureInfo.CurrentCulture));
        }

        [Fact]
        void Convert_Null_ReturnsNull()
        {
            Assert.Null(_converter.Convert((object)null, typeof(ImageSource), null, CultureInfo.CurrentCulture));
        }

        [Fact]
        void Convert_NoFile_ReturnsNull()
        {
            Assert.Null(_converter.Convert("nonexistingfile.png", typeof(ImageSource), null, CultureInfo.CurrentCulture));
        }

        [Fact]
        void Convert_ValidFile_ReturnsImageSource()
        {
            var imgSource = _converter.Convert(_sampleImagePath, typeof(ImageSource), null, CultureInfo.CurrentCulture);

            Assert.NotNull(imgSource);
            Assert.IsAssignableFrom<ImageSource>(imgSource);
        }

        [Fact]
        void MultiConvert_InvalidFile_UsesFallback()
        {
            object fallback = new object();
            var imgSource = _converter.Convert(new[] {"invalidfile.png", fallback}, typeof(ImageSource), null,
                CultureInfo.CurrentCulture);

            Assert.NotNull(imgSource);
            Assert.Equal(fallback, imgSource);
        }
    }
}
