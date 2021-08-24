using System.Runtime.InteropServices;

namespace PixelPalette.Util
{
    public static class Mouse
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct Win32Point
        {
            public readonly int X;
            public readonly int Y;
        }

        public static Position GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Position(w32Mouse.X, w32Mouse.Y);
        }
    }

    public class Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
