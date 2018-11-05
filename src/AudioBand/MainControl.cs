using AudioBand.AudioSource;
using AudioBand.Models;
using AudioBand.Settings;
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
using System.Reflection;
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
        private readonly SettingsManager _settingsManager = new SettingsManager();

        private IAudioSource _currentAudioSource;
        private DeskBandMenu _pluginSubMenu; 
        private CancellationTokenSource _audioSourceTokenSource = new CancellationTokenSource();

        #region Models

        private AlbumArt _albumArtModel;
        private AlbumArtPopup _albumArtPopupModel;
        private Models.AudioBand _audioBandModel;
        private AudioSourceSettingsCollection _audioSourceSettingsModel;
        private CustomLabelsCollection _customLabelsModel;
        private NextButton _nextButtonModel;
        private PlayPauseButton _playPauseButtonModel;
        private PreviousButton _previousButtonModel;
        private ProgressBar _progressBarModel;
        private Track _trackModel;

        #endregion

        #region Album art tooltip bindings

        // Tooltip will use bindings from this control
        public bool AlbumArtPopupIsVisible { get; set; }
        public int AlbumArtPopupWidth { get; set; }
        public int AlbumArtPopupHeight { get; set; }
        public int AlbumArtPopupX { get; set; }
        public int AlbumArtPopupY { get; set; }
        public Image AlbumArtPopupImage { get; set; }

        #endregion

        static MainControl()
        {
            var fileTarget = new FileTarget
            {
                DeleteOldFileOnStartup = true,
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
            InitializeComponent();
            CustomInitializeComponent();

            try
            {
                Options.ContextMenuItems = BuildContextMenu();
                ResizeDeskband();

                SelectAudioSourceFromSettings();
                ResetState();
            }
            catch (ReflectionTypeLoadException e)
            {
                _logger.Error(e);
                foreach (var loaderException in e.LoaderExceptions)
                {
                    _logger.Error(loaderException);
                }

                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
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

            _settingsManager.AudioSource = source.Name;
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

            _settingsManager.AudioSource = null;
            _currentAudioSource = null;
        }

        private void ResizeDeskband()
        {
            var audioBandSize = new Size(_audioBandModel.Width, _audioBandModel.Height);
            Options.MinHorizontalSize = audioBandSize;
            Options.HorizontalSize = audioBandSize;
            Options.MaxHorizontalHeight = audioBandSize.Height;

            MinimumSize = audioBandSize;
            Size = audioBandSize;
        }

        protected override void OnClose()
        {
            base.OnClose();
            _settingsManager.Save();
        }

        private void OpenSettingsWindow()
        {
            var window = new SettingsWindow();
            ElementHost.EnableModelessKeyboardInterop(window);
            window.Show();
        }

        private void ResetState()
        {
            _trackModel.Artist = "";
            _trackModel.TrackName = "";
            _trackModel.IsPlaying = false;
        }

        private void SelectAudioSourceFromSettings()
        {
            var audioSource = _settingsManager.AudioSource;
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
