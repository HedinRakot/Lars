using LarsProjekt.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LarsProjekt.UserApiAdapter
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserApi(this IServiceCollection services)
        {
            return services
                .AddScoped<IUserApiClient, UserApiClient>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IAddressService, AddressService>();
        }
    }
}
