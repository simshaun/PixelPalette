using System.Windows;
using System.Windows.Input;
using PixelPalette.Color;

namespace PixelPalette.Control.MainWindow
{
    public partial class RgbTabContent
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
                typeof(RgbTabContent),
                new FrameworkPropertyMetadata(GlobalState.Instance)
            );

        public RgbTabContent()
        {
            InitializeComponent();
            Loaded += (_, _) =>
            {
                if (GlobalState == null) return;

                var vm = new RgbTabViewModel(GlobalState);
                DataContext = vm;

                EventUtil.HandleKey(Key.Up, RgbRed, () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithRed(GlobalState.Rgb.Red + 0.01)); });
                EventUtil.HandleKey(Key.Up, RgbGreen, () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithGreen(GlobalState.Rgb.Green + 0.01)); });
                EventUtil.HandleKey(Key.Up, RgbBlue, () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithBlue(GlobalState.Rgb.Blue + 0.01)); });
                EventUtil.HandleKey(Key.Up, RgbScaledRed, () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithRed(GlobalState.Rgb.ScaledRed + 1)); });
                EventUtil.HandleKey(Key.Up, RgbScaledGreen, () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithGreen(GlobalState.Rgb.ScaledGreen + 1)); });
                EventUtil.HandleKey(Key.Up, RgbScaledBlue, () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithBlue(GlobalState.Rgb.ScaledBlue + 1)); });

                EventUtil.HandleKey(Key.Down, RgbRed, () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithRed(GlobalState.Rgb.Red - 0.01)); });
                EventUtil.HandleKey(Key.Down, RgbGreen, () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithGreen(GlobalState.Rgb.Green - 0.01)); });
                EventUtil.HandleKey(Key.Down, RgbBlue, () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithBlue(GlobalState.Rgb.Blue - 0.01)); });
                EventUtil.HandleKey(Key.Down, RgbScaledRed, () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithRed(GlobalState.Rgb.ScaledRed - 1)); });
                EventUtil.HandleKey(Key.Down, RgbScaledGreen,
                                    () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithGreen(GlobalState.Rgb.ScaledGreen - 1)); });
                EventUtil.HandleKey(Key.Down, RgbScaledBlue, () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithBlue(GlobalState.Rgb.ScaledBlue - 1)); });

                EventUtil.HandleMouseWheel(
                    RgbRed,
                    () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithRed(GlobalState.Rgb.Red + 0.01)); },
                    () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithRed(GlobalState.Rgb.Red - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    RgbGreen,
                    () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithGreen(GlobalState.Rgb.Green + 0.01)); },
                    () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithGreen(GlobalState.Rgb.Green - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    RgbBlue,
                    () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithBlue(GlobalState.Rgb.Blue + 0.01)); },
                    () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithBlue(GlobalState.Rgb.Blue - 0.01)); }
                );

                EventUtil.HandleMouseWheel(
                    RgbScaledRed,
                    () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithRed(GlobalState.Rgb.ScaledRed + 1)); },
                    () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithRed(GlobalState.Rgb.ScaledRed - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    RgbScaledGreen,
                    () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithGreen(GlobalState.Rgb.ScaledGreen + 1)); },
                    () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithGreen(GlobalState.Rgb.ScaledGreen - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    RgbScaledBlue,
                    () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithBlue(GlobalState.Rgb.ScaledBlue + 1)); },
                    () => { GlobalState.RefreshFromRgb(GlobalState.Rgb.WithBlue(GlobalState.Rgb.ScaledBlue - 1)); }
                );

                var redThrottler = new Throttler();
                var greenThrottler = new Throttler();
                var blueThrottler = new Throttler();

                EventUtil.HandleSliderChange(RgbRedSlider, value => { redThrottler.Throttle(1, _ => GlobalState.RefreshFromRgb(GlobalState.Rgb.WithRed(value))); });
                EventUtil.HandleSliderChange(RgbGreenSlider, value => { greenThrottler.Throttle(1, _ => GlobalState.RefreshFromRgb(GlobalState.Rgb.WithGreen(value))); });
                EventUtil.HandleSliderChange(RgbBlueSlider, value => { blueThrottler.Throttle(1, _ => GlobalState.RefreshFromRgb(GlobalState.Rgb.WithBlue(value))); });

                vm.PropertyChangedByUser += (_, ev) =>
                {
                    int intVal;
                    bool isInt;
                    bool isDouble;
                    double doubleVal;
                    Rgb? nullableRgb;

                    switch (ev.PropertyName)
                    {
                        case nameof(RgbTabViewModel.RgbText):
                            nullableRgb = Rgb.FromString(vm.RgbText);
                            if (nullableRgb.HasValue) GlobalState.RefreshFromRgb(nullableRgb.Value);
                            break;
                        case nameof(RgbTabViewModel.RgbScaledText):
                            nullableRgb = Rgb.FromScaledString(vm.RgbScaledText);
                            if (nullableRgb.HasValue) GlobalState.RefreshFromRgb(nullableRgb.Value);
                            break;
                        case nameof(RgbTabViewModel.RgbScaledRedText):
                            isInt = int.TryParse(vm.RgbScaledRedText, out intVal);
                            if (!isInt || !Rgb.IsValidScaledComponent(intVal)) return;
                            GlobalState.RefreshFromRgb(GlobalState.Rgb.WithRed(intVal));
                            break;
                        case nameof(RgbTabViewModel.RgbScaledGreenText):
                            isInt = int.TryParse(vm.RgbScaledGreenText, out intVal);
                            if (!isInt || !Rgb.IsValidScaledComponent(intVal)) return;
                            GlobalState.RefreshFromRgb(GlobalState.Rgb.WithGreen(intVal));
                            break;
                        case nameof(RgbTabViewModel.RgbScaledBlueText):
                            isInt = int.TryParse(vm.RgbScaledBlueText, out intVal);
                            if (!isInt || !Rgb.IsValidScaledComponent(intVal)) return;
                            GlobalState.RefreshFromRgb(GlobalState.Rgb.WithBlue(intVal));
                            break;
                        case nameof(RgbTabViewModel.RgbRedText):
                            isDouble = double.TryParse(vm.RgbRedText, out doubleVal);
                            if (!isDouble || !Rgb.IsValidComponent(doubleVal)) return;
                            GlobalState.RefreshFromRgb(GlobalState.Rgb.WithRed(doubleVal));
                            break;
                        case nameof(RgbTabViewModel.RgbGreenText):
                            isDouble = double.TryParse(vm.RgbGreenText, out doubleVal);
                            if (!isDouble || !Rgb.IsValidComponent(doubleVal)) return;
                            GlobalState.RefreshFromRgb(GlobalState.Rgb.WithGreen(doubleVal));
                            break;
                        case nameof(RgbTabViewModel.RgbBlueText):
                            isDouble = double.TryParse(vm.RgbBlueText, out doubleVal);
                            if (!isDouble || !Rgb.IsValidComponent(doubleVal)) return;
                            GlobalState.RefreshFromRgb(GlobalState.Rgb.WithBlue(doubleVal));
                            break;
                    }
                };

                RgbTextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.RgbText); };
                RgbScaledTextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.RgbScaledText); };
            };
        }
    }
}
