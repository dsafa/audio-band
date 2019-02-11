using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AudioSourceHost;
using NLog;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Proxy class for an IAudioSource in another process.
    /// </summary>
    internal class AudioSourceProxy : IInternalAudioSource
    {
        private readonly ILogger _logger;
        private readonly object _isActivatedLock = new object();
        private readonly Dictionary<string, object> _settingsCache = new Dictionary<string, object>();
        private readonly string _directory;
        private bool _isActivated;
        private AppDomain _appDomain;
        private AudioSourceWrapper _wrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceProxy"/> class
        /// with the directory and the host service.
        /// </summary>
        /// <param name="directory">The audio source directory.</param>
        public AudioSourceProxy(string directory)
        {
            _directory = directory;
            var directoryName = new DirectoryInfo(directory).Name;
            _logger = LogManager.GetLogger($"AudioSourceProxy({directoryName})");

            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;

            var domainSetupInfo = new AppDomainSetup
            {
                ApplicationName = $"AudioSourceHost({directoryName})",
                ApplicationBase = DirectoryHelper.BaseDirectory,
            };

            _appDomain = AppDomain.CreateDomain(directoryName, null, domainSetupInfo);
            CreateWrapper();
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
        public event EventHandler<float> VolumeChanged;

        /// <inheritdoc/>
        public string Name
        {
            get
            {
                try
                {
                    return _wrapper.Name;
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Error trying to get the name");
                    throw;
                }
            }
        }

        /// <inheritdoc/>
        public IAudioSourceLogger Logger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets the settings that the audio source has.
        /// </summary>
        public List<AudioSourceSettingAttribute> Settings { get; private set; } = new List<AudioSourceSettingAttribute>();

        private bool IsActivated
        {
            get
            {
                lock (_isActivatedLock)
                {
                    return _isActivated;
                }
            }

            set
            {
                lock (_isActivatedLock)
                {
                    _isActivated = value;
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
                try
                {
                    var settingValue = _wrapper.GetSettingValue(settingName);
                    _settingsCache[settingName] = settingValue;
                    return settingValue;
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Error trying to get setting.");
                    throw;
                }
            }

            set
            {
                try
                {
                    _settingsCache[settingName] = value;
                    _wrapper.UpdateSetting(settingName, value);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Error tring to update setting.");
                    throw;
                }
            }
        }

        /// <inheritdoc/>
        public async Task ActivateAsync()
        {
            Debug.Assert(!IsActivated, "Audio source already activated");
            if (await CallWrapperAsync(_wrapper.Activate))
            {
                IsActivated = true;
            }
            else
            {
                throw new InvalidOperationException("Unable to activate audiosource");
            }
        }

        /// <inheritdoc/>
        public async Task DeactivateAsync()
        {
            Debug.Assert(IsActivated, "Audio source is not activated");
            await CallWrapperAsync(_wrapper.Deactivate);
            IsActivated = false;
        }

        /// <inheritdoc/>
        public async Task NextTrackAsync()
        {
            if (!IsActivated)
            {
                return;
            }

            await CallWrapperAsync(_wrapper.NextTrack);
        }

        /// <inheritdoc/>
        public async Task PauseTrackAsync()
        {
            if (!IsActivated)
            {
                return;
            }

            await CallWrapperAsync(_wrapper.PauseTrack);
        }

        /// <inheritdoc/>
        public async Task PlayTrackAsync()
        {
            if (!IsActivated)
            {
                return;
            }

            await CallWrapperAsync(_wrapper.PlayTrack);
        }

        /// <inheritdoc/>
        public async Task PreviousTrackAsync()
        {
            if (!IsActivated)
            {
                return;
            }

            await CallWrapperAsync(_wrapper.PreviousTrack);
        }

        /// <inheritdoc/>
        public async Task SetVolumeAsync(float newVolume)
        {
            if (!IsActivated)
            {
                return;
            }

            await CallWrapperAsync(_wrapper.SetVolume, newVolume);
        }

        /// <inheritdoc/>
        public async Task SetPlaybackProgressAsync(TimeSpan newProgress)
        {
            if (!IsActivated)
            {
                return;
            }

            await CallWrapperAsync(_wrapper.SetPlayback, newProgress);
        }

        /// <inheritdoc/>
        public Type GetSettingType(string settingName)
        {
            return _wrapper.GetSettingType(settingName);
        }

        // Use load from context
        private static Assembly AssemblyResolve(object sender, ResolveEventArgs e)
        {
            string shortName = e.Name.Substring(0, e.Name.IndexOf(','));
            string fileName = Path.Combine(DirectoryHelper.BaseDirectory, shortName + ".dll");
            return File.Exists(fileName) ? Assembly.LoadFrom(fileName) : null;
        }

        private async Task Restart()
        {
            // Assuming this won't throw because it was created previously without errors
            CreateWrapper();

            // re-activate if the host was restarted
            if (IsActivated)
            {
                // If activation fails then don't auto restart otherwise it will be a loop
                await CallWrapperAsync(_wrapper.Activate, autoRestart: false);
            }
        }

        private void LoadAudioSourceSettings()
        {
            try
            {
                Settings = _wrapper.Settings;

                // apply the settings again, if host restarted, the cache should be populated
                foreach (var keyVal in _settingsCache)
                {
                    _wrapper.UpdateSetting(keyVal.Key, keyVal.Value);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error loading settings");
            }
        }

        private void CreateWrapper()
        {
            var wrapperType = typeof(AudioSourceWrapper);
            var dllPath = Path.Combine(DirectoryHelper.BaseDirectory, wrapperType.Assembly.GetName().Name + ".dll");
            _wrapper = (AudioSourceWrapper)_appDomain.CreateInstanceFromAndUnwrap(dllPath, wrapperType.FullName);

            if (!_wrapper.Initialize(_directory))
            {
                throw new InvalidOperationException("Unable to initialize wrapper");
            }

            _wrapper.SettingChanged += new MarshaledEventHandler<SettingChangedEventArgs>(e => SettingChanged?.Invoke(this, e)).Handler;
            _wrapper.TrackInfoChanged += new MarshaledEventHandler<TrackInfoChangedEventArgs>(e => TrackInfoChanged?.Invoke(this, e)).Handler;
            _wrapper.TrackPaused += new MarshaledEventHandler(() => TrackPaused?.Invoke(this, EventArgs.Empty)).Handler;
            _wrapper.TrackPlaying += new MarshaledEventHandler(() => TrackPlaying?.Invoke(this, EventArgs.Empty)).Handler;
            _wrapper.TrackProgressChanged += new MarshaledEventHandler<TimeSpan>(e => TrackProgressChanged?.Invoke(this, e)).Handler;
            _wrapper.VolumeChanged += new MarshaledEventHandler<float>(e => VolumeChanged?.Invoke(this, e)).Handler;

            LoadAudioSourceSettings();
        }

        private async Task<bool> CallWrapperAsync(Action<MarshaledTaskCompletionSource> wrapperAction, bool autoRestart = true)
        {
            try
            {
                var tcs = new MarshaledTaskCompletionSource();
                wrapperAction(tcs);

                await tcs.Task.ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Unexpected error when calling the wrapper, recreating wrapper");
                if (autoRestart)
                {
                    await Restart();
                }

                return false;
            }
        }

        private async Task<bool> CallWrapperAsync<TArg>(Action<TArg, MarshaledTaskCompletionSource> wrapperAction, TArg arg, bool autoRestart = true)
        {
            return await CallWrapperAsync((tcs) => wrapperAction(arg, tcs), autoRestart);
        }
    }
}
