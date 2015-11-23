using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AppStrap.Utils
{
    internal sealed class Scheduler
    {
        private CancellationTokenSource _token = null;

        public void Start(TimeSpan interval, Action action, Action<Exception> handleException = null, bool throwOnFirstException = false)
        {
            Start(interval, t =>
            {
                if (!t.IsCancellationRequested)
                {
                    action();
                }
            }, handleException, throwOnFirstException);
        }

        public void Start(TimeSpan interval, Action<CancellationToken> action, Action<Exception> handleException = null, bool throwOnFirstException = false)
        {
            Start(interval, t =>
            {
                action(t);
                return Completed();
            }, handleException, throwOnFirstException);
        }

        public void Start(TimeSpan interval, Func<Task> task, Action<Exception> handleException = null, bool throwOnFirstException = false)
        {
            Start(interval, t => t.IsCancellationRequested ? task() : Completed(), handleException, throwOnFirstException);
        }

        public void Start(TimeSpan interval, Func<CancellationToken, Task> task, Action<Exception> handleException = null, bool throwOnFirstException = false)
        {
            Preconditions.CheckLessZero((int)interval.TotalSeconds, "interval");
            Preconditions.CheckNull(_token, "Scheduler");

            _token = new CancellationTokenSource();

            RunScheduler(interval, task, _token, handleException, throwOnFirstException);
        }

        private static void RunScheduler(TimeSpan interval, Func<CancellationToken, Task> action, CancellationTokenSource token, Action<Exception> handleException = null, bool throwOnFirstException = false)
        {
            Task.Factory.StartNew(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(interval, token.Token);
                    try
                    {
                        await action(token.Token);
                    }
                    catch (Exception ex)
                    {
                        if (handleException != null)
                            handleException(ex);
                        token.Cancel(throwOnFirstException);
                    }
                }
            }, token.Token);
        }

        private static Task Completed()
        {
            return Task.FromResult(true);
        }


        public void Stop()
        {
            if (_token != null)
            {
                _token.Cancel();
            }
        }

        private void RunAction(Action<CancellationToken> action)
        {
            try
            {
                action(_token.Token);
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public void Dispose()
        {
            if (_token != null)
            {
                _token.Cancel();
                _token.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
