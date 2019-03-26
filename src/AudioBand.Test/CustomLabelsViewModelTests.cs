using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AudioBand.ViewModels;
using AudioBand.Models;
using Moq;
using AudioBand.Settings;

namespace AudioBand.Test
{
    /// <summary>
    /// Summary description for CustomLabelsViewModel
    /// </summary>
    [TestClass]
    public class CustomLabelsViewModelTests
    {
        private Mock<ICustomLabelService> _labelServiceMock;
        private Mock<IDialogService> _dialogMock;
        private Mock<IAppSettings> _appSettingsMock;
        private CustomLabel _label;
        private CustomLabelsVM _vm;

        [TestInitialize]
        public void Init()
        {
            _labelServiceMock = new Mock<ICustomLabelService>();
            _dialogMock = new Mock<IDialogService>();
            _appSettingsMock = new Mock<IAppSettings>();
            _appSettingsMock.Setup(x => x.CustomLabels).Returns(new List<CustomLabel>());
            _label = new CustomLabel();
            _vm = new CustomLabelsVM(_appSettingsMock.Object, _labelServiceMock.Object, _dialogMock.Object);
        }

        [TestMethod]
        public void AddLabel()
        {
            _vm.AddLabelCommand.Execute(null);
            
            Assert.AreEqual(1, _vm.CustomLabels.Count);
            _labelServiceMock.Verify(o => o.AddCustomTextLabel(It.IsAny<CustomLabelVM>()), Times.Once);
        }

        [TestMethod]
        public void RemoveLabel_confirm()
        {
            _dialogMock.Setup(o => o.ShowConfirmationDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _vm.AddLabelCommand.Execute(null);
            var newLabel = _vm.CustomLabels[0];
            _vm.RemoveLabelCommand.Execute(newLabel);

            Assert.AreEqual(0, _vm.CustomLabels.Count);
            _labelServiceMock.Verify(o => o.RemoveCustomTextLabel(newLabel), Times.Once);
            _dialogMock.Verify(o => o.ShowConfirmationDialog(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void RemoveLabel_deny()
        {
            _dialogMock.Setup(o => o.ShowConfirmationDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _vm.AddLabelCommand.Execute(null);
            var newLabel = _vm.CustomLabels[0];
            _vm.RemoveLabelCommand.Execute(newLabel);

            Assert.AreEqual(1, _vm.CustomLabels.Count);
            _labelServiceMock.Verify(o => o.RemoveCustomTextLabel(newLabel), Times.Never);
            _dialogMock.Verify(o => o.ShowConfirmationDialog(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void AddLabel_ThenCancel()
        {
            _vm.BeginEdit();
            _vm.AddLabelCommand.Execute(null);
            var newLabel = _vm.CustomLabels[0];
            _vm.CancelEdit();

            Assert.AreEqual(0, _vm.CustomLabels.Count);
            _labelServiceMock.Verify(o => o.AddCustomTextLabel(It.IsAny<CustomLabelVM>()), Times.Once);
            _labelServiceMock.Verify(o => o.RemoveCustomTextLabel(newLabel), Times.Once);
        }

        [TestMethod]
        public void RemoveLabel_ThenCancel()
        {
            _appSettingsMock.SetupGet(x => x.CustomLabels).Returns(new List<CustomLabel> { new CustomLabel() });
            _dialogMock.Setup(o => o.ShowConfirmationDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _vm = new CustomLabelsVM(_appSettingsMock.Object, _labelServiceMock.Object, _dialogMock.Object);
            _vm.BeginEdit();
            var label = _vm.CustomLabels[0];
            _vm.RemoveLabelCommand.Execute(label);
            _vm.CancelEdit();

            Assert.AreEqual(1, _vm.CustomLabels.Count);
            Assert.AreEqual(label, _vm.CustomLabels[0]);
            _labelServiceMock.Verify(o => o.RemoveCustomTextLabel(label), Times.Once);
        }

        [TestMethod]
        public void AddRemoveLabel_ThenCancel()
        {
            _vm.BeginEdit();
            _dialogMock.Setup(o => o.ShowConfirmationDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _vm.AddLabelCommand.Execute(null);
            var newLabel = _vm.CustomLabels[0];
            _vm.RemoveLabelCommand.Execute(newLabel);
            _vm.CancelEdit();

            Assert.AreEqual(0, _vm.CustomLabels.Count);
            _labelServiceMock.Verify(o => o.AddCustomTextLabel(It.IsAny<CustomLabelVM>()), Times.Once);
            _labelServiceMock.Verify(o => o.RemoveCustomTextLabel(newLabel), Times.Once);
        }
    }
}
