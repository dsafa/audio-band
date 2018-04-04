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

            artistFontDisplay.DataBindings.Add(nameof(artistFontDisplay.Text), _audioBandAppearance, $"{nameof(_audioBandAppearance.NowPlayingArtistFont)}.Name");
            artistColorDisplay.DataBindings.Add(nameof(artistColorDisplay.BackColor), _audioBandAppearance, nameof(_audioBandAppearance.NowPlayingArtistColor));
            songFontDisplay.DataBindings.Add(nameof(songFontDisplay.Text), _audioBandAppearance, $"{nameof(_audioBandAppearance.NowPlayingTrackNameFont)}.Name");
            songColorDisplay.DataBindings.Add(nameof(songColorDisplay.BackColor), _audioBandAppearance, nameof(_audioBandAppearance.NowPlayingTrackNameColor));
            progressColorDisplay.DataBindings.Add(nameof(progressColorDisplay.BackColor), _audioBandAppearance, nameof(_audioBandAppearance.TrackProgressColor));
            progressBackColorDisplay.DataBindings.Add(nameof(progressBackColorDisplay.BackColor), _audioBandAppearance, nameof(_audioBandAppearance.TrackProgressBackColor));
        }

        private void OnFormClosing(object sender, FormClosingEventArgs formClosingEventArgs)
        {
            formClosingEventArgs.Cancel = true;
            Hide();
        }

        private void ArtistFontButtonOnClick(object sender, EventArgs e)
        {
            fontDialog.Font = _audioBandAppearance.NowPlayingArtistFont;
            if (fontDialog.ShowDialog(this) == DialogResult.OK)
            {
                _audioBandAppearance.NowPlayingArtistFont = fontDialog.Font;
            }
        }

        private void ArtistColorButtonOnClick(object sender, EventArgs e)
        {
            colorDialog.CustomColors = new[] { ColorTranslator.ToOle(_audioBandAppearance.NowPlayingArtistColor) };
            if (colorDialog.ShowDialog(this) == DialogResult.OK)
            {
                _audioBandAppearance.NowPlayingArtistColor = colorDialog.Color;
            }
        }

        private void SongFontButtonOnClick(object sender, EventArgs e)
        {
            fontDialog.Font = _audioBandAppearance.NowPlayingTrackNameFont;
            if (fontDialog.ShowDialog(this) == DialogResult.OK)
            {
                _audioBandAppearance.NowPlayingTrackNameFont = fontDialog.Font;
            }
        }

        private void SongColorButtonOnClick(object sender, EventArgs e)
        {
            colorDialog.CustomColors = new[] { ColorTranslator.ToOle(_audioBandAppearance.NowPlayingTrackNameColor) };
            if (colorDialog.ShowDialog(this) == DialogResult.OK)
            {
                _audioBandAppearance.NowPlayingTrackNameColor = colorDialog.Color;
            }
        }

        private void ProgressColorButtonOnClick(object sender, EventArgs e)
        {
            colorDialog.CustomColors = new[] { ColorTranslator.ToOle(_audioBandAppearance.TrackProgressColor) };
            if (colorDialog.ShowDialog(this) == DialogResult.OK)
            {
                _audioBandAppearance.TrackProgressColor = colorDialog.Color;
            }
        }

        private void ProgressBackColorButtonOnClick(object sender, EventArgs e)
        {
            colorDialog.CustomColors = new[] { ColorTranslator.ToOle(_audioBandAppearance.TrackProgressBackColor) };
            if (colorDialog.ShowDialog(this) == DialogResult.OK)
            {
                _audioBandAppearance.TrackProgressBackColor = colorDialog.Color;
            }
        }
    }
}
