using AudioBand.Models;
using AudioBand.Settings.Models.v3;
using AutoMapper;

namespace AudioBand.Settings.MappingProfiles
{
    /// <summary>
    /// Maps from <see cref="UserProfile"/> to the settings profile model.
    /// </summary>
    public class UserProfileToProfileV3Profile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileToProfileV3Profile"/> class.
        /// </summary>
        public UserProfileToProfileV3Profile()
        {
            CreateMap<UserProfile, ProfileV3>()
                .ForMember(dest => dest.AlbumArtPopupSettings, opt => opt.MapFrom(src => src.AlbumArtPopup))
                .ForMember(dest => dest.AlbumArtSettings, opt => opt.MapFrom(src => src.AlbumArt))
                .ForMember(dest => dest.CustomLabelSettings, opt => opt.MapFrom(src => src.CustomLabels))
                .ForMember(dest => dest.GeneralSettings, opt => opt.MapFrom(src => src.GeneralSettings))
                .ForMember(dest => dest.NextButtonSettings, opt => opt.MapFrom(src => src.NextButton))
                .ForMember(dest => dest.PlayPauseButtonSettings, opt => opt.MapFrom(src => src.PlayPauseButton))
                .ForMember(dest => dest.PreviousButtonSettings, opt => opt.MapFrom(src => src.PreviousButton))
                .ForMember(dest => dest.ProgressBarSettings, opt => opt.MapFrom(src => src.ProgressBar))
                .ForMember(dest => dest.RepeatModeButtonSettings, opt => opt.MapFrom(src => src.RepeatModeButton))
                .ForMember(dest => dest.ShuffleModeButtonSettings, opt => opt.MapFrom(src => src.ShuffleModeButton))
                .ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.Ignore());
        }
    }
}
