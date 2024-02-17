using LarsProjekt.Application;
using LarsProjekt.Application.IService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Reflection;


namespace LarsProjekt.IntegrationTests.TestSetup;
//keine datenbank alles löschen, keine echte api aufrufen, api mocken
public class IntegrationTestsFixture : WebApplicationFactory<Program>
{
    public IProductService TestProductService = Substitute.For<IProductService>();
    public IApiClient TestClient = Substitute.For<IApiClient>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests")
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile(Path.Combine(
                    Path.GetDirectoryName(Assembly.GetAssembly(typeof(IntegrationTestsFixture)).Location),
                    "appsettings.integrationtests.json"
                ));
            })
            .ConfigureTestServices(services =>
            {
                //TODO mock api //----------------------------------------------------- Client
                services.AddSingleton(TestClient);
                services.AddSingleton(TestProductService);
            });

        base.ConfigureWebHost(builder);
    }
}
