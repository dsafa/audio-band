using System.Collections.Generic;
using System.Linq;
using AudioBand.AudioSource;
using FastMember;

namespace AudioSourceHost
{
    /// <summary>
    /// Extensions for <see cref="IAudioSource"/>.
    /// </summary>
    internal static class AudioSourceExtensions
    {
        /// <summary>
        /// Get settings exposed by the audio source.
        /// </summary>
        /// <param name="audiosource">Audio source.</param>
        /// <returns>A list of settings.</returns>
        public static List<AudioSourceSetting> GetSettings(this IAudioSource audiosource)
        {
            var typeAccessor = TypeAccessor.Create(audiosource.GetType());
            var objectAccessor = ObjectAccessor.Create(audiosource);

            return typeAccessor.GetMembers()
                .Where(m => m.IsDefined(typeof(AudioSourceSettingAttribute)))
                .Select(m => new AudioSourceSetting(audiosource, objectAccessor, m.Type, m.Name, (AudioSourceSettingAttribute)m.GetAttribute(typeof(AudioSourceSettingAttribute), true)))
                .ToList();
        }
    }
}
