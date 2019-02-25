using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NLog;

namespace AudioBand.Models
{
    /// <summary>
    /// Base class for models.
    /// </summary>
    public class ModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase"/> class.
        /// </summary>
        public ModelBase()
        {
            Logger = LogManager.GetLogger(GetType().FullName);
        }

        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the logger for the model
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Set property to a new value.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">Old value.</param>
        /// <param name="newValue">New value.</param>
        /// <param name="propertyName">Name of the property that changed.</param>
        /// <returns>True if the property changed.</returns>
        protected virtual bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
