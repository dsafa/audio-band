using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace AudioBand.UI
{
    /// <summary>
    /// Markup extension for theme resources.
    /// </summary>
    [MarkupExtensionReturnType(typeof(BindingExpression))]
    public class ThemeResourceExtension : MarkupExtension
    {
        private static readonly Lazy<ThemeDictionary> ThemeDictionaryInstance = new Lazy<ThemeDictionary>();
        private static readonly Lazy<FallbackThemeDictionary> FallbackInstance = new Lazy<FallbackThemeDictionary>();
        private static readonly bool ThemeSupported = Environment.OSVersion.Version.Major == 10;
        private readonly string _key;
        private readonly ThemeResourceKey _themeKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeResourceExtension"/> class.
        /// </summary>
        /// <param name="key">The resource key.</param>
        public ThemeResourceExtension(string key)
        {
            _key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeResourceExtension"/> class.
        /// </summary>
        /// <param name="key">The resource key.</param>
        public ThemeResourceExtension(ThemeResourceKey key)
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

            var target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            var property = target?.TargetProperty as DependencyProperty;
            if (property?.PropertyType == typeof(Color))
            {
                binding.Converter = Converters.BrushToColor;
            }

            return binding.ProvideValue(serviceProvider);
        }
    }
}
