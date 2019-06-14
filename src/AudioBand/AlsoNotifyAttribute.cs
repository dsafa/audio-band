using System;
using System.ComponentModel;

namespace AudioBand
{
    /// <summary>
    /// Specifies that <see cref="INotifyPropertyChanged.PropertyChanged"/> events will be raised for other properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class AlsoNotifyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlsoNotifyAttribute"/> class.
        /// with a list of property names to also raise change notifications for.
        /// </summary>
        /// <param name="alsoNotify">Name of other properties to also notify when the model's value changes.</param>
        public AlsoNotifyAttribute(params string[] alsoNotify)
        {
            AlsoNotify = alsoNotify;
        }

        /// <summary>
        /// Gets the names of other properties to also notify.
        /// </summary>
        public string[] AlsoNotify { get; }
    }
}
