using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AudioBand.ChangeTracking;
using AudioBand.Commands;
using AudioBand.Logging;
using AutoMapper;
using NLog;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Base class for view models. Extends <see cref="ObservableObject"/> with automatic support for
    /// <see cref="IChangeTrackable"/>, <see cref="IResettableObject"/>, <see cref="INotifyDataErrorInfo"/> and associated commands.
    /// </summary>
    public abstract class ViewModelBase : ObservableObject, IChangeTrackable, IResettableObject, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, IEnumerable<string>> _propertyErrors = new Dictionary<string, IEnumerable<string>>();
        private bool _isEditing;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
            Logger = AudioBandLogManager.GetLogger(GetType().FullName);

            BeginEditCommand = new RelayCommand(o => BeginEdit());
            EndEditCommand = new RelayCommand(o => EndEdit());
            CancelEditCommand = new RelayCommand(o => CancelEdit());
            ResetCommand = new RelayCommand(o => Reset());
        }

        /// <inheritdoc cref="INotifyDataErrorInfo.ErrorsChanged"/>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <inheritdoc />
        public event EventHandler IsEditingChanged;

        /// <inheritdoc cref="INotifyDataErrorInfo.HasErrors"/>
        public bool HasErrors => _propertyErrors.Any(entry => entry.Value?.Any() ?? false);

        /// <summary>
        /// Gets the command to start editing.
        /// </summary>
        public ICommand BeginEditCommand { get; }

        /// <summary>
        /// Gets the command to end editing.
        /// </summary>
        public ICommand EndEditCommand { get; }

        /// <summary>
        /// Gets the command to cancel edit.
        /// </summary>
        public ICommand CancelEditCommand { get; }

        /// <summary>
        /// Gets the command to reset the state.
        /// </summary>
        public ICommand ResetCommand { get; }

        /// <inheritdoc />
        public bool IsEditing
        {
            get => _isEditing;
            private set
            {
                if (SetProperty(ref _isEditing, value))
                {
                    IsEditingChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets the logger for the view model.
        /// </summary>
        protected ILogger Logger { get; }

        /// <inheritdoc cref="INotifyDataErrorInfo.GetErrors"/>
        public IEnumerable GetErrors(string propertyName)
        {
            if (_propertyErrors.TryGetValue(propertyName, out var errors) && errors.Any())
            {
                return _propertyErrors[propertyName];
            }

            return null;
        }

        /// <summary>
        /// Begins editing on this object.
        /// </summary>
        public void BeginEdit()
        {
            if (IsEditing)
            {
                return;
            }

            Logger.Debug("Starting edit");
            OnBeginEdit();
            IsEditing = true;
        }

        /// <summary>
        /// Ends and commits the changes to this object since the last <see cref="BeginEdit"/>.
        /// </summary>
        public void EndEdit()
        {
            if (!IsEditing)
            {
                return;
            }

            Logger.Debug("Ending edit");
            OnEndEdit();
            IsEditing = false;
        }

        /// <summary>
        /// Cancels all changes to this object since the last <see cref="BeginEdit"/>.
        /// </summary>
        public void CancelEdit()
        {
            if (!IsEditing)
            {
                return;
            }

            Logger.Debug("Cancelling edit");
            OnCancelEdit();
            IsEditing = false;
        }

        /// <inheritdoc cref="IResettableObject.Reset"/>
        public void Reset()
        {
            OnReset();
        }

        /// <summary>
        /// Called when <see cref="Reset"/> is called.
        /// </summary>
        protected virtual void OnReset()
        {
        }

        /// <summary>
        /// Called when <see cref="CancelEdit"/> is called.
        /// </summary>
        protected virtual void OnCancelEdit()
        {
        }

        /// <summary>
        /// Called when <see cref="EndEdit"/> is called.
        /// </summary>
        protected virtual void OnEndEdit()
        {
        }

        /// <summary>
        /// Called when <see cref="BeginEdit"/> is called.
        /// </summary>
        protected virtual void OnBeginEdit()
        {
        }

        /// <summary>
        /// Resets an object to its default state.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="obj">Object to reset.</param>
        protected void ResetObject<T>(T obj)
            where T : new()
        {
            MapType(new T(), obj);
        }

        /// <summary>
        /// Maps an object from to another instance of the same type.
        /// </summary>
        /// <typeparam name="T">The type of the object to map.</typeparam>
        /// <param name="objectFrom">The object to map.</param>
        /// <param name="objectTo">The other instance of the object to map to.</param>
        protected void MapType<T>(T objectFrom, T objectTo)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, T>()).CreateMapper();
            mapper.Map<T, T>(objectFrom, objectTo);
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
            RaiseValidationError(new[] { error }, propertyName);
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

        /// <summary>
        /// Clears all validation errors.
        /// </summary>
        protected void ClearErrors()
        {
            _propertyErrors.Clear();
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(string.Empty));
        }

        /// <summary>
        /// Clears validation errors for a property.
        /// </summary>
        /// <param name="propertyName">The name of the property to clear.</param>
        protected void ClearError([CallerMemberName] string propertyName = null)
        {
            _propertyErrors.Remove(propertyName);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the <paramref name="field"/> to the <paramref name="newValue"/> if they are different and raise <see cref="PropertyChanged"/>
        /// for the property and other properties marked with the <see cref="AlsoNotifyAttribute"/>. And invokes <see cref="BeginEdit"/>.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">Old value.</param>
        /// <param name="newValue">New value.</param>
        /// <param name="propertyName">Name of the property that changed.</param>
        /// <returns>True if the property changed.</returns>
        protected bool SetPropertyAndTrack<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            var didChange = SetProperty(ref field, newValue, propertyName);
            if (didChange)
            {
                BeginEdit();
            }

            return didChange;
        }

        /// <summary>
        /// Sets the <paramref name="modelPropertyName"/> property of the <paramref name="model"/> to the <paramref name="newValue"/>
        /// if they are different and raise <see cref="PropertyChanged"/> for the property and other properties marked with the <see cref="AlsoNotifyAttribute"/>.
        /// And invokes <see cref="BeginEdit"/>.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="model">The model instance.</param>
        /// <param name="modelPropertyName">The property name of the model.</param>
        /// <param name="newValue">The new value to set.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>True if the property changed.</returns>
        protected bool SetPropertyAndTrack<TModel, TValue>(TModel model, string modelPropertyName, TValue newValue, [CallerMemberName] string propertyName = null)
        {
            var didChange = SetProperty(model, modelPropertyName, newValue, propertyName);
            if (didChange)
            {
                BeginEdit();
            }

            return didChange;
        }

        /// <summary>
        /// Registers a model for automatic <see cref="BeginEdit"/>, <see cref="Reset"/>, <see cref="EndEdit"/> and <see cref="CancelEdit"/>.
        /// </summary>
        /// <param name="model">The model to register.</param>
        protected void RegisterModelForChangeTracking(object model)
        {
            _trackedModels.Add(model);
            _backupModels.Add(_trackedModels, model);
        }

        /// <summary>
        /// Removes a model for automatic <see cref="BeginEdit"/>, <see cref="Reset"/>, <see cref="EndEdit"/> and <see cref="CancelEdit"/>.
        /// </summary>
        /// <param name="model">The model to unregister.</param>
        protected void UnregisterModelFromChangeTracking(object model)
        {
            _trackedModels.Remove(model);
        }
    }
}
