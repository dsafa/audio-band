using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using AudioBand.Messages;
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
        private Mock<IAudioSession> _session;
        private Mock<IMessageBus> _messageBus;

        [TestInitialize]
        public void TestInit()
        {
            _appSettings = new Mock<IAppSettings>();
            _dialog = new Mock<IDialogService>();
            _session = new Mock<IAudioSession>();
            _messageBus = new Mock<IMessageBus>();
        }

        [TestMethod]
        public void NextButtonListensforProfileChanges()
        {
            var first = new NextButton() {Height = 1};
            var second = new NextButton() {Height = 2};
            _appSettings.SetupSequence(m => m.NextButton)
                .Returns(first)
                .Returns(second);

            var vm = new NextButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.AreEqual(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.IsTrue(raised);
            Assert.AreEqual(second.Height, vm.Height);
        }


        [TestMethod]
        public async Task NextButtonCommandCallsNextTrack()
        {
            _appSettings.SetupGet(m => m.NextButton).Returns(new NextButton());
            var audioSourceMock = new Mock<IInternalAudioSource>();
            audioSourceMock.Setup(m => m.NextTrackAsync()).Returns(Task.CompletedTask);
            _session.SetupGet(m => m.CurrentAudioSource).Returns(audioSourceMock.Object);
            var vm = new NextButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);

            await vm.NextTrackCommand.ExecuteAsync(null);
            audioSourceMock.Verify(m => m.NextTrackAsync());
        }

        [TestMethod]
        public void PlayPauseButtonListensForProfileChanges()
        {
            var first = new PlayPauseButton() {Height = 1};
            var second = new PlayPauseButton() {Height = 2};
            _appSettings.SetupSequence(m => m.PlayPauseButton)
                .Returns(first)
                .Returns(second);

            var vm = new PlayPauseButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.AreEqual(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.IsTrue(raised);
            Assert.AreEqual(second.Height, vm.Height);
        }

        [TestMethod]
        public void PlayPauseButtonMarkedAsEditingWhenContentIsEdited()
        {
            _appSettings.SetupGet(m => m.PlayPauseButton).Returns(new PlayPauseButton());
            var viewModel = new PlayPauseButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);

            viewModel.PlayContent.Text = "";
            Assert.IsTrue(viewModel.PlayContent.IsEditing);
            Assert.IsTrue(viewModel.IsEditing);
            
            viewModel.EndEdit();
            Assert.IsFalse(viewModel.IsEditing);
            Assert.IsFalse(viewModel.PlayContent.IsEditing);

            viewModel.PauseContent.Text = "";
            Assert.IsTrue(viewModel.IsEditing);
            Assert.IsTrue(viewModel.PauseContent.IsEditing);
        }

        [TestMethod]
        public void PlayPauseButtonListensToAudioSource()
        {
            _appSettings.SetupGet(m => m.PlayPauseButton).Returns(new PlayPauseButton());
            _session.SetupSequence(m => m.IsPlaying).Returns(true).Returns(false);
            var viewModel = new PlayPauseButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);

            _session.Raise(m => m.PropertyChanged+= null, null, new PropertyChangedEventArgs(nameof(IAudioSession.IsPlaying)));
            Assert.IsTrue(viewModel.IsPlaying);
            Assert.IsFalse(viewModel.IsPlayButtonShown);

            _session.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.IsPlaying)));
            Assert.IsFalse(viewModel.IsPlaying);
            Assert.IsTrue(viewModel.IsPlayButtonShown);
        }

        [TestMethod]
        public async Task PlayPauseButtonPlayAndPauseCommandWorks()
        {
            _appSettings.SetupGet(m => m.PlayPauseButton).Returns(new PlayPauseButton());
            var audioSourceMock = new Mock<IInternalAudioSource>();
            var isPlayingSequence = new[] {true, false};
            var index = 0;
            audioSourceMock.Setup(m => m.PlayTrackAsync())
                .Callback(() => Assert.AreEqual(isPlayingSequence[index++], true))
                .Returns(Task.CompletedTask);
            audioSourceMock.Setup(m => m.PauseTrackAsync())
                .Callback(() => Assert.AreEqual(isPlayingSequence[index++], false))
                .Returns(Task.CompletedTask);

            _session.SetupGet(m => m.CurrentAudioSource).Returns(audioSourceMock.Object);
            _session.SetupSequence(m => m.IsPlaying).Returns(true).Returns(false);

            var viewModel = new PlayPauseButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);
            await viewModel.PlayPauseTrackCommand.ExecuteAsync(null);
            _session.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.IsPlaying)));
            audioSourceMock.Raise(m => m.IsPlayingChanged += null, null, true);
            await viewModel.PlayPauseTrackCommand.ExecuteAsync(null);
        }

        [TestMethod]
        public void PreviousButtonListensForProfileChanges()
        {
            var first = new PreviousButton() { Height = 1 };
            var second = new PreviousButton() { Height = 2 };
            _appSettings.SetupSequence(m => m.PreviousButton)
                .Returns(first)
                .Returns(second);

            var vm = new PreviousButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.AreEqual(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.IsTrue(raised);
            Assert.AreEqual(second.Height, vm.Height);
        }

        [TestMethod]
        public async Task PreviousButtonCommandCallsPreviousTrack()
        {
            _appSettings.SetupGet(m => m.PreviousButton).Returns(new PreviousButton());
            var audioSourceMock = new Mock<IInternalAudioSource>();
            audioSourceMock.Setup(m => m.PreviousTrackAsync()).Returns(Task.CompletedTask);
            _session.SetupGet(m => m.CurrentAudioSource).Returns(audioSourceMock.Object);
            var vm = new PreviousButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);

            await vm.PreviousTrackCommand.ExecuteAsync(null);
            audioSourceMock.Verify(m => m.PreviousTrackAsync());
        }

        [TestMethod]
        public void RepeatModeButtonListensForProfileChanges()
        {
            var first = new RepeatModeButton() { Height = 1 };
            var second = new RepeatModeButton() { Height = 2 };
            _appSettings.SetupSequence(m => m.RepeatModeButton)
                .Returns(first)
                .Returns(second);

            var vm = new RepeatModeButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);
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
            var viewModel = new RepeatModeButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);

            Assert.IsFalse(viewModel.RepeatTrackContent.IsEditing);
            Assert.IsFalse(viewModel.IsEditing);

            viewModel.RepeatTrackContent.Text = "test";

            Assert.IsTrue(viewModel.RepeatTrackContent.IsEditing);
            Assert.IsTrue(viewModel.IsEditing);
        }

        [TestMethod]
        public async Task RepeatModeButtonCyclesRepeatMode()
        {
            var repeatSequence = new[] {RepeatMode.RepeatContext, RepeatMode.RepeatTrack, RepeatMode.Off};
            var index = 0;
            var audiosourceMock = new Mock<IInternalAudioSource>();
            audiosourceMock.Setup(m => m.SetRepeatModeAsync(It.IsAny<RepeatMode>()))
                .Callback((RepeatMode mode) => Assert.AreEqual(repeatSequence[index++], mode))
                .Returns(Task.CompletedTask);
            _appSettings.SetupGet(m => m.RepeatModeButton).Returns(new RepeatModeButton());
            _session.SetupGet(m => m.CurrentAudioSource).Returns(audiosourceMock.Object);
            _session.SetupSequence(m => m.RepeatMode)
                .Returns(RepeatMode.RepeatContext)
                .Returns(RepeatMode.RepeatTrack)
                .Returns(RepeatMode.Off)
                ;

            var viewModel = new RepeatModeButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);

            Assert.AreEqual(RepeatMode.Off, viewModel.RepeatMode);
            await viewModel.CycleRepeatModeCommand.ExecuteAsync(null);
            _session.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.RepeatMode)));
            await viewModel.CycleRepeatModeCommand.ExecuteAsync(null);
            _session.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.RepeatMode)));
            await viewModel.CycleRepeatModeCommand.ExecuteAsync(null);
        }

        [TestMethod]
        public void ShuffleModeButtonListensForProfileChanges()
        {
            var first = new ShuffleModeButton() { Height = 1 };
            var second = new ShuffleModeButton() { Height = 2 };
            _appSettings.SetupSequence(m => m.ShuffleModeButton)
                .Returns(first)
                .Returns(second);

            var vm = new ShuffleModeButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.AreEqual(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.IsTrue(raised);
            Assert.AreEqual(second.Height, vm.Height);
        }

        [TestMethod]
        public void ShuffleModeButtonMarkedAsEditingWhenContentIsEdited()
        {
            _appSettings.SetupGet(m => m.ShuffleModeButton).Returns(new ShuffleModeButton());
            var vm = new ShuffleModeButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);

            vm.ShuffleOnContent.Text = "A";
            Assert.IsTrue(vm.ShuffleOnContent.IsEditing);
            Assert.IsTrue(vm.IsEditing);

            vm.EndEdit();
            Assert.IsFalse(vm.ShuffleOnContent.IsEditing);
            Assert.IsFalse(vm.IsEditing);

            vm.ShuffleOffContent.Text = "...";
            Assert.IsTrue(vm.ShuffleOffContent.IsEditing);
            Assert.IsTrue(vm.IsEditing);
        }

        [TestMethod]
        public async Task ShuffleModeButtonCommandTogglesShuffle()
        {
            _appSettings.SetupGet(m => m.ShuffleModeButton).Returns(new ShuffleModeButton());
            var vm = new ShuffleModeButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);
            var audioSourceMock = new Mock<IInternalAudioSource>();
            _session.SetupGet(m => m.CurrentAudioSource).Returns(audioSourceMock.Object);
            _session.SetupSequence(m => m.IsShuffleOn)
                .Returns(true)
                .Returns(false);

            var sequence = new[] {true, false};
            var index = 0;
            audioSourceMock.Setup(m => m.SetShuffleAsync(It.IsAny<bool>()))
                .Callback((bool shuffle) => Assert.AreEqual(sequence[index++], shuffle))
                .Returns(Task.CompletedTask);

            await vm.ToggleShuffleCommand.ExecuteAsync(null);
            _session.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.IsShuffleOn)));
            await vm.ToggleShuffleCommand.ExecuteAsync(null);
        }
    }
}
