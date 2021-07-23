using System;
using System.Text;

namespace SpotifyAudioSource
{
    // Controls and gets info from spotify. Implementation from rainmeter.
    public class SpotifyControls
    {
        public const string SpotifyPausedWindowTitle = "Spotify";
        private const string SpotifyWindowClassName = "Chrome_WidgetWin_0"; // Last checked spotify version 1.0.88.353
        private IntPtr _spotifyHwnd;

        /// <summary>
        /// Gets the title of the spotify window.
        /// </summary>
        /// <returns>The title of the spotify window. Null if not found.</returns>
        public string GetSpotifyWindowTitle()
        {
            // find spotify window by class name since the title changes depending on if a song is playing
            var hwnd = NativeMethods.FindWindowEx(IntPtr.Zero, IntPtr.Zero, SpotifyWindowClassName, null);
            if (hwnd == IntPtr.Zero)
            {
                return null;
            }

            // We are looking for the topmost window with the classname
            // We keep looking upwards by calling findwindow again and setting the child as the previous value
            // Those other windows will have an empty window title
            while (GetTitle(hwnd).Length == 0)
            {
                // At the top, this call should return null
                var nextHwnd = NativeMethods.FindWindowEx(IntPtr.Zero, hwnd, SpotifyWindowClassName, null);
                if (nextHwnd == IntPtr.Zero)
                {
                    break;
                }

                hwnd = nextHwnd;
            }

            _spotifyHwnd = hwnd;
            return hwnd == IntPtr.Zero ? null : GetTitle(hwnd);
        }

        public string GetSpotifyWindowClassName()
        {
            return SpotifyWindowClassName;
        }

        public void Play()
        {
            if (_spotifyHwnd == IntPtr.Zero)
            {
                return;
            }

            NativeMethods.SendMessage(_spotifyHwnd, NativeMethods.WM_APPCOMMAND, 0, new IntPtr((int)NativeMethods.AppCommand.Play));
        }

        public void Pause()
        {
            if (_spotifyHwnd == IntPtr.Zero)
            {
                return;
            }

            NativeMethods.SendMessage(_spotifyHwnd, NativeMethods.WM_APPCOMMAND, 0, new IntPtr((int)NativeMethods.AppCommand.Pause));
        }

        public void Previous()
        {
            if (_spotifyHwnd == IntPtr.Zero)
            {
                return;
            }

            NativeMethods.SendMessage(_spotifyHwnd, NativeMethods.WM_APPCOMMAND, 0, new IntPtr((int)NativeMethods.AppCommand.Previous));
        }

        public void Next()
        {
            if (_spotifyHwnd == IntPtr.Zero)
            {
                return;
            }

            NativeMethods.SendMessage(_spotifyHwnd, NativeMethods.WM_APPCOMMAND, 0, new IntPtr((int)NativeMethods.AppCommand.Next));
        }

        public bool IsPaused()
        {
            return GetSpotifyWindowTitle() == SpotifyPausedWindowTitle;
        }

        private static string GetTitle(IntPtr hwnd)
        {
            var titleLength = NativeMethods.GetWindowTextLength(hwnd);
            var title = new StringBuilder(titleLength + 1);
            NativeMethods.GetWindowText(hwnd, title, title.Capacity);

            return title.ToString();
        }
    }
}
