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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using SettingsWindow = AudioBand.Views.Wpf.SettingsWindow;
using Size = System.Drawing.Size;

namespace AudioBand
{
    [Guid("957D8782-5B07-4126-9B24-1E917BAAAD64")]
    [ComVisible(true)]
    [CSDeskBandRegistration(Name = "Audio Band")]
    public partial class MainControl : CSDeskBandWin
    {
        private readonly ILogger _logger = LogManager.GetLogger("Audio Band");
        private readonly AudioSourceManager _audioSourceManager = new AudioSourceManager();
        private readonly AppSettings _appSettings = new AppSettings();
        private SettingsWindow _settingsWindow;
        private IAudioSource _currentAudioSource;
        private DeskBandMenu _pluginSubMenu; 
        private CancellationTokenSource _audioSourceTokenSource = new CancellationTokenSource();

        #region Models

        private AlbumArt _albumArtModel;
        private AlbumArtPopup _albumArtPopupModel;
        private Models.AudioBand _audioBandModel;
        private AudioSourceSettings _audioSourceSettingsModel;
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
                ConcurrentWrites = true
            };

            var fileRule = new LoggingRule("*", LogLevel.Debug, fileTarget);

            var config = new LoggingConfiguration();
            config.AddTarget("logfile", fileTarget);
            config.LoggingRules.Add(fileRule);

            LogManager.Configuration = config;

            AppDomain.CurrentDomain.UnhandledException += (sender, args) => LogManager.GetCurrentClassLogger().Error((Exception) args.ExceptionObject);
        }

        public MainControl()
        {
            try
            {
                InitializeComponent();
                Options.ContextMenuItems = BuildContextMenu();

                InitializeModels();
                SetupViewModels();
                SelectAudioSourceFromSettings();
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
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
            _trackModel = new Track();   
        }

        private void SetupViewModels()
        {
            var albumArt = new AlbumArtVM(_albumArtModel, _trackModel);
            var albumArtPopup = new AlbumArtPopupVM(_albumArtPopupModel, _trackModel);
            var audioBand = new AudioBandVM(_audioBandModel);
            var customLabels = new CustomLabelsVM(_customLabelsModel, this);
            var nextButton = new NextButtonVM(_nextButtonModel);
            var playPauseButton = new PlayPauseButtonVM(_playPauseButtonModel, _trackModel);
            var prevButton = new PreviousButtonVM(_previousButtonModel);
            var progressBar = new ProgressBarVM(_progressBarModel, _trackModel);

            InitializeBindingSources(albumArtPopup, albumArt, audioBand, nextButton, playPauseButton, prevButton, progressBar);

            _settingsWindow = new SettingsWindow();
            ElementHost.EnableModelessKeyboardInterop(_settingsWindow);
        }

        private List<DeskBandMenuItem> BuildContextMenu()
        {
            var pluginList = _audioSourceManager.AudioSources.Select(audioSource =>
            {
                var item = new DeskBandMenuAction(audioSource.Name);
                item.Clicked += AudioSourceMenuItemOnClicked;
                return item;
            });

            _pluginSubMenu = new DeskBandMenu("Audio Source", pluginList);
            var settingsMenuItem = new DeskBandMenuAction("Audio Band Settings");
            settingsMenuItem.Clicked += SettingsMenuItemOnClicked;

            return new List<DeskBandMenuItem>{ settingsMenuItem, _pluginSubMenu };
        }

        private async Task SubscribeToAudioSource(IAudioSource source)
        {
            if (source == null)
            {
                return;
            }

            source.TrackInfoChanged += AudioSourceOnTrackInfoChanged;
            source.TrackPlaying += AudioSourceOnTrackPlaying;
            source.TrackPaused += AudioSourceOnTrackPaused;
            source.TrackProgressChanged += AudioSourceOnTrackProgressChanged;

            _audioSourceTokenSource = new CancellationTokenSource();
            await source.ActivateAsync();

            _appSettings.AudioSource = source.Name;
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
        }

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

        protected override void OnClose()
        {
            base.OnClose();
            _appSettings.Save();
        }

        private void OpenSettingsWindow()
        {
            _settingsWindow.Show();
        }

        private void SelectAudioSourceFromSettings()
        {
            var audioSource = _appSettings.AudioSource;
            if (String.IsNullOrEmpty(audioSource))
            {
                return;
            }

            var menuItem = _pluginSubMenu.Items.Cast<DeskBandMenuAction>().FirstOrDefault(i => i.Text == audioSource);
            if (menuItem != null)
            {
                AudioSourceMenuItemOnClicked(menuItem, EventArgs.Empty);
            }
        }
    }
}
