using System;
using AudioBand.Commands;
using AudioBand.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AudioBand.ViewModels
{
    internal class CustomLabelsVM : ViewModelBase
    {
        private readonly MainControl _control;
        private readonly HashSet<CustomLabelVM> _added = new HashSet<CustomLabelVM>();
        private readonly HashSet<CustomLabelVM> _removed = new HashSet<CustomLabelVM>();

        public ObservableCollection<CustomLabelVM> CustomLabels { get; }
        public RelayCommand AddLabelCommand { get; }
        public AsyncRelayCommand<CustomLabelVM> RemoveLabelCommand { get; }
        public IDialogService DialogService { get; set; }

        public CustomLabelsVM(List<CustomLabel> customLabels, MainControl control)
        {
            CustomLabels = new ObservableCollection<CustomLabelVM>(customLabels.Select(customLabel => new CustomLabelVM(customLabel)));
            _control = control;

            foreach (var customLabelVm in CustomLabels)
            {
                _control.AddCustomTextLabel(customLabelVm);
            }

            AddLabelCommand = new RelayCommand(AddLabelCommandOnExecute);
            RemoveLabelCommand = new AsyncRelayCommand<CustomLabelVM>(RemoveLabelCommandOnExecute);
        }

        private void AddLabelCommandOnExecute(object o)
        {
            var newLabel = new CustomLabelVM(new CustomLabel()) {Name = "New Label"};
            CustomLabels.Add(newLabel);
            _control.AddCustomTextLabel(newLabel);

            _added.Add(newLabel);
        }

        private async Task RemoveLabelCommandOnExecute(CustomLabelVM labelVm)
        {
            if (!await DialogService.ShowConfirmationDialogAsync("Delete Label", $"Are you sure you want to delete the label '{labelVm.Name}'?"))
            {
                return;
            }

            CustomLabels.Remove(labelVm);
            _control.RemoveCustomTextLabel(labelVm);

            // Only add to removed if not a new label
            if (!_added.Remove(labelVm))
            {
                _removed.Add(labelVm);
            }
        }

        protected override void OnBeginEdit()
        {
            base.OnBeginEdit();
            _added.Clear();
            _removed.Clear();

            foreach (var customLabelVm in CustomLabels)
            {
                customLabelVm.BeginEdit();
            }
        }

        protected override void OnCancelEdit()
        {
            foreach (var label in _added)
            {
                CustomLabels.Remove(label);
                _control.RemoveCustomTextLabel(label);
            }

            foreach (var label in _removed)
            {
                CustomLabels.Add(label);
                _control.AddCustomTextLabel(label);
            }

            _added.Clear();
            _removed.Clear();

            foreach (var customLabelVm in CustomLabels)
            {
                customLabelVm.CancelEdit();
            }
        }
    }
}
