namespace AudioBand.Models
{
    /// <summary>
    /// Collection of global AudioBand settings.
    /// </summary>
    public class AudioBandSettings
    {
        /// <summary>
        /// Gets the last active profile name before going into idle.
        /// </summary>
        public string LastNonIdleProfileName { get; set; }

        /// <summary>
        /// Gets or sets whether to use the Idle Profile.
        /// </summary>
        public bool UseAutomaticIdleProfile { get; set; }

        /// <summary>
        /// Gets or sets whether to hide the Idle profile in the Profiles quick menu.
        /// </summary>
        public bool HideIdleProfileInQuickMenu { get; set; }

        /// <summary>
        /// Gets or sets the amount of time in seconds it should take for AudioBand to go idle.
        /// </summary>
        public int ShouldGoIdleAfterInSeconds { get; set; }
    }
}
