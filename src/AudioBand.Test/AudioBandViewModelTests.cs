using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioBand.Settings;
using AudioBand.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AudioBand.Test
{
    [TestClass]
    public class AudioBandViewModelTests
    {
        private Mock<IAppSettings> _appSettings;

        [TestInitialize]
        public void TestInit()
        {
            _appSettings = new Mock<IAppSettings>();
        }

        [TestMethod]
        public void AudioBandViewModelListensToProfileChanges()
        {
            var first = new Models.AudioBand(){Height = 10};
            var second = new Models.AudioBand(){Height = 20};
            _appSettings.SetupSequence(m => m.AudioBand)
                .Returns(first)
                .Returns(second);
            var vm = new AudioBandViewModel(_appSettings.Object, new Mock<IDialogService>().Object);

            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.AreEqual(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsTrue(raised);
            Assert.IsFalse(vm.IsEditing);
            Assert.AreEqual(second.Height, vm.Height);
        }
    }
}
