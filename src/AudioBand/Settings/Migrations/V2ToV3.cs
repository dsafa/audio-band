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
                    .ForMember(dest => dest.TextOverflow, opt => opt.Ignore());
                cfg.CreateMap<Models.V2.AudioSourceSettings, AudioSourceSettings>();
                cfg.CreateMap<Models.V2.NextButtonSettings, NextButton>()
                    .ForMember(dest => dest.HoveredImagePath, opt => opt.MapFrom(source => source.ImagePath))
                    .ForMember(dest => dest.ClickedImagePath, opt => opt.MapFrom(source => source.ImagePath))
                    .ForMember(dest => dest.BackgroundColor, opt => opt.Ignore())
                    .ForMember(dest => dest.HoveredBackgroundColor, opt => opt.Ignore())
                    .ForMember(dest => dest.ClickedBackgroundColor, opt => opt.Ignore())
                    .ForMember(dest => dest.ContentType, opt => opt.MapFrom(source => ButtonContentType.Image))
                    .ForMember(dest => dest.TextFontFamily, opt => opt.Ignore())
                    .ForMember(dest => dest.Text, opt => opt.Ignore())
                    .ForMember(dest => dest.TextColor, opt => opt.Ignore())
                    .ForMember(dest => dest.TextHoveredColor, opt => opt.Ignore())
                    .ForMember(dest => dest.TextClickedColor, opt => opt.Ignore());
                cfg.CreateMap<Models.V2.PreviousButtonSettings, PreviousButton>()
                    .ForMember(dest => dest.HoveredImagePath, opt => opt.MapFrom(source => source.ImagePath))
                    .ForMember(dest => dest.ClickedImagePath, opt => opt.MapFrom(source => source.ImagePath))
                    .ForMember(dest => dest.BackgroundColor, opt => opt.Ignore())
                    .ForMember(dest => dest.HoveredBackgroundColor, opt => opt.Ignore())
                    .ForMember(dest => dest.ClickedBackgroundColor, opt => opt.Ignore())
                    .ForMember(dest => dest.ContentType, opt => opt.MapFrom(source => ButtonContentType.Image))
                    .ForMember(dest => dest.TextFontFamily, opt => opt.Ignore())
                    .ForMember(dest => dest.Text, opt => opt.Ignore())
                    .ForMember(dest => dest.TextColor, opt => opt.Ignore())
                    .ForMember(dest => dest.TextHoveredColor, opt => opt.Ignore())
                    .ForMember(dest => dest.TextClickedColor, opt => opt.Ignore());
                cfg.CreateMap<Models.V2.PlayPauseButtonSettings, PlayPauseButton>()
                    .ForMember(dest => dest.DefaultBackgroundColor, opt => opt.Ignore())
                    .ForMember(dest => dest.HoveredBackgroundColor, opt => opt.Ignore())
                    .ForMember(dest => dest.ClickedBackgroundColor, opt => opt.Ignore())
                    .ForMember(dest => dest.PlayButtonHoveredImagePath, opt => opt.MapFrom(source => source.PlayButtonImagePath))
                    .ForMember(dest => dest.PlayButtonClickedImagePath, opt => opt.MapFrom(source => source.PlayButtonImagePath))
                    .ForMember(dest => dest.PauseButtonHoveredImagePath, opt => opt.MapFrom(source => source.PauseButtonImagePath))
                    .ForMember(dest => dest.PauseButtonClickedImagePath, opt => opt.MapFrom(source => source.PauseButtonImagePath))
                    .ForMember(dest => dest.PlayButtonContentType, opt => opt.MapFrom(source => ButtonContentType.Image))
                    .ForMember(dest => dest.PauseButtonContentType, opt => opt.MapFrom(source => ButtonContentType.Image))
                    .ForMember(dest => dest.PlayButtonTextFontFamily, opt => opt.Ignore())
                    .ForMember(dest => dest.PauseButtonTextFontFamily, opt => opt.Ignore())
                    .ForMember(dest => dest.PlayButtonText, opt => opt.Ignore())
                    .ForMember(dest => dest.PauseButtonText, opt => opt.Ignore())
                    .ForMember(dest => dest.PlayButtonTextColor, opt => opt.Ignore())
                    .ForMember(dest => dest.PlayButtonTextHoverColor, opt => opt.Ignore())
                    .ForMember(dest => dest.PlayButtonTextClickedColor, opt => opt.Ignore())
                    .ForMember(dest => dest.PauseButtonTextColor, opt => opt.Ignore())
                    .ForMember(dest => dest.PauseButtonTextHoverColor, opt => opt.Ignore())
                    .ForMember(dest => dest.PauseButtonTextClickedColor, opt => opt.Ignore());
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
                    .ForMember(dest => dest.ProgressBarSettings, opt => opt.MapFrom(source => source.ProgressBarSettings));
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
