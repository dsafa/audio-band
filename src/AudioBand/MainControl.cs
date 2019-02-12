using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using System.Windows.Threading;
using AudioBand.AudioSource;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.ViewModels;
using CSDeskBand;
using CSDeskBand.ContextMenu;
using CSDeskBand.Win;
using NLog;
using NLog.Config;
using SettingsWindow = AudioBand.Views.Wpf.SettingsWindow;
using Size = System.Drawing.Size;

namespace AudioBand
{
    [Guid("957D8782-5B07-4126-9B24-1E917BAAAD64")]
    [ComVisible(true)]
    [CSDeskBandRegistration(Name = "Audio Band", ShowDeskBand = true)]
    public partial class MainControl : CSDeskBandWin
    {
        private static readonly ILogger Logger = LogManager.GetLogger("Audio Band");
        private readonly AppSettings _appSettings = new AppSettings();
        private readonly Dispatcher _uiDispatcher;
        private AudioSourceManager _audioSourceManager;
        private SettingsWindow _settingsWindow;
        private IAudioSource _currentAudioSource;
        private DeskBandMenuAction _settingsMenuItem;
        private DeskBandMenu _pluginSubMenu;
        private List<DeskBandMenuAction> _audioSourceContextMenuItems;
        private SettingsWindowVM _settingsWindowVm;

        #region Models

        private AlbumArt _albumArtModel;
        private AlbumArtPopup _albumArtPopupModel;
        private Models.AudioBand _audioBandModel;
        private List<AudioSourceSettings> _audioSourceSettingsModel;
        private List<CustomLabel> _customLabelsModel;
        private NextButton _nextButtonModel;
        private PlayPauseButton _playPauseButtonModel;
        private PreviousButton _previousButtonModel;
        private ProgressBar _progressBarModel;
        private Track _trackModel;

        #endregion

        static MainControl()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => LogManager.GetCurrentClassLogger().Error((Exception)args.ExceptionObject, "Unhandled Exception");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainControl"/> class.
        /// Entry point.
        /// </summary>
        public MainControl()
        {
            InitializeComponent();

            LogManager.ThrowExceptions = true;
            LogManager.Configuration = new XmlLoggingConfiguration(Path.Combine(DirectoryHelper.BaseDirectory, "nlog.config"));
            _uiDispatcher = Dispatcher.CurrentDispatcher;
#pragma warning disable CS4014
            InitializeAsync();
#pragma warning restore CS4014
        }

        /// <summary>
        /// Update deskband options when the size changes
        /// </summary>
        /// <param name="eventArgs">.</param>
        protected override void OnResize(EventArgs eventArgs)
        {
            if (_audioBandModel == null)
            {
                return;
            }

            var audioBandSize = new Size(_audioBandModel.Width, _audioBandModel.Height);
            Options.MinHorizontalSize = audioBandSize;
            Options.HorizontalSize = audioBandSize;
            Options.MaxHorizontalHeight = audioBandSize.Height;
        }

        /// <summary>
        /// Save on close
        /// </summary>
        protected override void OnClose()
        {
            base.OnClose();
            _appSettings.Save();
            try
            {
                _currentAudioSource?.DeactivateAsync();
            }
            catch (Exception)
            {
                // ignore
            }
        }

        private async Task InitializeAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    _audioSourceContextMenuItems = new List<DeskBandMenuAction>();
                    _settingsMenuItem = new DeskBandMenuAction("Audio Band Settings");
                    _settingsMenuItem.Clicked += SettingsMenuItemOnClicked;
                    RefreshContextMenu();

