using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using PixelPalette.Annotations;
using PixelPalette.Bitmap;
using PixelPalette.Color;

namespace PixelPalette.Window
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private FreezeFrameWindow _freezeFrameWin;
        private CursorTrailWindow _cursorTrailWin;
        private readonly MainWindowViewModel _vm;

        // Shortcut method
        private static void HandleKey(Key? key, IEnumerable<IInputElement> controls, Action action)
        {
            foreach (var control in controls)
            {
                control.PreviewKeyDown += (o, ev) =>
                {
                    if (key != null && ev.Key != key) return;
                    action();
                };
            }
        }

        // Shortcut method
        private static void HandleKey(Key? key, IInputElement control, Action action)
        {
            HandleKey(key, new[] {control}, action);
        }

        // Shortcut method
        private static void HandleMouseWheel(
            IEnumerable<IInputElement> controls,
            [CanBeNull] Action upAction,
            [CanBeNull] Action downAction
        )
        {
            foreach (var control in controls)
            {
                control.MouseWheel += (o, ev) =>
                {
                    if (ev.Delta > 0)
                    {
                        upAction?.Invoke();
                    }
                    else
                    {
                        downAction?.Invoke();
                    }
                };
            }
        }

        // Shortcut method
        private static void HandleMouseWheel(
            IInputElement control,
            [CanBeNull] Action upAction,
            [CanBeNull] Action downAction
        )
        {
            HandleMouseWheel(new[] {control}, upAction, downAction);
        }

        // Shortcut method
        private static void HandleInput(TextBox control, Action<string, TextBox> action)
        {
            control.TextChanged += (o, ev) =>
            {
                if (!IsActiveControl(control)) return;
                var text = control.Text;
                action(text, control);
            };
        }

        // Shortcut method
        private static void HandleInputEnterOrFocusLost(TextBox control, Action<string, TextBox> action)
        {
            void HandleIt()
            {
                var text = control.Text;
                action(text, control);
            }

            control.KeyDown += (o, ev) =>
            {
                if (ev.Key != Key.Enter) return;
                HandleIt();
            };

            control.LostFocus += (o, ev) => { HandleIt(); };
        }

        // Shortcut method
        private static void HandleSliderChange(Slider control, Action<double, Slider> action)
        {
            control.ValueChanged += (o, ev) =>
            {
                if (!IsActiveControl(control)) return;
                action(ev.NewValue, control);
            };
        }

        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainWindowViewModel();
            _vm.LoadFromPersistedData(PersistedState.Data, ColorModelTabs);
            DataContext = _vm;

            if (_vm.Rgb == Rgb.Empty)
            {
                // Material Blue – #2196F3
                _vm.RefreshFromRgb(new Rgb(33, 150, 243));
            }

            // Eyedropper
            EyedropperButton.Click += (o, ev) => { StartEyedropper(); };

            // Lighter / Darker Buttons
            LighterButton.Click += (o, ev) => { _vm.RefreshFromHsl(_vm.Hsl.Lighter(5)); };
            DarkerButton.Click += (o, ev) => { _vm.RefreshFromHsl(_vm.Hsl.Darker(5)); };

            // Color model tabs
            _vm.PropertyChanged += (o, ev) =>
            {
                if (ev.PropertyName == "ActiveColorModelTabItem")
                {
                    _vm.SaveToPersistedData(PersistedState.Data);
                }
            };

            // Hex & RGB Arrow Keys
            HandleKey(Key.Up, new[] {RedHex, Red},
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithRed(_vm.Rgb.Red + 1)); });
            HandleKey(Key.Up, new[] {GreenHex, Green},
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithGreen(_vm.Rgb.Green + 1)); });
            HandleKey(Key.Up, new[] {BlueHex, Blue},
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithBlue(_vm.Rgb.Blue + 1)); });

            HandleKey(Key.Down, new[] {RedHex, Red},
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithRed(_vm.Rgb.Red - 1)); });
            HandleKey(Key.Down, new[] {GreenHex, Green},
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithGreen(_vm.Rgb.Green - 1)); });
            HandleKey(Key.Down, new[] {BlueHex, Blue},
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithBlue(_vm.Rgb.Blue - 1)); });

            // HSL Arrow Keys
            HandleKey(Key.Up, HslHue,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithHue(_vm.Hsl.Hue + 1)); });
            HandleKey(Key.Up, HslSaturation,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithSaturation(_vm.Hsl.Saturation + 1)); });
            HandleKey(Key.Up, HslLuminance,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithLuminance(_vm.Hsl.Luminance + 1)); });

            HandleKey(Key.Down, HslHue,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithHue(_vm.Hsl.Hue - 1)); });
            HandleKey(Key.Down, HslSaturation,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithSaturation(_vm.Hsl.Saturation - 1)); });
            HandleKey(Key.Down, HslLuminance,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithLuminance(_vm.Hsl.Luminance - 1)); });

            // HSV Arrow Keys
            HandleKey(Key.Up, HsvHue,
                () => { _vm.RefreshFromHsv(_vm.Hsv.WithHue(_vm.Hsv.Hue + 1)); });
            HandleKey(Key.Up, HsvSaturation,
                () => { _vm.RefreshFromHsv(_vm.Hsv.WithSaturation(_vm.Hsv.Saturation + 1)); });
            HandleKey(Key.Up, HsvValue,
                () => { _vm.RefreshFromHsv(_vm.Hsv.WithValue(_vm.Hsv.Value + 1)); });

            HandleKey(Key.Down, HsvHue,
                () => { _vm.RefreshFromHsv(_vm.Hsv.WithHue(_vm.Hsv.Hue - 1)); });
            HandleKey(Key.Down, HsvSaturation,
                () => { _vm.RefreshFromHsv(_vm.Hsv.WithSaturation(_vm.Hsv.Saturation - 1)); });
            HandleKey(Key.Down, HsvValue,
                () => { _vm.RefreshFromHsv(_vm.Hsv.WithValue(_vm.Hsv.Value - 1)); });

            // CMYK Arrow Keys
            HandleKey(Key.Up, CmykCyan,
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithCyan(_vm.Cmyk.Cyan + 1)); });
            HandleKey(Key.Up, CmykMagenta,
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithMagenta(_vm.Cmyk.Magenta + 1)); });
            HandleKey(Key.Up, CmykYellow,
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithYellow(_vm.Cmyk.Yellow + 1)); });
            HandleKey(Key.Up, CmykKey,
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithKey(_vm.Cmyk.Key + 1)); });

            HandleKey(Key.Down, CmykCyan,
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithCyan(_vm.Cmyk.Cyan - 1)); });
            HandleKey(Key.Down, CmykMagenta,
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithMagenta(_vm.Cmyk.Magenta - 1)); });
            HandleKey(Key.Down, CmykYellow,
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithYellow(_vm.Cmyk.Yellow - 1)); });
            HandleKey(Key.Down, CmykKey,
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithKey(_vm.Cmyk.Key - 1)); });

            // LAB Arrow Keys
            HandleKey(Key.Up, LabL,
                () => { _vm.RefreshFromLab(_vm.Lab.WithL(_vm.Lab.L + 1)); });
            HandleKey(Key.Up, LabA,
                () => { _vm.RefreshFromLab(_vm.Lab.WithA(_vm.Lab.A + 1)); });
            HandleKey(Key.Up, LabB,
                () => { _vm.RefreshFromLab(_vm.Lab.WithB(_vm.Lab.B + 1)); });

            HandleKey(Key.Down, LabL,
                () => { _vm.RefreshFromLab(_vm.Lab.WithL(_vm.Lab.L - 1)); });
            HandleKey(Key.Down, LabA,
                () => { _vm.RefreshFromLab(_vm.Lab.WithA(_vm.Lab.A - 1)); });
            HandleKey(Key.Down, LabB,
                () => { _vm.RefreshFromLab(_vm.Lab.WithB(_vm.Lab.B - 1)); });

            // Hex & RGB Mousewheel
            HandleMouseWheel(new[] {RedHex, Red},
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithRed(_vm.Rgb.Red + 1)); },
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithRed(_vm.Rgb.Red - 1)); });
            HandleMouseWheel(new[] {GreenHex, Green},
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithGreen(_vm.Rgb.Green + 1)); },
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithGreen(_vm.Rgb.Green - 1)); });
            HandleMouseWheel(new[] {BlueHex, Blue},
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithBlue(_vm.Rgb.Blue + 1)); },
                () => { _vm.RefreshFromRgb(_vm.Rgb.WithBlue(_vm.Rgb.Blue - 1)); });

            // HSL Mousewheel
            HandleMouseWheel(HslHue,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithHue(_vm.Hsl.Hue + 1)); },
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithHue(_vm.Hsl.Hue - 1)); });
            HandleMouseWheel(HslSaturation,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithSaturation(_vm.Hsl.Saturation + 1)); },
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithSaturation(_vm.Hsl.Saturation - 1)); });
            HandleMouseWheel(HslLuminance,
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithLuminance(_vm.Hsl.Luminance + 1)); },
                () => { _vm.RefreshFromHsl(_vm.Hsl.WithLuminance(_vm.Hsl.Luminance - 1)); });

            // HSV Mousewheel
            HandleMouseWheel(HsvHue,
                () => { _vm.RefreshFromHsv(_vm.Hsv.WithHue(_vm.Hsv.Hue + 1)); },
                () => { _vm.RefreshFromHsv(_vm.Hsv.WithHue(_vm.Hsv.Hue - 1)); });
            HandleMouseWheel(HsvSaturation,
                () => { _vm.RefreshFromHsv(_vm.Hsv.WithSaturation(_vm.Hsv.Saturation + 1)); },
                () => { _vm.RefreshFromHsv(_vm.Hsv.WithSaturation(_vm.Hsv.Saturation - 1)); });
            HandleMouseWheel(HsvValue,
                () => { _vm.RefreshFromHsv(_vm.Hsv.WithValue(_vm.Hsv.Value + 1)); },
                () => { _vm.RefreshFromHsv(_vm.Hsv.WithValue(_vm.Hsv.Value - 1)); });

            // CMYK Mousewheel
            HandleMouseWheel(CmykCyan,
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithCyan(_vm.Cmyk.Cyan + 1)); },
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithCyan(_vm.Cmyk.Cyan - 1)); });
            HandleMouseWheel(CmykMagenta,
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithMagenta(_vm.Cmyk.Magenta + 1)); },
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithMagenta(_vm.Cmyk.Magenta - 1)); });
            HandleMouseWheel(CmykYellow,
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithYellow(_vm.Cmyk.Yellow + 1)); },
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithYellow(_vm.Cmyk.Yellow - 1)); });
            HandleMouseWheel(CmykKey,
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithKey(_vm.Cmyk.Key + 1)); },
                () => { _vm.RefreshFromCmyk(_vm.Cmyk.WithKey(_vm.Cmyk.Key - 1)); });

            // LAB Mousewheel
            HandleMouseWheel(LabL,
                () => { _vm.RefreshFromLab(_vm.Lab.WithL(_vm.Lab.L + 1)); },
                () => { _vm.RefreshFromLab(_vm.Lab.WithL(_vm.Lab.L - 1)); });
            HandleMouseWheel(LabA,
                () => { _vm.RefreshFromLab(_vm.Lab.WithA(_vm.Lab.A + 1)); },
                () => { _vm.RefreshFromLab(_vm.Lab.WithA(_vm.Lab.A - 1)); });
            HandleMouseWheel(LabB,
                () => { _vm.RefreshFromLab(_vm.Lab.WithB(_vm.Lab.B + 1)); },
                () => { _vm.RefreshFromLab(_vm.Lab.WithB(_vm.Lab.B - 1)); });

            // HEX manual input
            HandleInputEnterOrFocusLost(HexText,
                (text, control) =>
                {
                    var nullable = Hex.FromString(text);
                    if (nullable.HasValue) _vm.RefreshFromHex(nullable.Value);
                });
            HandleInputEnterOrFocusLost(RedHex,
                (text, control) => { _vm.RefreshFromHex(_vm.Hex.WithRed(Hex.ClampedComponent(text))); });
            HandleInputEnterOrFocusLost(GreenHex,
                (text, control) => { _vm.RefreshFromHex(_vm.Hex.WithGreen(Hex.ClampedComponent(text))); });
            HandleInputEnterOrFocusLost(BlueHex,
                (text, control) => { _vm.RefreshFromHex(_vm.Hex.WithBlue(Hex.ClampedComponent(text))); });

            // RGB manual input
            HandleInputEnterOrFocusLost(RgbText,
                (text, control) =>
                {
                    var nullable = Rgb.FromString(text);
                    if (nullable.HasValue) _vm.RefreshFromRgb(nullable.Value);
                });
            HandleInput(Red, (text, control) =>
            {
                var isInt = int.TryParse(text, out var value);
                if (!isInt) return;
                var clamped = Rgb.ClampedComponent(value);
                _vm.RefreshFromRgb(_vm.Rgb.WithRed(clamped));
                control.Text = clamped.ToString();
            });
            HandleInput(Green, (text, control) =>
            {
                var isInt = int.TryParse(text, out var value);
                if (!isInt) return;
                var clamped = Rgb.ClampedComponent(value);
                _vm.RefreshFromRgb(_vm.Rgb.WithGreen(clamped));
                control.Text = clamped.ToString();
            });
            HandleInput(Blue, (text, control) =>
            {
                var isInt = int.TryParse(text, out var value);
                if (!isInt) return;
                var clamped = Rgb.ClampedComponent(value);
                _vm.RefreshFromRgb(_vm.Rgb.WithBlue(clamped));
                control.Text = clamped.ToString();
            });

            // HSL manual input
            HandleInputEnterOrFocusLost(HslText,
                (text, control) =>
                {
                    var nullable = Hsl.FromString(text);
                    if (nullable.HasValue) _vm.RefreshFromHsl(nullable.Value);
                });
            HandleInputEnterOrFocusLost(HslHue, (text, control) =>
            {
                var isDouble = double.TryParse(text, out var value);
                if (!isDouble) return;
                var clamped = Hsl.ClampedHue(value);
                _vm.RefreshFromHsl(_vm.Hsl.WithHue(clamped));
                control.Text = clamped.ToString(CultureInfo.InvariantCulture);
            });
            HandleInputEnterOrFocusLost(HslSaturation, (text, control) =>
            {
                var isDouble = double.TryParse(text, out var value);
                if (!isDouble) return;
                var clamped = Hsl.ClampedSaturation(value);
                _vm.RefreshFromHsl(_vm.Hsl.WithSaturation(clamped));
                control.Text = clamped.ToString(CultureInfo.InvariantCulture);
            });
            HandleInputEnterOrFocusLost(HslLuminance, (text, control) =>
            {
                var isDouble = double.TryParse(text, out var value);
                if (!isDouble) return;
                var clamped = Hsl.ClampedLuminance(value);
                _vm.RefreshFromHsl(_vm.Hsl.WithLuminance(clamped));
                control.Text = clamped.ToString(CultureInfo.InvariantCulture);
            });

            // HSV manual input
            HandleInputEnterOrFocusLost(HsvText,
                (text, control) =>
                {
                    var nullable = Hsv.FromString(text);
                    if (nullable.HasValue) _vm.RefreshFromHsv(nullable.Value);
                });
            HandleInputEnterOrFocusLost(HsvHue, (text, control) =>
            {
                var isDouble = double.TryParse(text, out var value);
                if (!isDouble) return;
                var clamped = Hsv.ClampedHue(value);
                _vm.RefreshFromHsv(_vm.Hsv.WithHue(clamped));
                control.Text = clamped.ToString(CultureInfo.InvariantCulture);
            });
            HandleInputEnterOrFocusLost(HsvSaturation, (text, control) =>
            {
                var isDouble = double.TryParse(text, out var value);
                if (!isDouble) return;
                var clamped = Hsv.ClampedSaturation(value);
                _vm.RefreshFromHsv(_vm.Hsv.WithSaturation(clamped));
                control.Text = clamped.ToString(CultureInfo.InvariantCulture);
            });
            HandleInputEnterOrFocusLost(HsvValue, (text, control) =>
            {
                var isDouble = double.TryParse(text, out var value);
                if (!isDouble) return;
                var clamped = Hsv.ClampedValue(value);
                _vm.RefreshFromHsv(_vm.Hsv.WithValue(clamped));
                control.Text = clamped.ToString(CultureInfo.InvariantCulture);
            });

            // CMYK manual input
            HandleInputEnterOrFocusLost(CmykText,
                (text, control) =>
                {
                    var nullable = Cmyk.FromString(text);
                    if (nullable.HasValue) _vm.RefreshFromCmyk(nullable.Value);
                });
            HandleInput(CmykCyan, (text, control) =>
            {
                var isDouble = double.TryParse(text, out var value);
                if (!isDouble) return;
                var clamped = Cmyk.ClampedComponent(value);
                _vm.RefreshFromCmyk(_vm.Cmyk.WithCyan(clamped));
                control.Text = clamped.ToString(CultureInfo.InvariantCulture);
            });
            HandleInput(CmykMagenta, (text, control) =>
            {
                var isDouble = double.TryParse(text, out var value);
                if (!isDouble) return;
                var clamped = Cmyk.ClampedComponent(value);
                _vm.RefreshFromCmyk(_vm.Cmyk.WithMagenta(clamped));
                control.Text = clamped.ToString(CultureInfo.InvariantCulture);
            });
            HandleInput(CmykYellow, (text, control) =>
            {
                var isDouble = double.TryParse(text, out var value);
                if (!isDouble) return;
                var clamped = Cmyk.ClampedComponent(value);
                _vm.RefreshFromCmyk(_vm.Cmyk.WithYellow(clamped));
                control.Text = clamped.ToString(CultureInfo.InvariantCulture);
            });
            HandleInput(CmykKey, (text, control) =>
            {
                var isDouble = double.TryParse(text, out var value);
                if (!isDouble) return;
                var clamped = Cmyk.ClampedComponent(value);
                _vm.RefreshFromCmyk(_vm.Cmyk.WithKey(clamped));
                control.Text = clamped.ToString(CultureInfo.InvariantCulture);
            });

            // CIELAB manual input
            HandleInputEnterOrFocusLost(LabText,
                (text, control) =>
                {
                    var nullable = Lab.FromString(text);
                    if (nullable.HasValue) _vm.RefreshFromLab(nullable.Value);
                });
            HandleInputEnterOrFocusLost(LabL, (text, control) =>
            {
                var isDouble = double.TryParse(text, out var value);
                if (!isDouble) return;
                var clamped = Lab.ClampedL(value);
                _vm.RefreshFromLab(_vm.Lab.WithL(clamped));
                control.Text = clamped.ToString(CultureInfo.InvariantCulture);
            });
            HandleInputEnterOrFocusLost(LabA, (text, control) =>
            {
                var isDouble = double.TryParse(text, out var value);
                if (!isDouble) return;
                var clamped = Lab.ClampedA(value);
                _vm.RefreshFromLab(_vm.Lab.WithA(clamped));
                control.Text = clamped.ToString(CultureInfo.InvariantCulture);
            });
            HandleInputEnterOrFocusLost(LabB, (text, control) =>
            {
                var isDouble = double.TryParse(text, out var value);
                if (!isDouble) return;
                var clamped = Lab.ClampedB(value);
                _vm.RefreshFromLab(_vm.Lab.WithB(clamped));
                control.Text = clamped.ToString(CultureInfo.InvariantCulture);
            });

            // HEX slider change
            HandleSliderChange(RedHexSlider, (value, slider) =>
            {
                Debug.WriteLine($"R: {value}");
                _vm.RefreshFromRgb(_vm.Rgb.WithRed((int) value));
            });
            HandleSliderChange(GreenHexSlider, (value, slider) =>
            {
                Debug.WriteLine($"G: {value}");
                _vm.RefreshFromRgb(_vm.Rgb.WithGreen((int) value));
            });
            HandleSliderChange(BlueHexSlider, (value, slider) =>
            {
                Debug.WriteLine($"B: {value}");
                _vm.RefreshFromRgb(_vm.Rgb.WithBlue((int) value));
            });

            // RGB slider change
            HandleSliderChange(RedSlider, (value, slider) =>
            {
                Debug.WriteLine($"R: {value}");
                _vm.RefreshFromRgb(_vm.Rgb.WithRed((int) value));
            });
            HandleSliderChange(GreenSlider, (value, slider) =>
            {
                Debug.WriteLine($"G: {value}");
                _vm.RefreshFromRgb(_vm.Rgb.WithGreen((int) value));
            });
            HandleSliderChange(BlueSlider, (value, slider) =>
            {
                Debug.WriteLine($"B: {value}");
                _vm.RefreshFromRgb(_vm.Rgb.WithBlue((int) value));
            });

            // HSL slider change
            HandleSliderChange(HslHueSlider, (value, slider) =>
            {
                Debug.WriteLine($"H: {value}");
                _vm.RefreshFromHsl(_vm.Hsl.WithHue(value));
            });
            HandleSliderChange(HslSaturationSlider, (value, slider) =>
            {
                Debug.WriteLine($"S: {value}");
                _vm.RefreshFromHsl(_vm.Hsl.WithSaturation(value));
            });
            HandleSliderChange(HslLuminanceSlider, (value, slider) =>
            {
                Debug.WriteLine($"L: {value}");
                _vm.RefreshFromHsl(_vm.Hsl.WithLuminance(value));
            });

            // HSV slider change
            HandleSliderChange(HsvHueSlider, (value, slider) =>
            {
                Debug.WriteLine($"H: {value}");
                _vm.RefreshFromHsv(_vm.Hsv.WithHue(value));
            });
            HandleSliderChange(HsvSaturationSlider, (value, slider) =>
            {
                Debug.WriteLine($"S: {value}");
                _vm.RefreshFromHsv(_vm.Hsv.WithSaturation(value));
            });
            HandleSliderChange(HsvValueSlider, (value, slider) =>
            {
                Debug.WriteLine($"V: {value}");
                _vm.RefreshFromHsv(_vm.Hsv.WithValue(value));
            });

            // CMYK slider change
            HandleSliderChange(CmykCyanSlider, (value, slider) =>
            {
                Debug.WriteLine($"C: {value}");
                _vm.RefreshFromCmyk(_vm.Cmyk.WithCyan((int) value));
            });
            HandleSliderChange(CmykMagentaSlider, (value, slider) =>
            {
                Debug.WriteLine($"M: {value}");
                _vm.RefreshFromCmyk(_vm.Cmyk.WithMagenta((int) value));
            });
            HandleSliderChange(CmykYellowSlider, (value, slider) =>
            {
                Debug.WriteLine($"Y: {value}");
                _vm.RefreshFromCmyk(_vm.Cmyk.WithYellow((int) value));
            });
            HandleSliderChange(CmykKeySlider, (value, slider) =>
            {
                Debug.WriteLine($"K: {value}");
                _vm.RefreshFromCmyk(_vm.Cmyk.WithKey((int) value));
            });

            // Lab slider change
            HandleSliderChange(LabLSlider, (value, slider) =>
            {
                Debug.WriteLine($"L*: {value}");
                _vm.RefreshFromLab(_vm.Lab.WithL(value));
            });
            HandleSliderChange(LabASlider, (value, slider) =>
            {
                Debug.WriteLine($"a*: {value}");
                _vm.RefreshFromLab(_vm.Lab.WithA(value));
            });
            HandleSliderChange(LabBSlider, (value, slider) =>
            {
                Debug.WriteLine($"b*: {value}");
                _vm.RefreshFromLab(_vm.Lab.WithB(value));
            });
        }

        private void StartEyedropper()
        {
            var cap = new ScreenCapture();
            var freezeFrame = cap.GetFullScreen();
            FreezeFrame.Instance.BitmapSource = freezeFrame;

            _freezeFrameWin = new FreezeFrameWindow(_vm);
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

        private static bool IsActiveControl(UIElement control)
        {
            return control.IsMouseOver || control.IsFocused || control.IsKeyboardFocusWithin;
        }

        #region Disable Maximize window

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        // ReSharper disable once InconsistentNaming
        private const int GWL_STYLE = -16;

        // ReSharper disable once InconsistentNaming
        private const int WS_MAXIMIZE_BOX = 0x10000;

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            var hWnd = new WindowInteropHelper((System.Windows.Window) sender).Handle;
            var value = GetWindowLong(hWnd, GWL_STYLE);
            SetWindowLong(hWnd, GWL_STYLE, value & ~WS_MAXIMIZE_BOX);
        }

        #endregion
    }
}