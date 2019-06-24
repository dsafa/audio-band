using System;
using System.Collections.Generic;
using System.Linq;
using AudioBand.AudioSource;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.ViewModels;
using Moq;
using Xunit;

namespace AudioBand.Test
{
    public class SettingsWindowViewModelTests
    {
        private Mock<IAppSettings> _appSettings;
        private Mock<IDialogService> _dialog;
        private Mock<IViewModelContainer> _container;
        private Mock<IMessageBus> _messageBus;

        public SettingsWindowViewModelTests()
        {
            _appSettings = new Mock<IAppSettings>();
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string>());
            _appSettings.SetupGet(m => m.CustomLabels).Returns(new List<CustomLabel>());
            _dialog = new Mock<IDialogService>();
            _container = new Mock<IViewModelContainer>();
            _messageBus = new Mock<IMessageBus>();
            _container.SetupGet(m => m.CustomLabelsViewModel).Returns(new CustomLabelsViewModel(_appSettings.Object, _dialog.Object, new Mock<IAudioSession>().Object, _messageBus.Object));
        }

        private SettingsWindowViewModel CreateVm()
        {
            return new SettingsWindowViewModel(_appSettings.Object, _dialog.Object, _container.Object, _messageBus.Object);
        }

        private class TestViewmodel : ViewModelBase
        {
        }

        [Fact]
        public void StartEditMessageIsPublished_HasUnsavedChangesPropertyIsTrue()
        {
            Action<EditStartMessage> handler = null;
            _messageBus.Setup(m => m.Subscribe(It.IsAny<Action<EditStartMessage>>()))
                .Callback<Action<EditStartMessage>>(x => handler = x);

            var vm = CreateVm();

            Assert.False(vm.HasUnsavedChanges);
            handler(default(EditStartMessage));
            Assert.True(vm.HasUnsavedChanges);
        }

        [Fact]
        public void UnsavedChangesIsTrue_SaveCommandCanExecuteIsTrue()
        {
            var vm = CreateVm();

            Assert.False(vm.SaveCommand.CanExecute(null));
            vm.HasUnsavedChanges = true;
            Assert.True(vm.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void SaveCommandExecuted_PublishesEndsEditMessageAndSavesSettings()
        {
            var vm = CreateVm();

            var childViewModel = new TestViewmodel();
            vm.HasUnsavedChanges = true; ;
            vm.SaveCommand.Execute(null);

            _appSettings.Verify(m => m.Save(), Times.Once);
            _messageBus.Verify(m => m.Publish(It.Is<EditEndMessage>(msg => msg == EditEndMessage.Accepted), It.IsAny<string>()));
            Assert.False(vm.HasUnsavedChanges);
        }

        [Fact]
        public void CloseCommandOnExecuted_ShowsDialogIfUnsavedChanges()
        {
            _dialog.Setup(m => m.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object[]>()))
                .Returns(true);

            var vm = CreateVm();
            vm.HasUnsavedChanges = true;

            vm.CloseCommand.Execute(null);
            _dialog.Verify(m => m.ShowConfirmationDialog(It.Is<ConfirmationDialogType>(d => d == ConfirmationDialogType.DiscardChanges), It.IsAny<object[]>()));
        }

        [Fact]
        public void CloseCommandExecuted_ShowsDialogIfUnsavedChanges_DiscardChangesCancelsEditsAndClosesWindow()
        {
            _dialog.Setup(m => m.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object[]>()))
                .Returns(true);

            var vm = CreateVm();
            vm.HasUnsavedChanges = true;

            vm.CloseCommand.Execute(null);
            _messageBus.Verify(m => m.Publish(It.Is<SettingsWindowMessage>(msg => msg == SettingsWindowMessage.CloseWindow), It.IsAny<string>()));
            Assert.False(vm.HasUnsavedChanges);
            _messageBus.Verify(m => m.Publish(It.Is<EditEndMessage>(msg => msg == EditEndMessage.Cancelled), It.IsAny<string>()));
        }

        [Fact]
        public void CloseCommandExecuted_ShowsDialogIfUnsavedChanges_CancelCloseDoesNotCloseWindow()
        {
            _dialog.Setup(m => m.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object[]>()))
                .Returns(false);

            var vm = CreateVm();
            vm.HasUnsavedChanges = true;

            vm.CloseCommand.Execute(null);
            _messageBus.Verify(m => m.Publish(It.Is<SettingsWindowMessage>(msg => msg == SettingsWindowMessage.CloseWindow), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void OnlyOneProfile_DeleteProfileCommandCanExecuteIsFalse()
        {
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string>{"profile 1"});
            var vm = CreateVm();

            Assert.False(vm.DeleteProfileCommand.CanExecute(null));
        }

        [Fact]
        public void SettingsHasProfiles_ProfilesCollectionMatches()
        {
            string profile1 = "profile 1";
            string profile2 = "profile 2";
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> {profile1, profile2});
            var vm = CreateVm();

            Assert.Equal(2, vm.Profiles.Count);
            Assert.Equal(profile1, vm.Profiles[0]);
            Assert.Equal(profile2, vm.Profiles[1]);
        }

