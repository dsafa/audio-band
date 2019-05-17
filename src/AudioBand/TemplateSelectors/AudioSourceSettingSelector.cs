using System;
using System.Windows;
using System.Windows.Controls;
using AudioBand.Logging;
using AudioBand.ViewModels;
using NLog;

namespace AudioBand.TemplateSelectors
{
    /// <summary>
    /// Template selector for <see cref="AudioSourceSettingKeyValue"/>. Used to format the setting label or change the type of setting input.
    /// </summary>
    internal class AudioSourceSettingSelector : DataTemplateSelector
    {
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<AudioSourceSettingSelector>();

        /// <summary>
        /// Gets or sets the string template.
        /// </summary>
        public DataTemplate StringTemplate { get; set; }

        /// <summary>
        /// Gets or sets the bool template.
        /// </summary>
        public DataTemplate BoolTemplate { get; set; }

        /// <summary>
        /// Gets or sets the int template.
        /// </summary>
        public DataTemplate IntTemplate { get; set; }

        /// <summary>
        /// Gets or sets the uint template.
        /// </summary>
        public DataTemplate UIntTemplate { get; set; }

        /// <summary>
        /// Gets or sets the sensitive text template.
        /// </summary>
        public DataTemplate PasswordStringTemplate { get; set; }

        /// <inheritdoc/>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var setting = item as AudioSourceSettingKeyValue;
            if (setting == null)
            {
                throw new ArgumentException(nameof(item));
            }

            return SelectValueTemplate(setting);
        }

        private DataTemplate SelectValueTemplate(AudioSourceSettingKeyValue setting)
        {
            var type = setting.SettingType;
            Logger.Debug("Selecting value template for setting {name}, type {type}", setting.Name, type);

            if (type == typeof(string))
            {
                return setting.Sensitive ? PasswordStringTemplate : StringTemplate;
            }

            if (type == typeof(bool))
            {
                return BoolTemplate;
            }

            if (type == typeof(int))
            {
                return IntTemplate;
            }

            if (type == typeof(uint))
            {
                return UIntTemplate;
            }

            throw new ArgumentException($"No matching value template for `{type}`");
        }
    }
}
