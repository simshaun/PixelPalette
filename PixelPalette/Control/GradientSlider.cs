using System;
using System.Runtime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(GradientSlider),
                new FrameworkPropertyMetadata(typeof(GradientSlider))
            );
        }

        public GradientSlider()
        {
            Minimum = 0.0;
            Maximum = 1.0;
            LargeChange = 0.05;
            SmallChange = 0.01;
            TickFrequency = 0.001;
            IsSnapToTickEnabled = true;

            // Speed up the arrow keys if Shift is held down:
            // Note: Currently broken due to KeyboardNavigation.DirectionalNavigation "None" being bugged.
            var originalSmallChange = SmallChange;
            KeyDown += (_, _) =>
            {
                if (Keyboard.Modifiers != ModifierKeys.Shift) return;
                SmallChange = 10;
            };

            KeyUp += (_, ev) =>
            {
                if (ev.Key != Key.LeftShift && ev.Key != Key.RightShift) return;
                SmallChange = originalSmallChange;
            };

            var slider = this;
            slider.AddHandler(Thumb.DragStartedEvent, new DragStartedEventHandler((_, _) => { GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency; }));
            slider.AddHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler((_, _) => { GCSettings.LatencyMode = GCLatencyMode.Interactive; }));
        }
    }
}
