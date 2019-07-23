using System.Collections.Generic;
using AudioBand.Models;
using AudioBand.Settings.Models.v3;
using Profile = AutoMapper.Profile;

namespace AudioBand.Settings.MappingProfiles
{
    /// <summary>
    /// Maps a settings v3 object to itself and fixes any null settings.
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
                .ForMember(dest => dest.AlbumArtPopupSettings, opt => opt.NullSubstitute(new AlbumArtPopup()))
                .ForMember(dest => dest.AlbumArtSettings, opt => opt.NullSubstitute(new AlbumArt()))
                .ForMember(dest => dest.GeneralSettings, opt => opt.NullSubstitute(new AudioBand.Models.GeneralSettings()))
                .ForMember(dest => dest.CustomLabelSettings, opt => opt.NullSubstitute(new List<CustomLabel>()))
                .ForMember(dest => dest.NextButtonSettings, opt => opt.NullSubstitute(new NextButton()))
                .ForMember(dest => dest.PlayPauseButtonSettings, opt => opt.NullSubstitute(new PlayPauseButton()))
                .ForMember(dest => dest.PreviousButtonSettings, opt => opt.NullSubstitute(new PreviousButton()))
                .ForMember(dest => dest.ProgressBarSettings, opt => opt.NullSubstitute(new ProgressBar()))
                .ForMember(dest => dest.RepeatModeButtonSettings, opt => opt.NullSubstitute(new RepeatModeButton()))
                .ForMember(dest => dest.ShuffleModeButtonSettings, opt => opt.NullSubstitute(new ShuffleModeButton()));
        }
    }
}
