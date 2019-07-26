using System;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.UI;
using Moq;
using Xunit;

namespace AudioBand.Test
{
    public class AudioBandViewModelTests
    {
        private Mock<IAppSettings> _appSettings;
        private Mock<IMessageBus> _messageBus;
        private Mock<IDialogService> _dialogService;

        public AudioBandViewModelTests()
        {
            _messageBus = new Mock<IMessageBus>();
            _appSettings = new Mock<IAppSettings>();
            _dialogService = new Mock<IDialogService>();
        }

        [Fact]
        public void GeneralSettingsViewModel_ProfileChangedEvent_ListensToProfileChanges()
        {
            var first = new UserProfile {GeneralSettings = new GeneralSettings() {Height = 10}};
            var second = new UserProfile { GeneralSettings = new GeneralSettings() { Height = 20 } };
            _appSettings.SetupSequence(m => m.CurrentProfile)
                .Returns(first)
                .Returns(second);
            var vm = new GeneralSettingsViewModel(_appSettings.Object, _dialogService.Object, _messageBus.Object);

            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.Equal(first.GeneralSettings.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.True(raised);
            Assert.False(vm.IsEditing);
            Assert.Equal(second.GeneralSettings.Height, vm.Height);
        }

        [Fact]
        public void GeneralSettingsViewModel_EndEdit_WritesChangesToAppSettings()
        {
            int newWidth = 10;
            var profile = new UserProfile {GeneralSettings = new GeneralSettings()};
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile);
            var vm = new GeneralSettingsViewModel(_appSettings.Object, _dialogService.Object, _messageBus.Object);

            vm.Width = newWidth;
            vm.EndEdit();

            Assert.Equal(newWidth, profile.GeneralSettings.Width);
        }

        [Fact]
        public void GeneralSettingsViewModel_CancelEdit_DoesNotWriteChangesToAppSettings()
        {
            int initialWidth = 0;
            var profile = new UserProfile { GeneralSettings = new GeneralSettings {Width = initialWidth} };
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile);
            var vm = new GeneralSettingsViewModel(_appSettings.Object, _dialogService.Object, _messageBus.Object);

            vm.Width = 10;
            vm.CancelEdit();

            Assert.Equal(initialWidth, profile.GeneralSettings.Width);
        }

        [Fact]
        public void GeneralSettingsViewModel_UsesMessageBus()
        {
            var profile = new UserProfile() {GeneralSettings = new GeneralSettings()};
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile);
            var vm = new GeneralSettingsViewModel(_appSettings.Object, _dialogService.Object, _messageBus.Object);

            _messageBus.Verify(m => m.Subscribe(It.IsAny<Action<EditEndMessage>>()));
        }
    }
}
