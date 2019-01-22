using AudioBand.AudioSource;
using System;
using System.Runtime.Serialization;

namespace AudioBand.ServiceContracts
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

        public static explicit operator AudioSourceSettingInfo(AudioSourceSettingAttribute attribute)
        {
            return new AudioSourceSettingInfo
            {
                Description = attribute.Description,
                Name = attribute.Name,
                Options = attribute.Options,
                Priority = attribute.Priority,
            };
        }

        public static explicit operator AudioSourceSettingAttribute(AudioSourceSettingInfo info)
        {
            return new AudioSourceSettingAttribute(info.Name)
            {
                Description = info.Description,
                Options = info.Options,
                Priority = info.Priority,
            };
        }
    }
}
