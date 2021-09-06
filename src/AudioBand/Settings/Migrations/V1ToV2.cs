using AudioBand.Settings.MappingProfiles;
using V1Settings = AudioBand.Settings.Models.V1.AudioBandSettings;
using V2Settings = AudioBand.Settings.Models.V2.SettingsV2;

namespace AudioBand.Settings.Migrations
{
    /// <summary>
    /// Migrates settings from version 1 to version 2.
    /// </summary>
    internal class V1ToV2 : ProfileSettingsMigrator<V1Settings, V2Settings, SettingsV1ToSettingsV2Profile>
    {
    }
}
