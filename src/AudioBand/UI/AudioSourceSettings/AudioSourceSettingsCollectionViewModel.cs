using AudioBand.AudioSource;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace AudioBand.UI
{
    /// <summary>
    /// Represents a collection of settings for a SINGLE audio source.
    /// </summary>
    public class AudioSourceSettingsCollectionViewModel : ViewModelBase
    {
        private readonly IInternalAudioSource _audioSource;
        private readonly IMessageBus _messageBus;
        private readonly IAppSettings _appSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceSettingsCollectionViewModel"/> class.
        /// </summary>
        /// <param name="audioSource">The audiosource that these settings belong to.</param>
        /// <param name="settingsModel">The settings model.</param>
        /// <param name="messageBus">The message bus.</param>
        /// <param name="appSettings">The app settings.</param>
        public AudioSourceSettingsCollectionViewModel(IInternalAudioSource audioSource, AudioSourceSettings settingsModel, IMessageBus messageBus, IAppSettings appSettings)
        {
            _audioSource = audioSource;
            _messageBus = messageBus;
            _appSettings = appSettings;
            SettingsList = new ObservableCollection<AudioSourceSettingKeyValue>(CreateKeyValuePairs(audioSource, settingsModel));
            _audioSource.SettingChanged += AudioSourceOnSettingChanged;

            foreach (var audioSourceSettingKeyValue in SettingsList)
            {
                audioSourceSettingKeyValue.PropertyChanged += AudioSourceSettingKeyValueOnPropertyChanged;
            }

            UseMessageBus(messageBus);
        }

        /// <summary>
        /// Gets the name of the audio source associated with these settings.
        /// </summary>
        public string AudioSourceName => _audioSource.Name;

        /// <summary>
        /// Gets the list of settings for this audio source.
        /// </summary>
        public ObservableCollection<AudioSourceSettingKeyValue> SettingsList { get; }

        /// <summary>
        /// Gets the description/extra info of this audio source.
        /// </summary>
        public string Description => _audioSource.Description;

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
        // Usually, settings are saved after the user edits a setting and clicks apply.
        // These changes occur outside of the normal settings lifecycle, so the only time that the new values can be saved
        // are when the application closes but there are some issues with detected that, so instead just save now.
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
                settingThatChanged.SyncToModel();
                _appSettings.Save();
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
