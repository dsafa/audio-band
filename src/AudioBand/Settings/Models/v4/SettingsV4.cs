using System.Collections.Generic;
using AudioBand.Models;

namespace AudioBand.Settings.Models.V4
{
    /// <summary>
    /// Version 4 of the settings.
    /// </summary>
    public class SettingsV4
    {
        /// <summary>
        /// Gets or sets the version. Setter is required for toml serialization.
        /// </summary>
        public string Version { get; set; } = "4";

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
        public List<UserProfile> Profiles { get; set; }

        /// <summary>
        /// Gets or sets the audio source settings.
        /// </summary>
        public List<AudioSourceSettings> AudioSourceSettings { get; set; }
    }
}
