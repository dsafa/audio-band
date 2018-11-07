using System.Linq;
using AudioBand.ViewModels;
using AudioBand.Views.Winforms;

namespace AudioBand
{
    partial class MainControl
    {
        private const string CustomLabelName = "CustomLabel";

        private void InitializeBindingSources(AlbumArtPopupVM albumArtPopupVm, AlbumArtVM albumartVm, AudioBandVM audioBandVm, NextButtonVM nextButtonVm,
            PlayPauseButtonVM playPauseButtonVm, PreviousButtonVM previousButtonVm, ProgressBarVM progressBarVm)
        {
            AlbumArtPopupVMBindingSource.DataSource = albumArtPopupVm;
            AlbumArtVMBindingSource.DataSource = albumartVm;
            AudioBandVMBindingSource.DataSource = audioBandVm;
            NextButtonVMBindingSource.DataSource = nextButtonVm;
            PlayPauseButtonVMBindingSource.DataSource = playPauseButtonVm;
            PreviousButtonVMBindingSource.DataSource = previousButtonVm;
            ProgressBarVMBindingSource.DataSource = progressBarVm;
        }

        internal void AddCustomTextLabel(CustomLabelVM customLabel)
        {
            var label = new FormattedTextLabel(customLabel.FormatString, customLabel.Color, customLabel.FontSize, customLabel.FontFamily, customLabel.TextAlignment);
            label.DataBindings.Add(nameof(label.Format), customLabel, nameof(customLabel.FormatString));
            label.DataBindings.Add(nameof(label.DefaultColor), customLabel, nameof(customLabel.Color));
            label.DataBindings.Add(nameof(label.FontSize), customLabel, nameof(customLabel.FontSize));
            label.DataBindings.Add(nameof(label.FontFamily), customLabel, nameof(customLabel.FontFamily));
            label.DataBindings.Add(nameof(label.Alignment), customLabel, nameof(customLabel.TextAlignment));
            label.DataBindings.Add(nameof(label.Visible), customLabel, nameof(customLabel.IsVisible));
            label.DataBindings.Add(nameof(label.Width), customLabel, nameof(customLabel.Width));
            label.DataBindings.Add(nameof(label.Height), customLabel, nameof(customLabel.Height));
            label.DataBindings.Add(nameof(label.Location), customLabel, nameof(customLabel.Location));
            label.DataBindings.Add(nameof(label.ScrollSpeed), customLabel, nameof(customLabel.ScrollSpeed));

            label.DataBindings.Add(nameof(label.AlbumName), _trackModel, nameof(_trackModel.AlbumName));
            label.DataBindings.Add(nameof(label.Artist), _trackModel, nameof(_trackModel.Artist));
            label.DataBindings.Add(nameof(label.SongName), _trackModel, nameof(_trackModel.TrackName));
            label.DataBindings.Add(nameof(label.SongLength), _trackModel, nameof(_trackModel.TrackLength));
            label.DataBindings.Add(nameof(label.SongProgress), _trackModel, nameof(_trackModel.TrackProgress));

            label.Tag = customLabel;
            Controls.Add(label);
        }

        internal void RemoveCustomTextLabel(CustomLabelVM customLabel)
        {
            var control = Controls.Find(CustomLabelName, true)
                .Cast<FormattedTextLabel>()
                .FirstOrDefault(l => l.Tag == customLabel);

            if (control == null)
            {
                _logger.Warn("Tried removing label but the control was not found");
                return;
            }

            Controls.Remove(control);
        }
    }
}
