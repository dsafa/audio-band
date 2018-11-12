using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AudioBand.Models;
using NLog;

namespace AudioBand.ViewModels
{
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies subsribers to a property change.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Base class for a viewmodel with a model.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    internal abstract class ViewModelBase<TModel> :  ViewModelBase, IEditableObject, IResettableObject
    where TModel: ModelBase
    {
        private readonly Dictionary<string, string[]> _alsoNotifyCache = new Dictionary<string, string[]>(); // View model property name -> other vm property names
        private readonly Dictionary<(object model, string modelPropName), string> _modelToPropertyName = new Dictionary<(object model, string modelPropName), string>();

        /// <summary>
        /// Model associated with this view model.
        /// </summary>
        public TModel Model { get; }

        protected ViewModelBase(TModel model)
        {
            Model = model;
            SetupModelBindings(Model);
            SetupAlsoNotify();
        }

        public void BeginEdit()
        {

        }

        public void EndEdit()
        {

        }

        public void CancelEdit()
        {

        }

        public void Reset()
        {

        }

        /// <summary>
        /// Sets the property in the <see cref="Model"/>.
        /// </summary>
        /// <typeparam name="TValue">Type of the property to set.</typeparam>
        /// <param name="modelPropertyName">Name of the property to set.</param>
        /// <param name="newValue">New value to set.</param>
        /// <param name="propertyName">Name of the property to notify with.</param>
        /// <returns>Returns true if new value was set</returns>
        protected bool SetProperty<TValue>(string modelPropertyName, TValue newValue, [CallerMemberName] string propertyName = null)
        {
            Model.GetType().GetProperty(propertyName).SetValue(Model, newValue);
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Sets the <paramref name="field"/> and calls property changed for it and any others given with a <see cref="AlsoNotifyAttribute"/>.
        /// </summary>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="field">Field to set.</param>
        /// <param name="newValue">New value of the field.</param>
        /// <param name="propertyName">Name of the property to notify with.</param>
        /// <returns>Returns true if new value was set</returns>
        protected bool SetProperty<TValue>(ref TValue field, TValue newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TValue>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;
            RaisePropertyChanged(propertyName);

            if (_alsoNotifyCache.TryGetValue(propertyName, out var alsoNotify))
            {
                foreach (var propName in alsoNotify)
                {
                    RaisePropertyChanged(propName);
                }
            }

            return true;
        }

        /// <summary>
        /// Setup to recieve change notifications from model.
        /// </summary>
        /// <param name="model">Model to subscribe to for <see cref="INotifyPropertyChanged.PropertyChanged"/>.</param>
        protected void SetupModelBindings<T>(T model) where T : ModelBase
        {
            var bindingProperties = GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PropertyChangeBindingAttribute)));
            foreach (var propertyInfo in bindingProperties)
            {
                var bindingAttr = propertyInfo.GetCustomAttribute<PropertyChangeBindingAttribute>();
                _modelToPropertyName.Add((model, bindingAttr.PropertyName), propertyInfo.Name);
            }

            model.PropertyChanged += ModelOnPropertyChanged;
        }

        private void SetupAlsoNotify()
        {
            var alsoNotifyProperties = GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(AlsoNotifyAttribute)));
            foreach (var propertyInfo in alsoNotifyProperties)
            {
                var attr = propertyInfo.GetCustomAttribute<AlsoNotifyAttribute>();
                _alsoNotifyCache.Add(propertyInfo.Name, attr.AlsoNotify);
            }
        }

        /// <summary>
        /// When a model property changes, raise <see cref="PropertyChanged"/> for the corresponding property marked with <see cref="PropertyChangeBindingAttribute"/> 
        /// and also properties in <see cref="AlsoNotifyAttribute"/>.
        /// </summary>
        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (!_modelToPropertyName.TryGetValue((sender, propertyChangedEventArgs.PropertyName), out var propertyName)) return;
            RaisePropertyChanged(propertyName);

            if (!_alsoNotifyCache.TryGetValue(nameof(propertyName), out var alsoNotfify)) return;
            foreach (var name in alsoNotfify)
            {
                RaisePropertyChanged(name);
            }
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
    }

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
