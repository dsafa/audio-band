using AudioBand.Models;
using System.Collections.Generic;

namespace AudioBand.Settings.Persistence
{
    /// <summary>
    /// Persists app settings and profiles.
    /// </summary>
    public interface IPersistentSettings
    {
        /// <summary>
        /// Checks if a user has old settings (using Toml instead of json) and converts it over if found.
        /// </summary>
        void CheckAndConvertOldSettings();

        /// <summary>
        /// Reads the settings.
        /// </summary>
        /// <returns>The previously persisted settings.</returns>
        Settings ReadSettings();

        /// <summary>
        /// Writes the settings.
        /// </summary>
        /// <param name="settings">The settings to persist.</param>
        void WriteSettings(Settings settings);

        /// <summary>
        /// Reads a UserProfile from a path.
        /// </summary>
        /// <param name="path">The path to read the profile from.</param>
        /// <returns>The UserProfile found in the file.</returns>
        UserProfile ReadProfile(string path);

        /// <summary>
        /// Reads the profiles loaded from the settings object.
        /// </summary>
        /// <returns>A collection of <see cref="UserProfile" />s.</returns>
        IEnumerable<UserProfile> ReadProfiles();

        /// <summary>
        /// Writes the profiles from the settings object.
        /// </summary>
        /// <param name="profiles">The settings object that contains the profiles to write.</param>
        void WriteProfiles(IEnumerable<UserProfile> profiles);

        /// <summary>
        /// Deletes a profile from the folder, so it wont be loaded in again.
        /// </summary>
        /// <param name="profileName">The name of the profile to delete.</param>
        void DeleteProfile(string profileName);
    }
}
