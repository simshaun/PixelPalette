using System.Windows;
using System.Windows.Input;
using PixelPalette.Color;

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

                EventUtil.HandleKey(Key.Up, HsvHue,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithHue(GlobalState.Hsv.Hue + 0.01)); });
                EventUtil.HandleKey(Key.Up, HsvSaturation,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithSaturation(GlobalState.Hsv.Saturation + 0.01)); });
                EventUtil.HandleKey(Key.Up, HsvValue,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithValue(GlobalState.Hsv.Value + 0.01)); });
                EventUtil.HandleKey(Key.Up, HsvScaledHue,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledHue(GlobalState.Hsv.ScaledHue + 1)); });
                EventUtil.HandleKey(Key.Up, HsvScaledSaturation,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledSaturation(GlobalState.Hsv.ScaledSaturation + 1)); });
                EventUtil.HandleKey(Key.Up, HsvScaledValue,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledValue(GlobalState.Hsv.ScaledValue + 1)); });

                EventUtil.HandleKey(Key.Down, HsvHue,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithHue(GlobalState.Hsv.Hue - 0.01)); });
                EventUtil.HandleKey(Key.Down, HsvSaturation,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithSaturation(GlobalState.Hsv.Saturation - 0.01)); });
                EventUtil.HandleKey(Key.Down, HsvValue,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithValue(GlobalState.Hsv.Value - 0.01)); });
                EventUtil.HandleKey(Key.Down, HsvScaledHue,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledHue(GlobalState.Hsv.ScaledHue - 1)); });
                EventUtil.HandleKey(Key.Down, HsvScaledSaturation,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledSaturation(GlobalState.Hsv.ScaledSaturation - 1)); });
                EventUtil.HandleKey(Key.Down, HsvScaledValue,
                                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledValue(GlobalState.Hsv.ScaledValue - 1)); });

                EventUtil.HandleMouseWheel(
                    HsvHue,
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithHue(GlobalState.Hsv.Hue + 0.01)); },
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithHue(GlobalState.Hsv.Hue - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    HsvSaturation,
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithSaturation(GlobalState.Hsv.Saturation + 0.01)); },
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithSaturation(GlobalState.Hsv.Saturation - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    HsvValue,
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithValue(GlobalState.Hsv.Value + 0.01)); },
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithValue(GlobalState.Hsv.Value - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    HsvScaledHue,
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledHue(GlobalState.Hsv.ScaledHue + 1)); },
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledHue(GlobalState.Hsv.ScaledHue - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    HsvScaledSaturation,
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledSaturation(GlobalState.Hsv.ScaledSaturation + 1)); },
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledSaturation(GlobalState.Hsv.ScaledSaturation - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    HsvScaledValue,
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledValue(GlobalState.Hsv.ScaledValue + 1)); },
                    () => { GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledValue(GlobalState.Hsv.ScaledValue - 1)); }
                );

                var hThrottler = new Throttler();
                var sThrottler = new Throttler();
                var vThrottler = new Throttler();

                EventUtil.HandleSliderChange(HsvHueSlider, value => { hThrottler.Throttle(1, _ => GlobalState.RefreshFromHsv(GlobalState.Hsv.WithHue(value))); });
                EventUtil.HandleSliderChange(HsvSaturationSlider, value => { sThrottler.Throttle(1, _ => GlobalState.RefreshFromHsv(GlobalState.Hsv.WithSaturation(value))); });
                EventUtil.HandleSliderChange(HsvValueSlider, value => { vThrottler.Throttle(1, _ => GlobalState.RefreshFromHsv(GlobalState.Hsv.WithValue(value))); });

                vm.PropertyChangedByUser += (_, ev) =>
                {
                    bool isDouble;
                    double doubleVal;
                    Hsv? nullableHsv;

                    switch (ev.PropertyName)
                    {
                        case nameof(HsvTabViewModel.HsvText):
                            nullableHsv = Hsv.FromString(vm.HsvText);
                            if (nullableHsv.HasValue) GlobalState.RefreshFromHsv(nullableHsv.Value);
                            break;
                        case nameof(HsvTabViewModel.HsvScaledText):
                            nullableHsv = Hsv.FromScaledString(vm.HsvScaledText);
                            if (nullableHsv.HasValue) GlobalState.RefreshFromHsv(nullableHsv.Value);
                            break;
                        case nameof(HsvTabViewModel.HsvHue):
                            isDouble = double.TryParse(vm.HsvHue, out doubleVal);
                            if (!isDouble || !Hsv.IsValidHue(doubleVal)) return;
                            GlobalState.RefreshFromHsv(GlobalState.Hsv.WithHue(doubleVal));
                            break;
                        case nameof(HsvTabViewModel.HsvSaturation):
                            isDouble = double.TryParse(vm.HsvSaturation, out doubleVal);
                            if (!isDouble || !Hsv.IsValidSaturation(doubleVal)) return;
                            GlobalState.RefreshFromHsv(GlobalState.Hsv.WithSaturation(doubleVal));
                            break;
                        case nameof(HsvTabViewModel.HsvValue):
                            isDouble = double.TryParse(vm.HsvValue, out doubleVal);
                            if (!isDouble || !Hsv.IsValidValue(doubleVal)) return;
                            GlobalState.RefreshFromHsv(GlobalState.Hsv.WithValue(doubleVal));
                            break;
                        case nameof(HsvTabViewModel.HsvScaledHue):
                            isDouble = double.TryParse(vm.HsvScaledHue, out doubleVal);
                            if (!isDouble || !Hsv.IsValidScaledHue(doubleVal)) return;
                            GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledHue(doubleVal));
                            break;
                        case nameof(HsvTabViewModel.HsvScaledSaturation):
                            isDouble = double.TryParse(vm.HsvScaledHue, out doubleVal);
                            if (!isDouble || !Hsv.IsValidScaledSaturation(doubleVal)) return;
                            GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledSaturation(doubleVal));
                            break;
                        case nameof(HsvTabViewModel.HsvScaledValue):
                            isDouble = double.TryParse(vm.HsvScaledHue, out doubleVal);
                            if (!isDouble || !Hsv.IsValidScaledValue(doubleVal)) return;
                            GlobalState.RefreshFromHsv(GlobalState.Hsv.WithScaledValue(doubleVal));
                            break;
                    }
                };

                HsvScaledTextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.HsvScaledText); };
                HsvTextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.HsvText); };
            };
        }
    }
}
