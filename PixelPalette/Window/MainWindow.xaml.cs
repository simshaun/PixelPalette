using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using PixelPalette.Bitmap;
using PixelPalette.Color;

// Ignore warnings about not awaiting async tasks. Used by clipboard buttons.
#pragma warning disable 4014

namespace PixelPalette.Window
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private FreezeFrameWindow _freezeFrameWin;
        private CursorTrailWindow _cursorTrailWin;
        private readonly HistoryWindow _historyWin;
        private readonly MainWindowViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
            Style = (Style) FindResource(typeof(System.Windows.Window));
            _vm = new MainWindowViewModel();
            _vm.LoadFromPersistedData(PersistedState.Data, ColorModelTabs);
            DataContext = _vm;

            if (_vm.Rgb == Rgb.Empty)
            {
                // Material Blue – #2196F3
                _vm.RefreshFromRgb(Rgb.FromScaledValues(33, 150, 243));
            }

            // Eyedropper
            EyedropperButton.Click += (_, ev) => { StartEyedropper(); };

            // Lighter / Darker Buttons
            LighterButton.Click += (_, ev) => { _vm.RefreshFromHsl(_vm.Hsl.Lighter(5)); };
            DarkerButton.Click += (_, ev) => { _vm.RefreshFromHsl(_vm.Hsl.Darker(5)); };

            // History Button
            var historyVm = new HistoryWindowViewModel();
            _historyWin = new HistoryWindow(historyVm, Left + Width, Top);
            _historyWin.HistoryItemSelected += (_, args) => { _vm.RefreshFromRgb(args.HistoryItem.Color.ToRgb()); };
            HistoryButton.Click += (_, ev) =>
            {
                if (_historyWin.IsVisible)
                    _historyWin.Hide();
                else
                {
                    RepositionHistoryWin();
                    _historyWin.Show();
                }
            };

            // Color model tabs
            _vm.PropertyChanged += (_, ev) =>
            {
                if (ev.PropertyName == "ActiveColorModelTabItem")
                {
                    _vm.SaveToPersistedData(PersistedState.Data);
                }
            };

            // Helper function to reduce clipboard button boilerplate:
            void SetupClipboardButton(IInputElement button, Func<string> valueGetter)
            {
                EventUtil.HandleClick(button, (sender, e) =>
                {
                    // SetDataObject seems to work better than SetText or other methods, when programs like RealVNC are locking up the clipboard.
                    Clipboard.SetDataObject(valueGetter());
                    DisplayClipboardCheckmark((Button) sender);
                });
            }

            //
            // RGB fields
            //

            EventUtil.HandleKey(Key.Up, RgbRed, () => { _vm.RefreshFromRgb(_vm.Rgb.WithRed(_vm.Rgb.Red + 0.01)); });
            EventUtil.HandleKey(Key.Up, RgbGreen, () => { _vm.RefreshFromRgb(_vm.Rgb.WithGreen(_vm.Rgb.Green + 0.01)); });
            EventUtil.HandleKey(Key.Up, RgbBlue, () => { _vm.RefreshFromRgb(_vm.Rgb.WithBlue(_vm.Rgb.Blue + 0.01)); });
            EventUtil.HandleKey(Key.Up, RgbScaledRed, () => { _vm.RefreshFromRgb(_vm.Rgb.WithRed(_vm.Rgb.ScaledRed + 1)); });
            EventUtil.HandleKey(Key.Up, RgbScaledGreen, () => { _vm.RefreshFromRgb(_vm.Rgb.WithGreen(_vm.Rgb.ScaledGreen + 1)); });
            EventUtil.HandleKey(Key.Up, RgbScaledBlue, () => { _vm.RefreshFromRgb(_vm.Rgb.WithBlue(_vm.Rgb.ScaledBlue + 1)); });

            EventUtil.HandleKey(Key.Down, RgbRed, () => { _vm.RefreshFromRgb(_vm.Rgb.WithRed(_vm.Rgb.Red - 0.01)); });
            EventUtil.HandleKey(Key.Down, RgbGreen, () => { _vm.RefreshFromRgb(_vm.Rgb.WithGreen(_vm.Rgb.Green - 0.01)); });
            EventUtil.HandleKey(Key.Down, RgbBlue, () => { _vm.RefreshFromRgb(_vm.Rgb.WithBlue(_vm.Rgb.Blue - 0.01)); });
            EventUtil.HandleKey(Key.Down, RgbScaledRed, () => { _vm.RefreshFromRgb(_vm.Rgb.WithRed(_vm.Rgb.ScaledRed - 1)); });
            EventUtil.HandleKey(Key.Down, RgbScaledGreen, () => { _vm.RefreshFromRgb(_vm.Rgb.WithGreen(_vm.Rgb.ScaledGreen - 1)); });
            EventUtil.HandleKey(Key.Down, RgbScaledBlue, () => { _vm.RefreshFromRgb(_vm.Rgb.WithBlue(_vm.Rgb.ScaledBlue - 1)); });

            EventUtil.HandleMouseWheel(
                RgbRed,
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithRed(_vm.Rgb.Red + 0.01)); },
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithRed(_vm.Rgb.Red - 0.01)); }
            );
            EventUtil.HandleMouseWheel(
                RgbGreen,
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithGreen(_vm.Rgb.Green + 0.01)); },
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithGreen(_vm.Rgb.Green - 0.01)); }
            );
            EventUtil.HandleMouseWheel(
                RgbBlue,
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithBlue(_vm.Rgb.Blue + 0.01)); },
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithBlue(_vm.Rgb.Blue - 0.01)); }
            );

            EventUtil.HandleMouseWheel(
                RgbScaledRed,
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithRed(_vm.Rgb.ScaledRed + 1)); },
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithRed(_vm.Rgb.ScaledRed - 1)); }
            );
            EventUtil.HandleMouseWheel(
                RgbScaledGreen,
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithGreen(_vm.Rgb.ScaledGreen + 1)); },
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithGreen(_vm.Rgb.ScaledGreen - 1)); }
            );
            EventUtil.HandleMouseWheel(
                RgbScaledBlue,
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithBlue(_vm.Rgb.ScaledBlue + 1)); },
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithBlue(_vm.Rgb.ScaledBlue - 1)); }
            );

            EventUtil.HandleSliderChange(RgbRedSlider, value => { _vm.RefreshFromRgb(_vm.Rgb.WithRed(value)); });
            EventUtil.HandleSliderChange(RgbGreenSlider, value => { _vm.RefreshFromRgb(_vm.Rgb.WithGreen(value)); });
            EventUtil.HandleSliderChange(RgbBlueSlider, value => { _vm.RefreshFromRgb(_vm.Rgb.WithBlue(value)); });

            _vm.PropertyChangedByUser += (o, ev) =>
            {
                int intVal;
                bool isInt;
                bool isDouble;
                double doubleVal;
                Rgb? nullableRgb;

                switch (ev.PropertyName)
                {
                    case nameof(MainWindowViewModel.RgbText):
                        nullableRgb = Rgb.FromString(_vm.RgbText);
                        if (nullableRgb.HasValue) _vm.RefreshFromRgb(nullableRgb.Value);
                        break;
                    case nameof(MainWindowViewModel.RgbScaledText):
                        nullableRgb = Rgb.FromScaledString(_vm.RgbScaledText);
                        if (nullableRgb.HasValue) _vm.RefreshFromRgb(nullableRgb.Value);
                        break;
                    case nameof(MainWindowViewModel.RgbScaledRed):
                        isInt = int.TryParse(_vm.RgbScaledRed, out intVal);
                        if (!isInt || !Rgb.IsValidScaledComponent(intVal)) return;
                        _vm.RefreshFromRgb(_vm.Rgb.WithRed(intVal));
                        break;
                    case nameof(MainWindowViewModel.RgbScaledGreen):
                        isInt = int.TryParse(_vm.RgbScaledGreen, out intVal);
                        if (!isInt || !Rgb.IsValidScaledComponent(intVal)) return;
                        _vm.RefreshFromRgb(_vm.Rgb.WithGreen(intVal));
                        break;
                    case nameof(MainWindowViewModel.RgbScaledBlue):
                        isInt = int.TryParse(_vm.RgbScaledBlue, out intVal);
                        if (!isInt || !Rgb.IsValidScaledComponent(intVal)) return;
                        _vm.RefreshFromRgb(_vm.Rgb.WithBlue(intVal));
                        break;
                    case nameof(MainWindowViewModel.RgbRed):
                        isDouble = double.TryParse(_vm.RgbRed, out doubleVal);
                        if (!isDouble || !Rgb.IsValidComponent(doubleVal)) return;
                        _vm.RefreshFromRgb(_vm.Rgb.WithRed(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.RgbGreen):
                        isDouble = double.TryParse(_vm.RgbGreen, out doubleVal);
                        if (!isDouble || !Rgb.IsValidComponent(doubleVal)) return;
                        _vm.RefreshFromRgb(_vm.Rgb.WithGreen(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.RgbBlue):
                        isDouble = double.TryParse(_vm.RgbBlue, out doubleVal);
                        if (!isDouble || !Rgb.IsValidComponent(doubleVal)) return;
                        _vm.RefreshFromRgb(_vm.Rgb.WithBlue(doubleVal));
                        break;
                }
            };

            SetupClipboardButton(RgbScaledTextClipboardButton, () => _vm.RgbScaledText);
            SetupClipboardButton(RgbTextClipboardButton, () => _vm.RgbText);

            //
            // HEX fields
            //

            EventUtil.HandleKey(Key.Up, HexRed, () => { _vm.RefreshFromHex(_vm.Hex.WithRed(_vm.Hex.Red + 1)); });
            EventUtil.HandleKey(Key.Up, HexGreen, () => { _vm.RefreshFromHex(_vm.Hex.WithGreen(_vm.Hex.Green + 1)); });
            EventUtil.HandleKey(Key.Up, HexBlue, () => { _vm.RefreshFromHex(_vm.Hex.WithBlue(_vm.Hex.Blue + 1)); });

            EventUtil.HandleKey(Key.Down, HexRed, () => { _vm.RefreshFromHex(_vm.Hex.WithRed(_vm.Hex.Red - 1)); });
            EventUtil.HandleKey(Key.Down, HexGreen, () => { _vm.RefreshFromHex(_vm.Hex.WithGreen(_vm.Hex.Green - 1)); });
            EventUtil.HandleKey(Key.Down, HexBlue, () => { _vm.RefreshFromHex(_vm.Hex.WithBlue(_vm.Hex.Blue - 1)); });

            EventUtil.HandleMouseWheel(
                HexRed,
                () => { _vm.RefreshFromHex(_vm.Hex.WithRed(_vm.Hex.Red + 1)); },
                () => { _vm.RefreshFromHex(_vm.Hex.WithRed(_vm.Hex.Red - 1)); }
            );
            EventUtil.HandleMouseWheel(
                HexGreen,
                () => { _vm.RefreshFromHex(_vm.Hex.WithGreen(_vm.Hex.Green + 1)); },
                () => { _vm.RefreshFromHex(_vm.Hex.WithGreen(_vm.Hex.Green - 1)); }
            );
            EventUtil.HandleMouseWheel(
                HexBlue,
                () => { _vm.RefreshFromHex(_vm.Hex.WithBlue(_vm.Hex.Blue + 1)); },
                () => { _vm.RefreshFromHex(_vm.Hex.WithBlue(_vm.Hex.Blue - 1)); }
            );

            EventUtil.HandleSliderChange(HexRedSlider, value => { _vm.RefreshFromRgb(_vm.Rgb.WithRed(value)); });
            EventUtil.HandleSliderChange(HexGreenSlider, value => { _vm.RefreshFromRgb(_vm.Rgb.WithGreen(value)); });
            EventUtil.HandleSliderChange(HexBlueSlider, value => { _vm.RefreshFromRgb(_vm.Rgb.WithBlue(value)); });

            // The box actively ignores 3-char hex strings while typing so that 6-chars may be entered without interruption.
            // However, 3-char hex strings can still be entered by pressing Enter or dropping focus:
            EventUtil.HandleInputEnterOrFocusLost(HexText, value =>
            {
                var nullableHex = Hex.FromString(_vm.HexText);
                if (nullableHex.HasValue) _vm.RefreshFromHex(nullableHex.Value);
            });

            _vm.PropertyChangedByUser += (o, ev) =>
            {
                switch (ev.PropertyName)
                {
                    case nameof(MainWindowViewModel.HexText):
                        var nullableHex = Hex.From6CharString(_vm.HexText);
                        if (nullableHex.HasValue) _vm.RefreshFromHex(nullableHex.Value);
                        break;
                    case nameof(MainWindowViewModel.HexRed):
                        // Length comparison for better UX. Stops converting "0" to "00" and moving user cursor.
                        if (!Hex.IsValidHexPart(_vm.HexRed) || _vm.HexRed.Length != 2) return;
                        _vm.RefreshFromHex(_vm.Hex.WithRed(_vm.HexRed));
                        break;
                    case nameof(MainWindowViewModel.HexGreen):
                        // Length comparison for better UX. Stops converting "0" to "00" and moving user cursor.
                        if (!Hex.IsValidHexPart(_vm.HexGreen) || _vm.HexGreen.Length != 2) return;
                        _vm.RefreshFromHex(_vm.Hex.WithGreen(_vm.HexGreen));
                        break;
                    case nameof(MainWindowViewModel.HexBlue):
                        // Length comparison for better UX. Stops converting "0" to "00" and moving user cursor.
                        if (!Hex.IsValidHexPart(_vm.HexBlue) || _vm.HexBlue.Length != 2) return;
                        _vm.RefreshFromHex(_vm.Hex.WithBlue(_vm.HexBlue));
                        break;
                }
            };

            SetupClipboardButton(HexTextClipboardButton, () => _vm.HexText);

            //
            // HSL fields
            //

            EventUtil.HandleKey(Key.Up, HslHue, () => { _vm.RefreshFromHsl(_vm.Hsl.WithHue(_vm.Hsl.Hue + 0.01)); });
            EventUtil.HandleKey(Key.Up, HslSaturation, () => { _vm.RefreshFromHsl(_vm.Hsl.WithSaturation(_vm.Hsl.Saturation + 0.01)); });
            EventUtil.HandleKey(Key.Up, HslLuminance, () => { _vm.RefreshFromHsl(_vm.Hsl.WithLuminance(_vm.Hsl.Luminance + 0.01)); });
            EventUtil.HandleKey(Key.Up, HslScaledHue, () => { _vm.RefreshFromHsl(_vm.Hsl.WithScaledHue(_vm.Hsl.ScaledHue + 1)); });
            EventUtil.HandleKey(Key.Up, HslScaledSaturation, () => { _vm.RefreshFromHsl(_vm.Hsl.WithScaledSaturation(_vm.Hsl.ScaledSaturation + 1)); });
            EventUtil.HandleKey(Key.Up, HslScaledLuminance, () => { _vm.RefreshFromHsl(_vm.Hsl.WithScaledLuminance(_vm.Hsl.ScaledLuminance + 1)); });

            EventUtil.HandleKey(Key.Down, HslHue, () => { _vm.RefreshFromHsl(_vm.Hsl.WithHue(_vm.Hsl.Hue - 0.01)); });
            EventUtil.HandleKey(Key.Down, HslSaturation, () => { _vm.RefreshFromHsl(_vm.Hsl.WithSaturation(_vm.Hsl.Saturation - 0.01)); });
            EventUtil.HandleKey(Key.Down, HslLuminance, () => { _vm.RefreshFromHsl(_vm.Hsl.WithLuminance(_vm.Hsl.Luminance - 0.01)); });
            EventUtil.HandleKey(Key.Down, HslScaledHue, () => { _vm.RefreshFromHsl(_vm.Hsl.WithScaledHue(_vm.Hsl.ScaledHue - 1)); });
            EventUtil.HandleKey(Key.Down, HslScaledSaturation, () => { _vm.RefreshFromHsl(_vm.Hsl.WithScaledSaturation(_vm.Hsl.ScaledSaturation - 1)); });
            EventUtil.HandleKey(Key.Down, HslScaledLuminance, () => { _vm.RefreshFromHsl(_vm.Hsl.WithScaledLuminance(_vm.Hsl.ScaledLuminance - 1)); });

            EventUtil.HandleMouseWheel(
                HslHue,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithHue(_vm.Hsl.Hue + 0.01)); },
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithHue(_vm.Hsl.Hue - 0.01)); }
            );
            EventUtil.HandleMouseWheel(
                HslSaturation,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithSaturation(_vm.Hsl.Saturation + 0.01)); },
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithSaturation(_vm.Hsl.Saturation - 0.01)); }
            );
            EventUtil.HandleMouseWheel(
                HslLuminance,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithLuminance(_vm.Hsl.Luminance + 0.01)); },
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithLuminance(_vm.Hsl.Luminance - 0.01)); }
            );
            EventUtil.HandleMouseWheel(
                HslScaledHue,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithScaledHue(_vm.Hsl.ScaledHue + 1)); },
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithScaledHue(_vm.Hsl.ScaledHue - 1)); }
            );
            EventUtil.HandleMouseWheel(
                HslScaledSaturation,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithScaledSaturation(_vm.Hsl.ScaledSaturation + 1)); },
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithScaledSaturation(_vm.Hsl.ScaledSaturation - 1)); }
            );
            EventUtil.HandleMouseWheel(
                HslScaledLuminance,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithScaledLuminance(_vm.Hsl.ScaledLuminance + 1)); },
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithScaledLuminance(_vm.Hsl.ScaledLuminance - 1)); }
            );

            EventUtil.HandleSliderChange(HslHueSlider, value => { _vm.RefreshFromHsl(_vm.Hsl.WithHue(value)); });
            EventUtil.HandleSliderChange(HslSaturationSlider, value => { _vm.RefreshFromHsl(_vm.Hsl.WithSaturation(value)); });
            EventUtil.HandleSliderChange(HslLuminanceSlider, value => { _vm.RefreshFromHsl(_vm.Hsl.WithLuminance(value)); });

            _vm.PropertyChangedByUser += (o, ev) =>
            {
                bool isDouble;
                double doubleVal;
                Hsl? nullableHsl;

                switch (ev.PropertyName)
                {
                    case nameof(MainWindowViewModel.HslText):
                        nullableHsl = Hsl.FromString(_vm.HslText);
                        if (nullableHsl.HasValue) _vm.RefreshFromHsl(nullableHsl.Value);
                        break;
                    case nameof(MainWindowViewModel.HslScaledText):
                        nullableHsl = Hsl.FromScaledString(_vm.HslScaledText);
                        if (nullableHsl.HasValue) _vm.RefreshFromHsl(nullableHsl.Value);
                        break;
                    case nameof(MainWindowViewModel.HslHue):
                        isDouble = double.TryParse(_vm.HslHue, out doubleVal);
                        if (!isDouble || !Hsl.IsValidHue(doubleVal)) return;
                        _vm.RefreshFromHsl(_vm.Hsl.WithHue(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.HslSaturation):
                        isDouble = double.TryParse(_vm.HslSaturation, out doubleVal);
                        if (!isDouble || !Hsl.IsValidSaturation(doubleVal)) return;
                        _vm.RefreshFromHsl(_vm.Hsl.WithSaturation(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.HslLuminance):
                        isDouble = double.TryParse(_vm.HslLuminance, out doubleVal);
                        if (!isDouble || !Hsl.IsValidLuminance(doubleVal)) return;
                        _vm.RefreshFromHsl(_vm.Hsl.WithLuminance(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.HslScaledHue):
                        isDouble = double.TryParse(_vm.HslScaledHue, out doubleVal);
                        if (!isDouble || !Hsl.IsValidScaledHue(doubleVal)) return;
                        _vm.RefreshFromHsl(_vm.Hsl.WithScaledHue(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.HslScaledSaturation):
                        isDouble = double.TryParse(_vm.HslScaledHue, out doubleVal);
                        if (!isDouble || !Hsl.IsValidScaledSaturation(doubleVal)) return;
                        _vm.RefreshFromHsl(_vm.Hsl.WithScaledSaturation(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.HslScaledLuminance):
                        isDouble = double.TryParse(_vm.HslScaledHue, out doubleVal);
                        if (!isDouble || !Hsl.IsValidScaledLuminance(doubleVal)) return;
                        _vm.RefreshFromHsl(_vm.Hsl.WithScaledLuminance(doubleVal));
                        break;
                }
            };

            SetupClipboardButton(HslScaledTextClipboardButton, () => _vm.HslScaledText);
            SetupClipboardButton(HslTextClipboardButton, () => _vm.HslText);

            //
            // HSV fields
            //

            EventUtil.HandleKey(Key.Up, HsvHue, () => { _vm.RefreshFromHsv(_vm.Hsv.WithHue(_vm.Hsv.Hue + 0.01)); });
            EventUtil.HandleKey(Key.Up, HsvSaturation, () => { _vm.RefreshFromHsv(_vm.Hsv.WithSaturation(_vm.Hsv.Saturation + 0.01)); });
            EventUtil.HandleKey(Key.Up, HsvValue, () => { _vm.RefreshFromHsv(_vm.Hsv.WithValue(_vm.Hsv.Value + 0.01)); });
            EventUtil.HandleKey(Key.Up, HsvScaledHue, () => { _vm.RefreshFromHsv(_vm.Hsv.WithScaledHue(_vm.Hsv.ScaledHue + 1)); });
            EventUtil.HandleKey(Key.Up, HsvScaledSaturation, () => { _vm.RefreshFromHsv(_vm.Hsv.WithScaledSaturation(_vm.Hsv.ScaledSaturation + 1)); });
            EventUtil.HandleKey(Key.Up, HsvScaledValue, () => { _vm.RefreshFromHsv(_vm.Hsv.WithScaledValue(_vm.Hsv.ScaledValue + 1)); });

            EventUtil.HandleKey(Key.Down, HsvHue, () => { _vm.RefreshFromHsv(_vm.Hsv.WithHue(_vm.Hsv.Hue - 0.01)); });
            EventUtil.HandleKey(Key.Down, HsvSaturation, () => { _vm.RefreshFromHsv(_vm.Hsv.WithSaturation(_vm.Hsv.Saturation - 0.01)); });
            EventUtil.HandleKey(Key.Down, HsvValue, () => { _vm.RefreshFromHsv(_vm.Hsv.WithValue(_vm.Hsv.Value - 0.01)); });
            EventUtil.HandleKey(Key.Down, HsvScaledHue, () => { _vm.RefreshFromHsv(_vm.Hsv.WithScaledHue(_vm.Hsv.ScaledHue - 1)); });
            EventUtil.HandleKey(Key.Down, HsvScaledSaturation, () => { _vm.RefreshFromHsv(_vm.Hsv.WithScaledSaturation(_vm.Hsv.ScaledSaturation - 1)); });
            EventUtil.HandleKey(Key.Down, HsvScaledValue, () => { _vm.RefreshFromHsv(_vm.Hsv.WithScaledValue(_vm.Hsv.ScaledValue - 1)); });

            EventUtil.HandleMouseWheel(HsvHue, () => { _vm.RefreshFromHsv(_vm.Hsv.WithHue(_vm.Hsv.Hue + 0.01)); },
                                       () => { _vm.RefreshFromHsv(_vm.Hsv.WithHue(_vm.Hsv.Hue - 0.01)); });
            EventUtil.HandleMouseWheel(HsvSaturation, () => { _vm.RefreshFromHsv(_vm.Hsv.WithSaturation(_vm.Hsv.Saturation + 0.01)); },
                                       () => { _vm.RefreshFromHsv(_vm.Hsv.WithSaturation(_vm.Hsv.Saturation - 0.01)); });
            EventUtil.HandleMouseWheel(HsvValue, () => { _vm.RefreshFromHsv(_vm.Hsv.WithValue(_vm.Hsv.Value + 0.01)); },
                                       () => { _vm.RefreshFromHsv(_vm.Hsv.WithValue(_vm.Hsv.Value - 0.01)); });
            EventUtil.HandleMouseWheel(HsvScaledHue, () => { _vm.RefreshFromHsv(_vm.Hsv.WithScaledHue(_vm.Hsv.ScaledHue + 1)); },
                                       () => { _vm.RefreshFromHsv(_vm.Hsv.WithScaledHue(_vm.Hsv.ScaledHue - 1)); });
            EventUtil.HandleMouseWheel(HsvScaledSaturation, () => { _vm.RefreshFromHsv(_vm.Hsv.WithScaledSaturation(_vm.Hsv.ScaledSaturation + 1)); },
                                       () => { _vm.RefreshFromHsv(_vm.Hsv.WithScaledSaturation(_vm.Hsv.ScaledSaturation - 1)); });
            EventUtil.HandleMouseWheel(HsvScaledValue, () => { _vm.RefreshFromHsv(_vm.Hsv.WithScaledValue(_vm.Hsv.ScaledValue + 1)); },
                                       () => { _vm.RefreshFromHsv(_vm.Hsv.WithScaledValue(_vm.Hsv.ScaledValue - 1)); });

            EventUtil.HandleSliderChange(HsvHueSlider, value => { _vm.RefreshFromHsv(_vm.Hsv.WithHue(value)); });
            EventUtil.HandleSliderChange(HsvSaturationSlider, value => { _vm.RefreshFromHsv(_vm.Hsv.WithSaturation(value)); });
            EventUtil.HandleSliderChange(HsvValueSlider, value => { _vm.RefreshFromHsv(_vm.Hsv.WithValue(value)); });

            _vm.PropertyChangedByUser += (o, ev) =>
            {
                bool isDouble;
                double doubleVal;
                Hsv? nullableHsv;

                switch (ev.PropertyName)
                {
                    case nameof(MainWindowViewModel.HsvText):
                        nullableHsv = Hsv.FromString(_vm.HsvText);
                        if (nullableHsv.HasValue) _vm.RefreshFromHsv(nullableHsv.Value);
                        break;
                    case nameof(MainWindowViewModel.HsvScaledText):
                        nullableHsv = Hsv.FromScaledString(_vm.HsvScaledText);
                        if (nullableHsv.HasValue) _vm.RefreshFromHsv(nullableHsv.Value);
                        break;
                    case nameof(MainWindowViewModel.HsvHue):
                        isDouble = double.TryParse(_vm.HsvHue, out doubleVal);
                        if (!isDouble || !Hsv.IsValidHue(doubleVal)) return;
                        _vm.RefreshFromHsv(_vm.Hsv.WithHue(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.HsvSaturation):
                        isDouble = double.TryParse(_vm.HsvSaturation, out doubleVal);
                        if (!isDouble || !Hsv.IsValidSaturation(doubleVal)) return;
                        _vm.RefreshFromHsv(_vm.Hsv.WithSaturation(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.HsvValue):
                        isDouble = double.TryParse(_vm.HsvValue, out doubleVal);
                        if (!isDouble || !Hsv.IsValidValue(doubleVal)) return;
                        _vm.RefreshFromHsv(_vm.Hsv.WithValue(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.HsvScaledHue):
                        isDouble = double.TryParse(_vm.HsvScaledHue, out doubleVal);
                        if (!isDouble || !Hsv.IsValidScaledHue(doubleVal)) return;
                        _vm.RefreshFromHsv(_vm.Hsv.WithScaledHue(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.HsvScaledSaturation):
                        isDouble = double.TryParse(_vm.HsvScaledHue, out doubleVal);
                        if (!isDouble || !Hsv.IsValidScaledSaturation(doubleVal)) return;
                        _vm.RefreshFromHsv(_vm.Hsv.WithScaledSaturation(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.HsvScaledValue):
                        isDouble = double.TryParse(_vm.HsvScaledHue, out doubleVal);
                        if (!isDouble || !Hsv.IsValidScaledValue(doubleVal)) return;
                        _vm.RefreshFromHsv(_vm.Hsv.WithScaledValue(doubleVal));
                        break;
                }
            };

            SetupClipboardButton(HsvScaledTextClipboardButton, () => _vm.HsvScaledText);
            SetupClipboardButton(HsvTextClipboardButton, () => _vm.HsvText);

            //
            // CMYK fields
            //

            EventUtil.HandleKey(Key.Up, CmykCyan, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithCyan(_vm.Cmyk.Cyan + 0.01)); });
            EventUtil.HandleKey(Key.Up, CmykMagenta, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithMagenta(_vm.Cmyk.Magenta + 0.01)); });
            EventUtil.HandleKey(Key.Up, CmykYellow, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithYellow(_vm.Cmyk.Yellow + 0.01)); });
            EventUtil.HandleKey(Key.Up, CmykKey, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithKey(_vm.Cmyk.Key + 0.01)); });
            EventUtil.HandleKey(Key.Up, CmykScaledCyan, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledCyan(_vm.Cmyk.ScaledCyan + 1)); });
            EventUtil.HandleKey(Key.Up, CmykScaledMagenta, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledMagenta(_vm.Cmyk.ScaledMagenta + 1)); });
            EventUtil.HandleKey(Key.Up, CmykScaledYellow, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledYellow(_vm.Cmyk.ScaledYellow + 1)); });
            EventUtil.HandleKey(Key.Up, CmykScaledKey, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledKey(_vm.Cmyk.ScaledKey + 1)); });

            EventUtil.HandleKey(Key.Down, CmykCyan, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithCyan(_vm.Cmyk.Cyan - 0.01)); });
            EventUtil.HandleKey(Key.Down, CmykMagenta, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithMagenta(_vm.Cmyk.Magenta - 0.01)); });
            EventUtil.HandleKey(Key.Down, CmykYellow, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithYellow(_vm.Cmyk.Yellow - 0.01)); });
            EventUtil.HandleKey(Key.Down, CmykKey, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithKey(_vm.Cmyk.Key - 0.01)); });
            EventUtil.HandleKey(Key.Down, CmykScaledCyan, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledCyan(_vm.Cmyk.ScaledCyan - 1)); });
            EventUtil.HandleKey(Key.Down, CmykScaledMagenta, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledMagenta(_vm.Cmyk.ScaledMagenta - 1)); });
            EventUtil.HandleKey(Key.Down, CmykScaledYellow, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledYellow(_vm.Cmyk.ScaledYellow - 1)); });
            EventUtil.HandleKey(Key.Down, CmykScaledKey, () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledKey(_vm.Cmyk.ScaledKey - 1)); });

            EventUtil.HandleMouseWheel(CmykCyan,
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithCyan(_vm.Cmyk.Cyan + 0.01)); },
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithCyan(_vm.Cmyk.Cyan - 0.01)); });
            EventUtil.HandleMouseWheel(CmykMagenta,
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithMagenta(_vm.Cmyk.Magenta + 0.01)); },
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithMagenta(_vm.Cmyk.Magenta - 0.01)); });
            EventUtil.HandleMouseWheel(CmykYellow,
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithYellow(_vm.Cmyk.Yellow + 0.01)); },
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithYellow(_vm.Cmyk.Yellow - 0.01)); });
            EventUtil.HandleMouseWheel(CmykKey,
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithKey(_vm.Cmyk.Key + 0.01)); },
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithKey(_vm.Cmyk.Key - 0.01)); });
            EventUtil.HandleMouseWheel(CmykScaledCyan,
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledCyan(_vm.Cmyk.ScaledCyan + 1)); },
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledCyan(_vm.Cmyk.ScaledCyan - 1)); });
            EventUtil.HandleMouseWheel(CmykScaledMagenta,
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledMagenta(_vm.Cmyk.ScaledMagenta + 1)); },
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledMagenta(_vm.Cmyk.ScaledMagenta - 1)); });
            EventUtil.HandleMouseWheel(CmykScaledYellow,
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledYellow(_vm.Cmyk.ScaledYellow + 1)); },
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledYellow(_vm.Cmyk.ScaledYellow - 1)); });
            EventUtil.HandleMouseWheel(CmykScaledKey,
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledKey(_vm.Cmyk.ScaledKey + 1)); },
                                       () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledKey(_vm.Cmyk.ScaledKey - 1)); });

            EventUtil.HandleSliderChange(CmykCyanSlider, value => { _vm.RefreshFromCmyk(_vm.Cmyk.WithCyan(value)); });
            EventUtil.HandleSliderChange(CmykMagentaSlider, value => { _vm.RefreshFromCmyk(_vm.Cmyk.WithMagenta(value)); });
            EventUtil.HandleSliderChange(CmykYellowSlider, value => { _vm.RefreshFromCmyk(_vm.Cmyk.WithYellow(value)); });
            EventUtil.HandleSliderChange(CmykKeySlider, value => { _vm.RefreshFromCmyk(_vm.Cmyk.WithKey(value)); });

            _vm.PropertyChangedByUser += (o, ev) =>
            {
                bool isDouble;
                double doubleVal;
                Cmyk? nullableCmyk;

                switch (ev.PropertyName)
                {
                    case nameof(MainWindowViewModel.CmykText):
                        nullableCmyk = Cmyk.FromString(_vm.CmykText);
                        if (nullableCmyk.HasValue) _vm.RefreshFromCmyk(nullableCmyk.Value);
                        break;
                    case nameof(MainWindowViewModel.CmykScaledText):
                        nullableCmyk = Cmyk.FromScaledString(_vm.CmykScaledText);
                        if (nullableCmyk.HasValue) _vm.RefreshFromCmyk(nullableCmyk.Value);
                        break;
                    case nameof(MainWindowViewModel.CmykCyan):
                        isDouble = double.TryParse(_vm.CmykCyan, out doubleVal);
                        if (!isDouble || !Cmyk.IsValidComponent(doubleVal)) return;
                        _vm.RefreshFromCmyk(_vm.Cmyk.WithCyan(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.CmykMagenta):
                        isDouble = double.TryParse(_vm.CmykMagenta, out doubleVal);
                        if (!isDouble || !Cmyk.IsValidComponent(doubleVal)) return;
                        _vm.RefreshFromCmyk(_vm.Cmyk.WithMagenta(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.CmykYellow):
                        isDouble = double.TryParse(_vm.CmykYellow, out doubleVal);
                        if (!isDouble || !Cmyk.IsValidComponent(doubleVal)) return;
                        _vm.RefreshFromCmyk(_vm.Cmyk.WithYellow(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.CmykKey):
                        isDouble = double.TryParse(_vm.CmykKey, out doubleVal);
                        if (!isDouble || !Cmyk.IsValidComponent(doubleVal)) return;
                        _vm.RefreshFromCmyk(_vm.Cmyk.WithKey(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.CmykScaledCyan):
                        isDouble = double.TryParse(_vm.CmykScaledCyan, out doubleVal);
                        if (!isDouble || !Cmyk.IsValidScaledComponent(doubleVal)) return;
                        _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledCyan(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.CmykScaledMagenta):
                        isDouble = double.TryParse(_vm.CmykScaledMagenta, out doubleVal);
                        if (!isDouble || !Cmyk.IsValidScaledComponent(doubleVal)) return;
                        _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledMagenta(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.CmykScaledYellow):
                        isDouble = double.TryParse(_vm.CmykScaledYellow, out doubleVal);
                        if (!isDouble || !Cmyk.IsValidScaledComponent(doubleVal)) return;
                        _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledYellow(doubleVal));
                        break;
                    case nameof(MainWindowViewModel.CmykScaledKey):
                        isDouble = double.TryParse(_vm.CmykScaledKey, out doubleVal);
                        if (!isDouble || !Cmyk.IsValidScaledComponent(doubleVal)) return;
                        _vm.RefreshFromCmyk(_vm.Cmyk.WithScaledKey(doubleVal));
                        break;
                }
            };

            SetupClipboardButton(CmykScaledTextClipboardButton, () => _vm.CmykScaledText);
            SetupClipboardButton(CmykTextClipboardButton, () => _vm.CmykText);

            //
            // LAB fields
            //

            EventUtil.HandleKey(Key.Up, LabL, () => { _vm.RefreshFromLab(_vm.Lab.WithL(_vm.Lab.L + 1)); });
            EventUtil.HandleKey(Key.Up, LabA, () => { _vm.RefreshFromLab(_vm.Lab.WithA(_vm.Lab.A + 1)); });
            EventUtil.HandleKey(Key.Up, LabB, () => { _vm.RefreshFromLab(_vm.Lab.WithB(_vm.Lab.B + 1)); });

            EventUtil.HandleKey(Key.Down, LabL, () => { _vm.RefreshFromLab(_vm.Lab.WithL(_vm.Lab.L - 1)); });
            EventUtil.HandleKey(Key.Down, LabA, () => { _vm.RefreshFromLab(_vm.Lab.WithA(_vm.Lab.A - 1)); });
            EventUtil.HandleKey(Key.Down, LabB, () => { _vm.RefreshFromLab(_vm.Lab.WithB(_vm.Lab.B - 1)); });

            EventUtil.HandleMouseWheel(LabL,
                                       () => { _vm.RefreshFromLab(_vm.Lab.WithL(_vm.Lab.L + 1)); },
                                       () => { _vm.RefreshFromLab(_vm.Lab.WithL(_vm.Lab.L - 1)); });
            EventUtil.HandleMouseWheel(LabA,
                                       () => { _vm.RefreshFromLab(_vm.Lab.WithA(_vm.Lab.A + 1)); },
                                       () => { _vm.RefreshFromLab(_vm.Lab.WithA(_vm.Lab.A - 1)); });
            EventUtil.HandleMouseWheel(LabB,
                                       () => { _vm.RefreshFromLab(_vm.Lab.WithB(_vm.Lab.B + 1)); },
                                       () => { _vm.RefreshFromLab(_vm.Lab.WithB(_vm.Lab.B - 1)); });

            EventUtil.HandleSliderChange(LabLSlider, value => { _vm.RefreshFromLab(_vm.Lab.WithL(value)); });
            EventUtil.HandleSliderChange(LabASlider, value => { _vm.RefreshFromLab(_vm.Lab.WithA(value)); });
            EventUtil.HandleSliderChange(LabBSlider, value => { _vm.RefreshFromLab(_vm.Lab.WithB(value)); });

            _vm.PropertyChangedByUser += (o, ev) =>
            {
                switch (ev.PropertyName)
                {
                    case nameof(MainWindowViewModel.LabText):
                        var nullableLab = Lab.FromString(_vm.LabText);
                        if (nullableLab.HasValue) _vm.RefreshFromLab(nullableLab.Value);
                        break;
                    case nameof(MainWindowViewModel.LabL):
                        if (!Lab.IsValidL(_vm.LabL)) return;
                        _vm.RefreshFromLab(_vm.Lab.WithL(_vm.LabL));
                        break;
                    case nameof(MainWindowViewModel.LabA):
                        if (!Lab.IsValidA(_vm.LabA)) return;
                        _vm.RefreshFromLab(_vm.Lab.WithA(_vm.LabA));
                        break;
                    case nameof(MainWindowViewModel.LabB):
                        if (!Lab.IsValidB(_vm.LabB)) return;
                        _vm.RefreshFromLab(_vm.Lab.WithB(_vm.LabB));
                        break;
                }
            };

            SetupClipboardButton(LabTextClipboardButton, () => _vm.LabText);

            //
            // ----
            //

            // Hack to bring window to front on start. (I had instances where I would open the tool
            // only for it to come up behind my current window.)
            if (!IsVisible) Show();
            WindowState = WindowState.Normal;
            Activate();
            Topmost = true;
            Topmost = false;
            Focus();
        }

        private void StartEyedropper()
        {
            var freezeFrame = ScreenCapture.GetFullScreen();
            FreezeFrame.Instance.BitmapSource = freezeFrame;

            // Debug - Write freeze frame to file
            // using (var fileStream = new FileStream("cap.png", FileMode.Create))
            // {
            //     BitmapEncoder encoder = new PngBitmapEncoder();
            //     encoder.Frames.Add(BitmapFrame.Create(freezeFrame));
            //     encoder.Save(fileStream);
            // }

            _freezeFrameWin = new FreezeFrameWindow(_vm);
            _freezeFrameWin.ColorPicked += (sender, args) =>
            {
                var el = WpfHelper.FindFirstVisualChild<TextBox>(_vm.ActiveColorModelTabItem);
                el?.Focus();
                el?.SelectAll();
            };
            _freezeFrameWin.Show();
            _freezeFrameWin.Focus();
            _freezeFrameWin.Closed += (o2, e2) =>
            {
                try
                {
                    _cursorTrailWin.Close();
                }
                catch (InvalidOperationException)
                {
                    // Already closed.
                }
                finally
                {
                    GC.Collect();
                }
            };

            _cursorTrailWin = new CursorTrailWindow();
            _cursorTrailWin.Show();
            _cursorTrailWin.Focus();
            _cursorTrailWin.Closed += (o2, e2) =>
            {
                try
                {
                    _freezeFrameWin.Close();
                }
                catch (InvalidOperationException)
                {
                    // Already closed.
                }
                finally
                {
                    GC.Collect();
                }
            };
        }

        private void OnColorModelTabSelected(object sender, RoutedEventArgs e)
        {
            PersistedState.Data.ActiveColorModelTab = ((TabItem) e.Source).Name;
        }

        private async Task DisplayClipboardCheckmark(DependencyObject button)
        {
            var image = WpfHelper.FindFirstVisualChild<Image>(button);
            if (image == null) return;
            var oldSource = image.Source;
            image.Source = (BitmapImage) Resources["CheckmarkImage"];
            await Task.Delay(500);
            image.Source = oldSource;
        }

        //
        // -------------
        //

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (e.Key)
            {
                case Key.Escape:
                    _freezeFrameWin?.Close();
                    Close();
                    break;
                case Key.F3:
                    StartEyedropper();
                    break;
            }
        }

        // This allows inputs to lose focus when the user clicks outside of them:
        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainGrid.Focus();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            PersistedState.Save();
        }

#region Disable Maximize window

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            var hWnd = new WindowInteropHelper((System.Windows.Window) sender).Handle;
            WindowHelper.DisableMaximize(hWnd);
        }

#endregion

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!EphemeralState.Data.DebugMode) return;
            var debugWin = new DebugWindow(Left + Width, Top);
            debugWin.Show();
        }

        private void MainWindow_OnLocationChanged(object sender, EventArgs e)
        {
            RepositionHistoryWin();
        }

        private void RepositionHistoryWin()
        {
            _historyWin.Left = Left + Width;
            _historyWin.Top = Top;
        }
    }
}
