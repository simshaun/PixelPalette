using System.Collections.ObjectModel;
using System.Windows.Media;
using PixelPalette.Color;

namespace PixelPalette.Window
{
    public class HistoryItem
    {
        public Brush ColorBrush { get; }
        public string ColorString { get; }
        public IColor Color { get; }

        public HistoryItem(IColor color)
        {
            Color = color;
            ColorString = color.ToRgb().ToHex().ToString();
            ColorBrush = new SolidColorBrush(Color.ToRgb().ToMediaColor());
        }
    }

    public sealed class HistoryWindowViewModel
    {
        public ObservableCollection<HistoryItem> States { get; } = new ObservableCollection<HistoryItem>();
    }
}