        [Fact]
        public void CreateProfileCommandExecuted_AddsProfileToSettingsAndViewModel()
        {
            _appSettings.Setup(m => m.CreateProfile(It.IsAny<string>()));
            var vm = CreateVm();

            Assert.Empty(vm.Profiles);
            vm.AddProfileCommand.Execute(null);

            Assert.Single(vm.Profiles);
            _appSettings.Verify(m => m.CreateProfile(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void SelectedProfileCommandExecuted_UpdatesProfileInSettings()
        {
            string profile1 = "profile 1";
            string profile2 = "profile 2";
            _appSettings.SetupSet(m => m.CurrentProfile = It.IsAny<string>());
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> {profile1, profile2});
            var vm = CreateVm();

            vm.SelectedProfileName = profile2;
            _appSettings.VerifySet(m => m.CurrentProfile = It.Is<string>(x => x == profile2));
        }

        [Fact]
        public void DeleteProfileCommandExecuted_ConfirmationDialogCancelDoesNotDelete()
        {
            string profile1 = "profile 1";
            string profile2 = "profile 2";

            _dialog.Setup(m => m.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object[]>()))
                .Returns(false);
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> { profile1, profile2 });
            var vm = CreateVm();

            vm.SelectedProfileName = profile2;
            vm.DeleteProfileCommand.Execute(profile2);

            Assert.Equal(2, vm.Profiles.Count);
            Assert.Equal(profile2, vm.SelectedProfileName);
            _dialog.Verify(m => m.ShowConfirmationDialog(It.Is<ConfirmationDialogType>(c => c == ConfirmationDialogType.DeleteProfile),
                It.Is<object[]>(data => (string)data[0] == profile2)));
            _appSettings.Verify(m => m.DeleteProfile(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void DeleteProfileCommandExecuted_SelectsDifferentProfile()
        {
            string profile1 = "profile 1";
            string profile2 = "profile 2";

            _dialog.Setup(m => m.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object[]>()))
                .Returns(true);
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> {profile1, profile2});
            var vm = CreateVm();

            vm.SelectedProfileName = profile2;
            vm.DeleteProfileCommand.Execute(profile2);

            Assert.Equal(profile1, vm.SelectedProfileName);
            _appSettings.Verify(m => m.DeleteProfile(It.Is<string>(x => x == profile2)), Times.Once);
        }

        [Fact]
        public void SelectProfileCommandExecuted_PublishesEndsEditMessage()
        {
            string profile1 = "profile 1";
            string profile2 = "profile 2";

            var vm = CreateVm();
            var childViewModel = new TestViewmodel();
            vm.SelectedViewModel = childViewModel;
            childViewModel.BeginEdit();
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> { profile1, profile2 });

            vm.SelectedProfileName = profile2;

            Assert.False(vm.IsEditing);
        }

        [Fact]
        public void RenameProfileCommandExecuted_DialogAcceptedRenamesProfile()
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
            Assert.Equal(profile2, vm.SelectedProfileName);
            Assert.Equal(profile2, vm.Profiles[0]);
            Assert.Single(vm.Profiles);
        }

        [Fact]
        public void RenameProfileCommandExecuted_DialogCanceledDoesNotRenameProfile()
        {
            string profile1 = "profile1";

            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> {profile1});
            _dialog.Setup(m => m.ShowRenameDialog(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns((string)null);
            var vm = CreateVm();
            vm.SelectedProfileName = profile1;

            _appSettings.Verify(m => m.RenameCurrentProfile(It.IsAny<string>()), Times.Never);
            Assert.Equal(profile1, vm.SelectedProfileName);
            Assert.Equal(profile1, vm.Profiles[0]);
        }

        [Fact]
        public void RenameProfileCommandExecuted_NameNotChangedInDialogDoesNotChangeProfileName()
        {
            string profile1 = "profile1";

            _appSettings.SetupGet(m => m.Profiles).Returns(new List<string> { profile1 });
            _dialog.Setup(m => m.ShowRenameDialog(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(profile1);
            var vm = CreateVm();
            vm.SelectedProfileName = profile1;

            _appSettings.Verify(m => m.RenameCurrentProfile(It.IsAny<string>()), Times.Never);
            Assert.Equal(profile1, vm.SelectedProfileName);
            Assert.Equal(profile1, vm.Profiles[0]);
        }
    }
}
