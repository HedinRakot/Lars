using LarsProjekt;
using LarsProjekt.Application;
using LarsProjekt.Authentication;
using LarsProjekt.ErrorHandling;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Console;
using NServiceBus;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddMvc();

builder.Logging.AddSimpleConsole(i => i.ColorBehavior = LoggerColorBehavior.Enabled);

builder.Services.Configure<ApiUserOptions>(builder.Configuration.GetSection(ApiUserOptions.Section));
builder.Services.Configure<ApiUrlOptions>(builder.Configuration.GetSection(ApiUrlOptions.Section));

//NServiceBus
var endpointConfiguration = new EndpointConfiguration("LarsProjekt");
endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.AuditProcessedMessagesTo("audit");
endpointConfiguration.EnableInstallers();

// Choose JSON to serialize and deserialize messages
endpointConfiguration.UseSerialization<NServiceBus.SystemJsonSerializer>();

var nserviceBusConnectionString = builder.Configuration.GetConnectionString("NServiceBus");

var transportConfig = new NServiceBus.SqlServerTransport(nserviceBusConnectionString)
{
    DefaultSchema = "dbo",
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive,
    Subscriptions =
    {
        CacheInvalidationPeriod = TimeSpan.FromMinutes(1),
        SubscriptionTableName = new NServiceBus.Transport.SqlServer.SubscriptionTableName(
            table: "Subscriptions",
            schema: "dbo")
    }
};

transportConfig.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
transportConfig.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");

var transport = endpointConfiguration.UseTransport<SqlServerTransport>(transportConfig);
//transport.RouteToEndpoint(typeof(TestCommand), "MyTemsAPI");    -- Jedes Command & den Empfänger konfigurieren

//persistence
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
dialect.Schema("dbo");
persistence.ConnectionBuilder(() => new SqlConnection(nserviceBusConnectionString));
persistence.TablePrefix("");

await SqlServerHelper.CreateSchema(nserviceBusConnectionString, "dbo");

var endpointContainer = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, builder.Services);
var endpointInstance = await endpointContainer.Start(builder.Services.BuildServiceProvider());

builder.Services.AddSingleton<IMessageSession>(endpointInstance);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

namespace LarsProjekt
{    public class Program
    {
    }
}
