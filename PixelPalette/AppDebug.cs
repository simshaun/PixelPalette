using System.Collections.Generic;

namespace PixelPalette
{
    public static class AppDebug
    {
        private static List<string> Lines { get; } = new List<string>();

        public static string LinesText => string.Join("\n", Lines);

        public static void WriteLine(string text)
        {
            Lines.Add(text);
        }
    }
}