using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using System.Windows.Threading;
using AudioBand.AudioSource;
using AudioBand.Logging;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.ViewModels;
using AudioBand.Views.Winforms;
using AudioBand.Views.Wpf;
using CSDeskBand;
using CSDeskBand.ContextMenu;
using NLog;
using Size = System.Drawing.Size;

namespace AudioBand
{
    public partial class MainControl : AudioBandControl
    {
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger("Audio Band");
        private readonly IAppSettings _appSettings;
        private readonly IAudioSourceManager _audioSourceManager;
        private readonly ISettingsWindow _settingsWindow;
        private readonly ICustomLabelService _labelService;
        private readonly Dispatcher _uiDispatcher;
        private readonly Track _track;
        private IAudioSource _currentAudioSource;
        private DeskBandMenuAction _settingsMenuItem;
        private DeskBandMenu _pluginSubMenu;
        private List<DeskBandMenuAction> _audioSourceContextMenuItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainControl"/> class.
        /// Entry point.
        /// </summary>
        /// <param name="options">The deskband options.</param>
        /// <param name="info">The taskbar info.</param>
        /// <param name="track">The track model.</param>
        /// <param name="appsettings">The app settings</param>
        /// <param name="audiosourceMananger">The audio source manager</param>
        /// <param name="settingsWindow">The settings window.</param>
        /// <param name="labelService">The label service.</param>
        public MainControl(
            CSDeskBandOptions options,
            TaskbarInfo info,
            Track track,
            IAppSettings appsettings,
            IAudioSourceManager audiosourceMananger,
            ISettingsWindow settingsWindow,
            ICustomLabelService labelService)
        {
            InitializeComponent(); System.Diagnostics.Debugger.Launch();

            _uiDispatcher = Dispatcher.CurrentDispatcher;
            Options = options;
            TaskbarInfo = info;
            _appSettings = appsettings;
            _audioSourceManager = audiosourceMananger;
            _track = track;
            _settingsWindow = settingsWindow;
            _labelService = labelService;

#pragma warning disable CS4014
            Task.Run(InitializeAsync);
#pragma warning restore CS4014
        }

        /// <summary>
        /// Gets the deskband options.
        /// </summary>
        public CSDeskBandOptions Options { get; }

        /// <summary>
        /// Gets the taskbar info.
        /// </summary>
        public TaskbarInfo TaskbarInfo { get; }

        /// <summary>
        /// Save on close
        /// </summary>
        public void CloseAudioband()
        {
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

        /// <summary>
        /// Update deskband options when the size changes
        /// </summary>
        /// <param name="eventArgs">.</param>
        protected override void OnResize(EventArgs eventArgs)
        {
            if (_appSettings?.AudioBand == null)
            {
                return;
            }

            // use model size because we want to only chnage when it changes instead of catching all resize events
            var audioBandSize = GetScaledSize(new Size(_appSettings.AudioBand.Width, _appSettings.AudioBand.Height));
            Options.MinHorizontalSize = audioBandSize;
            Options.HorizontalSize = audioBandSize;
            Options.MaxHorizontalHeight = audioBandSize.Height;
        }

        private async Task InitializeAsync()
        {
            try
            {
                Logger.Debug("Initialization started");

                _audioSourceContextMenuItems = new List<DeskBandMenuAction>();
                _settingsMenuItem = new DeskBandMenuAction("Audio Band Settings");
                _settingsMenuItem.Clicked += SettingsMenuItemOnClicked;
                RefreshContextMenu();

                _labelService.CustomLabelAdded += LabelServiceOnAddCustomTextLabel;
                _labelService.CustomLabelRemoved += LabelServiceOnRemoveCustomTextLabel;
                foreach (var label in _labelService.Labels)
                {
                    LabelServiceOnAddCustomTextLabel(null, label);
                }

                await _uiDispatcher.InvokeAsync(() => InitializeBindingSources(
                    _settingsWindow.AlbumArtPopupVM,
                    _settingsWindow.AlbumArtVM,
                    _settingsWindow.AudioBandVM,
                    _settingsWindow.NextButtonVM,
                    _settingsWindow.PlayPauseButtonVM,
                    _settingsWindow.PreviousButtonVM,
                    _settingsWindow.ProgressBarVM));

                _settingsWindow.Saved += SettingsWindowOnSaved;

                _audioSourceManager.AudioSources.CollectionChanged += AudioSourcesOnCollectionChanged;
                _audioSourceManager.LoadAudioSources();

                Logger.Debug("Initialization complete");
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error during initialization");
            }
        }

        private void OpenSettingsWindow()
        {
            _settingsWindow.ShowWindow();
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
                Logger.Warn("Tried subscribing to audiosource but it was null");
                return;
            }

            ResetTrack();

            source.TrackInfoChanged += AudioSourceOnTrackInfoChanged;
            source.IsPlayingChanged += AudioSourceOnIsPlayingChanged;
            source.TrackProgressChanged += AudioSourceOnTrackProgressChanged;

            Logger.Debug("Activating audio source {name}", source.Name);

            await source.ActivateAsync().ConfigureAwait(false);
            _appSettings.AudioSource = source.Name;

            Logger.Debug("Audio source {name} was activated", source.Name);
        }

        private async Task UnsubscribeToAudioSource(IAudioSource source)
        {
            if (source == null)
            {
                Logger.Warn("Tried unsubscribing to audio source but it was null");
                return;
            }

            source.TrackInfoChanged -= AudioSourceOnTrackInfoChanged;
            source.IsPlayingChanged -= AudioSourceOnIsPlayingChanged;
            source.TrackProgressChanged -= AudioSourceOnTrackProgressChanged;

            Logger.Debug("Deactivating audio source {name}", source.Name);

            await source.DeactivateAsync().ConfigureAwait(false);

            _appSettings.AudioSource = null;
            _currentAudioSource = null;

            ResetTrack();

            Logger.Debug("Audio source {name} deactivated", source.Name);
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
                    var menuItemsText = _audioSourceContextMenuItems.Select(m => m.Text);
                    var sources = _audioSourceManager.AudioSources.Select(s => s.Name);
                    Logger.Warn("Contenxt menu item {menuName} had no matching audiosource. Context menu items: {@items}. Audiosources: {@audiosources}", menuItem.Text, menuItemsText, sources);
                    return;
                }

                await SubscribeToAudioSource(_currentAudioSource).ConfigureAwait(false);
                menuItem.Checked = true;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error changing audio source. Current: {current}. Menu item: {menu}", _currentAudioSource?.Name, menuItem.Text);
                _currentAudioSource = null;
            }
        }

        private void ResetTrack()
        {
            _track.AlbumArt = null;
            _track.AlbumName = null;
            _track.Artist = null;
            _track.TrackName = null;
            _track.IsPlaying = false;
            _track.TrackLength = TimeSpan.Zero;
            _track.TrackProgress = TimeSpan.Zero;
        }
    }
}
