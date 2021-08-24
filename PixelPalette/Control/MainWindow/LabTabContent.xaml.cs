using System.Windows;
using System.Windows.Input;
using PixelPalette.Color;
using PixelPalette.State;
using PixelPalette.Util;
using Clipboard = PixelPalette.Util.Clipboard;

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

                EventUtil.HandleSliderChange(LSlider, value => { lThrottler.Throttle(1, _ => GlobalState.RefreshFromLab(GlobalState.Lab.WithL(value))); });
                EventUtil.HandleSliderChange(ASlider, value => { aThrottler.Throttle(1, _ => GlobalState.RefreshFromLab(GlobalState.Lab.WithA(value))); });
                EventUtil.HandleSliderChange(BSlider, value => { bThrottler.Throttle(1, _ => GlobalState.RefreshFromLab(GlobalState.Lab.WithB(value))); });

                vm.PropertyChangedByUser += (_, ev) =>
                {
                    switch (ev.PropertyName)
                    {
                        case nameof(LabTabViewModel.Text):
                            var nullableLab = Lab.FromString(vm.Text);
                            if (nullableLab.HasValue) GlobalState.RefreshFromLab(nullableLab.Value);
                            break;
                        case nameof(LabTabViewModel.L):
                            if (!Lab.IsValidL(vm.L)) return;
                            GlobalState.RefreshFromLab(GlobalState.Lab.WithL(vm.L));
                            break;
                        case nameof(LabTabViewModel.A):
                            if (!Lab.IsValidA(vm.A)) return;
                            GlobalState.RefreshFromLab(GlobalState.Lab.WithA(vm.A));
                            break;
                        case nameof(LabTabViewModel.B):
                            if (!Lab.IsValidB(vm.B)) return;
                            GlobalState.RefreshFromLab(GlobalState.Lab.WithB(vm.B));
                            break;
                    }
                };

                TextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.Text); };
            };
        }
    }
}
