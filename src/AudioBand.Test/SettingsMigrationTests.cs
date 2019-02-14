using System.Collections.Generic;
using System.Drawing;
using AudioBand.Models;
using AudioBand.Settings.Migrations;
using AudioBand.Settings.Models.V1;
using V1Settings = AudioBand.Settings.Models.V1.AudioBandSettings;
using V2Settings = AudioBand.Settings.Models.V2.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
