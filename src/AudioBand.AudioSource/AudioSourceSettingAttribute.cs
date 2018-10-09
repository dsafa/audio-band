using System;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Expose this property as a setting that can be set by a user
    /// </summary>
    /// <inheritdoc cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Property)]
    public class AudioSourceSettingAttribute : Attribute
    {
        /// <summary>
        /// Name that will be seen by user.
        /// </summary>
        public string Name { get; set; }

        public AudioSourceSettingAttribute(string name)
        {
            Name = name;
        }
    }
}
