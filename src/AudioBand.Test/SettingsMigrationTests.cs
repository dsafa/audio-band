using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.Settings.Migrations;
using AudioBand.Settings.Models.v3;
using AudioBand.Settings.Models.V1;
using V1Settings = AudioBand.Settings.Models.V1.AudioBandSettings;
using V2Settings = AudioBand.Settings.Models.V2.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nett;
using AudioSourceSetting = AudioBand.Settings.Models.V1.AudioSourceSetting;

namespace AudioBand.Test
{
    [TestClass]
    public class SettingsMigrationTests
    {
        [TestMethod]
        public void MigrateV1ToV2_Main()
        {
            var v1 = new V1Settings()
            {
                AudioSource = "test"
            };

            var v2 = Migration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.AreEqual(v2.Version, "2");
            Assert.AreEqual(v2.AudioSource, "test");
        }

        [TestMethod]
        public void MigrateV1ToV2_AlbumArtPopup()
        {
            var setting = new AlbumArtPopupAppearance
            {
                Width = 100,
                Height = 50,
                IsVisible = true,
                Margin = 100,
                XOffset = 50
            };

            var v1 = new V1Settings()
            {
                AlbumArtPopupAppearance = setting,
            };

            var v2 = Migration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.AreEqual(v2.AlbumArtPopupSettings.Width, setting.Width);
            Assert.AreEqual(v2.AlbumArtPopupSettings.Height, setting.Height);
            Assert.AreEqual(v2.AlbumArtPopupSettings.IsVisible, setting.IsVisible);
            Assert.AreEqual(v2.AlbumArtPopupSettings.Margin, setting.Margin);
            Assert.AreEqual(v2.AlbumArtPopupSettings.XPosition, setting.XOffset);
        }

        [TestMethod]
        public void MigrateV1ToV2_AlbumArt()
        {
            var setting = new AlbumArtAppearance
            {
                Width = 10,
                Height = 10,
                IsVisible = true,
                XPosition = 10,
                YPosition = 10,
                PlaceholderPath = "test"
            };

            var v1 = new V1Settings
            {
                AlbumArtAppearance = setting
            };

            var v2 = Migration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.AreEqual(v2.AlbumArtSettings.Width, setting.Width);
            Assert.AreEqual(v2.AlbumArtSettings.Height, setting.Height);
            Assert.AreEqual(v2.AlbumArtSettings.IsVisible, setting.IsVisible);
            Assert.AreEqual(v2.AlbumArtSettings.XPosition, setting.XPosition);
            Assert.AreEqual(v2.AlbumArtSettings.YPosition, setting.YPosition);
            Assert.AreEqual(v2.AlbumArtSettings.PlaceholderPath, setting.PlaceholderPath);
        }

        [TestMethod]
        public void MigrateV1ToV2_Audioband()
        {
            var setting = new AudioBandAppearance
            {
                Height = 20,
                Width = 50,
            };

            var v1 = new V1Settings
            {
                AudioBandAppearance = setting
            };

            var v2 = Migration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.AreEqual(v2.AudioBandSettings.Width, setting.Width);
            Assert.AreEqual(v2.AudioBandSettings.Height, setting.Height);
        }

        [TestMethod]
        public void MigrateV1ToV2_NextSong()
        {
            var setting = new NextSongButtonAppearance
            {
                Width = 20,
                Height = 20,
                XPosition = 10,
                IsVisible = false,
                YPosition = 30,
                ImagePath = "path"
            };

            var v1 = new V1Settings
            {
                NextSongButtonAppearance = setting
            };

            var v2 = Migration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.AreEqual(v2.NextButtonSettings.Width, setting.Width);
            Assert.AreEqual(v2.NextButtonSettings.Height, setting.Height);
            Assert.AreEqual(v2.NextButtonSettings.IsVisible, setting.IsVisible);
            Assert.AreEqual(v2.NextButtonSettings.XPosition, setting.XPosition);
            Assert.AreEqual(v2.NextButtonSettings.YPosition, setting.YPosition);
            Assert.AreEqual(v2.NextButtonSettings.ImagePath, setting.ImagePath);
        }

