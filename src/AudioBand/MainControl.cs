using AudioBand.AudioSource;
using AudioBand.Settings;
using CSDeskBand;
using CSDeskBand.ContextMenu;
using CSDeskBand.Win;
using NLog;
using NLog.Config;
using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using AudioBand.ViewModels;
using NLog.Targets;
using Size = System.Drawing.Size;

namespace AudioBand
{
    [Guid("957D8782-5B07-4126-9B24-1E917BAAAD64")]
    [ComVisible(true)]
    [CSDeskBandRegistration(Name = "Audio Band")]
    public partial class MainControl : CSDeskBandWin
    {
        private const int FixedWidth = 250;
        private static readonly SvgDocument PlayButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.play));
        private static readonly SvgDocument PauseButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.pause));
        private static readonly SvgDocument NextButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.next));
        private static readonly SvgDocument PreviousButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.previous));
        private static readonly SvgDocument AlbumArtPlaceholderSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.placeholder_album));
        private readonly int _maxHeight = CSDeskBandOptions.TaskbarHorizontalHeightLarge;
        private readonly int _minHeight = CSDeskBandOptions.TaskbarHorizontalHeightSmall;
        private readonly PlaybackViewModel _trackViewModel = new PlaybackViewModel();
        private readonly AudioSourceManager _audioSourceManager;
        private readonly ILogger _logger = LogManager.GetLogger("Audio Band");
        private readonly AlbumArtTooltip _albumArtTooltip = new AlbumArtTooltip { Size = new Size(FixedWidth, FixedWidth) };
        private readonly SettingsManager _settingsManager;
        private readonly SettingsWindow _settingsWindow;
        private IAudioSource _currentAudioSource;
        private DeskBandMenu _pluginSubMenu;
        private Image _albumArt = AlbumArtPlaceholderSvg.ToBitmap(); // Used so album art can be resized
        private CancellationTokenSource _audioSourceTokenSource = new CancellationTokenSource();

        static MainControl()
        {
            var fileTarget = new FileTarget
            {
                DeleteOldFileOnStartup = true,
                FileName = "${environment:variable=TEMP}/AudioBand.log"
            };

            var fileRule = new LoggingRule("*", LogLevel.Debug, fileTarget);

            var config = new LoggingConfiguration();
            config.AddTarget("logfile", fileTarget);
            config.LoggingRules.Add(fileRule);

            LogManager.Configuration = config;
        }

        public MainControl()
        {
            InitializeComponent();

            Options.HeightIncrement = 0;
            var maxSize = new Size(FixedWidth, _maxHeight);
            Options.HorizontalSize = Size = mainTable.Size = maxSize;
            mainTable.Height = maxSize.Height;
            mainTable.MaximumSize = maxSize;
            Options.MaxHorizontalHeight = maxSize.Height;
            Options.MinHorizontalSize = MinimumSize = mainTable.MinimumSize = new Size(FixedWidth, _minHeight);

            try
            {
                _settingsManager = new SettingsManager();
                var audioBandAppearance = _settingsManager.AudioBandSettings.AudioBandAppearance;
                _settingsWindow = new SettingsWindow();
                ElementHost.EnableModelessKeyboardInterop(_settingsWindow);
                _settingsWindow.Closing += SettingsWindowOnClosing;

                ResetState();
                _trackViewModel.PropertyChanged += TrackViewModelOnPropertyChanged;
                SizeChanged += OnSizeChanged;

                nowPlayingText.DataBindings.Add(nameof(nowPlayingText.NowPlayingText), _trackViewModel, nameof(PlaybackViewModel.NowPlayingText));
                nowPlayingText.DataBindings.Add(nameof(nowPlayingText.ArtistFont), audioBandAppearance, nameof(AudioBandAppearance.NowPlayingArtistFont));
                nowPlayingText.DataBindings.Add(nameof(nowPlayingText.ArtistColor), audioBandAppearance, nameof(AudioBandAppearance.NowPlayingArtistColor));
                nowPlayingText.DataBindings.Add(nameof(nowPlayingText.TrackNameFont), audioBandAppearance, nameof(AudioBandAppearance.NowPlayingTrackNameFont));
                nowPlayingText.DataBindings.Add(nameof(nowPlayingText.TrackNameColor), audioBandAppearance, nameof(AudioBandAppearance.NowPlayingTrackNameColor));
                albumArt.DataBindings.Add(nameof(albumArt.Image), _trackViewModel, nameof(PlaybackViewModel.AlbumArt));
                audioProgress.DataBindings.Add(nameof(audioProgress.Progress), _trackViewModel, nameof(PlaybackViewModel.AudioProgress));
                audioProgress.DataBindings.Add(nameof(audioProgress.ForeColor), audioBandAppearance, nameof(AudioBandAppearance.TrackProgressColor));
                audioProgress.DataBindings.Add(nameof(audioProgress.BackColor), audioBandAppearance, nameof(AudioBandAppearance.TrackProgressBackColor));
                previousButton.DataBindings.Add(nameof(previousButton.Image), _trackViewModel, nameof(PlaybackViewModel.PreviousButtonBitmap));
                playPauseButton.DataBindings.Add(nameof(playPauseButton.Image), _trackViewModel, nameof(PlaybackViewModel.PlayPauseButtonBitmap));
                nextButton.DataBindings.Add(nameof(nextButton.Image), _trackViewModel, nameof(PlaybackViewModel.NextButtonBitmap));

                _audioSourceManager = new AudioSourceManager();
                _audioSourceManager.AudioSourcesChanged += AudioSourceManagerOnAudioSourcesChanged;
                Options.ContextMenuItems = BuildContextMenu();

                SelectAudioSourceFromSettings();
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
            _albumArtTooltip.Hide(this);
        }

        private void AlbumArtOnMouseHover(object o, EventArgs args)
        {
            const int margin = 4;

            int yOffset = 0;
            if (TaskbarInfo.Edge == Edge.Bottom)
            {
                yOffset = -_albumArtTooltip.Size.Height - margin;
            }
            else if (TaskbarInfo.Edge == Edge.Top)
            {
                yOffset = Height + margin;
            }

            var pos = new Point(0, yOffset);
            _albumArtTooltip.ShowWithoutRequireFocus("Album Art", this, pos);
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
            await source.ActivateAsync(new AudioSourceContext(source.Name));

            _settingsManager.AudioBandSettings.AudioSource = source.Name;
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

            ResetState();
            _settingsManager.AudioBandSettings.AudioSource = null;
        }

        private void AudioSourceOnTrackProgressChanged(object o, double progress)
        {
            BeginInvoke(new Action(() => { _trackViewModel.AudioProgress = progress;}));
        }

        private void AudioSourceOnTrackPaused(object o, EventArgs args)
        {
            _logger.Debug("State set to paused");

            BeginInvoke(new Action(() =>_trackViewModel.IsPlaying = false));
        }

        private void AudioSourceOnTrackPlaying(object o, EventArgs args)
        {
            _logger.Debug("State set to playing");

            BeginInvoke(new Action(() => _trackViewModel.IsPlaying = true));
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

            _albumArt = trackInfoChangedEventArgs.AlbumArt ?? AlbumArtPlaceholderSvg.ToBitmap();
            _albumArtTooltip.AlbumArt = _albumArt;

            BeginInvoke(new Action(() =>
            {
                _trackViewModel.NowPlayingText = new NowPlayingText
                {
                    Artist = trackInfoChangedEventArgs.Artist,
                    TrackName = trackInfoChangedEventArgs.TrackName
                };

                UpdateAlbumArt(_albumArt);
            }));
        }

        private void TrackViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(PlaybackViewModel.IsPlaying))
            {
                UpdateControlSvgs();
            }
        }

        private async void PlayPauseButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (_trackViewModel.IsPlaying)
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

        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            UpdateAlbumArt(_albumArt);
            UpdateControlSvgs();
        }

        private void UpdateAlbumArt(Image newAlbumArt)
        {
            var height = mainTable.GetRowHeights().Take(2).Sum();
            mainTable.ColumnStyles[0].SizeType = SizeType.Absolute;
            mainTable.ColumnStyles[0].Width = height;

            var sizedAlbumArt = new Bitmap(height, height);
            using (var graphics = Graphics.FromImage(sizedAlbumArt))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(newAlbumArt, 0, 0, sizedAlbumArt.Width, sizedAlbumArt.Height);
            }

            _trackViewModel.AlbumArt = sizedAlbumArt;
        }

        // Update the svgs for play/pause, prev, next buttons
        private void UpdateControlSvgs()
        {
            // Issues with svg so need padding
            const int padding = 3;
            var height = buttonsTable.GetRowHeights()[0] - padding;

            SvgDocument playPauseSvg = _trackViewModel.IsPlaying ? PauseButtonSvg : PlayButtonSvg;
            playPauseSvg.Width = playPauseButton.Width;
            playPauseSvg.Height = height;
            _trackViewModel.PlayPauseButtonBitmap = playPauseSvg.ToBitmap();

            NextButtonSvg.Width = nextButton.Width;
            NextButtonSvg.Height = height;
            _trackViewModel.NextButtonBitmap = NextButtonSvg.ToBitmap();

            PreviousButtonSvg.Width = previousButton.Width;
            PreviousButtonSvg.Height = height;
            _trackViewModel.PreviousButtonBitmap = PreviousButtonSvg.ToBitmap();
        }

        // Reset all images to blank state
        private void ResetState()
        {
            var placeholder = AlbumArtPlaceholderSvg.ToBitmap();
            _albumArt = placeholder;
            UpdateAlbumArt(placeholder);
            _trackViewModel.NowPlayingText = new NowPlayingText();
            _trackViewModel.IsPlaying = false;
            _trackViewModel.AudioProgress = 0;
            _albumArtTooltip.AlbumArt = null;
        }

        protected override void OnClose()
        {
            base.OnClose();
            _settingsManager.Save();
        }

        private void SettingsWindowOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            _settingsManager.Save();
        }

        private void SelectAudioSourceFromSettings()
        {
            var audioSource = _settingsManager.AudioBandSettings.AudioSource;
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
