using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AudioBand.ViewModels
{
    internal class SettingsWindowVM : INotifyPropertyChanged
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
        public AudioSourceSettingsCollectionVM AudioSourceSettingsVM { get; set; }

        private object _selectedVM;
        public object SelectedVM
        {
            get => _selectedVM;
            set
            {
                if (Equals(value, _selectedVM))
                {
                    return;
                }

                _selectedVM = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
