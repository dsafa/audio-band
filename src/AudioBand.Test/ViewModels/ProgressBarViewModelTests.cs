using System;
using System.ComponentModel;
using AudioBand.Models;
using AudioBand.Settings;
using Moq;
using System.Windows.Media;
using AudioBand.AudioSource;
using AudioBand.Messages;
using AudioBand.UI;
using Xunit;

namespace AudioBand.Test
{
    public class ProgressBarViewModelTests
    {
        private Mock<IAppSettings> _appSettings;
        private Mock<IDialogService> _dialogService;
        private Mock<IAudioSession> _audioSession;
        private Mock<IMessageBus> _messageBus;

        public ProgressBarViewModelTests()
        {
            _dialogService = new Mock<IDialogService>();
            _audioSession = new Mock<IAudioSession>();
            _messageBus = new Mock<IMessageBus>();
            _appSettings = new Mock<IAppSettings>();
        }

        [Fact]
        public void ProgressBarViewModel_ProfileChanged_ListensForProfileChanges()
        {
            var first = new UserProfile() {ProgressBar = new ProgressBar() {BackgroundColor = Colors.AliceBlue}};
            var second = new UserProfile() { ProgressBar = new ProgressBar() { BackgroundColor = Colors.Aqua }};
            _appSettings.SetupSequence(m => m.CurrentProfile)
                .Returns(first)
                .Returns(second);

            var vm = new ProgressBarViewModel(_appSettings.Object, _dialogService.Object, _audioSession.Object, _messageBus.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.Equal(first.ProgressBar.BackgroundColor, vm.BackgroundColor);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.False(vm.IsEditing);
            Assert.True(raised);
            Assert.Equal(second.ProgressBar.BackgroundColor, vm.BackgroundColor);
        }

        [Fact]
        public void ProgressBarViewModel_EndEdit_WritesChangesToAppSettings()
        {
            var profile = new UserProfile() {ProgressBar = new ProgressBar()};
            var newWidth = 10;
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile);
            var vm = new ProgressBarViewModel(_appSettings.Object, _dialogService.Object, _audioSession.Object, _messageBus.Object);

            vm.Width = newWidth;
            vm.EndEdit();

            Assert.Equal(newWidth, profile.ProgressBar.Width);
        }

        [Fact]
        public void ProgressBarViewModel_CancelEdit_DoesNotWriteChangesToAppSettings()
        {
            var initialWidth = 0;
            var profile = new UserProfile() {ProgressBar = new ProgressBar {Width = initialWidth}};
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile);
            var vm = new ProgressBarViewModel(_appSettings.Object, _dialogService.Object, _audioSession.Object, _messageBus.Object);

            vm.Width = 20;
            vm.CancelEdit();

            Assert.Equal(initialWidth, profile.ProgressBar.Width);
        }

        [Fact]
        public void ProgressBarViewModel_UsesMessageBus()
        {
            var profile = new UserProfile() {ProgressBar = new ProgressBar()};
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile);
            var vm = new ProgressBarViewModel(_appSettings.Object, _dialogService.Object, _audioSession.Object, _messageBus.Object);

            _messageBus.Verify(m => m.Subscribe(It.IsAny<Action<EditEndMessage>>()));
        }

        [Fact]
        public void ProgressBarViewModel_ListensToAudioSession_UpdatesSongLengthAndProgress()
        {
            var profile = new UserProfile() { ProgressBar = new ProgressBar() };
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile);
            var vm = new ProgressBarViewModel(_appSettings.Object, _dialogService.Object, _audioSession.Object, _messageBus.Object);

            var length = TimeSpan.FromSeconds(50);
            var progress = TimeSpan.FromSeconds(10);
            _audioSession.SetupGet(m => m.SongLength).Returns(length);
            _audioSession.SetupGet(m => m.SongProgress).Returns(progress);

            _audioSession.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.SongLength)));
            _audioSession.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.SongProgress)));
            
            _audioSession.VerifyGet(m => m.SongLength);
            _audioSession.VerifyGet(m => m.SongProgress);
            Assert.Equal(length, vm.TrackLength);
            Assert.Equal(progress, vm.TrackProgress);
        }
    }
}
