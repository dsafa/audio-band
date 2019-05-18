using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace AudioBand
{
#pragma warning disable
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

        [DllImport("shell32")]
        public static extern int SHGetPropertyStoreForWindow(IntPtr hwnd, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyStore ppv);

        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        /// <summary>
        /// Sets the application user model id for the window.
        /// </summary>
        /// <param name="hwnd">The window.</param>
        /// <param name="appId">The application id to set.</param>
        public static void SetWindowAppId(IntPtr hwnd, string appId)
        {
            var iPropertyStoreGuid = new Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99");
            SHGetPropertyStoreForWindow(hwnd, ref iPropertyStoreGuid, out IPropertyStore propertyStore);
            var appid = new PROPVARIANT(appId);
            propertyStore.SetValue(ref PropertyKey.SystemAppUserModelIDPropertyKey, appid);
            PROPVARIANT.ClearProp(appid);
        }

        // Fix blur in windows 1903 causing lag by falling back to old blur behind.
        public static void FixWindowComposition(Window w)
        {
            var win1903 = new Version(10, 0, 18362);
            if (Environment.OSVersion.Version < win1903)
            {
                return;
            }

            var accent = default(AccentPolicy);
            accent.AccentFlags = 2;
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            var accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = default(WindowCompositionAttributeData);
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(new WindowInteropHelper(w).Handle, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }

        // From pinvoke.net
        [ComImport]
        [Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPropertyStore
        {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int GetCount([Out] out uint propertyCount);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int GetAt([In] uint propertyIndex, out PropertyKey key);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int GetValue([In] ref PropertyKey key, [Out] PROPVARIANT pv);

            [PreserveSig]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int SetValue([In] ref PropertyKey key, [In] PROPVARIANT pv);

            [PreserveSig]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int Commit();
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CWPSTRUCT
        {
            public IntPtr LParam;
            public IntPtr WParam;
            public int Message;
            public IntPtr Hwnd;
        }

        public struct PropertyKey
        {
            public static PropertyKey SystemAppUserModelIDPropertyKey = new PropertyKey(new Guid("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}"), 5);

            public Guid FormatId;
            public int PropertyId;

            public PropertyKey(Guid guid, int propertyId)
            {
                FormatId = guid;
                PropertyId = propertyId;
            }
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct PROPVARIANT
        {
            [FieldOffset(0)]
            internal ushort varType;
            [FieldOffset(2)]
            internal ushort wReserved1;
            [FieldOffset(4)]
            internal ushort wReserved2;
            [FieldOffset(6)]
            internal ushort wReserved3;
            [FieldOffset(8)]
            internal IntPtr pwszVal; // unicode string pointer

            public PROPVARIANT(string value)
            {
                varType = (ushort)VarEnum.VT_LPWSTR;
                pwszVal = Marshal.StringToCoTaskMemUni(value);
                wReserved1 = 0;
                wReserved2 = 0;
                wReserved3 = 0;
            }

            public static void ClearProp(PROPVARIANT p)
            {
                if (p.pwszVal != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(p.pwszVal);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }

        internal enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
            ACCENT_ENABLE_HOSTBACKDROP = 5,
            ACCENT_INVALID_STATE = 6,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public uint GradientColor;
            public int AnimationId;
        }
    }
#pragma warning restore
}
