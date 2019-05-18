using System.Windows.Media;

namespace AudioBand.Resources.Theming
{
    /// <summary>
    /// Dark theme resources.
    /// </summary>
#pragma warning disable
    public class DarkThemeDictionary : IThemeDictionary
    {
        public DarkThemeDictionary()
        {
            foreach (var propertyInfo in GetType().GetProperties())
            {
                ((Brush)propertyInfo.GetValue(this)).Freeze();
            }
        }

        public Brush SystemAltHighColor { get; } = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

        public Brush SystemAltLowColor { get; } = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));

        public Brush SystemAltMediumColor { get; } = new SolidColorBrush(Color.FromArgb(153, 0, 0, 0));

        public Brush SystemAltMediumHighColor { get; } = new SolidColorBrush(Color.FromArgb(204, 0, 0, 0));

        public Brush SystemAltMediumLowColor { get; } = new SolidColorBrush(Color.FromArgb(102, 0, 0, 0));

        public Brush SystemBaseHighColor { get; } = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

        public Brush SystemBaseLowColor { get; } = new SolidColorBrush(Color.FromArgb(51, 255, 255, 255));

        public Brush SystemBaseMediumColor { get; } = new SolidColorBrush(Color.FromArgb(153, 255, 255, 255));

        public Brush SystemBaseMediumHighColor { get; } = new SolidColorBrush(Color.FromArgb(204, 255, 255, 255));

        public Brush SystemBaseMediumLowColor { get; } = new SolidColorBrush(Color.FromArgb(102, 255, 255, 255));

        public Brush SystemChromeAltLowColor { get; } = new SolidColorBrush(Color.FromArgb(255, 242, 242, 242));

        public Brush SystemChromeBlackHighColor { get; } = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

        public Brush SystemChromeBlackLowColor { get; } = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));

        public Brush SystemChromeBlackMediumLowColor { get; } = new SolidColorBrush(Color.FromArgb(102, 0, 0, 0));

        public Brush SystemChromeBlackMediumColor { get; } = new SolidColorBrush(Color.FromArgb(204, 0, 0, 0));

        public Brush SystemChromeDisabledHighColor { get; } = new SolidColorBrush(Color.FromArgb(255, 51, 51, 51));

        public Brush SystemChromeDisabledLowColor { get; } = new SolidColorBrush(Color.FromArgb(255, 133, 133, 133));

        public Brush SystemChromeHighColor { get; } = new SolidColorBrush(Color.FromArgb(255, 118, 118, 118));

        public Brush SystemChromeLowColor { get; } = new SolidColorBrush(Color.FromArgb(255, 23, 23, 23));

        public Brush SystemChromeMediumColor { get; } = new SolidColorBrush(Color.FromArgb(255, 31, 31, 31));

        public Brush SystemChromeMediumLowColor { get; } = new SolidColorBrush(Color.FromArgb(255, 43, 43, 43));

        public Brush SystemChromeWhiteColor { get; } = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

        public Brush SystemListLowColor { get; } = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));

        public Brush SystemListMediumColor { get; } = new SolidColorBrush(Color.FromArgb(51, 255, 255, 255));
    }
}
#pragma warning enable
