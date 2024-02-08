using LarsProjekt.Application.IService;
using LarsProjekt.Application.Service;
using Microsoft.Extensions.DependencyInjection;

namespace LarsProjekt.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddScoped<IProductService, ProductService>()
            .AddScoped<ICouponService, CouponService>()
            .AddScoped<IApiClient, ApiClient>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<IAddressService, AddressService>();
    }
}
