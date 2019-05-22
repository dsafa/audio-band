#pragma warning disable
using System.Collections.Generic;

namespace AudioBand.Settings.Models.V1
{
    internal class AudioBandSettings
    {
        public string Version { get; set; } = "0.1";

        public string AudioSource { get; set; }

        public AudioBandAppearance AudioBandAppearance { get; set; } = new AudioBandAppearance();

        public PlayPauseButtonAppearance PlayPauseButtonAppearance { get; set; } = new PlayPauseButtonAppearance();

        public NextSongButtonAppearance NextSongButtonAppearance { get; set; } = new NextSongButtonAppearance();

        public PreviousSongButtonAppearance PreviousSongButtonAppearance { get; set; } = new PreviousSongButtonAppearance();

        public List<TextAppearance> TextAppearances { get; set; } = new List<TextAppearance> { new TextAppearance() };

        public ProgressBarAppearance ProgressBarAppearance { get; set; } = new ProgressBarAppearance();

        public AlbumArtAppearance AlbumArtAppearance { get; set; } = new AlbumArtAppearance();

        public AlbumArtPopupAppearance AlbumArtPopupAppearance { get; set; } = new AlbumArtPopupAppearance();

        public List<AudioSourceSettingsCollection> AudioSourceSettings { get; set; } = new List<AudioSourceSettingsCollection>();
    }
}
