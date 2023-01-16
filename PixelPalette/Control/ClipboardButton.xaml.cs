using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using PixelPalette.Util;

namespace PixelPalette.Control;

public partial class ClipboardButton
{
    public ClipboardButton()
    {
        InitializeComponent();
    }

    public event EventHandler? ButtonClicked;

    private async void Button_OnClick(object sender, RoutedEventArgs e)
    {
        ButtonClicked?.Invoke(this, EventArgs.Empty);
        await DisplayClipboardCheckmark((Button) sender);
    }

    private async Task DisplayClipboardCheckmark(DependencyObject button)
    {
        var image = WpfHelper.FindFirstVisualChild<Image>(button);
        if (image == null) return;
        var oldSource = image.Source;
        image.Source = (BitmapImage) Resources["CheckmarkImage"];
        await Task.Delay(500);
        image.Source = oldSource;
    }
}