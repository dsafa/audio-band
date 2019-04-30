namespace AudioBand.Settings.Migrations
{
    /// <summary>
    /// Migrates settings from one version to another.
    /// </summary>
    internal interface ISettingsMigrator
    {
        /// <summary>
        /// Migrate settings to new version.
        /// </summary>
        /// <param name="oldSetting">Old settings to migrate.</param>
        /// <returns>The new settings.</returns>
        object MigrateSetting(object oldSetting);
    }
}
