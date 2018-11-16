using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AudioBand.Views.Wpf.TemplateSelectors
{
    internal class AudioSourceSettingSelectorcs : DataTemplateSelector
    {
        public DataTemplate StringTemplate { get; set; }
        public DataTemplate BoolTemplate { get; set; }
        public DataTemplate IntTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is Type type)) throw new ArgumentException($"Invalid type {item}");

            if (type == typeof(string))
            {
                return StringTemplate;
            }

            if (type == typeof(bool))
            {
                return BoolTemplate;
            }

            if (type == typeof(int))
            {
                return IntTemplate;
            }

            throw new ArgumentException($"No matching template for `{type}`");
        }
    }
}
