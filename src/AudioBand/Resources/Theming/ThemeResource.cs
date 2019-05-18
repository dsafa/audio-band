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

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding
            {
                Source = ThemeDictionary.Instance,
                Mode = BindingMode.OneWay,
                Path = new PropertyPath($"[{_key ?? _themeKey.ToString()}]"),
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
