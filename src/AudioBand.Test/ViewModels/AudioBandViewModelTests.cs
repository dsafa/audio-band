using System;
using AudioBand.Messages;
using AudioBand.Settings;
using AudioBand.ViewModels;
using Moq;
using Xunit;

namespace AudioBand.Test
{
    public class AudioBandViewModelTests
    {
        private Mock<IAppSettings> _appSettings;
        private Mock<IMessageBus> _messageBus;

        public AudioBandViewModelTests()
        {
            _messageBus = new Mock<IMessageBus>();
            _appSettings = new Mock<IAppSettings>();
        }

        [Fact]
        public void AudioBandViewModelListensToProfileChanges()
        {
            var first = new Models.AudioBand(){Height = 10};
            var second = new Models.AudioBand(){Height = 20};
            _appSettings.SetupSequence(m => m.AudioBand)
                .Returns(first)
                .Returns(second);
            var vm = new AudioBandViewModel(_appSettings.Object, new Mock<IDialogService>().Object, _messageBus.Object);

            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.Equal(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.True(raised);
            Assert.False(vm.IsEditing);
            Assert.Equal(second.Height, vm.Height);
        }
    }
}