        [TestMethod]
        public void MigrateV1ToV2_AudioSourceSettings()
        {
            var setting1 = new AudioSourceSettingsCollection
            {
                Name = "test",
                Settings = new List<AudioSourceSetting> {new AudioSourceSetting {Name = "key1", Value = "val1"}}
            };

            var setting2 = new AudioSourceSettingsCollection
            {
                Name = "test2",
                Settings = new List<AudioSourceSetting> { new AudioSourceSetting { Name = "key2", Value = "val2" } }
            };

            var settings = new List<AudioSourceSettingsCollection> {setting1,setting2};
            var v1 = new V1Settings
            {
                AudioSourceSettings = settings
            };

            var v2 = Migration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.AreEqual(v2.AudioSourceSettings.Count, settings.Count);
            Assert.AreEqual(v2.AudioSourceSettings[0].AudioSourceName, setting1.Name);
            Assert.AreEqual(v2.AudioSourceSettings[0].Settings.Count, setting1.Settings.Count);
            Assert.AreEqual(v2.AudioSourceSettings[0].Settings[0].Name, setting1.Settings[0].Name);
            Assert.AreEqual(v2.AudioSourceSettings[0].Settings[0].Value, setting1.Settings[0].Value);

            Assert.AreEqual(v2.AudioSourceSettings[1].AudioSourceName, setting2.Name);
            Assert.AreEqual(v2.AudioSourceSettings[1].Settings.Count, setting2.Settings.Count);
            Assert.AreEqual(v2.AudioSourceSettings[1].Settings[0].Name, setting2.Settings[0].Name);
            Assert.AreEqual(v2.AudioSourceSettings[1].Settings[0].Value, setting2.Settings[0].Value);
        }

        [TestMethod]
        public void MigrateV1ToV2_PlayPauseButton()
        {
            var setting = new PlayPauseButtonAppearance
            {
                Width = 1,
                Height = 2,
                XPosition = 3,
                IsVisible = true,
                YPosition = 4,
                PauseButtonImagePath = "pause",
                PlayButtonImagePath = "play"
            };

            var v1 = new V1Settings {PlayPauseButtonAppearance = setting};
            var v2 = Migration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.AreEqual(v2.PlayPauseButtonSettings.Width, setting.Width);
            Assert.AreEqual(v2.PlayPauseButtonSettings.Height, setting.Height);
            Assert.AreEqual(v2.PlayPauseButtonSettings.XPosition, setting.XPosition);
            Assert.AreEqual(v2.PlayPauseButtonSettings.YPosition, setting.YPosition);
            Assert.AreEqual(v2.PlayPauseButtonSettings.IsVisible, setting.IsVisible);
            Assert.AreEqual(v2.PlayPauseButtonSettings.PauseButtonImagePath, setting.PauseButtonImagePath);
            Assert.AreEqual(v2.PlayPauseButtonSettings.PlayButtonImagePath, setting.PlayButtonImagePath);
        }

        [TestMethod]
        public void MigrateV1ToV2_PreviousButton()
        {
            var setting = new PreviousSongButtonAppearance
            {
                Width = 1,
                Height = 2,
                XPosition = 3,
                IsVisible = true,
                YPosition = 4,
                ImagePath = "path"
            };

            var v1 = new V1Settings {PreviousSongButtonAppearance = setting};
            var v2 = Migration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.AreEqual(v2.PreviousButtonSettings.Height, setting.Height);
            Assert.AreEqual(v2.PreviousButtonSettings.Width, setting.Width);
            Assert.AreEqual(v2.PreviousButtonSettings.XPosition, setting.XPosition);
            Assert.AreEqual(v2.PreviousButtonSettings.YPosition, setting.YPosition);
            Assert.AreEqual(v2.PreviousButtonSettings.IsVisible, setting.IsVisible);
            Assert.AreEqual(v2.PreviousButtonSettings.ImagePath, setting.ImagePath);
        }

