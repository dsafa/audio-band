using System;
using System.ComponentModel;
using FastMember;
using NLog;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Contains information for an audio source setting.
    /// </summary>
    internal class AudioSourceSettingInfo
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly ObjectAccessor _accessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceSettingInfo"/> class
        /// with the source and setting attributes.
        /// </summary>
        /// <param name="source">The audio source.</param>
        /// <param name="accessor">The accessor for the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="attribute">The setting attribute.</param>
        public AudioSourceSettingInfo(IAudioSource source, ObjectAccessor accessor, Type propertyType, string propertyName, AudioSourceSettingAttribute attribute)
        {
            _accessor = accessor;
            PropertyName = propertyName;
            Source = source;
            Attribute = attribute;
            PropertyType = propertyType;
        }

        /// <summary>
        /// Gets the <see cref="IAudioSource"/> that this setting belongs to.
        /// </summary>
        public IAudioSource Source { get; }

        /// <summary>
        /// Gets the name of the property associated with the setting.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the <see cref="AudioSourceSettingAttribute"/> attached to the property.
        /// </summary>
        public AudioSourceSettingAttribute Attribute { get; }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public Type PropertyType { get; }

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

            return (SettingValidationResult)method.Invoke(Source, new[] { value, PropertyName });
        }

        /// <summary>
        /// Updates the audio source with the new value.
        /// </summary>
        /// <param name="value">New value.</param>
        public void UpdateAudioSource(object value)
        {
            try
            {
                var newValue = ConvertToSettingType(value);
                _accessor[PropertyName] = newValue;
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error occured while changing audio source settings. property: `{PropertyName}`, value: {value}");
                throw;
            }
        }

        /// <summary>
        /// Gets the current setting value from the audio source.
        /// </summary>
        /// <returns>The value of the setting.</returns>
        public object GetValue()
        {
            return _accessor[PropertyName];
        }

        /// <summary>
        /// Convert the value to the type of the setting.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>The value converted to the type of the setting.</returns>
        public object ConvertToSettingType(object value)
        {
            if (value != null && value.GetType() != PropertyType)
            {
                var converter = TypeDescriptor.GetConverter(value.GetType());
                if (converter.CanConvertTo(PropertyType))
                {
                    return converter.ConvertTo(value, PropertyType);
                }

                if (TryChangeType(value, out var converted))
                {
                    return converted;
                }

                Logger.Error($"Unable to convert value to the setting's type. setting: `{Attribute.Name}` using value `{value}`. Setting type: `{PropertyType}`, value type: `{value.GetType()}`");
                throw new ArgumentException();
            }

            return value;
        }

        private bool TryChangeType(object value, out object converted)
        {
            try
            {
                converted = Convert.ChangeType(value, PropertyType);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                converted = null;
                return false;
            }
        }
    }
}
