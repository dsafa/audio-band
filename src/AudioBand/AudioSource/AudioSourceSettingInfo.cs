using System;
using System.ComponentModel;
using System.Reflection;
using NLog;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Contains information for an audio source setting
    /// </summary>
    internal class AudioSourceSettingInfo
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Audio source that this setting belongs to
        /// </summary>
        public IAudioSource Source { get; }

        /// <summary>
        /// Property that the <see cref="Attribute"/> is applied to.
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        /// <see cref="AudioSourceSettingAttribute"/> attached to the property.
        /// </summary>
        public AudioSourceSettingAttribute Attribute { get; }

        /// <summary>
        /// Type of the property.
        /// </summary>
        public Type PropertyType { get; }

        public AudioSourceSettingInfo(IAudioSource source, PropertyInfo property, AudioSourceSettingAttribute attribute)
        {
            Source = source;
            Property = property;
            Attribute = attribute;
            PropertyType = property.PropertyType;
        }

        /// <summary>
        /// Validate a value for this setting.
        /// </summary>
        /// <param name="value">Value to validate.</param>
        /// <returns>Result of validation.</returns>
        public SettingValidationResult ValidateSetting(object value)
        {
            var validationMethodName = Attribute.ValidatorName;
            if (validationMethodName == null)
            {
                return new SettingValidationResult(true);
            }

            var method = Source.GetType().GetMethod(validationMethodName);
            if (method == null)
            {
                Logger.Error($"Validation method {validationMethodName} not found for {Source}");
                return new SettingValidationResult(false, "Error with validation. See log for more details.");
            }

            return (SettingValidationResult)method.Invoke(Source, new[] { value, Property.Name });
        }


        /// <summary>
        /// Updates the audio source with the new value.
        /// </summary>
        /// <param name="value">New value.</param>
        public void UpdateAudioSource(object value)
        {
            try
            {
                var newValue = GetSettingType(value);
                Property.SetValue(Source, newValue);
            }
            catch (Exception e)
            {
                Logger.Error($"Error occured while change audio source settings. value: {value}, Exception: {e}");
                throw;
            }
        }

        /// <summary>
        /// Convert the value to the type of the setting.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object GetSettingType(object value)
        {
            if (value != null && value.GetType() != PropertyType)
            {
                var converter = TypeDescriptor.GetConverter(value.GetType());
                if (converter.CanConvertTo(PropertyType))
                {
                    return converter.ConvertTo(value, PropertyType);
                }

                Logger.Warn($"Trying to set invalid type for setting {value} {value.GetType()}");
                throw new ArgumentException();
            }

            return value;
        }
    }
}
