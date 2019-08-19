using AudioBand.Settings.MappingProfiles;
using AudioBand.Settings.Models.V3;
using AudioBand.Settings.Models.V4;

namespace AudioBand.Settings.Migrations
{
    /// <summary>
    /// Settings migrator from v3 to v4.
    /// </summary>
    public class V3ToV4 : ProfileSettingsMigrator<SettingsV3, SettingsV4, SettingsV3ToSettingsV4Profile>
    {
    }
}
