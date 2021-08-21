using System.Windows;
using System.Windows.Input;
using PixelPalette.Color;

namespace PixelPalette.Control.MainWindow
{
    public partial class HslTabContent
    {
        public GlobalState? GlobalState
        {
            get => GetValue(GlobalStateProperty) as GlobalState;
            set => SetValue(GlobalStateProperty, value);
        }

        public static readonly DependencyProperty GlobalStateProperty =
            DependencyProperty.Register(
                "GlobalState",
                typeof(GlobalState),
                typeof(HslTabContent),
                new FrameworkPropertyMetadata(GlobalState.Instance)
            );

        public HslTabContent()
        {
            InitializeComponent();
            Loaded += (_, _) =>
            {
                if (GlobalState == null) return;

                var vm = new HslTabViewModel(GlobalState);
                DataContext = vm;

                EventUtil.HandleKey(Key.Up, HueText,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithHue(GlobalState.Hsl.Hue + 0.01)); });
                EventUtil.HandleKey(Key.Up, SaturationText,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithSaturation(GlobalState.Hsl.Saturation + 0.01)); });
                EventUtil.HandleKey(Key.Up, LuminanceText,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithLuminance(GlobalState.Hsl.Luminance + 0.01)); });
                EventUtil.HandleKey(Key.Up, ScaledHueText,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledHue(GlobalState.Hsl.ScaledHue + 1)); });
                EventUtil.HandleKey(Key.Up, ScaledSaturationText,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledSaturation(GlobalState.Hsl.ScaledSaturation + 1)); });
                EventUtil.HandleKey(Key.Up, ScaledLuminanceText,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledLuminance(GlobalState.Hsl.ScaledLuminance + 1)); });

                EventUtil.HandleKey(Key.Down, HueText,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithHue(GlobalState.Hsl.Hue - 0.01)); });
                EventUtil.HandleKey(Key.Down, SaturationText,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithSaturation(GlobalState.Hsl.Saturation - 0.01)); });
                EventUtil.HandleKey(Key.Down, LuminanceText,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithLuminance(GlobalState.Hsl.Luminance - 0.01)); });
                EventUtil.HandleKey(Key.Down, ScaledHueText,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledHue(GlobalState.Hsl.ScaledHue - 1)); });
                EventUtil.HandleKey(Key.Down, ScaledSaturationText,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledSaturation(GlobalState.Hsl.ScaledSaturation - 1)); });
                EventUtil.HandleKey(Key.Down, ScaledLuminanceText,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledLuminance(GlobalState.Hsl.ScaledLuminance - 1)); });

                EventUtil.HandleMouseWheel(
                    HueText,
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithHue(GlobalState.Hsl.Hue + 0.01)); },
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithHue(GlobalState.Hsl.Hue - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    SaturationText,
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithSaturation(GlobalState.Hsl.Saturation + 0.01)); },
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithSaturation(GlobalState.Hsl.Saturation - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    LuminanceText,
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithLuminance(GlobalState.Hsl.Luminance + 0.01)); },
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithLuminance(GlobalState.Hsl.Luminance - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    ScaledHueText,
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledHue(GlobalState.Hsl.ScaledHue + 1)); },
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledHue(GlobalState.Hsl.ScaledHue - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    ScaledSaturationText,
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledSaturation(GlobalState.Hsl.ScaledSaturation + 1)); },
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledSaturation(GlobalState.Hsl.ScaledSaturation - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    ScaledLuminanceText,
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledLuminance(GlobalState.Hsl.ScaledLuminance + 1)); },
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledLuminance(GlobalState.Hsl.ScaledLuminance - 1)); }
                );

                var hThrottler = new Throttler();
                var sThrottler = new Throttler();
                var lThrottler = new Throttler();

                EventUtil.HandleSliderChange(HueSlider, value => { hThrottler.Throttle(1, _ => GlobalState.RefreshFromHsl(GlobalState.Hsl.WithHue(value))); });
                EventUtil.HandleSliderChange(SaturationSlider, value => { sThrottler.Throttle(1, _ => GlobalState.RefreshFromHsl(GlobalState.Hsl.WithSaturation(value))); });
                EventUtil.HandleSliderChange(LuminanceSlider, value => { lThrottler.Throttle(1, _ => GlobalState.RefreshFromHsl(GlobalState.Hsl.WithLuminance(value))); });

                vm.PropertyChangedByUser += (_, ev) =>
                {
                    bool isDouble;
                    double doubleVal;
                    Hsl? nullableHsl;

                    switch (ev.PropertyName)
                    {
                        case nameof(HslTabViewModel.Text):
                            nullableHsl = Hsl.FromString(vm.Text);
                            if (nullableHsl.HasValue) GlobalState.RefreshFromHsl(nullableHsl.Value);
                            break;
                        case nameof(HslTabViewModel.ScaledText):
                            nullableHsl = Hsl.FromScaledString(vm.ScaledText);
                            if (nullableHsl.HasValue) GlobalState.RefreshFromHsl(nullableHsl.Value);
                            break;
                        case nameof(HslTabViewModel.Hue):
                            isDouble = double.TryParse(vm.Hue, out doubleVal);
                            if (!isDouble || !Hsl.IsValidHue(doubleVal)) return;
                            GlobalState.RefreshFromHsl(GlobalState.Hsl.WithHue(doubleVal));
                            break;
                        case nameof(HslTabViewModel.Saturation):
                            isDouble = double.TryParse(vm.Saturation, out doubleVal);
                            if (!isDouble || !Hsl.IsValidSaturation(doubleVal)) return;
                            GlobalState.RefreshFromHsl(GlobalState.Hsl.WithSaturation(doubleVal));
                            break;
                        case nameof(HslTabViewModel.Luminance):
                            isDouble = double.TryParse(vm.Luminance, out doubleVal);
                            if (!isDouble || !Hsl.IsValidLuminance(doubleVal)) return;
                            GlobalState.RefreshFromHsl(GlobalState.Hsl.WithLuminance(doubleVal));
                            break;
                        case nameof(HslTabViewModel.ScaledHue):
                            isDouble = double.TryParse(vm.ScaledHue, out doubleVal);
                            if (!isDouble || !Hsl.IsValidScaledHue(doubleVal)) return;
                            GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledHue(doubleVal));
                            break;
                        case nameof(HslTabViewModel.ScaledSaturation):
                            isDouble = double.TryParse(vm.ScaledHue, out doubleVal);
                            if (!isDouble || !Hsl.IsValidScaledSaturation(doubleVal)) return;
                            GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledSaturation(doubleVal));
                            break;
                        case nameof(HslTabViewModel.ScaledLuminance):
                            isDouble = double.TryParse(vm.ScaledHue, out doubleVal);
                            if (!isDouble || !Hsl.IsValidScaledLuminance(doubleVal)) return;
                            GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledLuminance(doubleVal));
                            break;
                    }
                };

                ScaledTextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.ScaledText); };
                TextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.Text); };
            };
        }
    }
}
