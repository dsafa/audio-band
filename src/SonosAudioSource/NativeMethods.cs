using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SonosAudioSource
{
    public static class NativeMethods
    {
        public const int WM_APPCOMMAND = 0x0319;

        public enum AppCommand
        {
            Pause = 47 * 65536,
            Play = 46 * 65536,
            Next = 11 * 65536,
            Previous = 12 * 65536,
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowEx(IntPtr parent, IntPtr childAfter, string className, string windowName);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);
    }
}
