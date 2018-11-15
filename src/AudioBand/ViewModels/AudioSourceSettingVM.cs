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

        public AudioSourceSettingVM(AudioSourceSetting model, AudioSourceSettingInfo settingInfo) : base(model)
        {
            _settingInfo = settingInfo;
        }

        public void UpdateAudioSource()
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
            UpdateAudioSource();
        }
    }
}
