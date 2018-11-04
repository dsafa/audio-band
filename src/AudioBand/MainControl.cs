using AudioBand.AudioSource;
using AudioBand.Settings;
using AudioBand.ViewModels;
using AudioBand.Views.Winforms;
using AudioBand.Views.Wpf;
using CSDeskBand;
using CSDeskBand.ContextMenu;
using CSDeskBand.Win;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using AudioBand.Models;
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

        private SettingsWindow _settingsWindow;
        private IAudioSource _currentAudioSource;
        private DeskBandMenu _pluginSubMenu; 
        private CancellationTokenSource _audioSourceTokenSource = new CancellationTokenSource();
        private int _nextTag = 0;

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

            try
            {
                _audioSourceManager.AudioSourcesChanged += AudioSourceManagerOnAudioSourcesChanged;
                Options.ContextMenuItems = BuildContextMenu();
                CreateSettingsWindow();

                Options.HeightIncrement = 0;
                UpdateSize();

                LoadLabelsFromSettings();
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

        private void AlbumArtOnMouseLeave(object o, EventArgs args)
        {
            AlbumArtPopup.Hide(this);
        }

        private void AlbumArtOnMouseHover(object o, EventArgs args)
        {
            AlbumArtPopup.ShowWithoutRequireFocus("Album Art", this, TaskbarInfo);
        }

        private void AudioSourceManagerOnAudioSourcesChanged(object sender, EventArgs eventArgs)
        {
            BeginInvoke(new Action(() => { Options.ContextMenuItems = BuildContextMenu(); }));
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

        private void SettingsMenuItemOnClicked(object sender, EventArgs eventArgs)
        {
            _settingsWindow.Show();
        }

        private async void AudioSourceMenuItemOnClicked(object sender, EventArgs eventArgs)
        {
            var item = (DeskBandMenuAction)sender;
            if (item.Checked)
            {
                item.Checked = false;
                await UnsubscribeToAudioSource(_currentAudioSource);
                _currentAudioSource = null;
                return;
            }
            // Uncheck old item and unsubscribe from the current source
            var lastItemChecked = _pluginSubMenu.Items.Cast<DeskBandMenuAction>().FirstOrDefault(i => i.Text == _currentAudioSource?.Name);
            if (lastItemChecked != null)
            {
                lastItemChecked.Checked = false;
            }

            await UnsubscribeToAudioSource(_currentAudioSource);

            item.Checked = true;
            _currentAudioSource = _audioSourceManager.AudioSources.First(c => c.Name == item.Text);
            await SubscribeToAudioSource(_currentAudioSource);
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
        }

        private void AudioSourceOnTrackProgressChanged(object o, TimeSpan progress)
        {
            BeginInvoke(new Action(() => { _trackModel.TrackProgress = progress;}));
        }

        private void AudioSourceOnTrackPaused(object o, EventArgs args)
        {
            _logger.Debug("State set to paused");

            BeginInvoke(new Action(() =>_trackModel.IsPlaying = false));
        }

        private void AudioSourceOnTrackPlaying(object o, EventArgs args)
        {
            _logger.Debug("State set to playing");

            BeginInvoke(new Action(() => _trackModel.IsPlaying = true));
        }

        private void AudioSourceOnTrackInfoChanged(object sender, TrackInfoChangedEventArgs trackInfoChangedEventArgs)
        {
            if (trackInfoChangedEventArgs == null)
            {
                _logger.Error("TrackInforChanged event arg is empty");
                return;
            }

            if (trackInfoChangedEventArgs.TrackName == null)
            {
                trackInfoChangedEventArgs.TrackName = "";
                _logger.Warn("Track name is null");
            }

            if (trackInfoChangedEventArgs.Artist == null)
            {
                trackInfoChangedEventArgs.Artist = "";
                _logger.Warn("Artist is null");
            }

            _logger.Debug($"Track changed - Name: '{trackInfoChangedEventArgs.TrackName}', Artist: '{trackInfoChangedEventArgs.Artist}'");

            BeginInvoke(new Action(() =>
            {
                _trackModel.AlbumArt = trackInfoChangedEventArgs.AlbumArt;
                _trackModel.Artist = trackInfoChangedEventArgs.Artist;
                _trackModel.TrackName = trackInfoChangedEventArgs.TrackName;
                _trackModel.TrackLength = trackInfoChangedEventArgs.TrackLength;
                _trackModel.AlbumName = trackInfoChangedEventArgs.Album;
            }));
        }

        private void UpdateSize()
        {
            var audioBandSize = new Size(_audioBandModel.Width, _audioBandModel.Height);
            Options.MinHorizontalSize = audioBandSize;
            Options.HorizontalSize = audioBandSize;
            Options.MaxHorizontalHeight = audioBandSize.Height;

            MinimumSize = audioBandSize;
            Size = audioBandSize;
        }

        private async void PlayPauseButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (_trackModel.IsPlaying)
            {
                await (_currentAudioSource?.PauseTrackAsync(_audioSourceTokenSource.Token) ?? Task.CompletedTask);
            }
            else
            {
                await (_currentAudioSource?.PlayTrackAsync(_audioSourceTokenSource.Token) ?? Task.CompletedTask);
            }
        }

        private async void PreviousButtonOnClick(object sender, EventArgs eventArgs)
        {
            await (_currentAudioSource?.PreviousTrackAsync(_audioSourceTokenSource.Token) ?? Task.CompletedTask);
        }

        private async void NextButtonOnClick(object sender, EventArgs eventArgs)
        {
            await (_currentAudioSource?.NextTrackAsync(_audioSourceTokenSource.Token) ?? Task.CompletedTask);
        }

        protected override void OnClose()
        {
            base.OnClose();
            _settingsManager.Save();
        }


        private void CreateSettingsWindow()
        {
            _settingsWindow = new SettingsWindow();
            ElementHost.EnableModelessKeyboardInterop(_settingsWindow);
        }

        private void SettingsWindowOnClosed(object sender, EventArgs eventArgs)
        {
            CreateSettingsWindow();
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

        private void LoadLabelsFromSettings()
        {
        }
    }
}
