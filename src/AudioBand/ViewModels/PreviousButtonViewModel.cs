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
    /// View model for the previous button.
    /// </summary>
    public class PreviousButtonViewModel : ButtonViewModelBase<PreviousButton>
    {
        private readonly IAppSettings _appSettings;
        private IAudioSource _audioSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviousButtonViewModel"/> class.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        public PreviousButtonViewModel(IAppSettings appSettings, IDialogService dialogService)
            : base(appSettings.PreviousButton, dialogService)
        {
            _appSettings = appSettings;
            _appSettings.ProfileChanged += AppsSettingsOnProfileChanged;
            PreviousTrackCommand = new AsyncRelayCommand<object>(PreviousTrackCommandOnExecute);
            Content = new ButtonContentViewModel(Model.Content, new PreviousButton().Content, dialogService);
            TrackContentViewModel(Content);
        }

        /// <summary>
        /// Gets the button content.
        /// </summary>
        public ButtonContentViewModel Content { get; }

        /// <summary>
        /// Sets the audio source.
        /// </summary>
        public IAudioSource AudioSource
        {
            set => UpdateAudioSource(value);
        }

        /// <summary>
        /// Gets the previous track command.
        /// </summary>
        public IAsyncCommand PreviousTrackCommand { get; }

        private void UpdateAudioSource(IAudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        private async Task PreviousTrackCommandOnExecute(object arg)
        {
            if (_audioSource == null)
            {
                return;
            }

            await _audioSource.NextTrackAsync();
        }

        private void AppsSettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(_appSettings.PreviousButton);
        }
    }
}
