using System;
using System.Collections.Generic;
using System.Linq;
using AudioBand.AudioSource;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Settings;
using AudioBand.UI;
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
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<UserProfile>()
            {
                new UserProfile()
                {
                    CustomLabels = new List<CustomLabel>()
                }
            });

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
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<UserProfile>{new UserProfile()});
            var vm = CreateVm();

            Assert.False(vm.DeleteProfileCommand.CanExecute(null));
        }

        [Fact]
        public void SettingsHasProfiles_ProfilesCollectionMatches()
        {
            var profile1 = new UserProfile { Name = "profile 1" };
            var profile2 = new UserProfile { Name = "profile 2" };
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<UserProfile> { profile1, profile2 });
            var vm = CreateVm();

            Assert.Equal(2, vm.ProfileNames.Count);
            Assert.Equal(profile1.Name, vm.ProfileNames[0]);
            Assert.Equal(profile2.Name, vm.ProfileNames[1]);
        }

        [Fact]
        public void CreateProfileCommandExecuted_AddsProfileToSettingsAndViewModel()
        {
            _appSettings.Setup(m => m.CreateProfile(It.IsAny<string>()));
            var vm = CreateVm();

            Assert.Empty(vm.ProfileNames);
            vm.AddProfileCommand.Execute(null);

            Assert.Single(vm.ProfileNames);
            _appSettings.Verify(m => m.CreateProfile(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void SelectedProfileCommandExecuted_UpdatesProfileInSettings()
        {
            var profile1 = new UserProfile { Name = "profile 1" };
            var profile2 = new UserProfile { Name = "profile 2" };
            _appSettings.Setup(m => m.SelectProfile(It.IsAny<string>()));
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<UserProfile> { profile1, profile2 });
            var vm = CreateVm();

            vm.SelectedProfileName = profile2.Name;
            _appSettings.Verify(m => m.SelectProfile(It.Is<string>(x => x == profile2.Name)));
        }

        [Fact]
        public void DeleteProfileCommandExecuted_ConfirmationDialogCancelDoesNotDelete()
        {
            var profile1 = new UserProfile { Name = "profile 1" };
            var profile2 = new UserProfile { Name = "profile 2" };

            _dialog.Setup(m => m.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object[]>()))
                .Returns(false);
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<UserProfile> { profile1, profile2 });
            var vm = CreateVm();

            vm.SelectedProfileName = profile2.Name;
            vm.DeleteProfileCommand.Execute(profile2);

            Assert.Equal(2, vm.ProfileNames.Count);
            Assert.Equal(profile2.Name, vm.SelectedProfileName);
            _dialog.Verify(m => m.ShowConfirmationDialog(It.Is<ConfirmationDialogType>(c => c == ConfirmationDialogType.DeleteProfile),
                It.Is<object[]>(data => (string)data[0] == profile2.Name)));
            _appSettings.Verify(m => m.DeleteProfile(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void DeleteProfileCommandExecuted_SelectsDifferentProfile()
        {
            var profile1 = new UserProfile { Name = "profile 1" };
            var profile2 = new UserProfile { Name = "profile 2" };

            _dialog.Setup(m => m.ShowConfirmationDialog(It.IsAny<ConfirmationDialogType>(), It.IsAny<object[]>()))
                .Returns(true);
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<UserProfile> { profile1, profile2 });
            var vm = CreateVm();

            vm.SelectedProfileName = profile2.Name;
            vm.DeleteProfileCommand.Execute(profile2);

            Assert.Equal(profile1.Name, vm.SelectedProfileName);
            _appSettings.Verify(m => m.DeleteProfile(It.Is<string>(x => x == profile2.Name)), Times.Once);
        }

        [Fact]
        public void SelectProfileCommandExecuted_PublishesEndsEditMessage()
        {
            var profile1 = new UserProfile { Name = "profile 1" };
            var profile2 = new UserProfile { Name = "profile 2" };

            var vm = CreateVm();
            var childViewModel = new TestViewmodel();
            vm.SelectedViewModel = childViewModel;
            childViewModel.BeginEdit();
            _appSettings.SetupGet(m => m.Profiles).Returns(new List<UserProfile> { profile1, profile2 });

            vm.SelectedProfileName = profile2.Name;

            Assert.False(vm.IsEditing);
        }

        [Fact]
        public void RenameProfileCommandExecuted_DialogAcceptedRenamesProfile()
        {
            var profile1 = new UserProfile { Name = "profile 1" };
            var profile2 = new UserProfile { Name = "profile 2" };

            _appSettings.SetupGet(m => m.Profiles).Returns(new List<UserProfile> { profile1 });
            _appSettings.SetupGet(m => m.CurrentProfile).Returns(profile1);
            _dialog.Setup(m => m.ShowRenameDialog(It.Is<string>(s => s == profile1.Name), It.Is<IEnumerable<string>>(e => e.Contains(profile1.Name))))
                .Returns(profile2.Name);
            var vm = CreateVm();

            vm.RenameProfileCommand.Execute(null);

            _dialog.Verify(m => m.ShowRenameDialog(It.Is<string>(s => s == profile1.Name), It.Is<IEnumerable<string>>(e => e.Contains(profile1.Name))));
            _appSettings.Verify(m => m.RenameCurrentProfile(It.Is<string>(s => s == profile2.Name)), Times.Once);
            Assert.Equal(profile2.Name, vm.SelectedProfileName);
            Assert.Equal(profile2.Name, vm.ProfileNames[0]);
            Assert.Single(vm.ProfileNames);
        }

        [Fact]
        public void RenameProfileCommandExecuted_DialogCanceledDoesNotRenameProfile()
        {
            var profile1 = new UserProfile { Name = "profile 1" };

            _appSettings.SetupGet(m => m.Profiles).Returns(new List<UserProfile> { profile1  });
            _dialog.Setup(m => m.ShowRenameDialog(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns((string)null);
            var vm = CreateVm();
            vm.SelectedProfileName = profile1.Name;

            _appSettings.Verify(m => m.RenameCurrentProfile(It.IsAny<string>()), Times.Never);
            Assert.Equal(profile1.Name, vm.SelectedProfileName);
            Assert.Equal(profile1.Name, vm.ProfileNames[0]);
        }

        [Fact]
        public void RenameProfileCommandExecuted_NameNotChangedInDialogDoesNotChangeProfileName()
        {
            var profile1 = new UserProfile { Name = "profile 1" };

            _appSettings.SetupGet(m => m.Profiles).Returns(new List<UserProfile> { profile1 });
            _dialog.Setup(m => m.ShowRenameDialog(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(profile1.Name);
            var vm = CreateVm();
            vm.SelectedProfileName = profile1.Name;

            _appSettings.Verify(m => m.RenameCurrentProfile(It.IsAny<string>()), Times.Never);
            Assert.Equal(profile1.Name, vm.SelectedProfileName);
            Assert.Equal(profile1.Name, vm.ProfileNames[0]);
        }
    }
}
