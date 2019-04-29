using System;
using System.Collections.Generic;
using System.Linq;
using AudioBand.Commands;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the rename profile dialog.
    /// </summary>
    public class RenameProfileDialogVM : ViewModelBase
    {
        private readonly IEnumerable<string> _profileNames;
        private string _currentName;
        private string _newProfileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenameProfileDialogVM"/> class.
        /// </summary>
        /// <param name="currentName">The current profile name.</param>
        /// <param name="profileNames">The list of existing profile names.</param>
        public RenameProfileDialogVM(string currentName, IEnumerable<string> profileNames)
        {
            _currentName = currentName;
            _newProfileName = currentName;
            _profileNames = profileNames;

            OkCommand = new RelayCommand(OkCommandOnExecute, OkCommandCanExecute);
            OkCommand.Observe(this, nameof(NewProfileName));
            CancelCommand = new RelayCommand(CancelCommandOnExecute);
        }

        /// <summary>
        /// Occurs when the new name is accepted.
        /// </summary>
        public event EventHandler Accepted;

        /// <summary>
        /// Occurs when the new dialog is canceled.
        /// </summary>
        public event EventHandler Canceled;

        /// <summary>
        /// Gets or sets the current name.
        /// </summary>
        public string CurrentName
        {
            get => _currentName;
            set => SetProperty(ref _currentName, value, trackChanges: false);
        }

        /// <summary>
        /// Gets or sets the new profile name.
        /// </summary>
        public string NewProfileName
        {
            get => _newProfileName;
            set
            {
                if (value == _currentName)
                {
                    return;
                }

                if (_profileNames.Contains(value) || string.IsNullOrEmpty(value))
                {
                    RaiseValidationError("Invalid name");
                }
                else
                {
                    ClearErrors();
                }

                SetProperty(ref _newProfileName, value, trackChanges: false);
            }
        }

        /// <summary>
        /// Gets the ok command
        /// </summary>
        public RelayCommand OkCommand { get; }

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        public RelayCommand CancelCommand { get; }

        private void CancelCommandOnExecute(object obj)
        {
            Canceled?.Invoke(this, EventArgs.Empty);
        }

        private void OkCommandOnExecute(object obj)
        {
            Accepted?.Invoke(this, EventArgs.Empty);
        }

        private bool OkCommandCanExecute(object obj)
        {
            return !HasErrors;
        }
    }
}
