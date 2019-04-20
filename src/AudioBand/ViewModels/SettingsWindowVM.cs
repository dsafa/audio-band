using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using AudioBand.Commands;
using AudioBand.Messages;
using AudioBand.Settings;
using PubSub.Extension;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the settings window.
    /// </summary>
    public class SettingsWindowVM : ViewModelBase
    {
        private readonly IAppSettings _appSettings;
        private readonly IDialogService _dialogService;
        private ViewModelBase _selectedVM;
        private string _selectedProfileName;
        private bool _hasUnsavedChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindowVM"/> class.
        /// </summary>
        /// <param name="appSettings">The app settings</param>
        /// <param name="dialogService">The dialog service</param>
        public SettingsWindowVM(IAppSettings appSettings, IDialogService dialogService)
        {
            _appSettings = appSettings;
            _dialogService = dialogService;
            _selectedProfileName = appSettings.CurrentProfile;
            Profiles = new ObservableCollection<string>(appSettings.Profiles);
            SelectViewModelCommand = new RelayCommand<ViewModelBase>(SelectViewModelOnExecute);
            DeleteProfileCommand = new RelayCommand<string>(DeleteProfileCommandOnExecute, DeleteProfileCommandCanExecute);
            DeleteProfileCommand.Observe(Profiles);
            AddProfileCommand = new RelayCommand(AddProfileCommandOnExecute);
            RenameProfileCommand = new RelayCommand(RenameProfileCommandOnExecute);

            this.Subscribe<StartEditMessage>(StartEditMessageHandler);
            this.Subscribe<EndEditMessage>(EndEditMessageHandler);
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
        /// Gets or sets the selected profile name
        /// </summary>
        public string SelectedProfileName
        {
            get => _selectedProfileName;
            set
            {
                if (SetProperty(ref _selectedProfileName, value, trackChanges: false))
                {
                    this.Publish(EndEditMessage.AcceptEdits);
                    _appSettings.CurrentProfile = value;
                }
            }
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
        public RelayCommand<ViewModelBase> SelectViewModelCommand { get; }

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

        private void SelectViewModelOnExecute(ViewModelBase newViewmodel)
        {
            SelectedVM = newViewmodel;
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

        private void StartEditMessageHandler(StartEditMessage obj)
        {
            _hasUnsavedChanges = true;
        }

        private void EndEditMessageHandler(EndEditMessage obj)
        {
            _hasUnsavedChanges = false;
        }
    }
}
