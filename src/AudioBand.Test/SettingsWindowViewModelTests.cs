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
        private Mock<IDialogService> _dialog;

        [TestInitialize]
        public void TestSetup()
        {
            _appSettings = new Mock<IAppSettings>();
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string>());
            _dialog = new Mock<IDialogService>();
        }

        [TestMethod]
        public void HasUnsavedChangesWhenEditMessagePublished()
        {
            var vm = new SettingsWindowVM(_appSettings.Object, _dialog.Object);

            Assert.IsFalse(vm.HasUnsavedChanges);
            this.Publish(StartEditMessage.Started);
            Assert.IsTrue(vm.HasUnsavedChanges);
        }

        [TestMethod]
        public void NoUnsavedChangesWhenEndEdit()
        {
            var vm = new SettingsWindowVM(_appSettings.Object, _dialog.Object);

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
            var vm = new SettingsWindowVM(_appSettings.Object, _dialog.Object);

            Assert.IsFalse(vm.DeleteProfileCommand.CanExecute(null));
        }

        [TestMethod]
        public void ProfilesCollectionMatchesSettings()
        {
            string profile1 = "profile 1";
            string profile2 = "profile 2";
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> {profile1, profile2});
            var vm = new SettingsWindowVM(_appSettings.Object, _dialog.Object);

            Assert.AreEqual(2, vm.Profiles.Count);
            Assert.AreEqual(profile1, vm.Profiles[0]);
            Assert.AreEqual(profile2, vm.Profiles[1]);
        }

        [TestMethod]
        public void CreateProfileAddsProfileToSettingsAndViewModel()
        {
            _appSettings.Setup(m => m.CreateProfile(It.IsAny<string>()));
            var vm = new SettingsWindowVM(_appSettings.Object, _dialog.Object);

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
            var vm = new SettingsWindowVM(_appSettings.Object, _dialog.Object);

            vm.SelectedProfileName = profile2;
            _appSettings.VerifySet(m => m.CurrentProfile = It.Is<string>(x => x == profile2));
        }

        [TestMethod]
        public void DeleteProfileConfirmationCancelDoesNotDelete()
        {
            string profile1 = "profile 1";
            string profile2 = "profile 2";

            _dialog.Setup(m => m.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object[]>()))
                .Returns(false);
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> { profile1, profile2 });
            var vm = new SettingsWindowVM(_appSettings.Object, _dialog.Object);
            vm.SelectedProfileName = profile2;
            vm.DeleteProfileCommand.Execute(profile2);

            Assert.AreEqual(2, vm.Profiles.Count);
            Assert.AreEqual(profile2, vm.SelectedProfileName);
            _dialog.Verify(m => m.ShowConfirmationDialog(It.Is<ConfirmationDialogType>(c => c == ConfirmationDialogType.DeleteProfile),
                It.Is<object[]>(data => (string)data[0] == profile2)));
            _appSettings.Verify(m => m.DeleteProfile(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void DeleteProfileSelectsNewItem()
        {
            string profile1 = "profile 1";
            string profile2 = "profile 2";

            _dialog.Setup(m => m.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object[]>()))
                .Returns(true);
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> {profile1, profile2});
            var vm = new SettingsWindowVM(_appSettings.Object, _dialog.Object);
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
            var vm = new SettingsWindowVM(_appSettings.Object, _dialog.Object);

            EndEditMessage msg = EndEditMessage.CancelEdits;
            this.Subscribe<EndEditMessage>(m => msg = m);

            vm.SelectedProfileName = profile2;

            Assert.AreEqual(EndEditMessage.AcceptEdits, msg);
        }

        [TestMethod]
        public void RenameProfileDialogAccepted()
        {
            string profile1 = "profile1";
            string profile2 = "profile2";

            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> {profile1});
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile1);
            _dialog.Setup(m => m.ShowRenameDialog(It.Is<string>(s => s == profile1), It.Is<IEnumerable<string>>(e => e.Contains(profile1))))
                .Returns(profile2);

            var vm = new SettingsWindowVM(_appSettings.Object, _dialog.Object);
            vm.RenameProfileCommand.Execute(null);

            _dialog.Verify(m => m.ShowRenameDialog(It.Is<string>(s => s == profile1), It.Is<IEnumerable<string>>(e => e.Contains(profile1))));
            _appSettings.Verify(m => m.RenameCurrentProfile(It.Is<string>(s => s == profile2)), Times.Once);
            Assert.AreEqual(profile2, vm.SelectedProfileName);
            Assert.AreEqual(profile2, vm.Profiles[0]);
            Assert.AreEqual(1, vm.Profiles.Count);
        }

        [TestMethod]
        public void RenameProfileDialogCanceled()
        {
            string profile1 = "profile1";

            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> {profile1});
            _dialog.Setup(m => m.ShowRenameDialog(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns((string)null);
            var vm = new SettingsWindowVM(_appSettings.Object, _dialog.Object);
            vm.SelectedProfileName = profile1;

            _appSettings.Verify(m => m.RenameCurrentProfile(It.IsAny<string>()), Times.Never);
            Assert.AreEqual(profile1, vm.SelectedProfileName);
            Assert.AreEqual(profile1, vm.Profiles[0]);
        }

        [TestMethod]
        public void RenameProfileSameProfileIgnored()
        {
            string profile1 = "profile1";

            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> { profile1 });
            _dialog.Setup(m => m.ShowRenameDialog(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(profile1);
            var vm = new SettingsWindowVM(_appSettings.Object, _dialog.Object);
            vm.SelectedProfileName = profile1;

            _appSettings.Verify(m => m.RenameCurrentProfile(It.IsAny<string>()), Times.Never);
            Assert.AreEqual(profile1, vm.SelectedProfileName);
            Assert.AreEqual(profile1, vm.Profiles[0]);
        }
    }
}
