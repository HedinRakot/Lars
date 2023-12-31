﻿using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LarsProjekt.Database;

public static class DbServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var result = services.AddDbContext<ApplicationDbContext>(options =>
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
