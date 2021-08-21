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

                EventUtil.HandleKey(Key.Up, HslHue,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithHue(GlobalState.Hsl.Hue + 0.01)); });
                EventUtil.HandleKey(Key.Up, HslSaturation,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithSaturation(GlobalState.Hsl.Saturation + 0.01)); });
                EventUtil.HandleKey(Key.Up, HslLuminance,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithLuminance(GlobalState.Hsl.Luminance + 0.01)); });
                EventUtil.HandleKey(Key.Up, HslScaledHue,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledHue(GlobalState.Hsl.ScaledHue + 1)); });
                EventUtil.HandleKey(Key.Up, HslScaledSaturation,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledSaturation(GlobalState.Hsl.ScaledSaturation + 1)); });
                EventUtil.HandleKey(Key.Up, HslScaledLuminance,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledLuminance(GlobalState.Hsl.ScaledLuminance + 1)); });

                EventUtil.HandleKey(Key.Down, HslHue,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithHue(GlobalState.Hsl.Hue - 0.01)); });
                EventUtil.HandleKey(Key.Down, HslSaturation,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithSaturation(GlobalState.Hsl.Saturation - 0.01)); });
                EventUtil.HandleKey(Key.Down, HslLuminance,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithLuminance(GlobalState.Hsl.Luminance - 0.01)); });
                EventUtil.HandleKey(Key.Down, HslScaledHue,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledHue(GlobalState.Hsl.ScaledHue - 1)); });
                EventUtil.HandleKey(Key.Down, HslScaledSaturation,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledSaturation(GlobalState.Hsl.ScaledSaturation - 1)); });
                EventUtil.HandleKey(Key.Down, HslScaledLuminance,
                                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledLuminance(GlobalState.Hsl.ScaledLuminance - 1)); });

                EventUtil.HandleMouseWheel(
                    HslHue,
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithHue(GlobalState.Hsl.Hue + 0.01)); },
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithHue(GlobalState.Hsl.Hue - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    HslSaturation,
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithSaturation(GlobalState.Hsl.Saturation + 0.01)); },
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithSaturation(GlobalState.Hsl.Saturation - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    HslLuminance,
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithLuminance(GlobalState.Hsl.Luminance + 0.01)); },
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithLuminance(GlobalState.Hsl.Luminance - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    HslScaledHue,
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledHue(GlobalState.Hsl.ScaledHue + 1)); },
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledHue(GlobalState.Hsl.ScaledHue - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    HslScaledSaturation,
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledSaturation(GlobalState.Hsl.ScaledSaturation + 1)); },
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledSaturation(GlobalState.Hsl.ScaledSaturation - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    HslScaledLuminance,
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledLuminance(GlobalState.Hsl.ScaledLuminance + 1)); },
                    () => { GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledLuminance(GlobalState.Hsl.ScaledLuminance - 1)); }
                );

                var hThrottler = new Throttler();
                var sThrottler = new Throttler();
                var lThrottler = new Throttler();

                EventUtil.HandleSliderChange(HslHueSlider, value => { hThrottler.Throttle(1, _ => GlobalState.RefreshFromHsl(GlobalState.Hsl.WithHue(value))); });
                EventUtil.HandleSliderChange(HslSaturationSlider, value => { sThrottler.Throttle(1, _ => GlobalState.RefreshFromHsl(GlobalState.Hsl.WithSaturation(value))); });
                EventUtil.HandleSliderChange(HslLuminanceSlider, value => { lThrottler.Throttle(1, _ => GlobalState.RefreshFromHsl(GlobalState.Hsl.WithLuminance(value))); });

                vm.PropertyChangedByUser += (_, ev) =>
                {
                    bool isDouble;
                    double doubleVal;
                    Hsl? nullableHsl;

                    switch (ev.PropertyName)
                    {
                        case nameof(HslTabViewModel.HslText):
                            nullableHsl = Hsl.FromString(vm.HslText);
                            if (nullableHsl.HasValue) GlobalState.RefreshFromHsl(nullableHsl.Value);
                            break;
                        case nameof(HslTabViewModel.HslScaledText):
                            nullableHsl = Hsl.FromScaledString(vm.HslScaledText);
                            if (nullableHsl.HasValue) GlobalState.RefreshFromHsl(nullableHsl.Value);
                            break;
                        case nameof(HslTabViewModel.HslHue):
                            isDouble = double.TryParse(vm.HslHue, out doubleVal);
                            if (!isDouble || !Hsl.IsValidHue(doubleVal)) return;
                            GlobalState.RefreshFromHsl(GlobalState.Hsl.WithHue(doubleVal));
                            break;
                        case nameof(HslTabViewModel.HslSaturation):
                            isDouble = double.TryParse(vm.HslSaturation, out doubleVal);
                            if (!isDouble || !Hsl.IsValidSaturation(doubleVal)) return;
                            GlobalState.RefreshFromHsl(GlobalState.Hsl.WithSaturation(doubleVal));
                            break;
                        case nameof(HslTabViewModel.HslLuminance):
                            isDouble = double.TryParse(vm.HslLuminance, out doubleVal);
                            if (!isDouble || !Hsl.IsValidLuminance(doubleVal)) return;
                            GlobalState.RefreshFromHsl(GlobalState.Hsl.WithLuminance(doubleVal));
                            break;
                        case nameof(HslTabViewModel.HslScaledHue):
                            isDouble = double.TryParse(vm.HslScaledHue, out doubleVal);
                            if (!isDouble || !Hsl.IsValidScaledHue(doubleVal)) return;
                            GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledHue(doubleVal));
                            break;
                        case nameof(HslTabViewModel.HslScaledSaturation):
                            isDouble = double.TryParse(vm.HslScaledHue, out doubleVal);
                            if (!isDouble || !Hsl.IsValidScaledSaturation(doubleVal)) return;
                            GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledSaturation(doubleVal));
                            break;
                        case nameof(HslTabViewModel.HslScaledLuminance):
                            isDouble = double.TryParse(vm.HslScaledHue, out doubleVal);
                            if (!isDouble || !Hsl.IsValidScaledLuminance(doubleVal)) return;
                            GlobalState.RefreshFromHsl(GlobalState.Hsl.WithScaledLuminance(doubleVal));
                            break;
                    }
                };

                HslScaledTextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.HslScaledText); };
                HslTextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.HslText); };
            };
        }
    }
}
