using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AudioBand.ViewModels;

namespace AudioBand.Settings
{
    internal static class Converters
    {
        public static string FontToString(Font font)
        {
            return String.Join(";", font.Name, font.Size.ToString(), font.Style.ToString(), font.Unit.ToString());
        }

        public static Font StringToFont(string fontString)
        {
            var vals = fontString.Split(';');
            return new Font(vals[0], float.Parse(vals[1]), ToEnum<FontStyle>(vals[2]), ToEnum<GraphicsUnit>(vals[3]));
        }

        public static T ToEnum<T>(string val)
        {
            return (T)Enum.Parse(typeof(T), val);
        }

        public static string EnumToString<T>(T value)
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException($"Value is not an enum {value} | {typeof(T)}");
            }
            return Enum.GetName(typeof(T), value);
        }

        public static Appearance ToModel(this AudioBandSettings settings)
        {
            return new Appearance
            {
                PreviousSongButtonAppearance = settings.PreviousSongButtonAppearance,
                NextSongButtonAppearance = settings.NextSongButtonAppearance,
                PlayPauseButtonAppearance = settings.PlayPauseButtonAppearance,
                AudioBandAppearance = settings.AudioBandAppearance,
                ProgressBarAppearance = settings.ProgressBarAppearance,
                AlbumArtAppearance = settings.AlbumArtAppearance,
                AlbumArtPopupAppearance = settings.AlbumArtPopupAppearance,
                TextAppearances = settings.TextAppearances
            };
        }

        public static AudioBandSettings ToSettings(this Appearance appearance)
        {
            return new AudioBandSettings
            {
                PreviousSongButtonAppearance = appearance.PreviousSongButtonAppearance,
                NextSongButtonAppearance = appearance.NextSongButtonAppearance,
                PlayPauseButtonAppearance = appearance.PlayPauseButtonAppearance,
                AudioBandAppearance = appearance.AudioBandAppearance,
                ProgressBarAppearance = appearance.ProgressBarAppearance,
                AlbumArtAppearance = appearance.AlbumArtAppearance,
                AlbumArtPopupAppearance = appearance.AlbumArtPopupAppearance,
                TextAppearances = appearance.TextAppearances
            };
        }
    }
}
