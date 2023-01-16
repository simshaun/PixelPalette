using System;
using System.Windows.Threading;

namespace PixelPalette.Util;

public class Throttler
{
    private DispatcherTimer? _timer;
    private DateTime TimerStarted { get; set; } = DateTime.UtcNow.AddYears(-1);

    public void Throttle(
        int interval,
        Action<object?> action,
        object? param = null,
        DispatcherPriority priority = DispatcherPriority.ApplicationIdle,
        Dispatcher? dispatcher = null
    )
    {
        // kill pending timer and pending ticks
        _timer?.Stop();
        _timer = null;

        dispatcher ??= Dispatcher.CurrentDispatcher;

        var curTime = DateTime.UtcNow;
        var difference = curTime.Subtract(TimerStarted).TotalMilliseconds;

        // if timeout is not up yet - adjust timeout to fire with potentially new Action parameters           
        if (difference < interval)
        {
            interval -= (int) difference;
        }

        _timer = new DispatcherTimer(
            TimeSpan.FromMilliseconds(interval),
            priority,
            (_, _) =>
            {
                if (_timer == null) return;
                _timer.Stop();
                _timer = null;
                action.Invoke(param);
            },
            dispatcher
        );

        _timer.Start();
        TimerStarted = curTime;
    }
}