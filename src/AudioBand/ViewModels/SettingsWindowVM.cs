using AudioBand.Commands;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the settings window.
    /// </summary>
    public class SettingsWindowVM : ViewModelBase
    {
        private object _selectedVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindowVM"/> class.
        /// </summary>
        public SettingsWindowVM()
        {
            SelectViewModelCommand = new RelayCommand<object>(SelectViewModelOnExecute);
        }

        /// <summary>
        /// Gets or sets the currently selected view model.
        /// </summary>
        public object SelectedVM
        {
            get => _selectedVM;
            set
            {
                SetProperty(ref _selectedVM, value);

                // Start object editing when view model is selected. Maybe take another look in the future.
                if (value is ViewModelBase vm && !vm.IsEditing)
                {
                    vm.BeginEdit();
                }
            }
        }

        /// <summary>
        /// Gets the command to select the view model.
        /// </summary>
        public RelayCommand<object> SelectViewModelCommand { get; private set; }

        private void SelectViewModelOnExecute(object newViewmodel)
        {
            SelectedVM = newViewmodel;
        }
    }
}
