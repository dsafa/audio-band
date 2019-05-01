using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace AudioBand.Extensions
{
    /// <summary>
    /// Extensions to get the dpi.
    /// </summary>
    public static class DpiExtensions
    {
        private static readonly Version Win10Anniversary = new Version(10, 0, 14393);

        /// <summary>
        /// Gets the dpi for the control.
        /// </summary>
        /// <param name="control">The control to get the dpi for.</param>
        /// <returns>The dpi for the control.</returns>
        public static double GetDpi(this Control control)
        {
            if (Environment.OSVersion.Version.CompareTo(Win10Anniversary) < 0)
            {
                return 96.0;
            }

            return NativeMethods.GetDpiForWindow(control.Handle);
        }

        /// <summary>
        /// Gets the dpi for the window.
        /// </summary>
        /// <param name="window">The window to get the dpi for.</param>
        /// <returns>The dpi of the window.</returns>
        public static double GetDpi(this Window window)
        {
            if (Environment.OSVersion.Version.CompareTo(Win10Anniversary) < 0)
            {
                return 96.0;
            }

            var interopHelper = new WindowInteropHelper(window);
            interopHelper.EnsureHandle();
            return NativeMethods.GetDpiForWindow(interopHelper.Handle);
        }
    }
}