        [TestMethod]
        public void MigrateV1ToV2_ProgressBar()
        {
            var setting = new ProgressBarAppearance
            {
                Height = 1,
                Width = 2,
                XPosition = 3,
                IsVisible = false,
                YPosition = 5,
                BackgroundColor = Color.White,
                ForegroundColor = Color.Red,
            };

            var v1 = new V1Settings {ProgressBarAppearance = setting};
            var v2 = Migration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.AreEqual(v2.ProgressBarSettings.Width, setting.Width);
            Assert.AreEqual(v2.ProgressBarSettings.Height, setting.Height);
            Assert.AreEqual(v2.ProgressBarSettings.XPosition, setting.XPosition);
            Assert.AreEqual(v2.ProgressBarSettings.YPosition, setting.YPosition);
            Assert.AreEqual(v2.ProgressBarSettings.IsVisible, setting.IsVisible);
            Assert.AreEqual(v2.ProgressBarSettings.BackgroundColor, setting.BackgroundColor);
            Assert.AreEqual(v2.ProgressBarSettings.ForegroundColor, setting.ForegroundColor);
        }

        [TestMethod]
        public void MigrateV1ToV2_CustomText()
        {
            var text1 = new TextAppearance
            {
                Color = Color.Red,
                Width = 1,
                Height = 2,
                Name = "test",
                XPosition = 4,
                IsVisible = true,
                YPosition = 10,
                ScrollSpeed = 20,
                FontSize = 1f,
                FontFamily = "family",
                Alignment = CustomLabel.TextAlignment.Center,
                FormatString = "123",
            };

            var texts = new List<TextAppearance>() {text1};
            var v1 = new V1Settings {TextAppearances = texts};
            var v2 = Migration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.AreEqual(v2.CustomLabelSettings.Count, texts.Count);
            Assert.AreEqual(v2.CustomLabelSettings[0].Color, text1.Color);
            Assert.AreEqual(v2.CustomLabelSettings[0].Width, text1.Width);
            Assert.AreEqual(v2.CustomLabelSettings[0].Height, text1.Height);
            Assert.AreEqual(v2.CustomLabelSettings[0].Name, text1.Name);
            Assert.AreEqual(v2.CustomLabelSettings[0].XPosition, text1.XPosition);
            Assert.AreEqual(v2.CustomLabelSettings[0].IsVisible, text1.IsVisible);
            Assert.AreEqual(v2.CustomLabelSettings[0].YPosition, text1.YPosition);
            Assert.AreEqual(v2.CustomLabelSettings[0].ScrollSpeed, text1.ScrollSpeed);
            Assert.AreEqual(v2.CustomLabelSettings[0].FontSize, text1.FontSize);
            Assert.AreEqual(v2.CustomLabelSettings[0].FontFamily, text1.FontFamily);
            Assert.AreEqual(v2.CustomLabelSettings[0].Alignment, text1.Alignment);
            Assert.AreEqual(v2.CustomLabelSettings[0].FormatString, text1.FormatString);
        }

