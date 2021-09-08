using AudioBand.Models;
using System.Collections.Generic;

namespace AudioBand.Settings.Persistence
{
    /// <summary>
    /// Represents the required information for persisted settings.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Gets or sets the current audio source.
        /// </summary>
        public string CurrentAudioSource { get; set; }

        /// <summary>
        /// Gets or sets the current profile name.
        /// </summary>
        public string CurrentProfileName { get; set; }

        /// <summary>
        /// Gets or sets the audio source settings.
        /// </summary>
        public IEnumerable<AudioSourceSettings> AudioSourceSettings { get; set; } = new List<AudioSourceSettings>();

        /// <summary>
        /// Gets or sets the AudioBand settings.
        /// </summary>
        public AudioBandSettings AudioBandSettings { get; set; } = new AudioBandSettings();
    }
}
