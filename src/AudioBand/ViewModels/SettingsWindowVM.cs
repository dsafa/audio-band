using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using AudioBand.Commands;
using AudioBand.Messages;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the settings window.
    /// </summary>
    public class SettingsWindowVM : ViewModelBase
    {
        private readonly IAppSettings _appSettings;
        private readonly IDialogService _dialogService;
        private readonly IMessageBus _messageBus;
        private readonly List<ViewModelBase> _dirtyViewModels = new List<ViewModelBase>();
        private ViewModelBase _selectedViewModel;
        private string _selectedProfileName;
        private bool _hasUnsavedChanges;
        private string _selectedViewHeader;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindowVM"/> class.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="viewModels">The view models.</param>
        /// <param name="messageBus">The message bus.</param>
        public SettingsWindowVM(IAppSettings appSettings, IDialogService dialogService, IViewModelContainer viewModels, IMessageBus messageBus)
        {
            _appSettings = appSettings;
            _dialogService = dialogService;
            _messageBus = messageBus;
            ViewModels = viewModels;
            _selectedProfileName = appSettings.CurrentProfile;
            Profiles = new ObservableCollection<string>(appSettings.Profiles);

            SelectViewModelCommand = new RelayCommand<object[]>(SelectViewModelOnExecute);
            DeleteProfileCommand = new RelayCommand<string>(DeleteProfileCommandOnExecute, DeleteProfileCommandCanExecute);
            DeleteProfileCommand.Observe(Profiles);
            AddProfileCommand = new RelayCommand(AddProfileCommandOnExecute);
            RenameProfileCommand = new RelayCommand(RenameProfileCommandOnExecute);
            SaveCommand = new RelayCommand(SaveCommandOnExecute, SaveCommandCanExecute);
            SaveCommand.Observe(this, nameof(HasUnsavedChanges));
            CloseCommand = new RelayCommand(CloseCommandOnExecute);

            ViewModels.CustomLabelsVM.PropertyChanged += ViewModelOnEditChanged;
        }

        /// <summary>
        /// Gets the view models.
        /// </summary>
        public IViewModelContainer ViewModels { get; }

        /// <summary>
        /// Gets or sets the currently selected view model.
        /// </summary>
        public ViewModelBase SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                var old = _selectedViewModel;
                if (SetProperty(ref _selectedViewModel, value, trackChanges: false))
                {
                    if (old != null)
                    {
                        old.PropertyChanged -= ViewModelOnEditChanged;
                    }

                    if (value != null)
                    {
                        value.PropertyChanged += ViewModelOnEditChanged;
                        if (value.IsEditing)
                        {
                            HandleViewModelEditing(value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected profile name.
        /// </summary>
        public string SelectedProfileName
        {
            get => _selectedProfileName;
            set
            {
                if (SetProperty(ref _selectedProfileName, value, trackChanges: false))
                {
                    EndEdits();
                    _appSettings.CurrentProfile = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the current title of the view.
        /// </summary>
        public string SelectedViewHeader
        {
            get => _selectedViewHeader;
            set => SetProperty(ref _selectedViewHeader, value, trackChanges: false);
        }

        /// <summary>
        /// Gets or sets a value indicating whether there are unsaved changes.
        /// </summary>
        public bool HasUnsavedChanges
        {
            get => _hasUnsavedChanges;
            set => SetProperty(ref _hasUnsavedChanges, value, trackChanges: false);
        }

        /// <summary>
        /// Gets the list of profiles
        /// </summary>
        public ObservableCollection<string> Profiles { get; }

        /// <summary>
        /// Gets the command to select the view model.
        /// </summary>
        public RelayCommand<object[]> SelectViewModelCommand { get; }

        /// <summary>
        /// Gets the command to delete a profile.
        /// </summary>
        public RelayCommand<string> DeleteProfileCommand { get; }

        /// <summary>
        /// Gets the command to add a profile.
        /// </summary>
        public RelayCommand AddProfileCommand { get; }

        /// <summary>
        /// Gets the command to rename the current profile.
        /// </summary>
        public RelayCommand RenameProfileCommand { get; }

        /// <summary>
        /// Gets the command to save settings.
        /// </summary>
        public RelayCommand SaveCommand { get; }

        /// <summary>
        /// Gets the command to close the settings editor.
        /// </summary>
        public RelayCommand CloseCommand { get; }

        private void SelectViewModelOnExecute(object[] data)
        {
            SelectedViewModel = data[0] as ViewModelBase;
            SelectedViewHeader = data[1] as string;
        }

        private void DeleteProfileCommandOnExecute(string profileName)
        {
            Debug.Assert(Profiles.Count > 1, "Should not be able to delete profiles if there is only one");

            var deleteConfirmed = _dialogService.ShowConfirmationDialog(ConfirmationDialogType.DeleteProfile, profileName);
            if (!deleteConfirmed)
            {
                return;
            }

            _appSettings.DeleteProfile(profileName);
            Profiles.Remove(profileName);

            _appSettings.CurrentProfile = Profiles[0];
            SelectedProfileName = Profiles[0];
        }

        private bool DeleteProfileCommandCanExecute(string obj)
        {
            return Profiles.Count > 1;
        }

        private void AddProfileCommandOnExecute(object o)
        {
            const string NewProfileName = "New Profile";
            string newprofile = NewProfileName;
            int count = 1;
            while (Profiles.Contains(newprofile))
            {
                newprofile = $"{NewProfileName} {count++}";
            }

            _appSettings.CreateProfile(newprofile);
            Profiles.Add(newprofile);
        }

        private void RenameProfileCommandOnExecute(object obj)
        {
            string newProfileName = _dialogService.ShowRenameDialog(SelectedProfileName, Profiles.ToList());
            if (newProfileName == null || newProfileName == SelectedProfileName)
            {
                return;
            }

            _appSettings.RenameCurrentProfile(newProfileName);
            var index = Profiles.IndexOf(SelectedProfileName);
            Profiles[index] = newProfileName;
            SelectedProfileName = newProfileName;
        }

        private void CloseCommandOnExecute(object obj)
        {
            if (!HasUnsavedChanges)
            {
                _messageBus.Publish(SettingsWindowMessage.CloseWindow);
                return;
            }

            var discardChanges = _dialogService.ShowConfirmationDialog(ConfirmationDialogType.DiscardChanges);
            if (discardChanges)
            {
                CancelEdits();
                _messageBus.Publish(SettingsWindowMessage.CloseWindow);
            }
        }

        private void SaveCommandOnExecute(object obj)
        {
            _appSettings.Save();
            EndEdits();
        }

        private bool SaveCommandCanExecute(object obj)
        {
            return HasUnsavedChanges;
        }

        private void EndEdits()
        {
            foreach (var dirtyViewModel in _dirtyViewModels)
            {
                dirtyViewModel.EndEdit();
            }

            _dirtyViewModels.Clear();
            HasUnsavedChanges = false;
        }

        private void CancelEdits()
        {
            foreach (var dirtyViewModel in _dirtyViewModels)
            {
                dirtyViewModel.CancelEdit();
            }

            _dirtyViewModels.Clear();
            HasUnsavedChanges = false;
        }

        private void ViewModelOnEditChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IsEditing))
            {
                return;
            }

            var vm = (ViewModelBase)sender;
            if (vm.IsEditing)
            {
                HandleViewModelEditing(vm);
            }
        }

        private void HandleViewModelEditing(ViewModelBase vm)
        {
            HasUnsavedChanges = true;
            _dirtyViewModels.Add(vm);
        }
    }
}
