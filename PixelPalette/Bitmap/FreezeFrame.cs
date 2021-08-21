using System.Windows.Media.Imaging;

namespace PixelPalette.Bitmap
{
    /// <summary>
    /// A dirty singleton. Purpose is to reduce memory allocation for some bitmap stuff.
    ///
    /// Not thread-safe. https://csharpindepth.com/articles/singleton
    /// </summary>
    public sealed class FreezeFrame
    {
        private static FreezeFrame? _instance;

        public static FreezeFrame Instance => _instance ??= new FreezeFrame();

        private BitmapSource? _bitmapSource;

        public BitmapSource? BitmapSource
        {
            get => _bitmapSource;
            set
            {
                if (value != null)
                {
                    var bytesPerPixel = value.Format.BitsPerPixel / 8;
                    var width = value.PixelWidth;
                    var height = value.PixelHeight;
                    var stride = width * bytesPerPixel;
                    var buffer = new byte[stride * height];
                    value.CopyPixels(buffer, stride, 0);
                    PixelBuffer = buffer;
                }

                _bitmapSource = value;
            }
        }

        public byte[]? PixelBuffer { get; private set; }
    }
}
