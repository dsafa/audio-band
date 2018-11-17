using System.Threading.Tasks;
using System.Windows.Media;

namespace AudioBand.ViewModels
{
    internal interface IDialogService
    {
        Task<bool> ShowConfirmationDialogAsync(string title, string message);
    }
}
