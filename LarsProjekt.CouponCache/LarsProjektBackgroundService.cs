using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LarsProjekt.CouponCache;

public class LarsProjektBackgroundService : BackgroundService
{
    private readonly IHostApplicationLifetime _lifetime;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<LarsProjektBackgroundService> _logger;

    private readonly TimeSpan _refreshInterval = TimeSpan.FromSeconds(10);

    public LarsProjektBackgroundService(IHostApplicationLifetime lifetime, IServiceProvider serviceProvider, ILogger<LarsProjektBackgroundService> logger)
    {
        _lifetime = lifetime;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken,
                _lifetime.ApplicationStarted);

            await Task.Delay(-1, tokenSource.Token);
        }
        catch(TaskCanceledException)
        { 
            
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();

                var cache = scope.ServiceProvider.GetRequiredService<ICouponCache>();

                await cache.Refresh(stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error...");
            }

            try
            {
                await Task.Delay(_refreshInterval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }
}
