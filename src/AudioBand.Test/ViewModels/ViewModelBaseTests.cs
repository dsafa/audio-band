using AudioBand.UI;
using Xunit;

namespace AudioBand.Test
{
    public class ViewModelBaseTests
    {
        [Fact]
        public void SetProperty_WithChangedField_CallsPropertyChanged()
        {
            string propertyName = null;
            var vm = new ViewModel();
            vm.PropertyChanged += (o, e) => { propertyName = e.PropertyName; };
            vm.Field = 10;

            Assert.Equal(nameof(ViewModel.Field), propertyName);
            Assert.Equal(10, vm.Field);
        }

        [Fact]
        public void SetProperty_SameField_DoesNotCallPropertyChanged()
        {
            int called = 0;
            string propertyName = null;
            var vm = new ViewModel();
            vm.PropertyChanged += (o, e) => { propertyName = e.PropertyName; called++; };
            vm.Field = 0;

            Assert.Equal(0, called);
            Assert.Null(propertyName);
            Assert.Equal(0, vm.Field);
        }

        [Fact]
        public void SetProperty_WithTrackedStateAttribute_AutomaticallyStartsEdit()
        {
            var vm = new ViewModel();
            vm.Field = 0;

            Assert.False(vm.IsEditing);
            vm.Field = 1;
            Assert.True(vm.IsEditing);
        }

        [Fact]
        public void SetProperty_WithoutTrackedStateAttribute_DoesNotStartEdit()
        {
            var vm = new ViewModel();
            vm.FieldNotTracked = 0;

            Assert.False(vm.IsEditing);
            vm.FieldNotTracked = 1;
            Assert.False(vm.IsEditing);
        }

        [Fact]
        public void SetPropertyWithModel_AutomaticallyStartsEdit()
        {
            var vm = new ViewModelWithModel();
            vm.Field = 0;

            Assert.False(vm.IsEditing);
            vm.Field = 1;
            Assert.True(vm.IsEditing);
        }

        [Fact]
        public void ResetViewModelWithModel_StartsEdit()
        {
            var vm = new ViewModelWithModel();
            vm.Reset();

            Assert.True(vm.IsEditing);
        }

        private class ViewModel : ViewModelBase
        {
            private int _field = 0;
            private int _notTracked = 0;

            [TrackState]
            public int Field
            {
                get => _field;
                set => SetProperty(ref _field, value);
            }

            public int FieldNotTracked
            {
                get => _notTracked;
                set => SetProperty(ref _notTracked, value);
            }
        }

        private class ViewModelWithModel : ViewModelBase
        {
            private Model model = new Model();

            public ViewModelWithModel()
            {
            }

            public ViewModelWithModel(Model m)
            {
                model = m;
            }

            [TrackState]
            public int Field
            {
                get => model.Value;
                set => SetProperty(model, nameof(ViewModelBaseTests.Model.Value), value);
            }
        }

        private class Model
        {
            public int Value { get; set; } = 0;
        }
    }
}
