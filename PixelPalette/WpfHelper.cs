using System.Linq;
using System.Windows;

namespace PixelPalette
{
    public static class WpfHelper
    {
        public static T FindFirstVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj is T o)
            {
                return o;
            }

            foreach (var child in LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>())
            {
                if (child is T c)
                {
                    return c;
                }

                var match = FindFirstVisualChild<T>(child);
                if (match != null)
                {
                    return match;
                }
            }

            return null;
        }
    }
}