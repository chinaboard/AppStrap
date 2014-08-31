using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AppStrap.Utils
{
    public sealed class Scheduler : IScheduler
    {
        private CancellationTokenSource token = null;

        public void Start(TimeSpan interval, Action action)
        {
            Start(interval, t =>
            {
                if (!t.IsCancellationRequested)
                {
                    action();
                }
            });
        }

        public void Start(TimeSpan interval, Action<CancellationToken> action)
        {
            Start(interval, t =>
            {
                action(t);
                return Completed();
            });
        }

        public void Start(TimeSpan interval, Func<Task> task)
        {
            Start(interval, t => t.IsCancellationRequested ? task() : Completed());
        }

        public void Start(TimeSpan interval, Func<CancellationToken, Task> task)
        {
            Preconditions.CheckLessZero((int)interval.TotalSeconds, "interval");
            Preconditions.CheckNull(this.token, "Scheduler");

            this.token = new CancellationTokenSource();

            RunScheduler(interval, task, this.token);
        }

        private static void RunScheduler(TimeSpan interval, Func<CancellationToken, Task> action, CancellationTokenSource token)
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
                    catch (Exception x)
                    {
                        HandleException(x);
                        token.Cancel();
                    }
                }
            }, token.Token);
        }

        private static Task Completed()
        {
            return Task.FromResult(true);
        }

        private static void HandleException(Exception x)
        {
            //nothing
        }

        public void Stop()
        {
            if (this.token != null)
            {
                token.Cancel();
            }
        }

        private void RunAction(Action<CancellationToken> action)
        {
            try
            {
                action(this.token.Token);
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public void Dispose()
        {
            if (this.token != null)
            {
                this.token.Cancel();
                this.token.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
