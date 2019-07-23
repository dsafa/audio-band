using System;
using System.Collections.Generic;
using AudioBand.Models;

namespace AudioBand.Settings
{
    /// <summary>
    /// Manages application settings.
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// Occurs when a profile changes
        /// </summary>
        event EventHandler ProfileChanged;

        /// <summary>
        ///  Gets or sets the saved audio source.
        /// </summary>
        string AudioSource { get; set; }

        /// <summary>
        /// Gets the saved audio source settings.
        /// </summary>
        List<AudioSourceSettings> AudioSourceSettings { get; }

        /// <summary>
        /// Gets the current profile.
        /// </summary>
        UserProfile CurrentProfile { get; }

        /// <summary>
        /// Gets the list of profiles.
        /// </summary>
        IEnumerable<UserProfile> Profiles { get; }

        /// <summary>
        /// Selects a new profile.
        /// </summary>
        /// <param name="profileName">The name of the profile to switch to.</param>
        void SelectProfile(string profileName);

        /// <summary>
        /// Creates a new profile.
        /// </summary>
        /// <param name="profileName">The name of the new profile.</param>
        void CreateProfile(string profileName);

        /// <summary>
        /// Deletes a profile.
        /// </summary>
        /// <param name="profileName">The name of the profile to delete.</param>
        void DeleteProfile(string profileName);

        /// <summary>
        /// Renames the current profile.
        /// </summary>
        /// <param name="newProfileName">The name of the new profile.</param>
        void RenameCurrentProfile(string newProfileName);

        /// <summary>
        /// Save the settings.
        /// </summary>
        void Save();

        /// <summary>
        /// Import settings from a path.
        /// </summary>
        /// <param name="path">The path of the settings file.</param>
        void ImportProfilesFromPath(string path);

        /// <summary>
        /// Export settings to a path.
        /// </summary>
        /// <param name="path">The path to output the settings file.</param>
        void ExportProfilesToPath(string path);
    }
}
