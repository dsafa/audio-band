using System;
using System.Windows;
using System.Windows.Controls;
using AudioBand.ViewModels;

namespace AudioBand.Views.Wpf.TemplateSelectors
{
    /// <summary>
    /// Template selector for <see cref="AudioSourceSettingVM"/>.
    /// </summary>
    internal class AudioSourceSettingSelector : DataTemplateSelector
    {
        /// <summary>
        /// The type of template to select.
        /// </summary>
        public enum SettingTemplateType
        {
            /// <summary>
            /// Template for the setting key.
            /// </summary>
            Key,

            /// <summary>
            /// Template for the setting value.
            /// </summary>
            Value
        }

        /// <summary>
        /// Gets or sets the type of the template.
        /// </summary>
        public SettingTemplateType TemplateType { get; set; }

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
        public DataTemplate SensitiveTemplate { get; set; }

        /// <summary>
        /// Gets or sets the normal label template.
        /// </summary>
        public DataTemplate NormalLabelTemplate { get; set; }

        /// <summary>
        /// Gets or sets the sensitive label template.
        /// </summary>
        public DataTemplate SensitiveLabelTemplate { get; set; }

        /// <inheritdoc/>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var setting = item as AudioSourceSettingVM;
            if (setting == null)
            {
                throw new ArgumentException();
            }

            var type = setting.SettingType;
            return TemplateType == SettingTemplateType.Value ? SelectValueTemplate(type, setting) : SelectKeyTemplate(setting);
        }

        private DataTemplate SelectValueTemplate(Type type, AudioSourceSettingVM setting)
        {
            if (type == typeof(string))
            {
                return setting.Sensitive ? SensitiveTemplate : StringTemplate;
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

        private DataTemplate SelectKeyTemplate(AudioSourceSettingVM setting)
        {
            return setting.Sensitive ? SensitiveLabelTemplate : NormalLabelTemplate;
        }
    }
}
