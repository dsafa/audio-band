using System;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the shuffle button.
    /// </summary>
    public class ShuffleModeButtonViewModel : ButtonViewModelBase<ShuffleModeButton>
    {
        private readonly IAppSettings _appSettings;
        private IAudioSource _audioSource;
        private bool _isShuffleOn;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShuffleModeButtonViewModel"/> class.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        public ShuffleModeButtonViewModel(IAppSettings appSettings, IDialogService dialogService)
            : base(appSettings.ShuffleModeButton, dialogService)
        {
            _appSettings = appSettings;
            _appSettings.ProfileChanged += AppSettingsOnProfileChanged;

            ToggleShuffleCommand = new AsyncRelayCommand<object>(ToggleShuffleCommandOnExecute);
            var resetBase = new ShuffleModeButton();
            ShuffleOnContent = new ButtonContentViewModel(Model.ShuffleOnContent, resetBase.ShuffleOnContent, dialogService);
            ShuffleOffContent = new ButtonContentViewModel(Model.ShuffleOffContent, resetBase.ShuffleOffContent, dialogService);
            TrackContentViewModel(ShuffleOnContent);
            TrackContentViewModel(ShuffleOffContent);
        }

        /// <summary>
        /// Gets the view model for the button content when shuffle is on.
        /// </summary>
        public ButtonContentViewModel ShuffleOnContent { get; }

        /// <summary>
        /// Gets the view model for the button content when shuffle is off.
        /// </summary>
        public ButtonContentViewModel ShuffleOffContent { get; }

        /// <summary>
        /// Gets the command to toggle the shuffle mode.
        /// </summary>
        public IAsyncCommand ToggleShuffleCommand { get; }

        /// <summary>
        /// Gets a value indicating whether shuffle is on.
        /// </summary>
        public bool IsShuffleOn
        {
            get => _isShuffleOn;
            private set => SetProperty(ref _isShuffleOn, value, false);
        }

        /// <summary>
        /// Sets the audio source.
        /// </summary>
        public IAudioSource AudioSource
        {
            private get => _audioSource;
            set => UpdateAudioSource(value);
        }

        private void UpdateAudioSource(IAudioSource audioSource)
        {
            if (_audioSource != null)
            {
                _audioSource.ShuffleChanged -= AudioSourceOnShuffleChanged;
            }

            _audioSource = audioSource;
            if (_audioSource == null)
            {
                IsShuffleOn = false;
                return;
            }

            _audioSource.ShuffleChanged += AudioSourceOnShuffleChanged;
        }

        private void AudioSourceOnShuffleChanged(object sender, bool e)
        {
            IsShuffleOn = e;
        }

        private void AppSettingsOnProfileChanged(object sender, EventArgs e)
        {
            ReplaceModel(_appSettings.ShuffleModeButton);
        }

        private async Task ToggleShuffleCommandOnExecute(object arg)
        {
            if (AudioSource == null)
            {
                return;
            }

            await AudioSource.SetShuffleAsync(!IsShuffleOn);
        }
    }
}
