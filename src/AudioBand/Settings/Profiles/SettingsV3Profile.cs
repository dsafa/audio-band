using AudioBand.Models;
using AudioBand.Settings.Models.v3;
using AutoMapper;

namespace AudioBand.Settings.Profiles
{
    /// <summary>
    /// Fixes new values that are added to the configuration.
    /// </summary>
    internal class SettingsV3Profile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsV3Profile"/> class.
        /// </summary>
        public SettingsV3Profile()
        {
            CreateMap<SettingsV3, SettingsV3>();
            CreateMap<ProfileV3, ProfileV3>()
                .ForMember(dest => dest.RepeatModeButtonSettings, opt => opt.NullSubstitute(new RepeatModeButton())) // Repeat mode button was added after
                .ForMember(dest => dest.ShuffleModeButtonSettings, opt => opt.NullSubstitute(new ShuffleModeButton()));
        }
    }
}
