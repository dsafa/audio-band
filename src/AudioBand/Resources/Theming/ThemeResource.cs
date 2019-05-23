using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AudioBand.Resources.Theming
{
    /// <summary>
    /// Markup extension for theme resources.
    /// </summary>
    [MarkupExtensionReturnType(typeof(BindingExpression))]
    public class ThemeResource : MarkupExtension
    {
        private static readonly Lazy<ThemeDictionary> ThemeDictionaryInstance = new Lazy<ThemeDictionary>();
        private static readonly Lazy<FallbackThemeDictionary> FallbackInstance = new Lazy<FallbackThemeDictionary>();
        private static readonly bool ThemeSupported = Environment.OSVersion.Version.Major == 10;
        private readonly string _key;
        private readonly ThemeResourceKey _themeKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeResource"/> class.
        /// </summary>
        /// <param name="key">The resource key.</param>
        public ThemeResource(string key)
        {
            _key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeResource"/> class.
        /// </summary>
        /// <param name="key">The resource key.</param>
        public ThemeResource(ThemeResourceKey key)
        {
            _themeKey = key;
        }

        /// <summary>
        /// Gets the instance of the theme dictionary.
        /// </summary>
        public static IThemeDictionary Instance => ThemeSupported ? (IThemeDictionary)ThemeDictionaryInstance.Value : FallbackInstance.Value;

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding
            {
                Source = Instance,
                Mode = BindingMode.OneWay,
                Path = new PropertyPath($"[{_key ?? _themeKey.ToString()}]"),
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
