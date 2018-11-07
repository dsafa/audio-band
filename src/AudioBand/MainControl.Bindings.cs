using AudioBand.ViewModels;

namespace AudioBand
{
    partial class MainControl
    {
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
    }
}
