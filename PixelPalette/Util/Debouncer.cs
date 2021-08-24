using System;
using System.Windows.Threading;

namespace PixelPalette.Util
{
    public class Debouncer
    {
        private readonly DispatcherPriority _dispatcherPriority;
        private readonly Dispatcher _dispatcher;
        private DispatcherTimer? _timer;

        public Debouncer(Dispatcher? dispatcher = null, DispatcherPriority priority = DispatcherPriority.ApplicationIdle)
        {
            _dispatcherPriority = priority;
            _dispatcher = dispatcher ?? Dispatcher.CurrentDispatcher;
        }

        public void Debounce(TimeSpan interval, Action<object?> action, object? parameter = null)
        {
            _timer?.Stop();
            _timer = null;

            _timer = new DispatcherTimer(
                interval,
                _dispatcherPriority,
                (_, _) => ExecuteDebouncedAction(action, parameter),
                _dispatcher);

            _timer.Start();
        }

        private void ExecuteDebouncedAction(Action<object?> action, object? parameter)
        {
            if (_timer == null) return;

            _timer?.Stop();
            _timer = null;
            action.Invoke(parameter);
        }
    }
}
