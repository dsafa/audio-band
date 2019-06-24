using AudioBand.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AudioBand.Test
{
    [TestClass]
    public class ViewModelBaseTests
    {
        [TestMethod]
        public void SetPropertyWithChangedFieldCallsPropertyChanged()
        {
            string propertyName = null;
            var vm = new ViewModel();
            vm.PropertyChanged += (o, e) => { propertyName = e.PropertyName; };
            vm.Field = 10;

            Assert.AreEqual(nameof(ViewModel.Field), propertyName);
            Assert.AreEqual(10, vm.Field);
        }

        [TestMethod]
        public void SetPropertySameField()
        {
            int called = 0;
            string propertyName = null;
            var vm = new ViewModel();
            vm.PropertyChanged += (o, e) => { propertyName = e.PropertyName; called++; };
            vm.Field = 0;

            Assert.AreEqual(0, called);
            Assert.AreEqual(null, propertyName);
            Assert.AreEqual(0, vm.Field);
        }

        [TestMethod]
        public void SetPropertyWithTrackedStateAttributeAutomaticallyStartsEdit()
        {
            var vm = new ViewModel();
            vm.Field = 0;

            Assert.IsFalse(vm.IsEditing);
            vm.Field = 1;
            Assert.IsTrue(vm.IsEditing);
        }

        [TestMethod]
        public void SetPropertyWithoutTrackedStateAttributeDoesNotStartEdit()
        {
            var vm = new ViewModel();
            vm.FieldNotTracked = 0;

            Assert.IsFalse(vm.IsEditing);
            vm.FieldNotTracked = 1;
            Assert.IsFalse(vm.IsEditing);
        }

        [TestMethod]
        public void SetPropertyWithModelAutomaticallyStartsEdit()
        {
            var vm = new ViewModelWithModel();
            vm.Field = 0;

            Assert.IsFalse(vm.IsEditing);
            vm.Field = 1;
            Assert.IsTrue(vm.IsEditing);
        }

        [TestMethod]
        public void ResetViewModelWithModelStartsEdit()
        {
            var vm = new ViewModelWithModel();
            vm.Reset();
            
            Assert.IsTrue(vm.IsEditing);
        }

        [TestMethod]
        public void ViewModelWithModelUnbindModelProperly()
        {
            var m = new Model();
            var vm = new ViewModelWithModel(m);

            bool raised = false;
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(ViewModelWithModel.Field))
                {
                    raised = true;
                }
            };

            Assert.IsFalse(raised);
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
