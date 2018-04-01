using Nett;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace AudioBand.Settings
{
    internal class SettingsManager
    {
        public static string SettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioBand");
        public static string SettingsFile = Path.Combine(SettingsDirectory, "settings.toml");

        public AudioBandSettings AudioBandSettings { get; }

        private const string SettingsKey = "AudioBand";
        private readonly TomlTable _root;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly TomlSettings _tomlSettings;

        public SettingsManager()
        {
            if (!Directory.Exists(SettingsDirectory))
            {
                Directory.CreateDirectory(SettingsDirectory);
            }

            if (!File.Exists(SettingsFile))
            {
                File.CreateText(SettingsFile).Close();
            }

            _tomlSettings = TomlSettings.Create(cfg =>
            {
                cfg.ConfigureType<Color>(type => type.WithConversionFor<TomlString>(convert => convert
                    .ToToml(ColorTranslator.ToHtml)
                    .FromToml(tomlInt => ColorTranslator.FromHtml(tomlInt.Value))));

                cfg.ConfigureType<Font>(type => type.WithConversionFor<TomlString>(convert => convert
                    .ToToml(Converters.FontToString)
                    .FromToml(tomlString => Converters.StringToFont(tomlString.Value))));
            });


            _root = Toml.ReadFile(SettingsFile, _tomlSettings);
            try
            {
                AudioBandSettings = _root.Get<AudioBandSettings>(SettingsKey);
            }
            catch (KeyNotFoundException)
            {
                AudioBandSettings = new AudioBandSettings();
                _root[SettingsKey] = Toml.Create(AudioBandSettings, _tomlSettings);
            }

        }

        public void Save()
        {
            try
            {
                _root[SettingsKey] = Toml.Create(AudioBandSettings, _tomlSettings);
                Toml.WriteFile(_root, SettingsFile, _tomlSettings);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }
    }
}
