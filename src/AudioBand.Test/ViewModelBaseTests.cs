using System;
using System.Windows.Controls;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.ViewModels;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
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
            vm._field = 0;
            vm.Field = 10;

            Assert.AreEqual(nameof(ViewModel.Field), propertyName);
            Assert.AreEqual(10, vm._field);
        }

        [TestMethod]
        public void SetPropertySameField()
        {
            int called = 0;
            string propertyName = null;
            var vm = new ViewModel();
            vm.PropertyChanged += (o, e) => { propertyName = e.PropertyName; called++; };
            vm._field = 0;
            vm.Field = 0;

            Assert.AreEqual(0, called);
            Assert.AreEqual(null, propertyName);
            Assert.AreEqual(0, vm._field);
        }

        [TestMethod]
        public void SetPropertyAutomaticallyStartsEdit()
        {
            var vm = new ViewModel();
            vm._field = 0;
            vm.Field = 0;

            Assert.IsFalse(vm.IsEditing);
            vm.Field = 1;
            Assert.IsTrue(vm.IsEditing);
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
        public void ViewModelWithModelSetupBindingsProperly()
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

            m.ModelField = 10;

            Assert.IsTrue(raised);
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

            vm.Unbind();

            m.ModelField = 10;

            Assert.IsFalse(raised);
        }

        private class ViewModel : ViewModelBase
        {
            public int _field;

            public int Field
            {
                get => _field;
                set => SetProperty(ref _field, value);
            }
        }

        private class ViewModelWithModel : ViewModelBase<Model>
        {
            public ViewModelWithModel() : base(new Model())
            {
            }

            public ViewModelWithModel(Model m) : base(m)
            {
            }

            [PropertyChangeBinding(nameof(ViewModelBaseTests.Model.ModelField))]
            public int Field
            {
                get => Model.ModelField;
                set => SetProperty(nameof(ViewModelBaseTests.Model.ModelField), value);
            }

            public void Unbind()
            {
                UnbindModel(Model);
            }
        }

        private class Model : ModelBase
        {
            private int _field = 0;

            public int ModelField
            {
                get => _field;
                set => SetProperty(ref _field, value);
            }
        }
    }
}
