using System;
using System.Collections.Generic;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Internal extension to <see cref="IAudioSource"/> that exposes additional methods.
    /// </summary>
    public interface IInternalAudioSource : IAudioSource
    {
        /// <summary>
        /// Gets the settings that the audio source exposes.
        /// </summary>
        List<AudioSourceSettingAttribute> Settings { get; }

        /// <summary>
        /// Gets or sets a setting.
        /// </summary>
        /// <param name="settingName">The name of the setting.</param>
        /// <returns>The value of the setting.</returns>
        object this[string settingName] { get; set; }

        /// <summary>
        /// Gets the type of the setting.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>The type of the setting.</returns>
        Type GetSettingType(string settingName);
    }
}
