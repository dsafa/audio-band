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

        public bool ReadOnly => _settingInfo.Attribute.Options.HasFlag(SettingOptions.ReadOnly);
        public bool Visible => !_settingInfo.Attribute.Options.HasFlag(SettingOptions.Hidden);
        public bool Sensitive => _settingInfo.Attribute.Options.HasFlag(SettingOptions.Sensitive);
        public string PropertyName => _settingInfo.Property.Name;
        public Type SettingType => _settingInfo.PropertyType;
        public string Description => null;

        public AudioSourceSettingVM(AudioSourceSetting model, AudioSourceSettingInfo settingInfo) : base(model)
        {
            _settingInfo = settingInfo;
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
