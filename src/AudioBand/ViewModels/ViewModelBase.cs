using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AudioBand.Commands;
using AudioBand.Logging;
using AutoMapper;
using FastMember;
using NLog;
using PubSub.Extension;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Base class for view models with automatic support for
    /// <see cref="INotifyPropertyChanged"/>, <see cref="IResettableObject"/>, <see cref="INotifyDataErrorInfo"/> and commands.
    /// </summary>
    /// <remarks>
    /// <see cref="IEditableObject"/> is implemented explicitly through methods instead of the interface because of issues with the methods being automatically called.
    /// </remarks>
    public abstract class ViewModelBase : INotifyPropertyChanged, IResettableObject, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, IEnumerable<string>> _propertyErrors = new Dictionary<string, IEnumerable<string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
            Logger = AudioBandLogManager.GetLogger(GetType().FullName);
            Accessor = TypeAccessor.Create(GetType());

            BeginEditCommand = new RelayCommand(o => BeginEdit());
            EndEditCommand = new RelayCommand(o => EndEdit());
            CancelEditCommand = new RelayCommand(o => CancelEdit());
            ResetCommand = new RelayCommand(o => Reset());

            SetupAlsoNotify();
        }

        /// <inheritdoc cref="INotifyDataErrorInfo.ErrorsChanged"/>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc cref="INotifyDataErrorInfo.HasErrors"/>
        public bool HasErrors => _propertyErrors.Any(entry => entry.Value?.Any() ?? false);

        /// <summary>
        /// Gets the command to start editing.
        /// </summary>
        public RelayCommand BeginEditCommand { get; }

        /// <summary>
        /// Gets the command to end editing.
        /// </summary>
        public RelayCommand EndEditCommand { get; }

        /// <summary>
        /// Gets the command to cancel edit.
        /// </summary>
        public RelayCommand CancelEditCommand { get; }

        /// <summary>
        /// Gets the command to reset the state.
        /// </summary>
        public RelayCommand ResetCommand { get; }

        /// <summary>
        /// Gets a value indicating whether the current object is being edited.
        /// </summary>
        public bool IsEditing { get; private set; }

        /// <summary>
        /// Gets the map from [model property name] to [other vm property names]
        /// </summary>
        protected Dictionary<string, string[]> AlsoNotifyMap { get; } = new Dictionary<string, string[]>();

        /// <summary>
        /// Gets the logger for the view model
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets the type accessor for this object.
        /// </summary>
        protected TypeAccessor Accessor { get; }

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
            this.Subscribe<EditMessage>(EditMessageOnPublished);
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
            this.Unsubscribe<EditMessage>();
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
            this.Unsubscribe<EditMessage>();
        }

        /// <inheritdoc cref="IResettableObject.Reset"/>
        public void Reset()
        {
            OnReset();
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
        protected void ResetObject<T>(T obj)
            where T : new()
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

        private void SetupAlsoNotify()
        {
            var alsoNotifyProperties = Accessor.GetMembers().Where(m => m.IsDefined(typeof(AlsoNotifyAttribute)));
            foreach (var propertyInfo in alsoNotifyProperties)
            {
                var attr = (AlsoNotifyAttribute)propertyInfo.GetAttribute(typeof(AlsoNotifyAttribute), true);
                AlsoNotifyMap.Add(propertyInfo.Name, attr.AlsoNotify);
            }
        }

        private void EditMessageOnPublished(EditMessage message)
        {
            switch (message)
            {
                case EditMessage.AcceptEdits:
                    EndEdit();
                    break;
                case EditMessage.CancelEdits:
                    CancelEdit();
                    break;
            }
        }
    }
}
