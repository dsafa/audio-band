using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AudioBand.ViewModels;
using AudioBand.Models;
using Moq;

namespace AudioBand.Test
{
    /// <summary>
    /// Summary description for CustomLabelsViewModel
    /// </summary>
    [TestClass]
    public class CustomLabelsViewModelTests
    {
        private Mock<ICustomLabelService> _hostMock;
        private Mock<IDialogService> _dialogMock;
        private CustomLabel _label;
        private CustomLabelsVM _vm;

        [TestInitialize]
        public void Init()
        {
            _hostMock = new Mock<ICustomLabelService>();
            _dialogMock = new Mock<IDialogService>();
            _label = new CustomLabel();
            _vm = new CustomLabelsVM(new List<CustomLabel>(), _hostMock.Object);
        }

        [TestMethod]
        public void AddLabel()
        {
            _vm.AddLabelCommand.Execute(null);
            
            Assert.AreEqual(1, _vm.CustomLabels.Count);
            _hostMock.Verify(o => o.AddCustomTextLabel(It.IsAny<CustomLabelVM>()), Times.Once);
        }

        [TestMethod]
        public async Task RemoveLabel_confirm()
        {
            _dialogMock.Setup(o => o.ShowConfirmationDialogAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            _vm.DialogService = _dialogMock.Object;
            _vm.AddLabelCommand.Execute(null);
            var newLabel = _vm.CustomLabels[0];
            await _vm.RemoveLabelCommand.ExecuteAsync(newLabel);

            Assert.AreEqual(0, _vm.CustomLabels.Count);
            _hostMock.Verify(o => o.RemoveCustomTextLabel(newLabel), Times.Once);
            _dialogMock.Verify(o => o.ShowConfirmationDialogAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task RemoveLabel_deny()
        {
            _dialogMock.Setup(o => o.ShowConfirmationDialogAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(false));
            _vm.DialogService = _dialogMock.Object;
            _vm.AddLabelCommand.Execute(null);
            var newLabel = _vm.CustomLabels[0];
            await _vm.RemoveLabelCommand.ExecuteAsync(newLabel);

            Assert.AreEqual(1, _vm.CustomLabels.Count);
            _hostMock.Verify(o => o.RemoveCustomTextLabel(newLabel), Times.Never);
            _dialogMock.Verify(o => o.ShowConfirmationDialogAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void AddLabel_ThenCancel()
        {
            _vm.BeginEdit();
            _vm.AddLabelCommand.Execute(null);
            var newLabel = _vm.CustomLabels[0];
            _vm.CancelEdit();

            Assert.AreEqual(0, _vm.CustomLabels.Count);
            _hostMock.Verify(o => o.AddCustomTextLabel(It.IsAny<CustomLabelVM>()), Times.Once);
            _hostMock.Verify(o => o.RemoveCustomTextLabel(newLabel), Times.Once);
        }

        [TestMethod]
        public async Task RemoveLabel_ThenCancel()
        {
            var host = new Mock<ICustomLabelService>();
            var dialog = new Mock<IDialogService>();
            var vm = new CustomLabelsVM(new List<CustomLabel> { new CustomLabel()}, host.Object);
            vm.BeginEdit();

            dialog.Setup(o => o.ShowConfirmationDialogAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            vm.DialogService = dialog.Object;
            var label = vm.CustomLabels[0];
            await vm.RemoveLabelCommand.ExecuteAsync(label);
            vm.CancelEdit();

            Assert.AreEqual(1, vm.CustomLabels.Count);
            Assert.AreEqual(label, vm.CustomLabels[0]);
            host.Verify(o => o.RemoveCustomTextLabel(label), Times.Once);
        }

        [TestMethod]
        public async Task AddRemoveLabel_ThenCancel()
        {
            _vm.BeginEdit();
            _dialogMock.Setup(o => o.ShowConfirmationDialogAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            _vm.DialogService = _dialogMock.Object;
            _vm.AddLabelCommand.Execute(null);
            var newLabel = _vm.CustomLabels[0];
            await _vm.RemoveLabelCommand.ExecuteAsync(newLabel);
            _vm.CancelEdit();

            Assert.AreEqual(0, _vm.CustomLabels.Count);
            _hostMock.Verify(o => o.AddCustomTextLabel(It.IsAny<CustomLabelVM>()), Times.Once);
            _hostMock.Verify(o => o.RemoveCustomTextLabel(newLabel), Times.Once);
        }
    }
}
