using System;
using System.Collections.Generic;
using AudioBand.AudioSource;
using AudioBand.Models;

namespace AudioBand.Extensions
{
    /// <summary>
    /// Extenstions for <see cref="IAudioSource"/>.
    /// </summary>
    internal static class AudioSourceExtensions
    {
        /// <summary>
        /// Get settings exposed by the audio source.
        /// </summary>
        /// <param name="audioSource">Audio source.</param>
        /// <returns>A list of settings.</returns>
        public static List<AudioSourceSettingAttribute> GetSettings(this IAudioSource audioSource)
        {
            return audioSource.ToProxy().AudioSourceSettings;
        }

        public static void UpdateSetting(this IAudioSource audioSource, string settingName, object value)
        {
            audioSource.ToProxy()[settingName] = value;
        }

        public static object GetSettingValue(this IAudioSource audioSource, string settingName)
        {
            return audioSource.ToProxy()[settingName];
        }

        private static AudioSourceProxy ToProxy(this IAudioSource audioSource)
        {
            var proxy = audioSource as AudioSourceProxy;
            if (proxy == null)
            {
                throw new NotSupportedException("Only audio source proxy is supported");
            }

            return proxy;
        }
    }
}
