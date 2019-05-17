using System.Collections.Generic;
using AudioBand.Models;

namespace AudioBand.Settings.Models.v3
{
    /// <summary>
    /// Version 3 for settings.
    /// </summary>
    public class SettingsV3
    {
        /// <summary>
        /// The default profile name.
        /// </summary>
        public const string DefaultProfileName = "Default Profile";

        /// <summary>
        /// Gets or sets the version. Setter is required for toml serialization.
        /// </summary>
        public string Version { get; set; } = "3";

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
        public Dictionary<string, ProfileV3> Profiles { get; set; }

        /// <summary>
        /// Gets or sets the audio source settings.
        /// </summary>
        public List<AudioSourceSettings> AudioSourceSettings { get; set; }
    }
}
