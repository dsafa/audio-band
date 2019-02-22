using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioBand.Views.Winforms
{
    public abstract class AudioBandControl : Control
    {
        private const double LogicalDpi = 96.0;
        private const int WmDpiChanged = 0x02E0;
        private const int WHCallWndProc = 4;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly IntPtr _hook;
        private readonly CallWndProc _callback;
        private readonly IntPtr _taskBarHwnd;
        private Size _logicalSize;
        private Point _logicalLocation;

        private delegate IntPtr CallWndProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32", EntryPoint = "SetWindowsHookExA")]
        private static extern IntPtr SetWindowsHookEx(int idHook, CallWndProc lpfn, IntPtr hmod, IntPtr dwThreadId);

        [DllImport("user32", EntryPoint = "UnhookWindowsHookEx")]
        private static extern IntPtr UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("user32", EntryPoint = "CallNextHookEx")]
        private static extern IntPtr CallNextHook(IntPtr hHook, int ncode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32")]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);

        [DllImport("user32")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern int GetDpiForWindow(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        private struct CWPSTRUCT
        {
            public IntPtr LParam;
            public IntPtr WParam;
            public int Message;
            public IntPtr Hwnd;
        }

        public AudioBandControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            Dpi = GetDpiForWindow(Handle);
            OnDpiChanged();

            _callback = HookCallback;
            _taskBarHwnd = FindWindow("Shell_TrayWnd", null);
            var taskbarHwndThreadId = GetWindowThreadProcessId(_taskBarHwnd, IntPtr.Zero);
            _hook = SetWindowsHookEx(WHCallWndProc, _callback, IntPtr.Zero, taskbarHwndThreadId);
            if (_hook == IntPtr.Zero)
            {
                Logger.Error("Unable to set windows hook");
            }
        }

        ~AudioBandControl()
        {
            if (_hook != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hook);
            }
        }

        [Browsable(true)]
        [Bindable(BindableSupport.Yes)]
        public Size LogicalSize
        {
            get => _logicalSize;
            set
            {
                _logicalSize = value;
                UpdateSize();
            }
        }

        [Browsable(true)]
        [Bindable(BindableSupport.Yes)]
        public Point LogicalLocation
        {
            get => _logicalLocation;
            set
            {
                _logicalLocation = value;
                UpdateLocation();
            }
        }

        private double Dpi { get; set; } = LogicalDpi;

        private double ScalingFactor => Dpi / LogicalDpi;

        private static short HiWord(IntPtr ptr)
        {
            return unchecked((short)((long)ptr >> 16));
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0 || wParam != IntPtr.Zero)
            {
                return CallNextHook(IntPtr.Zero, nCode, wParam, lParam);
            }

            var info = Marshal.PtrToStructure<CWPSTRUCT>(lParam);
            if (info.Message == WmDpiChanged && info.Hwnd == _taskBarHwnd)
            {
                var newDpi = HiWord(info.WParam);
                Dpi = newDpi;
                OnDpiChanged();
            }

            return CallNextHook(IntPtr.Zero, nCode, wParam, lParam);
        }

        private void OnDpiChanged()
        {
            UpdateLocation();
            UpdateSize();
        }

        private void UpdateSize()
        {
            Size = new Size((int)(_logicalSize.Width * ScalingFactor), (int)(_logicalSize.Height * ScalingFactor));
        }

        private void UpdateLocation()
        {
            Location = new Point((int)(_logicalLocation.X * ScalingFactor), (int)(_logicalLocation.Y * ScalingFactor));
        }
    }
}
