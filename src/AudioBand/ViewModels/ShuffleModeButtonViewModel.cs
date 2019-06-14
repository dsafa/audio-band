using System;
using System.ComponentModel;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Messages;
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
        private readonly IAudioSession _audioSession;
        private bool _isShuffleOn;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShuffleModeButtonViewModel"/> class.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="audioSession">The audio session.</param>
        /// <param name="messageBus">The message bus.</param>
        public ShuffleModeButtonViewModel(IAppSettings appSettings, IDialogService dialogService, IAudioSession audioSession, IMessageBus messageBus)
            : base(appSettings.ShuffleModeButton, dialogService, messageBus)
        {
            _appSettings = appSettings;
            _audioSession = audioSession;
            _audioSession.PropertyChanged += AudioSessionOnPropertyChanged;
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
            private set => SetProperty(ref _isShuffleOn, value);
        }

        /// <inheritdoc />
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            MapSelf(Model, _appSettings.ShuffleModeButton);
        }

        private void AudioSessionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IAudioSession.IsShuffleOn))
            {
                return;
            }

            OnSufflechanged(_audioSession.IsShuffleOn);
        }

        private void OnSufflechanged(bool e)
        {
            IsShuffleOn = e;
        }

        private void AppSettingsOnProfileChanged(object sender, EventArgs e)
        {
            MapSelf(_appSettings.ShuffleModeButton, Model);
            RaisePropertyChangedAll();
        }

        private async Task ToggleShuffleCommandOnExecute(object arg)
        {
            if (_audioSession.CurrentAudioSource == null)
            {
                return;
            }

            await _audioSession.CurrentAudioSource.SetShuffleAsync(!IsShuffleOn);
        }
    }
}
