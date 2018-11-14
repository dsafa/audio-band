using System.Collections.Generic;

namespace AudioBand.ViewModels
{
    internal class SettingsWindowVM : ViewModelBase
    {
        public AudioBandVM AudioBandVM { get; set; }
        public AlbumArtPopupVM AlbumArtPopupVM { get; set; }
        public AlbumArtVM AlbumArtVM { get; set; }
        public CustomLabelsVM CustomLabelsVM { get; set; }
        public HelpVM HelpVM { get; set; }
        public NextButtonVM NextButtonVM { get; set; }
        public PlayPauseButtonVM PlayPauseButtonVM { get; set; }
        public PreviousButtonVM PreviousButtonVM { get; set; }
        public ProgressBarVM ProgressBarVM { get; set; }
        public List<AudioSourceSettingsVM> AudioSourceSettingsVM { get; set; }

        private object _selectedVM;
        public object SelectedVM
        {
            get => _selectedVM;
            set => SetProperty(ref _selectedVM, value);
        }

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
