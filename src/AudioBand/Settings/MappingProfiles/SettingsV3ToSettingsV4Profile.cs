using System.Collections.Generic;
using AudioBand.Models;
using AudioBand.Settings.Models.V3;
using AudioBand.Settings.Models.V4;
using AutoMapper;

namespace AudioBand.Settings.MappingProfiles
{
    /// <summary>
    /// Mapping profile that maps from version 3 settings to version 4 settings.
    /// </summary>
    public class SettingsV3ToSettingsV4Profile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsV3ToSettingsV4Profile"/> class.
        /// </summary>
        public SettingsV3ToSettingsV4Profile()
        {
            CreateMap<SettingsV3, SettingsV4>()
                .ForMember(dest => dest.AudioSource, opt => opt.MapFrom(src => src.AudioSource))
                .ForMember(dest => dest.CurrentProfileName, opt => opt.MapFrom(src => src.CurrentProfileName))
                .ForMember(dest => dest.AudioSourceSettings, opt => opt.MapFrom(src => src.AudioSourceSettings))
                .ForMember(dest => dest.Profiles, opt => opt.MapFrom(src => src.Profiles));
            CreateMap<KeyValuePair<string, ProfileV3>, UserProfile>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.AlbumArtPopup, opt => opt.MapFrom(src => src.Value.AlbumArtPopupSettings))
                .ForMember(dest => dest.AlbumArt, opt => opt.MapFrom(src => src.Value.AlbumArtSettings))
                .ForMember(dest => dest.CustomLabels, opt => opt.MapFrom(src => src.Value.CustomLabelSettings))
                .ForMember(dest => dest.GeneralSettings, opt => opt.MapFrom(src => src.Value.GeneralSettings))
                .ForMember(dest => dest.NextButton, opt => opt.MapFrom(src => src.Value.NextButtonSettings))
                .ForMember(dest => dest.PlayPauseButton, opt => opt.MapFrom(src => src.Value.PlayPauseButtonSettings))
                .ForMember(dest => dest.PreviousButton, opt => opt.MapFrom(src => src.Value.PreviousButtonSettings))
                .ForMember(dest => dest.ProgressBar, opt => opt.MapFrom(src => src.Value.ProgressBarSettings))
                .ForMember(dest => dest.RepeatModeButton, opt => opt.MapFrom(src => src.Value.RepeatModeButtonSettings))
                .ForMember(dest => dest.ShuffleModeButton, opt => opt.MapFrom(src => src.Value.ShuffleModeButtonSettings));
        }
    }
}
