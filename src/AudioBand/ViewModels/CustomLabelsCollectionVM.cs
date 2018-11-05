using AudioBand.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace AudioBand.ViewModels
{
    internal class CustomLabelsCollectionVM : ViewModelBase<CustomLabelsCollection>
    {
        public ObservableCollection<CustomLabelVM> CustomLabels { get; set; }
        private MainControl _control;

        public CustomLabelsCollectionVM(CustomLabelsCollection model, MainControl control) : base(model)
        {
            CustomLabels = new ObservableCollection<CustomLabelVM>(model.CustomLabels.Select(customLabel => new CustomLabelVM(customLabel)));
            _control = control;
        }
    }
}
