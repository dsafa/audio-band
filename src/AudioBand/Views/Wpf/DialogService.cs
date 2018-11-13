using AudioBand.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace AudioBand.Views.Wpf
{
    internal class DialogService : IDialogService
    {
        private readonly MetroWindow _window;

        public DialogService(MetroWindow window)
        {
            _window = window;
        }

        public async Task<bool> ShowConfirmationDialogAsync(string title, string message)
        {
            var dialogSettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
                AnimateHide = false,
                AnimateShow = false,
            };

            var res = await _window.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative, dialogSettings);
            return res == MessageDialogResult.Affirmative;
        }
    }
}
