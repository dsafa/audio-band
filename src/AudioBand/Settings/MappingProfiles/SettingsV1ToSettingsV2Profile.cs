using AudioBand.Settings.Models.V1;
using AutoMapper;

namespace AudioBand.Settings.MappingProfiles
{
    /// <summary>
    /// Mapping profile from settings version 1 <see cref="AudioBandSettings"/> to settings version 2 <see cref="AudioBand.Settings.Models.V2"/>.
    /// </summary>
    public class SettingsV1ToSettingsV2Profile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsV1ToSettingsV2Profile"/> class.
        /// </summary>
        public SettingsV1ToSettingsV2Profile()
        {
            CreateMap<Models.V1.AudioSourceSetting, Models.V2.AudioSourceSetting>();
            CreateMap<Models.V1.AlbumArtAppearance, Models.V2.AlbumArtSettings>();
            CreateMap<Models.V1.AudioBandAppearance, Models.V2.AudioBandSettings>();
            CreateMap<Models.V1.NextSongButtonAppearance, Models.V2.NextButtonSettings>();
            CreateMap<Models.V1.PlayPauseButtonAppearance, Models.V2.PlayPauseButtonSettings>();
            CreateMap<Models.V1.PreviousSongButtonAppearance, Models.V2.PreviousButtonSettings>();
            CreateMap<Models.V1.ProgressBarAppearance, Models.V2.ProgressBarSettings>();
            CreateMap<Models.V1.AlbumArtPopupAppearance, Models.V2.AlbumArtPopupSettings>()
                .ForMember(dest => dest.XPosition, opts => opts.MapFrom(source => source.XOffset));
            CreateMap<Models.V1.TextAppearance, Models.V2.CustomLabelSettings>();
            CreateMap<Models.V1.AudioSourceSettingsCollection, Models.V2.AudioSourceSettings>()
                .ForMember(dest => dest.AudioSourceName, opts => opts.MapFrom(source => source.Name));
            CreateMap<AudioBandSettings, Models.V2.SettingsV2>()
                .ForMember(dest => dest.AlbumArtPopupSettings, opts => opts.MapFrom(source => source.AlbumArtPopupAppearance))
                .ForMember(dest => dest.AlbumArtSettings, opts => opts.MapFrom(source => source.AlbumArtAppearance))
                .ForMember(dest => dest.AudioBandSettings, opts => opts.MapFrom(source => source.AudioBandAppearance))
                .ForMember(dest => dest.NextButtonSettings, opts => opts.MapFrom(source => source.NextSongButtonAppearance))
                .ForMember(dest => dest.PlayPauseButtonSettings, opts => opts.MapFrom(source => source.PlayPauseButtonAppearance))
                .ForMember(dest => dest.PreviousButtonSettings, opts => opts.MapFrom(source => source.PreviousSongButtonAppearance))
                .ForMember(dest => dest.ProgressBarSettings, opts => opts.MapFrom(source => source.ProgressBarAppearance))
                .ForMember(dest => dest.CustomLabelSettings, opts => opts.MapFrom(source => source.TextAppearances))
                .ForMember(dest => dest.Version, opts => opts.Ignore());
        }
    }
}
