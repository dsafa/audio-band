using Nett;
using NLog;
using System;
using System.Drawing;
using System.IO;
using AudioBand.Models;

namespace AudioBand.Settings
{
    internal class SettingsManager
    {
        private static readonly string SettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioBand");
        private static readonly string SettingsFilePath = Path.Combine(SettingsDirectory, "audioband.settings");
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly TomlSettings _tomlSettings;
        private readonly Models.v2.Settings _settings;

        public string Version => _settings.Version;
        public string AudioSource
        {
            get => _settings.AudioSource;
            set => _settings.AudioSource = value;
        }

        public SettingsManager()
        {
            _tomlSettings = TomlSettings.Create(cfg =>
            {
                cfg.ConfigureType<Color>(type => type.WithConversionFor<TomlString>(convert => convert
                    .ToToml(ColorTranslator.ToHtml)
                    .FromToml(tomlString => ColorTranslator.FromHtml(tomlString.Value))));
            });
               
            if (!Directory.Exists(SettingsDirectory))
            {
                Directory.CreateDirectory(SettingsDirectory);
            }

            if (!File.Exists(SettingsFilePath))
            {
                _settings = new Models.v2.Settings();

                Toml.WriteFile(_settings, SettingsFilePath, _tomlSettings);
            }
            else
            {
                _settings = Toml.ReadFile<Models.v2.Settings>(SettingsFilePath, _tomlSettings);
            }
        }

        public void Save()
        {
            try
            {
                Toml.WriteFile(_settings, SettingsFilePath, _tomlSettings);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }
    }
}
