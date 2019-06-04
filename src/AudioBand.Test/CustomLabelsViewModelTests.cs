using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using AudioBand.AudioSource;
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
        private CustomLabelsViewModel _viewModel;

        [TestInitialize]
        public void Init()
        {
            _dialogMock = new Mock<IDialogService>();
            _appSettingsMock = new Mock<IAppSettings>();
            _appSettingsMock.Setup(x => x.CustomLabels).Returns(new List<CustomLabel>());
            _label = new CustomLabel();
            _viewModel = new CustomLabelsViewModel(_appSettingsMock.Object, _dialogMock.Object);
        }

        [TestMethod]
        public void AddLabel()
        {
            _viewModel.AddLabelCommand.Execute(null);
            
            Assert.AreEqual(1, _viewModel.CustomLabels.Count);
        }

        [TestMethod]
        public void RemoveLabel_confirm()
        {
            _dialogMock.Setup(o => o.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object>())).Returns(true);
            _viewModel.AddLabelCommand.Execute(null);
            var newLabel = _viewModel.CustomLabels[0];
            _viewModel.RemoveLabelCommand.Execute(newLabel);

            Assert.AreEqual(0, _viewModel.CustomLabels.Count);
            _dialogMock.Verify(o => o.ShowConfirmationDialog(
                It.Is<ConfirmationDialogType>(type => type == ConfirmationDialogType.DeleteLabel),
                It.Is<object[]>(data => data.Length == 1)), 
                Times.Once);
        }

        [TestMethod]
        public void RemoveLabel_deny()
        {
            _dialogMock.Setup(o => o.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object>())).Returns(false);
            _viewModel.AddLabelCommand.Execute(null);
            var newLabel = _viewModel.CustomLabels[0];
            _viewModel.RemoveLabelCommand.Execute(newLabel);

            Assert.AreEqual(1, _viewModel.CustomLabels.Count);
            _dialogMock.Verify(o => o.ShowConfirmationDialog(
                It.Is<ConfirmationDialogType>(type => type == ConfirmationDialogType.DeleteLabel),
                It.Is<object[]>(data => data.Length == 1)),
                Times.Once);
        }

        [TestMethod]
        public void AddLabel_ThenCancel()
        {
            _viewModel.BeginEdit();
            _viewModel.AddLabelCommand.Execute(null);
            var newLabel = _viewModel.CustomLabels[0];
            _viewModel.CancelEdit();

            Assert.AreEqual(0, _viewModel.CustomLabels.Count);
        }

        [TestMethod]
        public void RemoveLabel_ThenCancel()
        {
            _appSettingsMock.SetupGet(x => x.CustomLabels).Returns(new List<CustomLabel> { new CustomLabel() });
            _dialogMock.Setup(o => o.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object>())).Returns(true);
            _viewModel = new CustomLabelsViewModel(_appSettingsMock.Object, _dialogMock.Object);
            _viewModel.BeginEdit();
            var label = _viewModel.CustomLabels[0];
            _viewModel.RemoveLabelCommand.Execute(label);
            _viewModel.CancelEdit();

            Assert.AreEqual(1, _viewModel.CustomLabels.Count);
            Assert.AreEqual(label, _viewModel.CustomLabels[0]);
        }

        [TestMethod]
        public void AddRemoveLabel_ThenCancel()
        {
            _viewModel.BeginEdit();
            _dialogMock.Setup(o => o.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>())).Returns(true);
            _viewModel.AddLabelCommand.Execute(null);
            var newLabel = _viewModel.CustomLabels[0];
            _viewModel.RemoveLabelCommand.Execute(newLabel);
            _viewModel.CancelEdit();

            Assert.AreEqual(0, _viewModel.CustomLabels.Count);
        }

        [TestMethod, Ignore("Unable to setup sequence")]
        public void ProfileChangeRemovesAllLabelsAndAddsNewOnes()
        {
            var settingsMock = new Mock<IAppSettings>();
            settingsMock.SetupSequence(m => m.CustomLabels)
                .Returns(new List<CustomLabel> { new CustomLabel { Name = "test" } })
                .Returns(new List<CustomLabel> { new CustomLabel { Name = "second" } });

            var vm = new CustomLabelsViewModel(settingsMock.Object, new Mock<IDialogService>().Object);
            Assert.AreEqual(1, vm.CustomLabels.Count);
            Assert.AreEqual("test", vm.CustomLabels[0].Name);
            _appSettingsMock.Raise(m => m.ProfileChanged += null, null, EventArgs.Empty);
            Assert.AreEqual("second", vm.CustomLabels[0].Name);
        }

        [TestMethod]
        public void ProfileChangeUpdateAudioSources()
        {
            var settingsMock = new Mock<IAppSettings>();
            settingsMock.SetupSequence(m => m.CustomLabels)
                .Returns(new List<CustomLabel> {new CustomLabel()});
            var audioSourceMock = new Mock<IInternalAudioSource>();
            audioSourceMock.SetupGet(m => m.LastTrackInfo).Returns(new TrackInfoChangedEventArgs());

            var vm = new CustomLabelsViewModel(settingsMock.Object, new Mock<IDialogService>().Object);
            vm.AudioSource = audioSourceMock.Object;
            _appSettingsMock.Raise(m => m.ProfileChanged += null, null, EventArgs.Empty);
            audioSourceMock.Raise(m => m.IsPlayingChanged += null, null, true);
            Assert.IsTrue(vm.CustomLabels[0].IsPlaying);
        }
    }
}
