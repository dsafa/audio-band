using AutoMapper;

namespace AudioBand.Settings.Migrations
{
    /// <summary>
    /// A <see cref="ISettingsMigrator"/> that uses a mapping <see cref="Profile"/> for migration.
    /// </summary>
    /// <typeparam name="TOldSettings">The type of the old setting to migrate.</typeparam>
    /// <typeparam name="TNewSettings">The desired type of the setting to migrate to.</typeparam>
    /// <typeparam name="TProfile">The type of the mapping profile to use.</typeparam>
    public class ProfileSettingsMigrator<TOldSettings, TNewSettings, TProfile> : ISettingsMigrator
        where TProfile : Profile
    {
        /// <summary>
        /// Migrate settings to new version.
        /// </summary>
        /// <param name="oldSetting">Old settings to migrate.</param>
        /// <returns>The new settings.</returns>
        public object MigrateSetting(object oldSetting)
        {
            var source = (TOldSettings)oldSetting;
            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(typeof(TProfile)));
            mappingConfig.AssertConfigurationIsValid();
            return mappingConfig.CreateMapper().Map<TOldSettings, TNewSettings>(source);
        }
    }
}
