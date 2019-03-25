using System.Collections.Generic;
using System.Linq;
using AudioBand.AudioSource;
using AudioBand.Models;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for all audio source settings
    /// </summary>
    public class AudioSourceSettingsVM : ViewModelBase<AudioSourceSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceSettingsVM"/> class
        /// with the settings and the audio source.
        /// </summary>
        /// <param name="settings">Settings for the audio source.</param>
        /// <param name="audioSource">The audio source.</param>
        public AudioSourceSettingsVM(AudioSourceSettings settings, IInternalAudioSource audioSource)
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
            Settings.FirstOrDefault(s => s.Name == e.SettingName)?.ValueChanged();
        }

        private List<AudioSourceSettingVM> CreateSettingViewModels(AudioSourceSettings existingSettings, IInternalAudioSource source)
        {
            var viewmodels = new List<AudioSourceSettingVM>();
            var settingAttributes = source.Settings;

            foreach (var settingAttribute in settingAttributes)
            {
                var matchingSetting = existingSettings.Settings.FirstOrDefault(s => s.Name == settingAttribute.Name);
                if (matchingSetting != null)
                {
                    viewmodels.Add(new AudioSourceSettingVM(source, matchingSetting, settingAttribute));
                }
                else
                {
                    var name = settingAttribute.Name;
                    var defaultValue = source[name];
                    var newSetting = new AudioSourceSetting { Name = name, Value = defaultValue };
                    Model.Settings.Add(newSetting);
                    viewmodels.Add(new AudioSourceSettingVM(source, newSetting, settingAttribute, false));
                }
            }

            // apply changes in priority
            foreach (var viewModel in viewmodels.OrderByDescending(vm => vm.Priority))
            {
                viewModel.ApplyChanges();
            }

            return viewmodels;
        }
    }
}
