using Microsoft.Extensions.DependencyInjection;

namespace LarsProjekt.CouponCache;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCouponCache(this IServiceCollection services)
    {
        return services
            .AddSingleton<ICouponCache, CouponCache>()
            .AddHostedService<LarsProjektBackgroundService>();
    }
}
