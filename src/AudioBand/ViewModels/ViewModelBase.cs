using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AudioBand.Commands;
using AudioBand.Models;
using NLog;
using AutoMapper;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Base class for view models. With automatic support for 
    /// <see cref="INotifyPropertyChanged"/>, <see cref="IEditableObject"/>, <see cref="IResettableObject"/>, <see cref="INotifyDataErrorInfo"/> and commands.
    /// </summary>
    internal abstract class ViewModelBase : INotifyPropertyChanged, IEditableObject, IResettableObject, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, IEnumerable<string>> _propertyErrors = new Dictionary<string, IEnumerable<string>>();

        /// <inheritdoc cref="INotifyDataErrorInfo.GetErrors"/>
        public IEnumerable GetErrors(string propertyName)
        {
            if (_propertyErrors.TryGetValue(propertyName, out var errors) && errors.Any())
            {
                return _propertyErrors[propertyName];
            }

            return null;
        }

        ///<inheritdoc cref="INotifyDataErrorInfo.HasErrors"/>
        public bool HasErrors => _propertyErrors.Any(entry => entry.Value?.Any() ?? false);

        /// <inheritdoc cref="INotifyDataErrorInfo.ErrorsChanged"/>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Command to start editing.
        /// </summary>
        public RelayCommand BeginEditCommand { get; }

        /// <summary>
        /// Command to end editing.
        /// </summary>
        public RelayCommand EndEditCommand { get; }

        /// <summary>
        /// Command to cancel edit.
        /// </summary>
        public RelayCommand CancelEditCommand { get; }

        /// <summary>
        /// Command to reset the state to default.
        /// </summary>
        public RelayCommand ResetCommand { get; }

        /// <inheritdoc cref="IEditableObject.BeginEdit"/>
        public void BeginEdit()
        {
            OnBeginEdit();
        }

        /// <inheritdoc cref="IEditableObject.EndEdit"/>
        public void EndEdit()
        {
            OnEndEdit();
        }
        /// <inheritdoc cref="IEditableObject.CancelEdit"/>
        public void CancelEdit()
        {
            OnCancelEdit();
        }

        /// <inheritdoc cref="IResettableObject.Reset"/>
        public void Reset()
        {
            OnReset();
        }

        /// <summary>
        /// Map from [model property name] to [other vm property names]
        /// </summary>
        protected Dictionary<string, string[]> AlsoNotifyMap { get; } = new Dictionary<string, string[]>();

        protected Logger Logger { get; }

        protected ViewModelBase()
        {
            BeginEditCommand = new RelayCommand(o => BeginEdit());
            EndEditCommand = new RelayCommand(o => EndEdit());
            CancelEditCommand = new RelayCommand(o => CancelEdit());
            ResetCommand = new RelayCommand(o => Reset());
            SetupAlsoNotify();
            Logger = LogManager.GetLogger(GetType().FullName);
        }

        /// <summary>
        /// Notifies subsribers to a property change.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Resets an object to its default state.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="obj">Object to reset.</param>
        protected void ResetObject<T>(T obj) where T: new()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, T>()).CreateMapper();
            mapper.Map<T, T>(new T(), obj);
        }

        /// <summary>
        /// Called when <see cref="Reset"/> is called.
        /// </summary>
        protected virtual void OnReset() { }

        /// <summary>
        /// Called when <see cref="CancelEdit"/> is called.
        /// </summary>
        protected virtual void OnCancelEdit() { }

        /// <summary>
        /// Called when <see cref="EndEdit"/> is called.
        /// </summary>
        protected virtual void OnEndEdit() { }

        /// <summary>
        /// Called when <see cref="BeginEdit"/> is called.
        /// </summary>
        protected virtual void OnBeginEdit() { }

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

            if (AlsoNotifyMap.TryGetValue(propertyName, out var alsoNotify))
            {
                foreach (var propName in alsoNotify)
                {
                    RaisePropertyChanged(propName);
                }
            }

            return true;
        }

        /// <summary>
        /// Raises a <see cref="ErrorsChanged"/> event.
        /// </summary>
        /// <param name="errors">Errors that occured during validation.</param>
        /// <param name="propertyName">Property that failed validation.</param>
        protected void RaiseValidationError(IEnumerable<string> errors, [CallerMemberName] string propertyName = null)
        {
            _propertyErrors.Clear();
            _propertyErrors[propertyName] = errors;
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises a <see cref="ErrorsChanged"/> event.
        /// </summary>
        /// <param name="error">Error that occured during validation.</param>
        /// <param name="propertyName">Property that failed validation.</param>
        protected void RaiseValidationError(string error, [CallerMemberName] string propertyName = null)
        {
            RaiseValidationError(new[] {error}, propertyName);
        }

        /// <summary>
        /// Raises a <see cref="ErrorsChanged"/> event.
        /// </summary>
        /// <param name="e">Exception that occured during validation.</param>
        /// <param name="propertyName">Property that failed validation.</param>
        protected void RaiseValidationError(Exception e, [CallerMemberName] string propertyName = null)
        {
            RaiseValidationError(e.ToString(), propertyName);
        }

        private void SetupAlsoNotify()
        {
            var alsoNotifyProperties = GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(AlsoNotifyAttribute)));
            foreach (var propertyInfo in alsoNotifyProperties)
            {
                var attr = propertyInfo.GetCustomAttribute<AlsoNotifyAttribute>();
                AlsoNotifyMap.Add(propertyInfo.Name, attr.AlsoNotify);
            }
        }
    }

    /// <summary>
    /// Base class for a viewmodel with a model.
    /// </summary>
    /// <typeparam name="TModel">Type of the model</typeparam>
    internal abstract class ViewModelBase<TModel> : ViewModelBase
    where TModel: ModelBase, new()
    {
        // Mapping from a model and model property to the viewmodel property name
        private readonly Dictionary<(object model, string modelPropName), string> _modelToPropertyName = new Dictionary<(object model, string modelPropName), string>();
        private readonly Dictionary<(object model, string property), PropertyInfo> _modelPropCache = new Dictionary<(object model, string property), PropertyInfo>();
        private TModel _backup;
        private readonly MapperConfiguration _mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<TModel, TModel>());

        /// <summary>
        /// Get the model representation of this viewmodel. By default it returns <see cref="Model"/>.
        /// </summary>
        /// <returns>A model representation</returns>
        public virtual TModel GetModel()
        {
            return Model;
        }

        /// <summary>
        /// Model associated with this view model.
        /// </summary>
        protected TModel Model { get; set; }

        protected ViewModelBase(TModel model)
        {
            Model = model;
            SetupModelBindings(Model);
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
            var prop = _modelPropCache[(Model, modelPropertyName)];
            prop.SetValue(Model, newValue);
            var currentValue = (TValue)prop.GetValue(Model);
            return EqualityComparer<TValue>.Default.Equals(currentValue, newValue);
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

                var modelProp = model.GetType().GetProperty(bindingAttr.PropertyName);
                _modelPropCache.Add((model, bindingAttr.PropertyName), modelProp);
            }

            model.PropertyChanged += ModelOnPropertyChanged;
        }

        protected override void OnReset()
        {
            base.OnReset();

            ResetObject(Model);
        }

        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();

            if (_backup == null)
            {
                Logger.Warn("Backup is null. Begin edit wasn't called.");
            }

            _mapperConfiguration.CreateMapper().Map(_backup, Model);
            _backup = null;
        }

        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            _backup = null;
        }

        protected override void OnBeginEdit()
        {
            base.OnBeginEdit();
            _backup = new TModel();
            _mapperConfiguration.CreateMapper().Map(Model, _backup);
        }

        /// <summary>
        /// When a model property changes, raise <see cref="PropertyChanged"/> for the corresponding property marked with <see cref="PropertyChangeBindingAttribute"/> 
        /// and also properties in <see cref="AlsoNotifyAttribute"/>.
        /// </summary>
        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (!_modelToPropertyName.TryGetValue((sender, propertyChangedEventArgs.PropertyName), out var propertyName)) return;
            RaisePropertyChanged(propertyName);

            if (!AlsoNotifyMap.TryGetValue(nameof(propertyName), out var alsoNotfify)) return;
            foreach (var name in alsoNotfify)
            {
                RaisePropertyChanged(name);
            }
        }

        /// <summary>
        /// Try to load image from path or default image if invalid file.
        /// </summary>
        /// <param name="path">Path to load image from.</param>
        /// <param name="defaultImage">Default image to use if unable to laod file.</param>
        /// <returns>The loaded image or default image.</returns>
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
