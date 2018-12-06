using System.Collections.Generic;
using System.Linq;
using AudioBand.AudioSource;
using AudioBand.Extensions;
using AudioBand.Models;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for all audio source settings
    /// </summary>
    internal class AudioSourceSettingsVM : ViewModelBase<AudioSourceSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceSettingsVM"/> class
        /// with the settings and the audio source.
        /// </summary>
        /// <param name="settings">Settings for the audio source.</param>
        /// <param name="audioSource">The audio source.</param>
        public AudioSourceSettingsVM(AudioSourceSettings settings, IAudioSource audioSource)
            : base(settings)
        {
            Settings = CreateSettingViewModels(Model, audioSource);
            audioSource.SettingChanged += AudioSourceOnSettingChanged;
        }

        /// <summary>
        /// Gets the name of the audio source.
        /// </summary>
        public string AudioSourceName => Model.AudioSourceName;

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="AudioSourceSettingVM"/> belonging to this audio source.
        /// </summary>
        public List<AudioSourceSettingVM> Settings { get; }

        /// <inheritdoc/>
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            foreach (var audioSourceSettingVm in Settings)
            {
                audioSourceSettingVm.CancelEdit();
            }
        }

        /// <inheritdoc/>
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            foreach (var audioSourceSettingVm in Settings.OrderByDescending(s => s.Priority))
            {
                audioSourceSettingVm.EndEdit();
            }
        }

        /// <inheritdoc/>
        protected override void OnBeginEdit()
        {
            base.OnBeginEdit();
            foreach (var audioSourceSettingVm in Settings)
            {
                audioSourceSettingVm.BeginEdit();
            }
        }

        private void AudioSourceOnSettingChanged(object sender, SettingChangedEventArgs e)
        {
            Settings.FirstOrDefault(s => s.PropertyName == e.PropertyName)?.ValueChanged();
        }

        private List<AudioSourceSettingVM> CreateSettingViewModels(AudioSourceSettings existingSettings, IAudioSource source)
        {
            var viewmodels = new List<AudioSourceSettingVM>();
            var audioSourceSettingInfos = source.GetSettings();

            foreach (var audioSourceSettingInfo in audioSourceSettingInfos.OrderByDescending(s => s.Attribute.Priority))
            {
                var matchingSetting = existingSettings.Settings.FirstOrDefault(s => s.Name == audioSourceSettingInfo.Attribute.Name);
                if (matchingSetting != null)
                {
                    viewmodels.Add(new AudioSourceSettingVM(matchingSetting, audioSourceSettingInfo));
                }
                else
                {
                    var name = audioSourceSettingInfo.Attribute.Name;
                    var defaultValue = audioSourceSettingInfo.GetValue();
                    var newSetting = new AudioSourceSetting { Name = name, Value = defaultValue };
                    Model.Settings.Add(newSetting);
                    viewmodels.Add(new AudioSourceSettingVM(newSetting, audioSourceSettingInfo, false));
                }
            }

            return viewmodels;
        }
    }
}
