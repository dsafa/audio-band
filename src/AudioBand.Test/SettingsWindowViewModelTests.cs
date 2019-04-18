using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using AudioBand.Messages;
using AudioBand.Settings;
using AudioBand.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PubSub.Extension;

namespace AudioBand.Test
{
    [TestClass]
    public class SettingsWindowViewModelTests
    {
        private Mock<IAppSettings> _appSettings;

        [TestInitialize]
        public void TestSetup()
        {
            _appSettings = new Mock<IAppSettings>();
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string>());
        }

        [TestMethod]
        public void HasUnsavedChangesWhenEditMessagePublished()
        {
            var vm = new SettingsWindowVM(_appSettings.Object);

            Assert.IsFalse(vm.HasUnsavedChanges);
            this.Publish(StartEditMessage.Started);
            Assert.IsTrue(vm.HasUnsavedChanges);
        }

        [TestMethod]
        public void NoUnsavedChangesWhenEndEdit()
        {
            var vm = new SettingsWindowVM(_appSettings.Object);

            vm.HasUnsavedChanges = true;
            this.Publish(EndEditMessage.CancelEdits);
            Assert.IsFalse(vm.HasUnsavedChanges);

            vm.HasUnsavedChanges = true;
            this.Publish(EndEditMessage.AcceptEdits);
            Assert.IsFalse(vm.HasUnsavedChanges);
        }

        [TestMethod]
        public void DisableDeleteProfileCommandWhenOnlyOneProfile()
        {
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string>{"profile 1"});
            var vm = new SettingsWindowVM(_appSettings.Object);

            Assert.IsFalse(vm.DeleteProfileCommand.CanExecute(null));
        }

        [TestMethod]
        public void ProfilesCollectionMatchesSettings()
        {
            string profile1 = "profile 1";
            string profile2 = "profile 2";
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> {profile1, profile2});
            var vm = new SettingsWindowVM(_appSettings.Object);

            Assert.AreEqual(2, vm.Profiles.Count);
            Assert.AreEqual(profile1, vm.Profiles[0]);
            Assert.AreEqual(profile2, vm.Profiles[1]);
        }

        [TestMethod]
        public void CreateProfileAddsProfileToSettingsAndViewModel()
        {
            _appSettings.Setup(m => m.CreateProfile(It.IsAny<string>()));
            var vm = new SettingsWindowVM(_appSettings.Object);

            Assert.AreEqual(0, vm.Profiles.Count);
            vm.AddProfileCommand.Execute(null);

            Assert.AreEqual(1, vm.Profiles.Count);
            _appSettings.Verify(m => m.CreateProfile(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SelectedProfileCommandUpdatesSettings()
        {
            string profile1 = "profile 1";
            string profile2 = "profile 2";
            _appSettings.SetupSet(m => m.CurrentProfile = It.IsAny<string>());
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> {profile1, profile2});
            var vm = new SettingsWindowVM(_appSettings.Object);

            vm.SelectedProfileName = profile2;
            _appSettings.VerifySet(m => m.CurrentProfile = It.Is<string>(x => x == profile2));
        }

        [TestMethod]
        public void DeleteProfileSelectsNewItem()
        {
            string profile1 = "profile 1";
            string profile2 = "profile 2";

            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> {profile1, profile2});
            var vm = new SettingsWindowVM(_appSettings.Object);
            vm.SelectedProfileName = profile2;
            vm.DeleteProfileCommand.Execute(profile2);

            Assert.AreEqual(profile1, vm.SelectedProfileName);
            _appSettings.Verify(m => m.DeleteProfile(It.Is<string>(x => x == profile2)), Times.Once);
        }

        [TestMethod]
        public void SelectProfileEndsEdits()
        {
            string profile1 = "profile 1";
            string profile2 = "profile 2";

            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> { profile1, profile2 });
            var vm = new SettingsWindowVM(_appSettings.Object);

            EndEditMessage msg = EndEditMessage.CancelEdits;
            this.Subscribe<EndEditMessage>(m => msg = m);

            vm.SelectedProfileName = profile2;

            Assert.AreEqual(EndEditMessage.AcceptEdits, msg);
        }
    }
}
