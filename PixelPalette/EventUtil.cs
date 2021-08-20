using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using PixelPalette.Annotations;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace PixelPalette
{
    internal static class EventUtil
    {
        public static void HandleClick(IEnumerable<IInputElement> controls, Action action)
        {
            void Handler(object sender, RoutedEventArgs e)
            {
                action();
            }

            foreach (var control in controls)
            {
                control.AddHandler(ButtonBase.ClickEvent, (RoutedEventHandler) Handler);
            }
        }

        public static void HandleClick(IInputElement control, Action action)
        {
            HandleClick(new[] { control }, action);
        }

        public static void HandleClick(IEnumerable<IInputElement> controls, RoutedEventHandler handler)
        {
            foreach (var control in controls)
            {
                control.AddHandler(ButtonBase.ClickEvent, handler);
            }
        }

        public static void HandleClick(IInputElement control, RoutedEventHandler handler)
        {
            HandleClick(new[] { control }, handler);
        }

        public static void HandleKey(Key? key, IEnumerable<IInputElement> controls, Action action)
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

        public static void HandleKey(Key? key, IInputElement control, Action action)
        {
            HandleKey(key, new[] { control }, action);
        }

        public static void HandleMouseWheel(IEnumerable<IInputElement> controls, [CanBeNull] Action upAction, [CanBeNull] Action downAction)
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

        public static void HandleMouseWheel(IInputElement control, [CanBeNull] Action upAction, [CanBeNull] Action downAction)
        {
            HandleMouseWheel(new[] { control }, upAction, downAction);
        }

        public static void HandleInputEnterOrFocusLost(IEnumerable<TextBox> controls, Action<string> action)
        {
            foreach (var control in controls)
            {
                void HandleIt()
                {
                    var text = control.Text;
                    action(text);
                }

                control.KeyDown += (o, ev) =>
                {
                    if (ev.Key != Key.Enter) return;
                    HandleIt();
                };

                control.LostFocus += (o, ev) => { HandleIt(); };
            }
        }

        public static void HandleInputEnterOrFocusLost(TextBox control, Action<string> action)
        {
            HandleInputEnterOrFocusLost(new[] { control }, action);
        }

        public static void HandleSliderChange(Slider control, Action<double> action)
        {
            control.ValueChanged += (o, ev) =>
            {
                if (!IsActiveControl(control)) return;
                action(ev.NewValue);
            };
        }

        private static bool IsActiveControl(UIElement control)
        {
            return control.IsMouseOver || control.IsFocused || control.IsKeyboardFocusWithin;
        }
    }
}
