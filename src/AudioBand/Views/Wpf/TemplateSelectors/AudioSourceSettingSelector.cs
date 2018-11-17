using AudioBand.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AudioBand.Views.Wpf.TemplateSelectors
{
    internal class AudioSourceSettingSelector : DataTemplateSelector
    {
        public SettingTemplateType TemplateType { get; set; }
        public DataTemplate StringTemplate { get; set; }
        public DataTemplate BoolTemplate { get; set; }
        public DataTemplate IntTemplate { get; set; }
        public DataTemplate SensitiveTemplate { get; set; }

        public DataTemplate NormalLabelTemplate { get; set; }
        public DataTemplate SensitiveLabelTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is AudioSourceSettingVM setting)) throw new ArgumentException();

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

            throw new ArgumentException($"No matching value template for `{type}`");
        }

        private DataTemplate SelectKeyTemplate(AudioSourceSettingVM setting)
        {
            return setting.Sensitive ? SensitiveLabelTemplate : NormalLabelTemplate;
        }

        public enum SettingTemplateType
        {
            Key,
            Value
        }
    }
}
