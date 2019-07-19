using System;
using System.ComponentModel;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.UI;
using Moq;
using Xunit;

namespace AudioBand.Test
{
    public class ButtonViewModels
    {
        private Mock<IAppSettings> _appSettings;
        private Mock<IDialogService> _dialog;
        private Mock<IAudioSession> _session;
        private Mock<IMessageBus> _messageBus;

        public ButtonViewModels()
        {
            _appSettings = new Mock<IAppSettings>();
            _dialog = new Mock<IDialogService>();
            _session = new Mock<IAudioSession>();
            _messageBus = new Mock<IMessageBus>();
        }

        [Fact]
        public void NextButton_ProfilesChanged_ListensforProfileChanges()
        {
            var first = new NextButton() {Height = 1};
            var second = new NextButton() {Height = 2};
            _appSettings.SetupSequence(m => m.NextButton)
                .Returns(first)
                .Returns(second);

            var vm = new NextButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.Equal(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.False(vm.IsEditing);
            Assert.True(raised);
            Assert.Equal(second.Height, vm.Height);
        }


        [Fact]
        public async Task NextButton_NextCommandExecuted_CallsNextTrack()
        {
            _appSettings.SetupGet(m => m.NextButton).Returns(new NextButton());
            var audioSourceMock = new Mock<IInternalAudioSource>();
            audioSourceMock.Setup(m => m.NextTrackAsync()).Returns(Task.CompletedTask);
            _session.SetupGet(m => m.CurrentAudioSource).Returns(audioSourceMock.Object);
            var vm = new NextButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);

            await vm.NextTrackCommand.ExecuteAsync(null);
            audioSourceMock.Verify(m => m.NextTrackAsync());
        }

        [Fact]
        public void PlayPauseButton_ProfileChanged_ListensForProfileChanges()
        {
            var first = new PlayPauseButton() {Height = 1};
            var second = new PlayPauseButton() {Height = 2};
            _appSettings.SetupSequence(m => m.PlayPauseButton)
                .Returns(first)
                .Returns(second);

            var vm = new PlayPauseButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.Equal(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.False(vm.IsEditing);
            Assert.True(raised);
            Assert.Equal(second.Height, vm.Height);
        }

        [Fact]
        public void PlayPauseButton_PropertiesInContentAreChanged_ViewModelIsMarkedAsEditing()
        {
            _appSettings.SetupGet(m => m.PlayPauseButton).Returns(new PlayPauseButton());
            var viewModel = new PlayPauseButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);

            viewModel.PlayContent.Text = "";
            Assert.True(viewModel.PlayContent.IsEditing);
            Assert.True(viewModel.IsEditing);
            
            viewModel.EndEdit();
            Assert.False(viewModel.IsEditing);
            Assert.False(viewModel.PlayContent.IsEditing);

            viewModel.PauseContent.Text = "";
            Assert.True(viewModel.IsEditing);
            Assert.True(viewModel.PauseContent.IsEditing);
        }

        [Fact]
        public void PlayPauseButton_AudioSessionPlayStateChanged_ListensToEvent()
        {
            _appSettings.SetupGet(m => m.PlayPauseButton).Returns(new PlayPauseButton());
            _session.SetupSequence(m => m.IsPlaying).Returns(true).Returns(false);
            var viewModel = new PlayPauseButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);

            _session.Raise(m => m.PropertyChanged+= null, null, new PropertyChangedEventArgs(nameof(IAudioSession.IsPlaying)));
            Assert.True(viewModel.IsPlaying);
            Assert.False(viewModel.IsPlayButtonShown);

