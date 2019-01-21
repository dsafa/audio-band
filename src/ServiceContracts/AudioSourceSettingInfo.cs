using AudioBand.AudioSource;
using System;
using System.Runtime.Serialization;

namespace ServiceContracts
{
    /// <summary>
    /// Data contract for <see cref="AudioSourceSettingAttribute"/>.
    /// </summary>
    [DataContract]
    public class AudioSourceSettingInfo
    {
        /// <summary>
        /// Data member for <see cref="AudioSourceSettingAttribute.Name"/>
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Data member for <see cref="AudioSourceSettingAttribute.Options"/>.
        /// </summary>
        [DataMember]
        public SettingOptions Options { get; set; }

        /// <summary>
        /// Data member for <see cref="AudioSourceSettingAttribute.Priority"/>.
        /// </summary>
        [DataMember]
        public int Priority { get; set; }

        /// <summary>
        /// Data member fore <see cref="AudioSourceSettingAttribute.Description"/>.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Value of the setting.
        /// </summary>
        [DataMember]
        public object Value { get; set; }

        /// <summary>
        /// Type of the setting value.
        /// </summary>
        [DataMember]
        public Type ValueType { get; set; }

        public static AudioSourceSettingInfo From(AudioSourceSettingAttribute attribute, object value, Type valueType)
        {
            return new AudioSourceSettingInfo
            {
                Description = attribute.Description,
                Name = attribute.Name,
                Options = attribute.Options,
                Priority = attribute.Priority,
                Value = value,
                ValueType = valueType
            };
        }
    }
}
