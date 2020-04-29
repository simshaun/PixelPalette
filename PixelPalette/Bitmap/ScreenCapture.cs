using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace PixelPalette.Bitmap
{
    public class ScreenCapture
    {
        public BitmapSource GetFullScreen()
        {
            return GetCapture(
                0,
                0,
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
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions()
                );

                return source;
            }
            finally
            {
                // Prevent memory leak. http://msdn.microsoft.com/en-us/library/1dz311e4.aspx
                BitmapUtil.DeleteObject(hBitmap);
            }
        }
    }
}