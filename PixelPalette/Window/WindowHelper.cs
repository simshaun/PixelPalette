using System.Runtime.InteropServices;

namespace PixelPalette.Window;

public static partial class WindowHelper
{
    [LibraryImport("user32.dll", EntryPoint = "GetWindowLongW")]
    private static partial int GetWindowLong(nint hWnd, int nIndex);

    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongW")]
    private static partial int SetWindowLong(nint hWnd, int nIndex, int dwNewLong);

    // ReSharper disable once InconsistentNaming
    private const int GWL_STYLE = -16;

    // ReSharper disable once InconsistentNaming
    private const int GWL_EXSTYLE = -20;

    // ReSharper disable once InconsistentNaming
    private const int WS_MAXIMIZE_BOX = 0x10000;

    // ReSharper disable once InconsistentNaming
    private const int WS_EX_TOOLWINDOW = 0x00000080;

    public static void DisableMaximize(nint hWnd)
    {
        var value = GetWindowLong(hWnd, GWL_STYLE);
        SetWindowLong(hWnd, GWL_STYLE, value & ~WS_MAXIMIZE_BOX);
    }

    public static void DisableAltTab(nint hWnd)
    {
        var value = GetWindowLong(hWnd, GWL_EXSTYLE);
        SetWindowLong(hWnd, GWL_EXSTYLE, value | WS_EX_TOOLWINDOW);
    }
}
