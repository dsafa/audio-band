using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AudioBand.Settings.Models.V2;
using AudioBand.Settings;
using AudioBand.Models;

namespace AudioBand.Test
{
    [TestClass]
    public class SettingsMapping
    {
        [TestMethod]
        public void AlbumArtPopup()
        {
            var popupSettings = new AlbumArtPopupSettings
            {
                Width = 1,
                Height = 2,
                XPosition = 4,
                IsVisible = true,
                Margin = 10,
            };

            var model = SettingsMapper.ToModel<AlbumArtPopup>(popupSettings);

            Assert.AreEqual(model.Width, popupSettings.Width);
            Assert.AreEqual(model.Height, popupSettings.Height);
            Assert.AreEqual(model.XPosition, popupSettings.XPosition);
            Assert.AreEqual(model.IsVisible, popupSettings.IsVisible);
            Assert.AreEqual(model.Margin, popupSettings.Margin);
        }
    }
}
