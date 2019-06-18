using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AudioBand.Test
{
    [TestClass]
    public class AlbumArtViewModelTests
    {
        private Mock<IAppSettings> _appSettings;
        private Mock<IDialogService> _dialog;
        private Mock<IMessageBus> _messageBus;

        [TestInitialize]
        public void TestInit()
        {
            _appSettings = new Mock<IAppSettings>();
            _appSettings.SetupGet(m => m.AlbumArt).Returns(new AlbumArt());
            _dialog = new Mock<IDialogService>();
            _messageBus = new Mock<IMessageBus>();
        }

        [TestMethod]
        public void ListensToProfileChanges()
        {
            var first = new AlbumArt() {Height = 10};
            var second = new AlbumArt() {Height = 20};
            _appSettings.SetupSequence(m => m.AlbumArt)
                .Returns(first)
                .Returns(second);
            var vm = new AlbumArtViewModel(_appSettings.Object, _dialog.Object, new Mock<IAudioSession>().Object, _messageBus.Object);
            bool raised = false;
            vm.PropertyChanged += (sender, e) => raised = true;

            Assert.AreEqual(vm.Height, first.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.IsTrue(raised);
            Assert.AreEqual(second.Height, vm.Height);
        }
    }
}
