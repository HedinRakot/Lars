using LarsProjekt.Application;
using LarsProjekt.Authentication;
using LarsProjekt.ErrorHandling;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging.Console;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
        options.LoginPath = "/Login/SignIn/";
        options.AccessDeniedPath = "/Login/Forbidden/";
    });
    //.AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationScheme.DefaultScheme, null);

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy(AuthorizeControllerModelConvention.PolicyName, policy =>
    {
        policy.RequireAuthenticatedUser();        
        //policy.AddAuthenticationSchemes(ApiKeyAuthenticationScheme.DefaultScheme);
    });
});

builder.Services.AddSession();

builder.Services.AddApplication();

builder.Services.AddMvc(options =>
{
    options.Conventions.Add(new AuthorizeControllerModelConvention());
});

builder.Logging.AddSimpleConsole(i => i.ColorBehavior = LoggerColorBehavior.Enabled);

builder.Services.AddScoped<ApiKeyAuthorizationFilter>();

builder.Services.Configure<ApiUserOptions>(builder.Configuration.GetSection(ApiUserOptions.Section));
builder.Services.Configure<ApiUrlOptions>(builder.Configuration.GetSection(ApiUrlOptions.Section));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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
