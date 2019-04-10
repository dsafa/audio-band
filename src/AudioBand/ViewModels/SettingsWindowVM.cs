using AudioBand.Commands;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the settings window.
    /// </summary>
    public class SettingsWindowVM : ViewModelBase
    {
        private ViewModelBase _selectedVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindowVM"/> class.
        /// </summary>
        public SettingsWindowVM()
        {
            SelectViewModelCommand = new RelayCommand<ViewModelBase>(SelectViewModelOnExecute);
        }

        /// <summary>
        /// Gets or sets the currently selected view model.
        /// </summary>
        public ViewModelBase SelectedVM
        {
            get => _selectedVM;
            set => SetProperty(ref _selectedVM, value, trackChanges: false);
        }

        /// <summary>
        /// Gets the command to select the view model.
        /// </summary>
        public RelayCommand<ViewModelBase> SelectViewModelCommand { get; private set; }

        private void SelectViewModelOnExecute(ViewModelBase newViewmodel)
        {
            SelectedVM = newViewmodel;
        }
    }
}
