using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTemsAPI.Domain;
using MyTemsAPI.Domain.IRepositories;
using MyTemsAPI.Repositories;

namespace MyTemsAPI.Database;

public static class DbServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var result = services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("MyTemsDb"));
        });

        return result.AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IProductRepository, ProductRepository>()
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<IOrderDetailRepository, OrderDetailRepository>()
            .AddScoped<IAddressRepository, AddressRepository>()
            .AddScoped<ICouponRepository, CouponRepository>()
            .AddScoped<ICouponCountService, CouponCountService>()
            .AddScoped<ISqlUnitOfWork, SqlUnitOfWork>();
    }
}
