using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gmtl.HandyLib.AspNetCore.HostedServices
{
    public abstract class ScopedBackgroundService : BackgroundService
    {
        protected readonly IServiceScopeFactory _serviceScopeFactory;

        protected ScopedBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger logger) : base(logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Process(CancellationToken stoppingCtsToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            try
            {
                await ProcessInScope(scope.ServiceProvider, stoppingCtsToken);
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, GetType().Name);
            }
        }

        public abstract Task ProcessInScope(IServiceProvider serviceProvider, CancellationToken token);
    }
}
