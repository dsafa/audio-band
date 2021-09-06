using AudioBand.Models;
using AudioBand.Settings.Models.V3;
using AutoMapper;
using System.Collections.Generic;

namespace AudioBand.Settings.MappingProfiles
{
    /// <summary>
    /// Mapping profile to maps a version 2 settings <see cref="AudioBand.Settings.Models.V2.SettingsV2"/> to a version 3 settings <see cref="SettingsV3"/>.
    /// </summary>
    public class SettingsV2ToSettingsV3Profile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsV2ToSettingsV3Profile"/> class.
        /// </summary>
        public SettingsV2ToSettingsV3Profile()
        {
            CreateMap<Models.V2.CustomLabelSettings, CustomLabel>()
                    .ForMember(dest => dest.Anchor, opt => opt.MapFrom(source => PositionAnchor.TopLeft))
                    .ForMember(dest => dest.ScrollBehavior, opt => opt.Ignore())
                    .ForMember(dest => dest.TextOverflow, opt => opt.Ignore())
                    .ForMember(dest => dest.LeftFadeOffset, opt => opt.Ignore())
                    .ForMember(dest => dest.RightFadeOffset, opt => opt.Ignore())
                    .ForMember(dest => dest.FadeEffect, opt => opt.Ignore());
            CreateMap<Models.V2.AudioSourceSettings, AudioSourceSettings>();
            CreateMap<Models.V2.NextButtonSettings, NextButton>()
                .ForMember(dest => dest.Anchor, opt => opt.MapFrom(source => PositionAnchor.TopLeft))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(source => new ButtonContent()))
                .ForPath(dest => dest.Content.ImagePath, opt => opt.MapFrom(source => source.ImagePath))
                .ForPath(dest => dest.Content.HoveredImagePath, opt => opt.MapFrom(source => source.ImagePath))
                .ForPath(dest => dest.Content.ClickedImagePath, opt => opt.MapFrom(source => source.ImagePath))
                .ForMember(dest => dest.BackgroundColor, opt => opt.Ignore())
                .ForMember(dest => dest.HoveredBackgroundColor, opt => opt.Ignore())
                .ForMember(dest => dest.ClickedBackgroundColor, opt => opt.Ignore())
                .ForPath(dest => dest.Content.ContentType, opt => opt.MapFrom(source => ButtonContentType.Image))
                .ForPath(dest => dest.Content.FontFamily, opt => opt.Ignore())
                .ForPath(dest => dest.Content.Text, opt => opt.Ignore())
                .ForPath(dest => dest.Content.TextColor, opt => opt.Ignore())
                .ForPath(dest => dest.Content.HoveredTextColor, opt => opt.Ignore())
                .ForPath(dest => dest.Content.ClickedTextColor, opt => opt.Ignore());
            CreateMap<Models.V2.PreviousButtonSettings, PreviousButton>()
                .ForMember(dest => dest.Anchor, opt => opt.MapFrom(source => PositionAnchor.TopLeft))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(source => new ButtonContent()))
                .ForPath(dest => dest.Content.ImagePath, opt => opt.MapFrom(source => source.ImagePath))
                .ForPath(dest => dest.Content.HoveredImagePath, opt => opt.MapFrom(source => source.ImagePath))
                .ForPath(dest => dest.Content.ClickedImagePath, opt => opt.MapFrom(source => source.ImagePath))
                .ForMember(dest => dest.BackgroundColor, opt => opt.Ignore())
                .ForMember(dest => dest.HoveredBackgroundColor, opt => opt.Ignore())
                .ForMember(dest => dest.ClickedBackgroundColor, opt => opt.Ignore())
                .ForPath(dest => dest.Content.ContentType, opt => opt.MapFrom(source => ButtonContentType.Image))
                .ForPath(dest => dest.Content.FontFamily, opt => opt.Ignore())
                .ForPath(dest => dest.Content.Text, opt => opt.Ignore())
                .ForPath(dest => dest.Content.TextColor, opt => opt.Ignore())
                .ForPath(dest => dest.Content.HoveredTextColor, opt => opt.Ignore())
                .ForPath(dest => dest.Content.ClickedTextColor, opt => opt.Ignore());
            CreateMap<Models.V2.PlayPauseButtonSettings, PlayPauseButton>()
                .ForMember(dest => dest.Anchor, opt => opt.MapFrom(source => PositionAnchor.TopLeft))
                .ForMember(dest => dest.BackgroundColor, opt => opt.Ignore())
                .ForMember(dest => dest.HoveredBackgroundColor, opt => opt.Ignore())
                .ForMember(dest => dest.ClickedBackgroundColor, opt => opt.Ignore())
                .ForMember(dest => dest.PlayContent, opt => opt.MapFrom(source => new ButtonContent()))
                .ForMember(dest => dest.PauseContent, opt => opt.MapFrom(source => new ButtonContent()))
                .ForPath(dest => dest.PlayContent.ImagePath, opt => opt.MapFrom(source => source.PlayButtonImagePath))
                .ForPath(dest => dest.PlayContent.HoveredImagePath, opt => opt.MapFrom(source => source.PlayButtonImagePath))
                .ForPath(dest => dest.PlayContent.ClickedImagePath, opt => opt.MapFrom(source => source.PlayButtonImagePath))
                .ForPath(dest => dest.PlayContent.ContentType, opt => opt.MapFrom(source => ButtonContentType.Image))
                .ForPath(dest => dest.PlayContent.FontFamily, opt => opt.Ignore())
                .ForPath(dest => dest.PlayContent.Text, opt => opt.Ignore())
                .ForPath(dest => dest.PlayContent.TextColor, opt => opt.Ignore())
                .ForPath(dest => dest.PlayContent.HoveredTextColor, opt => opt.Ignore())
                .ForPath(dest => dest.PlayContent.ClickedTextColor, opt => opt.Ignore())
                .ForPath(dest => dest.PauseContent.ImagePath, opt => opt.MapFrom(source => source.PauseButtonImagePath))
                .ForPath(dest => dest.PauseContent.HoveredImagePath, opt => opt.MapFrom(source => source.PauseButtonImagePath))
                .ForPath(dest => dest.PauseContent.ClickedImagePath, opt => opt.MapFrom(source => source.PauseButtonImagePath))
                .ForPath(dest => dest.PauseContent.ContentType, opt => opt.MapFrom(source => ButtonContentType.Image))
                .ForPath(dest => dest.PauseContent.FontFamily, opt => opt.Ignore())
                .ForPath(dest => dest.PauseContent.Text, opt => opt.Ignore())
                .ForPath(dest => dest.PauseContent.TextColor, opt => opt.Ignore())
                .ForPath(dest => dest.PauseContent.HoveredTextColor, opt => opt.Ignore())
                .ForPath(dest => dest.PauseContent.ClickedTextColor, opt => opt.Ignore());
            CreateMap<Models.V2.ProgressBarSettings, ProgressBar>()
                .ForMember(dest => dest.Anchor, opt => opt.MapFrom(source => PositionAnchor.TopLeft))
                .ForMember(dest => dest.HoverColor, opt => opt.Ignore());
            CreateMap<Models.V2.AudioBandSettings, AudioBand.Models.GeneralSettings>()
                .ForMember(dest => dest.BackgroundColor, opt => opt.Ignore());
            CreateMap<Models.V2.AlbumArtSettings, AlbumArt>()
                .ForMember(dest => dest.Anchor, opt => opt.MapFrom(source => PositionAnchor.TopLeft));
            CreateMap<Models.V2.AlbumArtPopupSettings, AlbumArtPopup>();
            CreateMap<Models.V2.SettingsV2, ProfileV3>()
                .ForMember(dest => dest.AlbumArtPopupSettings, opt => opt.MapFrom(source => source.AlbumArtPopupSettings))
                .ForMember(dest => dest.AlbumArtSettings, opt => opt.MapFrom(source => source.AlbumArtSettings))
                .ForMember(dest => dest.GeneralSettings, opt => opt.MapFrom(source => source.AudioBandSettings))
                .ForMember(dest => dest.CustomLabelSettings, opt => opt.MapFrom(source => source.CustomLabelSettings))
                .ForMember(dest => dest.NextButtonSettings, opt => opt.MapFrom(source => source.NextButtonSettings))
                .ForMember(dest => dest.PlayPauseButtonSettings, opt => opt.MapFrom(source => source.PlayPauseButtonSettings))
                .ForMember(dest => dest.PreviousButtonSettings, opt => opt.MapFrom(source => source.PreviousButtonSettings))
                .ForMember(dest => dest.ProgressBarSettings, opt => opt.MapFrom(source => source.ProgressBarSettings))
                .ForMember(dest => dest.RepeatModeButtonSettings, opt => opt.MapFrom(source => new RepeatModeButton()))
                .ForMember(dest => dest.ShuffleModeButtonSettings, opt => opt.MapFrom(source => new ShuffleModeButton()));
            CreateMap<Models.V2.SettingsV2, Dictionary<string, ProfileV3>>().ConvertUsing<ProfilesConverter>();
            CreateMap<Models.V2.AudioSourceSetting, AudioSourceSetting>();
            CreateMap<Models.V2.SettingsV2, SettingsV3>()
                .ForMember(dest => dest.Version, opt => opt.Ignore())
                .ForMember(dest => dest.AudioSourceSettings, opt => opt.MapFrom(source => source.AudioSourceSettings))
                .ForMember(dest => dest.Profiles, opt => opt.MapFrom(source => source))
                .ForMember(dest => dest.CurrentProfileName, opt => opt.MapFrom(source => SettingsV3.DefaultProfileName));
        }

        /// <summary>
        /// Converts older settings into a profile supported by newer settings.
        /// </summary>
        private class ProfilesConverter : ITypeConverter<Models.V2.SettingsV2, Dictionary<string, ProfileV3>>
        {
            public Dictionary<string, ProfileV3> Convert(Models.V2.SettingsV2 source, Dictionary<string, ProfileV3> destination, ResolutionContext context)
            {
                var profile = context.Mapper.Map<Models.V2.SettingsV2, ProfileV3>(source);
                return new Dictionary<string, ProfileV3> { { SettingsV3.DefaultProfileName, profile } };
            }
        }
    }
}
