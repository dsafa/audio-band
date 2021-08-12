using System.Collections.Generic;
using AudioBand.Models;

namespace AudioBand.Settings.Persistence
{
    /// <summary>
    /// Represents the required information for persisted settings.
    /// </summary>
    public class PersistedSettingsDto
    {
        /// <summary>
        /// Gets or sets the current audio source.
        /// </summary>
        public string AudioSource { get; set; }

        /// <summary>
        /// Gets or sets the current profile name.
        /// </summary>
        public string CurrentProfileName { get; set; }

        /// <summary>
        /// Gets or sets the profiles.
        /// </summary>
        public IEnumerable<UserProfile> Profiles { get; set; }

        /// <summary>
        /// Gets or sets the audio source settings.
        /// </summary>
        public IEnumerable<AudioSourceSettings> AudioSourceSettings { get; set; }

        /// <summary>
        /// Gets or sets the AudioBand settings.
        /// </summary>
        public AudioBandSettings AudioBandSettings { get; }
    }
}
