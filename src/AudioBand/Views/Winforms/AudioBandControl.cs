using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AudioBand.Logging;
using NLog;

namespace AudioBand.Views.Winforms
{
    /// <summary>
    /// Base class for audioband controls. Handles dpi changes.
    /// </summary>
    public class AudioBandControl : UserControl
    {
        private const double LogicalDpi = 96.0;
        private const int WmDpiChanged = 0x02E0;
        private const int WHCallWndProc = 4;
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<AudioBandControl>();
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
            UpdateDpi(GetDpi());

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
        /// <remarks>This is the device independent size.</remarks>
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
        /// <remarks>This is the device independent position.</remarks>
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

        /// <summary>
        /// Gets the dpi.
        /// </summary>
        public double Dpi { get; private set; } = LogicalDpi;

        /// <summary>
        /// Gets the scaling factor.
        /// </summary>
        public double ScalingFactor => Dpi / LogicalDpi;

        /// <summary>
        /// Gets the size scaled by the <see cref="ScalingFactor"/>.
        /// </summary>
        /// <param name="size">The size to scale.</param>
        /// <returns>The new scaled size.</returns>
        protected Size GetScaledSize(Size size)
        {
            return new Size((int)Math.Round(size.Width * ScalingFactor), (int)Math.Round(size.Height * ScalingFactor));
        }

        /// <summary>
        /// Gets the point scaled by the <see cref="ScalingFactor"/>.
        /// </summary>
        /// <param name="point">The point to scale.</param>
        /// <returns>The new scaled point.</returns>
        protected Point GetScaledPoint(Point point)
        {
            return new Point((int)Math.Round(point.X * ScalingFactor), (int)Math.Round(point.Y * ScalingFactor));
        }

        /// <summary>
        /// Cross thread safe refresh.
        /// </summary>
        protected void InvokeRefresh()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(Refresh));
            }
            else
            {
                Refresh();
            }
        }

        /// <summary>
        /// Occurs when dpi changed.
        /// </summary>
        protected virtual void OnDpiChanged()
        {
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
                UpdateDpi(newDpi);
                OnDpiChanged();
            }

            return NativeMethods.CallNextHook(IntPtr.Zero, nCode, wParam, lParam);
        }

        private void UpdateDpi(double newDpi)
        {
            Dpi = newDpi;
            UpdateLocation();
            UpdateSize();
        }

        private void UpdateSize()
        {
            var size = GetScaledSize(LogicalSize);
            Size = size;
            MinimumSize = size;
            MaximumSize = size;
        }

        private void UpdateLocation()
        {
            Location = GetScaledPoint(LogicalLocation);
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
