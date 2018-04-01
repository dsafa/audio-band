using MetroFramework.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AudioBand.Settings
{
    internal partial class SettingsWindow : MetroForm
    {
        private readonly AppearanceViewModel _appearanceViewModel;

        public SettingsWindow(AppearanceViewModel appearanceViewModel)
        {
            InitializeComponent();

            _appearanceViewModel = appearanceViewModel;

            artistFontDialog.Font = _appearanceViewModel.NowPlayingArtistFont;
            artistFontDisplay.DataBindings.Add(nameof(artistFontDisplay.Text), _appearanceViewModel, $"{nameof(_appearanceViewModel.NowPlayingArtistFont)}.Name");

            artistColorDialog.CustomColors = new[] {ColorTranslator.ToOle(_appearanceViewModel.NowPlayingArtistColor)};
            artistColorDisplay.DataBindings.Add(nameof(artistColorDisplay.BackColor), _appearanceViewModel, nameof(_appearanceViewModel.NowPlayingArtistColor));

            songFontDialog.Font = _appearanceViewModel.NowPlayingTrackNameFont;
            songFontDisplay.DataBindings.Add(nameof(songFontDisplay.Text), _appearanceViewModel, $"{nameof(_appearanceViewModel.NowPlayingTrackNameFont)}.Name");

            songColorDialog.CustomColors = new[] {ColorTranslator.ToOle(_appearanceViewModel.NowPlayingTrackNameColor)};
            songColorDisplay.DataBindings.Add(nameof(songColorDisplay.BackColor), _appearanceViewModel, nameof(_appearanceViewModel.NowPlayingTrackNameColor));

            progressColorDialog.CustomColors = new[] {ColorTranslator.ToOle(_appearanceViewModel.TrackProgessColor)};
            progressColorDisplay.DataBindings.Add(nameof(progressColorDisplay.BackColor), _appearanceViewModel, nameof(_appearanceViewModel.TrackProgessColor));
        }

        private void OnFormClosing(object sender, FormClosingEventArgs formClosingEventArgs)
        {
            formClosingEventArgs.Cancel = true;
            Hide();
        }

        private void ArtistFontButtonOnClick(object sender, EventArgs e)
        {
            if (artistFontDialog.ShowDialog(this) == DialogResult.OK)
            {
                _appearanceViewModel.NowPlayingArtistFont = artistFontDialog.Font;
            }
        }

        private void ArtistColorButtonOnClick(object sender, EventArgs e)
        {
            if (artistColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                _appearanceViewModel.NowPlayingArtistColor = artistColorDialog.Color;
            }
        }

        private void SongFontButtonOnClick(object sender, EventArgs e)
        {
            if (songFontDialog.ShowDialog(this) == DialogResult.OK)
            {
                _appearanceViewModel.NowPlayingTrackNameFont = songFontDialog.Font;
            }
        }

        private void SongColorButtonOnClick(object sender, EventArgs e)
        {
            if (songColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                _appearanceViewModel.NowPlayingTrackNameColor = songColorDialog.Color;
            }
        }

        private void ProgressColorButtonOnClick(object sender, EventArgs e)
        {
            if (progressColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                _appearanceViewModel.TrackProgessColor = progressColorDialog.Color;
            }
        }
    }
}
