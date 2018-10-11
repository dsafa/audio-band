using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AudioBand.Annotations;
using AudioBand.AudioSource;
using AudioBand.Models;
using NLog;

namespace AudioBand.ViewModels
{
    internal class AudioSourceSettingsCollectionViewModel : INotifyPropertyChanged
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private Dictionary<string, AudioSourceSettingViewModel> _selectedSettings = new Dictionary<string, AudioSourceSettingViewModel>();

        // audio source -> (setting name -> viewmodel)
        public Dictionary<string, Dictionary<string, AudioSourceSettingViewModel>> Settings { get; } = new Dictionary<string, Dictionary<string, AudioSourceSettingViewModel>>();

        public Dictionary<string, AudioSourceSettingViewModel> SelectedSettings
        {
            get => _selectedSettings;
            set
            {
                if (Equals(value, _selectedSettings)) return;
                _selectedSettings = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AudioSourceSettingsCollectionViewModel(List<IAudioSource> audioSources, List<AudioSourceSettingsCollection> audioSourceSettings)
        {
            Merge(audioSources, audioSourceSettings);
        }

        /// <summary>
        /// Merge to update the audiosources values with ones that are saved
        /// </summary>
        private void Merge(List<IAudioSource> audioSources, List<AudioSourceSettingsCollection> savedSettings)
        {
            foreach (var audioSource in audioSources)
            {
                var settings = GetExportedSettings(audioSource);
                Settings.Add(audioSource.Name, settings);
            }

            foreach (var savedSetting in savedSettings)
            {
                if (Settings.TryGetValue(savedSetting.Name, out var settings))
                {
                   savedSetting.Settings.ForEach(s =>
                   {
                       if (settings.ContainsKey(s.Name))
                       {
                           settings[s.Name].Value = s.Value;
                       }
                       else
                       {
                           _logger.Warn($"Setting {s.Name} for audio source {savedSetting.Name} found but no matching setting found in the audio source");
                       }
                   });
                }
                else
                {
                    _logger.Warn($"Setting for audio source {savedSetting.Name} found but not matching audio source is found");
                }
            }
        }

        /// <summary>
        /// Look for properties marked with the setting attribute
        /// </summary>
        private Dictionary<string, AudioSourceSettingViewModel> GetExportedSettings(IAudioSource audioSource)
        {
            return audioSource.GetType().GetProperties()
                .Select(property => new {Property = property, Attributes = property.GetCustomAttributes(typeof(AudioSourceSettingAttribute), true)})
                .Where(p => p.Attributes.Length == 1)
                .Select(p =>
                {
                    var attr = (AudioSourceSettingAttribute) p.Attributes[0];
                    var s = new AudioSourceSetting() {Name = attr.Name};
                    return new AudioSourceSettingViewModel(audioSource, p.Property, s);
                })
                .ToDictionary(s => s.Name, s => s);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Holds additional information about the original source and property
    internal class AudioSourceSettingViewModel : INotifyPropertyChanged
    {
        private readonly AudioSourceSetting _audioSourceSetting;
        private readonly PropertyInfo _propertyInfo;
        private readonly IAudioSource _audioSource;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public string Name
        {
            get => _audioSourceSetting.Name;
            set
            {
                _audioSourceSetting.Name = value;
                OnPropertyChanged();
            }
        }

        public object Value
        {
            get => _audioSourceSetting.Value;
            set
            {
                // Check if value is valid
                if (!_audioSource.ValidateSettingChange(Name, value))
                {
                    return;
                }

                // Update the value in the audio source
                _audioSourceSetting.Value = value;

                try
                {
                    _propertyInfo.GetSetMethod(true)?.Invoke(_audioSource, new[] {value});
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }

                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AudioSourceSettingViewModel(IAudioSource source, PropertyInfo propertyInfo, AudioSourceSetting setting)
        {
            _audioSource = source;
            _propertyInfo = propertyInfo;
            _audioSourceSetting = setting;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
