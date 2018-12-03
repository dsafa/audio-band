using System;
using AudioBand.AudioSource;
using AudioBand.Models;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for <see cref="AudioSourceSetting"/>.
    /// </summary>
    internal class AudioSourceSettingVM : ViewModelBase<AudioSourceSetting>
    {
        private readonly AudioSourceSettingInfo _settingInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceSettingVM"/> class
        /// with the model, setting information and if it was saved,
        /// </summary>
        /// <param name="model">The <see cref="AudioSource"/>.</param>
        /// <param name="settingInfo">The <see cref="AudioSourceSettingInfo"/>.</param>
        /// <param name="saved">If this setting was saved.</param>
        public AudioSourceSettingVM(AudioSourceSetting model, AudioSourceSettingInfo settingInfo, bool saved = true)
            : base(model)
        {
            _settingInfo = settingInfo;
            model.Value = settingInfo.ConvertToSettingType(Model.Value);

            // If sensitive data and was not saved from before, don't automatically remember it
            if (Sensitive && !saved)
            {
                Remember = false;
            }

            ApplyChanges();
        }

        /// <summary>
        /// Gets the setting name.
        /// </summary>
        [PropertyChangeBinding(nameof(AudioSourceSetting.Name))]
        public string Name => Model.Name;

        /// <summary>
        /// Gets or sets the value of the setting, can be any basic type.
        /// </summary>
        [PropertyChangeBinding(nameof(AudioSourceSetting.Value))]
        public object Value
        {
            get => Model.Value;
            set
            {
                var res = _settingInfo.ValidateSetting(value);
                if (res.IsValid)
                {
                    SetProperty(nameof(Model.Value), value);
                }
                else
                {
                    RaiseValidationError(res.ErrorMessage);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets whether or not to save the data.
        /// </summary>
        [PropertyChangeBinding(nameof(AudioSourceSetting.Remember))]
        public bool Remember
        {
            get => Model.Remember;
            private set => SetProperty(nameof(Model.Remember), value);
        }

        /// <summary>
        /// Gets a value indicating whether the user shouldn't modify it.
        /// </summary>
        public bool ReadOnly => _settingInfo.Attribute.Options.HasFlag(SettingOptions.ReadOnly);

        /// <summary>
        /// Gets a value indicating whether the setting is visible in the settings window.
        /// </summary>
        public bool Visible => !_settingInfo.Attribute.Options.HasFlag(SettingOptions.Hidden);

        /// <summary>
        /// Gets a value indicating whether the setting is sensitive.
        /// </summary>
        public bool Sensitive => _settingInfo.Attribute.Options.HasFlag(SettingOptions.Sensitive);

        /// <summary>
        /// Gets the name of the property that the setting is attached to.
        /// </summary>
        public string PropertyName => _settingInfo.PropertyName;

        /// <summary>
        /// Gets the type of the setting.
        /// </summary>
        public Type SettingType => _settingInfo.PropertyType;

        /// <summary>
        /// Gets the description of the setting.
        /// </summary>
        public string Description => _settingInfo.Attribute.Description;

        /// <summary>
        /// Gets the priority of the setting.
        /// </summary>
        public int Priority => _settingInfo.Attribute.Priority;

        /// <summary>
        /// Applies the value to the audio source
        /// </summary>
        public void ApplyChanges()
        {
            try
            {
                _settingInfo.UpdateAudioSource(Value);
            }
            catch (Exception)
            {
                RaiseValidationError("An unexpected error occured. Check the log for more details.");
            }
        }

        /// <summary>
        /// Notify that the value of the setting was changed by the audio source.
        /// </summary>
        public void ValueChanged()
        {
            Model.Value = _settingInfo.GetValue();
        }

        /// <inheritdoc/>
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            if (ReadOnly)
            {
                return;
            }

            ApplyChanges();
        }
    }
}
