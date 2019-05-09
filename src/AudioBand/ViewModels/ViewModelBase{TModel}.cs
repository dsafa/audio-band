using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AudioBand.Models;
using AutoMapper;
using FastMember;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Base class for a viewmodel with a model.
    /// </summary>
    /// <typeparam name="TModel">Type of the model.</typeparam>
    public abstract class ViewModelBase<TModel> : ViewModelBase
        where TModel : ModelBase, new()
    {
        // Mapping from a model and model property to the viewmodel property name
        private readonly Dictionary<(object model, string modelPropName), List<string>> _modelToPropertyName = new Dictionary<(object model, string modelPropName), List<string>>();
        private readonly Dictionary<object, ObjectAccessor> _modelToAccessor = new Dictionary<object, ObjectAccessor>();
        private readonly MapperConfiguration _mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<TModel, TModel>());
        private TModel _backup;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{TModel}"/> class
        /// with the model and hooks up change notifications.
        /// </summary>
        /// <param name="model">The model.</param>
        protected ViewModelBase(TModel model)
        {
            Model = model;
            SetupModelBindings(Model);
        }

        /// <summary>
        /// Gets or sets the model associated with this view model.
        /// </summary>
        protected TModel Model { get; set; }

        /// <summary>
        /// Get the model representation of this viewmodel. By default it returns <see cref="Model"/>.
        /// </summary>
        /// <returns>A model representation.</returns>
        public virtual TModel GetModel()
        {
            return Model;
        }

        /// <summary>
        /// Sets the property in the <see cref="Model"/>.
        /// </summary>
        /// <typeparam name="TValue">Type of the property to set.</typeparam>
        /// <param name="modelPropertyName">Name of the property to set.</param>
        /// <param name="newValue">New value to set.</param>
        /// <param name="trackChanges">True if changes are tracked by calling <see cref="ViewModelBase.BeginEdit"/>.</param>
        /// <param name="propertyName">Name of the property to notify with.</param>
        /// <returns>Returns true if new value was set.</returns>
        protected bool SetProperty<TValue>(string modelPropertyName, TValue newValue, bool trackChanges = true, [CallerMemberName] string propertyName = null)
        {
            var currentModelValue = (TValue)_modelToAccessor[Model][modelPropertyName];
            if (EqualityComparer<TValue>.Default.Equals(currentModelValue, newValue))
            {
                return false;
            }

            if (trackChanges)
            {
                BeginEdit();
            }

            _modelToAccessor[Model][modelPropertyName] = newValue;
            return true;
        }

        /// <summary>
        /// Performs setup to recieve change notifications from <paramref name="model"/> and wire it to properties marked with <see cref="PropertyChangeBindingAttribute"/>.
        /// </summary>
        /// <param name="model">Model to subscribe to for <see cref="INotifyPropertyChanged.PropertyChanged"/>.</param>
        /// <typeparam name="T">The type of the model.</typeparam>
        protected void SetupModelBindings<T>(T model)
            where T : ModelBase
        {
            _modelToAccessor.Add(model, ObjectAccessor.Create(model));

            var members = Accessor.GetMembers().Where(m => m.IsDefined(typeof(PropertyChangeBindingAttribute)));
            foreach (var member in members)
            {
                var bindingAttr = (PropertyChangeBindingAttribute)member.GetAttribute(typeof(PropertyChangeBindingAttribute), true);
                var key = (model, bindingAttr.PropertyName);
                if (_modelToPropertyName.ContainsKey(key))
                {
                    _modelToPropertyName[key].Add(member.Name);
                }
                else
                {
                    _modelToPropertyName.Add(key, new List<string> { member.Name });
                }
            }

            model.PropertyChanged += ModelOnPropertyChanged;
        }

        /// <summary>
        /// Unbinds the model.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="model">The model to unbind.</param>
        protected void UnbindModel<T>(T model)
            where T : ModelBase
        {
            if (model == null)
            {
                return;
            }

            _modelToAccessor.Remove(model);
            model.PropertyChanged -= ModelOnPropertyChanged;
            var keys = _modelToPropertyName.Keys.Where(key => key.model == model).ToList();
            foreach (var key in keys)
            {
                _modelToPropertyName.Remove(key);
            }
        }

        /// <summary>
        /// Unbinds the current <see cref="Model"/> and binds to new one.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="newModel">The new model to replace..</param>
        protected void ReplaceModel<T>(T newModel)
            where T : TModel
        {
            UnbindModel(Model);
            Model = newModel;
            SetupModelBindings(Model);
            RaisePropertyChangedAll();
        }

        /// <summary>
        /// Resets the <see cref="Model"/> to a state as if it was just instantiated.
        /// </summary>
        protected override void OnReset()
        {
            base.OnReset();
            BeginEdit();
            ResetObject(Model);
        }

        /// <summary>
        /// Cancels edits to the <see cref="Model"/>.
        /// </summary>
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            _mapperConfiguration.CreateMapper().Map(_backup, Model);
            _backup = null;
        }

        /// <summary>
        /// Accepts edits to the <see cref="Model"/>.
        /// </summary>
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            _backup = null;
        }

        /// <summary>
        /// Starts tracking changes to the <see cref="Model"/>.
        /// </summary>
        protected override void OnBeginEdit()
        {
            base.OnBeginEdit();
            _backup = new TModel();
            _mapperConfiguration.CreateMapper().Map(Model, _backup);
        }

        /// <summary>
        /// Called when a model property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected virtual void OnModelPropertyChanged(string propertyName) { }

        /// <summary>
        /// When a model property changes, raise <see cref="INotifyPropertyChanged.PropertyChanged"/> for the corresponding property marked with <see cref="PropertyChangeBindingAttribute"/>
        /// and also properties in <see cref="AlsoNotifyAttribute"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="propertyChangedEventArgs">The event args.</param>
        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (!_modelToPropertyName.TryGetValue((sender, propertyChangedEventArgs.PropertyName), out var propertyNames))
            {
                return;
            }

            foreach (var propertyName in propertyNames)
            {
                RaisePropertyChanged(propertyName);
                OnModelPropertyChanged(propertyName);

                if (!AlsoNotifyMap.TryGetValue(propertyName, out var alsoNotfify))
                {
                    continue;
                }

                foreach (var name in alsoNotfify)
                {
                    RaisePropertyChanged(name);
                }
            }
        }
    }
}
