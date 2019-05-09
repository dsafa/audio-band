using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using AudioBand.Commands;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Viewmodel for all the custom labels.
    /// </summary>
    public class CustomLabelsVM : ViewModelBase
    {
        private readonly HashSet<CustomLabelVM> _added = new HashSet<CustomLabelVM>();
        private readonly HashSet<CustomLabelVM> _removed = new HashSet<CustomLabelVM>();
        private readonly IAppSettings _appsettings;
        private readonly IDialogService _dialogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLabelsVM"/> class
        /// with the list of custom labels and a label host.
        /// </summary>
        /// <param name="appsettings">The app setings.</param>
        /// <param name="dialogService">The dialog service.</param>
        public CustomLabelsVM(IAppSettings appsettings, IDialogService dialogService)
        {
            _appsettings = appsettings;
            _dialogService = dialogService;
            SetupLabels();

            AddLabelCommand = new RelayCommand(AddLabelCommandOnExecute);
            RemoveLabelCommand = new RelayCommand<CustomLabelVM>(RemoveLabelCommandOnExecute);
            _appsettings.ProfileChanged += AppsettingsOnProfileChanged;
        }

        /// <summary>
        /// Gets the collection of custom label viewmodels.
        /// </summary>
        public ObservableCollection<CustomLabelVM> CustomLabels { get; } = new ObservableCollection<CustomLabelVM>();

        /// <summary>
        /// Gets the command to add a new label.
        /// </summary>
        public RelayCommand AddLabelCommand { get; }

        /// <summary>
        /// Gets the command to remove a label.
        /// </summary>
        public RelayCommand<CustomLabelVM> RemoveLabelCommand { get; }

        /// <inheritdoc/>
        protected override void OnBeginEdit()
        {
            _added.Clear();
            _removed.Clear();
        }

        /// <inheritdoc/>
        protected override void OnCancelEdit()
        {
            foreach (var label in _added)
            {
                CustomLabels.Remove(label);
            }

            foreach (var label in _removed)
            {
                CustomLabels.Add(label);
            }

            _added.Clear();
            _removed.Clear();
        }

        /// <inheritdoc/>
        protected override void OnEndEdit()
        {
            _added.Clear();
            _removed.Clear();

            _appsettings.CustomLabels.Clear();

            foreach (var customLabelVm in CustomLabels)
            {
                _appsettings.CustomLabels.Add(customLabelVm.GetModel());
            }
        }

        private void AddLabelCommandOnExecute(object o)
        {
            BeginEdit();

            var newLabel = new CustomLabelVM(new CustomLabel(), _dialogService) { Name = "New Label" };
            CustomLabels.Add(newLabel);

            _added.Add(newLabel);
        }

        private void RemoveLabelCommandOnExecute(CustomLabelVM labelVm)
        {
            BeginEdit();

            if (!_dialogService.ShowConfirmationDialog(ConfirmationDialogType.DeleteLabel, labelVm.Name))
            {
                return;
            }

            CustomLabels.Remove(labelVm);

            // Only add to removed if not a new label
            if (!_added.Remove(labelVm))
            {
                _removed.Add(labelVm);
            }
        }

        private void SetupLabels()
        {
            CustomLabels.Clear();
            foreach (var customLabelVm in _appsettings.CustomLabels.Select(customLabel => new CustomLabelVM(customLabel, _dialogService)))
            {
                CustomLabels.Add(customLabelVm);
            }
        }

        private void AppsettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing while profile is changing");

            CustomLabels.Clear();

            // Add labels for new profile
            SetupLabels();
        }
    }
}
