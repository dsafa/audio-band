using AudioBand.Models;
using AudioBand.Settings.Migrations;
using AudioBand.Settings.Models.V1;
using AudioBand.Settings.Models.V3;
using AudioBand.Settings.Models.V4;
using AudioBand.Settings.Persistence;
using Nett;
using System.Collections.Generic;
using System.Windows.Media;
using Xunit;
using V1AudioSourceSetting = AudioBand.Settings.Models.V1.AudioSourceSetting;
using V1Settings = AudioBand.Settings.Models.V1.AudioBandSettings;
using V2Settings = AudioBand.Settings.Models.V2.SettingsV2;

namespace AudioBand.Test
{
    public class SettingsMigrationTests
    {
        [Fact]
        public void MigrateV1ToV2_Main()
        {
            var v1 = new V1Settings()
            {
                AudioSource = "test"
            };

            var v2 = SettingsMigration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.Equal("2", v2.Version);
            Assert.Equal("test", v2.AudioSource);
        }

        [Fact]
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

            var v2 = SettingsMigration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.Equal(v2.AlbumArtPopupSettings.Width, setting.Width);
            Assert.Equal(v2.AlbumArtPopupSettings.Height, setting.Height);
            Assert.Equal(v2.AlbumArtPopupSettings.IsVisible, setting.IsVisible);
            Assert.Equal(v2.AlbumArtPopupSettings.Margin, setting.Margin);
            Assert.Equal(v2.AlbumArtPopupSettings.XPosition, setting.XOffset);
        }

        [Fact]
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

