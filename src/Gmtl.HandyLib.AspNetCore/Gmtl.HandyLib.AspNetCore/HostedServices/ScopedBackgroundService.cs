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

        protected override async Task ExecuteAction(CancellationToken stoppingCtsToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await ExecuteActionInScope(scope.ServiceProvider, stoppingCtsToken);
        }

        protected abstract Task ExecuteActionInScope(IServiceProvider serviceProvider, CancellationToken token);
    }
}
