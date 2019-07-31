using AudioBand.Settings.MappingProfiles;
using AudioBand.Settings.Models.v3;
using AudioBand.Settings.Models.v4;
using AutoMapper;

namespace AudioBand.Settings.Migrations
{
    /// <summary>
    /// Settings migrator from v3 to v4.
    /// </summary>
    public class V3ToV4 : ProfileSettingsMigrator<SettingsV3, SettingsV4, SettingsV3ToSettingsV4Profile>
    {
        /// <inheritdoc />
        protected override void ConfigureMapperConfig(IMapperConfigurationExpression cfg)
        {
            base.ConfigureMapperConfig(cfg);
            cfg.AddProfile<UserProfileToProfileV3Profile>();
        }
    }
}
