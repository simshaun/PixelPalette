using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelPalette.Color;

// ReSharper disable InlineTemporaryVariable

namespace PixelPalette.Bitmap
{
    public static class BitmapUtil
    {
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static BitmapSource CropBitmapSource(
            BitmapSource source,
            int sourceX,
            int sourceY,
            int width,
            int height,
            byte[] cachedSourcePixels = null
        )
        {
            // A couple things are hardcoded to expect 4 bytes per pixel.
            if (source.Format != PixelFormats.Bgra32) throw new Exception("Unexpected Bitmap format.");

            var bytesPerPixel = source.Format.BitsPerPixel / 8;

            var sourceWidth = source.PixelWidth;
            var sourceHeight = source.PixelHeight;
            var sourceStride = sourceWidth * bytesPerPixel;
            var sourceBuffer = cachedSourcePixels;
            if (cachedSourcePixels == null)
            {
                sourceBuffer = new byte[sourceStride * sourceHeight];
                source.CopyPixels(sourceBuffer, sourceStride, 0);
            }

            var outputWidth = width;
            var outputHeight = height;
            var outputStride = outputWidth * bytesPerPixel;
            var outputBuffer = new byte[outputStride * outputHeight];

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
                if (rowOffset < 0 || rowOffset >= sourceBuffer.Length) continue;

                for (var x = 0; x < width; x++)
                {
                    // Ignore out-of-range pixels.
                    if (sourceX + x < 0 || sourceX + x > sourceWidth - 1) continue;

                    var sourceOffset = rowOffset + (sourceX + x) * bytesPerPixel;
                    var outputOffset = y * outputStride + x * bytesPerPixel;

                    if (sourceOffset < 0 || sourceOffset >= sourceBuffer.Length) continue;
                    outputBuffer[outputOffset] = sourceBuffer[sourceOffset];
                    outputBuffer[outputOffset + 1] = sourceBuffer[sourceOffset + 1];
                    outputBuffer[outputOffset + 2] = sourceBuffer[sourceOffset + 2];
                    outputBuffer[outputOffset + 3] = 255;
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

        public static Rgb PixelToRgb(BitmapSource source, int x, int y)
        {
            if (source.Format != PixelFormats.Bgra32) throw new Exception("Unexpected Bitmap format.");

            var bytesPerPixel = source.Format.BitsPerPixel / 8;
            var bytes = new byte[bytesPerPixel];
            var rect = new Int32Rect(x, y, 1, 1);
            source.CopyPixels(rect, bytes, bytesPerPixel, 0);

            return Rgb.FromScaledValues(bytes[2], bytes[1], bytes[0]);
        }

        public static Rgb AverageColor(BitmapSource source)
        {
            if (source.Format != PixelFormats.Bgra32) throw new Exception("Unexpected Bitmap format.");

            var width = source.PixelWidth;
            var height = source.PixelHeight;
            var numPixels = width * height;
            var bytesPerPixel = source.Format.BitsPerPixel / 8;
            var pixelBuffer = new byte[numPixels * bytesPerPixel];
            source.CopyPixels(pixelBuffer, width * bytesPerPixel, 0);

            long blue = 0;
            long green = 0;
            long red = 0;

            for (var i = 0; i < pixelBuffer.Length; i += bytesPerPixel)
            {
                blue += pixelBuffer[i];
                green += pixelBuffer[i + 1];
                red += pixelBuffer[i + 2];
            }

            return Rgb.FromScaledValues((byte) (red / numPixels), (byte) (green / numPixels), (byte) (blue / numPixels));
        }
    }
}