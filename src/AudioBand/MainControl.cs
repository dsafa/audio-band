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
using Appearance = AudioBand.ViewModels.Appearance;
using Size = System.Drawing.Size;

namespace AudioBand
{
    [Guid("957D8782-5B07-4126-9B24-1E917BAAAD64")]
    [ComVisible(true)]
    [CSDeskBandRegistration(Name = "Audio Band")]
    public partial class MainControl : CSDeskBandWin
    {
        private static readonly SvgDocument PlayButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.play));
        private static readonly SvgDocument PauseButtonSvg = SvgDocument.Open<SvgDocument>(new  MemoryStream(Properties.Resources.pause));
        private static readonly SvgDocument NextButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.next));
        private static readonly SvgDocument PreviousButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.previous));
        private static readonly SvgDocument AlbumArtPlaceholderSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.placeholder_album));
        private const string LocationXPropertyName = "Location.X";
        private const string LocationYPropertyName = "Location.Y";
        private readonly AudioSourceStatus _audioSourceStatus = new AudioSourceStatus();
        private readonly AudioSourceManager _audioSourceManager;
        private readonly ILogger _logger = LogManager.GetLogger("Audio Band");
        private readonly AlbumArtTooltip _albumArtTooltip = new AlbumArtTooltip { Size = new Size(100, 100) };
        private readonly SettingsManager _settingsManager;
        private readonly SettingsWindow _settingsWindow;
        private readonly Appearance _appearance;
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

            try
            {
                _settingsManager = new SettingsManager();
                _appearance = _settingsManager.Appearance;
                _settingsWindow = new SettingsWindow(_appearance);
                _settingsWindow.Saved += SettingsWindowOnSaved;
                ElementHost.EnableModelessKeyboardInterop(_settingsWindow);

                Options.HeightIncrement = 0;
                UpdateSize();

                _audioSourceStatus.PropertyChanged += AudioSourceStatusOnPropertyChanged;
                _appearance.AudioBandAppearance.PropertyChanged += AudioBandAppearanceOnPropertyChanged;

                albumArt.DataBindings.Add(nameof(albumArt.Visible), _appearance.AlbumArtAppearance, nameof(AlbumArtDisplay.IsVisible));
                albumArt.DataBindings.Add(nameof(albumArt.Width), _appearance.AlbumArtAppearance, nameof(AlbumArtDisplay.Width));
                albumArt.DataBindings.Add(nameof(albumArt.Height), _appearance.AlbumArtAppearance, nameof(AlbumArtDisplay.Height));
                albumArt.DataBindings.Add(nameof(albumArt.Location), _appearance.AlbumArtAppearance, nameof(AlbumArtDisplay.Location));
                albumArt.DataBindings.Add(nameof(albumArt.Image), _audioSourceStatus, nameof(AudioSourceStatus.AlbumArt));
                // TODO load placeholder

                audioProgress.DataBindings.Add(nameof(audioProgress.Visible), _appearance.ProgressBarAppearance, nameof(ProgressBarAppearance.IsVisible));
                audioProgress.DataBindings.Add(nameof(audioProgress.Width), _appearance.ProgressBarAppearance, nameof(ProgressBarAppearance.Width));
                audioProgress.DataBindings.Add(nameof(audioProgress.Height), _appearance.ProgressBarAppearance, nameof(ProgressBarAppearance.Height));
                audioProgress.DataBindings.Add(nameof(audioProgress.Location), _appearance.ProgressBarAppearance, nameof(ProgressBarAppearance.Location));
                audioProgress.DataBindings.Add(nameof(audioProgress.ForeColor), _appearance.ProgressBarAppearance, nameof(ProgressBarAppearance.ForegroundColor));
                audioProgress.DataBindings.Add(nameof(audioProgress.BackColor), _appearance.ProgressBarAppearance, nameof(ProgressBarAppearance.BackgroundColor));
                audioProgress.DataBindings.Add(nameof(audioProgress.Progress), _audioSourceStatus, nameof(AudioSourceStatus.AudioProgress));

                playPauseButton.DataBindings.Add(nameof(playPauseButton.Visible), _appearance.PlayPauseButtonAppearance, nameof(PlayPauseButtonAppearance.IsVisible));
                playPauseButton.DataBindings.Add(nameof(playPauseButton.Width), _appearance.PlayPauseButtonAppearance, nameof(PlayPauseButtonAppearance.Width));
                playPauseButton.DataBindings.Add(nameof(playPauseButton.Height), _appearance.PlayPauseButtonAppearance, nameof(PlayPauseButtonAppearance.Height));
                playPauseButton.DataBindings.Add(nameof(playPauseButton.Location), _appearance.PlayPauseButtonAppearance, nameof(PlayPauseButtonAppearance.Location));
                playPauseButton.DataBindings.Add(nameof(playPauseButton.Image), _appearance.PlayPauseButtonAppearance, nameof(PlayPauseButtonAppearance.CurrentImage));

                previousButton.DataBindings.Add(nameof(previousButton.Visible), _appearance.PreviousSongButtonAppearance, nameof(PreviousSongButtonAppearance.IsVisible));
                previousButton.DataBindings.Add(nameof(previousButton.Width), _appearance.PreviousSongButtonAppearance, nameof(PreviousSongButtonAppearance.Width));
                previousButton.DataBindings.Add(nameof(previousButton.Height), _appearance.PreviousSongButtonAppearance, nameof(PreviousSongButtonAppearance.Height));
                previousButton.DataBindings.Add(nameof(previousButton.Location), _appearance.PreviousSongButtonAppearance, nameof(PreviousSongButtonAppearance.Location));
                previousButton.DataBindings.Add(nameof(previousButton.Image), _appearance.PreviousSongButtonAppearance, nameof(PreviousSongButtonAppearance.Image));

                nextButton.DataBindings.Add(nameof(nextButton.Visible), _appearance.NextSongButtonAppearance, nameof(NextSongButtonAppearance.IsVisible));
                nextButton.DataBindings.Add(nameof(nextButton.Width), _appearance.NextSongButtonAppearance, nameof(NextSongButtonAppearance.Width));
                nextButton.DataBindings.Add(nameof(nextButton.Height), _appearance.NextSongButtonAppearance, nameof(NextSongButtonAppearance.Height));
                nextButton.DataBindings.Add(nameof(nextButton.Location), _appearance.NextSongButtonAppearance, nameof(NextSongButtonAppearance.Location));
                nextButton.DataBindings.Add(nameof(nextButton.Image), _appearance.NextSongButtonAppearance, nameof(NextSongButtonAppearance.Image));

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
            _albumArtTooltip.Size = new Size(_appearance.AlbumArtPopupAppearance.Width, _appearance.AlbumArtPopupAppearance.Height);
            var margin = _appearance.AlbumArtPopupAppearance.Margin;
            var xOffSet = _appearance.AlbumArtPopupAppearance.XOffset;
            int yOffset = 0;

            if (TaskbarInfo.Edge == Edge.Bottom)
            {
                yOffset = -_albumArtTooltip.Size.Height - margin;
            }
            else if (TaskbarInfo.Edge == Edge.Top)
            {
                yOffset = Height + margin;
            }

            var pos = new Point(xOffSet, yOffset);

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

            ResetState();
            _settingsManager.AudioSource = null;
        }

        private void AudioSourceOnTrackProgressChanged(object o, double progress)
        {
            BeginInvoke(new Action(() => { _audioSourceStatus.AudioProgress = progress;}));
        }

        private void AudioSourceOnTrackPaused(object o, EventArgs args)
        {
            _logger.Debug("State set to paused");

            BeginInvoke(new Action(() =>_audioSourceStatus.IsPlaying = false));
        }

        private void AudioSourceOnTrackPlaying(object o, EventArgs args)
        {
            _logger.Debug("State set to playing");

            BeginInvoke(new Action(() => _audioSourceStatus.IsPlaying = true));
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
                //_audioSourceStatus.NowPlayingText = new NowPlayingText
                //{
                //    Artist = trackInfoChangedEventArgs.Artist,
                //    TrackName = trackInfoChangedEventArgs.TrackName
                //};

                UpdateAlbumArt(_albumArt);
            }));
        }

        private void AudioSourceStatusOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(AudioSourceStatus.IsPlaying))
            {
                UpdateControlSvgs();
            }
        }

        private void AudioBandAppearanceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            UpdateSize();
        }

        private void UpdateSize()
        {
            var audioBandSize = new Size(_appearance.AudioBandAppearance.Width, _appearance.AudioBandAppearance.Height);
            Options.HorizontalSize = Size = audioBandSize;
            Options.MaxHorizontalHeight = audioBandSize.Height;
            Options.MinHorizontalSize = MinimumSize = audioBandSize;
        }

        private async void PlayPauseButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (_audioSourceStatus.IsPlaying)
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

        private void UpdateAlbumArt(Image newAlbumArt)
        {
            var sizedAlbumArt = new Bitmap(1, 1);
            using (var graphics = Graphics.FromImage(sizedAlbumArt))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(newAlbumArt, 0, 0, sizedAlbumArt.Width, sizedAlbumArt.Height);
            }

            //_audioSourceStatus.AlbumArt = sizedAlbumArt;
        }

        // Update the svgs for play/pause, prev, next buttons
        private void UpdateControlSvgs()
        {
            // Issues with svg so need padding
            const int padding = 3;
            var height = padding;

            SvgDocument playPauseSvg = _audioSourceStatus.IsPlaying ? PauseButtonSvg : PlayButtonSvg;
            playPauseSvg.Width = playPauseButton.Width;
            playPauseSvg.Height = height;
            //_audioSourceStatus.PlayPauseButtonBitmap = playPauseSvg.ToBitmap();

            NextButtonSvg.Width = nextButton.Width;
            NextButtonSvg.Height = height;
            //_audioSourceStatus.NextButtonBitmap = NextButtonSvg.ToBitmap();

            PreviousButtonSvg.Width = previousButton.Width;
            PreviousButtonSvg.Height = height;
            //_audioSourceStatus.PreviousButtonBitmap = PreviousButtonSvg.ToBitmap();
        }

        // Reset all images to blank state
        private void ResetState()
        {
            var placeholder = AlbumArtPlaceholderSvg.ToBitmap();
            _albumArt = placeholder;
            UpdateAlbumArt(placeholder);
            _audioSourceStatus.IsPlaying = false;
            _audioSourceStatus.AudioProgress = 0;
            _albumArtTooltip.AlbumArt = null;
        }

        protected override void OnClose()
        {
            base.OnClose();
            _settingsManager.Save();
        }

        private void SettingsWindowOnSaved(object sender, EventArgs eventArgs)
        {
            _settingsManager.Save();
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
