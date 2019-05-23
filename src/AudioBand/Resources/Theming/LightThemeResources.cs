using System.Windows.Media;

namespace AudioBand.Resources.Theming
{
    /// <summary>
    /// Light theme colors.
    /// </summary>
#pragma warning disable
    public class LightThemeResources : IThemeResources
    {
        public LightThemeResources()
        {
            foreach (var propertyInfo in GetType().GetProperties())
            {
                ((Brush)propertyInfo.GetValue(this)).Freeze();
            }
        }

        public Brush SystemAltHighColor { get; } = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

        public Brush SystemAltLowColor { get; } = new SolidColorBrush(Color.FromArgb(51, 255, 255, 255));

        public Brush SystemAltMediumColor { get; } = new SolidColorBrush(Color.FromArgb(153, 255, 255, 255));

        public Brush SystemAltMediumHighColor { get; } = new SolidColorBrush(Color.FromArgb(204, 255, 255, 255));

        public Brush SystemAltMediumLowColor { get; } = new SolidColorBrush(Color.FromArgb(102, 255, 255, 255));

        public Brush SystemBaseHighColor { get; } = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

        public Brush SystemBaseLowColor { get; } = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));

        public Brush SystemBaseMediumColor { get; } = new SolidColorBrush(Color.FromArgb(153, 0, 0, 0));

        public Brush SystemBaseMediumHighColor { get; } = new SolidColorBrush(Color.FromArgb(204, 0, 0, 0));

        public Brush SystemBaseMediumLowColor { get; } = new SolidColorBrush(Color.FromArgb(102, 0, 0, 0));

        public Brush SystemChromeAltLowColor { get; } = new SolidColorBrush(Color.FromArgb(255, 23, 23, 23));

        public Brush SystemChromeBlackHighColor { get; } = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

        public Brush SystemChromeBlackLowColor { get; } = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));

        public Brush SystemChromeBlackMediumLowColor { get; } = new SolidColorBrush(Color.FromArgb(102, 0, 0, 0));

        public Brush SystemChromeBlackMediumColor { get; } = new SolidColorBrush(Color.FromArgb(204, 0, 0, 0));

        public Brush SystemChromeDisabledHighColor { get; } = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));

        public Brush SystemChromeDisabledLowColor { get; } = new SolidColorBrush(Color.FromArgb(255, 122, 122, 122));

        public Brush SystemChromeHighColor { get; } = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));

        public Brush SystemChromeLowColor { get; } = new SolidColorBrush(Color.FromArgb(255, 242, 242, 242));

        public Brush SystemChromeMediumColor { get; } = new SolidColorBrush(Color.FromArgb(255, 230, 230, 230));

        public Brush SystemChromeMediumLowColor { get; } = new SolidColorBrush(Color.FromArgb(255, 242, 242, 242));

        public Brush SystemChromeWhiteColor { get; } = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

        public Brush SystemListLowColor { get; } = new SolidColorBrush(Color.FromArgb(25, 0, 0, 0));

        public Brush SystemListMediumColor { get; } = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));
    }
}
#pragma warning enable
