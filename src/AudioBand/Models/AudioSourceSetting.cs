using System;
using System.ComponentModel;
using AudioBand.AudioSource;

namespace AudioBand.Models
{
    /// <summary>
    /// A key / value pair that represents a single setting for an audio source
    /// </summary>
    internal class AudioSourceSetting : ModelBase
    {
        private string _name;
        private object _value;

        /// <summary>
        /// Name of the setting
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Value of the setting serialized as a string
        /// </summary>
        public object Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        /// <summary>
        /// Updates the audio source with the new value.
        /// </summary>
        /// <param name="settingInfo">Setting info for the audio source.</param>
        /// <param name="value">New value.</param>
        public void UpdateAudioSource(AudioSourceSettingInfo settingInfo, object value)
        {
            try
            {
                var newValue = ConvertToType(settingInfo, value);
                settingInfo.Property.SetMethod.Invoke(settingInfo.Source, new[] { newValue });
            }
            catch (Exception e)
            {
                Logger.Error($"Error occured while change audio source settings. info: {settingInfo}, value: {value}, Exception: {e}");
                throw new Exception("An unexpected error occured. Check the log for more details.");
            }
        }

        public SettingValidationResult Validate(AudioSourceSettingInfo settingInfo, object value)
        {
            var audioSource = settingInfo.Source;
            var attribute = settingInfo.Attribute;
            var property = settingInfo.Property;

            return attribute.Validate(audioSource, value, property.Name);
        }

        private object ConvertToType(AudioSourceSettingInfo settingInfo, object value)
        {
            if (value != null && value.GetType() != settingInfo.PropertyType)
            {
                var converter = TypeDescriptor.GetConverter(value.GetType());
                if (converter.CanConvertTo(settingInfo.PropertyType))
                {
                    return converter.ConvertTo(value, settingInfo.PropertyType);
                }

                Logger.Warn($"Trying to set invalid type for setting {settingInfo} - {value} {value.GetType()}");
                throw new ArgumentException();
            }

            return value;
        }
    }
}