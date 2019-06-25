using System;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.ViewModels;
using Moq;
using System.Windows.Media;
using AudioBand.AudioSource;
using AudioBand.Messages;
using Xunit;

namespace AudioBand.Test
{
    public class ProgressBarViewModelTests
    {
        [Fact]
        public void ProgressBarViewModel_ProfileChanged_ListensForProfileChanges()
        {
            var settingsMock = new Mock<IAppSettings>();

            var first = new ProgressBar() {BackgroundColor = Colors.AliceBlue};
            var second = new ProgressBar() {BackgroundColor = Colors.Aqua};
            settingsMock.SetupSequence(m => m.ProgressBar)
                .Returns(first)
                .Returns(second);

            var vm = new ProgressBarViewModel(settingsMock.Object, new Mock<IDialogService>().Object, new Mock<IAudioSession>().Object, new Mock<IMessageBus>().Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.Equal(first.BackgroundColor, vm.BackgroundColor);
            settingsMock.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.False(vm.IsEditing);
            Assert.True(raised);
            Assert.Equal(second.BackgroundColor, vm.BackgroundColor);
        }
    }
}
