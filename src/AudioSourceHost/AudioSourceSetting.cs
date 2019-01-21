﻿using System;
using System.ComponentModel;
using AudioBand.AudioSource;
using FastMember;
using NLog;

namespace AudioSourceHost
{
    /// <summary>
    /// Contains information for an audio source setting.
    /// </summary>
    internal class AudioSourceSetting
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
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

        /// <summary>
        /// Convert the value to the type of the setting.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>The value converted to the type of the setting.</returns>
        public object ConvertToSettingType(object value)
        {
            if (value != null && value.GetType() != SettingType)
            {
                var converter = TypeDescriptor.GetConverter(value.GetType());
                if (converter.CanConvertTo(SettingType))
                {
                    return converter.ConvertTo(value, SettingType);
                }

                if (TryChangeType(value, out var converted))
                {
                    return converted;
                }

                Logger.Error($"Unable to convert value to the setting's type. setting: `{Attribute.Name}` using value `{value}`. Setting type: `{SettingType}`, value type: `{value.GetType()}`");
                throw new ArgumentException();
            }

            return value;
        }

        private void UpdateValue(object value)
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

        private bool TryChangeType(object value, out object converted)
        {
            try
            {
                converted = Convert.ChangeType(value, SettingType);
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
