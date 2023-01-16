using System;
using System.Runtime;
using System.Windows.Media.Imaging;

namespace PixelPalette.Bitmap;

/// <summary>
/// A singleton whose purpose is to reduce memory allocation for bitmap stuff.
/// </summary>
public sealed class FreezeFrame : IDisposable
{
    private static FreezeFrame? _instance;
    public byte[]? PixelBuffer { get; private set; }

    private static readonly object Padlock = new();

    public static FreezeFrame Instance
    {
        get
        {
            lock (Padlock)
            {
                return _instance ??= new FreezeFrame();
            }
        }
    }

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

    public void Dispose()
    {
        _bitmapSource = null;
        PixelBuffer = null;

        /*
         * The buffer array is large enough to be placed in the Large Object Heap (LOH).
         * Force it to be collected.
         */
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        GC.Collect();
    }
}