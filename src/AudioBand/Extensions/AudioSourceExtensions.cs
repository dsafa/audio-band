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

        /// <summary>
        /// Updates the value of a setting.
        /// </summary>
        /// <param name="audioSource">The audio source to update.</param>
        /// <param name="settingName">The name of the setting to update.</param>
        /// <param name="value">The new value to set.</param>
        public static void UpdateSetting(this IAudioSource audioSource, string settingName, object value)
        {
            audioSource.ToProxy()[settingName] = value;
        }

        /// <summary>
        /// Gets the value of a setting.
        /// </summary>
        /// <param name="audioSource">The audio source.</param>
        /// <param name="settingName">The name of the setting.</param>
        /// <returns>The value of the setting.</returns>
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
