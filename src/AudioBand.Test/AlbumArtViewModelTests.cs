using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioBand.Models;
using AudioBand.Resources;
using AudioBand.Settings;
using AudioBand.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Svg;

namespace AudioBand.Test
{
    [TestClass]
    public class AlbumArtViewModelTests
    {
        private Mock<IAppSettings> _appSettings;
        private Mock<IResourceLoader> _loader;

        [TestInitialize]
        public void TestInit()
        {
            _appSettings = new Mock<IAppSettings>();
            _appSettings.SetupGet(m => m.AlbumArt).Returns(new AlbumArt());
            _loader = new Mock<IResourceLoader>();
            _loader.Setup(m => m.LoadSVGFromResource(It.IsAny<byte[]>())).Returns(new SvgDocument());
        }

        [TestMethod]
        public void LoadsDefaultImage()
        {
            var vm = new AlbumArtVM(_appSettings.Object, _loader.Object, new Track());

            _loader.Verify(m => m.LoadSVGFromResource(It.IsAny<byte[]>()), Times.Once);
            _loader.Verify(m => m.TryLoadImageFromPath(It.IsAny<string>(), It.IsAny<Image>()));
        }

        [TestMethod]
        public void ListensToProfileChanges()
        {
            var first = new AlbumArt() {Height = 10};
            var second = new AlbumArt() {Height = 20};
            _appSettings.SetupSequence(m => m.AlbumArt)
                .Returns(first)
                .Returns(second);
            var vm = new AlbumArtVM(_appSettings.Object, _loader.Object, new Track());
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
