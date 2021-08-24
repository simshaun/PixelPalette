using System.Windows;
using System.Windows.Input;
using PixelPalette.Color;
using PixelPalette.State;
using PixelPalette.Util;
using Clipboard = PixelPalette.Util.Clipboard;

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
                        case nameof(RgbTabViewModel.Text):
                            nullableRgb = Rgb.FromString(vm.Text);
                            if (nullableRgb.HasValue) GlobalState.RefreshFromRgb(nullableRgb.Value);
                            break;
                        case nameof(RgbTabViewModel.ScaledText):
                            nullableRgb = Rgb.FromScaledString(vm.ScaledText);
                            if (nullableRgb.HasValue) GlobalState.RefreshFromRgb(nullableRgb.Value);
                            break;
                        case nameof(RgbTabViewModel.ScaledRedText):
                            isInt = int.TryParse(vm.ScaledRedText, out intVal);
                            if (!isInt || !Rgb.IsValidScaledComponent(intVal)) return;
                            GlobalState.RefreshFromRgb(GlobalState.Rgb.WithRed(intVal));
                            break;
                        case nameof(RgbTabViewModel.ScaledGreenText):
                            isInt = int.TryParse(vm.ScaledGreenText, out intVal);
                            if (!isInt || !Rgb.IsValidScaledComponent(intVal)) return;
                            GlobalState.RefreshFromRgb(GlobalState.Rgb.WithGreen(intVal));
                            break;
                        case nameof(RgbTabViewModel.ScaledBlueText):
                            isInt = int.TryParse(vm.ScaledBlueText, out intVal);
                            if (!isInt || !Rgb.IsValidScaledComponent(intVal)) return;
                            GlobalState.RefreshFromRgb(GlobalState.Rgb.WithBlue(intVal));
                            break;
                        case nameof(RgbTabViewModel.RedText):
                            isDouble = double.TryParse(vm.RedText, out doubleVal);
                            if (!isDouble || !Rgb.IsValidComponent(doubleVal)) return;
                            GlobalState.RefreshFromRgb(GlobalState.Rgb.WithRed(doubleVal));
                            break;
                        case nameof(RgbTabViewModel.GreenText):
                            isDouble = double.TryParse(vm.GreenText, out doubleVal);
                            if (!isDouble || !Rgb.IsValidComponent(doubleVal)) return;
                            GlobalState.RefreshFromRgb(GlobalState.Rgb.WithGreen(doubleVal));
                            break;
                        case nameof(RgbTabViewModel.BlueText):
                            isDouble = double.TryParse(vm.BlueText, out doubleVal);
                            if (!isDouble || !Rgb.IsValidComponent(doubleVal)) return;
                            GlobalState.RefreshFromRgb(GlobalState.Rgb.WithBlue(doubleVal));
                            break;
                    }
                };

                TextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.Text); };
                ScaledTextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.ScaledText); };
            };
        }
    }
}
