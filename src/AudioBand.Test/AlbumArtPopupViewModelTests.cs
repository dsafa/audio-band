using System;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AudioBand.Test
{
    [TestClass]
    public class AlbumArtPopupViewModelTests
    {
        private Mock<IAppSettings> _appSettings;
        private Mock<IMessageBus> _messageBus;

        [TestInitialize]
        public void TestInit()
        {
            _appSettings = new Mock<IAppSettings>();
            _messageBus = new Mock<IMessageBus>();
        }

        [TestMethod]
        public void ListensForProfileChangesAndMapsProperly()
        {
            var first = new AlbumArtPopup() {Height = 10};
            var second = new AlbumArtPopup() {Height = 20};
            _appSettings.SetupSequence(m => m.AlbumArtPopup)
                .Returns(first)
                .Returns(second)
                .Returns(second);

            var vm = new AlbumArtPopupViewModel(_appSettings.Object, _messageBus.Object);

            Assert.AreEqual(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.AreEqual(second.Height, vm.Height);
        }
    }
}
