using System.Windows;
using System.Windows.Input;
using PixelPalette.Color;
using PixelPalette.State;
using PixelPalette.Util;
using Clipboard = PixelPalette.Util.Clipboard;

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

                EventUtil.HandleKey(Key.Up, CyanText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithCyan(GlobalState.Cmyk.Cyan + 0.01)); });
                EventUtil.HandleKey(Key.Up, MagentaText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithMagenta(GlobalState.Cmyk.Magenta + 0.01)); });
                EventUtil.HandleKey(Key.Up, YellowText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithYellow(GlobalState.Cmyk.Yellow + 0.01)); });
                EventUtil.HandleKey(Key.Up, KeyText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithKey(GlobalState.Cmyk.Key + 0.01)); });
                EventUtil.HandleKey(Key.Up, ScaledCyanText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledCyan(GlobalState.Cmyk.ScaledCyan + 1)); });
                EventUtil.HandleKey(Key.Up, ScaledMagentaText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledMagenta(GlobalState.Cmyk.ScaledMagenta + 1)); });
                EventUtil.HandleKey(Key.Up, ScaledYellowText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledYellow(GlobalState.Cmyk.ScaledYellow + 1)); });
                EventUtil.HandleKey(Key.Up, ScaledKeyText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledKey(GlobalState.Cmyk.ScaledKey + 1)); });

                EventUtil.HandleKey(Key.Down, CyanText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithCyan(GlobalState.Cmyk.Cyan - 0.01)); });
                EventUtil.HandleKey(Key.Down, MagentaText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithMagenta(GlobalState.Cmyk.Magenta - 0.01)); });
                EventUtil.HandleKey(Key.Down, YellowText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithYellow(GlobalState.Cmyk.Yellow - 0.01)); });
                EventUtil.HandleKey(Key.Down, KeyText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithKey(GlobalState.Cmyk.Key - 0.01)); });
                EventUtil.HandleKey(Key.Down, ScaledCyanText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledCyan(GlobalState.Cmyk.ScaledCyan - 1)); });
                EventUtil.HandleKey(Key.Down, ScaledMagentaText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledMagenta(GlobalState.Cmyk.ScaledMagenta - 1)); });
                EventUtil.HandleKey(Key.Down, ScaledYellowText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledYellow(GlobalState.Cmyk.ScaledYellow - 1)); });
                EventUtil.HandleKey(Key.Down, ScaledKeyText, () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledKey(GlobalState.Cmyk.ScaledKey - 1)); });

                EventUtil.HandleMouseWheel(
                    CyanText,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithCyan(GlobalState.Cmyk.Cyan + 0.01)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithCyan(GlobalState.Cmyk.Cyan - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    MagentaText,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithMagenta(GlobalState.Cmyk.Magenta + 0.01)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithMagenta(GlobalState.Cmyk.Magenta - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    YellowText,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithYellow(GlobalState.Cmyk.Yellow + 0.01)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithYellow(GlobalState.Cmyk.Yellow - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    KeyText,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithKey(GlobalState.Cmyk.Key + 0.01)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithKey(GlobalState.Cmyk.Key - 0.01)); }
                );
                EventUtil.HandleMouseWheel(
                    ScaledCyanText,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledCyan(GlobalState.Cmyk.ScaledCyan + 1)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledCyan(GlobalState.Cmyk.ScaledCyan - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    ScaledMagentaText,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledMagenta(GlobalState.Cmyk.ScaledMagenta + 1)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledMagenta(GlobalState.Cmyk.ScaledMagenta - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    ScaledYellowText,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledYellow(GlobalState.Cmyk.ScaledYellow + 1)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledYellow(GlobalState.Cmyk.ScaledYellow - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    ScaledKeyText,
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledKey(GlobalState.Cmyk.ScaledKey + 1)); },
                    () => { GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledKey(GlobalState.Cmyk.ScaledKey - 1)); }
                );

                var cThrottler = new Throttler();
                var mThrottler = new Throttler();
                var yThrottler = new Throttler();
                var kThrottler = new Throttler();

                EventUtil.HandleSliderChange(CyanSlider, value => { cThrottler.Throttle(1, _ => GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithCyan(value))); });
                EventUtil.HandleSliderChange(MagentaSlider, value => { mThrottler.Throttle(1, _ => GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithMagenta(value))); });
                EventUtil.HandleSliderChange(YellowSlider, value => { yThrottler.Throttle(1, _ => GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithYellow(value))); });
                EventUtil.HandleSliderChange(KeySlider, value => { kThrottler.Throttle(1, _ => GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithKey(value))); });

                vm.PropertyChangedByUser += (_, ev) =>
                {
                    bool isDouble;
                    double doubleVal;
                    Cmyk? nullableCmyk;

                    switch (ev.PropertyName)
                    {
                        case nameof(CmykTabViewModel.Text):
                            nullableCmyk = Cmyk.FromString(vm.Text);
                            if (nullableCmyk.HasValue) GlobalState.RefreshFromCmyk(nullableCmyk.Value);
                            break;
                        case nameof(CmykTabViewModel.ScaledText):
                            nullableCmyk = Cmyk.FromScaledString(vm.ScaledText);
                            if (nullableCmyk.HasValue) GlobalState.RefreshFromCmyk(nullableCmyk.Value);
                            break;
                        case nameof(CmykTabViewModel.Cyan):
                            isDouble = double.TryParse(vm.Cyan, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithCyan(doubleVal));
                            break;
                        case nameof(CmykTabViewModel.Magenta):
                            isDouble = double.TryParse(vm.Magenta, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithMagenta(doubleVal));
                            break;
                        case nameof(CmykTabViewModel.Yellow):
                            isDouble = double.TryParse(vm.Yellow, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithYellow(doubleVal));
                            break;
                        case nameof(CmykTabViewModel.Key):
                            isDouble = double.TryParse(vm.Key, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithKey(doubleVal));
                            break;
                        case nameof(CmykTabViewModel.ScaledCyan):
                            isDouble = double.TryParse(vm.ScaledCyan, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidScaledComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledCyan(doubleVal));
                            break;
                        case nameof(CmykTabViewModel.ScaledMagenta):
                            isDouble = double.TryParse(vm.ScaledMagenta, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidScaledComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledMagenta(doubleVal));
                            break;
                        case nameof(CmykTabViewModel.ScaledYellow):
                            isDouble = double.TryParse(vm.ScaledYellow, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidScaledComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledYellow(doubleVal));
                            break;
                        case nameof(CmykTabViewModel.ScaledKey):
                            isDouble = double.TryParse(vm.ScaledKey, out doubleVal);
                            if (!isDouble || !Cmyk.IsValidScaledComponent(doubleVal)) return;
                            GlobalState.RefreshFromCmyk(GlobalState.Cmyk.WithScaledKey(doubleVal));
                            break;
                    }
                };

                ScaledTextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.ScaledText); };
                TextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.Text); };
            };
        }
    }
}
