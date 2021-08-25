using System.Collections.ObjectModel;
using System.Windows.Media;
using PixelPalette.Color;

namespace PixelPalette.Window
{
    public class HistoryItem
    {
        public Brush ColorBrush { get; }
        public string ColorString { get; }
        public Rgb Color { get; }
        
        public HistoryItem(Rgb color)
        {
            Color = color;
            ColorString = color.ToHex().ToString();
            ColorBrush = new SolidColorBrush(Color.ToMediaColor());
        }
    }

    public sealed class HistoryWindowViewModel
    {
        public ObservableCollection<HistoryItem> States { get; } = new();
    }
}
