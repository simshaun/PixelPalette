using System.Windows;
using System.Windows.Input;
using PixelPalette.Color;

namespace PixelPalette.Control.MainWindow
{
    public partial class LabTabContent
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
                typeof(LabTabContent),
                new FrameworkPropertyMetadata(GlobalState.Instance)
            );

        public LabTabContent()
        {
            InitializeComponent();
            Loaded += (_, _) =>
            {
                if (GlobalState == null) return;

                var vm = new LabTabViewModel(GlobalState);
                DataContext = vm;

                EventUtil.HandleKey(Key.Up, LabL, () => { GlobalState.RefreshFromLab(GlobalState.Lab.WithL(GlobalState.Lab.L + 1)); });
                EventUtil.HandleKey(Key.Up, LabA, () => { GlobalState.RefreshFromLab(GlobalState.Lab.WithA(GlobalState.Lab.A + 1)); });
                EventUtil.HandleKey(Key.Up, LabB, () => { GlobalState.RefreshFromLab(GlobalState.Lab.WithB(GlobalState.Lab.B + 1)); });

                EventUtil.HandleKey(Key.Down, LabL, () => { GlobalState.RefreshFromLab(GlobalState.Lab.WithL(GlobalState.Lab.L - 1)); });
                EventUtil.HandleKey(Key.Down, LabA, () => { GlobalState.RefreshFromLab(GlobalState.Lab.WithA(GlobalState.Lab.A - 1)); });
                EventUtil.HandleKey(Key.Down, LabB, () => { GlobalState.RefreshFromLab(GlobalState.Lab.WithB(GlobalState.Lab.B - 1)); });

                EventUtil.HandleMouseWheel(LabL,
                                           () => { GlobalState.RefreshFromLab(GlobalState.Lab.WithL(GlobalState.Lab.L + 1)); },
                                           () => { GlobalState.RefreshFromLab(GlobalState.Lab.WithL(GlobalState.Lab.L - 1)); });
                EventUtil.HandleMouseWheel(LabA,
                                           () => { GlobalState.RefreshFromLab(GlobalState.Lab.WithA(GlobalState.Lab.A + 1)); },
                                           () => { GlobalState.RefreshFromLab(GlobalState.Lab.WithA(GlobalState.Lab.A - 1)); });
                EventUtil.HandleMouseWheel(LabB,
                                           () => { GlobalState.RefreshFromLab(GlobalState.Lab.WithB(GlobalState.Lab.B + 1)); },
                                           () => { GlobalState.RefreshFromLab(GlobalState.Lab.WithB(GlobalState.Lab.B - 1)); });

                var lThrottler = new Throttler();
                var aThrottler = new Throttler();
                var bThrottler = new Throttler();

                EventUtil.HandleSliderChange(LabLSlider, value => { lThrottler.Throttle(1, _ => GlobalState.RefreshFromLab(GlobalState.Lab.WithL(value))); });
                EventUtil.HandleSliderChange(LabASlider, value => { aThrottler.Throttle(1, _ => GlobalState.RefreshFromLab(GlobalState.Lab.WithA(value))); });
                EventUtil.HandleSliderChange(LabBSlider, value => { bThrottler.Throttle(1, _ => GlobalState.RefreshFromLab(GlobalState.Lab.WithB(value))); });

                vm.PropertyChangedByUser += (_, ev) =>
                {
                    switch (ev.PropertyName)
                    {
                        case nameof(LabTabViewModel.LabText):
                            var nullableLab = Lab.FromString(vm.LabText);
                            if (nullableLab.HasValue) GlobalState.RefreshFromLab(nullableLab.Value);
                            break;
                        case nameof(LabTabViewModel.LabL):
                            if (!Lab.IsValidL(vm.LabL)) return;
                            GlobalState.RefreshFromLab(GlobalState.Lab.WithL(vm.LabL));
                            break;
                        case nameof(LabTabViewModel.LabA):
                            if (!Lab.IsValidA(vm.LabA)) return;
                            GlobalState.RefreshFromLab(GlobalState.Lab.WithA(vm.LabA));
                            break;
                        case nameof(LabTabViewModel.LabB):
                            if (!Lab.IsValidB(vm.LabB)) return;
                            GlobalState.RefreshFromLab(GlobalState.Lab.WithB(vm.LabB));
                            break;
                    }
                };

                LabTextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.LabText); };
            };
        }
    }
}
