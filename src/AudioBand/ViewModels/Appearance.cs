using System.Collections.Generic;

namespace AudioBand.ViewModels
{
    internal class Appearance
    {
        public AudioBandAppearance AudioBandAppearance { get; set; }
        public PlayPauseButtonAppearance PlayPauseButtonAppearance { get; set; }
        public NextSongButtonAppearance NextSongButtonAppearance { get; set; }
        public PreviousSongButtonAppearance PreviousSongButtonAppearance { get; set; }
        public List<TextAppearance> TextAppearances { get; set; }
        public ProgressBarAppearance ProgressBarAppearance { get; set; }
        public AlbumArtDisplay AlbumArtAppearance { get; set; }
        public AlbumArtPopup AlbumArtPopupAppearance { get; set; }
    }
}
