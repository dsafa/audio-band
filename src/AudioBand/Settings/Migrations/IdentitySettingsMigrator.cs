namespace AudioBand.Settings.Migrations
{
    /// <summary>
    /// Performs no migration.
    /// </summary>
    internal class IdentitySettingsMigrator : ISettingsMigrator
    {
        /// <summary>
        /// Migrate settings to new version.
        /// </summary>
        /// <param name="oldSetting">Old settings to migrate.</param>
        /// <returns>The new settings.</returns>
        public object MigrateSetting(object oldSetting)
        {
            return oldSetting;
        }
    }
}
