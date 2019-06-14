using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FastMember;

namespace AudioBand
{
    /// <summary>
    /// Base object that implements and provides facilities for <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        private readonly Dictionary<string, string[]> _alsoNotifyMap = new Dictionary<string, string[]>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableObject"/> class.
        /// </summary>
        protected ObservableObject()
        {
            Accessor = TypeAccessor.Create(GetType(), true);
            SetupAlsoNotify();
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the type accessor for this object.
        /// </summary>
        protected TypeAccessor Accessor { get; }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Notifies all properties changed.
        /// </summary>
        protected void RaisePropertyChangedAll()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }

        /// <summary>
        /// Sets the <paramref name="field"/> to the <paramref name="newValue"/> if they are different and raise <see cref="PropertyChanged"/>
        /// for the property and other properties marked with the <see cref="AlsoNotifyAttribute"/>.
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

            OnPropertyChanging(propertyName);
            field = newValue;
            RaisePropertyChanged(propertyName);
            OnPropertyChanged(propertyName);

            if (_alsoNotifyMap.TryGetValue(propertyName, out var alsoNotify))
            {
                foreach (var alsoNotifyPropertyName in alsoNotify)
                {
                    OnPropertyChanging(alsoNotifyPropertyName);
                    RaisePropertyChanged(alsoNotifyPropertyName);
                    OnPropertyChanged(alsoNotifyPropertyName);
                }
            }

            return true;
        }

        /// <summary>
        /// Override this method to handle the event when a property is changing.
        /// </summary>
        /// <param name="propertyName">The name of the property that is changing.</param>
        protected virtual void OnPropertyChanging(string propertyName)
        {
        }

        /// <summary>
        /// Override this method to handle the event when a property was changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
        }

        private void SetupAlsoNotify()
        {
            var alsoNotifyProperties = Accessor.GetMembers().Where(m => m.IsDefined(typeof(AlsoNotifyAttribute)));
            foreach (var propertyInfo in alsoNotifyProperties)
            {
                var attr = (AlsoNotifyAttribute)propertyInfo.GetAttribute(typeof(AlsoNotifyAttribute), true);
                _alsoNotifyMap.Add(propertyInfo.Name, attr.AlsoNotify);
            }
        }
    }
}
