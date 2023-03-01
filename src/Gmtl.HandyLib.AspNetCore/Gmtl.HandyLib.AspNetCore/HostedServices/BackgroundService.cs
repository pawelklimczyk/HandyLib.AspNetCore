using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gmtl.HandyLib.AspNetCore.HostedServices
{
    public abstract class BackgroundService : IHostedService
    {
        private Task _executingTask;

        protected readonly CancellationTokenSource _stoppingCts = new();
        protected readonly ILogger _logger;

        public BackgroundService(ILogger logger)
        {
            _logger = logger;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            _logger.LogInformation($"Started {GetType().Name}");

            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
            {
                _logger.LogInformation($"Stopped {GetType().Name}");
                return;
            }

            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite,
                    cancellationToken));

                _logger.LogInformation($"Stopped {GetType().Name}");
            }
        }

        protected virtual async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await Process(_stoppingCts.Token);
            }
            catch (TaskCanceledException)
            {
                //leave that exception. It happends when service is shutdown
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, GetType().Name);
            }
        }

        protected abstract Task Process(CancellationToken stoppingCtsToken);
    }
}
