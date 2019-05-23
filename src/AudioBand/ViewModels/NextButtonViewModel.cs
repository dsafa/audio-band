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
    /// View model for the next button.
    /// </summary>
    public class NextButtonViewModel : ButtonViewModelBase<NextButton>
    {
        private readonly IAppSettings _appSettings;
        private IAudioSource _audioSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextButtonViewModel"/> class.
        /// </summary>
        /// <param name="appSettings">The appSettings.</param>
        /// <param name="dialogService">The dialog service.</param>
        public NextButtonViewModel(IAppSettings appSettings, IDialogService dialogService)
            : base(appSettings.NextButton, dialogService)
        {
            _appSettings = appSettings;
            _appSettings.ProfileChanged += AppsSettingsOnProfileChanged;
            NextTrackCommand = new AsyncRelayCommand<object>(NextTrackCommandOnExecute);
            Content = new ButtonContentViewModel(Model.Content, new NextButton().Content, dialogService);
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
        /// Gets the next track command.
        /// </summary>
        public IAsyncCommand NextTrackCommand { get; }

        private void UpdateAudioSource(IAudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        private async Task NextTrackCommandOnExecute(object arg)
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
            ReplaceModel(_appSettings.NextButton);
        }
    }
}
