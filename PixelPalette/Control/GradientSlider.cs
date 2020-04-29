using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PixelPalette.Control
{
    /// <summary>
    /// Step 1) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:control="clr-namespace:PixelPalette.Control"
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <control:GradientSlider/>
    ///
    /// </summary>
    public class GradientSlider : Slider
    {
        static GradientSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GradientSlider),
                new FrameworkPropertyMetadata(typeof(GradientSlider)));
        }

        public GradientSlider()
        {
            Minimum = 0;
            Maximum = 255;
            LargeChange = 50;
            SmallChange = 1;
            TickFrequency = 1;
            IsSnapToTickEnabled = true;

            // Speed up the arrow keys if Shift is held down:
            // Note: Currently broken due to KeyboardNavigation.DirectionalNavigation "None" being bugged.
            var originalSmallChange = SmallChange;
            KeyDown += (o, ev) =>
            {
                if (Keyboard.Modifiers != ModifierKeys.Shift) return;
                SmallChange = 10;
            };

            KeyUp += (o, ev) =>
            {
                if (ev.Key != Key.LeftShift && ev.Key != Key.RightShift) return;
                SmallChange = originalSmallChange;
            };
        }
    }
}