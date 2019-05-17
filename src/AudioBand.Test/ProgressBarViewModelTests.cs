using System;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Windows.Media;

namespace AudioBand.Test
{
    [TestClass]
    public class ProgressBarViewModelTests
    {
        [TestMethod]
        public void ListensForProfileChanges()
        {
            var settingsMock = new Mock<IAppSettings>();

            var first = new ProgressBar() {BackgroundColor = Colors.AliceBlue};
            var second = new ProgressBar() {BackgroundColor = Colors.Aqua};
            settingsMock.SetupSequence(m => m.ProgressBar)
                .Returns(first)
                .Returns(second);

            var vm = new ProgressBarVM(settingsMock.Object, new Mock<IDialogService>().Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.AreEqual(first.BackgroundColor, vm.BackgroundColor);
            settingsMock.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.IsTrue(raised);
            Assert.AreEqual(second.BackgroundColor, vm.BackgroundColor);
        }
    }
}
