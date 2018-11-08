using System;
using System.Collections.Generic;
using AudioBand.Models;
using AutoMapper;

namespace AudioBand.Settings.Models.v2
{
    internal class Settings : ISettings
    {
        public string Version { get; set; } = "2";
        public string AudioSource { get; set; }
        public AudioBandSettings AudioBandSettings { get; set; }
        public PreviousButtonSettings PreviousButtonSettings { get; set; }
        public PlayPauseButtonSettings PlayPauseButtonSettings{ get; set; }
        public NextButtonSettings NextButtonSettings { get; set; }
        public ProgressBarSettings ProgressBarSettings { get; set; }
        public AlbumArtSettings AlbumArtSettings { get; set; }
        public AlbumArtPopupSettings AlbumArtPopupSettings { get; set; }
        public List<CustomLabelSettings> CustomLabelSettings { get; set; }
        public List<AudioSourceSettings> AudioSourceSettings { get; set; }

        private static readonly MapperConfiguration MapperConfig;

        static Settings()
        {
            MapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlbumArtSettings, AlbumArt>();
                cfg.CreateMap<AlbumArtPopupSettings, AlbumArtPopup>();
                cfg.CreateMap<AudioBandSettings, AudioBand.Models.AudioBand>();
                cfg.CreateMap<AudioBand.Settings.Models.v2.AudioSourceSetting, AudioBand.Models.AudioSourceSetting>();
                cfg.CreateMap<AudioBand.Settings.Models.v2.AudioSourceSettings, AudioBand.Models.AudioSourceSettings>();
                cfg.CreateMap<CustomLabelSettings, CustomLabel>();
                cfg.CreateMap<NextButtonSettings, NextButton>();
                cfg.CreateMap<PlayPauseButtonSettings, PlayPauseButton>();
                cfg.CreateMap<PreviousButtonSettings, PreviousButton>();
                cfg.CreateMap<ProgressBarSettings, ProgressBar>();
            });
        }

        public T GetModel<T>()
        {
            var type = typeof(T);
            object source;

            if (type == typeof(AlbumArt)) source = AlbumArtSettings;
            else if (type == typeof(AlbumArtPopup)) source = AlbumArtPopupSettings;
            else if (type == typeof(AudioBand.Models.AudioBand)) source = AudioBandSettings;
            else if (type == typeof(AudioBand.Models.AudioSourceSettings)) source = AudioSourceSettings;
            else if (type == typeof(CustomLabel)) source = CustomLabelSettings;
            else if (type == typeof(NextButton)) source = NextButtonSettings;
            else if (type == typeof(PlayPauseButton)) source = PlayPauseButtonSettings;
            else if (type == typeof(PreviousButton)) source = PreviousButtonSettings;
            else if (type == typeof(ProgressBar)) source = ProgressBarSettings;
            else throw new InvalidOperationException($"{type} could not be converted");

            return MapperConfig.CreateMapper().Map<T>(source);
        }
    }
}