                    InitializeModels();
                }).ConfigureAwait(false);

                _settingsWindowVm = await SetupViewModels().ConfigureAwait(false);

                await _uiDispatcher.InvokeAsync(() =>
                {
                    _settingsWindow = new SettingsWindow(_settingsWindowVm);
                    _settingsWindow.Saved += SettingsWindowOnSaved;
                    _settingsWindow.Canceled += SettingsWindowOnCanceled;
                    ElementHost.EnableModelessKeyboardInterop(_settingsWindow);
                });

                _audioSourceManager = new AudioSourceManager();
                _audioSourceManager.AudioSources.CollectionChanged += AudioSourcesOnCollectionChanged;
                _audioSourceManager.LoadAudioSources();

                Logger.Debug("Initialization complete");
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private void InitializeModels()
        {
            _albumArtModel = _appSettings.AlbumArt;
            _albumArtPopupModel = _appSettings.AlbumArtPopup;
            _audioBandModel = _appSettings.AudioBand;
            _customLabelsModel = _appSettings.CustomLabels;
            _nextButtonModel = _appSettings.NextButton;
            _playPauseButtonModel = _appSettings.PlayPauseButton;
            _previousButtonModel = _appSettings.PreviousButton;
            _progressBarModel = _appSettings.ProgressBar;
            _audioSourceSettingsModel = _appSettings.AudioSourceSettings;
            _trackModel = new Track();
        }

        private async Task<SettingsWindowVM> SetupViewModels()
        {
            var albumArt = new AlbumArtVM(_albumArtModel, _trackModel);
            var albumArtPopup = new AlbumArtPopupVM(_albumArtPopupModel, _trackModel);
            var audioBand = new AudioBandVM(_audioBandModel);
            var customLabels = new CustomLabelsVM(_customLabelsModel, this);
            var nextButton = new NextButtonVM(_nextButtonModel);
            var playPauseButton = new PlayPauseButtonVM(_playPauseButtonModel, _trackModel);
            var prevButton = new PreviousButtonVM(_previousButtonModel);
            var progressBar = new ProgressBarVM(_progressBarModel, _trackModel);

            await _uiDispatcher.InvokeAsync(() => InitializeBindingSources(albumArtPopup, albumArt, audioBand, nextButton, playPauseButton, prevButton, progressBar));

            return new SettingsWindowVM
            {
                AlbumArtPopupVM = albumArtPopup,
                ProgressBarVM = progressBar,
                PreviousButtonVM = prevButton,
                PlayPauseButtonVM = playPauseButton,
                NextButtonVM = nextButton,
                AudioBandVM = audioBand,
                AboutVm = new AboutVM(),
                AlbumArtVM = albumArt,
                CustomLabelsVM = customLabels,
                AudioSourceSettingsVM = new ObservableCollection<AudioSourceSettingsVM>()
            };
        }

        private void OpenSettingsWindow()
        {
            _settingsWindowVm.BeginEdit();
            _settingsWindow.Show();
        }

        private void RefreshContextMenu()
        {
            _pluginSubMenu = new DeskBandMenu("Audio Source", _audioSourceContextMenuItems);
            Options.ContextMenuItems = new List<DeskBandMenuItem> { _settingsMenuItem, _pluginSubMenu };
        }

        private async Task SubscribeToAudioSource(IAudioSource source)
        {
            if (source == null)
            {
                return;
            }

            ResetTrack();

            source.TrackInfoChanged += AudioSourceOnTrackInfoChanged;
            source.TrackPlaying += AudioSourceOnTrackPlaying;
            source.TrackPaused += AudioSourceOnTrackPaused;
            source.TrackProgressChanged += AudioSourceOnTrackProgressChanged;

            await source.ActivateAsync().ConfigureAwait(false);

            _appSettings.AudioSource = source.Name;

            Logger.Debug($"Audio source selected: `{source.Name}`");
        }

        private async Task UnsubscribeToAudioSource(IAudioSource source)
        {
            if (source == null)
            {
                return;
            }

            source.TrackInfoChanged -= AudioSourceOnTrackInfoChanged;
            source.TrackPlaying -= AudioSourceOnTrackPlaying;
            source.TrackPaused -= AudioSourceOnTrackPaused;
            source.TrackProgressChanged -= AudioSourceOnTrackProgressChanged;

            await source.DeactivateAsync().ConfigureAwait(false);

            _appSettings.AudioSource = null;
            _currentAudioSource = null;

            ResetTrack();

            Logger.Debug($"Audio source `{source.Name}` deactivated");
        }

        private async Task HandleAudioSourceContextMenuItemClick(DeskBandMenuAction menuItem)
        {
            try
            {
                if (menuItem.Checked)
                {
                    menuItem.Checked = false;
                    await UnsubscribeToAudioSource(_currentAudioSource).ConfigureAwait(false);
                    return;
                }

                // Uncheck old items and unsubscribe from the current source
                foreach (var otherMenuItem in _pluginSubMenu.Items.Cast<DeskBandMenuAction>().Where(i => i != null))
                {
                    otherMenuItem.Checked = false;
                }

                await UnsubscribeToAudioSource(_currentAudioSource).ConfigureAwait(false);

                _currentAudioSource = _audioSourceManager.AudioSources.FirstOrDefault(c => c.Name == menuItem.Text);
                if (_currentAudioSource == null)
                {
                    Logger.Warn($"Could not find matching audio source. Looking for {menuItem.Text}.");
                    return;
                }

                await SubscribeToAudioSource(_currentAudioSource).ConfigureAwait(false);
                menuItem.Checked = true;
            }
            catch (Exception e)
            {
                Logger.Debug(e, $"Error activating audio source `{_currentAudioSource?.Name}`");
                _currentAudioSource = null;
            }
        }

        private void ResetTrack()
        {
            _trackModel.AlbumArt = null;
            _trackModel.AlbumName = null;
            _trackModel.Artist = null;
            _trackModel.TrackName = null;
            _trackModel.IsPlaying = false;
            _trackModel.TrackLength = TimeSpan.Zero;
            _trackModel.TrackProgress = TimeSpan.Zero;
        }
    }
}
