using System;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace PixelPalette.Window.Behaviors
{
    public class LockInitialSize : Behavior<System.Windows.Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ContentRendered += OnContentRendered;
        }

        protected override void OnDetaching()
        {
            // possibly detached before ever rendering
            AssociatedObject.ContentRendered -= OnContentRendered;
            base.OnDetaching();
        }

        private void OnContentRendered(object s, EventArgs e)
        {
            // Just once
            AssociatedObject.ContentRendered -= OnContentRendered;

            if (MinWidth)
            {
                AssociatedObject.MinWidth = AssociatedObject.ActualWidth;
            }

            if (MinHeight)
            {
                AssociatedObject.MinHeight = AssociatedObject.ActualHeight;
            }

            if (MaxWidth)
            {
                AssociatedObject.MaxWidth = AssociatedObject.ActualWidth;
            }

            if (MaxHeight)
            {
                AssociatedObject.MaxHeight = AssociatedObject.ActualHeight;
            }
        }

        public bool MinWidth
        {
            get => (bool) GetValue(MinWidthProperty);
            set => SetValue(MinWidthProperty, value);
        }

        public static readonly DependencyProperty MinWidthProperty =
            DependencyProperty.Register(nameof(MinWidth), typeof(bool), typeof(LockInitialSize));

        public bool MinHeight
        {
            get => (bool) GetValue(MinHeightProperty);
            set => SetValue(MinHeightProperty, value);
        }

        public static readonly DependencyProperty MinHeightProperty =
            DependencyProperty.Register(nameof(MinHeight), typeof(bool), typeof(LockInitialSize));

        public bool MaxWidth
        {
            get => (bool) GetValue(MaxWidthProperty);
            set => SetValue(MaxWidthProperty, value);
        }

        public static readonly DependencyProperty MaxWidthProperty =
            DependencyProperty.Register(nameof(MaxWidth), typeof(bool), typeof(LockInitialSize));

        public bool MaxHeight
        {
            get => (bool) GetValue(MaxHeightProperty);
            set => SetValue(MaxHeightProperty, value);
        }

        public static readonly DependencyProperty MaxHeightProperty =
            DependencyProperty.Register(nameof(MaxHeight), typeof(bool), typeof(LockInitialSize));
    }
}