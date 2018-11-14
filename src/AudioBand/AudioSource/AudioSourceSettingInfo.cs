using System;
using System.Reflection;

namespace AudioBand.AudioSource
{
    internal class AudioSourceSettingInfo
    {
        public IAudioSource Source { get; }
        public PropertyInfo Property { get; }
        public AudioSourceSettingAttribute Attribute { get; }
        public Type PropertyType { get; }

        public AudioSourceSettingInfo(IAudioSource source, PropertyInfo property, AudioSourceSettingAttribute attribute)
        {
            Source = source;
            Property = property;
            Attribute = attribute;
            PropertyType = property.PropertyType;
        }
    }
}
