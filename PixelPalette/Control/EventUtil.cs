using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace PixelPalette.Control;

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
            control.PreviewKeyDown += (_, ev) =>
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

    public static void HandleMouseWheel(IEnumerable<IInputElement> controls, Action? upAction, Action? downAction)
    {
        foreach (var control in controls)
        {
            control.MouseWheel += (_, ev) =>
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

    public static void HandleMouseWheel(IInputElement control, Action? upAction, Action? downAction)
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

            control.KeyDown += (_, ev) =>
            {
                if (ev.Key != Key.Enter) return;
                HandleIt();
            };

            control.LostFocus += (_, _) => { HandleIt(); };
        }
    }

    public static void HandleInputEnterOrFocusLost(TextBox control, Action<string> action)
    {
        HandleInputEnterOrFocusLost(new[] { control }, action);
    }

    public static void HandleSliderChange(Slider control, Action<double> action)
    {
        control.ValueChanged += (_, ev) =>
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