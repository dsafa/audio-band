using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Media;

namespace AudioBand.Behaviors
{
    /// <summary>
    /// Behaviour to configure dpi scaling. Dpi scaling is manually handled for wpf controls that are children hosted in a HwndSource.
    /// </summary>
    public class DpiScaling : Behavior<FrameworkElement>
    {
        /// <summary>
        /// Dependency property fore <see cref="CurrentDpi"/>.
        /// </summary>
        public static readonly DependencyProperty CurrentDpiProperty
            = DependencyProperty.Register(nameof(CurrentDpi), typeof(double), typeof(DpiScaling), new PropertyMetadata(96.0));

        /// <summary>
        /// Dependency property for <see cref="InitialDpi"/>.
        /// </summary>
        public static readonly DependencyProperty InitialDpiProperty
            = DependencyProperty.Register(nameof(InitialDpi), typeof(double), typeof(DpiScaling), new PropertyMetadata(96.0));

        private static readonly Version Win10Anniversary = new Version(10, 0, 14393);

        /// <summary>
        /// Gets or sets the current dpi.
        /// </summary>
        public double CurrentDpi
        {
            get => (double)GetValue(CurrentDpiProperty);
            set => SetValue(CurrentDpiProperty, value);
        }

        /// <summary>
        /// Gets or sets the initial dpi.
        /// </summary>
        public double InitialDpi
        {
            get => (double)GetValue(InitialDpiProperty);
            set => SetValue(InitialDpiProperty, value);
        }

        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject.IsLoaded)
            {
                AssociatedObjectOnLoaded(null, null);
            }
            else
            {
                AssociatedObject.Loaded += AssociatedObjectOnLoaded;
            }
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
            var hwndSource = PresentationSource.FromVisual(AssociatedObject) as HwndSource;
            hwndSource.RemoveHook(HwndSourceHook);
        }

        private static double GetParentWindowDpi(Visual visual)
        {
            if (Environment.OSVersion.Version.CompareTo(Win10Anniversary) < 0)
            {
                return 96.0;
            }

            var hwnd = PresentationSource.FromVisual(visual) as HwndSource;
            if (hwnd == null)
            {
                return 96.0;
            }

            return NativeMethods.GetDpiForWindow(hwnd.Handle);
        }

        private static short HiWord(IntPtr ptr)
        {
            return unchecked((short)((long)ptr >> 16));
        }

        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e)
        {
            var hwndSource = PresentationSource.FromVisual(AssociatedObject) as HwndSource;
            hwndSource.AddHook(HwndSourceHook);

            InitialDpi = VisualTreeHelper.GetDpi(AssociatedObject).PixelsPerInchY;
            CurrentDpi = InitialDpi;

            UpdateDpi(GetParentWindowDpi(AssociatedObject));
        }

        [DebuggerStepThrough]
        private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            const int WM_DPICHANGED_AFTERPARENT = 0x02E3;
            const int WM_DPICHANGED = 0x02E0;

            switch (msg)
            {
                case WM_DPICHANGED:
                    UpdateDpi(HiWord(wparam));

                    break;
                case WM_DPICHANGED_AFTERPARENT:
                    UpdateDpi(GetParentWindowDpi(AssociatedObject));
                    break;
            }

            return IntPtr.Zero;
        }

        private void UpdateDpi(double newDpi)
        {
            var dpiScale = newDpi / CurrentDpi;
            CurrentDpi = newDpi;

            AssociatedObject.Width *= dpiScale;
            AssociatedObject.Height *= dpiScale;

            if (VisualTreeHelper.GetChildrenCount(AssociatedObject) == 0)
            {
                return;
            }

            var child = VisualTreeHelper.GetChild(AssociatedObject, 0);
            double renderScale = newDpi / InitialDpi;

            var scaleTransform = Math.Abs(renderScale - 1) < 0.0001
                ? Transform.Identity
                : new ScaleTransform(renderScale, renderScale);
            child.SetValue(FrameworkElement.LayoutTransformProperty, scaleTransform);
        }
    }
}
