using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using AudioBand.Extensions;
using Windows.UI.ViewManagement;

namespace AudioBand.UI
{
    /// <summary>
    /// Contains standard theme brushes.
    /// </summary>
    public class ThemeDictionary : IThemeDictionary
    {
        private static readonly Dictionary<string, Brush> LightThemeResources = BuildDictionary(new LightThemeResources());
        private static readonly Dictionary<string, Brush> DarkThemeResources = BuildDictionary(new DarkThemeResources());
        private static readonly IEnumerable<UIColorType> ColorTypes = Enum.GetValues(typeof(UIColorType)).Cast<UIColorType>().Where(t => t != UIColorType.Complement);
        private static readonly Dictionary<UIColorType, string> ColorTypeToBrushName = new Dictionary<UIColorType, string>
        {
            { UIColorType.Accent, ThemeResourceKey.SystemAccentColor.ToString() },
            { UIColorType.AccentDark1, ThemeResourceKey.SystemAccentColorDark1.ToString() },
            { UIColorType.AccentDark2, ThemeResourceKey.SystemAccentColorDark2.ToString() },
            { UIColorType.AccentDark3, ThemeResourceKey.SystemAccentColorDark3.ToString() },
            { UIColorType.AccentLight1, ThemeResourceKey.SystemAccentColorLight1.ToString() },
            { UIColorType.AccentLight2, ThemeResourceKey.SystemAccentColorLight2.ToString() },
            { UIColorType.AccentLight3, ThemeResourceKey.SystemAccentColorLight3.ToString() },
            { UIColorType.Background, ThemeResourceKey.SystemBackgroundColor.ToString() },
            { UIColorType.Foreground, ThemeResourceKey.SystemForegroundColor.ToString() },
        };

        private readonly Dictionary<string, Brush> _dynamicResources = new Dictionary<string, Brush>();
        private readonly UISettings _uiSettings = new UISettings();
        private Color _currentAccentColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeDictionary"/> class.
        /// </summary>
        public ThemeDictionary()
        {
            _uiSettings.ColorValuesChanged += UiSettingsOnColorValuesChanged;
            UpdateColors();
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the resource from the key.
        /// </summary>
        /// <param name="key">The resource key.</param>
        public Brush this[string key]
        {
            get => ResolveValue(key);
            private set => _dynamicResources[key] = value; // Only set dynamic resources
        }

        /// <summary>
        /// Gets the resource from the resource key.
        /// </summary>
        /// <param name="key">The theme resource key.</param>
        /// <returns>The resource.</returns>
        private Brush this[ThemeResourceKey key]
        {
            get => ResolveValue(key.ToString());
            set => _dynamicResources[key.ToString()] = value;
        }

        private static Dictionary<string, Brush> BuildDictionary(object o)
        {
            return o.GetType().GetProperties().ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => (Brush)propertyInfo.GetValue(o));
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RaiseThemeChanged()
        {
            OnPropertyChanged("Item[]");
        }

        private void UiSettingsOnColorValuesChanged(UISettings sender, object args)
        {
            // If only light / dark theme changed
            if (Color.AreClose(_uiSettings.GetColorValue(UIColorType.Accent).ToWpfColor(), _currentAccentColor))
            {
                UpdateDynamicAccent();
                RaiseThemeChanged();
                return;
            }

            UpdateColors();
        }

        private void UpdateColors()
        {
            foreach (var uiColorType in ColorTypes)
            {
                var color = _uiSettings.GetColorValue(uiColorType).ToWpfColor();
                var brush = new SolidColorBrush(color);
                brush.Freeze();
                this[ColorTypeToBrushName[uiColorType]] = brush;
            }

            UpdateDynamicAccent();

            _currentAccentColor = _uiSettings.GetColorValue(UIColorType.Accent).ToWpfColor();
            RaiseThemeChanged();
        }

        private void UpdateDynamicAccent()
        {
            if (IsLightTheme())
            {
                this[ThemeResourceKey.SystemAccentColorLow] = this[ThemeResourceKey.SystemAccentColorLight3];
                this[ThemeResourceKey.SystemAccentColorMedium] = this[ThemeResourceKey.SystemAccentColorLight2];
                this[ThemeResourceKey.SystemAccentColorHigh] = this[ThemeResourceKey.SystemAccentColorLight1];
            }
            else
            {
                this[ThemeResourceKey.SystemAccentColorLow] = this[ThemeResourceKey.SystemAccentColorDark3];
                this[ThemeResourceKey.SystemAccentColorMedium] = this[ThemeResourceKey.SystemAccentColorDark2];
                this[ThemeResourceKey.SystemAccentColorHigh] = this[ThemeResourceKey.SystemAccentColorDark1];
            }
        }

        private bool IsLightTheme()
        {
            var color = _uiSettings.GetColorValue(UIColorType.Foreground);
            return color.A == 255 && color.R == 0 && color.G == 0 && color.B == 0;
        }

        private Brush ResolveValue(string key)
        {
            if (_dynamicResources.ContainsKey(key))
            {
                return _dynamicResources[key];
            }

            try
            {
                return IsLightTheme() ? LightThemeResources[key] : DarkThemeResources[key];
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("No theme resource found with key: " + key);
            }
        }
    }
}
