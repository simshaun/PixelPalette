using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PixelPalette.ValueConverters;

public class ColorToBrushConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || (string) value == "")
        {
            value = "transparent";
        }

        return new BrushConverter().ConvertFrom(value) as SolidColorBrush ?? throw new InvalidOperationException();
    }

    public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        Debug.Assert(value != null, nameof(value) + " != null");

        return ((SolidColorBrush) value).Color.ToString();
    }
}