using Flurl.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MySDK.Logger
{
    internal class MyLoggerSender<LogT> : IDisposable
    {
        private readonly LoggerConfiguration _configuration;
        private readonly ConcurrentQueue<LogT> _queue;
        private readonly Task _senderTask;
        private readonly CancellationTokenSource _senderTokenSource;

        public MyLoggerSender(LoggerConfiguration configuration)
        {
            _configuration = configuration;
            if (string.IsNullOrEmpty(_configuration.LogURL))
            {
                throw new ArgumentNullException("LogURL", "The logger configuration's LogURL must configure.");
            }
            
            _queue = new ConcurrentQueue<LogT>();
            _senderTokenSource = new CancellationTokenSource();
            _senderTask = new Task(async () => { await SendingAsync(); },
                _senderTokenSource.Token,
                TaskCreationOptions.LongRunning);
        }

        public void Dispose()
        {
            if (!_senderTokenSource.IsCancellationRequested)
            {
                _senderTokenSource.CancelAfter(3000);
            }
        }

        public void Sending(LogT log)
        {
            if (!_senderTokenSource.IsCancellationRequested
                && null != log
                && _queue.Count < _configuration.MaxQueueLength)
            {
                _queue.Enqueue(log);
                if (_senderTask.Status == TaskStatus.Created)
                {
                    _senderTask.Start();
                }
            }
        }

        private async Task SendingAsync()
        {
            Stopwatch watch = Stopwatch.StartNew();
            while (!_senderTokenSource.IsCancellationRequested)
            {
                watch.Start();
                if (_queue.Count > _configuration.SaveLogCountEveryTime || (_queue.Count > 0 &&  watch.ElapsedMilliseconds > _configuration.SaveLogIntervel))
                {
                    var dequeueCount = _queue.Count > _configuration.SaveLogCountEveryTime
                        ? _configuration.SaveLogCountEveryTime
                        : _queue.Count;

                    List<LogT> logs = new List<LogT>();
                    while (dequeueCount > 0)
                    {
                        if (_queue.TryDequeue(out LogT log))
                        {
                            logs.Add(log);
                        }
                        else
                        {
                            break;
                        }
                        dequeueCount--;
                    }

                    if (logs.Any())
                    {
                        try
                        {
                            await _configuration.LogURL.PostJsonAsync(logs);
                        }
                        catch { }
                        watch.Stop();
                    }
                }

                Thread.Sleep(2000);
            }
        }

    }
}
