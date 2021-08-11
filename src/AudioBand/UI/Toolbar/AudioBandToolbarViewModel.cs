using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Logging;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;
using NLog;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq;

namespace AudioBand.UI
{
    /// <summary>
    /// View model for audio band toolbar.
    /// </summary>
    public class AudioBandToolbarViewModel : ObservableObject
    {
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<AudioBandToolbarViewModel>();
        private readonly IAppSettings _appSettings;
        private readonly IAudioSourceManager _audioSourceManager;
        private readonly IMessageBus _messageBus;
        private readonly IAudioSession _audioSession;
        private IAudioSource _selectedAudioSource;
        private UserProfile _selectedUserProfile;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioBandToolbarViewModel"/> class.
        /// </summary>
        /// <param name="viewModels">The view models.</param>
        /// <param name="appSettings">The app settings.</param>
        /// <param name="audioSourceManager">The audio source mananger.</param>
        /// <param name="messageBus">The message bus.</param>
        /// <param name="audioSession">The audio session.</param>
        public AudioBandToolbarViewModel(IViewModelContainer viewModels, IAppSettings appSettings, IAudioSourceManager audioSourceManager, IMessageBus messageBus, IAudioSession audioSession)
        {
            _appSettings = appSettings;
            _audioSourceManager = audioSourceManager;
            _messageBus = messageBus;
            _audioSession = audioSession;
            ViewModels = viewModels;

            ShowSettingsWindowCommand = new RelayCommand(ShowSettingsWindowCommandOnExecute);
            LoadCommand = new AsyncRelayCommand<object>(LoadCommandOnExecute);
            DoubleClickCommand = new AsyncRelayCommand<RoutedEventArgs>(OnDoubleClick);
            SelectAudioSourceCommand = new AsyncRelayCommand<IInternalAudioSource>(SelectAudioSourceCommandOnExecute);
            SelectProfileCommand = new AsyncRelayCommand<UserProfile>(SelectProfileCommandOnExecute);
        }

        /// <summary>
        /// A list of all the ViewModels.
        /// </summary>
        public IViewModelContainer ViewModels { get; }

        /// <summary>
        /// A collection of all available AudioSources.
        /// </summary>
        public ObservableCollection<IInternalAudioSource> AudioSources { get; private set; }

        /// <summary>
        /// A collection of all possible UserProfiles.
        /// </summary>
        public ObservableCollection<UserProfile> Profiles { get; private set; }

        /// <summary>
        /// Gets the command to show the settings window.
        /// </summary>
        public ICommand ShowSettingsWindowCommand { get; }

        /// <summary>
        /// Gets the command to initialize loading.
        /// </summary>
        public ICommand LoadCommand { get; }

        /// <summary>
        /// Gets the command to handle double clicks.
        /// </summary>
        public ICommand DoubleClickCommand { get; }

        /// <summary>
        /// Gets the command to select an audio source.
        /// </summary>
        public ICommand SelectAudioSourceCommand { get; }

        /// <summary>
        /// Gets the command to select a profile.
        /// </summary>
        public ICommand SelectProfileCommand { get; }

        /// <summary>
        /// Gets or sets the selected audio source.
        /// </summary>
        public IAudioSource SelectedAudioSource
        {
            get => _selectedAudioSource;
            set
            {
                if (SetProperty(ref _selectedAudioSource, value))
                {
                    _appSettings.AudioSource = value?.Name;
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected profile.
        /// </summary>
        public UserProfile SelectedProfile
        {
            get => _selectedUserProfile;
            set
            {
                if (SetProperty(ref _selectedUserProfile, value))
                {
                    _appSettings.AudioSource = value?.Name;
                }
            }
        }

        private void ShowSettingsWindowCommandOnExecute()
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
                ViewModels.AudioSourceSettingsViewModel.CreateViewModelForAudioSource(audioSource);

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

            // Initalize Profiles
            Profiles = new ObservableCollection<UserProfile>(_appSettings.Profiles);
            SelectedProfile = _appSettings.CurrentProfile;
            RaisePropertyChanged(nameof(Profiles));
            Logger.Debug($"Profiles loaded. Loaded {Profiles.Count} profiles.");
        }

        private async Task OnDoubleClick(RoutedEventArgs e)
        {
            if (SelectedAudioSource == null)
            {
                return;
            }

            var windowPtr = NativeMethods.FindWindow(SelectedAudioSource.WindowClassName, null);

            // Spotify has some weird shenanigans with their windows, doign it like normal
            // results in the wrong window handle being returned.
            if (SelectedAudioSource.Name == "Spotify")
            {
                var spotifyProcesses = Process.GetProcessesByName("spotify");
                var title = spotifyProcesses.FirstOrDefault(x => !string.IsNullOrEmpty(x.MainWindowTitle))?.MainWindowTitle;
                windowPtr = NativeMethods.FindWindow(null, title);
            }

            if (windowPtr == IntPtr.Zero)
            {
                Logger.Warn("Could not find the associated window to open with double click.");
            }
            else
            {
                if (NativeMethods.IsIconic(windowPtr))
                {
                    NativeMethods.ShowWindow(windowPtr, 9);
                }

                NativeMethods.SetForegroundWindow(windowPtr);
            }
        }

        private async Task SelectAudioSourceCommandOnExecute(IInternalAudioSource audioSource)
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
                if (_appSettings.AudioBandSettings.UseAutomaticIdleProfile)
                {
                    _appSettings.AudioBandSettings.LastNonIdleProfile = SelectedProfile;
                    SelectProfileCommand.Execute(UserProfile.IdleProfileName);
                }

                SelectedAudioSource = null;
                _appSettings.Save();
                return;
            }

            _audioSession.CurrentAudioSource = audioSource;

            Logger.Debug("Activating new audio source {audiosource}", audioSource.Name);
            try
            {
                if (_appSettings.AudioBandSettings.UseAutomaticIdleProfile)
                {
                    SelectProfileCommand.Execute(_appSettings.AudioBandSettings.LastNonIdleProfile);
                }

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

        private async Task SelectProfileCommandOnExecute(UserProfile profile)
        {
            _appSettings.SelectProfile(profile.Name);
            SelectedProfile = _appSettings.CurrentProfile;
        }
    }
}
