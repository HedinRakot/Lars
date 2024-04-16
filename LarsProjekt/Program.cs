using LarsProjekt.Application;
using LarsProjekt.CouponCache;
using LarsProjekt.Authentication;
using LarsProjekt.ErrorHandling;
using Microsoft.AspNetCore.Authentication.Cookies;
using NServiceBus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
try
{
    LarsProjekt.Logging.SerilogConfigExtension.AddSerilogWithTracing(builder, "MyTemsDb");

    builder.Services.AddControllersWithViews();

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
            options.LoginPath = "/Login/SignIn/";
            options.AccessDeniedPath = "/Login/Forbidden/";
        });

    builder.Services.AddAuthorization(o =>
    {
        o.AddPolicy(AuthorizeControllerModelConvention.PolicyName, policy =>
        {
            policy.RequireAuthenticatedUser();
        });
    });

    builder.Services.AddSession();

    builder.Services.AddApplication();
    builder.Services.AddCouponCache();

    builder.Services.AddMvc();

    builder.Services.Configure<ApiUserOptions>(builder.Configuration.GetSection(ApiUserOptions.Section));
    builder.Services.Configure<ApiUrlOptions>(builder.Configuration.GetSection(ApiUrlOptions.Section));

    await LarsProjekt.NServiceBus.ConfigExtension.AddNServiceBus(builder.Configuration, builder.Services, "LarsProjekt", "NServiceBus");

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    LarsProjekt.Logging.SerilogConfigExtension.AddSerilogRequestLoggingWithTracingListener(app);

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseSession();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.UseMiddleware<ErrorHandlingMiddleware>();

    await app.RunAsync();

    IEndpointInstance? endpointInstance = app.Services.GetService<IEndpointInstance>();

    await endpointInstance.Stop()
                          .ConfigureAwait(false);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.CloseAndFlush();
}

namespace LarsProjekt
{
    public class Program
    {
    }
}
