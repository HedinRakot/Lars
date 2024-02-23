using Microsoft.Extensions.DependencyInjection;
using MyTemsAPI.Application.IServices;
using MyTemsAPI.Application.Services;

namespace MyTemsAPI.Application;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddScoped<ICreateOrderService, CreateOrderService>();
    }
}
