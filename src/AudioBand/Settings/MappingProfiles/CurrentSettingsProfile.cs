using AudioBand.Models;
using AutoMapper;
using System.Collections.Generic;
using CurrentSettings = AudioBand.Settings.Models.V4.SettingsV4;

namespace AudioBand.Settings.MappingProfiles
{
    /// <summary>
    /// Maps a settings v3 object to itself and fixes any null settings.
    /// </summary>
    internal class CurrentSettingsProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSettingsProfile"/> class.
        /// </summary>
        public CurrentSettingsProfile()
        {
            CreateMap<CurrentSettings, CurrentSettings>()
                .ForMember(dest => dest.Profiles, opt => opt.NullSubstitute(new List<CurrentSettings>()));
            CreateMap<UserProfile, UserProfile>()
                .ForMember(dest => dest.AlbumArtPopup, opt => opt.NullSubstitute(new AlbumArtPopup()))
                .ForMember(dest => dest.AlbumArt, opt => opt.NullSubstitute(new AlbumArt()))
                .ForMember(dest => dest.GeneralSettings, opt => opt.NullSubstitute(new AudioBand.Models.GeneralSettings()))
                .ForMember(dest => dest.CustomLabels, opt => opt.NullSubstitute(new List<CustomLabel>()))
                .ForMember(dest => dest.NextButton, opt => opt.NullSubstitute(new NextButton()))
                .ForMember(dest => dest.PlayPauseButton, opt => opt.NullSubstitute(new PlayPauseButton()))
                .ForMember(dest => dest.PreviousButton, opt => opt.NullSubstitute(new PreviousButton()))
                .ForMember(dest => dest.ProgressBar, opt => opt.NullSubstitute(new ProgressBar()))
                .ForMember(dest => dest.RepeatModeButton, opt => opt.NullSubstitute(new RepeatModeButton()))
                .ForMember(dest => dest.ShuffleModeButton, opt => opt.NullSubstitute(new ShuffleModeButton()));
        }
    }
}
