using System;
using System.Runtime.InteropServices;

namespace PixelPalette.Window
{
    public static class WindowHelper
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        // ReSharper disable once InconsistentNaming
        private const int GWL_STYLE = -16;

        // ReSharper disable once InconsistentNaming
        private const int GWL_EXSTYLE = -20;

        // ReSharper disable once InconsistentNaming
        private const int WS_MAXIMIZE_BOX = 0x10000;

        // ReSharper disable once InconsistentNaming
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        public static void DisableMaximize(IntPtr hWnd)
        {
            var value = GetWindowLong(hWnd, GWL_STYLE);
            SetWindowLong(hWnd, GWL_STYLE, value & ~WS_MAXIMIZE_BOX);
        }

        public static void DisableAltTab(IntPtr hWnd)
        {
            var value = GetWindowLong(hWnd, GWL_EXSTYLE);
            SetWindowLong(hWnd, GWL_EXSTYLE, value | WS_EX_TOOLWINDOW);
        }
    }
}
