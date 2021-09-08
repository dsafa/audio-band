using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.UI;
using Moq;
using System;
using Xunit;

namespace AudioBand.Test
{
    public class AlbumArtPopupViewModelTests
    {
        private Mock<IAppSettings> _appSettings;
        private Mock<IMessageBus> _messageBus;

        public AlbumArtPopupViewModelTests()
        {
            _appSettings = new Mock<IAppSettings>();
            _messageBus = new Mock<IMessageBus>();
        }

        [Fact]
        public void AlbumArtPopupViewModel_ProfileChangedEvent_ListensToProfileChanges()
        {
            var first = new UserProfile()
            {
                AlbumArtPopup = new AlbumArtPopup() { Height = 10 }
            };

            var second = new UserProfile
            {
                AlbumArtPopup = new AlbumArtPopup() { Height = 20 }
            };
            _appSettings.SetupSequence(m => m.CurrentProfile)
                .Returns(first)
                .Returns(second)
                .Returns(second);

            var vm = new AlbumArtPopupViewModel(_appSettings.Object, _messageBus.Object);

            Assert.Equal(first.AlbumArtPopup.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.False(vm.IsEditing);
            Assert.Equal(second.AlbumArtPopup.Height, vm.Height);
        }

        [Fact]
        public void AlbumArtPopupViewModel_EndEdit_WritesChangesToAppSettings()
        {
            var profile = new UserProfile
            {
                AlbumArtPopup = new AlbumArtPopup { Width = 50 }
            };
            const int newWidth = 100;
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile);
            var vm = new AlbumArtPopupViewModel(_appSettings.Object, _messageBus.Object);

            vm.Width = newWidth;
            vm.EndEdit();

            Assert.Equal(newWidth, profile.AlbumArtPopup.Width);
        }

        [Fact]
        public void AlbumArtPopupViewModel_CancelEdit_DoesNotWriteChangesToAppSettings()
        {
            const int initialWidth = 50;
            var profile = new UserProfile
            {
                AlbumArtPopup = new AlbumArtPopup { Width = initialWidth }
            };
            const int newWidth = 100;
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile);
            var vm = new AlbumArtPopupViewModel(_appSettings.Object, _messageBus.Object);

            vm.Width = newWidth;
            vm.CancelEdit();

            Assert.Equal(initialWidth, profile.AlbumArtPopup.Width);
        }

        [Fact]
        public void AlbumArtPopupViewModel_UsesMessageBus()
        {
            var profile = new UserProfile
            {
                AlbumArtPopup = new AlbumArtPopup()
            };
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile);
            var vm = new AlbumArtPopupViewModel(_appSettings.Object, _messageBus.Object);

            _messageBus.Verify(m => m.Subscribe(It.IsAny<Action<EditEndMessage>>()));
        }
    }
}