        [TestMethod]
        public void MigrateV2ToV3()
        {
            string settingsFile = @"Version = ""2""
AudioSource = ""Spotify""

[AudioBandSettings]
Width = 500
Height = 30

[PreviousButtonSettings]
ImagePath = """"
IsVisible = false
Width = 73
Height = 12
XPosition = 30
YPosition = 15

[PlayPauseButtonSettings]
PlayButtonImagePath = """"
PauseButtonImagePath = """"
XPosition = 109
YPosition = 15
Width = 73
Height = 12
IsVisible = false

[NextButtonSettings]
ImagePath = """"
IsVisible = false
Width = 73
Height = 12
XPosition = 176
YPosition = 15

[ProgressBarSettings]
ForegroundColor = ""#32ABCD""
BackgroundColor = ""#232323""
IsVisible = true
XPosition = 330
YPosition = 24
Width = 130
Height = 3

[AlbumArtSettings]
IsVisible = true
Width = 30
Height = 30
XPosition = 260
YPosition = 0
PlaceholderPath = """"

[AlbumArtPopupSettings]
IsVisible = true
Width = 300
Height = 300
XPosition = 125
Margin = 4

[[CustomLabelSettings]]
IsVisible = true
Width = 200
Height = 20
XPosition = 295
YPosition = 0
FontFamily = ""Segoe UI""
FontSize = 11.0
Color = ""#FFFFFF""
FormatString = ""{*song}""
Alignment = ""Center""
Name = ""Song""
ScrollSpeed = 50
[[CustomLabelSettings]]
IsVisible = true
Width = 34
Height = 12
XPosition = 292
YPosition = 17
FontFamily = ""Segoe UI""
FontSize = 8.0
Color = ""#C3C3C3""
FormatString = ""{time}""
Alignment = ""Left""
Name = ""Time""
ScrollSpeed = 50
[[CustomLabelSettings]]
IsVisible = true
Width = 35
Height = 12
XPosition = 460
YPosition = 17
FontFamily = ""Segoe UI""
FontSize = 8.0
Color = ""#C3C3C3""
FormatString = ""{length}""
Alignment = ""Right""
Name = ""Song Length""
ScrollSpeed = 50
[[CustomLabelSettings]]
IsVisible = true
Width = 260
Height = 15
XPosition = 0
YPosition = 0
FontFamily = ""Segoe UI""
FontSize = 9.0
Color = ""White""
FormatString = ""{artist}""
Alignment = ""Right""
Name = ""Artist""
ScrollSpeed = 50
[[CustomLabelSettings]]
IsVisible = true
Width = 260
Height = 15
XPosition = 0
YPosition = 15
FontFamily = ""Segoe UI""
FontSize = 9.0
Color = ""#AFAFAF""
FormatString = ""{album}""
Alignment = ""Right""
Name = ""Album""
ScrollSpeed = 50

[[AudioSourceSettings]]
AudioSourceName = ""Spotify""

[[AudioSourceSettings.Settings]]
Name = ""Spotify Client ID""
Value = ""id""
[[AudioSourceSettings.Settings]]
Name = ""Spotify Client secret""
Value = ""secret""
";
            var settings = TomlSettings.Create(cfg =>
            {
                cfg.ConfigureType<Color>(type => type.WithConversionFor<TomlString>(convert => convert
                    .ToToml(ColorTranslator.ToHtml)
                    .FromToml(tomlString => ColorTranslator.FromHtml(tomlString.Value))));
                cfg.ConfigureType<CustomLabel.TextAlignment>(type => type.WithConversionFor<TomlString>(convert => convert
                    .ToToml(SerializationConversions.EnumToString)
                    .FromToml(str => SerializationConversions.StringToEnum<CustomLabel.TextAlignment>(str.Value))));
            });

            var v2 = Toml.ReadString<V2Settings>(settingsFile, settings);
            var v3 = Migration.MigrateSettings<SettingsV3>(v2, "2", "3");

            Assert.AreEqual("3", v3.Version);
            Assert.AreEqual(v2.AudioSource, v3.AudioSource);

            Assert.AreEqual(v2.AlbumArtPopupSettings.Width, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtPopupSettings.Width);
            Assert.AreEqual(v2.AlbumArtPopupSettings.Height, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtPopupSettings.Height);
            Assert.AreEqual(v2.AlbumArtPopupSettings.IsVisible, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtPopupSettings.IsVisible);
            Assert.AreEqual(v2.AlbumArtPopupSettings.Margin, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtPopupSettings.Margin);
            Assert.AreEqual(v2.AlbumArtPopupSettings.XPosition, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtPopupSettings.XPosition);

            Assert.AreEqual(v2.AlbumArtSettings.Width, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtSettings.Width);
            Assert.AreEqual(v2.AlbumArtSettings.Height, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtSettings.Height);
            Assert.AreEqual(v2.AlbumArtSettings.IsVisible, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtSettings.IsVisible);
            Assert.AreEqual(v2.AlbumArtSettings.XPosition, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtSettings.XPosition);
            Assert.AreEqual(v2.AlbumArtSettings.YPosition, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtSettings.YPosition);
            Assert.AreEqual(v2.AlbumArtSettings.PlaceholderPath, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtSettings.PlaceholderPath);

            Assert.AreEqual(v2.AudioBandSettings.Width, v3.Profiles[SettingsV3.DefaultProfileName].AudioBandSettings.Width);
            Assert.AreEqual(v2.AudioBandSettings.Height, v3.Profiles[SettingsV3.DefaultProfileName].AudioBandSettings.Height);

            Assert.AreEqual(v2.NextButtonSettings.Width, v3.Profiles[SettingsV3.DefaultProfileName].NextButtonSettings.Width);
            Assert.AreEqual(v2.NextButtonSettings.Height, v3.Profiles[SettingsV3.DefaultProfileName].NextButtonSettings.Height);
            Assert.AreEqual(v2.NextButtonSettings.IsVisible, v3.Profiles[SettingsV3.DefaultProfileName].NextButtonSettings.IsVisible);
            Assert.AreEqual(v2.NextButtonSettings.XPosition, v3.Profiles[SettingsV3.DefaultProfileName].NextButtonSettings.XPosition);
            Assert.AreEqual(v2.NextButtonSettings.YPosition, v3.Profiles[SettingsV3.DefaultProfileName].NextButtonSettings.YPosition);
            Assert.AreEqual(v2.NextButtonSettings.ImagePath, v3.Profiles[SettingsV3.DefaultProfileName].NextButtonSettings.ImagePath);

            Assert.AreEqual(v2.AudioSourceSettings.Count, v3.AudioSourceSettings.Count);
            Assert.AreEqual(v2.AudioSourceSettings[0].AudioSourceName, v3.AudioSourceSettings[0].AudioSourceName);
            Assert.AreEqual(v2.AudioSourceSettings[0].Settings.Count, v3.AudioSourceSettings[0].Settings.Count);
            Assert.AreEqual(v2.AudioSourceSettings[0].Settings[0].Name, v3.AudioSourceSettings[0].Settings[0].Name);
            Assert.AreEqual(v2.AudioSourceSettings[0].Settings[0].Value, v3.AudioSourceSettings[0].Settings[0].Value);

            Assert.AreEqual(v2.PlayPauseButtonSettings.Width, v3.Profiles[SettingsV3.DefaultProfileName].PlayPauseButtonSettings.Width);
            Assert.AreEqual(v2.PlayPauseButtonSettings.Height, v3.Profiles[SettingsV3.DefaultProfileName].PlayPauseButtonSettings.Height);
            Assert.AreEqual(v2.PlayPauseButtonSettings.XPosition, v3.Profiles[SettingsV3.DefaultProfileName].PlayPauseButtonSettings.XPosition);
            Assert.AreEqual(v2.PlayPauseButtonSettings.YPosition, v3.Profiles[SettingsV3.DefaultProfileName].PlayPauseButtonSettings.YPosition);
            Assert.AreEqual(v2.PlayPauseButtonSettings.IsVisible, v3.Profiles[SettingsV3.DefaultProfileName].PlayPauseButtonSettings.IsVisible);
            Assert.AreEqual(v2.PlayPauseButtonSettings.PauseButtonImagePath, v3.Profiles[SettingsV3.DefaultProfileName].PlayPauseButtonSettings.PauseButtonImagePath);
            Assert.AreEqual(v2.PlayPauseButtonSettings.PlayButtonImagePath, v3.Profiles[SettingsV3.DefaultProfileName].PlayPauseButtonSettings.PlayButtonImagePath);

            Assert.AreEqual(v2.PreviousButtonSettings.Height, v3.Profiles[SettingsV3.DefaultProfileName].PreviousButtonSettings.Height);
            Assert.AreEqual(v2.PreviousButtonSettings.Width, v3.Profiles[SettingsV3.DefaultProfileName].PreviousButtonSettings.Width);
            Assert.AreEqual(v2.PreviousButtonSettings.XPosition, v3.Profiles[SettingsV3.DefaultProfileName].PreviousButtonSettings.XPosition);
            Assert.AreEqual(v2.PreviousButtonSettings.YPosition, v3.Profiles[SettingsV3.DefaultProfileName].PreviousButtonSettings.YPosition);
            Assert.AreEqual(v2.PreviousButtonSettings.IsVisible, v3.Profiles[SettingsV3.DefaultProfileName].PreviousButtonSettings.IsVisible);
            Assert.AreEqual(v2.PreviousButtonSettings.ImagePath, v3.Profiles[SettingsV3.DefaultProfileName].PreviousButtonSettings.ImagePath);

            Assert.AreEqual(v2.ProgressBarSettings.Width, v3.Profiles[SettingsV3.DefaultProfileName].ProgressBarSettings.Width);
            Assert.AreEqual(v2.ProgressBarSettings.Height, v3.Profiles[SettingsV3.DefaultProfileName].ProgressBarSettings.Height);
            Assert.AreEqual(v2.ProgressBarSettings.XPosition, v3.Profiles[SettingsV3.DefaultProfileName].ProgressBarSettings.XPosition);
            Assert.AreEqual(v2.ProgressBarSettings.YPosition, v3.Profiles[SettingsV3.DefaultProfileName].ProgressBarSettings.YPosition);
            Assert.AreEqual(v2.ProgressBarSettings.IsVisible, v3.Profiles[SettingsV3.DefaultProfileName].ProgressBarSettings.IsVisible);
            Assert.AreEqual(v2.ProgressBarSettings.BackgroundColor, v3.Profiles[SettingsV3.DefaultProfileName].ProgressBarSettings.BackgroundColor);
            Assert.AreEqual(v2.ProgressBarSettings.ForegroundColor, v3.Profiles[SettingsV3.DefaultProfileName].ProgressBarSettings.ForegroundColor);

            Assert.AreEqual(v2.CustomLabelSettings.Count, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings.Count);
            Assert.AreEqual(v2.CustomLabelSettings[0].Color, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].Color);
            Assert.AreEqual(v2.CustomLabelSettings[0].Width, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].Width);
            Assert.AreEqual(v2.CustomLabelSettings[0].Height, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].Height);
            Assert.AreEqual(v2.CustomLabelSettings[0].Name, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].Name);
            Assert.AreEqual(v2.CustomLabelSettings[0].XPosition, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].XPosition);
            Assert.AreEqual(v2.CustomLabelSettings[0].IsVisible, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].IsVisible);
            Assert.AreEqual(v2.CustomLabelSettings[0].YPosition, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].YPosition);
            Assert.AreEqual(v2.CustomLabelSettings[0].ScrollSpeed, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].ScrollSpeed);
            Assert.AreEqual(v2.CustomLabelSettings[0].FontSize, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].FontSize);
            Assert.AreEqual(v2.CustomLabelSettings[0].FontFamily, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].FontFamily);
            Assert.AreEqual(v2.CustomLabelSettings[0].Alignment, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].Alignment);
            Assert.AreEqual(v2.CustomLabelSettings[0].FormatString, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].FormatString);
        }
    }
}
