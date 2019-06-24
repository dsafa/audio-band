using System;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.ViewModels;
using Moq;
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
        public void ListensForProfileChangesAndMapsProperly()
        {
            var first = new AlbumArtPopup() {Height = 10};
            var second = new AlbumArtPopup() {Height = 20};
            _appSettings.SetupSequence(m => m.AlbumArtPopup)
                .Returns(first)
                .Returns(second)
                .Returns(second);

            var vm = new AlbumArtPopupViewModel(_appSettings.Object, _messageBus.Object);

            Assert.Equal(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.False(vm.IsEditing);
            Assert.Equal(second.Height, vm.Height);
        }
    }
}
