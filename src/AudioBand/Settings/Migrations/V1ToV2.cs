using System;
using AutoMapper;
using V1Settings = AudioBand.Settings.Models.V1.AudioBandSettings;
using V2Settings = AudioBand.Settings.Models.V2.Settings;

namespace AudioBand.Settings.Migrations
{
    /// <summary>
    /// Migrates settings from version 1 to version 2.
    /// </summary>
    internal class V1ToV2 : ISettingsMigrator
    {
        private static readonly MapperConfiguration MapConfig;

        static V1ToV2()
        {
            MapConfig = new MapperConfiguration(c =>
            {
                c.CreateMap<Models.V1.AlbumArtPopupAppearance, Models.V2.AlbumArtPopupSettings>()
                    .ForMember(dest => dest.XPosition, opts => opts.MapFrom(source => source.XOffset));
                c.CreateMap<Models.V1.TextAppearance, Models.V2.CustomLabelSettings>();
                c.CreateMap<Models.V1.AudioSourceSettingsCollection, Models.V2.AudioSourceSettings>()
                    .ForMember(dest => dest.AudioSourceName, opts => opts.MapFrom(source => source.Name));
                c.CreateMap<V1Settings, V2Settings>()
                    .ForMember(dest => dest.AlbumArtPopupSettings, opts => opts.MapFrom(source => source.AlbumArtPopupAppearance))
                    .ForMember(dest => dest.AlbumArtSettings, opts => opts.MapFrom(source => source.AlbumArtAppearance))
                    .ForMember(dest => dest.AudioBandSettings, opts => opts.MapFrom(source => source.AudioBandAppearance))
                    .ForMember(dest => dest.NextButtonSettings, opts => opts.MapFrom(source => source.NextSongButtonAppearance))
                    .ForMember(dest => dest.PlayPauseButtonSettings, opts => opts.MapFrom(source => source.PlayPauseButtonAppearance))
                    .ForMember(dest => dest.PreviousButtonSettings, opts => opts.MapFrom(source => source.PreviousSongButtonAppearance))
                    .ForMember(dest => dest.ProgressBarSettings, opts => opts.MapFrom(source => source.ProgressBarAppearance))
                    .ForMember(dest => dest.CustomLabelSettings, opts => opts.MapFrom(source => source.TextAppearances))
                    .ForMember(dest => dest.Version, opts => opts.Ignore());
            });
        }

        /// <inheritdoc/>
        public object MigrateSetting(object oldSetting)
        {
            var source = oldSetting as V1Settings;
            if (source == null)
            {
                throw new ArgumentException($"{oldSetting} is not of type {typeof(V1Settings)}");
            }

            return MapConfig.CreateMapper().Map<V1Settings, V2Settings>(source);
        }
    }
}
