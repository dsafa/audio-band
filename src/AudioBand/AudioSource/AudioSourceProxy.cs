using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using AudioBand.ServiceContracts;
using NLog;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Proxy class for an IAudioSource in another process.
    /// </summary>
    internal class AudioSourceProxy : IInternalAudioSource
    {
        private readonly ILogger _logger;
        private readonly object _isClosingLock = new object();
        private readonly TaskCompletionSource<bool> _initializationCompletionSource;
        private readonly string _directory;

        private readonly Dictionary<string, object> _settingsCache = new Dictionary<string, object>();
        private IAudioSourceHostService _hostService;
        private bool _hasInitializedOnce;

        private bool _isClosing;
        private bool _isActivated;

        private AudioSourceProxy(string directory, TaskCompletionSource<bool> initializationCompletionSource, IAudioSourceHostService hostService)
        {
            _initializationCompletionSource = initializationCompletionSource;
            _directory = directory;
            _logger = LogManager.GetLogger($"AudioSourceProxy({new DirectoryInfo(directory).Name})");

            _hostService = hostService;
            hostService.HostCallback.SettingChanged += (o, e) => SettingChanged?.Invoke(this, e);
            hostService.HostCallback.TrackInfoChanged += (o, e) => TrackInfoChanged?.Invoke(this, e);
            hostService.HostCallback.TrackPlaying += (o, e) => TrackPlaying?.Invoke(this, e);
            hostService.HostCallback.TrackPaused += (o, e) => TrackPaused?.Invoke(this, e);
            hostService.HostCallback.TrackProgressChanged += (o, e) => TrackProgressChanged?.Invoke(this, e);
            hostService.Restarted += HostServiceRestarted;
        }

        /// <inheritdoc/>
        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        /// <inheritdoc/>
        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        /// <inheritdoc/>
        public event EventHandler TrackPlaying;

        /// <inheritdoc/>
        public event EventHandler TrackPaused;

        /// <inheritdoc/>
        public event EventHandler<TimeSpan> TrackProgressChanged;

        /// <inheritdoc/>
        public string Name
        {
            get
            {
                try
                {
                    return _hostService.GetHost().GetName();
                }
                catch (Exception e)
                {
                    HandleError(e);
                    throw;
                }
            }
        }

        /// <inheritdoc/>
        public IAudioSourceLogger Logger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets the settings that the audio source has.
        /// </summary>
        public List<AudioSourceSettingAttribute> Settings { get; private set; }

        private bool IsClosing
        {
            get
            {
                lock (_isClosingLock)
                {
                    return _isClosing;
                }
            }

            set
            {
                lock (_isClosingLock)
                {
                    _isClosing = value;
                }
            }
        }

        /// <summary>
        /// Get or set a setting.
        /// </summary>
        /// <param name="settingName">Name of the setting</param>
        /// <returns>The setting value.</returns>
        public object this[string settingName]
        {
            get
            {
                if (IsClosing)
                {
                    return null;
                }

                try
                {
                    var settingValue = _hostService.GetHost().GetSettingValue(settingName);
                    _settingsCache[settingName] = settingValue;
                    return settingValue;
                }
                catch (Exception e)
                {
                    HandleError(e);
                    return null;
                }
            }

            set
            {
                if (IsClosing)
                {
                    return;
                }

                try
                {
                    _settingsCache[settingName] = value;
                    _hostService.GetHost().UpdateSetting(settingName, value);
                }
                catch (Exception e)
                {
                    HandleError(e);
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="AudioSourceProxy"/> for the given directory.
        /// </summary>
        /// <param name="directory">Directory containing the audio source.</param>
        /// <param name="hostService">The host service used to access a host.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation that creates an <see cref="AudioSourceProxy"/>.</returns>
        public static async Task<AudioSourceProxy> CreateProxy(string directory, IAudioSourceHostService hostService)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            var proxy = new AudioSourceProxy(directory, taskCompletionSource, hostService);

            if (await taskCompletionSource.Task)
            {
                return proxy;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Try to shut down gracefully.
        /// </summary>
        public void Close()
        {
            IsClosing = true;
        }

        /// <inheritdoc/>
        public async Task ActivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsClosing)
            {
                return;
            }

            try
            {
                await _hostService.GetHost().ActivateAsync().ConfigureAwait(false);
                _isActivated = true;
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        /// <inheritdoc/>
        public async Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsClosing)
            {
                return;
            }

            try
            {
                await _hostService.GetHost().DeactivateAsync().ConfigureAwait(false);
                _isActivated = false;
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        /// <inheritdoc/>
        public async Task NextTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsClosing)
            {
                return;
            }

            try
            {
                await _hostService.GetHost().NextTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        /// <inheritdoc/>
        public async Task PauseTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsClosing)
            {
                return;
            }

            try
            {
                await _hostService.GetHost().PauseTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        /// <inheritdoc/>
        public async Task PlayTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsClosing)
            {
                return;
            }

            try
            {
                await _hostService.GetHost().PlayTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        /// <inheritdoc/>
        public async Task PreviousTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsClosing)
            {
                return;
            }

            try
            {
                await _hostService.GetHost().PreviousTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        private void HandleError(Exception e)
        {
            if (e is CommunicationObjectFaultedException || e is CommunicationObjectAbortedException)
            {
                _logger.Debug("Communication already closed");
            }
            else
            {
                _logger.Error(e);
            }

            // if this is the first time the host is registered, send exit signal.
            if (!_hasInitializedOnce)
            {
                _initializationCompletionSource.SetResult(false);
            }

            _hostService.Restart();
        }

        private async void HostServiceRestarted(object sender, EventArgs e)
        {
            UpdateSettings();

            // if this is the first time the the service was started
            if (!_hasInitializedOnce)
            {
                _hasInitializedOnce = true;
                _initializationCompletionSource.SetResult(true);
            }

            // re-activate if the host was restarted
            if (_isActivated)
            {
                await ActivateAsync();
            }
        }

        private void UpdateSettings()
        {
            IAudioSourceHost host = _hostService.GetHost();

            Settings = host.GetAudioSourceSettings().Select(s => (AudioSourceSettingAttribute)s).ToList();

            // apply the settings again, if host restarted, the cache should be populated
            foreach (var keyVal in _settingsCache)
            {
                host.UpdateSetting(keyVal.Key, keyVal.Value);
            }
        }
    }
}
