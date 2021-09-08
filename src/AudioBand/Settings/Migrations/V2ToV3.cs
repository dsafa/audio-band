using AudioBand.Settings.MappingProfiles;
using V2Settings = AudioBand.Settings.Models.V2.SettingsV2;
using V3Settings = AudioBand.Settings.Models.V3.SettingsV3;

namespace AudioBand.Settings.Migrations
{
    /// <summary>
    /// Migrates settings from v2 to v3.
    /// </summary>
    public class V2ToV3 : ProfileSettingsMigrator<V2Settings, V3Settings, SettingsV2ToSettingsV3Profile>
    {
    }
}
