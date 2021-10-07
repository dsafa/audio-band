using AudioBand.AudioSource;
using AudioBand.Logging;
using FastMember;
using NLog;
using System;

namespace AudioSourceHost
{
    /// <summary>
    /// Contains information for an audio source setting.
    /// </summary>
    internal class AudioSourceSetting
    {
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<AudioSourceSetting>();
        private readonly ObjectAccessor _accessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceSetting"/> class
        /// with the source and setting attributes.
        /// </summary>
        /// <param name="accessor">The accessor for the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="attribute">The setting attribute.</param>
        public AudioSourceSetting(ObjectAccessor accessor, Type propertyType, string propertyName, AudioSourceSettingAttribute attribute)
        {
            _accessor = accessor;
            PropertyName = propertyName;
            Attribute = attribute;
            SettingType = propertyType;
        }

        /// <summary>
        /// Gets the name of the property associated with the setting.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the <see cref="AudioSourceSettingAttribute"/> attached to the property.
        /// </summary>
        public AudioSourceSettingAttribute Attribute { get; }

        /// <summary>
        /// Gets or sets the current setting value from the audio source.
        /// </summary>
        public object SettingValue
        {
            get
            {
                return _accessor[PropertyName];
            }

            set
            {
                UpdateValue(value);
            }
        }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public Type SettingType { get; }

        private void UpdateValue(object value)
        {
            try
            {
                var newValue = TypeConvertHelper.ConvertToType(value, SettingType);
                _accessor[PropertyName] = newValue;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error occured while updating audio source settings. {info}", new { PropertyName, Value = value, SettingType });

                if (SettingType.IsValueType)
                {
                    Logger.Error("Using default value of the type.");
                    _accessor[PropertyName] = Activator.CreateInstance(SettingType);
                }
            }
        }
    }
}
