using LarsProjekt.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LarsProjekt.OrderApiAdapter
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderApi(this IServiceCollection services)
        {
            return services
                .AddScoped<IOrderApiClient, OrderApiClient>()
                .AddScoped<ICreateOrderService, CreateOrderService>()
                .AddScoped<IOrderService, OrderService>();
        }
    }
}
