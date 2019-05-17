using System.Collections.Generic;

namespace AudioBand.Settings.Models.v3
{
    /// <summary>
    /// The format for exported profiles.
    /// </summary>
    public class ProfileExportV3
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; } = "3";

        /// <summary>
        /// Gets or sets the profiles.
        /// </summary>
        public Dictionary<string, ProfileV3> Profiles { get; set; }
    }
}
