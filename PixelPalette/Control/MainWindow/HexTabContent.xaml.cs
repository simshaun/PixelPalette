using System.Windows;
using System.Windows.Input;
using PixelPalette.Color;

namespace PixelPalette.Control.MainWindow
{
    public partial class HexTabContent
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
                typeof(HexTabContent),
                new FrameworkPropertyMetadata(GlobalState.Instance)
            );

        public HexTabContent()
        {
            InitializeComponent();
            Loaded += (_, _) =>
            {
                if (GlobalState == null) return;

                var vm = new HexTabViewModel(GlobalState);
                DataContext = vm;

                EventUtil.HandleKey(Key.Up, RedText, () => { GlobalState.RefreshFromHex(GlobalState.Hex.WithRed(GlobalState.Hex.Red + 1)); });
                EventUtil.HandleKey(Key.Up, GreenText, () => { GlobalState.RefreshFromHex(GlobalState.Hex.WithGreen(GlobalState.Hex.Green + 1)); });
                EventUtil.HandleKey(Key.Up, BlueText, () => { GlobalState.RefreshFromHex(GlobalState.Hex.WithBlue(GlobalState.Hex.Blue + 1)); });

                EventUtil.HandleKey(Key.Down, RedText, () => { GlobalState.RefreshFromHex(GlobalState.Hex.WithRed(GlobalState.Hex.Red - 1)); });
                EventUtil.HandleKey(Key.Down, GreenText, () => { GlobalState.RefreshFromHex(GlobalState.Hex.WithGreen(GlobalState.Hex.Green - 1)); });
                EventUtil.HandleKey(Key.Down, BlueText, () => { GlobalState.RefreshFromHex(GlobalState.Hex.WithBlue(GlobalState.Hex.Blue - 1)); });

                EventUtil.HandleMouseWheel(
                    RedText,
                    () => { GlobalState.RefreshFromHex(GlobalState.Hex.WithRed(GlobalState.Hex.Red + 1)); },
                    () => { GlobalState.RefreshFromHex(GlobalState.Hex.WithRed(GlobalState.Hex.Red - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    GreenText,
                    () => { GlobalState.RefreshFromHex(GlobalState.Hex.WithGreen(GlobalState.Hex.Green + 1)); },
                    () => { GlobalState.RefreshFromHex(GlobalState.Hex.WithGreen(GlobalState.Hex.Green - 1)); }
                );
                EventUtil.HandleMouseWheel(
                    BlueText,
                    () => { GlobalState.RefreshFromHex(GlobalState.Hex.WithBlue(GlobalState.Hex.Blue + 1)); },
                    () => { GlobalState.RefreshFromHex(GlobalState.Hex.WithBlue(GlobalState.Hex.Blue - 1)); }
                );

                var rThrottler = new Throttler();
                var gThrottler = new Throttler();
                var bThrottler = new Throttler();

                EventUtil.HandleSliderChange(RedSlider, value => { rThrottler.Throttle(1, _ => GlobalState.RefreshFromRgb(GlobalState.Rgb.WithRed(value))); });
                EventUtil.HandleSliderChange(GreenSlider, value => { gThrottler.Throttle(1, _ => GlobalState.RefreshFromRgb(GlobalState.Rgb.WithGreen(value))); });
                EventUtil.HandleSliderChange(BlueSlider, value => { bThrottler.Throttle(1, _ => GlobalState.RefreshFromRgb(GlobalState.Rgb.WithBlue(value))); });

                // The box actively ignores 3-char hex strings while typing so that 6-chars may be entered without interruption.
                // However, 3-char hex strings can still be entered by pressing Enter or dropping focus:
                EventUtil.HandleInputEnterOrFocusLost(Text, _ =>
                {
                    var nullableHex = Hex.FromString(vm.Text);
                    if (nullableHex.HasValue) GlobalState.RefreshFromHex(nullableHex.Value);
                });

                vm.PropertyChangedByUser += (_, ev) =>
                {
                    switch (ev.PropertyName)
                    {
                        case nameof(HexTabViewModel.Text):
                            var nullableHex = Hex.From6CharString(vm.Text);
                            if (nullableHex.HasValue) GlobalState.RefreshFromHex(nullableHex.Value);
                            break;
                        case nameof(HexTabViewModel.Red):
                            // Length comparison for better UX. Stops converting "0" to "00" and moving user cursor.
                            if (!Hex.IsValidHexPart(vm.Red) || vm.Red.Length != 2) return;
                            GlobalState.RefreshFromHex(GlobalState.Hex.WithRed(vm.Red));
                            break;
                        case nameof(HexTabViewModel.Green):
                            // Length comparison for better UX. Stops converting "0" to "00" and moving user cursor.
                            if (!Hex.IsValidHexPart(vm.Green) || vm.Green.Length != 2) return;
                            GlobalState.RefreshFromHex(GlobalState.Hex.WithGreen(vm.Green));
                            break;
                        case nameof(HexTabViewModel.Blue):
                            // Length comparison for better UX. Stops converting "0" to "00" and moving user cursor.
                            if (!Hex.IsValidHexPart(vm.Blue) || vm.Blue.Length != 2) return;
                            GlobalState.RefreshFromHex(GlobalState.Hex.WithBlue(vm.Blue));
                            break;
                    }
                };

                TextClipboardButton.ButtonClicked += (_, _) => { Clipboard.Set(vm.Text); };
            };
        }
    }
}
