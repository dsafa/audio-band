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
        private readonly ICustomLabelHost _labelHost;
        private readonly HashSet<CustomLabelVM> _added = new HashSet<CustomLabelVM>();
        private readonly HashSet<CustomLabelVM> _removed = new HashSet<CustomLabelVM>();

        public ObservableCollection<CustomLabelVM> CustomLabels { get; }
        public RelayCommand AddLabelCommand { get; }
        public AsyncRelayCommand<CustomLabelVM> RemoveLabelCommand { get; }
        public IDialogService DialogService { get; set; }

        public CustomLabelsVM(List<CustomLabel> customLabels, ICustomLabelHost labelHost)
        {
            CustomLabels = new ObservableCollection<CustomLabelVM>(customLabels.Select(customLabel => new CustomLabelVM(customLabel)));
            _labelHost = labelHost;

            foreach (var customLabelVm in CustomLabels)
            {
                _labelHost.AddCustomTextLabel(customLabelVm);
            }

            AddLabelCommand = new RelayCommand(AddLabelCommandOnExecute);
            RemoveLabelCommand = new AsyncRelayCommand<CustomLabelVM>(RemoveLabelCommandOnExecute);
        }

        private void AddLabelCommandOnExecute(object o)
        {
            var newLabel = new CustomLabelVM(new CustomLabel()) {Name = "New Label"};
            CustomLabels.Add(newLabel);
            _labelHost.AddCustomTextLabel(newLabel);

            _added.Add(newLabel);
        }

        private async Task RemoveLabelCommandOnExecute(CustomLabelVM labelVm)
        {
            if (!await DialogService.ShowConfirmationDialogAsync("Delete Label", $"Are you sure you want to delete the label '{labelVm.Name}'?"))
            {
                return;
            }

            CustomLabels.Remove(labelVm);
            _labelHost.RemoveCustomTextLabel(labelVm);

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
                _labelHost.RemoveCustomTextLabel(label);
            }

            foreach (var label in _removed)
            {
                CustomLabels.Add(label);
                _labelHost.AddCustomTextLabel(label);
            }

            _added.Clear();
            _removed.Clear();

            foreach (var customLabelVm in CustomLabels)
            {
                customLabelVm.CancelEdit();
            }
        }
    }

    internal interface ICustomLabelHost
    {
        void AddCustomTextLabel(CustomLabelVM label);
        void RemoveCustomTextLabel(CustomLabelVM label);
    }
}
