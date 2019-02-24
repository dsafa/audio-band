using System;
using System.Runtime.InteropServices;

namespace AudioBand
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements must be documented
    public static class NativeMethods
    {
        public delegate IntPtr CallWndProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32", EntryPoint = "SetWindowsHookExA")]
        public static extern IntPtr SetWindowsHookEx(int idHook, CallWndProc lpfn, IntPtr hmod, IntPtr dwThreadId);

        [DllImport("user32", EntryPoint = "UnhookWindowsHookEx")]
        public static extern IntPtr UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("user32", EntryPoint = "CallNextHookEx")]
        public static extern IntPtr CallNextHook(IntPtr hHook, int ncode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);

        [DllImport("user32")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // requires win 10 anniversary
        [DllImport("user32")]
        public static extern uint GetDpiForWindow(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct CWPSTRUCT
        {
            public IntPtr LParam;
            public IntPtr WParam;
            public int Message;
            public IntPtr Hwnd;
        }
    }
#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
