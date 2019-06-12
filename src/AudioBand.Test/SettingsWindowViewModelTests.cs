using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Windows.Controls;
using System.Windows.Forms;
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
    public class SettingsWindowViewModelTests
    {
        private Mock<IAppSettings> _appSettings;
        private Mock<IDialogService> _dialog;
        private Mock<IViewModelContainer> _container;
        private Mock<IMessageBus> _messageBus;

        [TestInitialize]
        public void TestSetup()
        {
            _appSettings = new Mock<IAppSettings>();
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string>());
            _appSettings.SetupGet(m => m.CustomLabels).Returns(new List<CustomLabel>());
            _dialog = new Mock<IDialogService>();
            _container = new Mock<IViewModelContainer>();
            _container.SetupGet(m => m.CustomLabelsViewModel).Returns(new CustomLabelsViewModel(_appSettings.Object, _dialog.Object, new Mock<IAudioSession>().Object));
            _messageBus = new Mock<IMessageBus>();
        }

        private SettingsWindowViewModel CreateVm()
        {
            return new SettingsWindowViewModel(_appSettings.Object, _dialog.Object, _container.Object, _messageBus.Object);
        }

        private class TestViewmodel : ViewModelBase
        {
        }

        [TestMethod]
        public void HasUnsavedChangesWhenSelectedViewModelIsEditing()
        {
            var vm = CreateVm();

            Assert.IsFalse(vm.HasUnsavedChanges);
            var newViewModel = new TestViewmodel();
            vm.SelectedViewModel = newViewModel;

            newViewModel.BeginEdit();
            Assert.IsTrue(vm.HasUnsavedChanges);
        }

        [TestMethod]
        public void SaveCommandCanExecuteBasedOnUnsavedChanges()
        {
            var vm = CreateVm();

            Assert.IsFalse(vm.SaveCommand.CanExecute(null));
            vm.HasUnsavedChanges = true;
            Assert.IsTrue(vm.SaveCommand.CanExecute(null));
        }

        [TestMethod]
        public void SaveCommandEndsEditsAndSavesSettings()
        {
            var vm = CreateVm();

            var childViewModel = new TestViewmodel();
            vm.HasUnsavedChanges = true;
            vm.SelectedViewModel = childViewModel;
            childViewModel.BeginEdit();
            vm.SaveCommand.Execute(null);

            _appSettings.Verify(m => m.Save(), Times.Once);
            Assert.IsFalse(childViewModel.IsEditing);
            Assert.IsFalse(vm.HasUnsavedChanges);
        }

        [TestMethod]
        public void CloseCommandShowsDialogIfUnsavedChanges()
        {
            _dialog.Setup(m => m.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object[]>()))
                .Returns(true);

            var vm = CreateVm();
            var childViewModel = new TestViewmodel();
            vm.SelectedViewModel = childViewModel;
            childViewModel.BeginEdit();

            vm.CloseCommand.Execute(null);
            _dialog.Verify(m => m.ShowConfirmationDialog(It.Is<ConfirmationDialogType>(d => d == ConfirmationDialogType.DiscardChanges), It.IsAny<object[]>()));
        }

        [TestMethod]
        public void CloseCommandShowsDialogIfUnsavedChanges_DiscardChangesCancelsEditsAndClosesWindow()
        {
            _dialog.Setup(m => m.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object[]>()))
                .Returns(true);

            var vm = CreateVm();
            var childViewModel = new TestViewmodel();
            vm.SelectedViewModel = childViewModel;
            childViewModel.BeginEdit();

            vm.CloseCommand.Execute(null);
            Assert.IsFalse(childViewModel.IsEditing);
            _messageBus.Verify(m => m.Publish(It.Is<SettingsWindowMessage>(msg => msg == SettingsWindowMessage.CloseWindow), It.IsAny<string>()));
            Assert.IsFalse(vm.HasUnsavedChanges);
        }

        [TestMethod]
        public void CloseCommandShowsDialogIfUnsavedChanges_CancelCloseDoesNotCloseWindow()
        {
            _dialog.Setup(m => m.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object[]>()))
                .Returns(false);

            var vm = CreateVm();
            var childViewModel = new TestViewmodel();
            vm.SelectedViewModel = childViewModel;
            childViewModel.BeginEdit();

            vm.CloseCommand.Execute(null);
            Assert.IsTrue(childViewModel.IsEditing);
            _messageBus.Verify(m => m.Publish(It.Is<SettingsWindowMessage>(msg => msg == SettingsWindowMessage.CloseWindow), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void DisableDeleteProfileCommandWhenOnlyOneProfile()
        {
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string>{"profile 1"});
            var vm = CreateVm();

            Assert.IsFalse(vm.DeleteProfileCommand.CanExecute(null));
        }

        [TestMethod]
        public void ProfilesCollectionMatchesSettings()
        {
            string profile1 = "profile 1";
            string profile2 = "profile 2";
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> {profile1, profile2});
            var vm = CreateVm();

            Assert.AreEqual(2, vm.Profiles.Count);
            Assert.AreEqual(profile1, vm.Profiles[0]);
            Assert.AreEqual(profile2, vm.Profiles[1]);
        }

        [TestMethod]
        public void CreateProfileAddsProfileToSettingsAndViewModel()
        {
            _appSettings.Setup(m => m.CreateProfile(It.IsAny<string>()));
            var vm = CreateVm();

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
            var vm = CreateVm();

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
            var vm = CreateVm();

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
            var vm = CreateVm();

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

            var vm = CreateVm();
            var childViewModel = new TestViewmodel();
            vm.SelectedViewModel = childViewModel;
            childViewModel.BeginEdit();
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> { profile1, profile2 });

            vm.SelectedProfileName = profile2;

            Assert.IsFalse(vm.IsEditing);
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
            var vm = CreateVm();

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
            var vm = CreateVm();
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
            var vm = CreateVm();
            vm.SelectedProfileName = profile1;

            _appSettings.Verify(m => m.RenameCurrentProfile(It.IsAny<string>()), Times.Never);
            Assert.AreEqual(profile1, vm.SelectedProfileName);
            Assert.AreEqual(profile1, vm.Profiles[0]);
        }
    }
}
