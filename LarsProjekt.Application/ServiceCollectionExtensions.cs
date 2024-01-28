using Microsoft.Extensions.DependencyInjection;

namespace LarsProjekt.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddScoped<IProductService, ProductService>();
    }
}
