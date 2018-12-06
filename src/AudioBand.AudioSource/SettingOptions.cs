using System;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Flags for audio source settings.
    /// </summary>
    [Flags]
    public enum SettingOptions
    {
        /// <summary>
        /// Setting is invisible to the user.
        /// </summary>
        Hidden = 1 << 0,

        /// <summary>
        /// Setting cannot be modified by the user.
        /// </summary>
        ReadOnly = 1 << 1,

        /// <summary>
        /// Indicates a sensitive setting such as a password, causing a warning to be given.
        /// </summary>
        Sensitive = 1 << 2,
    }
}
