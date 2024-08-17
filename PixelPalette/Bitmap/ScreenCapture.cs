using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace PixelPalette.Bitmap;

public static class ScreenCapture
{
    public static BitmapSource GetFullScreen()
    {
        return GetCapture(
            SystemInformation.VirtualScreen.Left,
            SystemInformation.VirtualScreen.Top,
            SystemInformation.VirtualScreen.Width,
            SystemInformation.VirtualScreen.Height
        );
    }

    private static BitmapSource GetCapture(int x, int y, int width, int height)
    {
        // Use a smaller pixel format Format24bppRgb to reduce memory usage compared to Format32bppPArgb
        using var screenBmp = new System.Drawing.Bitmap(width, height, PixelFormat.Format24bppRgb);
        using var gfx = Graphics.FromImage(screenBmp);
        gfx.CopyFromScreen(x, y, 0, 0, screenBmp.Size);

        var hBitmap = screenBmp.GetHbitmap();
        using var bitmapHandle = new SafeHBitmapHandle(hBitmap, true);
        
        // Note: MS's Interop creates this bitmap in Bgra32 format.
        var source = Imaging.CreateBitmapSourceFromHBitmap(
            hBitmap,
            nint.Zero,
            Int32Rect.Empty,
            BitmapSizeOptions.FromWidthAndHeight(width, height)
        );

        source.Freeze();

        return source;
    }
}