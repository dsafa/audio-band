using System.Collections.Generic;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the settings window.
    /// </summary>
    internal class SettingsWindowVM : ViewModelBase
    {
        private object _selectedVM;

        public AudioBandVM AudioBandVM { get; set; }

        public AlbumArtPopupVM AlbumArtPopupVM { get; set; }

        public AlbumArtVM AlbumArtVM { get; set; }

        public CustomLabelsVM CustomLabelsVM { get; set; }

        public AboutVM AboutVm { get; set; }

        public NextButtonVM NextButtonVM { get; set; }

        public PlayPauseButtonVM PlayPauseButtonVM { get; set; }

        public PreviousButtonVM PreviousButtonVM { get; set; }

        public ProgressBarVM ProgressBarVM { get; set; }

        public List<AudioSourceSettingsVM> AudioSourceSettingsVM { get; set; }

        /// <summary>
        /// Gets or sets the currently selected view model.
        /// </summary>
        public object SelectedVM
        {
            get => _selectedVM;
            set => SetProperty(ref _selectedVM, value);
        }

        /// <inheritdoc/>
        protected override void OnBeginEdit()
        {
            base.OnBeginEdit();

            AudioBandVM.BeginEdit();
            AlbumArtPopupVM.BeginEdit();
            AlbumArtVM.BeginEdit();
            CustomLabelsVM.BeginEdit();
            NextButtonVM.BeginEdit();
            PlayPauseButtonVM.BeginEdit();
            PreviousButtonVM.BeginEdit();
            ProgressBarVM.BeginEdit();
            foreach (var audioSourceSettingsVm in AudioSourceSettingsVM)
            {
                audioSourceSettingsVm.BeginEdit();
            }
        }

        /// <inheritdoc/>
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();

            AudioBandVM.CancelEdit();
            AlbumArtPopupVM.CancelEdit();
            AlbumArtVM.CancelEdit();
            CustomLabelsVM.CancelEdit();
            NextButtonVM.CancelEdit();
            PlayPauseButtonVM.CancelEdit();
            PreviousButtonVM.CancelEdit();
            ProgressBarVM.CancelEdit();
            foreach (var audioSourceSettingsVm in AudioSourceSettingsVM)
            {
                audioSourceSettingsVm.CancelEdit();
            }
        }

        /// <inheritdoc/>
        protected override void OnEndEdit()
        {
            base.OnEndEdit();

            AudioBandVM.EndEdit();
            AlbumArtPopupVM.EndEdit();
            AlbumArtVM.EndEdit();
            CustomLabelsVM.EndEdit();
            NextButtonVM.EndEdit();
            PlayPauseButtonVM.EndEdit();
            PreviousButtonVM.EndEdit();
            ProgressBarVM.EndEdit();
            foreach (var audioSourceSettingsVm in AudioSourceSettingsVM)
            {
                audioSourceSettingsVm.EndEdit();
            }
        }
    }
}
