using System;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubSub.Extension;

namespace AudioBand.Test
{
    [TestClass]
    public class ViewModelBaseTests
    {
        [TestMethod]
        public void SetPropertyWithChangedFieldCallsPropertyChanged()
        {
            int called = 0;
            string propertyName = null;
            var vm = new ViewModel();
            vm.PropertyChanged += (o, e) => { propertyName = e.PropertyName; called++; };
            vm._field = 0;
            vm.Field = 10;

            Assert.AreEqual(1, called);
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
        public void ListensForEndEditMessage()
        {
            var vm = new ViewModel();
            vm.BeginEdit();

            Assert.IsTrue(vm.IsEditing);
            this.Publish(EndEditMessage.AcceptEdits);
            Assert.IsFalse(vm.IsEditing);
        }

        [TestMethod]
        public void ListensForCancelEditMessage()
        {
            var vm = new ViewModel();
            vm.BeginEdit();

            Assert.IsTrue(vm.IsEditing);
            this.Publish(EndEditMessage.CancelEdits);
            Assert.IsFalse(vm.IsEditing);
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

            [PropertyChangeBinding(nameof(ViewModelBaseTests.Model.ModelField))]
            public int Field
            {
                get => Model.ModelField;
                set => SetProperty(nameof(ViewModelBaseTests.Model.ModelField), value);
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
