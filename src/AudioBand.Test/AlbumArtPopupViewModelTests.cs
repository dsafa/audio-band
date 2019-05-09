using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [TestInitialize]
        public void TestInit()
        {
            _appSettings = new Mock<IAppSettings>();
        }

        [TestMethod]
        public void ListensForProfileChangesAndMapsProperly()
        {
            var first = new AlbumArtPopup() {Height = 10};
            var second = new AlbumArtPopup() {Height = 20};
            _appSettings.SetupSequence(m => m.AlbumArtPopup)
                .Returns(first)
                .Returns(second);
            var vm = new AlbumArtPopupVM(_appSettings.Object);
            bool raise = false;
            vm.PropertyChanged += (_, __) => raise = true;

            Assert.AreEqual(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.IsTrue(raise);
            Assert.AreEqual(second.Height, vm.Height);
        }
    }
}
