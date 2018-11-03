using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using NLog;

namespace AudioBand.ViewModels
{
    internal abstract class ViewModelBase<TModel> : INotifyPropertyChanged, IEditableObject, IResettableObject
    {
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Model associated with this view model.
        /// </summary>
        public TModel Model { get; set; }

        protected ViewModelBase(TModel model)
        {
            Model = model;
        }

        /// <summary>
        /// Notifies subsribers to a property change.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the property in the <see cref="Model"/> and calls property changed.
        /// </summary>
        /// <typeparam name="TValue">Type of the property to set.</typeparam>
        /// <param name="modelPropertyName">Name of the property to set.</param>
        /// <param name="newValue">New value to set.</param>
        /// <param name="propertyName">Name of the property to notify with.</param>
        /// <param name="alsoNotify">Other properties to notify.</param>
        /// <returns>Returns true if new value was set</returns>
        protected bool SetModelProperty<TValue>(string modelPropertyName, TValue newValue, [CallerMemberName] string propertyName = null, params string[] alsoNotify)
        {
            var currentValue = (TValue) GetType().GetProperty(propertyName).GetValue(this);
            if (EqualityComparer<TValue>.Default.Equals(currentValue, newValue))
            {
                return true;
            }

            Model.GetType().GetProperty(propertyName).SetValue(Model, newValue);

            RaisePropertyChanged(propertyName);
            foreach (var otherPropertyName in alsoNotify)
            {
                RaisePropertyChanged(otherPropertyName);
            }

            return false;
        }

        /// <summary>
        /// Sets the <paramref name="field"/> and calls property changed.
        /// </summary>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="field">Field to set.</param>
        /// <param name="newValue">New value of the field.</param>
        /// <param name="propertyName">Name of the property to notify with.</param>
        /// <param name="alsoNotify">Other properties to notify.</param>
        /// <returns>Returns true if new value was set</returns>
        protected bool SetProperty<TValue>(ref TValue field, TValue newValue, [CallerMemberName] string propertyName = null, params string[] alsoNotify)
        {
            if (EqualityComparer<TValue>.Default.Equals(field, newValue))
            {
                return true;
            }

            field = newValue;
            RaisePropertyChanged(propertyName);
            foreach (var otherPropertyName in alsoNotify)
            {
                RaisePropertyChanged(otherPropertyName);
            }

            return false;
        }

        protected Image LoadImage(string path, Image defaultImage)
        {
            try
            {
                return string.IsNullOrEmpty(path) ? defaultImage : Image.FromFile(path);
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().Debug($"Error loading image from {path}, {e}");
                return defaultImage;
            }
        }

        public void BeginEdit()
        {
            throw new System.NotImplementedException();
        }

        public void EndEdit()
        {
            throw new System.NotImplementedException();
        }

        public void CancelEdit()
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }
    }
}
