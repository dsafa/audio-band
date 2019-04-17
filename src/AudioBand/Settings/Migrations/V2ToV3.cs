using System;
using System.Collections.Generic;
using AudioBand.Models;
using AudioBand.Settings.Models.v3;
using AutoMapper;

namespace AudioBand.Settings.Migrations
{
    /// <summary>
    /// Migrates settings from v2 to v3
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
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Models.V2.CustomLabelSettings, CustomLabel>();
                cfg.CreateMap<Models.V2.AudioSourceSettings, AudioSourceSettings>();
                cfg.CreateMap<Models.V2.AudioSourceSetting, AudioSourceSetting>()
                    .ForMember(dest => dest.Remember, opt => opt.Ignore());
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
