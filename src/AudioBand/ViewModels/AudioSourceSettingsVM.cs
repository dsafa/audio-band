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
    internal class AudioSourceSettingsVM : ViewModelBase
    {
        private readonly AudioSourceSettings _settings;
        private readonly IAudioSource _audioSource;

        public string AudioSourceName => _audioSource.Name;
        public List<AudioSourceSettingVM> Settings { get; }

        public AudioSourceSettingsVM(AudioSourceSettings settings, IAudioSource audioSource)
        {
            _settings = settings;
            _audioSource = audioSource;
            Settings = CreateSettingViewModels(_settings, _audioSource);
        }

        private List<AudioSourceSettingVM> CreateSettingViewModels(AudioSourceSettings audioSourceSettings, IAudioSource source)
        {
            var viewmodels = new List<AudioSourceSettingVM>();
            var audioSourceSettingInfos = source.GetSettings();
            foreach (var audioSourceSettingInfo in audioSourceSettingInfos)
            {
                var matchingSetting = audioSourceSettings.Settings.FirstOrDefault(s => s.Name == audioSourceSettingInfo.Attribute.Name);
                if (matchingSetting != null)
                {
                    viewmodels.Add(new AudioSourceSettingVM(matchingSetting, audioSourceSettingInfo));
                }
                else
                {
                    var name = audioSourceSettingInfo.Attribute.Name;
                    var defaultValue = audioSourceSettingInfo.Property.GetMethod.Invoke(source, null);
                    viewmodels.Add(new AudioSourceSettingVM(new AudioSourceSetting { Name = name, Value = defaultValue }, audioSourceSettingInfo));
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
