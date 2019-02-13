using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using NLog;
using NLog.Config;

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

        public AudioSourceWrapper()
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            LogManager.Configuration = new XmlLoggingConfiguration(Path.Combine(basePath, "nlog.config"));
        }

        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        public event EventHandler<bool> IsPlayingChanged;

        public event EventHandler<TimeSpan> TrackProgressChanged;

        public event EventHandler<float> VolumeChanged;

        public event EventHandler<bool> ShuffleChanged;

        public event EventHandler<RepeatMode> RepeatModeChanged;

        public string Name => _audioSource.Name;

        public List<AudioSourceSettingAttribute> Settings => _audioSourceSettingsList.Select(s => s.Attribute).ToList();

        public void Activate(MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.ActivateAsync, tcs);
        }

        public void Deactivate(MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.DeactivateAsync, tcs);
        }

        public void NextTrack(MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.NextTrackAsync, tcs);
        }

        public void PauseTrack(MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.PauseTrackAsync, tcs);
        }

        public void PlayTrack(MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.PlayTrackAsync, tcs);
        }

        public void PreviousTrack(MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.PreviousTrackAsync, tcs);
        }

        public void SetVolume(float newVolume, MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.SetVolumeAsync, newVolume, tcs);
        }

        public void SetPlayback(TimeSpan newProgress, MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.SetPlaybackProgressAsync, newProgress, tcs);
        }

        public void SetShuffle(bool shuffle, MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.SetShuffleAsync, shuffle, tcs);
        }

        public void SetRepeatMode(RepeatMode repeatMode, MarshaledTaskCompletionSource tcs)
        {
            StartTask(_audioSource.SetRepeatModeAsync, repeatMode, tcs);
        }

        public bool Initialize(string audioSourceDirectory)
        {
            try
            {
                _logger = LogManager.GetLogger($"AudioSourceWrapper({new DirectoryInfo(audioSourceDirectory).Name})");
                _logger.Debug("Initializing wrapper");
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

        public Type GetSettingType(string settingName)
        {
            return _audioSourceSettings[settingName].SettingType;
        }

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
