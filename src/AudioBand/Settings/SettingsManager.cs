using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nett;
using NLog;

namespace AudioBand.Settings
{
    internal class SettingsManager
    {
        public static string SettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioBand");
        public static string SettingsFile = Path.Combine(SettingsDirectory, "settings.toml");

        public AppSettings AppSettings { get; }

        private const string SettingsKey = "AudioBand";
        private readonly TomlTable _root;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

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

            _root = Toml.ReadFile(SettingsFile);
            try
            {
                AppSettings = _root.Get<AppSettings>(SettingsKey);
            }
            catch (KeyNotFoundException)
            {
                AppSettings = new AppSettings();
                _root[SettingsKey] = Toml.Create(AppSettings);
            }

        }

        public void Save()
        {
            try
            {
                _root[SettingsKey] = Toml.Create(AppSettings);
                Toml.WriteFile(_root, SettingsFile);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }
    }
}
