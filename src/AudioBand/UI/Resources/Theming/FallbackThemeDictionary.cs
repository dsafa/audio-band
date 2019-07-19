using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace AudioBand.UI
{
    /// <summary>
    /// Fallback theme dictionary.
    /// </summary>
    public class FallbackThemeDictionary : IThemeDictionary
    {
        private static readonly Dictionary<string, Brush> Resources = BuildDictionary(new LightThemeResources());

        /// <summary>
        /// Initializes a new instance of the <see cref="FallbackThemeDictionary"/> class.
        /// </summary>
        public FallbackThemeDictionary()
        {
            var brushConverter = new BrushConverter();
            this[ThemeResourceKey.SystemAccentColor] = brushConverter.ConvertFrom("#FF0063B1") as SolidColorBrush;
            this[ThemeResourceKey.SystemAccentColorDark1] = brushConverter.ConvertFrom("#FF004275") as SolidColorBrush;
            this[ThemeResourceKey.SystemAccentColorDark2] = brushConverter.ConvertFrom("#FF002038") as SolidColorBrush;
            this[ThemeResourceKey.SystemAccentColorDark3] = brushConverter.ConvertFrom("#FF0063B1") as SolidColorBrush;
            this[ThemeResourceKey.SystemAccentColorLight1] = brushConverter.ConvertFrom("#FF1E91EA") as SolidColorBrush;
            this[ThemeResourceKey.SystemAccentColorLight2] = brushConverter.ConvertFrom("#FF5FB2F2") as SolidColorBrush;
            this[ThemeResourceKey.SystemAccentColorLight3] = brushConverter.ConvertFrom("#FF86CAFF") as SolidColorBrush;
            this[ThemeResourceKey.SystemAccentColorLow] = brushConverter.ConvertFrom("#FF86CAFF") as SolidColorBrush;
            this[ThemeResourceKey.SystemAccentColorMedium] = brushConverter.ConvertFrom("#FF5FB2F2") as SolidColorBrush;
            this[ThemeResourceKey.SystemAccentColorHigh] = brushConverter.ConvertFrom("#FF1E91EA") as SolidColorBrush;
            this[ThemeResourceKey.SystemBackgroundColor] = Brushes.White;
            this[ThemeResourceKey.SystemForegroundColor] = Brushes.Black;
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { }
            remove { }
        }

        /// <inheritdoc />
        public Brush this[string key] => Resources[key];

        private Brush this[ThemeResourceKey key]
        {
            set => Resources[key.ToString()] = value;
        }

        private static Dictionary<string, Brush> BuildDictionary(object o)
        {
            return o.GetType().GetProperties().ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => (Brush)propertyInfo.GetValue(o));
        }
    }
}
