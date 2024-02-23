using Microsoft.AspNetCore.Authentication;
using MyTemsAPI.Authentication;
using MyTemsAPI.Application;
using MyTemsAPI.Controllers;
using MyTemsAPI.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationScheme.DefaultScheme, null);

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy(AuthorizeControllerModelConvention.PolicyName, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddAuthenticationSchemes(ApiKeyAuthenticationScheme.DefaultScheme);
    });
});

builder.Services.Configure<ApiAuthOptions>(builder.Configuration.GetSection(ApiAuthOptions.Section));

//NServiceBus
var endpointConfiguration = new NServiceBus.EndpointConfiguration("MyTemsAPI");

// Choose JSON to serialize and deserialize messages
endpointConfiguration.UseSerialization<NServiceBus.SystemJsonSerializer>();

var transport = endpointConfiguration.UseTransport<LearningTransport>();


var endpointContainer = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, builder.Services);
var endpointInstance = await endpointContainer.Start(builder.Services.BuildServiceProvider());

builder.Services.AddSingleton<IMessageSession>(endpointInstance);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyAuthMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.MapAddressEndpoints();

app.Run();

namespace MyTemsAPI
{
    public class Program
    {
    }
}
