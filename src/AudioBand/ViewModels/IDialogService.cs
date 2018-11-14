using System.Threading.Tasks;

namespace AudioBand.ViewModels
{
    internal interface IDialogService
    {
        Task<bool> ShowConfirmationDialogAsync(string title, string message);
    }
}
