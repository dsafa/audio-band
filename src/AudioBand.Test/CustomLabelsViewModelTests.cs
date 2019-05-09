﻿using System;
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
        private Mock<IDialogService> _dialogMock;
        private Mock<IAppSettings> _appSettingsMock;
        private CustomLabel _label;
        private CustomLabelsVM _vm;

        [TestInitialize]
        public void Init()
        {
            _dialogMock = new Mock<IDialogService>();
            _appSettingsMock = new Mock<IAppSettings>();
            _appSettingsMock.Setup(x => x.CustomLabels).Returns(new List<CustomLabel>());
            _label = new CustomLabel();
            _vm = new CustomLabelsVM(_appSettingsMock.Object, _dialogMock.Object);
        }

        [TestMethod]
        public void AddLabel()
        {
            _vm.AddLabelCommand.Execute(null);
            
            Assert.AreEqual(1, _vm.CustomLabels.Count);
        }

        [TestMethod]
        public void RemoveLabel_confirm()
        {
            _dialogMock.Setup(o => o.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object>())).Returns(true);
            _vm.AddLabelCommand.Execute(null);
            var newLabel = _vm.CustomLabels[0];
            _vm.RemoveLabelCommand.Execute(newLabel);

            Assert.AreEqual(0, _vm.CustomLabels.Count);
            _dialogMock.Verify(o => o.ShowConfirmationDialog(
                It.Is<ConfirmationDialogType>(type => type == ConfirmationDialogType.DeleteLabel),
                It.Is<object[]>(data => data.Length == 1)), 
                Times.Once);
        }

        [TestMethod]
        public void RemoveLabel_deny()
        {
            _dialogMock.Setup(o => o.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object>())).Returns(false);
            _vm.AddLabelCommand.Execute(null);
            var newLabel = _vm.CustomLabels[0];
            _vm.RemoveLabelCommand.Execute(newLabel);

            Assert.AreEqual(1, _vm.CustomLabels.Count);
            _dialogMock.Verify(o => o.ShowConfirmationDialog(
                It.Is<ConfirmationDialogType>(type => type == ConfirmationDialogType.DeleteLabel),
                It.Is<object[]>(data => data.Length == 1)),
                Times.Once);
        }

        [TestMethod]
        public void AddLabel_ThenCancel()
        {
            _vm.BeginEdit();
            _vm.AddLabelCommand.Execute(null);
            var newLabel = _vm.CustomLabels[0];
            _vm.CancelEdit();

            Assert.AreEqual(0, _vm.CustomLabels.Count);
        }

        [TestMethod]
        public void RemoveLabel_ThenCancel()
        {
            _appSettingsMock.SetupGet(x => x.CustomLabels).Returns(new List<CustomLabel> { new CustomLabel() });
            _dialogMock.Setup(o => o.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object>())).Returns(true);
            _vm = new CustomLabelsVM(_appSettingsMock.Object, _dialogMock.Object);
            _vm.BeginEdit();
            var label = _vm.CustomLabels[0];
            _vm.RemoveLabelCommand.Execute(label);
            _vm.CancelEdit();

            Assert.AreEqual(1, _vm.CustomLabels.Count);
            Assert.AreEqual(label, _vm.CustomLabels[0]);
        }

        [TestMethod]
        public void AddRemoveLabel_ThenCancel()
        {
            _vm.BeginEdit();
            _dialogMock.Setup(o => o.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>())).Returns(true);
            _vm.AddLabelCommand.Execute(null);
            var newLabel = _vm.CustomLabels[0];
            _vm.RemoveLabelCommand.Execute(newLabel);
            _vm.CancelEdit();

            Assert.AreEqual(0, _vm.CustomLabels.Count);
        }

        [TestMethod, Ignore("Issue with verifying invocation")]
        public void ProfileChangeRemovesAllLabelsAndAddsNewOnes()
        {

            var settingsMock = new Mock<IAppSettings>();
            settingsMock.SetupGet(m => m.CustomLabels).Returns(new List<CustomLabel> {new CustomLabel()});

            var vm = new CustomLabelsVM(settingsMock.Object, new Mock<IDialogService>().Object);
            Assert.AreEqual(1, vm.CustomLabels.Count);
            _appSettingsMock.Raise(m => m.ProfileChanged += null, EventArgs.Empty);
        }
    }
}
