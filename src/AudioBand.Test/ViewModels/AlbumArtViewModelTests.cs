using System;
using AudioBand.AudioSource;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.ViewModels;
using Moq;
using Xunit;

namespace AudioBand.Test
{
    public class AlbumArtViewModelTests
    {
        private Mock<IAppSettings> _appSettings;
        private Mock<IDialogService> _dialog;
        private Mock<IMessageBus> _messageBus;

        public AlbumArtViewModelTests()
        {
            _appSettings = new Mock<IAppSettings>();
            _appSettings.SetupGet(m => m.AlbumArt).Returns(new AlbumArt());
            _dialog = new Mock<IDialogService>();
            _messageBus = new Mock<IMessageBus>();
        }

        [Fact]
        public void AlbumArtViewModel_ProfileChangedEvent_ListensToProfileChanges()
        {
            var first = new AlbumArt() {Height = 10};
            var second = new AlbumArt() {Height = 20};
            _appSettings.SetupSequence(m => m.AlbumArt)
                .Returns(first)
                .Returns(second);
            var vm = new AlbumArtViewModel(_appSettings.Object, _dialog.Object, new Mock<IAudioSession>().Object, _messageBus.Object);
            bool raised = false;
            vm.PropertyChanged += (sender, e) => raised = true;

            Assert.Equal(vm.Height, first.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.False(vm.IsEditing);
            Assert.True(raised);
            Assert.Equal(second.Height, vm.Height);
        }
    }
}
