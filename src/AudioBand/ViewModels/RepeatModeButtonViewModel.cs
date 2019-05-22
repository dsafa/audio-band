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
    /// View model for the repeat mode button.
    /// </summary>
    public class RepeatModeButtonViewModel : ButtonViewModelBase<RepeatModeButton>
    {
        private readonly IAppSettings _appSettings;
        private IAudioSource _audioSource;
        private RepeatMode _repeatMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatModeButtonViewModel"/> class.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        public RepeatModeButtonViewModel(IAppSettings appSettings, IDialogService dialogService)
            : base(appSettings.RepeatModeButton, dialogService)
        {
            _appSettings = appSettings;
            _appSettings.ProfileChanged += AppSettingsOnProfileChanged;

            var resetState = new RepeatModeButton();
            RepeatOffContent = new ButtonContentViewModel(Model.RepeatOffContent, resetState.RepeatOffContent, dialogService);
            RepeatContextContent = new ButtonContentViewModel(Model.RepeatContextContent, resetState.RepeatContextContent, dialogService);
            RepeatTrackContent = new ButtonContentViewModel(Model.RepeatTrackContent, resetState.RepeatTrackContent, dialogService);
            CycleRepeatModeCommand = new AsyncRelayCommand<object>(CycleRepeatModeCommandOnExecute);

            TrackContentViewModel(RepeatOffContent);
            TrackContentViewModel(RepeatContextContent);
            TrackContentViewModel(RepeatTrackContent);
        }

        /// <summary>
        /// Gets the button content when repeat is off.
        /// </summary>
        public ButtonContentViewModel RepeatOffContent { get; }

        /// <summary>
        /// Gets the button content when repeat is on.
        /// </summary>
        public ButtonContentViewModel RepeatContextContent { get; }

        /// <summary>
        /// Gets the button content when repeat is on for the track.
        /// </summary>
        public ButtonContentViewModel RepeatTrackContent { get; }

        /// <summary>
        /// Gets the command to change the repeat mode.
        /// </summary>
        public ICommand CycleRepeatModeCommand { get; }

        /// <summary>
        /// Gets the current repeat mode.
        /// </summary>
        public RepeatMode RepeatMode
        {
            get => _repeatMode;
            private set => SetProperty(ref _repeatMode, value, false);
        }

        /// <summary>
        /// Sets the audio source.
        /// </summary>
        public IAudioSource AudioSource
        {
            set => UpdateAudioSource(value);
        }

        private void UpdateAudioSource(IAudioSource audioSource)
        {
            if (_audioSource != null)
            {
                _audioSource.RepeatModeChanged -= AudioSourceOnRepeatModeChanged;
            }

            _audioSource = audioSource;
            if (_audioSource == null)
            {
                RepeatMode = RepeatMode.Off;
                return;
            }

            _audioSource.RepeatModeChanged += AudioSourceOnRepeatModeChanged;
        }

        private void AudioSourceOnRepeatModeChanged(object sender, RepeatMode e)
        {
            RepeatMode = e;
        }

        private void AppSettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(_appSettings.RepeatModeButton);
        }

        private async Task CycleRepeatModeCommandOnExecute(object obj)
        {
            var nextRepeatMode = RepeatMode;
            switch (RepeatMode)
            {
                case RepeatMode.Off:
                    nextRepeatMode = RepeatMode.RepeatContext;
                    break;
                case RepeatMode.RepeatContext:
                    nextRepeatMode = RepeatMode.RepeatTrack;
                    break;
                case RepeatMode.RepeatTrack:
                    nextRepeatMode = RepeatMode.Off;
                    break;
            }

            await _audioSource.SetRepeatModeAsync(nextRepeatMode);
        }
    }
}
