using System;
using System.ComponentModel;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Specifies that <see cref="INotifyPropertyChanged.PropertyChanged"/> event of <see cref="ViewModelBase{TModel}"/> will be raised for this property
    /// when the property of a model changes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class PropertyChangeBindingAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangeBindingAttribute"/> class
        /// with the property name of the model.
        /// </summary>
        /// <param name="propertyName">Name of the property in the model that will trigger a <see cref="INotifyPropertyChanged.PropertyChanged"/> event.</param>
        public PropertyChangeBindingAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Gets the name of the property in the model that will trigger a <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        public string PropertyName { get; }
    }
}
