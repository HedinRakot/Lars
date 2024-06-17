using LarsProjekt.Application;
using LarsProjekt.CouponCache;
using LarsProjekt.Authentication;
using LarsProjekt.ErrorHandling;
using Microsoft.AspNetCore.Authentication.Cookies;
using NServiceBus;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using IdentityModel.Client;

var builder = WebApplication.CreateBuilder(args);
try
{
    LarsProjekt.Logging.SerilogConfigExtension.AddSerilogWithTracing(builder, "MyTemsDb");

    // discover endpoints from metadata
    var client = new HttpClient();
    var disco = await client.GetDiscoveryDocumentAsync("https://localhost:7099");
    if (disco.IsError)
    {
        Console.WriteLine(disco.Error);
        return;
    }

    builder.Services.AddControllersWithViews();

    builder.Services.AddAuthentication(options =>
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
        options.Scope.Add("samplemytemsapiscope");
        options.Scope.Add("offline_access");
        options.Scope.Add("verification");
        options.ClaimActions.MapJsonKey("email_verified", "email_verified");
        options.GetClaimsFromUserInfoEndpoint = true;

        options.MapInboundClaims = false; // Don't rename claim types

        options.SaveTokens = true;
    });

    //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    //    .AddCookie(options =>
    //    {
    //        options.Cookie.HttpOnly = true;
    //        options.Cookie.SameSite = SameSiteMode.Lax;
    //        options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
    //        options.LoginPath = "/Login/SignIn/";
    //        options.AccessDeniedPath = "/Login/Forbidden/";
    //    });

    //builder.Services.AddAuthorization(o =>
    //{
    //    o.AddPolicy(AuthorizeControllerModelConvention.PolicyName, policy =>
    //    {
    //        policy.RequireAuthenticatedUser();
    //    });
    //});

    builder.Services.AddSession();

    builder.Services.AddApplication();
    //builder.Services.AddCouponCache();

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
