using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Viewmodel for all the custom labels.
    /// </summary>
    public class CustomLabelsViewModel : ViewModelBase
    {
        private readonly HashSet<CustomLabelViewModel> _added = new HashSet<CustomLabelViewModel>();
        private readonly HashSet<CustomLabelViewModel> _removed = new HashSet<CustomLabelViewModel>();
        private readonly IAppSettings _appsettings;
        private readonly IDialogService _dialogService;
        private readonly IAudioSession _audioSession;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLabelsViewModel"/> class
        /// with the list of custom labels and a label host.
        /// </summary>
        /// <param name="appsettings">The app setings.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="audioSession">The audio session.</param>
        public CustomLabelsViewModel(IAppSettings appsettings, IDialogService dialogService, IAudioSession audioSession)
        {
            _appsettings = appsettings;
            _dialogService = dialogService;
            _audioSession = audioSession;
            SetupLabels();

            AddLabelCommand = new RelayCommand(AddLabelCommandOnExecute);
            RemoveLabelCommand = new RelayCommand<CustomLabelViewModel>(RemoveLabelCommandOnExecute);
            _appsettings.ProfileChanged += AppsettingsOnProfileChanged;
        }

        /// <summary>
        /// Gets the collection of custom label viewmodels.
        /// </summary>
        public ObservableCollection<CustomLabelViewModel> CustomLabels { get; } = new ObservableCollection<CustomLabelViewModel>();

        /// <summary>
        /// Gets the command to add a new label.
        /// </summary>
        public RelayCommand AddLabelCommand { get; }

        /// <summary>
        /// Gets the command to remove a label.
        /// </summary>
        public RelayCommand<CustomLabelViewModel> RemoveLabelCommand { get; }

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

            var newLabel = new CustomLabelViewModel(new CustomLabel(), _dialogService, _audioSession) { Name = "New Label" };
            CustomLabels.Add(newLabel);

            _added.Add(newLabel);
        }

        private void RemoveLabelCommandOnExecute(CustomLabelViewModel labelViewModel)
        {
            BeginEdit();

            if (!_dialogService.ShowConfirmationDialog(ConfirmationDialogType.DeleteLabel, labelViewModel.Name))
            {
                return;
            }

            CustomLabels.Remove(labelViewModel);

            // Only add to removed if not a new label
            if (!_added.Remove(labelViewModel))
            {
                _removed.Add(labelViewModel);
            }
        }

        private void SetupLabels()
        {
            CustomLabels.Clear();
            foreach (var customLabelVm in _appsettings.CustomLabels.Select(customLabel => new CustomLabelViewModel(customLabel, _dialogService, _audioSession)))
            {
                CustomLabels.Add(customLabelVm);
            }
        }

        private void AppsettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing while profile is changing");

            // Add labels for new profile
            SetupLabels();
        }
    }
}
