using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for audio band toolbar.
    /// </summary>
    public class AudioBandToolbarViewModel : ViewModelBase
    {
        private readonly IAppSettings _appSettings;
        private readonly IAudioSourceManager _audioSourceManager;
        private readonly IMessageBus _messageBus;
        private IAudioSource _selectedAudioSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioBandToolbarViewModel"/> class.
        /// </summary>
        /// <param name="viewModels">The view models.</param>
        /// <param name="appSettings">The app settings.</param>
        /// <param name="audioSourceManager">The audio source mananger.</param>
        /// <param name="messageBus">The message bus.</param>
        public AudioBandToolbarViewModel(IViewModelContainer viewModels, IAppSettings appSettings, IAudioSourceManager audioSourceManager, IMessageBus messageBus)
        {
            _appSettings = appSettings;
            _audioSourceManager = audioSourceManager;
            _messageBus = messageBus;
            ViewModels = viewModels;

            ShowSettingsWindowCommand = new RelayCommand(ShowSettingsWindowCommandOnExecute);
            LoadCommand = new AsyncRelayCommand<object>(LoadCommandOnExecute);
            SelectAudioSourceCommand = new AsyncRelayCommand<IAudioSource>(SelectAudioSourceCommandOnExecute);
        }

        /// <summary>
        /// Gets the view models.
        /// </summary>
        public IViewModelContainer ViewModels { get; }

        /// <summary>
        /// Gets the audio sources.
        /// </summary>
        public ObservableCollection<IInternalAudioSource> AudioSources { get; private set; }

        /// <summary>
        /// Gets the command to show the settings window.
        /// </summary>
        public ICommand ShowSettingsWindowCommand { get; }

        /// <summary>
        /// Gets the command to initialize loading.
        /// </summary>
        public ICommand LoadCommand { get; }

        /// <summary>
        /// Gets the command to select an audio source.
        /// </summary>
        public ICommand SelectAudioSourceCommand { get; }

        /// <summary>
        /// Gets or sets the selected audio source.
        /// </summary>
        public IAudioSource SelectedAudioSource
        {
            get => _selectedAudioSource;
            set => SetProperty(ref _selectedAudioSource, value, false);
        }

        private void AddNewAudioSource(IInternalAudioSource audioSource)
        {
            var settings = audioSource.Settings;
            if (settings.Count > 0)
            {
                // check if the settings were saved previously and reuse them
                var matchingSetting = _appSettings.AudioSourceSettings.FirstOrDefault(s => s.AudioSourceName == audioSource.Name);
                if (matchingSetting != null)
                {
                    var viewModel = new AudioSourceSettingsVM(matchingSetting, audioSource);
                    ViewModels.AudioSourceSettingsVM.Add(viewModel);
                }
                else
                {
                    var newSettingsModel = new AudioSourceSettings { AudioSourceName = audioSource.Name };
                    _appSettings.AudioSourceSettings.Add(newSettingsModel);
                    var newViewModel = new AudioSourceSettingsVM(newSettingsModel, audioSource);
                    ViewModels.AudioSourceSettingsVM.Add(newViewModel);
                }
            }

            // If user was using this audio source last, then automatically activate it
            var savedAudioSourceName = _appSettings.AudioSource;
            if (savedAudioSourceName == null || audioSource.Name != savedAudioSourceName)
            {
                return;
            }

            SelectAudioSourceCommand.Execute(audioSource);
        }

        private void ShowSettingsWindowCommandOnExecute(object obj)
        {
            _messageBus.Publish(SettingsWindowMessage.OpenWindow);
        }

        private async Task LoadCommandOnExecute(object arg)
        {
            Logger.Debug("Loading audio sources");

            var audioSources = await _audioSourceManager.LoadAudioSourcesAsync();
            AudioSources = new ObservableCollection<IInternalAudioSource>(audioSources);
            foreach (var source in audioSources)
            {
                AddNewAudioSource(source);
            }

            RaisePropertyChanged(nameof(AudioSources));

            Logger.Debug("Audio sources loaded. Loaded {num} sources", AudioSources.Count);
        }

        private async Task SelectAudioSourceCommandOnExecute(IAudioSource audioSource)
        {
            if (SelectedAudioSource != null)
            {
                Logger.Debug("Deactivating current audio source {audiosource}", SelectedAudioSource.Name);
                await SelectedAudioSource.DeactivateAsync();
            }

            if (audioSource == null || audioSource == SelectedAudioSource)
            {
                SelectedAudioSource = null;
                return;
            }

            UpdateViewModels(audioSource);

            Logger.Debug("Activating new audio source {audiosource}", audioSource.Name);
            await audioSource.ActivateAsync();
            SelectedAudioSource = audioSource;

            _appSettings.Save();
        }

        private void UpdateViewModels(IAudioSource audioSource)
        {
            ViewModels.AlbumArtVM.AudioSource = audioSource;
            ViewModels.NextButtonVM.AudioSource = audioSource;
            ViewModels.PreviousButtonVM.AudioSource = audioSource;
            ViewModels.PlayPauseButtonVM.AudioSource = audioSource;
            ViewModels.ProgressBarVM.AudioSource = audioSource;
            foreach (var customLabelVm in ViewModels.CustomLabelsVM.CustomLabels)
            {
                customLabelVm.AudioSource = audioSource;
            }
        }
    }
}
