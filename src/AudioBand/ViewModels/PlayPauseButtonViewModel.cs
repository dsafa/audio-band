using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the play/pause button.
    /// </summary>
    public class PlayPauseButtonViewModel : ButtonViewModelBase<PlayPauseButton>
    {
        private readonly IAppSettings _appSettings;
        private bool _isPlaying;
        private IAudioSource _audioSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayPauseButtonViewModel"/> class.
        /// </summary>
        /// <param name="appSettings">App settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        public PlayPauseButtonViewModel(IAppSettings appSettings, IDialogService dialogService)
            : base(appSettings.PlayPauseButton, dialogService)
        {
            _appSettings = appSettings;
            _appSettings.ProfileChanged += AppSettingsOnProfileChanged;
            PlayPauseTrackCommand = new AsyncRelayCommand<object>(PlayPauseTrackCommandOnExecute);

            var resetBase = new PlayPauseButton();
            PlayContent = new ButtonContentViewModel(Model.PlayContent, resetBase.PlayContent, dialogService);
            PauseContent = new ButtonContentViewModel(Model.PauseContent, resetBase.PauseContent, dialogService);
            TrackContentViewModel(PlayContent);
            TrackContentViewModel(PauseContent);
        }

        /// <summary>
        /// Gets the view model for the button in the play state.
        /// </summary>
        public ButtonContentViewModel PlayContent { get; }

        /// <summary>
        /// Gets the view model for the button in the pause state.
        /// </summary>
        public ButtonContentViewModel PauseContent { get; }

        /// <summary>
        /// Sets the audio source.
        /// </summary>
        public IAudioSource AudioSource
        {
            set => UpdateAudioSource(value);
        }

        /// <summary>
        /// Gets the play pause command.
        /// </summary>
        public IAsyncCommand PlayPauseTrackCommand { get; }

        /// <summary>
        /// Gets a value indicating whether a track is playing.
        /// </summary>
        [AlsoNotify(nameof(IsPlayButtonShown))]
        public bool IsPlaying
        {
            get => _isPlaying;
            private set => SetProperty(ref _isPlaying, value, false);
        }

        /// <summary>
        /// Gets a value indicating whether the button is playing.
        /// </summary>
        public bool IsPlayButtonShown => !_isPlaying;

        private void AppSettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(_appSettings.PlayPauseButton);
        }

        private void UpdateAudioSource(IAudioSource audioSource)
        {
            if (_audioSource != null)
            {
                _audioSource.IsPlayingChanged -= AudioSourceOnIsPlayingChanged;
            }

            _audioSource = audioSource;
            if (_audioSource == null)
            {
                IsPlaying = false;
                return;
            }

            _audioSource.IsPlayingChanged += AudioSourceOnIsPlayingChanged;
        }

        private void AudioSourceOnIsPlayingChanged(object sender, bool e)
        {
            IsPlaying = e;
        }

        private async Task PlayPauseTrackCommandOnExecute(object arg)
        {
            if (_audioSource == null)
            {
                return;
            }

            if (IsPlaying)
            {
                await _audioSource.PauseTrackAsync();
            }
            else
            {
                await _audioSource.PlayTrackAsync();
            }
        }
    }
}
