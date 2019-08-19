using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using AudioBand.Commands;
using AudioBand.Logging;
using AudioBand.Messages;
using AutoMapper;
using NLog;

namespace AudioBand.UI
{
    /// <summary>
    /// Base class for view models. Extends <see cref="ValidatingViewModelBase"/> with support for
    /// <see cref="IEditableObject"/>, <see cref="IResettableObject"/>, and associated commands.
    /// </summary>
    public abstract class ViewModelBase : ValidatingViewModelBase, IEditableObject, IResettableObject
    {
        private readonly Dictionary<Type, MapperConfiguration> _mapperCache = new Dictionary<Type, MapperConfiguration>();
        private readonly HashSet<string> _trackedProperties = new HashSet<string>();
        private bool _isEditing;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
            Logger = AudioBandLogManager.GetLogger(GetType().FullName);

            BeginEditCommand = new RelayCommand(BeginEdit);
            EndEditCommand = new RelayCommand(EndEdit);
            CancelEditCommand = new RelayCommand(CancelEdit);
            ResetCommand = new RelayCommand(Reset);

            GetTrackingProperties();
        }

        /// <summary>
        /// Occurs when <see cref="IsEditing"/> changes.
        /// </summary>
        public event EventHandler IsEditingChanged;

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

        /// <summary>
        /// Gets a value indicating whether the object is being edited.
        /// </summary>
        /// <value>True when <see cref="BeginEdit"/> is called and <see cref="EndEdit"/> or <see cref="CancelEdit"/> has not been called.</value>
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

        /// <summary>
        /// Gets the message bus.
        /// </summary>
        protected IMessageBus MessageBus { get; private set; }

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
            RaisePropertyChangedAll();
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
            RaisePropertyChangedAll();
        }

        /// <inheritdoc cref="IResettableObject.Reset"/>
        public void Reset()
        {
            BeginEdit();
            OnReset();
            RaisePropertyChangedAll();
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
            MessageBus?.Publish(default(EditStartMessage));
        }

        /// <summary>
        /// Resets an object to its default state.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="obj">Object to reset.</param>
        protected void ResetObject<T>(T obj)
            where T : new()
        {
            MapSelf(new T(), obj);
        }

        /// <summary>
        /// Maps an object from to another instance of the same type.
        /// </summary>
        /// <typeparam name="T">The type of the object to map.</typeparam>
        /// <param name="objectFrom">The object to map.</param>
        /// <param name="objectTo">The other instance of the object to map to.</param>
        protected void MapSelf<T>(T objectFrom, T objectTo)
        {
            if (!_mapperCache.ContainsKey(typeof(T)))
            {
                _mapperCache.Add(typeof(T), new MapperConfiguration(cfg => cfg.CreateMap<T, T>()));
            }

            _mapperCache[typeof(T)].CreateMapper().Map<T, T>(objectFrom, objectTo);
        }

        /// <inheritdoc />
        protected override void OnPropertyChanging(string propertyName)
        {
            base.OnPropertyChanging(propertyName);
            if (_trackedProperties.Contains(propertyName))
            {
                BeginEdit();
            }
        }

        /// <summary>
        /// Use the message bus for listening and publishing edit events.
        /// </summary>
        /// <param name="messageBus">The message bus to use.</param>
        protected void UseMessageBus(IMessageBus messageBus)
        {
            MessageBus = messageBus;
            messageBus.Subscribe<EditEndMessage>(OnEditEndMessagePublished);
        }

        private void GetTrackingProperties()
        {
            foreach (var member in Accessor.GetMembers())
            {
                if (member.IsDefined(typeof(TrackStateAttribute)))
                {
                    _trackedProperties.Add(member.Name);
                }
            }
        }

        private void OnEditEndMessagePublished(EditEndMessage message)
        {
            if (message == EditEndMessage.Cancelled)
            {
                CancelEdit();
            }
            else if (message == EditEndMessage.Accepted)
            {
                EndEdit();
            }
        }
    }
}