            _session.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.IsPlaying)));
            Assert.False(viewModel.IsPlaying);
            Assert.True(viewModel.IsPlayButtonShown);
        }

        [Fact]
        public async Task PlayPauseButton_PlayAndPauseCommandExecuted_AudioSourceIsNotified()
        {
            _appSettings.SetupGet(m => m.PlayPauseButton).Returns(new PlayPauseButton());
            var audioSourceMock = new Mock<IInternalAudioSource>();
            var isPlayingSequence = new[] {true, false};
            var index = 0;
            audioSourceMock.Setup(m => m.PlayTrackAsync())
                .Callback(() => Assert.True(isPlayingSequence[index++]))
                .Returns(Task.CompletedTask);
            audioSourceMock.Setup(m => m.PauseTrackAsync())
                .Callback(() => Assert.False(isPlayingSequence[index++]))
                .Returns(Task.CompletedTask);

            _session.SetupGet(m => m.CurrentAudioSource).Returns(audioSourceMock.Object);
            _session.SetupSequence(m => m.IsPlaying).Returns(true).Returns(false);

            var viewModel = new PlayPauseButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);
            await viewModel.PlayPauseTrackCommand.ExecuteAsync(null);
            _session.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.IsPlaying)));
            audioSourceMock.Raise(m => m.IsPlayingChanged += null, null, true);
            await viewModel.PlayPauseTrackCommand.ExecuteAsync(null);
        }

        [Fact]
        public void PreviousButton_ProfileChanged_ListensForProfileChanges()
        {
            var first = new PreviousButton() { Height = 1 };
            var second = new PreviousButton() { Height = 2 };
            _appSettings.SetupSequence(m => m.PreviousButton)
                .Returns(first)
                .Returns(second);

            var vm = new PreviousButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.Equal(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.False(vm.IsEditing);
            Assert.True(raised);
            Assert.Equal(second.Height, vm.Height);
        }

        [Fact]
        public async Task PreviousButton_PreviousCommand_CallsPreviousTrackOnAudioSource()
        {
            _appSettings.SetupGet(m => m.PreviousButton).Returns(new PreviousButton());
            var audioSourceMock = new Mock<IInternalAudioSource>();
            audioSourceMock.Setup(m => m.PreviousTrackAsync()).Returns(Task.CompletedTask);
            _session.SetupGet(m => m.CurrentAudioSource).Returns(audioSourceMock.Object);
            var vm = new PreviousButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);

            await vm.PreviousTrackCommand.ExecuteAsync(null);
            audioSourceMock.Verify(m => m.PreviousTrackAsync());
        }

        [Fact]
        public void RepeatModeButton_ProfileChanged_ListensForProfileChanges()
        {
            var first = new RepeatModeButton() { Height = 1 };
            var second = new RepeatModeButton() { Height = 2 };
            _appSettings.SetupSequence(m => m.RepeatModeButton)
                .Returns(first)
                .Returns(second);

            var vm = new RepeatModeButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.Equal(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.False(vm.IsEditing);
            Assert.True(raised);
            Assert.Equal(second.Height, vm.Height);
        }

        [Fact]
        public void RepeatModeButton_ContentPropertiesAreModified_ViewModelIsMarkedAsEditing()
        {
            _appSettings.SetupGet(m => m.RepeatModeButton).Returns(new RepeatModeButton());
            var viewModel = new RepeatModeButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);

            Assert.False(viewModel.RepeatTrackContent.IsEditing);
            Assert.False(viewModel.IsEditing);

            viewModel.RepeatTrackContent.Text = "test";

            Assert.True(viewModel.RepeatTrackContent.IsEditing);
            Assert.True(viewModel.IsEditing);
        }

        [Fact]
        public async Task RepeatModeButton_RepeatCommandExecuted_CyclesThroughRepeatModes()
        {
            var repeatSequence = new[] {RepeatMode.RepeatContext, RepeatMode.RepeatTrack, RepeatMode.Off};
            var index = 0;
            var audiosourceMock = new Mock<IInternalAudioSource>();
            audiosourceMock.Setup(m => m.SetRepeatModeAsync(It.IsAny<RepeatMode>()))
                .Callback((RepeatMode mode) => Assert.Equal(repeatSequence[index++], mode))
                .Returns(Task.CompletedTask);
            _appSettings.SetupGet(m => m.RepeatModeButton).Returns(new RepeatModeButton());
            _session.SetupGet(m => m.CurrentAudioSource).Returns(audiosourceMock.Object);
            _session.SetupSequence(m => m.RepeatMode)
                .Returns(RepeatMode.RepeatContext)
                .Returns(RepeatMode.RepeatTrack)
                .Returns(RepeatMode.Off)
                ;

            var viewModel = new RepeatModeButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);

            Assert.Equal(RepeatMode.Off, viewModel.RepeatMode);
            await viewModel.CycleRepeatModeCommand.ExecuteAsync(null);
            _session.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.RepeatMode)));
            await viewModel.CycleRepeatModeCommand.ExecuteAsync(null);
            _session.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.RepeatMode)));
            await viewModel.CycleRepeatModeCommand.ExecuteAsync(null);
        }

        [Fact]
        public void ShuffleModeButton_ProfileChanged_ListensForProfileChanges()
        {
            var first = new ShuffleModeButton() { Height = 1 };
            var second = new ShuffleModeButton() { Height = 2 };
            _appSettings.SetupSequence(m => m.ShuffleModeButton)
                .Returns(first)
                .Returns(second);

            var vm = new ShuffleModeButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.Equal(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.False(vm.IsEditing);
            Assert.True(raised);
            Assert.Equal(second.Height, vm.Height);
        }

        [Fact]
        public void ShuffleModeButton_ContentIsEdited_ViewModelIsMarkedAsEditing()
        {
            _appSettings.SetupGet(m => m.ShuffleModeButton).Returns(new ShuffleModeButton());
            var vm = new ShuffleModeButtonViewModel(_appSettings.Object, _dialog.Object, _session.Object, _messageBus.Object);

            vm.ShuffleOnContent.Text = "A";
            Assert.True(vm.ShuffleOnContent.IsEditing);
            Assert.True(vm.IsEditing);

            vm.EndEdit();
            Assert.False(vm.ShuffleOnContent.IsEditing);
            Assert.False(vm.IsEditing);

            vm.ShuffleOffContent.Text = "...";
            Assert.True(vm.ShuffleOffContent.IsEditing);
            Assert.True(vm.IsEditing);
        }

        [Fact]
        public async Task ShuffleModeButton_ShuffleCommandExecuted_TogglesShuffle()
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
                .Callback((bool shuffle) => Assert.Equal(sequence[index++], shuffle))
                .Returns(Task.CompletedTask);

            await vm.ToggleShuffleCommand.ExecuteAsync(null);
            _session.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.IsShuffleOn)));
            await vm.ToggleShuffleCommand.ExecuteAsync(null);
        }
    }
}
