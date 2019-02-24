using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NLog;

namespace AudioBand.Views.Winforms
{
    /// <summary>
    /// Base class for audioband controls. Handles dpi changes.
    /// </summary>
    public abstract class AudioBandControl : Control
    {
        private const double LogicalDpi = 96.0;
        private const int WmDpiChanged = 0x02E0;
        private const int WHCallWndProc = 4;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly IntPtr _hook;
        private readonly NativeMethods.CallWndProc _callback;
        private readonly IntPtr _taskBarHwnd;
        private Size _logicalSize;
        private Point _logicalLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioBandControl"/> class.
        /// </summary>
        public AudioBandControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            OnDpiChanged(GetDpi());

            // Hook the window proc of the taskbar to listen to WM_DPICHANGED events
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
        /// Finalizes an instance of the <see cref="AudioBandControl"/> class.
        /// </summary>
        ~AudioBandControl()
        {
            if (_hook != IntPtr.Zero)
            {
                NativeMethods.UnhookWindowsHookEx(_hook);
            }
        }

        /// <summary>
        /// Gets or sets the logical size of the control.
        /// </summary>
        /// <remarks>This is the device independent size</remarks>
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

        /// <summary>
        /// Gets or sets the logical position of the control.
        /// </summary>
        /// <remarks>This is the device independent position</remarks>
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
                return NativeMethods.CallNextHook(IntPtr.Zero, nCode, wParam, lParam);
            }

            var info = Marshal.PtrToStructure<NativeMethods.CWPSTRUCT>(lParam);
            if (info.Message == WmDpiChanged && info.Hwnd == _taskBarHwnd)
            {
                var newDpi = HiWord(info.WParam);
                OnDpiChanged(newDpi);
            }

            return NativeMethods.CallNextHook(IntPtr.Zero, nCode, wParam, lParam);
        }

        private void OnDpiChanged(double newDpi)
        {
            Dpi = newDpi;
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

        private double GetDpi()
        {
            var win10Anniversary = new Version(10, 0, 14393);
            if (Environment.OSVersion.Version.CompareTo(win10Anniversary) < 0)
            {
                return 96.0;
            }

            return NativeMethods.GetDpiForWindow(Handle);
        }
    }
}
