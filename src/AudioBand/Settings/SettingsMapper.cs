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
                cfg.CreateMap<AlbumArtPopupSettings, AlbumArtPopup>();
                cfg.CreateMap<AudioBandSettings, AudioBand.Models.AudioBand>();
                cfg.CreateMap<AudioBand.Settings.Models.v2.AudioSourceSetting, AudioBand.Models.AudioSourceSetting>()
                    .ForMember(m => m.Value, c => c.MapFrom(s => s.Value));
                cfg.CreateMap<AudioBand.Settings.Models.v2.AudioSourceSettings, AudioBand.Models.AudioSourceSettings>();
                cfg.CreateMap<CustomLabelSettings, CustomLabel>();
                cfg.CreateMap<NextButtonSettings, NextButton>();
                cfg.CreateMap<PlayPauseButtonSettings, PlayPauseButton>();
                cfg.CreateMap<PreviousButtonSettings, PreviousButton>();
                cfg.CreateMap<ProgressBarSettings, ProgressBar>();
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
    }
}
