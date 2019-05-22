using System;
using System.Collections.Generic;
using AudioBand.Models;
using AudioBand.Settings.Models.v3;
using AutoMapper;

namespace AudioBand.Settings.Migrations
{
    /// <summary>
    /// Migrates settings from v2 to v3.
    /// </summary>
    public class V2ToV3 : ISettingsMigrator
    {
        /// <inheritdoc />
        public object MigrateSetting(object oldSetting)
        {
            var source = oldSetting as Models.V2.Settings;
            if (source == null)
            {
                throw new ArgumentException($"Expected type of {typeof(Models.V2.Settings)}", nameof(oldSetting));
            }

            return GetMapConfig().CreateMapper().Map<Models.V2.Settings, SettingsV3>(source);
        }

        private MapperConfiguration GetMapConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Models.V2.CustomLabelSettings, CustomLabel>()
                    .ForMember(dest => dest.ScrollBehavior, opt => opt.Ignore())
                    .ForMember(dest => dest.TextOverflow, opt => opt.Ignore())
                    .ForMember(dest => dest.LeftFadeOffset, opt => opt.Ignore())
                    .ForMember(dest => dest.RightFadeOffset, opt => opt.Ignore())
                    .ForMember(dest => dest.FadeEffect, opt => opt.Ignore());
                cfg.CreateMap<Models.V2.AudioSourceSettings, AudioSourceSettings>();
                cfg.CreateMap<Models.V2.NextButtonSettings, NextButton>()
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
                cfg.CreateMap<Models.V2.PreviousButtonSettings, PreviousButton>()
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
                cfg.CreateMap<Models.V2.PlayPauseButtonSettings, PlayPauseButton>()
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
                cfg.CreateMap<Models.V2.ProgressBarSettings, ProgressBar>()
                    .ForMember(dest => dest.HoverColor, opt => opt.Ignore());
                cfg.CreateMap<Models.V2.Settings, ProfileV3>()
                    .ForMember(dest => dest.AlbumArtPopupSettings, opt => opt.MapFrom(source => source.AlbumArtPopupSettings))
                    .ForMember(dest => dest.AlbumArtSettings, opt => opt.MapFrom(source => source.AlbumArtSettings))
                    .ForMember(dest => dest.AudioBandSettings, opt => opt.MapFrom(source => source.AudioBandSettings))
                    .ForMember(dest => dest.CustomLabelSettings, opt => opt.MapFrom(source => source.CustomLabelSettings))
                    .ForMember(dest => dest.NextButtonSettings, opt => opt.MapFrom(source => source.NextButtonSettings))
                    .ForMember(dest => dest.PlayPauseButtonSettings, opt => opt.MapFrom(source => source.PlayPauseButtonSettings))
                    .ForMember(dest => dest.PreviousButtonSettings, opt => opt.MapFrom(source => source.PreviousButtonSettings))
                    .ForMember(dest => dest.ProgressBarSettings, opt => opt.MapFrom(source => source.ProgressBarSettings))
                    .ForMember(dest => dest.RepeatModeButtonSettings, opt => opt.MapFrom(source => new RepeatModeButton()));
                cfg.CreateMap<Models.V2.Settings, Dictionary<string, ProfileV3>>().ConvertUsing<ProfilesConverter>();
                cfg.CreateMap<Models.V2.Settings, SettingsV3>()
                    .ForMember(dest => dest.Version, opt => opt.Ignore())
                    .ForMember(dest => dest.AudioSourceSettings, opt => opt.MapFrom(source => source.AudioSourceSettings))
                    .ForMember(dest => dest.Profiles, opt => opt.MapFrom(source => source))
                    .ForMember(dest => dest.CurrentProfileName, opt => opt.MapFrom(source => SettingsV3.DefaultProfileName));
            });

#if DEBUG
            config.AssertConfigurationIsValid();
#endif
            return config;
        }

        private class ProfilesConverter : ITypeConverter<Models.V2.Settings, Dictionary<string, ProfileV3>>
        {
            public Dictionary<string, ProfileV3> Convert(Models.V2.Settings source, Dictionary<string, ProfileV3> destination, ResolutionContext context)
            {
                var profile = context.Mapper.Map<Models.V2.Settings, ProfileV3>(source);
                return new Dictionary<string, ProfileV3> { { SettingsV3.DefaultProfileName, profile } };
            }
        }
    }
}
