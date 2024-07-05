using LarsProjekt.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LarsProjekt.StoreApiAdapter
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStoreApi(this IServiceCollection services)
        {
            return services
                .AddScoped<IStoreApiClient, StoreApiClient>()
                .AddScoped<IProductService, ProductService>();
                //.AddScoped<ICouponService, CouponService>();
        }
    }
}
