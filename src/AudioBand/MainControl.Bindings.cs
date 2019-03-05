using System;
using System.Linq;
using System.Windows.Forms;
using AudioBand.Models;
using AudioBand.ViewModels;
using AudioBand.Views.Winforms;

namespace AudioBand
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    partial class MainControl : ICustomLabelHost
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        private const string CustomLabelControlsKey = "CustomLabel";

        /// <inheritdoc/>
        void ICustomLabelHost.AddCustomTextLabel(CustomLabelVM customLabel)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => AddCustomLabelText(customLabel)));
            }
            else
            {
                AddCustomLabelText(customLabel);
            }
        }

        /// <inheritdoc/>
        void ICustomLabelHost.RemoveCustomTextLabel(CustomLabelVM customLabel)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => RemoveCustomTextLabel(customLabel)));
            }
            else
            {
                RemoveCustomTextLabel(customLabel);
            }
        }

        private void InitializeBindingSources(
            AlbumArtPopupVM albumArtPopupVm,
            AlbumArtVM albumartVm,
            AudioBandVM audioBandVm,
            NextButtonVM nextButtonVm,
            PlayPauseButtonVM playPauseButtonVm,
            PreviousButtonVM previousButtonVm,
            ProgressBarVM progressBarVm)
        {
            AlbumArtPopupVMBindingSource.DataSource = albumArtPopupVm;
            AlbumArtVMBindingSource.DataSource = albumartVm;
            AudioBandVMBindingSource.DataSource = audioBandVm;
            NextButtonVMBindingSource.DataSource = nextButtonVm;
            PlayPauseButtonVMBindingSource.DataSource = playPauseButtonVm;
            PreviousButtonVMBindingSource.DataSource = previousButtonVm;
            ProgressBarVMBindingSource.DataSource = progressBarVm;
        }

        private void AddCustomLabelText(CustomLabelVM customLabel)
        {
            var label = new FormattedTextLabel(customLabel.FormatString, customLabel.Color, customLabel.FontSize, customLabel.FontFamily, customLabel.TextAlignment);
            var labelBindingSource = new BindingSource(components);
            var trackBindingSource = new BindingSource(components);
            labelBindingSource.DataSource = customLabel;
            trackBindingSource.DataSource = _trackModel;
            label.DataBindings.Add(new Binding(nameof(label.Format), labelBindingSource, nameof(customLabel.FormatString), true, DataSourceUpdateMode.OnPropertyChanged));
            label.DataBindings.Add(new Binding(nameof(label.DefaultColor), labelBindingSource, nameof(customLabel.Color), true, DataSourceUpdateMode.OnPropertyChanged));
            label.DataBindings.Add(new Binding(nameof(label.FontSize), labelBindingSource, nameof(customLabel.FontSize), true, DataSourceUpdateMode.OnPropertyChanged));
            label.DataBindings.Add(new Binding(nameof(label.FontFamily), labelBindingSource, nameof(customLabel.FontFamily), true, DataSourceUpdateMode.OnPropertyChanged));
            label.DataBindings.Add(new Binding(nameof(label.Alignment), labelBindingSource, nameof(customLabel.TextAlignment), true, DataSourceUpdateMode.OnPropertyChanged));
            label.DataBindings.Add(new Binding(nameof(label.Visible), labelBindingSource, nameof(customLabel.IsVisible), true, DataSourceUpdateMode.OnPropertyChanged));
            label.DataBindings.Add(new Binding(nameof(label.LogicalSize), labelBindingSource, nameof(customLabel.Size), true, DataSourceUpdateMode.OnPropertyChanged));
            label.DataBindings.Add(new Binding(nameof(label.LogicalLocation), labelBindingSource, nameof(customLabel.Location), true, DataSourceUpdateMode.OnPropertyChanged));
            label.DataBindings.Add(new Binding(nameof(label.ScrollSpeed), labelBindingSource, nameof(customLabel.ScrollSpeed), true, DataSourceUpdateMode.OnPropertyChanged));
            label.DataBindings.Add(new Binding(nameof(label.AlbumName), trackBindingSource, nameof(_trackModel.AlbumName), true, DataSourceUpdateMode.OnPropertyChanged));
            label.DataBindings.Add(new Binding(nameof(label.Artist), trackBindingSource, nameof(_trackModel.Artist), true, DataSourceUpdateMode.OnPropertyChanged));
            label.DataBindings.Add(new Binding(nameof(label.SongName), trackBindingSource, nameof(_trackModel.TrackName), true, DataSourceUpdateMode.OnPropertyChanged));
            label.DataBindings.Add(new Binding(nameof(label.SongLength), trackBindingSource, nameof(_trackModel.TrackLength), true, DataSourceUpdateMode.OnPropertyChanged));
            label.DataBindings.Add(new Binding(nameof(label.SongProgress), trackBindingSource, nameof(_trackModel.TrackProgress), true, DataSourceUpdateMode.OnPropertyChanged));

            label.Tag = customLabel;
            label.Name = CustomLabelControlsKey;
            Controls.Add(label);
        }

        private void RemoveCustomTextLabel(CustomLabelVM customLabel)
        {
            var control = Controls.Find(CustomLabelControlsKey, true)
                .Cast<FormattedTextLabel>()
                .FirstOrDefault(l => l.Tag == customLabel);

            if (control == null)
            {
                Logger.Warn("Tried removing label but the control was not found. Label: {@label}", customLabel);
                return;
            }

            Controls.Remove(control);
        }
    }
}
