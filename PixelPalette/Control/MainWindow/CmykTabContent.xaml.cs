using System.Windows;
using System.Windows.Input;
using PixelPalette.Color;

namespace PixelPalette.Control.MainWindow
{
    public partial class CmykTabContent
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
                typeof(CmykTabContent),
                new FrameworkPropertyMetadata(GlobalState.Instance)
            );

        public CmykTabContent()
        {
            InitializeComponent();
            Loaded += (_, _) =>
            {
                if (GlobalState == null) return;

                var vm = new CmykTabViewModel(GlobalState);
                DataContext = vm;

                EventUtil.HandleKey(Key.Up, CmykCyan, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithCyan(GlobalState.Cmyk.Cyan + 0.01)); });
                EventUtil.HandleKey(Key.Up, CmykMagenta, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithMagenta(GlobalState.Cmyk.Magenta + 0.01)); });
                EventUtil.HandleKey(Key.Up, CmykYellow, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithYellow(GlobalState.Cmyk.Yellow + 0.01)); });
                EventUtil.HandleKey(Key.Up, CmykKey, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithKey(GlobalState.Cmyk.Key + 0.01)); });
                EventUtil.HandleKey(Key.Up, CmykScaledCyan, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledCyan(GlobalState.Cmyk.ScaledCyan + 1)); });
                EventUtil.HandleKey(Key.Up, CmykScaledMagenta, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledMagenta(GlobalState.Cmyk.ScaledMagenta + 1)); });
                EventUtil.HandleKey(Key.Up, CmykScaledYellow, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledYellow(GlobalState.Cmyk.ScaledYellow + 1)); });
                EventUtil.HandleKey(Key.Up, CmykScaledKey, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledKey(GlobalState.Cmyk.ScaledKey + 1)); });

                EventUtil.HandleKey(Key.Down, CmykCyan, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithCyan(GlobalState.Cmyk.Cyan - 0.01)); });
                EventUtil.HandleKey(Key.Down, CmykMagenta, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithMagenta(GlobalState.Cmyk.Magenta - 0.01)); });
                EventUtil.HandleKey(Key.Down, CmykYellow, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithYellow(GlobalState.Cmyk.Yellow - 0.01)); });
                EventUtil.HandleKey(Key.Down, CmykKey, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithKey(GlobalState.Cmyk.Key - 0.01)); });
                EventUtil.HandleKey(Key.Down, CmykScaledCyan, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledCyan(GlobalState.Cmyk.ScaledCyan - 1)); });
                EventUtil.HandleKey(Key.Down, CmykScaledMagenta, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledMagenta(GlobalState.Cmyk.ScaledMagenta - 1)); });
                EventUtil.HandleKey(Key.Down, CmykScaledYellow, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledYellow(GlobalState.Cmyk.ScaledYellow - 1)); });
                EventUtil.HandleKey(Key.Down, CmykScaledKey, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledKey(GlobalState.Cmyk.ScaledKey - 1)); });

                EventUtil.HandleMouseWheel(
                    CmykCyan,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithCyan(GlobalState.Cmyk.Cyan + 0.01)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithCyan(GlobalState.Cmyk.Cyan - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    CmykMagenta,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithMagenta(GlobalState.Cmyk.Magenta + 0.01)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithMagenta(GlobalState.Cmyk.Magenta - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    CmykYellow,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithYellow(GlobalState.Cmyk.Yellow + 0.01)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithYellow(GlobalState.Cmyk.Yellow - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    CmykKey,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithKey(GlobalState.Cmyk.Key + 0.01)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithKey(GlobalState.Cmyk.Key - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    CmykScaledCyan,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledCyan(GlobalState.Cmyk.ScaledCyan + 1)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledCyan(GlobalState.Cmyk.ScaledCyan - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    CmykScaledMagenta,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledMagenta(GlobalState.Cmyk.ScaledMagenta + 1)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledMagenta(GlobalState.Cmyk.ScaledMagenta - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    CmykScaledYellow,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledYellow(GlobalState.Cmyk.ScaledYellow + 1)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledYellow(GlobalState.Cmyk.ScaledYellow - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    CmykScaledKey,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledKey(GlobalState.Cmyk.ScaledKey + 1)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledKey(GlobalState.Cmyk.ScaledKey - 1)); }
                );

                var cThrottler = new Throttler();
                var mThrottler = new Throttler();
                var yThrottler = new Throttler();
                var kThrottler = new Throttler();

                EventUtil.HandleSliderChange(CmykCyanSlider, value => { cThrottler.Throttle(1, _ => GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithCyan(value))); });
                EventUtil.HandleSliderChange(CmykMagentaSlider, value => { mThrottler.Throttle(1, _ => GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithMagenta(value))); });
                EventUtil.HandleSliderChange(CmykYellowSlider, value => { yThrottler.Throttle(1, _ => GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithYellow(value))); });
                EventUtil.HandleSliderChange(CmykKeySlider, value => { kThrottler.Throttle(1, _ => GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithKey(value))); });

                vm.PropertyChangedByUser += (_, ev) =>
                {
                    bool isDouble;
                    double doubleVal;
                    Cmyk? nullableCmyk;

                    switch (ev.PropertyName)
                    {
                        case nameof(CmykTabViewModel.CmykText):
                            nullableCmyk = Cmyk.FromString(vm.CmykText);
                            if (nullableCmyk.HasValue) GlobalState.RefreshFromCmyk(nullableCmyk.Value);
                            break;
                        case nameof(CmykTabViewModel.CmykScaledText):
                            nullableCmyk = Cmyk.FromScaledString(vm.CmykScaledText);
                            if (nullableCmyk.HasValue) GlobalState.RefreshFromCmyk(nullableCmyk.Value);
                            break;
                        case nameof(CmykTabViewModel.CmykCyan):
                            isDouble = double.TryParse(vm.CmykCyan, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithCyan(doubleVal));
                            break;
                        case nameof(CmykTabViewModel.CmykMagenta):
                            isDouble = double.TryParse(vm.CmykMagenta, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithMagenta(doubleVal));
                            break;
                        case nameof(CmykTabViewModel.CmykYellow):
                            isDouble = double.TryParse(vm.CmykYellow, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithYellow(doubleVal));
                            break;
                        case nameof(CmykTabViewModel.CmykKey):
                            isDouble = double.TryParse(vm.CmykKey, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithKey(doubleVal));
                            break;
                        case nameof(CmykTabViewModel.CmykScaledCyan):
                            isDouble = double.TryParse(vm.CmykScaledCyan, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidScaledComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledCyan(doubleVal));
                            break;
                        case nameof(CmykTabViewModel.CmykScaledMagenta):
                            isDouble = double.TryParse(vm.CmykScaledMagenta, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidScaledComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledMagenta(doubleVal));
                            break;
                        case nameof(CmykTabViewModel.CmykScaledYellow):
                            isDouble = double.TryParse(vm.CmykScaledYellow, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidScaledComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledYellow(doubleVal));
                            break;
                        case nameof(CmykTabViewModel.CmykScaledKey):
                            isDouble = double.TryParse(vm.CmykScaledKey, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidScaledComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledKey(doubleVal));
                            break;
                    }
                };

                CmykScaledTextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.CmykScaledText); };
                CmykTextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.CmykText); };
            };
        }
    }
}
