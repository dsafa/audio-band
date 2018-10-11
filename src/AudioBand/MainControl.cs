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
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using AudioBand.Models;
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
        private readonly AudioSourceStatus _audioSourceStatus = new AudioSourceStatus();
        private readonly AudioSourceManager _audioSourceManager;
        private readonly ILogger _logger = LogManager.GetLogger("Audio Band");
        private readonly AlbumArtTooltip _albumArtTooltip = new AlbumArtTooltip { Size = new Size(100, 100) };
        private readonly SettingsManager _settingsManager;
        private SettingsWindow _settingsWindow;
        private readonly Appearance _appearance;
        private IAudioSource _currentAudioSource;
        private DeskBandMenu _pluginSubMenu; 
        private CancellationTokenSource _audioSourceTokenSource = new CancellationTokenSource();
        private int _nextTag = 0;

        public bool AlbumArtPopupIsVisible { get; set; }
        public int AlbumArtPopupWidth { get; set; }
        public int AlbumArtPopupHeight { get; set; }
        public int AlbumArtPopupX { get; set; }
        public int AlbumArtPopupY { get; set; }

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
        }

        public MainControl()
        {
            InitializeComponent();

            try
            {
                _audioSourceManager = new AudioSourceManager();
                _audioSourceManager.AudioSourcesChanged += AudioSourceManagerOnAudioSourcesChanged;
                Options.ContextMenuItems = BuildContextMenu();

                _settingsManager = new SettingsManager();
                _appearance = _settingsManager.Appearance;
                CreateSettingsWindow();

                Options.HeightIncrement = 0;
                UpdateSize();

                _audioSourceStatus.PropertyChanged += AudioSourceStatusOnPropertyChanged;
                _appearance.AudioBandAppearance.PropertyChanged += AudioBandAppearanceOnPropertyChanged;
                _appearance.AlbumArtAppearance.PropertyChanged += AlbumArtAppearanceOnPropertyChanged;

                albumArt.DataBindings.Add(nameof(albumArt.Visible), _appearance.AlbumArtAppearance, nameof(AlbumArtDisplay.IsVisible));
                albumArt.DataBindings.Add(nameof(albumArt.Width), _appearance.AlbumArtAppearance, nameof(AlbumArtDisplay.Width));
                albumArt.DataBindings.Add(nameof(albumArt.Height), _appearance.AlbumArtAppearance, nameof(AlbumArtDisplay.Height));
                albumArt.DataBindings.Add(nameof(albumArt.Location), _appearance.AlbumArtAppearance, nameof(AlbumArtDisplay.Location));
                albumArt.DataBindings.Add(nameof(albumArt.Image), _appearance.AlbumArtAppearance, nameof(AlbumArtDisplay.CurrentAlbumArt));

                audioProgress.DataBindings.Add(nameof(audioProgress.Visible), _appearance.ProgressBarAppearance, nameof(ProgressBarAppearance.IsVisible));
                audioProgress.DataBindings.Add(nameof(audioProgress.Width), _appearance.ProgressBarAppearance, nameof(ProgressBarAppearance.Width));
                audioProgress.DataBindings.Add(nameof(audioProgress.Height), _appearance.ProgressBarAppearance, nameof(ProgressBarAppearance.Height));
                audioProgress.DataBindings.Add(nameof(audioProgress.Location), _appearance.ProgressBarAppearance, nameof(ProgressBarAppearance.Location));
                audioProgress.DataBindings.Add(nameof(audioProgress.ForeColor), _appearance.ProgressBarAppearance, nameof(ProgressBarAppearance.ForegroundColor));
                audioProgress.DataBindings.Add(nameof(audioProgress.BackColor), _appearance.ProgressBarAppearance, nameof(ProgressBarAppearance.BackgroundColor));
                audioProgress.DataBindings.Add(nameof(audioProgress.Progress), _audioSourceStatus, nameof(AudioSourceStatus.SongProgress));

                playPauseButton.DataBindings.Add(nameof(playPauseButton.Visible), _appearance.PlayPauseButtonAppearance, nameof(PlayPauseButtonAppearance.IsVisible));
                playPauseButton.DataBindings.Add(nameof(playPauseButton.Width), _appearance.PlayPauseButtonAppearance, nameof(PlayPauseButtonAppearance.Width));
                playPauseButton.DataBindings.Add(nameof(playPauseButton.Height), _appearance.PlayPauseButtonAppearance, nameof(PlayPauseButtonAppearance.Height));
                playPauseButton.DataBindings.Add(nameof(playPauseButton.Location), _appearance.PlayPauseButtonAppearance, nameof(PlayPauseButtonAppearance.Location));
                playPauseButton.DataBindings.Add(nameof(playPauseButton.Image), _appearance.PlayPauseButtonAppearance, nameof(PlayPauseButtonAppearance.Image));

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

                // Add bindings here since tooltips dont have any
                DataBindings.Add(nameof(AlbumArtPopupIsVisible), _appearance.AlbumArtPopupAppearance, nameof(AlbumArtPopup.IsVisible));
                DataBindings.Add(nameof(AlbumArtPopupWidth), _appearance.AlbumArtPopupAppearance, nameof(AlbumArtPopup.Width));
                DataBindings.Add(nameof(AlbumArtPopupHeight), _appearance.AlbumArtPopupAppearance, nameof(AlbumArtPopup.Height));
                DataBindings.Add(nameof(AlbumArtPopupX), _appearance.AlbumArtPopupAppearance, nameof(AlbumArtPopup.XOffset));
                DataBindings.Add(nameof(AlbumArtPopupY), _appearance.AlbumArtPopupAppearance, nameof(AlbumArtPopup.Margin));

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
            _albumArtTooltip.Hide(this);
        }

        private void AlbumArtOnMouseHover(object o, EventArgs args)
        {
            _albumArtTooltip.ShowWithoutRequireFocus("Album Art", this, TaskbarInfo);
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

            _settingsManager.AudioSource = null;
        }

        private void AudioSourceOnTrackProgressChanged(object o, double progress)
        {
            //BeginInvoke(new Action(() => { _audioSourceStatus.SongProgress = progress;}));
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

            BeginInvoke(new Action(() =>
            {
                var art = trackInfoChangedEventArgs.AlbumArt ?? _appearance.AlbumArtAppearance.Placeholder;
                _appearance.AlbumArtAppearance.CurrentAlbumArt = art;
                _appearance.AlbumArtPopupAppearance.CurrentAlbumArt = art;
                _albumArtTooltip.AlbumArt = _appearance.AlbumArtPopupAppearance.CurrentAlbumArt;

                _audioSourceStatus.Artist = trackInfoChangedEventArgs.Artist;
                _audioSourceStatus.SongName = trackInfoChangedEventArgs.TrackName;
            }));
        }

        private void AudioSourceStatusOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != nameof(AudioSourceStatus.IsPlaying))
            {
                return;
            }

            _appearance.PlayPauseButtonAppearance.IsPlaying = _audioSourceStatus.IsPlaying;
        }

        private void AudioBandAppearanceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            UpdateSize();
        }

        private void AlbumArtAppearanceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            // if not playing update placeholder
            if (propertyChangedEventArgs.PropertyName == nameof(AlbumArtDisplay.Placeholder) && !_audioSourceStatus.IsPlaying)
            {
                _appearance.AlbumArtAppearance.CurrentAlbumArt = _appearance.AlbumArtAppearance.Placeholder;
                _albumArtTooltip.AlbumArt = _appearance.AlbumArtAppearance.Placeholder;
            }
        }

        private void UpdateSize()
        {
            var audioBandSize = new Size(_appearance.AudioBandAppearance.Width, _appearance.AudioBandAppearance.Height);
            Options.MinHorizontalSize = audioBandSize;
            Options.HorizontalSize = audioBandSize;
            Options.MaxHorizontalHeight = audioBandSize.Height;

            MinimumSize = audioBandSize;
            Size = audioBandSize;
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

        protected override void OnClose()
        {
            base.OnClose();
            _settingsManager.Save();
        }

        private void SettingsWindowOnSaved(object sender, EventArgs eventArgs)
        {
            _settingsManager.Save();
        }

        private void CreateSettingsWindow()
        {
            _settingsWindow = new SettingsWindow(_appearance, _audioSourceManager.AudioSources.ToList(), _settingsManager.AudioSourceSettings);
            _settingsWindow.Saved += SettingsWindowOnSaved;
            _settingsWindow.NewLabelCreated += SettingsWindowOnNewLabelCreated;
            _settingsWindow.LabelDeleted += SettingsWindowOnLabelDeleted;
            _settingsWindow.Closed += SettingsWindowOnClosed;
            ElementHost.EnableModelessKeyboardInterop(_settingsWindow);
        }

        private void SettingsWindowOnClosed(object sender, EventArgs eventArgs)
        {
            CreateSettingsWindow();
        }

        private void ResetState()
        {
            var art = _appearance.AlbumArtAppearance.Placeholder;
            _appearance.AlbumArtAppearance.CurrentAlbumArt = art;
            _appearance.AlbumArtPopupAppearance.CurrentAlbumArt = art;
            _albumArtTooltip.AlbumArt = _appearance.AlbumArtPopupAppearance.CurrentAlbumArt;

            _audioSourceStatus.Artist = "";
            _audioSourceStatus.SongName = "";
            _audioSourceStatus.IsPlaying = false;
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
            foreach (var textAppearance in _appearance.TextAppearances)
            {
                CreateTextLabel(textAppearance);
            }
        }

        private void CreateTextLabel(TextAppearance appearance)
        {
            var label = new FormattedTextLabel(appearance.FormatString, appearance.Color, appearance.FontSize, appearance.FontFamily, appearance.TextAlignment);
            label.DataBindings.Add(nameof(label.Format), appearance, nameof(appearance.FormatString));
            label.DataBindings.Add(nameof(label.DefaultColor), appearance, nameof(appearance.Color));
            label.DataBindings.Add(nameof(label.FontSize), appearance, nameof(appearance.FontSize));
            label.DataBindings.Add(nameof(label.FontFamily), appearance, nameof(appearance.FontFamily));
            label.DataBindings.Add(nameof(label.Alignment), appearance, nameof(appearance.TextAlignment));

            label.DataBindings.Add(nameof(label.Visible), appearance, nameof(appearance.IsVisible));
            label.DataBindings.Add(nameof(label.Width), appearance, nameof(appearance.Width));
            label.DataBindings.Add(nameof(label.Height), appearance, nameof(appearance.Height));
            label.DataBindings.Add(nameof(label.Location), appearance, nameof(appearance.Location));
            label.DataBindings.Add(nameof(label.ScrollSpeed), appearance, nameof(appearance.ScrollSpeed));

            label.AlbumName = _audioSourceStatus.AlbumName;
            label.Artist = _audioSourceStatus.Artist;
            label.SongName = _audioSourceStatus.SongName;
            label.SongLength = _audioSourceStatus.SongLength;
            label.SongProgress = _audioSourceStatus.SongProgress;

            label.DataBindings.Add(nameof(label.AlbumName), _audioSourceStatus, nameof(_audioSourceStatus.AlbumName));
            label.DataBindings.Add(nameof(label.Artist), _audioSourceStatus, nameof(_audioSourceStatus.Artist));
            label.DataBindings.Add(nameof(label.SongName), _audioSourceStatus, nameof(_audioSourceStatus.SongName));
            label.DataBindings.Add(nameof(label.SongLength), _audioSourceStatus, nameof(_audioSourceStatus.SongLength));
            label.DataBindings.Add(nameof(label.SongProgress), _audioSourceStatus, nameof(_audioSourceStatus.SongProgress));

            appearance.Tag = _nextTag++;
            label.TagId = appearance.Tag;
            label.Name = "formatted label";
            Controls.Add(label);
        }

        private void SettingsWindowOnNewLabelCreated(object sender, TextLabelChangedEventArgs textLabelChangedEventArgs)
        {
            CreateTextLabel(textLabelChangedEventArgs.Appearance);
        }

        private void SettingsWindowOnLabelDeleted(object sender, TextLabelChangedEventArgs textLabelChangedEventArgs)
        {
            Controls.Remove(Controls.Find("formatted label", true).Cast<FormattedTextLabel>().FirstOrDefault(l => l.TagId == textLabelChangedEventArgs.Appearance.Tag));
        }
    }
}
