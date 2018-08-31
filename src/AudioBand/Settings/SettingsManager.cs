using Nett;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AudioBand.ViewModels;

namespace AudioBand.Settings
{
    internal class SettingsManager
    {
        public static string SettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioBand");
        public static string SettingsFilePath = Path.Combine(SettingsDirectory, "audioband.settings");

        public Appearance Appearance => _settings.ToModel();
        public string Version => _settings.Version;
        public string AudioSource
        {
            get => _settings.AudioSource;
            set => _settings.AudioSource = value;
        }

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly TomlSettings _tomlSettings;
        private readonly AudioBandSettings _settings;

        public SettingsManager()
        {
            _tomlSettings = TomlSettings.Create(cfg =>
            {
                cfg.ConfigureType<Color>(type => type.WithConversionFor<TomlString>(convert => convert
                    .ToToml(ColorTranslator.ToHtml)
                    .FromToml(tomlInt => ColorTranslator.FromHtml(tomlInt.Value))));

                cfg.ConfigureType<Font>(type => type.WithConversionFor<TomlString>(convert => convert
                    .ToToml(Converters.FontToString)
                    .FromToml(tomlString => Converters.StringToFont(tomlString.Value))));

                cfg.ConfigureType<PlayPauseButtonAppearance>(t =>
                {
                    t.IgnoreProperty(o => o.PlayImage);
                    t.IgnoreProperty(o => o.PauseImage);
                    t.IgnoreProperty(o => o.Image);
                    t.IgnoreProperty(o => o.Location);
                    t.IgnoreProperty(o => o.IsPlaying);
                });

                cfg.ConfigureType<NextSongButtonAppearance>(t =>
                {
                    t.IgnoreProperty(o => o.Location);
                    t.IgnoreProperty(o => o.Image);
                });
                cfg.ConfigureType<PreviousSongButtonAppearance>(t =>
                {
                    t.IgnoreProperty(o => o.Location);
                    t.IgnoreProperty(o => o.Image);
                });
                cfg.ConfigureType<AlbumArtDisplay>(t =>
                {
                    t.IgnoreProperty(o => o.Location);
                    t.IgnoreProperty(o => o.Placeholder);
                    t.IgnoreProperty(o => o.CurrentAlbumArt);
                });
                cfg.ConfigureType<AlbumArtPopup>(t => t.IgnoreProperty(o => o.CurrentAlbumArt));
                cfg.ConfigureType<ProgressBarAppearance>(t => t.IgnoreProperty(o => o.Location));
            });

            if (!Directory.Exists(SettingsDirectory))
            {
                Directory.CreateDirectory(SettingsDirectory);
            }

            if (!File.Exists(SettingsFilePath))
            {
                _settings = new AudioBandSettings();

                Toml.WriteFile<AudioBandSettings>(_settings, SettingsFilePath, _tomlSettings);
            }
            else
            {
                _settings = Toml.ReadFile<AudioBandSettings>(SettingsFilePath, _tomlSettings);
            }
        }

        public void Save()
        {
            try
            {
                Toml.WriteFile<AudioBandSettings>(_settings, SettingsFilePath, _tomlSettings);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }
    }
}
