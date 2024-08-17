using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelPalette.Color;

// ReSharper disable InlineTemporaryVariable

namespace PixelPalette.Bitmap;

public static class BitmapUtil
{
    public static BitmapSource CropBitmapSource(
        WriteableBitmap source,
        int sourceX,
        int sourceY,
        int width,
        int height,
        byte[]? cachedOutputPixelBuffer = null
    )
    {
        // A couple things are hardcoded to expect 4 bytes per pixel.
        if (source.Format != PixelFormats.Bgra32) throw new Exception("Unexpected Bitmap format.");
        const int bytesPerPixel = 4;

        var sourceWidth = source.PixelWidth;
        var sourceHeight = source.PixelHeight;
        var sourceStride = sourceWidth * bytesPerPixel;

        var outputWidth = width;
        var outputHeight = height;
        var outputStride = outputWidth * bytesPerPixel;
        var outputBuffer = cachedOutputPixelBuffer ?? new byte[outputStride * outputHeight];

        // Fill the crop with black
        for (var x = 0; x < outputWidth; x++)
        {
            for (var y = 0; y < outputHeight; y++)
            {
                var offset = outputStride * y + x * bytesPerPixel;
                outputBuffer[offset] = 0; // B
                outputBuffer[offset + 1] = 0; // G
                outputBuffer[offset + 2] = 0; // R
                outputBuffer[offset + 3] = 255; // A
            }
        }

        // Copy the crop-area pixels from the source to the output bitmap.
        for (var y = 0; y < height; y++)
        {
            // Ignore out-of-range pixels.
            if (sourceY + y < 0 || sourceY + y > sourceHeight - 1) continue;

            var rowOffset = (sourceY + y) * sourceStride;

            for (var x = 0; x < width; x++)
            {
                // Ignore out-of-range pixels.
                if (sourceX + x < 0 || sourceX + x > sourceWidth - 1) continue;

                var sourceOffset = rowOffset + (sourceX + x) * bytesPerPixel;
                var outputOffset = y * outputStride + x * bytesPerPixel;

                unsafe
                {
                    // Get the pixel data directly from the source bitmap.
                    var sourcePointer = (byte*)source.BackBuffer.ToPointer() + sourceOffset;
                    outputBuffer[outputOffset] = sourcePointer[0]; // B
                    outputBuffer[outputOffset + 1] = sourcePointer[1]; // G
                    outputBuffer[outputOffset + 2] = sourcePointer[2]; // R
                    outputBuffer[outputOffset + 3] = 255; // A
                }
            }
        }

        return BitmapSource.Create(
            width,
            height,
            96,
            96,
            source.Format,
            null,
            outputBuffer,
            outputStride
        );
    }

    private static readonly byte[] PixelToRgbBuffer = new byte[4]; // Bgra32 pixel buffer

    public static Rgb PixelToRgb(BitmapSource source, int x, int y)
    {
        if (source.Format != PixelFormats.Bgra32) throw new Exception("Unexpected Bitmap format.");

        const int bytesPerPixel = 4; // Bgra32 has 4 bytes per pixel
        var stride = source.PixelWidth * bytesPerPixel;

        source.CopyPixels(new Int32Rect(x, y, 1, 1), PixelToRgbBuffer, stride, 0);

        return Rgb.FromScaledValues(PixelToRgbBuffer[2], PixelToRgbBuffer[1], PixelToRgbBuffer[0]);
    }

    public static Rgb AverageColor(BitmapSource source)
    {
        if (source.Format != PixelFormats.Bgra32) throw new Exception("Unexpected Bitmap format.");

        const int bytesPerPixel = 4; // Bgra32 format has 4 bytes per pixel
        var width = source.PixelWidth;
        var height = source.PixelHeight;
        var numPixels = width * height;

        long blue = 0;
        long green = 0;
        long red = 0;

        // Create a buffer to hold pixel data
        var stride = width * bytesPerPixel;
        var buffer = new byte[height * stride];
        source.CopyPixels(buffer, stride, 0);

        for (var i = 0; i < numPixels; i++)
        {
            var offset = i * bytesPerPixel;
            blue += buffer[offset];
            green += buffer[offset + 1];
            red += buffer[offset + 2];
        }
        
        return Rgb.FromScaledValues(
            (byte) (red / numPixels),
            (byte) (green / numPixels),
            (byte) (blue / numPixels)
        );
    }
}