using System.Collections.Generic;

namespace AudioBand.Models
{
    /// <summary>
    /// Collection of settings for a specific audio source.
    /// </summary>
    internal class AudioSourceSettings : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the audio source.
        /// </summary>
        public string AudioSourceName { get; set; }

        /// <summary>
        /// Gets or sets the list of settings that the audio source exposes.
        /// </summary>
        public List<AudioSourceSetting> Settings { get; set; } = new List<AudioSourceSetting>();
    }
}
