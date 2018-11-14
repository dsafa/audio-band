using AudioBand.AudioSource;
using AudioBand.Extensions;
using AudioBand.Models;
using System.Collections.Generic;
using System.Linq;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for all audio source settings
    /// </summary>
    internal class AudioSourceSettingsVM : ViewModelBase<AudioSourceSettings>
    {
        public string AudioSourceName => Model.AudioSourceName;
        public List<AudioSourceSettingVM> Settings { get; }

        public AudioSourceSettingsVM(AudioSourceSettings settings, IAudioSource audioSource) : base(settings)
        {
            Settings = CreateSettingViewModels(Model, audioSource);
            foreach (var audioSourceSettingVm in Settings)
            {
                audioSourceSettingVm.UpdateAudioSource();
            }
        }

        private List<AudioSourceSettingVM> CreateSettingViewModels(AudioSourceSettings existingSettings, IAudioSource source)
        {
            var viewmodels = new List<AudioSourceSettingVM>();
            var audioSourceSettingInfos = source.GetSettings();

            foreach (var audioSourceSettingInfo in audioSourceSettingInfos)
            {
                var matchingSetting = existingSettings.Settings.FirstOrDefault(s => s.Name == audioSourceSettingInfo.Attribute.Name);
                if (matchingSetting != null)
                {
                    viewmodels.Add(new AudioSourceSettingVM(matchingSetting, audioSourceSettingInfo));
                }
                else
                {
                    var name = audioSourceSettingInfo.Attribute.Name;
                    var defaultValue = audioSourceSettingInfo.Property.GetValue(source);
                    var newSetting = new AudioSourceSetting {Name = name, Value = defaultValue};
                    Model.Settings.Add(newSetting);
                    viewmodels.Add(new AudioSourceSettingVM(newSetting, audioSourceSettingInfo));
                }
            }

            return viewmodels;
        }

        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            foreach (var audioSourceSettingVm in Settings)
            {
                audioSourceSettingVm.CancelEdit();
            }
        }

        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            foreach (var audioSourceSettingVm in Settings)
            {
                audioSourceSettingVm.EndEdit();
            }
        }

        protected override void OnBeginEdit()
        {
            base.OnBeginEdit();
            foreach (var audioSourceSettingVm in Settings)
            {
                audioSourceSettingVm.BeginEdit();
            }
        }
    }
}
