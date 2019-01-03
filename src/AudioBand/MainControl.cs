using System;
using System.Collections.Generic;
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
using NLog.Targets;
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
        private static readonly object _audiosourceListLock = new object();
        private readonly AppSettings _appSettings = new AppSettings();
        private readonly Dispatcher _uiDispatcher;
        private AudioSourceManager _audioSourceManager;
        private SettingsWindow _settingsWindow;
        private IAudioSource _currentAudioSource;
        private DeskBandMenu _pluginSubMenu;
        private CancellationTokenSource _audioSourceTokenSource = new CancellationTokenSource();
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
            var fileTarget = new FileTarget
            {
                MaxArchiveFiles = 3,
                ArchiveOldFileOnStartup = true,
                FileName = "${environment:variable=TEMP}/AudioBand.log",
                KeepFileOpen = true,
                OpenFileCacheTimeout = 30,
                Layout = NLog.Layouts.Layout.FromString("${longdate}|${level:uppercase=true}|${logger}|${message} ${exception:format=tostring}")
            };

            var nullTarget = new NullTarget();

            var filter = new LoggingRule("CSDeskBand.*", LogLevel.Trace, nullTarget) { Final = true };
            var fileRule = new LoggingRule("*", LogLevel.Debug, fileTarget);

            var config = new LoggingConfiguration();
            config.AddTarget("logfile", fileTarget);
            config.AddTarget("null", nullTarget);
            config.LoggingRules.Add(filter);
            config.LoggingRules.Add(fileRule);

            LogManager.Configuration = config;

            AppDomain.CurrentDomain.UnhandledException += (sender, args) => LogManager.GetCurrentClassLogger().Error((Exception)args.ExceptionObject);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainControl"/> class.
        /// Entry point.
        /// </summary>
        public MainControl()
        {
            InitializeComponent();
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif
            _uiDispatcher = Dispatcher.CurrentDispatcher;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            InitializeAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
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
            _currentAudioSource.DeactivateAsync();
            _audioSourceManager.Close();
        }

        private async Task InitializeAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    _audioSourceManager = new AudioSourceManager();
                    _audioSourceManager.AudioSourcesChanged += AudioSourceManagerOnAudioSourcesChanged;
                    _audioSourceManager.LoadAudioSources();
                    Options.ContextMenuItems = BuildContextMenu();
                    InitializeModels();
                }).ConfigureAwait(false);

                _settingsWindowVm = await SetupViewModels().ConfigureAwait(false);

                await _uiDispatcher.InvokeAsync(() =>
                {
                    _settingsWindow = new SettingsWindow(_settingsWindowVm);
                    _settingsWindow.Saved += Saved;
                    _settingsWindow.Canceled += Canceled;
                    ElementHost.EnableModelessKeyboardInterop(_settingsWindow);
                });

                await SelectAudioSourceFromSettings().ConfigureAwait(false);
                Logger.Debug("Initialization complete");
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private void AudioSourceManagerOnAudioSourcesChanged(object sender, EventArgs e)
        {
            Options.ContextMenuItems = BuildContextMenu();
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
            var allAudioSourceSettings = new List<AudioSourceSettingsVM>();

            foreach (var audioSource in _audioSourceManager.AudioSources)
            {
                var matchingSetting = _audioSourceSettingsModel.FirstOrDefault(s => s.AudioSourceName == audioSource.Name);
                if (matchingSetting != null)
                {
                    allAudioSourceSettings.Add(new AudioSourceSettingsVM(matchingSetting, audioSource));
                }
                else
                {
                    var newSettings = new AudioSourceSettings { AudioSourceName = audioSource.Name };
                    _audioSourceSettingsModel.Add(newSettings);
                    allAudioSourceSettings.Add(new AudioSourceSettingsVM(newSettings, audioSource));
                }
            }

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
                AudioSourceSettingsVM = allAudioSourceSettings
            };
        }

        private void OpenSettingsWindow()
        {
            _settingsWindowVm.BeginEdit();
            _settingsWindow.Show();
        }

        private List<DeskBandMenuItem> BuildContextMenu()
        {
            List<DeskBandMenuAction> pluginList;

            lock (_audiosourceListLock)
            {
                pluginList = _audioSourceManager.AudioSources.Select(audioSource =>
                {
                    var item = new DeskBandMenuAction(audioSource.Name);
                    item.Clicked += AudioSourceMenuItemOnClicked;
                    return item;
                }).ToList();
            }

            _pluginSubMenu = new DeskBandMenu("Audio Source", pluginList);
            var settingsMenuItem = new DeskBandMenuAction("Audio Band Settings");
            settingsMenuItem.Clicked += SettingsMenuItemOnClicked;

            return new List<DeskBandMenuItem> { settingsMenuItem, _pluginSubMenu };
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

            _audioSourceTokenSource = new CancellationTokenSource();
            await source.ActivateAsync(_audioSourceTokenSource.Token);

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

            _audioSourceTokenSource.Cancel();
            await source.DeactivateAsync();

            _appSettings.AudioSource = null;
            _currentAudioSource = null;

            ResetTrack();

            Logger.Debug($"Audio source `{source.Name}` deactivated");
        }

        private async Task SelectAudioSourceFromSettings()
        {
            try
            {
                var audioSource = _appSettings.AudioSource;
                if (string.IsNullOrEmpty(audioSource))
                {
                    return;
                }

                var menuItem = _pluginSubMenu.Items.Cast<DeskBandMenuAction>().FirstOrDefault(i => i.Text == audioSource);
                if (menuItem != null)
                {
                    await Task.Run(() => AudioSourceMenuItemOnClicked(menuItem, EventArgs.Empty));
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
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
