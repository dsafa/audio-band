using AudioBand.AudioSource;
using AudioBand.Models;
using System;
using AudioBand.Extensions;

namespace AudioBand.ViewModels
{
    internal class AudioSourceSettingVM : ViewModelBase<AudioSourceSetting>
    {
        private readonly AudioSourceSettingInfo _settingInfo;

        /// <summary>
        /// Setting name
        /// </summary>
        [PropertyChangeBinding(nameof(AudioSourceSetting.Name))]
        public string Name => Model.Name;

        /// <summary>
        /// Value of the setting, can be any basic type
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
        /// Whether or not to save the data
        /// </summary>
        [PropertyChangeBinding(nameof(AudioSourceSetting.Remember))]
        public bool Remember
        {
            get => Model.Remember;
            set => SetProperty(nameof(Model.Remember), value);
        }

        public bool ReadOnly => _settingInfo.Attribute.Options.HasFlag(SettingOptions.ReadOnly);
        public bool Visible => !_settingInfo.Attribute.Options.HasFlag(SettingOptions.Hidden);
        public bool Sensitive => _settingInfo.Attribute.Options.HasFlag(SettingOptions.Sensitive);
        public string PropertyName => _settingInfo.Property.Name;
        public Type SettingType => _settingInfo.PropertyType;
        public string Description => _settingInfo.Attribute.Description;
        public int Priority => _settingInfo.Attribute.Priority;

        public AudioSourceSettingVM(AudioSourceSetting model, AudioSourceSettingInfo settingInfo, bool saved = true ) : base(model)
        {
            _settingInfo = settingInfo;

            // If sensitive data and was not saved from before, don't automatically remember it
            if (Sensitive && !saved)
            {
                Remember = false;
            }
        }

        /// <summary>
        /// Applies the value to the audio source
        /// </summary>
        public void ApplyChanges()
        {
            try
            {
                _settingInfo.UpdateAudioSource(Value);
            }
            catch (Exception e)
            {
                RaiseValidationError("An unexpected error occured. Check the log for more details.");
            }
        }

        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            if (ReadOnly) return;
            ApplyChanges();
        }

        public void UpdateValue()
        {
            Model.Value = _settingInfo.GetValue();
        }
    }
}
