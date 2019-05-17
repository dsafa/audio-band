using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;
using NLog.Fluent;

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
            set
            {
                if (SetProperty(ref _selectedAudioSource, value, false))
                {
                    _appSettings.AudioSource = value?.Name;
                }
            }
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

            foreach (var audioSource in audioSources)
            {
                ViewModels.AudioSourceSettingsVm.CreateViewModelForAudioSource(audioSource);

                // If user was using this audio source last, then automatically activate it
                var savedAudioSourceName = _appSettings.AudioSource;
                if (savedAudioSourceName == null || audioSource.Name != savedAudioSourceName)
                {
                    continue;
                }

                SelectAudioSourceCommand.Execute(audioSource);
            }

            // Raise property changed after everything is set up so audio source settings can't be changed by the user in the middle.
            RaisePropertyChanged(nameof(AudioSources));
            Logger.Debug("Audio sources loaded. Loaded {num} sources", AudioSources.Count);
        }

        private async Task SelectAudioSourceCommandOnExecute(IAudioSource audioSource)
        {
            if (SelectedAudioSource != null)
            {
                Logger.Debug("Deactivating current audio source {audiosource}", SelectedAudioSource.Name);
                try
                {
                    await SelectedAudioSource.DeactivateAsync().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Error deactivating audio source");
                }
            }

            if (audioSource == null || audioSource == SelectedAudioSource)
            {
                SelectedAudioSource = null;
                _appSettings.Save();
                return;
            }

            UpdateViewModels(audioSource);

            Logger.Debug("Activating new audio source {audiosource}", audioSource.Name);
            try
            {
                await audioSource.ActivateAsync();
                SelectedAudioSource = audioSource;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error activating audio source");
                SelectedAudioSource = null;
            }
            finally
            {
                _appSettings.Save();
            }
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
