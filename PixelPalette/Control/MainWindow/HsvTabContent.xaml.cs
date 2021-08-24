using System.Windows;
using System.Windows.Input;
using PixelPalette.Color;
using PixelPalette.State;
using PixelPalette.Util;
using Clipboard = PixelPalette.Util.Clipboard;

namespace PixelPalette.Control.MainWindow
{
    public partial class HsvTabContent
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
                typeof(HsvTabContent),
                new FrameworkPropertyMetadata(GlobalState.Instance)
            );

        public HsvTabContent()
        {
            InitializeComponent();
            Loaded += (_, _) =>
            {
                if (GlobalState == null) return;

                var vm = new HsvTabViewModel(GlobalState);
                DataContext = vm;

                EventUtil.HandleKey(Key.Up, HueText,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithHue(GlobalState.Hsv.Hue + 0.01)); });
                EventUtil.HandleKey(Key.Up, SaturationText,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithSaturation(GlobalState.Hsv.Saturation + 0.01)); });
                EventUtil.HandleKey(Key.Up, ValueText,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithValue(GlobalState.Hsv.Value + 0.01)); });
                EventUtil.HandleKey(Key.Up, ScaledHueText,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledHue(GlobalState.Hsv.ScaledHue + 1)); });
                EventUtil.HandleKey(Key.Up, ScaledSaturationText,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledSaturation(GlobalState.Hsv.ScaledSaturation + 1)); });
                EventUtil.HandleKey(Key.Up, ScaledValueText,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledValue(GlobalState.Hsv.ScaledValue + 1)); });

                EventUtil.HandleKey(Key.Down, HueText,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithHue(GlobalState.Hsv.Hue - 0.01)); });
                EventUtil.HandleKey(Key.Down, SaturationText,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithSaturation(GlobalState.Hsv.Saturation - 0.01)); });
                EventUtil.HandleKey(Key.Down, ValueText,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithValue(GlobalState.Hsv.Value - 0.01)); });
                EventUtil.HandleKey(Key.Down, ScaledHueText,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledHue(GlobalState.Hsv.ScaledHue - 1)); });
                EventUtil.HandleKey(Key.Down, ScaledSaturationText,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledSaturation(GlobalState.Hsv.ScaledSaturation - 1)); });
                EventUtil.HandleKey(Key.Down, ScaledValueText,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledValue(GlobalState.Hsv.ScaledValue - 1)); });

                EventUtil.HandleMouseWheel(
                    HueText,
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithHue(GlobalState.Hsv.Hue + 0.01)); },
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithHue(GlobalState.Hsv.Hue - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    SaturationText,
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithSaturation(GlobalState.Hsv.Saturation + 0.01)); },
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithSaturation(GlobalState.Hsv.Saturation - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    ValueText,
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithValue(GlobalState.Hsv.Value + 0.01)); },
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithValue(GlobalState.Hsv.Value - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    ScaledHueText,
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledHue(GlobalState.Hsv.ScaledHue + 1)); },
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledHue(GlobalState.Hsv.ScaledHue - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    ScaledSaturationText,
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledSaturation(GlobalState.Hsv.ScaledSaturation + 1)); },
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledSaturation(GlobalState.Hsv.ScaledSaturation - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    ScaledValueText,
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledValue(GlobalState.Hsv.ScaledValue + 1)); },
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledValue(GlobalState.Hsv.ScaledValue - 1)); }
                );

                var hThrottler = new Throttler();
                var sThrottler = new Throttler();
                var vThrottler = new Throttler();

                EventUtil.HandleSliderChange(HueSlider, value => { hThrottler.Throttle(1, _ => GlobalState.RefreshFromHsv(GlobalState.Hsv.WithHue(value))); });
                EventUtil.HandleSliderChange(SaturationSlider, value => { sThrottler.Throttle(1, _ => GlobalState.RefreshFromHsv(GlobalState.Hsv.WithSaturation(value))); });
                EventUtil.HandleSliderChange(ValueSlider, value => { vThrottler.Throttle(1, _ => GlobalState.RefreshFromHsv(GlobalState.Hsv.WithValue(value))); });

                vm.PropertyChangedByUser += (_, ev) =>
                {
                    bool isDouble;
                    double doubleVal;
                    Hsv? nullableHsv;

                    switch (ev.PropertyName)
                    {
                        case nameof(HsvTabViewModel.Text):
                            nullableHsv = Hsv.FromString(vm.Text);
                            if (nullableHsv.HasValue) GlobalState.RefreshFromHsv(nullableHsv.Value);
                            break;
                        case nameof(HsvTabViewModel.ScaledText):
                            nullableHsv = Hsv.FromScaledString(vm.ScaledText);
                            if (nullableHsv.HasValue) GlobalState.RefreshFromHsv(nullableHsv.Value);
                            break;
                        case nameof(HsvTabViewModel.Hue):
                            isDouble = double.TryParse(vm.Hue, out doubleVal);
                            if (!isDouble || !Hsv.IsValidHue(doubleVal)) return;
                            GlobalState.RefreshFromHsv(GlobalState.Hsv.WithHue(doubleVal));
                            break;
                        case nameof(HsvTabViewModel.Saturation):
                            isDouble = double.TryParse(vm.Saturation, out doubleVal);
                            if (!isDouble || !Hsv.IsValidSaturation(doubleVal)) return;
                            GlobalState.RefreshFromHsv(GlobalState.Hsv.WithSaturation(doubleVal));
                            break;
                        case nameof(HsvTabViewModel.Value):
                            isDouble = double.TryParse(vm.Value, out doubleVal);
                            if (!isDouble || !Hsv.IsValidValue(doubleVal)) return;
                            GlobalState.RefreshFromHsv(GlobalState.Hsv.WithValue(doubleVal));
                            break;
                        case nameof(HsvTabViewModel.ScaledHue):
                            isDouble = double.TryParse(vm.ScaledHue, out doubleVal);
                            if (!isDouble || !Hsv.IsValidScaledHue(doubleVal)) return;
                            GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledHue(doubleVal));
                            break;
                        case nameof(HsvTabViewModel.ScaledSaturation):
                            isDouble = double.TryParse(vm.ScaledHue, out doubleVal);
                            if (!isDouble || !Hsv.IsValidScaledSaturation(doubleVal)) return;
                            GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledSaturation(doubleVal));
                            break;
                        case nameof(HsvTabViewModel.ScaledValue):
                            isDouble = double.TryParse(vm.ScaledHue, out doubleVal);
                            if (!isDouble || !Hsv.IsValidScaledValue(doubleVal)) return;
                            GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledValue(doubleVal));
                            break;
                    }
                };

                ScaledTextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.ScaledText); };
                TextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.Text); };
            };
        }
    }
}
