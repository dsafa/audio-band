using System;
using AudioBand.AudioSource;
using AudioBand.Models;
using AudioSourceHost;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for <see cref="AudioSourceSetting"/>.
    /// </summary>
    public class AudioSourceSettingVM : ViewModelBase<AudioSourceSetting>
    {
        private readonly AudioSourceSettingAttribute _settingAttribute;
        private readonly IInternalAudioSource _audioSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceSettingVM"/> class
        /// with the model, setting information and if it was saved,
        /// </summary>
        /// <param name="audioSource">The associated <see cref="IInternalAudioSource"/>.</param>
        /// <param name="model">The <see cref="AudioSource"/>.</param>
        /// <param name="settingAttribute">The <see cref="AudioSourceSettingAttribute"/>.</param>
        /// <param name="saved">If this setting was saved.</param>
        public AudioSourceSettingVM(IInternalAudioSource audioSource, AudioSourceSetting model, AudioSourceSettingAttribute settingAttribute, bool saved = true)
            : base(model)
        {
            _settingAttribute = settingAttribute;
            _audioSource = audioSource;

            // Model value was deserialized from string maybe so change to correct type
            model.Value = TypeConvertHelper.ConvertToType(model.Value, SettingType);

            // If sensitive data and was not saved from before, don't automatically remember it
            if (Sensitive && !saved)
            {
                Remember = false;
            }
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
            set => SetProperty(nameof(Model.Value), value);
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
        public bool ReadOnly => _settingAttribute.Options.HasFlag(SettingOptions.ReadOnly);

        /// <summary>
        /// Gets a value indicating whether the setting is visible in the settings window.
        /// </summary>
        public bool Visible => !_settingAttribute.Options.HasFlag(SettingOptions.Hidden);

        /// <summary>
        /// Gets a value indicating whether the setting is sensitive.
        /// </summary>
        public bool Sensitive => _settingAttribute.Options.HasFlag(SettingOptions.Sensitive);

        /// <summary>
        /// Gets the type of the setting.
        /// </summary>
        public Type SettingType => _audioSource.GetSettingType(Name);

        /// <summary>
        /// Gets the description of the setting.
        /// </summary>
        public string Description => _settingAttribute.Description;

        /// <summary>
        /// Gets the priority of the setting.
        /// </summary>
        public int Priority => _settingAttribute.Priority;

        /// <summary>
        /// Applies the value to the audio source
        /// </summary>
        public void ApplyChanges()
        {
            try
            {
                _audioSource[Name] = Value;
            }
            catch (Exception)
            {
                RaiseValidationError("An unexpected error occured. Check the log for more details.");
            }
        }

        /// <summary>
        /// Notify that the value of the setting was changed by the audio source so we have to update the model.
        /// </summary>
        public void ValueChanged()
        {
            Model.Value = _audioSource[Name];
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
