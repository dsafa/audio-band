using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;

namespace AudioBand.UI
{
    /// <summary>
    /// Attached behaviour that changes the application user model id so that the window is separate from explorer in the taskbar.
    /// </summary>
    public class AppUserModelID : Behavior<Window>
    {
        private static readonly string ApplicationID = "Dsafa.AudioBand";

        /// <inheritdoc />
        protected override void OnAttached()
        {
            var interopHelper = new WindowInteropHelper(AssociatedObject);
            interopHelper.EnsureHandle();
            NativeMethods.SetWindowAppId(interopHelper.Handle, ApplicationID);
        }
    }
}
