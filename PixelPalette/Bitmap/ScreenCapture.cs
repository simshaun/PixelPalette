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
        using var screenBmp = new System.Drawing.Bitmap(width, height, PixelFormat.Format32bppPArgb);
        using var gfx = Graphics.FromImage(screenBmp);
        gfx.CopyFromScreen(x, y, 0, 0, screenBmp.Size);

        var hBitmap = screenBmp.GetHbitmap();

        try
        {
            var source = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                nint.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );

            source.Freeze();

            return source;
        }
        finally
        {
            // Prevent memory leak. http://msdn.microsoft.com/en-us/library/1dz311e4.aspx
            BitmapUtil.DeleteObject(hBitmap);
        }
    }
}