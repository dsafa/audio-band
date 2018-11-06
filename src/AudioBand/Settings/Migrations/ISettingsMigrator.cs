namespace AudioBand.Settings.Migrations
{
    /// <summary>
    /// Migrates settings from one version to another
    /// </summary>
    internal interface ISettingsMigrator
    {
        object MigrateSetting(object oldSetting);
    }
}
