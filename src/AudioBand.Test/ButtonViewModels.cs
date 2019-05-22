using System;
using System.Drawing;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AudioBand.Test
{
    [TestClass]
    public class ButtonViewModels
    {
        private Mock<IAppSettings> _appSettings;
        private Mock<IDialogService> _dialog;

        [TestInitialize]
        public void TestInit()
        {
            _appSettings = new Mock<IAppSettings>();
            _dialog = new Mock<IDialogService>();
        }

        [TestMethod]
        public void NextButtonListensforProfileChanges()
        {
            var first = new NextButton() {Height = 1};
            var second = new NextButton() {Height = 2};
            _appSettings.SetupSequence(m => m.NextButton)
                .Returns(first)
                .Returns(second);

            var vm = new NextButtonViewModel(_appSettings.Object, _dialog.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.AreEqual(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.IsTrue(raised);
            Assert.AreEqual(second.Height, vm.Height);
        }

        [TestMethod]
        public void PlayPauseButtonListensForProfileChanges()
        {
            var first = new PlayPauseButton() {Height = 1};
            var second = new PlayPauseButton() {Height = 2};
            _appSettings.SetupSequence(m => m.PlayPauseButton)
                .Returns(first)
                .Returns(second);

            var vm = new PlayPauseButtonVM(_appSettings.Object, _dialog.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.AreEqual(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.IsTrue(raised);
            Assert.AreEqual(second.Height, vm.Height);
        }

        [TestMethod]
        public void PreviousButtonListensForProfileChanges()
        {
            var first = new PreviousButton() { Height = 1 };
            var second = new PreviousButton() { Height = 2 };
            _appSettings.SetupSequence(m => m.PreviousButton)
                .Returns(first)
                .Returns(second);

            var vm = new PreviousButtonViewModel(_appSettings.Object, _dialog.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.AreEqual(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.IsTrue(raised);
            Assert.AreEqual(second.Height, vm.Height);
        }

        [TestMethod]
        public void RepeatModeButtonListensForProfileChanges()
        {
            var first = new RepeatModeButton() { Height = 1 };
            var second = new RepeatModeButton() { Height = 2 };
            _appSettings.SetupSequence(m => m.RepeatModeButton)
                .Returns(first)
                .Returns(second);

            var vm = new RepeatModeButtonViewModel(_appSettings.Object, _dialog.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.AreEqual(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.IsTrue(raised);
            Assert.AreEqual(second.Height, vm.Height);
        }

        [TestMethod]
        public void RepeatModeButtonMarkedAsEditingWhenContentIsEditing()
        {
            _appSettings.SetupGet(m => m.RepeatModeButton).Returns(new RepeatModeButton());
            var viewModel = new RepeatModeButtonViewModel(_appSettings.Object, _dialog.Object);

            Assert.IsFalse(viewModel.RepeatTrackContent.IsEditing);
            Assert.IsFalse(viewModel.IsEditing);

            viewModel.RepeatTrackContent.Text = "test";

            Assert.IsTrue(viewModel.RepeatTrackContent.IsEditing);
            Assert.IsTrue(viewModel.IsEditing);
        }

        [TestMethod]
        public void RepeatModeButtonSubscribesToAudioSource()
        {
            _appSettings.SetupGet(m => m.RepeatModeButton).Returns(new RepeatModeButton());
            var viewModel = new RepeatModeButtonViewModel(_appSettings.Object, _dialog.Object);
            var audiosourceMock = new Mock<IAudioSource>();
            viewModel.AudioSource = audiosourceMock.Object;

            audiosourceMock.Raise(m => m.RepeatModeChanged += null, null, RepeatMode.RepeatTrack);
            Assert.AreEqual(RepeatMode.RepeatTrack, viewModel.RepeatMode);
        }

        [TestMethod]
        public async Task RepeatModeButtonCyclesRepeatMode()
        {
            _appSettings.SetupGet(m => m.RepeatModeButton).Returns(new RepeatModeButton());
            var viewModel = new RepeatModeButtonViewModel(_appSettings.Object, _dialog.Object);
            var audiosourceMock = new Mock<IAudioSource>();
            var repeatSequence = new[] {RepeatMode.RepeatContext, RepeatMode.RepeatTrack, RepeatMode.Off};
            var index = 0;
            audiosourceMock.Setup(m => m.SetRepeatModeAsync(It.IsAny<RepeatMode>()))
                .Callback((RepeatMode mode) => Assert.AreEqual(repeatSequence[index++], mode))
                .Returns(Task.CompletedTask);

            viewModel.AudioSource = audiosourceMock.Object;

            Assert.AreEqual(RepeatMode.Off, viewModel.RepeatMode);
            await viewModel.CycleRepeatModeCommand.ExecuteAsync(null);
            audiosourceMock.Raise(m => m.RepeatModeChanged += null, null, RepeatMode.RepeatContext);
            await viewModel.CycleRepeatModeCommand.ExecuteAsync(null);
            audiosourceMock.Raise(m => m.RepeatModeChanged += null, null, RepeatMode.RepeatTrack);
            await viewModel.CycleRepeatModeCommand.ExecuteAsync(null);
        }
    }
}
