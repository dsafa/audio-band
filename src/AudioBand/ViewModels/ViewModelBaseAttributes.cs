using System;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Specifies that <see cref="ViewModelBase{TModel}.PropertyChanged"/> will be raised for this property.
    /// when the property of a model changes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class PropertyChangeBindingAttribute : Attribute
    {
        /// <summary>
        /// Name of the property in the model that will trigger a <see cref="ViewModelBase{TModel}.PropertyChanged"/>.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Creates attribute with the property name.
        /// </summary>
        /// <param name="propertyName">Name of the property in the model that will trigger a <see cref="ViewModelBase{TModel}.PropertyChanged"/>.</param>
        public PropertyChangeBindingAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }

    /// <summary>
    /// Specifies that <see cref="ViewModelBase{TModel}.PropertyChanged"/> will be raise for other properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class AlsoNotifyAttribute : Attribute
    {
        /// <summary>
        /// Name of other properties to also notify when the model's value changes.
        /// </summary>
        public string[] AlsoNotify { get; }

        /// <summary>
        /// Creates attribute with list of attributes to notify
        /// </summary>
        /// <param name="alsoNotify">Name of other properties to also notify when the model's value changes.</param>
        public AlsoNotifyAttribute(params string[] alsoNotify)
        {
            AlsoNotify = alsoNotify;
        }
    }
}
