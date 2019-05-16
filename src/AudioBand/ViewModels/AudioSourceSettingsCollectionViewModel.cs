using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using AudioBand.AudioSource;
using AudioBand.Models;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Represents a collection of settings for a SINGLE audio source.
    /// </summary>
    public class AudioSourceSettingsCollectionViewModel : ViewModelBase
    {
        private readonly IInternalAudioSource _audioSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceSettingsCollectionViewModel"/> class.
        /// </summary>
        /// <param name="audioSource">The audiosource that these settings belong to.</param>
        /// <param name="settingsModel">The settings model.</param>
        public AudioSourceSettingsCollectionViewModel(IInternalAudioSource audioSource, AudioSourceSettings settingsModel)
        {
            _audioSource = audioSource;
            SettingsList = new ObservableCollection<AudioSourceSettingKeyValue>(CreateKeyValuePairs(audioSource, settingsModel));
            _audioSource.SettingChanged += AudioSourceOnSettingChanged;

            foreach (var audioSourceSettingKeyValue in SettingsList)
            {
                audioSourceSettingKeyValue.PropertyChanged += AudioSourceSettingKeyValueOnPropertyChanged;
            }
        }

        /// <summary>
        /// Gets the name of the audio source associated with these settings.
        /// </summary>
        public string AudioSourceName => _audioSource.Name;

        /// <summary>
        /// Gets the list of settings for this audio source.
        /// </summary>
        public ObservableCollection<AudioSourceSettingKeyValue> SettingsList { get; }

        /// <inheritdoc/>
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            foreach (var keyPair in SettingsList)
            {
                keyPair.CancelEdit();
            }
        }

        /// <inheritdoc/>
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            foreach (var keyPair in SettingsList.OrderByDescending(s => s.Priority))
            {
                keyPair.EndEdit();
            }
        }

        private List<AudioSourceSettingKeyValue> CreateKeyValuePairs(IInternalAudioSource source, AudioSourceSettings existingSettings)
        {
            // Using a list instead of a dictionary is simple and good enough for now. There are no observable dictionaries either.
            var keyPairViewModels = new List<AudioSourceSettingKeyValue>();
            var settingAttributes = source.Settings;

            foreach (var settingAttribute in settingAttributes)
            {
                var matchingSetting = existingSettings.Settings.FirstOrDefault(s => s.Name == settingAttribute.Name);
                if (matchingSetting != null)
                {
                    keyPairViewModels.Add(new AudioSourceSettingKeyValue(source, matchingSetting, settingAttribute));
                }
                else
                {
                    var name = settingAttribute.Name;
                    var defaultValue = source[name];
                    var newSettingModel = new AudioSourceSetting { Name = name, Value = defaultValue };
                    existingSettings.Settings.Add(newSettingModel);

                    keyPairViewModels.Add(new AudioSourceSettingKeyValue(source, newSettingModel, settingAttribute));
                }
            }

            // apply changes in priority
            foreach (var keyPair in keyPairViewModels.OrderByDescending(vm => vm.Priority))
            {
                keyPair.PropagateSettingToAudioSource();
            }

            return keyPairViewModels;
        }

        // Audio sources can change settings themselves so we need to listen for them.
        private void AudioSourceOnSettingChanged(object sender, SettingChangedEventArgs e)
        {
            var settingThatChanged = SettingsList.FirstOrDefault(s => s.Name == e.SettingName);
            if (settingThatChanged == null)
            {
                Logger.Warn("Audiosource {audiosource} sent change notification of {setting} but there was no matching setting", _audioSource.Name, e.SettingName);
                return;
            }

            try
            {
                settingThatChanged.Value = _audioSource[e.SettingName];
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error while updating setting {setting} for {audiosource}", e.SettingName, _audioSource.Name);
            }
        }

        // Listen so that we can mark this view model as dirty if a setting changes.
        private void AudioSourceSettingKeyValueOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(AudioSourceSettingKeyValue.Value))
            {
                return;
            }

            if (!IsEditing)
            {
                BeginEdit();
            }
        }
    }
}
