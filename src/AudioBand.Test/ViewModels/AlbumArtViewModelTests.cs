using AudioBand.AudioSource;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.UI;
using Moq;
using System;
using System.ComponentModel;
using System.Drawing;
using Xunit;

namespace AudioBand.Test
{
    public class AlbumArtViewModelTests
    {
        private Mock<IAppSettings> _appSettings;
        private Mock<IDialogService> _dialog;
        private Mock<IMessageBus> _messageBus;
        private Mock<IAudioSession> _audioSession;

        public AlbumArtViewModelTests()
        {
            _appSettings = new Mock<IAppSettings>();
            _dialog = new Mock<IDialogService>();
            _messageBus = new Mock<IMessageBus>();
            _audioSession = new Mock<IAudioSession>();
        }

        [Fact]
        public void AlbumArtViewModel_ProfileChangedEvent_ListensToProfileChanges()
        {
            var first = new UserProfile
            {
                AlbumArt = new AlbumArt() { Height = 10 }
            };
            var second = new UserProfile
            {
                AlbumArt = new AlbumArt() { Height = 20 }
            };
            _appSettings.SetupSequence(m => m.CurrentProfile)
                .Returns(first)
                .Returns(second);
            var vm = new AlbumArtViewModel(_appSettings.Object, _dialog.Object, _audioSession.Object, _messageBus.Object);
            bool raised = false;
            vm.PropertyChanged += (sender, e) => raised = true;

            Assert.Equal(vm.Height, first.AlbumArt.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.False(vm.IsEditing);
            Assert.True(raised);
            Assert.Equal(second.AlbumArt.Height, vm.Height);
        }

        [Fact]
        public void AlbumArtViewModel_EndEdit_WritesChangesToAppSettings()
        {
            var profile = new UserProfile { AlbumArt = new AlbumArt() };
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile);
            var vm = new AlbumArtViewModel(_appSettings.Object, _dialog.Object, _audioSession.Object, _messageBus.Object);

            int newWidth = 10;
            vm.Width = newWidth;
            vm.EndEdit();

            Assert.Equal(newWidth, profile.AlbumArt.Width);
        }

        [Fact]
        public void AlbumArtViewModel_CancelEdit_DoesNotWriteChangesToAppSettings()
        {
            int initialWidth = 0;
            int newWidth = 10;
            var profile = new UserProfile { AlbumArt = new AlbumArt { Width = initialWidth } };
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile);
            var vm = new AlbumArtViewModel(_appSettings.Object, _dialog.Object, _audioSession.Object, _messageBus.Object);

            vm.Width = newWidth;
            vm.CancelEdit();

            Assert.Equal(initialWidth, profile.AlbumArt.Width);
        }

        [Fact]
        public void AlbumArtViewModel_UsesMessageBus()
        {
            var profile = new UserProfile { AlbumArt = new AlbumArt() };
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile);
            var vm = new AlbumArtViewModel(_appSettings.Object, _dialog.Object, _audioSession.Object, _messageBus.Object);

            _messageBus.Verify(m => m.Subscribe(It.IsAny<Action<EditEndMessage>>()));
        }

        [Fact]
        public void AlbumArtViewModel_ListensToAlbumArtChanges()
        {
            var profile = new UserProfile { AlbumArt = new AlbumArt() };
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile);
            var vm = new AlbumArtViewModel(_appSettings.Object, _dialog.Object, _audioSession.Object, _messageBus.Object);

            _audioSession.SetupGet(m => m.AlbumArt).Returns(new Bitmap(1, 1));
            _audioSession.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.AlbumArt)));
            _audioSession.Verify(m => m.AlbumArt, Times.Once);
        }
    }
}
