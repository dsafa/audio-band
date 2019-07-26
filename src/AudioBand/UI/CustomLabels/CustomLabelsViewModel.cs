using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.UI
{
    /// <summary>
    /// Viewmodel for all the custom labels.
    /// </summary>
    public class CustomLabelsViewModel : ViewModelBase
    {
        private readonly List<CustomLabelViewModel> _added = new List<CustomLabelViewModel>();
        private readonly List<CustomLabelViewModel> _removed = new List<CustomLabelViewModel>();
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
        /// <param name="messageBus">The message bus.</param>
        public CustomLabelsViewModel(IAppSettings appsettings, IDialogService dialogService, IAudioSession audioSession, IMessageBus messageBus)
        {
            _appsettings = appsettings;
            _dialogService = dialogService;
            _audioSession = audioSession;
            UseMessageBus(messageBus);
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
            base.OnBeginEdit();

            _added.Clear();
            _removed.Clear();
        }

        /// <inheritdoc/>
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();

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
            base.OnEndEdit();

            _added.Clear();
            _removed.Clear();

            _appsettings.CurrentProfile.CustomLabels.Clear();

            foreach (var customLabelVm in CustomLabels)
            {
                _appsettings.CurrentProfile.CustomLabels.Add(customLabelVm.GetModel());
            }
        }

        private void AddLabelCommandOnExecute()
        {
            BeginEdit();

            var newLabel = new CustomLabelViewModel(new CustomLabel(), _dialogService, _audioSession, MessageBus) { Name = "New Label" };
            CustomLabels.Add(newLabel);

            _added.Add(newLabel);
        }

        private void RemoveLabelCommandOnExecute(CustomLabelViewModel labelViewModel)
        {
            if (!_dialogService.ShowConfirmationDialog(ConfirmationDialogType.DeleteLabel, labelViewModel.Name))
            {
                return;
            }

            BeginEdit();

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
            foreach (var customLabelVm in _appsettings.CurrentProfile.CustomLabels.Select(customLabel => new CustomLabelViewModel(customLabel, _dialogService, _audioSession, MessageBus)))
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
