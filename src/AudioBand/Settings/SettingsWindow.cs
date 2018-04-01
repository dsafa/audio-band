using MetroFramework.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AudioBand.Settings
{
    internal partial class SettingsWindow : MetroForm
    {
        private readonly AudioBandAppearance _audioBandAppearance;

        public SettingsWindow(AudioBandAppearance audioBandAppearance)
        {
            InitializeComponent();

            _audioBandAppearance = audioBandAppearance;

            artistFontDialog.Font = _audioBandAppearance.NowPlayingArtistFont;
            artistFontDisplay.DataBindings.Add(nameof(artistFontDisplay.Text), _audioBandAppearance, $"{nameof(_audioBandAppearance.NowPlayingArtistFont)}.Name");

            artistColorDialog.CustomColors = new[] {ColorTranslator.ToOle(_audioBandAppearance.NowPlayingArtistColor)};
            artistColorDisplay.DataBindings.Add(nameof(artistColorDisplay.BackColor), _audioBandAppearance, nameof(_audioBandAppearance.NowPlayingArtistColor));

            songFontDialog.Font = _audioBandAppearance.NowPlayingTrackNameFont;
            songFontDisplay.DataBindings.Add(nameof(songFontDisplay.Text), _audioBandAppearance, $"{nameof(_audioBandAppearance.NowPlayingTrackNameFont)}.Name");

            songColorDialog.CustomColors = new[] {ColorTranslator.ToOle(_audioBandAppearance.NowPlayingTrackNameColor)};
            songColorDisplay.DataBindings.Add(nameof(songColorDisplay.BackColor), _audioBandAppearance, nameof(_audioBandAppearance.NowPlayingTrackNameColor));

            progressColorDialog.CustomColors = new[] {ColorTranslator.ToOle(_audioBandAppearance.TrackProgessColor)};
            progressColorDisplay.DataBindings.Add(nameof(progressColorDisplay.BackColor), _audioBandAppearance, nameof(_audioBandAppearance.TrackProgessColor));
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
                _audioBandAppearance.NowPlayingArtistFont = artistFontDialog.Font;
            }
        }

        private void ArtistColorButtonOnClick(object sender, EventArgs e)
        {
            if (artistColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                _audioBandAppearance.NowPlayingArtistColor = artistColorDialog.Color;
            }
        }

        private void SongFontButtonOnClick(object sender, EventArgs e)
        {
            if (songFontDialog.ShowDialog(this) == DialogResult.OK)
            {
                _audioBandAppearance.NowPlayingTrackNameFont = songFontDialog.Font;
            }
        }

        private void SongColorButtonOnClick(object sender, EventArgs e)
        {
            if (songColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                _audioBandAppearance.NowPlayingTrackNameColor = songColorDialog.Color;
            }
        }

        private void ProgressColorButtonOnClick(object sender, EventArgs e)
        {
            if (progressColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                _audioBandAppearance.TrackProgessColor = progressColorDialog.Color;
            }
        }
    }
}
