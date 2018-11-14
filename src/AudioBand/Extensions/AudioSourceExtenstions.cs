using AudioBand.AudioSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AudioBand.Extensions
{
    internal static class AudioSourceExtenstions
    {
        /// <summary>
        /// Get settings exposed by the audio source.
        /// </summary>
        /// <param name="audiosource">Audio source.</param>
        /// <returns>A list of settings.</returns>
        public static List<AudioSourceSettingInfo> GetSettings(this IAudioSource audiosource)
        {
            return audiosource.GetType().GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(AudioSourceSettingAttribute)))
                .Select(prop => new AudioSourceSettingInfo(audiosource, prop, prop.GetCustomAttribute<AudioSourceSettingAttribute>()))
                .ToList();
        }
    }
}
