﻿using AudioBand.Annotations;
using AudioBand.AudioSource;
using AudioBand.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AudioBand.ViewModels
{
    internal class AudioSourceSettingsCollectionViewModel : INotifyPropertyChanged
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private Dictionary<string, AudioSourceSettingViewModel> _selectedSettings = new Dictionary<string, AudioSourceSettingViewModel>();

        /// <summary>
        /// Mapping of settings of audiosource -> collection of (setting name, setting)
        /// </summary>
        public Dictionary<string, Dictionary<string, AudioSourceSettingViewModel>> Settings { get; } = new Dictionary<string, Dictionary<string, AudioSourceSettingViewModel>>();

        /// <summary>
        /// The currently selected collection of audio source settings
        /// </summary>
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
            CreateSettings(audioSources, audioSourceSettings);
        }

        public List<AudioSourceSettingsCollection> ToModel()
        {
            return Settings.Select(s => new AudioSourceSettingsCollection
                {
                    Name = s.Key,
                    Settings = s.Value.Values.Select(x => x.ToModel()).ToList()
                }).ToList();
        }

        private void CreateSettings(List<IAudioSource> audioSources, List<AudioSourceSettingsCollection> savedSettings)
        {
            foreach (var audioSource in audioSources)
            {
                var settings = GetExportedSettings(audioSource, savedSettings);
                if (settings.Count == 0)
                {
                    continue;
                }

                Settings.Add(audioSource.Name, settings);
            }
        }

        /// <summary>
        /// Look for properties marked with the setting attribute
        /// </summary>
        private Dictionary<string, AudioSourceSettingViewModel> GetExportedSettings(IAudioSource audioSource, List<AudioSourceSettingsCollection> savedSettings)
        {
            var saved = savedSettings.ToDictionary(s => s.Name, s => s.Settings.ToDictionary(s1 => s1.Name, s1 => s1));

            return audioSource.GetType().GetProperties()
                .Select(property => new {Property = property, Attributes = property.GetCustomAttributes(typeof(AudioSourceSettingAttribute), true)})
                .Where(p => p.Attributes.Length == 1)
                .Select(p =>
                {
                        try
                        {
                            // Merge settings with those that are saved
                            var attr = (AudioSourceSettingAttribute) p.Attributes[0];
                            var type = p.Property.PropertyType;

                            var value = saved.ContainsKey(audioSource.Name) && saved[audioSource.Name].ContainsKey(attr.Name)
                                ? saved[audioSource.Name][attr.Name].Value 
                                : p.Property.GetGetMethod(true).Invoke(audioSource, null)?.ToString();

                            return new AudioSourceSettingViewModel(audioSource, p.Property, type, value, attr.Name);
                        }
                        catch (MissingMethodException)
                        {
                            _logger.Error($"Cannot create setting of type {p.Property.PropertyType}");
                            return null;
                        }
                })
                .Where(s => s != null)
                .ToDictionary(s => s.Name, s => s);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class AudioSourceSettingViewModel : INotifyPropertyChanged
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly IAudioSource _audioSource;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly Type _valueType;
        private object _value;

        /// <summary>
        /// Setting name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Value of the setting, can be any basic type
        /// </summary>
        public object Value
        {
            get => _value;
            set
            {
                if (Equals(value, _value))
                {
                    return;
                }

                if (value?.GetType() != _valueType)
                {
                    var converter = TypeDescriptor.GetConverter(value.GetType());
                    if (converter.CanConvertTo(_valueType))
                    {
                        value = converter.ConvertTo(value, _valueType);
                    }
                    else
                    {
                        _logger.Warn($"Trying to set invalid type for setting {Name} - {value} {value?.GetType()}");
                        return;
                    }
                }

                _value = value;

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

        public AudioSourceSettingViewModel(IAudioSource source, PropertyInfo propertyInfo, Type valueType, string valueString, string settingName)
        {
            Name = settingName;
            _valueType = valueType;
            _audioSource = source;
            _propertyInfo = propertyInfo;

            try
            {
                _value = TypeDescriptor.GetConverter(valueType).ConvertFromString(valueString);
            }
            catch (Exception e)
            {
                _logger.Error($"Setting value does not match the type exported: {e}");
            }
        }

        public AudioSourceSetting ToModel()
        {
            return new AudioSourceSetting(Name, _value.ToString());
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
