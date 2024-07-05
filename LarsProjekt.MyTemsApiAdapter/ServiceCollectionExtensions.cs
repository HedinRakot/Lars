using LarsProjekt.Domain.Interfaces;
using LarsProjekt.MyTemsApiAdapter.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LarsProjekt.MyTemsApiAdapter
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyTemsApi(this IServiceCollection services)
        {
            return services
                .AddScoped<IMyTemsApiClient, MyTemsApiClient>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<ICouponService, CouponService>();
                //.AddScoped<IUserService, UserService>()
                //.AddScoped<IOrderService, OrderService>()
                //.AddScoped<IAddressService, AddressService>()
                //.AddScoped<ICreateOrderService, CreateOrderService>()
                ;
        }
    }
}
