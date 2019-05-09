using System;
using System.Drawing;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Svg;

namespace AudioBand.Test
{
    [TestClass]
    public class ButtonViewModels
    {
        private Mock<IAppSettings> _appSettings;
        private Mock<IDialogService> _dialog;

        [TestInitialize]
        public void TestInit()
        {
            _appSettings = new Mock<IAppSettings>();
            _dialog = new Mock<IDialogService>();
        }

        [TestMethod]
        public void NextButtonListensforProfileChanges()
        {
            var first = new NextButton() {Height = 1};
            var second = new NextButton() {Height = 2};
            _appSettings.SetupSequence(m => m.NextButton)
                .Returns(first)
                .Returns(second);

            var vm = new NextButtonVM(_appSettings.Object, _dialog.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.AreEqual(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.IsTrue(raised);
            Assert.AreEqual(second.Height, vm.Height);
        }

        [TestMethod]
        public void PlayPauseButtonListensForProfileChanges()
        {
            var first = new PlayPauseButton() {Height = 1};
            var second = new PlayPauseButton() {Height = 2};
            _appSettings.SetupSequence(m => m.PlayPauseButton)
                .Returns(first)
                .Returns(second);

            var vm = new PlayPauseButtonVM(_appSettings.Object, _dialog.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.AreEqual(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.IsTrue(raised);
            Assert.AreEqual(second.Height, vm.Height);
        }

        [TestMethod]
        public void PreviousButtonListensForProfileChanges()
        {
            var first = new PreviousButton() { Height = 1 };
            var second = new PreviousButton() { Height = 2 };
            _appSettings.SetupSequence(m => m.PreviousButton)
                .Returns(first)
                .Returns(second);

            var vm = new PreviousButtonVM(_appSettings.Object, _dialog.Object);
            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            Assert.AreEqual(first.Height, vm.Height);
            _appSettings.Raise(m => m.ProfileChanged += null, EventArgs.Empty);

            Assert.IsFalse(vm.IsEditing);
            Assert.IsTrue(raised);
            Assert.AreEqual(second.Height, vm.Height);
        }
    }
}
