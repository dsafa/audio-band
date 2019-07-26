using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.UI
{
    /// <summary>
    /// View model for the play/pause button.
    /// </summary>
    public class PlayPauseButtonViewModel : ButtonViewModelBase<PlayPauseButton>
    {
        private readonly IAppSettings _appSettings;
        private readonly IAudioSession _audioSession;
        private bool _isPlaying;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayPauseButtonViewModel"/> class.
        /// </summary>
        /// <param name="appSettings">App settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="audioSession">The audio session.</param>
        /// <param name="messageBus">The message bus.</param>
        public PlayPauseButtonViewModel(IAppSettings appSettings, IDialogService dialogService, IAudioSession audioSession, IMessageBus messageBus)
            : base(appSettings.CurrentProfile.PlayPauseButton, dialogService, messageBus)
        {
            _appSettings = appSettings;
            _audioSession = audioSession;
            _audioSession.PropertyChanged += AudioSessionOnPropertyChanged;
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
            private set => SetProperty(ref _isPlaying, value);
        }

        /// <summary>
        /// Gets a value indicating whether the button is playing.
        /// </summary>
        public bool IsPlayButtonShown => !_isPlaying;

        /// <inheritdoc />
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            MapSelf(Model, _appSettings.CurrentProfile.PlayPauseButton);
        }

        private void AppSettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            MapSelf(_appSettings.CurrentProfile.PlayPauseButton, Model);
            RaisePropertyChangedAll();
        }

        private void AudioSessionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IAudioSession.IsPlaying))
            {
                return;
            }

            OnIsPlayingChanged(_audioSession.IsPlaying);
        }

        private void OnIsPlayingChanged(bool isPlaying)
        {
            IsPlaying = isPlaying;
        }

        private async Task PlayPauseTrackCommandOnExecute(object arg)
        {
            if (_audioSession.CurrentAudioSource == null)
            {
                return;
            }

            if (IsPlaying)
            {
                await _audioSession.CurrentAudioSource.PauseTrackAsync();
            }
            else
            {
                await _audioSession.CurrentAudioSource.PlayTrackAsync();
            }
        }
    }
}
