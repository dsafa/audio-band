using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AudioBand.Settings.Models.v2;
using AudioBand.Settings;
using AudioBand.Models;
using V2Settings = AudioBand.Settings.Models.v2.Settings;

namespace AudioBand.Test
{
    [TestClass]
    public class SettingsMapping
    {
        [TestMethod]
        public void V2_AlbumArt()
        {
            var settings = new AlbumArtSettings
            {
                Width = 1,
                Height = 2,
                XPosition = 4,
                IsVisible = true,
                YPosition = 10,
            };

            var v2 = new V2Settings {AlbumArtSettings = settings};
            var model = v2.GetModel<AlbumArt>();

            Assert.AreEqual(model.Width, settings.Width);
            Assert.AreEqual(model.Height, settings.Height);
            Assert.AreEqual(model.XPosition, settings.XPosition);
            Assert.AreEqual(model.IsVisible, settings.IsVisible);
            Assert.AreEqual(model.YPosition, settings.YPosition);
        }

        [TestMethod]
        public void V2_AlbumArtPopup()
        {
            var settings = new AlbumArtPopupSettings
            {
                Width = 10,
                Height = 2,
                XPosition = 1,
                IsVisible = true,
                Margin = 10,
            };

            var v2 = new V2Settings {AlbumArtPopupSettings = settings};
            var model = v2.GetModel<AlbumArtPopup>();

            Assert.AreEqual(model.Width, settings.Width);
            Assert.AreEqual(model.Height, settings.Height);
            Assert.AreEqual(model.XPosition, settings.XPosition);
            Assert.AreEqual(model.Margin, settings.Margin);
            Assert.AreEqual(model.IsVisible, settings.IsVisible);
        }
    }
}
