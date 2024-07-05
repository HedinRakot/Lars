using LarsProjekt.CouponCache;
using LarsProjekt.ErrorHandling;
using NServiceBus;
using Serilog;
using Microsoft.AspNetCore.Authentication;
using LarsProjekt.UserApiAdapter;
using LarsProjekt.StoreApiAdapter;
using LarsProjekt.OrderApiAdapter;
using LarsProjekt.MyTemsApiAdapter;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
try
{
    LarsProjekt.Logging.SerilogConfigExtension.AddSerilogWithTracing(builder, "MyTemsDb");

    builder.Services.AddControllersWithViews();

    builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:7099";

        options.ClientId = "aspnetcoreweb";
        options.ClientSecret = "secret";
        options.ResponseType = "code";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("aspnetmvcscope");
        options.Scope.Add("mytemsapiscope");
        options.Scope.Add("offline_access");
        options.GetClaimsFromUserInfoEndpoint = true;
        //options.ClaimActions.MapUniqueJsonKey("name", "name");
        //options.TokenValidationParameters = new TokenValidationParameters
        //{
        //    NameClaimType = "name"
        //    //, RoleClaimType = "role"
        //};
        options.MapInboundClaims = false;
        options.SaveTokens = true;
    });

    builder.Services.AddSession();

    builder.Services.AddCouponCache();

    builder.Services.AddMvc();

    builder.Services.AddStoreApi();
    builder.Services.Configure<StoreApiUserOptions>(builder.Configuration.GetSection(StoreApiUserOptions.Section));
    builder.Services.Configure<StoreApiUrlOptions>(builder.Configuration.GetSection(StoreApiUrlOptions.Section));

    builder.Services.AddOrderApi();
    builder.Services.Configure<OrderApiUserOptions>(builder.Configuration.GetSection(OrderApiUserOptions.Section));
    builder.Services.Configure<OrderApiUrlOptions>(builder.Configuration.GetSection(OrderApiUrlOptions.Section));

    builder.Services.AddMyTemsApi();
    builder.Services.Configure<MyTemsApiUserOptions>(builder.Configuration.GetSection(MyTemsApiUserOptions.Section));
    builder.Services.Configure<MyTemsApiUrlOptions>(builder.Configuration.GetSection(MyTemsApiUrlOptions.Section));

    builder.Services.AddUserApi();
    builder.Services.Configure<ApiUserOptions>(builder.Configuration.GetSection(ApiUserOptions.Section));
    builder.Services.Configure<UserApiUrlOptions>(builder.Configuration.GetSection(UserApiUrlOptions.Section));

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
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .RequireAuthorization();

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
