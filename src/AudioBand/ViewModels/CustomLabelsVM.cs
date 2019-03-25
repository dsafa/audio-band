using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly ICustomLabelService _labelService;
        private readonly HashSet<CustomLabelVM> _added = new HashSet<CustomLabelVM>();
        private readonly HashSet<CustomLabelVM> _removed = new HashSet<CustomLabelVM>();
        private readonly List<CustomLabel> _customLabels;
        private readonly IDialogService _dialogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLabelsVM"/> class
        /// with the list of custom labels and a label host.
        /// </summary>
        /// <param name="appsettings">The app setings.</param>
        /// <param name="labelService">The host for the labels.</param>
        /// <param name="dialogService">The dialog service</param>
        public CustomLabelsVM(IAppSettings appsettings, ICustomLabelService labelService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _customLabels = appsettings.CustomLabels;
            CustomLabels = new ObservableCollection<CustomLabelVM>(_customLabels.Select(customLabel => new CustomLabelVM(customLabel, dialogService)));
            _labelService = labelService;

            foreach (var customLabelVm in CustomLabels)
            {
                _labelService.AddCustomTextLabel(customLabelVm);
            }

            AddLabelCommand = new RelayCommand(AddLabelCommandOnExecute);
            RemoveLabelCommand = new RelayCommand<CustomLabelVM>(RemoveLabelCommandOnExecute);
        }

        /// <summary>
        /// Gets the collection of custom label viewmodels.
        /// </summary>
        public ObservableCollection<CustomLabelVM> CustomLabels { get; }

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
                _labelService.RemoveCustomTextLabel(label);
            }

            foreach (var label in _removed)
            {
                CustomLabels.Add(label);
                _labelService.AddCustomTextLabel(label);
            }

            _added.Clear();
            _removed.Clear();
        }

        /// <inheritdoc/>
        protected override void OnEndEdit()
        {
            _added.Clear();
            _removed.Clear();

            _customLabels.Clear();

            foreach (var customLabelVm in CustomLabels)
            {
                _customLabels.Add(customLabelVm.GetModel());
            }
        }

        private void AddLabelCommandOnExecute(object o)
        {
            BeginEdit();

            var newLabel = new CustomLabelVM(new CustomLabel(), _dialogService) { Name = "New Label" };
            CustomLabels.Add(newLabel);
            _labelService.AddCustomTextLabel(newLabel);

            _added.Add(newLabel);
        }

        private void RemoveLabelCommandOnExecute(CustomLabelVM labelVm)
        {
            BeginEdit();

            if (!_dialogService.ShowConfirmationDialog("Delete Label", $"Are you sure you want to delete the label '{labelVm.Name}'?"))
            {
                return;
            }

            CustomLabels.Remove(labelVm);
            _labelService.RemoveCustomTextLabel(labelVm);

            // Only add to removed if not a new label
            if (!_added.Remove(labelVm))
            {
                _removed.Add(labelVm);
            }
        }
    }
}
