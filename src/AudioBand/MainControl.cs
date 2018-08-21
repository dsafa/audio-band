using AudioBand.AudioSource;
using AudioBand.Settings;
using CSDeskBand;
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
        private readonly AudioBandViewModel _audioBandViewModel = new AudioBandViewModel();
        private readonly AudioSourceManager _audioSourceManager;
        private readonly NLog.ILogger _logger = LogManager.GetLogger("Audio Band");
        private readonly AlbumArtTooltip _albumArtTooltip = new AlbumArtTooltip { Size = new Size(FixedWidth, FixedWidth) };
        private readonly SettingsManager _settingsManager;
        private readonly SettingsWindow _settingsWindow;
        private IAudioSource _currentAudioSource;
        private CSDeskBandMenu _pluginSubMenu;
        private Image _albumArt = DrawSvg(AlbumArtPlaceholderSvg); // Used so album art can be resized
        private CancellationTokenSource _connectorTokenSource = new CancellationTokenSource();

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

            Options.Increment = 0;
            var maxSize = new Size(FixedWidth, _maxHeight);
            Options.Horizontal = Size = mainTable.Size = maxSize;
            Options.MaxHorizontal = MaximumSize = mainTable.MaximumSize = maxSize;
            Options.MinHorizontal = MinimumSize = mainTable.MinimumSize = new Size(FixedWidth, _minHeight);

            try
            {
                _settingsManager = new SettingsManager();
                var audioBandAppearance = _settingsManager.AudioBandSettings.AudioBandAppearance;
                _settingsWindow = new SettingsWindow(audioBandAppearance);
                _settingsWindow.Closing += SettingsWindowOnClosing;

                ResetState();
                _audioBandViewModel.PropertyChanged += AudioBandViewModelOnPropertyChanged;
                SizeChanged += OnSizeChanged;

                nowPlayingText.DataBindings.Add(nameof(nowPlayingText.NowPlayingText), _audioBandViewModel, nameof(AudioBandViewModel.NowPlayingText));
                nowPlayingText.DataBindings.Add(nameof(nowPlayingText.ArtistFont), audioBandAppearance, nameof(AudioBandAppearance.NowPlayingArtistFont));
                nowPlayingText.DataBindings.Add(nameof(nowPlayingText.ArtistColor), audioBandAppearance, nameof(AudioBandAppearance.NowPlayingArtistColor));
                nowPlayingText.DataBindings.Add(nameof(nowPlayingText.TrackNameFont), audioBandAppearance, nameof(AudioBandAppearance.NowPlayingTrackNameFont));
                nowPlayingText.DataBindings.Add(nameof(nowPlayingText.TrackNameColor), audioBandAppearance, nameof(AudioBandAppearance.NowPlayingTrackNameColor));
                albumArt.DataBindings.Add(nameof(albumArt.Image), _audioBandViewModel, nameof(AudioBandViewModel.AlbumArt));
                audioProgress.DataBindings.Add(nameof(audioProgress.Progress), _audioBandViewModel, nameof(AudioBandViewModel.AudioProgress));
                audioProgress.DataBindings.Add(nameof(audioProgress.ForeColor), audioBandAppearance, nameof(AudioBandAppearance.TrackProgressColor));
                audioProgress.DataBindings.Add(nameof(audioProgress.BackColor), audioBandAppearance, nameof(AudioBandAppearance.TrackProgressBackColor));
                previousButton.DataBindings.Add(nameof(previousButton.Image), _audioBandViewModel, nameof(AudioBandViewModel.PreviousButtonBitmap));
                playPauseButton.DataBindings.Add(nameof(playPauseButton.Image), _audioBandViewModel, nameof(AudioBandViewModel.PlayPauseButtonBitmap));
                nextButton.DataBindings.Add(nameof(nextButton.Image), _audioBandViewModel, nameof(AudioBandViewModel.NextButtonBitmap));

                _audioSourceManager = new AudioSourceManager();
                _audioSourceManager.AudioSourcesChanged += AudioSourceManagerOnAudioSourcesChanged;
                Options.ContextMenuItems = BuildContextMenu();

                ApplySettings();
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
                yOffset = -FixedWidth - margin;
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

        private List<CSDeskBandMenuItem> BuildContextMenu()
        {
            var pluginList = _audioSourceManager.AudioConnectors.Select(connector =>
            {
                var item = new CSDeskBandMenuAction(connector.ConnectorName);
                item.Clicked += ConnectorMenuItemOnClicked;
                return item;
            });

            _pluginSubMenu = new CSDeskBandMenu("Audio Source", pluginList);
            var settingsMenuItem = new CSDeskBandMenuAction("Audio Band Settings");
            settingsMenuItem.Clicked += SettingsMenuItemOnClicked;

            return new List<CSDeskBandMenuItem>{ settingsMenuItem, _pluginSubMenu };
        }

        private void SettingsMenuItemOnClicked(object sender, EventArgs eventArgs)
        {
            _settingsWindow.Show(this);
        }

        private async void ConnectorMenuItemOnClicked(object sender, EventArgs eventArgs)
        {
            var item = (CSDeskBandMenuAction)sender;
            if (item.Checked)
            {
                item.Checked = false;
                await UnsubscribeToConnector(_currentAudioSource);
                _currentAudioSource = null;
                return;
            }
            // Uncheck old item and unsubscribe from the current source
            var lastItemChecked = _pluginSubMenu.Items.Cast<CSDeskBandMenuAction>().FirstOrDefault(i => i.Text == _currentAudioSource?.ConnectorName);
            if (lastItemChecked != null)
            {
                lastItemChecked.Checked = false;
            }

            await UnsubscribeToConnector(_currentAudioSource);

            item.Checked = true;
            _currentAudioSource = _audioSourceManager.AudioConnectors.First(c => c.ConnectorName == item.Text);
            await SubscribeToConnector(_currentAudioSource);
        }

        private async Task SubscribeToConnector(IAudioSource source)
        {
            if (source == null)
            {
                return;
            }

            source.TrackInfoChanged += ConnectorOnTrackInfoChanged;
            source.TrackPlaying += ConnectorOnTrackPlaying;
            source.TrackPaused += ConnectorOnTrackPaused;
            source.TrackProgressChanged += ConnectorOnTrackProgressChanged;

            _connectorTokenSource = new CancellationTokenSource();
            await source.ActivateAsync(new AudioSourceContext(source.ConnectorName));

            _settingsManager.AudioBandSettings.Connector = source.ConnectorName;
        }

        private async Task UnsubscribeToConnector(IAudioSource source)
        {
            if (source == null)
            {
                return;
            }

            source.TrackInfoChanged -= ConnectorOnTrackInfoChanged;
            source.TrackPlaying -= ConnectorOnTrackPlaying;
            source.TrackPaused -= ConnectorOnTrackPaused;
            source.TrackProgressChanged -= ConnectorOnTrackProgressChanged;

            _connectorTokenSource.Cancel();
            await source.DeactivateAsync();

            ResetState();
            _settingsManager.AudioBandSettings.Connector = null;
        }

        private void ConnectorOnTrackProgressChanged(object o, double progress)
        {
            BeginInvoke(new Action(() => { _audioBandViewModel.AudioProgress = progress;}));
        }

        private void ConnectorOnTrackPaused(object o, EventArgs args)
        {
            _logger.Debug("State set to paused");

            BeginInvoke(new Action(() =>_audioBandViewModel.IsPlaying = false));
        }

        private void ConnectorOnTrackPlaying(object o, EventArgs args)
        {
            _logger.Debug("State set to playing");

            BeginInvoke(new Action(() => _audioBandViewModel.IsPlaying = true));
        }

        private void ConnectorOnTrackInfoChanged(object sender, TrackInfoChangedEventArgs trackInfoChangedEventArgs)
        {
            if (trackInfoChangedEventArgs?.TrackName == null || trackInfoChangedEventArgs?.Artist == null)
            {
                _logger.Warn($"Trackname or artist is null, track '{trackInfoChangedEventArgs?.TrackName}' artist '{trackInfoChangedEventArgs?.Artist}'");
                return;
            }

            _logger.Debug($"Track changed - Name: '{trackInfoChangedEventArgs.TrackName}', Artist: '{trackInfoChangedEventArgs.Artist}'");

            _albumArt = trackInfoChangedEventArgs.AlbumArt;
            _albumArtTooltip.AlbumArt = _albumArt;
            if (_albumArt == null)
            {
                _albumArt = DrawSvg(AlbumArtPlaceholderSvg);
            }

            BeginInvoke(new Action(() =>
            {
                _audioBandViewModel.NowPlayingText = new NowPlayingText
                {
                    Artist = trackInfoChangedEventArgs.Artist,
                    TrackName = trackInfoChangedEventArgs.TrackName
                };

                UpdateAlbumArt(_albumArt);
            }));
        }

        private void AudioBandViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case nameof(AudioBandViewModel.IsPlaying):
                    UpdateControlSvgs();
                    break;
            }
        }

        private async void PlayPauseButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (_audioBandViewModel.IsPlaying)
            {
                await (_currentAudioSource?.PauseTrackAsync(_connectorTokenSource.Token) ?? Task.CompletedTask);
            }
            else
            {
                await (_currentAudioSource?.PlayTrackAsync(_connectorTokenSource.Token) ?? Task.CompletedTask);
            }
        }

        private async void PreviousButtonOnClick(object sender, EventArgs eventArgs)
        {
            await (_currentAudioSource?.PreviousTrackAsync(_connectorTokenSource.Token) ?? Task.CompletedTask);
        }

        private async void NextButtonOnClick(object sender, EventArgs eventArgs)
        {
            await (_currentAudioSource?.NextTrackAsync(_connectorTokenSource.Token) ?? Task.CompletedTask);
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

            _audioBandViewModel.AlbumArt = sizedAlbumArt;
        }

        private void UpdateControlSvgs()
        {
            // Issues with svg
            const int padding = 3;
            var height = buttonsTable.GetRowHeights()[0] - padding;

            SvgDocument playPauseSvg = _audioBandViewModel.IsPlaying ? PauseButtonSvg : PlayButtonSvg;
            playPauseSvg.Width = playPauseButton.Width;
            playPauseSvg.Height = height;
            _audioBandViewModel.PlayPauseButtonBitmap = DrawSvg(playPauseSvg);

            NextButtonSvg.Width = nextButton.Width;
            NextButtonSvg.Height = height;
            _audioBandViewModel.NextButtonBitmap = DrawSvg(NextButtonSvg);

            PreviousButtonSvg.Width = previousButton.Width;
            PreviousButtonSvg.Height = height;
            _audioBandViewModel.PreviousButtonBitmap = DrawSvg(PreviousButtonSvg);
        }

        private static Bitmap DrawSvg(SvgDocument svg)
        {
            var bmp = new Bitmap((int)svg.Width.Value, (int)svg.Height.Value);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.High;
                svg.Draw(graphics);
                return bmp;
            }
        }

        private void ResetState()
        {
            var placeholder = DrawSvg(AlbumArtPlaceholderSvg);
            _albumArt = placeholder;
            UpdateAlbumArt(placeholder);
            _audioBandViewModel.NowPlayingText = new NowPlayingText();
            _audioBandViewModel.IsPlaying = false;
            _audioBandViewModel.AudioProgress = 0;
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

        private void ApplySettings()
        {
            var connector = _settingsManager.AudioBandSettings.Connector;
            if (!String.IsNullOrEmpty(connector))
            {
                var menuItem = _pluginSubMenu.Items.Cast<CSDeskBandMenuAction>().FirstOrDefault(i => i.Text == connector);
                if (menuItem != null)
                {
                    ConnectorMenuItemOnClicked(menuItem, EventArgs.Empty);
                }
            }
        }
    }
}
