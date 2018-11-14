using System;
using System.Collections.Generic;
using AudioBand.AudioSource;

namespace AudioBand.Models
{
    /// <summary>
    /// Collection of settings for a specific audio source
    /// </summary>
    internal class AudioSourceSettings : ModelBase
    {
        /// <summary>
        /// Name of the audio source
        /// </summary>
        public string AudioSourceName { get; set; }

        /// <summary>
        /// List of settings that the audio source exposes
        /// </summary>
        public List<AudioSourceSetting> Settings { get; set; } = new List<AudioSourceSetting>();
    }
}