            var v2 = SettingsMigration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.Equal(v2.AlbumArtSettings.Width, setting.Width);
            Assert.Equal(v2.AlbumArtSettings.Height, setting.Height);
            Assert.Equal(v2.AlbumArtSettings.IsVisible, setting.IsVisible);
            Assert.Equal(v2.AlbumArtSettings.XPosition, setting.XPosition);
            Assert.Equal(v2.AlbumArtSettings.YPosition, setting.YPosition);
            Assert.Equal(v2.AlbumArtSettings.PlaceholderPath, setting.PlaceholderPath);
        }

        [Fact]
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

            var v2 = SettingsMigration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.Equal(v2.AudioBandSettings.Width, setting.Width);
            Assert.Equal(v2.AudioBandSettings.Height, setting.Height);
        }

        [Fact]
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

            var v2 = SettingsMigration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.Equal(v2.NextButtonSettings.Width, setting.Width);
            Assert.Equal(v2.NextButtonSettings.Height, setting.Height);
            Assert.Equal(v2.NextButtonSettings.IsVisible, setting.IsVisible);
            Assert.Equal(v2.NextButtonSettings.XPosition, setting.XPosition);
            Assert.Equal(v2.NextButtonSettings.YPosition, setting.YPosition);
            Assert.Equal(v2.NextButtonSettings.ImagePath, setting.ImagePath);
        }

        [Fact]
        public void MigrateV1ToV2_AudioSourceSettings()
        {
            var setting1 = new AudioSourceSettingsCollection
            {
                Name = "test",
                Settings = new List<V1AudioSourceSetting> { new V1AudioSourceSetting { Name = "key1", Value = "val1" } }
            };

            var setting2 = new AudioSourceSettingsCollection
            {
                Name = "test2",
                Settings = new List<V1AudioSourceSetting> { new V1AudioSourceSetting { Name = "key2", Value = "val2" } }
            };

            var settings = new List<AudioSourceSettingsCollection> { setting1, setting2 };
            var v1 = new V1Settings
            {
                AudioSourceSettings = settings
            };

            var v2 = SettingsMigration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.Equal(v2.AudioSourceSettings.Count, settings.Count);
            Assert.Equal(v2.AudioSourceSettings[0].AudioSourceName, setting1.Name);
            Assert.Equal(v2.AudioSourceSettings[0].Settings.Count, setting1.Settings.Count);
            Assert.Equal(v2.AudioSourceSettings[0].Settings[0].Name, setting1.Settings[0].Name);
            Assert.Equal(v2.AudioSourceSettings[0].Settings[0].Value, setting1.Settings[0].Value);

            Assert.Equal(v2.AudioSourceSettings[1].AudioSourceName, setting2.Name);
            Assert.Equal(v2.AudioSourceSettings[1].Settings.Count, setting2.Settings.Count);
            Assert.Equal(v2.AudioSourceSettings[1].Settings[0].Name, setting2.Settings[0].Name);
            Assert.Equal(v2.AudioSourceSettings[1].Settings[0].Value, setting2.Settings[0].Value);
        }

        [Fact]
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

            var v1 = new V1Settings { PlayPauseButtonAppearance = setting };
            var v2 = SettingsMigration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.Equal(v2.PlayPauseButtonSettings.Width, setting.Width);
            Assert.Equal(v2.PlayPauseButtonSettings.Height, setting.Height);
            Assert.Equal(v2.PlayPauseButtonSettings.XPosition, setting.XPosition);
            Assert.Equal(v2.PlayPauseButtonSettings.YPosition, setting.YPosition);
            Assert.Equal(v2.PlayPauseButtonSettings.IsVisible, setting.IsVisible);
            Assert.Equal(v2.PlayPauseButtonSettings.PauseButtonImagePath, setting.PauseButtonImagePath);
            Assert.Equal(v2.PlayPauseButtonSettings.PlayButtonImagePath, setting.PlayButtonImagePath);
        }

        [Fact]
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

            var v1 = new V1Settings { PreviousSongButtonAppearance = setting };
            var v2 = SettingsMigration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.Equal(v2.PreviousButtonSettings.Height, setting.Height);
            Assert.Equal(v2.PreviousButtonSettings.Width, setting.Width);
            Assert.Equal(v2.PreviousButtonSettings.XPosition, setting.XPosition);
            Assert.Equal(v2.PreviousButtonSettings.YPosition, setting.YPosition);
            Assert.Equal(v2.PreviousButtonSettings.IsVisible, setting.IsVisible);
            Assert.Equal(v2.PreviousButtonSettings.ImagePath, setting.ImagePath);
        }

        [Fact]
        public void MigrateV1ToV2_ProgressBar()
        {
            var setting = new ProgressBarAppearance
            {
                Height = 1,
                Width = 2,
                XPosition = 3,
                IsVisible = false,
                YPosition = 5,
                BackgroundColor = Colors.White,
                ForegroundColor = Colors.Red,
            };

            var v1 = new V1Settings { ProgressBarAppearance = setting };
            var v2 = SettingsMigration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.Equal(v2.ProgressBarSettings.Width, setting.Width);
            Assert.Equal(v2.ProgressBarSettings.Height, setting.Height);
            Assert.Equal(v2.ProgressBarSettings.XPosition, setting.XPosition);
            Assert.Equal(v2.ProgressBarSettings.YPosition, setting.YPosition);
            Assert.Equal(v2.ProgressBarSettings.IsVisible, setting.IsVisible);
            Assert.Equal(v2.ProgressBarSettings.BackgroundColor, setting.BackgroundColor);
            Assert.Equal(v2.ProgressBarSettings.ForegroundColor, setting.ForegroundColor);
        }

        [Fact]
        public void MigrateV1ToV2_CustomText()
        {
            var text1 = new TextAppearance
            {
                Color = Colors.Red,
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

            var texts = new List<TextAppearance>() { text1 };
            var v1 = new V1Settings { TextAppearances = texts };
            var v2 = SettingsMigration.MigrateSettings<V2Settings>(v1, "0.1", "2");

            Assert.Equal(v2.CustomLabelSettings.Count, texts.Count);
            Assert.Equal(v2.CustomLabelSettings[0].Color, text1.Color);
            Assert.Equal(v2.CustomLabelSettings[0].Width, text1.Width);
            Assert.Equal(v2.CustomLabelSettings[0].Height, text1.Height);
            Assert.Equal(v2.CustomLabelSettings[0].Name, text1.Name);
            Assert.Equal(v2.CustomLabelSettings[0].XPosition, text1.XPosition);
            Assert.Equal(v2.CustomLabelSettings[0].IsVisible, text1.IsVisible);
            Assert.Equal(v2.CustomLabelSettings[0].YPosition, text1.YPosition);
            Assert.Equal(v2.CustomLabelSettings[0].ScrollSpeed, text1.ScrollSpeed);
            Assert.Equal(v2.CustomLabelSettings[0].FontSize, text1.FontSize);
            Assert.Equal(v2.CustomLabelSettings[0].FontFamily, text1.FontFamily);
            Assert.Equal(v2.CustomLabelSettings[0].Alignment, text1.Alignment);
            Assert.Equal(v2.CustomLabelSettings[0].FormatString, text1.FormatString);
        }

        [Fact]
        public void MigrateV2ToV3_MigratesSuccessfully()
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
            var settings = TomlHelper.DefaultSettings;

            var v2 = Toml.ReadString<V2Settings>(settingsFile, settings);
            var v3 = SettingsMigration.MigrateSettings<SettingsV3>(v2, "2", "3");

            Assert.Equal("3", v3.Version);
            Assert.Equal(v2.AudioSource, v3.AudioSource);

            Assert.Equal(v2.AlbumArtPopupSettings.Width, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtPopupSettings.Width);
            Assert.Equal(v2.AlbumArtPopupSettings.Height, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtPopupSettings.Height);
            Assert.Equal(v2.AlbumArtPopupSettings.IsVisible, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtPopupSettings.IsVisible);
            Assert.Equal(v2.AlbumArtPopupSettings.Margin, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtPopupSettings.Margin);
            Assert.Equal(v2.AlbumArtPopupSettings.XPosition, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtPopupSettings.XPosition);

            Assert.Equal(v2.AlbumArtSettings.Width, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtSettings.Width);
            Assert.Equal(v2.AlbumArtSettings.Height, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtSettings.Height);
            Assert.Equal(v2.AlbumArtSettings.IsVisible, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtSettings.IsVisible);
            Assert.Equal(v2.AlbumArtSettings.XPosition, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtSettings.XPosition);
            Assert.Equal(v2.AlbumArtSettings.YPosition, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtSettings.YPosition);
            Assert.Equal(v2.AlbumArtSettings.PlaceholderPath, v3.Profiles[SettingsV3.DefaultProfileName].AlbumArtSettings.PlaceholderPath);

            Assert.Equal(v2.AudioBandSettings.Width, v3.Profiles[SettingsV3.DefaultProfileName].GeneralSettings.Width);
            Assert.Equal(v2.AudioBandSettings.Height, v3.Profiles[SettingsV3.DefaultProfileName].GeneralSettings.Height);

            Assert.Equal(v2.NextButtonSettings.Width, v3.Profiles[SettingsV3.DefaultProfileName].NextButtonSettings.Width);
            Assert.Equal(v2.NextButtonSettings.Height, v3.Profiles[SettingsV3.DefaultProfileName].NextButtonSettings.Height);
            Assert.Equal(v2.NextButtonSettings.IsVisible, v3.Profiles[SettingsV3.DefaultProfileName].NextButtonSettings.IsVisible);
            Assert.Equal(v2.NextButtonSettings.XPosition, v3.Profiles[SettingsV3.DefaultProfileName].NextButtonSettings.XPosition);
            Assert.Equal(v2.NextButtonSettings.YPosition, v3.Profiles[SettingsV3.DefaultProfileName].NextButtonSettings.YPosition);
            Assert.Equal(v2.NextButtonSettings.ImagePath, v3.Profiles[SettingsV3.DefaultProfileName].NextButtonSettings.Content.ImagePath);

            Assert.Equal(v2.AudioSourceSettings.Count, v3.AudioSourceSettings.Count);
            Assert.Equal(v2.AudioSourceSettings[0].AudioSourceName, v3.AudioSourceSettings[0].AudioSourceName);
            Assert.Equal(v2.AudioSourceSettings[0].Settings.Count, v3.AudioSourceSettings[0].Settings.Count);
            Assert.Equal(v2.AudioSourceSettings[0].Settings[0].Name, v3.AudioSourceSettings[0].Settings[0].Name);
            Assert.Equal(v2.AudioSourceSettings[0].Settings[0].Value, v3.AudioSourceSettings[0].Settings[0].Value);

            Assert.Equal(v2.PlayPauseButtonSettings.Width, v3.Profiles[SettingsV3.DefaultProfileName].PlayPauseButtonSettings.Width);
            Assert.Equal(v2.PlayPauseButtonSettings.Height, v3.Profiles[SettingsV3.DefaultProfileName].PlayPauseButtonSettings.Height);
            Assert.Equal(v2.PlayPauseButtonSettings.XPosition, v3.Profiles[SettingsV3.DefaultProfileName].PlayPauseButtonSettings.XPosition);
            Assert.Equal(v2.PlayPauseButtonSettings.YPosition, v3.Profiles[SettingsV3.DefaultProfileName].PlayPauseButtonSettings.YPosition);
            Assert.Equal(v2.PlayPauseButtonSettings.IsVisible, v3.Profiles[SettingsV3.DefaultProfileName].PlayPauseButtonSettings.IsVisible);
            Assert.Equal(v2.PlayPauseButtonSettings.PauseButtonImagePath, v3.Profiles[SettingsV3.DefaultProfileName].PlayPauseButtonSettings.PauseContent.ImagePath);
            Assert.Equal(v2.PlayPauseButtonSettings.PlayButtonImagePath, v3.Profiles[SettingsV3.DefaultProfileName].PlayPauseButtonSettings.PlayContent.ImagePath);

            Assert.Equal(v2.PreviousButtonSettings.Height, v3.Profiles[SettingsV3.DefaultProfileName].PreviousButtonSettings.Height);
            Assert.Equal(v2.PreviousButtonSettings.Width, v3.Profiles[SettingsV3.DefaultProfileName].PreviousButtonSettings.Width);
            Assert.Equal(v2.PreviousButtonSettings.XPosition, v3.Profiles[SettingsV3.DefaultProfileName].PreviousButtonSettings.XPosition);
            Assert.Equal(v2.PreviousButtonSettings.YPosition, v3.Profiles[SettingsV3.DefaultProfileName].PreviousButtonSettings.YPosition);
            Assert.Equal(v2.PreviousButtonSettings.IsVisible, v3.Profiles[SettingsV3.DefaultProfileName].PreviousButtonSettings.IsVisible);
            Assert.Equal(v2.PreviousButtonSettings.ImagePath, v3.Profiles[SettingsV3.DefaultProfileName].PreviousButtonSettings.Content.ImagePath);

            Assert.Equal(v2.ProgressBarSettings.Width, v3.Profiles[SettingsV3.DefaultProfileName].ProgressBarSettings.Width);
            Assert.Equal(v2.ProgressBarSettings.Height, v3.Profiles[SettingsV3.DefaultProfileName].ProgressBarSettings.Height);
            Assert.Equal(v2.ProgressBarSettings.XPosition, v3.Profiles[SettingsV3.DefaultProfileName].ProgressBarSettings.XPosition);
            Assert.Equal(v2.ProgressBarSettings.YPosition, v3.Profiles[SettingsV3.DefaultProfileName].ProgressBarSettings.YPosition);
            Assert.Equal(v2.ProgressBarSettings.IsVisible, v3.Profiles[SettingsV3.DefaultProfileName].ProgressBarSettings.IsVisible);
            Assert.Equal(v2.ProgressBarSettings.BackgroundColor, v3.Profiles[SettingsV3.DefaultProfileName].ProgressBarSettings.BackgroundColor);
            Assert.Equal(v2.ProgressBarSettings.ForegroundColor, v3.Profiles[SettingsV3.DefaultProfileName].ProgressBarSettings.ForegroundColor);

            Assert.Equal(v2.CustomLabelSettings.Count, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings.Count);
            Assert.Equal(v2.CustomLabelSettings[0].Color, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].Color);
            Assert.Equal(v2.CustomLabelSettings[0].Width, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].Width);
            Assert.Equal(v2.CustomLabelSettings[0].Height, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].Height);
            Assert.Equal(v2.CustomLabelSettings[0].Name, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].Name);
            Assert.Equal(v2.CustomLabelSettings[0].XPosition, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].XPosition);
            Assert.Equal(v2.CustomLabelSettings[0].IsVisible, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].IsVisible);
            Assert.Equal(v2.CustomLabelSettings[0].YPosition, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].YPosition);
            Assert.Equal(v2.CustomLabelSettings[0].ScrollSpeed, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].ScrollSpeed);
            Assert.Equal(v2.CustomLabelSettings[0].FontSize, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].FontSize);
            Assert.Equal(v2.CustomLabelSettings[0].FontFamily, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].FontFamily);
            Assert.Equal(v2.CustomLabelSettings[0].Alignment, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].Alignment);
            Assert.Equal(v2.CustomLabelSettings[0].FormatString, v3.Profiles[SettingsV3.DefaultProfileName].CustomLabelSettings[0].FormatString);
        }

        [Fact]
        public void ChainedMigrations_V1ToV3_SuccessfulMigration()
        {
            var v1Settings = @"
Version = ""0.1""

[AudioBandAppearance]
Width = 300
Height = 30

[PlayPauseButtonAppearance]
XPosition = 0
YPosition = 0
Width = 30
Height = 10
IsVisible = true

[NextSongButtonAppearance]
IsVisible = true
Width = 30
Height = 10
XPosition = 0
YPosition = 0

[PreviousSongButtonAppearance]
IsVisible = true
Width = 30
Height = 10
XPosition = 0
YPosition = 0

[[TextAppearances]]
IsVisible = true
Width = 100
Height = 15
XPosition = 150
YPosition = 10
FontSize = 10.0
Color = ""White""
Alignment = ""Center""
ScrollSpeed = 0
FormatString = ""{song}""

[ProgressBarAppearance]
ForegroundColor = ""Blue""
BackgroundColor = ""Gray""
IsVisible = true
XPosition = 0
YPosition = 26
Width = 200
Height = 2

[AlbumArtAppearance]
IsVisible = true
Width = 30
Height = 30
XPosition = 0
YPosition = 0

[AlbumArtPopupAppearance]
IsVisible = true
Width = 500
Height = 500
XOffset = 50
Margin = 6

";
            var v1 = Toml.ReadString<V1Settings>(v1Settings, TomlHelper.DefaultSettings);
            var v3 = SettingsMigration.MigrateSettings<SettingsV3>(v1, "0.1", "3");
            var v3Profile = v3.Profiles[SettingsV3.DefaultProfileName];

            Assert.Equal(v1.AudioBandAppearance.Width, v3Profile.GeneralSettings.Width);
            Assert.Equal(v1.AudioBandAppearance.Height, v3Profile.GeneralSettings.Height);

            Assert.Equal(v1.PlayPauseButtonAppearance.Width, v3Profile.PlayPauseButtonSettings.Width);
            Assert.Equal(v1.PlayPauseButtonAppearance.Height, v3Profile.PlayPauseButtonSettings.Height);
            Assert.Equal(v1.PlayPauseButtonAppearance.XPosition, v3Profile.PlayPauseButtonSettings.XPosition);
            Assert.Equal(v1.PlayPauseButtonAppearance.YPosition, v3Profile.PlayPauseButtonSettings.YPosition);
            Assert.Equal(v1.PlayPauseButtonAppearance.IsVisible, v3Profile.PlayPauseButtonSettings.IsVisible);

            Assert.Equal(v1.NextSongButtonAppearance.Width, v3Profile.NextButtonSettings.Width);
            Assert.Equal(v1.NextSongButtonAppearance.Height, v3Profile.NextButtonSettings.Height);
            Assert.Equal(v1.NextSongButtonAppearance.XPosition, v3Profile.NextButtonSettings.XPosition);
            Assert.Equal(v1.NextSongButtonAppearance.YPosition, v3Profile.NextButtonSettings.YPosition);
            Assert.Equal(v1.NextSongButtonAppearance.IsVisible, v3Profile.NextButtonSettings.IsVisible);

            Assert.Equal(v1.PreviousSongButtonAppearance.Width, v3Profile.PreviousButtonSettings.Width);
            Assert.Equal(v1.PreviousSongButtonAppearance.Height, v3Profile.PreviousButtonSettings.Height);
            Assert.Equal(v1.PreviousSongButtonAppearance.XPosition, v3Profile.PreviousButtonSettings.XPosition);
            Assert.Equal(v1.PreviousSongButtonAppearance.YPosition, v3Profile.PreviousButtonSettings.YPosition);
            Assert.Equal(v1.PreviousSongButtonAppearance.IsVisible, v3Profile.PreviousButtonSettings.IsVisible);

            Assert.Single(v3Profile.CustomLabelSettings);
            Assert.Equal(v1.TextAppearances[0].Color, v3Profile.CustomLabelSettings[0].Color);
            Assert.Equal(v1.TextAppearances[0].Alignment, v3Profile.CustomLabelSettings[0].Alignment);
            Assert.Equal(v1.TextAppearances[0].FontFamily, v3Profile.CustomLabelSettings[0].FontFamily);
            Assert.Equal(v1.TextAppearances[0].FontSize, v3Profile.CustomLabelSettings[0].FontSize);
            Assert.Equal(v1.TextAppearances[0].FormatString, v3Profile.CustomLabelSettings[0].FormatString);
            Assert.Equal(v1.TextAppearances[0].Height, v3Profile.CustomLabelSettings[0].Height);
            Assert.Equal(v1.TextAppearances[0].IsVisible, v3Profile.CustomLabelSettings[0].IsVisible);
            Assert.Equal(v1.TextAppearances[0].Name, v3Profile.CustomLabelSettings[0].Name);
            Assert.Equal(v1.TextAppearances[0].ScrollSpeed, v3Profile.CustomLabelSettings[0].ScrollSpeed);
            Assert.Equal(v1.TextAppearances[0].Width, v3Profile.CustomLabelSettings[0].Width);
            Assert.Equal(v1.TextAppearances[0].XPosition, v3Profile.CustomLabelSettings[0].XPosition);
            Assert.Equal(v1.TextAppearances[0].YPosition, v3Profile.CustomLabelSettings[0].YPosition);

            Assert.Equal(v1.AlbumArtAppearance.Height, v3Profile.AlbumArtSettings.Height);
            Assert.Equal(v1.AlbumArtAppearance.IsVisible, v3Profile.AlbumArtSettings.IsVisible);
            Assert.Equal(v1.AlbumArtAppearance.PlaceholderPath, v3Profile.AlbumArtSettings.PlaceholderPath);
            Assert.Equal(v1.AlbumArtAppearance.Width, v3Profile.AlbumArtSettings.Width);
            Assert.Equal(v1.AlbumArtAppearance.XPosition, v3Profile.AlbumArtSettings.XPosition);
            Assert.Equal(v1.AlbumArtAppearance.YPosition, v3Profile.AlbumArtSettings.YPosition);
        }

        [Fact]
        public void MigrateV3ToV4_MigratesSuccessfully()
        {
            var settings = @"
Version = ""3""
AudioSource = ""Spotify""
CurrentProfileName = ""Default Profile""

[Profiles]

[Profiles.'Default Profile']

[Profiles.'Default Profile'.GeneralSettings]
Width = 500.0
Height = 30.0
BackgroundColor = ""#00FFFFFF""

[Profiles.'Default Profile'.PreviousButtonSettings]
BackgroundColor = ""#00FFFFFF""
HoveredBackgroundColor = ""#19FFFFFF""
ClickedBackgroundColor = ""#0FFFFFFF""
IsVisible = true
Width = 40.0
Height = 15.0
XPosition = 330.0
YPosition = 3.0
Anchor = ""TopLeft""

[Profiles.'Default Profile'.PreviousButtonSettings.Content]
ContentType = ""Text""
FontFamily = ""Segoe MDL2 Assets""
Text = """"
TextColor = ""#FFFFFFFF""
HoveredTextColor = ""#FFFFFFFF""
ClickedTextColor = ""#FFFFFFFF""

[Profiles.'Default Profile'.PlayPauseButtonSettings]
BackgroundColor = ""#00FFFFFF""
HoveredBackgroundColor = ""#19FFFFFF""
ClickedBackgroundColor = ""#0FFFFFFF""
IsVisible = true
Width = 40.0
Height = 14.0
XPosition = 370.0
YPosition = 3.0
Anchor = ""TopLeft""

[Profiles.'Default Profile'.PlayPauseButtonSettings.PlayContent]
ContentType = ""Text""
FontFamily = ""Segoe MDL2 Assets""
Text = """"
TextColor = ""#FFFFFFFF""
HoveredTextColor = ""#FFFFFFFF""
ClickedTextColor = ""#FFFFFFFF""

[Profiles.'Default Profile'.PlayPauseButtonSettings.PauseContent]
ContentType = ""Text""
FontFamily = ""Segoe MDL2 Assets""
Text = """"
TextColor = ""#FFFFFFFF""
HoveredTextColor = ""#FFFFFFFF""
ClickedTextColor = ""#FFFFFFFF""

[Profiles.'Default Profile'.NextButtonSettings]
BackgroundColor = ""#00FFFFFF""
HoveredBackgroundColor = ""#19FFFFFF""
ClickedBackgroundColor = ""#0FFFFFFF""
IsVisible = true
Width = 40.0
Height = 15.0
XPosition = 410.0
YPosition = 3.0
Anchor = ""TopLeft""

[Profiles.'Default Profile'.NextButtonSettings.Content]
ContentType = ""Text""
FontFamily = ""Segoe MDL2 Assets""
Text = """"
TextColor = ""#FFFFFFFF""
HoveredTextColor = ""#FFFFFFFF""
ClickedTextColor = ""#FFFFFFFF""

[Profiles.'Default Profile'.RepeatModeButtonSettings]
BackgroundColor = ""#00FFFFFF""
HoveredBackgroundColor = ""#19FFFFFF""
ClickedBackgroundColor = ""#0FFFFFFF""
IsVisible = true
Width = 40.0
Height = 15.0
XPosition = 450.0
YPosition = 3.0
Anchor = ""TopLeft""

[Profiles.'Default Profile'.RepeatModeButtonSettings.RepeatOffContent]
ContentType = ""Text""
ImagePath = """"
FontFamily = ""Segoe MDL2 Assets""
Text = """"
TextColor = ""#FF696969""
HoveredTextColor = ""#FFFFFFFF""
ClickedTextColor = ""#FF808080""

[Profiles.'Default Profile'.RepeatModeButtonSettings.RepeatContextContent]
ContentType = ""Text""
FontFamily = ""Segoe MDL2 Assets""
Text = """"
TextColor = ""#FF1E90FF""
HoveredTextColor = ""#FF6495ED""
ClickedTextColor = ""#FF4169E1""

[Profiles.'Default Profile'.RepeatModeButtonSettings.RepeatTrackContent]
ContentType = ""Text""
FontFamily = ""Segoe MDL2 Assets""
Text = """"
TextColor = ""#FF1E90FF""
HoveredTextColor = ""#FF6495ED""
ClickedTextColor = ""#FF4169E1""

[Profiles.'Default Profile'.ShuffleModeButtonSettings]
BackgroundColor = ""#00FFFFFF""
HoveredBackgroundColor = ""#19FFFFFF""
ClickedBackgroundColor = ""#0FFFFFFF""
IsVisible = true
Width = 40.0
Height = 15.0
XPosition = 290.0
YPosition = 3.0
Anchor = ""TopLeft""

[Profiles.'Default Profile'.ShuffleModeButtonSettings.ShuffleOffContent]
ContentType = ""Text""
FontFamily = ""Segoe MDL2 Assets""
Text = """"
TextColor = ""#FF696969""
HoveredTextColor = ""#FFFFFFFF""
ClickedTextColor = ""#FF808080""

[Profiles.'Default Profile'.ShuffleModeButtonSettings.ShuffleOnContent]
ContentType = ""Text""
FontFamily = ""Segoe MDL2 Assets""
Text = """"
TextColor = ""#FF1E90FF""
HoveredTextColor = ""#FF6495ED""
ClickedTextColor = ""#FF4169E1""

[Profiles.'Default Profile'.ProgressBarSettings]
ForegroundColor = ""#FF1E90FF""
BackgroundColor = ""#FF696969""
HoverColor = ""#FF00BFFF""
IsVisible = true
Width = 130.0
Height = 4.0
XPosition = 325.0
YPosition = 22.0
Anchor = ""TopLeft""

[Profiles.'Default Profile'.AlbumArtSettings]
PlaceholderPath = """"
IsVisible = true
Width = 30.0
Height = 30.0
XPosition = 245.0
YPosition = 0.0
Anchor = ""TopLeft""

[Profiles.'Default Profile'.AlbumArtPopupSettings]
IsVisible = true
Width = 250.0
Height = 250.0
XPosition = -110.0
Margin = -4.0

[[Profiles.'Default Profile'.CustomLabelSettings]]
FontFamily = ""Segoe UI""
FontSize = 12.0
Color = ""#FFC3C3C3""
FormatString = ""{length}""
Alignment = ""Right""
Name = ""Song Length""
ScrollSpeed = 5000
TextOverflow = ""Scroll""
ScrollBehavior = ""Always""
FadeEffect = ""OnlyWhenScrolling""
LeftFadeOffset = 0.1
RightFadeOffset = 0.9
IsVisible = true
Width = 40.0
Height = 15.0
XPosition = 460.0
YPosition = 14.0
Anchor = ""TopLeft""

[[AudioSourceSettings]]
AudioSourceName = ""Spotify""

[[AudioSourceSettings.Settings]]
Name = ""Spotify Client ID""
Value = ""id""
[[AudioSourceSettings.Settings]]
Name = ""Spotify Client secret""
Value = ""secret""
[[AudioSourceSettings.Settings]]
Name = ""Callback Port""
Value = 80
[[AudioSourceSettings.Settings]]
Name = ""Proxy Host""
Value = ""localhost""
[[AudioSourceSettings.Settings]]
Name = ""Proxy Password""
Value = ""1""
[[AudioSourceSettings.Settings]]
Name = ""Proxy Port""
Value = 5555
[[AudioSourceSettings.Settings]]
Name = ""Proxy Username""
Value = ""1""
[[AudioSourceSettings.Settings]]
Name = ""Spotify Refresh Token""
Value = ""token""
[[AudioSourceSettings.Settings]]
Name = ""Use Proxy""
Value = false

";
            var v3 = Toml.ReadString<SettingsV3>(settings, TomlHelper.DefaultSettings);
            var v4 = SettingsMigration.MigrateSettings<SettingsV4>(v3, "3", "4");

            Assert.Equal(v3.AudioSource, v4.AudioSource);
            Assert.Equal(v3.CurrentProfileName, v4.CurrentProfileName);
            Assert.Equal(v3.AudioSourceSettings, v4.AudioSourceSettings);

            Assert.Equal(v3.Profiles.Count, v4.Profiles.Count);
            var v3Profile = v3.Profiles[v3.CurrentProfileName];
            var v4Profile = v4.Profiles.Find(p => p.Name == v4.CurrentProfileName);
            Assert.NotNull(v4Profile.Name);

            Assert.Equal(v3Profile.AlbumArtPopupSettings.Width, v4Profile.AlbumArtPopup.Width);
            Assert.Equal(v3Profile.AlbumArtPopupSettings.Height, v4Profile.AlbumArtPopup.Height);
            Assert.Equal(v3Profile.AlbumArtPopupSettings.IsVisible, v4Profile.AlbumArtPopup.IsVisible);
            Assert.Equal(v3Profile.AlbumArtPopupSettings.Margin, v4Profile.AlbumArtPopup.Margin);
            Assert.Equal(v3Profile.AlbumArtPopupSettings.XPosition, v4Profile.AlbumArtPopup.XPosition);

            Assert.Equal(v3Profile.AlbumArtSettings.Width, v4Profile.AlbumArt.Width);
            Assert.Equal(v3Profile.AlbumArtSettings.Height, v4Profile.AlbumArt.Height);
            Assert.Equal(v3Profile.AlbumArtSettings.IsVisible, v4Profile.AlbumArt.IsVisible);
            Assert.Equal(v3Profile.AlbumArtSettings.XPosition, v4Profile.AlbumArt.XPosition);
            Assert.Equal(v3Profile.AlbumArtSettings.YPosition, v4Profile.AlbumArt.YPosition);
            Assert.Equal(v3Profile.AlbumArtSettings.PlaceholderPath, v4Profile.AlbumArt.PlaceholderPath);

            Assert.Equal(v3Profile.GeneralSettings.Width, v4Profile.GeneralSettings.Width);
            Assert.Equal(v3Profile.GeneralSettings.Height, v4Profile.GeneralSettings.Height);

            Assert.Equal(v3Profile.NextButtonSettings.Width, v4Profile.NextButton.Width);
            Assert.Equal(v3Profile.NextButtonSettings.Height, v4Profile.NextButton.Height);
            Assert.Equal(v3Profile.NextButtonSettings.IsVisible, v4Profile.NextButton.IsVisible);
            Assert.Equal(v3Profile.NextButtonSettings.XPosition, v4Profile.NextButton.XPosition);
            Assert.Equal(v3Profile.NextButtonSettings.YPosition, v4Profile.NextButton.YPosition);
            Assert.Equal(v3Profile.NextButtonSettings.Content.ImagePath, v4Profile.NextButton.Content.ImagePath);

            Assert.Equal(v3Profile.PlayPauseButtonSettings.Width, v4Profile.PlayPauseButton.Width);
            Assert.Equal(v3Profile.PlayPauseButtonSettings.Height, v4Profile.PlayPauseButton.Height);
            Assert.Equal(v3Profile.PlayPauseButtonSettings.XPosition, v4Profile.PlayPauseButton.XPosition);
            Assert.Equal(v3Profile.PlayPauseButtonSettings.YPosition, v4Profile.PlayPauseButton.YPosition);
            Assert.Equal(v3Profile.PlayPauseButtonSettings.IsVisible, v4Profile.PlayPauseButton.IsVisible);
            Assert.Equal(v3Profile.PlayPauseButtonSettings.PauseContent.ImagePath, v4Profile.PlayPauseButton.PauseContent.ImagePath);
            Assert.Equal(v3Profile.PlayPauseButtonSettings.PlayContent.ImagePath, v4Profile.PlayPauseButton.PlayContent.ImagePath);

            Assert.Equal(v3Profile.PreviousButtonSettings.Height, v4Profile.PreviousButton.Height);
            Assert.Equal(v3Profile.PreviousButtonSettings.Width, v4Profile.PreviousButton.Width);
            Assert.Equal(v3Profile.PreviousButtonSettings.XPosition, v4Profile.PreviousButton.XPosition);
            Assert.Equal(v3Profile.PreviousButtonSettings.YPosition, v4Profile.PreviousButton.YPosition);
            Assert.Equal(v3Profile.PreviousButtonSettings.IsVisible, v4Profile.PreviousButton.IsVisible);
            Assert.Equal(v3Profile.PreviousButtonSettings.Content.ImagePath, v4Profile.PreviousButton.Content.ImagePath);

            Assert.Equal(v3Profile.ProgressBarSettings.Width, v4Profile.ProgressBar.Width);
            Assert.Equal(v3Profile.ProgressBarSettings.Height, v4Profile.ProgressBar.Height);
            Assert.Equal(v3Profile.ProgressBarSettings.XPosition, v4Profile.ProgressBar.XPosition);
            Assert.Equal(v3Profile.ProgressBarSettings.YPosition, v4Profile.ProgressBar.YPosition);
            Assert.Equal(v3Profile.ProgressBarSettings.IsVisible, v4Profile.ProgressBar.IsVisible);
            Assert.Equal(v3Profile.ProgressBarSettings.BackgroundColor, v4Profile.ProgressBar.BackgroundColor);
            Assert.Equal(v3Profile.ProgressBarSettings.ForegroundColor, v4Profile.ProgressBar.ForegroundColor);

            Assert.Equal(v3Profile.CustomLabelSettings.Count, v4Profile.CustomLabels.Count);
            Assert.Equal(v3Profile.CustomLabelSettings[0].Color, v4Profile.CustomLabels[0].Color);
            Assert.Equal(v3Profile.CustomLabelSettings[0].Width, v4Profile.CustomLabels[0].Width);
            Assert.Equal(v3Profile.CustomLabelSettings[0].Height, v4Profile.CustomLabels[0].Height);
            Assert.Equal(v3Profile.CustomLabelSettings[0].Name, v4Profile.CustomLabels[0].Name);
            Assert.Equal(v3Profile.CustomLabelSettings[0].XPosition, v4Profile.CustomLabels[0].XPosition);
            Assert.Equal(v3Profile.CustomLabelSettings[0].IsVisible, v4Profile.CustomLabels[0].IsVisible);
            Assert.Equal(v3Profile.CustomLabelSettings[0].YPosition, v4Profile.CustomLabels[0].YPosition);
            Assert.Equal(v3Profile.CustomLabelSettings[0].ScrollSpeed, v4Profile.CustomLabels[0].ScrollSpeed);
            Assert.Equal(v3Profile.CustomLabelSettings[0].FontSize, v4Profile.CustomLabels[0].FontSize);
            Assert.Equal(v3Profile.CustomLabelSettings[0].FontFamily, v4Profile.CustomLabels[0].FontFamily);
            Assert.Equal(v3Profile.CustomLabelSettings[0].Alignment, v4Profile.CustomLabels[0].Alignment);
            Assert.Equal(v3Profile.CustomLabelSettings[0].FormatString, v4Profile.CustomLabels[0].FormatString);
        }
    }
}
