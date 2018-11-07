using AudioBand.Commands;
using AudioBand.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AudioBand.ViewModels
{
    internal class CustomLabelsVM
    {
        private readonly MainControl _control;

        public ObservableCollection<CustomLabelVM> CustomLabels { get; set; }
        public RelayCommand AddLabelCommand { get; }
        public RelayCommand<CustomLabelVM> RemoveLabelCommand { get; }

        public CustomLabelsVM(List<CustomLabel> customLabels, MainControl control)
        {
            CustomLabels = new ObservableCollection<CustomLabelVM>(customLabels.Select(customLabel => new CustomLabelVM(customLabel)));
            _control = control;

            foreach (var customLabelVm in CustomLabels)
            {
                _control.AddCustomTextLabel(customLabelVm);
            }

            AddLabelCommand = new RelayCommand(AddLabelCommandOnExecute);
            RemoveLabelCommand = new RelayCommand<CustomLabelVM>(RemoveLabelCommandOnExecute);
        }

        private void AddLabelCommandOnExecute(object o)
        {
            var newLabel = new CustomLabelVM(new CustomLabel());
            CustomLabels.Add(newLabel);
            _control.AddCustomTextLabel(newLabel);
        }

        private void RemoveLabelCommandOnExecute(CustomLabelVM labelVm)
        {
            CustomLabels.Remove(labelVm);
            _control.RemoveCustomTextLabel(labelVm);
        }
    }
}
