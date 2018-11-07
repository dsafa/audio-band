using System.Collections.Generic;
using AudioBand.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace AudioBand.ViewModels
{
    internal class CustomLabelsVM
    {
        public ObservableCollection<CustomLabelVM> CustomLabels { get; set; }
        private MainControl _control;

        public CustomLabelsVM(List<CustomLabel> customLabels, MainControl control)
        {
            CustomLabels = new ObservableCollection<CustomLabelVM>(customLabels.Select(customLabel => new CustomLabelVM(customLabel)));
            _control = control;
        }
    }
}
