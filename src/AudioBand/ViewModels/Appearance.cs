using System.Collections.Generic;
using System.ComponentModel;

namespace AudioBand.ViewModels
{
    internal class Appearance : IEditableObject, IResettableObject
    {
        public AudioBandAppearance AudioBandAppearance { get; set; }
        public PlayPauseButtonAppearance PlayPauseButtonAppearance { get; set; }
        public NextSongButtonAppearance NextSongButtonAppearance { get; set; }
        public PreviousSongButtonAppearance PreviousSongButtonAppearance { get; set; }
        public List<TextAppearance> TextAppearances { get; set; }
        public ProgressBarAppearance ProgressBarAppearance { get; set; }
        public AlbumArtDisplay AlbumArtAppearance { get; set; }
        public AlbumArtPopup AlbumArtPopupAppearance { get; set; }

        public void BeginEdit()
        {
            AlbumArtPopupAppearance.BeginEdit();
            TextAppearances.ForEach(a => a.BeginEdit());
            AlbumArtAppearance.BeginEdit();
            AudioBandAppearance.BeginEdit();
            NextSongButtonAppearance.BeginEdit();
            PreviousSongButtonAppearance.BeginEdit();
            PlayPauseButtonAppearance.BeginEdit();
            ProgressBarAppearance.BeginEdit();
        }

        public void EndEdit() {}

        public void CancelEdit()
        {
            AlbumArtPopupAppearance.CancelEdit();
            TextAppearances.ForEach(a => a.CancelEdit());
            AlbumArtAppearance.CancelEdit();
            AudioBandAppearance.CancelEdit();
            NextSongButtonAppearance.CancelEdit();
            PreviousSongButtonAppearance.CancelEdit();
            PlayPauseButtonAppearance.CancelEdit();
            ProgressBarAppearance.CancelEdit();
        }

        public void Reset()
        {
            AlbumArtPopupAppearance.Reset();
            TextAppearances = new List<TextAppearance> {new TextAppearance()};
            AlbumArtAppearance.Reset();
            AudioBandAppearance.Reset();
            NextSongButtonAppearance.Reset();
            PreviousSongButtonAppearance.Reset();
            PlayPauseButtonAppearance.Reset();
            ProgressBarAppearance.Reset();
        }
    }
}
