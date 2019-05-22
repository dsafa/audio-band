using System.Windows;
using System.Windows.Media;

namespace AudioBand.Behaviors
{
    /// <summary>
    /// Attached properties for binding fallback value binding.
    /// </summary>
    public static class Fallback
    {
        /// <summary>
        /// Dependency property for a fallback image.
        /// </summary>
        public static readonly DependencyProperty FallbackImageSourceProperty = DependencyProperty.RegisterAttached("FallbackImageSource", typeof(ImageSource), typeof(Fallback));

        /// <summary>
        /// Gets the fallback image source.
        /// </summary>
        /// <param name="d">The object to get the fallback image from.</param>
        /// <returns>The fallback image.</returns>
        public static ImageSource GetFallbackImageSource(DependencyObject d) => (ImageSource)d.GetValue(FallbackImageSourceProperty);

        /// <summary>
        /// Sets the fallback image source.
        /// </summary>
        /// <param name="d">The object to set the fallback image on.</param>
        /// <param name="value">The fallback image.</param>
        public static void SetFallbackImageSource(DependencyObject d, ImageSource value) => d.SetValue(FallbackImageSourceProperty, value);
    }
}
