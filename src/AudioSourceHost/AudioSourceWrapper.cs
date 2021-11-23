using AudioBand.AudioSource;
using AudioBand.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AudioSourceHost
{
    /// <summary>
    /// Wrapper that isolates an audiosource.
    /// </summary>
    public class AudioSourceWrapper : MarshalByRefObject
    {
        private readonly Dictionary<string, AudioSourceSetting> _audioSourceSettings = new Dictionary<string, AudioSourceSetting>();
        private ILogger _logger;
        private IAudioSource _audioSource;
        private List<AudioSourceSetting> _audioSourceSettingsList; // so we can keep the order of the settings.

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceWrapper"/> class.
        /// </summary>
        public AudioSourceWrapper()
        {
            AudioBandLogManager.Initialize();
        }

        /// <summary>
        /// Wrapper for <see cref="IAudioSource.SettingChanged"/>.
        /// </summary>
        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        /// <summary>
        /// Wrapper for <see cref="IAudioSource.TrackInfoChanged"/>.
        /// </summary>
        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        /// <summary>
        /// Wrapper for <see cref="IAudioSource.IsPlayingChanged"/>.
        /// </summary>
        public event EventHandler<bool> IsPlayingChanged;

        /// <summary>
        /// Wrapper for <see cref="IAudioSource.TrackProgressChanged"/>.
        /// </summary>
        public event EventHandler<TimeSpan> TrackProgressChanged;

        /// <summary>
        /// Wrapper for <see cref="IAudioSource.VolumeChanged"/>.
        /// </summary>
        public event EventHandler<int> VolumeChanged;

        /// <summary>
        /// Wrapper for <see cref="IAudioSource.ShuffleChanged"/>.
        /// </summary>
        public event EventHandler<bool> ShuffleChanged;

        /// <summary>
        /// Wrapper for <see cref="IAudioSource.RepeatModeChanged"/>.
        /// </summary>
        public event EventHandler<RepeatMode> RepeatModeChanged;

        /// <summary>
        /// Gets the name of the audio source.
        /// </summary>
        public string Name => _audioSource.Name;

        /// <summary>
        /// Gets the description of the audio source.
        /// </summary>
        public string Description => _audioSource.Description;

        /// <summary>
        /// Gets the Window Class Name of the audio source.
        /// </summary>
        public string WindowClassName => _audioSource.WindowClassName;

        /// <summary>
        /// Gets the list of audio source settings.
        /// </summary>
        public List<AudioSourceSettingAttribute> Settings => _audioSourceSettingsList.Select(s => s.Attribute).ToList();

        /// <summary>
        /// Activates the audio source.
        /// </summary>
        /// <param name="tcs">The task completion source.</param>
        public void Activate(MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.ActivateAsync, tcs);
        }

        /// <summary>
        /// Deactivates the audio source.
        /// </summary>
        /// <param name="tcs">The task completion source.</param>
        public void Deactivate(MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.DeactivateAsync, tcs);
        }

        /// <summary>
        /// Seeks to next track.
        /// </summary>
        /// <param name="tcs">The task completion source.</param>
        public void NextTrack(MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.NextTrackAsync, tcs);
        }

        /// <summary>
        /// Pauses the track.
        /// </summary>
        /// <param name="tcs">The task completion source.</param>
        public void PauseTrack(MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.PauseTrackAsync, tcs);
        }

        /// <summary>
        /// Plays the track.
        /// </summary>
        /// <param name="tcs">The task completion source.</param>
        public void PlayTrack(MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.PlayTrackAsync, tcs);
        }

        /// <summary>
        /// Seeks to the previous track.
        /// </summary>
        /// <param name="tcs">The task completion source.</param>
        public void PreviousTrack(MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.PreviousTrackAsync, tcs);
        }

        /// <summary>
        /// Sets the volume.
        /// </summary>
        /// <param name="newVolume">The new volume.</param>
        /// <param name="tcs">The task completion source.</param>
        public void SetVolume(int newVolume, MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.SetVolumeAsync, newVolume, tcs);
        }

        /// <summary>
        /// Sets the playback progress.
        /// </summary>
        /// <param name="newProgress">The new progress.</param>
        /// <param name="tcs">The task completion source.</param>
        public void SetPlayback(TimeSpan newProgress, MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.SetPlaybackProgressAsync, newProgress, tcs);
        }

        /// <summary>
        /// Sets the shuffle mode.
        /// </summary>
        /// <param name="shuffle">The shuffle.</param>
        /// <param name="tcs">The task completion source.</param>
        public void SetShuffle(bool shuffle, MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.SetShuffleAsync, shuffle, tcs);
        }

        /// <summary>
        /// Sets the repeat mode.
        /// </summary>
        /// <param name="repeatMode">The repeat mode.</param>
        /// <param name="tcs">The task completion source.</param>
        public void SetRepeatMode(RepeatMode repeatMode, MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.SetRepeatModeAsync, repeatMode, tcs);
        }

        /// <summary>
        /// Initializes the wrapper.
        /// </summary>
        /// <param name="audioSourceDirectory">The directory of the audio source.</param>
        /// <returns>True if successful.</returns>
        public bool Initialize(string audioSourceDirectory)
        {
            try
            {
                _logger = AudioBandLogManager.GetLogger($"AudioSourceWrapper({new DirectoryInfo(audioSourceDirectory).Name})");
                _logger.Debug("Initializing wrapper");

                AppDomain.CurrentDomain.UnhandledException += (o, e) => _logger.Error(e.ExceptionObject as Exception, "Unhandled exception in wrapper");

                _audioSource = AudioSourceLoader.LoadFromDirectory(audioSourceDirectory);
                _audioSource.Logger = new AudioSourceLogger(_audioSource.Name);

                _audioSource.SettingChanged += (o, e) => SettingChanged?.Invoke(this, e);
                _audioSource.TrackInfoChanged += (o, e) => TrackInfoChanged?.Invoke(this, e);
                _audioSource.IsPlayingChanged += (o, e) => IsPlayingChanged?.Invoke(this, e);
                _audioSource.TrackProgressChanged += (o, e) => TrackProgressChanged?.Invoke(this, e);
                _audioSource.VolumeChanged += (o, e) => VolumeChanged?.Invoke(this, e);
                _audioSource.ShuffleChanged += (o, e) => ShuffleChanged?.Invoke(this, e);
                _audioSource.RepeatModeChanged += (o, e) => RepeatModeChanged?.Invoke(this, e);

                _audioSourceSettingsList = _audioSource.GetSettings();
                foreach (AudioSourceSetting setting in _audioSourceSettingsList)
                {
                    _audioSourceSettings.Add(setting.Attribute.Name, setting);
                }

                _logger.Debug("Wrapper initialization complete");
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }
        }

        /// <summary>
        /// Updates an audio source setting.
        /// </summary>
        /// <param name="settingName">The name of the setting.</param>
        /// <param name="value">The value.</param>
        public void UpdateSetting(string settingName, object value)
        {
            try
            {
                _audioSourceSettings[settingName].SettingValue = value;
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }

        /// <summary>
        /// Gets the value of an audio source setting.
        /// </summary>
        /// <param name="settingName">The setting name.</param>
        /// <returns>The value of the setting.</returns>
        public object GetSettingValue(string settingName)
        {
            try
            {
                return _audioSourceSettings[settingName].SettingValue;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return null;
            }
        }

        /// <summary>
        /// Gets the type of a setting.
        /// </summary>
        /// <param name="settingName">The setting name.</param>
        /// <returns>The type of the setting.</returns>
        public Type GetSettingType(string settingName)
        {
            return _audioSourceSettings[settingName].SettingType;
        }

        /// <inheritdoc />
        /// Prevent gc.
        public override object InitializeLifetimeService()
        {
            return null;
        }

        private void StartTask(Func<Task> action, MarshaledTaskCompletionSource tcs)
        {
            try
            {
                SetupTask(action(), tcs);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }

        private void StartTask<TArg>(Func<TArg, Task> action, TArg arg, MarshaledTaskCompletionSource tcs)
        {
            try
            {
                SetupTask(action(arg), tcs);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }

        private void SetupTask(Task task, MarshaledTaskCompletionSource tcs)
    {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    tcs.SetException(t.Exception.InnerException);
                }
                else
                {
                    tcs.SetResult();
                }
            }, TaskScheduler.Default);
    }
    }
}
