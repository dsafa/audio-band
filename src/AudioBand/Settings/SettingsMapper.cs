using System.Linq;
using AudioBand.Models;
using AudioBand.Settings.Models.v2;
using AutoMapper;

namespace AudioBand.Settings
{
    /// <summary>
    /// Maps settings to domain models
    /// </summary>
    internal static class SettingsMapper
    {
        private static readonly MapperConfiguration MapperConfig;

        static SettingsMapper()
        {
            MapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlbumArtSettings, AlbumArt>();
                cfg.CreateMap<AlbumArt, AlbumArtSettings>();

                cfg.CreateMap<AlbumArtPopupSettings, AlbumArtPopup>();
                cfg.CreateMap<AlbumArtPopup, AlbumArtSettings>();

                cfg.CreateMap<AudioBandSettings, AudioBand.Models.AudioBand>();
                cfg.CreateMap<AudioBand.Models.AudioBand, AudioBandSettings>();

                cfg.CreateMap<AudioBand.Settings.Models.v2.AudioSourceSetting, AudioBand.Models.AudioSourceSetting>()
                    .ForMember(m => m.Value, c => c.MapFrom(s => s.Value));
                cfg.CreateMap<AudioBand.Models.AudioSourceSetting, AudioBand.Settings.Models.v2.AudioSourceSetting>()
                    .ForMember(m => m.Value, c => c.MapFrom(s => s.Remember ? s.Value.ToString() : null));

                cfg.CreateMap<AudioBand.Settings.Models.v2.AudioSourceSettings, AudioBand.Models.AudioSourceSettings>();
                cfg.CreateMap<AudioBand.Models.AudioSourceSettings, AudioBand.Settings.Models.v2.AudioSourceSettings>()
                    .ForMember(m => m.Settings, c => c.MapFrom(s => s.Settings.Where(se => se.Remember)));

                cfg.CreateMap<CustomLabelSettings, CustomLabel>();
                cfg.CreateMap<CustomLabel, CustomLabelSettings>();

                cfg.CreateMap<NextButtonSettings, NextButton>();
                cfg.CreateMap<NextButton, NextButtonSettings>();

                cfg.CreateMap<PlayPauseButtonSettings, PlayPauseButton>();
                cfg.CreateMap<PlayPauseButton, PlayPauseButtonSettings>();

                cfg.CreateMap<PreviousButtonSettings, PreviousButton>();
                cfg.CreateMap<PreviousButton, PreviousButtonSettings>();

                cfg.CreateMap<ProgressBarSettings, ProgressBar>();
                cfg.CreateMap<ProgressBar, ProgressBarSettings>();
            });
        }

        /// <summary>
        /// Convert setting to a model.
        /// </summary>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <param name="source">Source to convert.</param>
        /// <returns>The converted model</returns>
        public static TModel ToModel<TModel>(object source)
        {
            return MapperConfig.CreateMapper().Map<TModel>(source);
        }

        public static T ToSettings<T>(object source)
        {
            return MapperConfig.CreateMapper().Map<T>(source);
        }
    }
}
