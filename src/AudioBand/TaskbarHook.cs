using System;
using System.Runtime.InteropServices;
using AudioBand.Logging;
using AudioBand.Messages;
using NLog;

namespace AudioBand
{
    /// <summary>
    /// Hooks taskbar for window messages.
    /// </summary>
    public class TaskbarHook
    {
        private const int WmDpiChanged = 0x02E0;
        private const int WHCallWndProc = 4;
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<TaskbarHook>();
        private readonly IMessageBus _messageBus;

        private readonly IntPtr _hook;
        private readonly NativeMethods.CallWndProc _callback;
        private readonly IntPtr _taskBarHwnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskbarHook"/> class.
        /// </summary>
        /// <param name="messageBus">The message bus.</param>
        public TaskbarHook(IMessageBus messageBus)
        {
            _messageBus = messageBus;

            _callback = HookCallback;
            _taskBarHwnd = NativeMethods.FindWindow("Shell_TrayWnd", null);
            var taskbarHwndThreadId = NativeMethods.GetWindowThreadProcessId(_taskBarHwnd, IntPtr.Zero);
            _hook = NativeMethods.SetWindowsHookEx(WHCallWndProc, _callback, IntPtr.Zero, taskbarHwndThreadId);
            if (_hook == IntPtr.Zero)
            {
                Logger.Error("Unable to set windows hook");
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="TaskbarHook"/> class.
        /// </summary>
        ~TaskbarHook()
        {
            if (_hook != IntPtr.Zero)
            {
                NativeMethods.UnhookWindowsHookEx(_hook);
            }
        }

        private static short HiWord(IntPtr ptr)
        {
            return unchecked((short)((long)ptr >> 16));
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0 || wParam != IntPtr.Zero)
            {
                return NativeMethods.CallNextHook(IntPtr.Zero, nCode, wParam, lParam);
            }

            var info = Marshal.PtrToStructure<NativeMethods.CWPSTRUCT>(lParam);
            if (info.Message == WmDpiChanged && info.Hwnd == _taskBarHwnd)
            {
                var newDpi = HiWord(info.WParam);
                _messageBus.Publish(new DpiChangedMessage(newDpi));
            }

            return NativeMethods.CallNextHook(IntPtr.Zero, nCode, wParam, lParam);
        }
    }
}
