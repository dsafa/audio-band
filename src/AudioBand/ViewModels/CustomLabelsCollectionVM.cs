using AudioBand.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace AudioBand.ViewModels
{
    internal class CustomLabelsCollectionVM : ViewModelBase<CustomLabelsCollection>
    {
        public ObservableCollection<CustomLabelVM> CustomLabels { get; set; }

        public CustomLabelsCollectionVM(CustomLabelsCollection model) : base(model)
        {
            CustomLabels = new ObservableCollection<CustomLabelVM>(model.CustomLabels.Select(customLabel => new CustomLabelVM(customLabel)));
        }
    }
}
