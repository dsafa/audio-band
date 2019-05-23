using System.Collections.ObjectModel;
using System.Linq;
using AudioBand.AudioSource;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for ALL audio source settings.
    /// </summary>
    public class AudioSourceSettingsViewModel : ViewModelBase
    {
        private readonly IAppSettings _appSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceSettingsViewModel"/> class
        /// with the settings.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        public AudioSourceSettingsViewModel(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        /// <summary>
        /// Gets all the audio sources and their settings.
        /// </summary>
        public ObservableCollection<AudioSourceSettingsCollectionViewModel> AudioSourcesSettings { get; } = new ObservableCollection<AudioSourceSettingsCollectionViewModel>();

        /// <summary>
        /// Creates a settings view model for the new audio source.
        /// </summary>
        /// <param name="audioSource">The audio source.</param>
        public void CreateViewModelForAudioSource(IInternalAudioSource audioSource)
        {
            var settings = audioSource.Settings;
            if (settings.Count == 0)
            {
                return;
            }

            // check if the settings were saved previously and reuse them
            var matchingSetting = _appSettings.AudioSourceSettings.FirstOrDefault(s => s.AudioSourceName == audioSource.Name);
            if (matchingSetting != null)
            {
                var viewModel = new AudioSourceSettingsCollectionViewModel(audioSource, matchingSetting);
                AudioSourcesSettings.Add(viewModel);
            }
            else
            {
                var newSettingsModel = new AudioSourceSettings { AudioSourceName = audioSource.Name };
                _appSettings.AudioSourceSettings.Add(newSettingsModel);
                var newViewModel = new AudioSourceSettingsCollectionViewModel(audioSource, newSettingsModel);
                AudioSourcesSettings.Add(newViewModel);
            }

            _appSettings.Save();
        }
    }
}
