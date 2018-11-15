using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AudioBand.Commands;
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
}
