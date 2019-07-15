using System;
using AudioBand.AudioSource;
using AudioBand.Messages;
using AudioBand.Models;
using AudioSourceHost;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for <see cref="AudioSourceSetting"/>. Represents one key-value pair.
    /// </summary>
    public class AudioSourceSettingKeyValue : ViewModelBase
    {
        private readonly AudioSourceSettingAttribute _settingAttribute;
        private readonly IInternalAudioSource _audioSource;
        private readonly AudioSourceSetting _model = new AudioSourceSetting();
        private readonly AudioSourceSetting _originalSource;
        private readonly AudioSourceSetting _backup = new AudioSourceSetting();

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceSettingKeyValue"/> class
        /// with the model, setting information and if it was saved.
        /// </summary>
        /// <param name="audioSource">The associated <see cref="IInternalAudioSource"/>.</param>
        /// <param name="source">The setting model.</param>
        /// <param name="settingAttribute">The <see cref="AudioSourceSettingAttribute"/>.</param>
        public AudioSourceSettingKeyValue(IInternalAudioSource audioSource, AudioSourceSetting source, AudioSourceSettingAttribute settingAttribute)
        {
            _settingAttribute = settingAttribute;
            _audioSource = audioSource;
            _originalSource = source;

            MapSelf(_originalSource, _model);

            // Model value was deserialized from string maybe so change to correct type
            source.Value = TypeConvertHelper.ConvertToType(source.Value, SettingType);
        }

        /// <summary>
        /// Gets the setting name.
        /// </summary>
        public string Name => _model.Name;

        /// <summary>
        /// Gets or sets the value of the setting, can be any basic type.
        /// </summary>
        [TrackState]
        public object Value
        {
            get => _model.Value;
            set => SetProperty(_model, nameof(_model.Value), value);
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
        /// Applies the value to the audio source.
        /// </summary>
        public void PropagateSettingToAudioSource()
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
        /// When the audio sources updates the setting itself instead of from audioband. Sync the changes back to the model.
        /// </summary>
        public void SyncToModel()
        {
            MapSelf(_model, _originalSource);
        }

        /// <inheritdoc />
        protected override void OnBeginEdit()
        {
            base.OnBeginEdit();
            MapSelf(_model, _backup);
        }

        /// <inheritdoc />
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            if (ReadOnly)
            {
                return;
            }

            MapSelf(_backup, _model);
        }

        /// <inheritdoc/>
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            if (ReadOnly)
            {
                return;
            }

            SyncToModel();
            PropagateSettingToAudioSource();
        }
    }
}
