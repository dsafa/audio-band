using AudioBand.Models;
using System.Collections.Generic;

namespace AudioBand.Settings.Persistence
{
    /// <summary>
    /// Persists app settings and profiles.
    /// </summary>
    public interface IPersistSettings
    {
        /// <summary>
        /// Writes the settings.
        /// </summary>
        /// <param name="settings">The settings to persist.</param>
        void WriteSettings(PersistedSettingsDto settings);

        /// <summary>
        /// Reads the settings.
        /// </summary>
        /// <returns>The previously persisted settings.</returns>
        PersistedSettingsDto ReadSettings();

        /// <summary>
        /// Writes the profiles from the settings object.
        /// </summary>
        /// <param name="profiles">The settings object that contains the profiles to write.</param>
        /// <param name="path">The path to write to.</param>
        void WriteProfiles(IEnumerable<UserProfile> profiles, string path);

        /// <summary>
        /// Reads the profiles loaded from the settings object.
        /// </summary>
        /// <param name="path">The path to read the profiles from.</param>
        /// <returns>The settings object that contains the profiles.</returns>
        IEnumerable<UserProfile> ReadProfiles(string path);
    }
}
