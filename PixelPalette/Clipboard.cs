namespace PixelPalette
{
    public static class Clipboard
    {
        /// <summary>
        /// Convenience method so I can easily fix problems from one location, in case I have more trouble with the clipboard.
        /// </summary>
        public static void Set(object data)
        {
            // SetDataObject seems to work better than SetText or other methods, when programs like RealVNC are locking up the clipboard.
            System.Windows.Clipboard.SetDataObject(data);
        }
    }
}
