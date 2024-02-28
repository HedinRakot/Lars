using LarsProjekt.Application.IService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Testing;
using NSubstitute;
using System.Reflection;

namespace LarsProjekt.IntegrationTests.TestSetup;
public class IntegrationTestsFixture : WebApplicationFactory<Program>
{
    public IProductService TestProductService = Substitute.For<IProductService>();
    public IOrderService TestOrderService = Substitute.For<IOrderService>();
    public IUserService TestUserService = Substitute.For<IUserService>();
    public ICreateOrderService TestCreateOrderService = Substitute.For<ICreateOrderService>();
    //public TestableMessageSession TestMessageSession = Substitute.For<TestableMessageSession>();

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
                services.AddSingleton(TestProductService);
                services.AddSingleton(TestOrderService);
                services.AddSingleton(TestUserService);
                services.AddSingleton(TestCreateOrderService);
                //services.AddSingleton(TestMessageSession);
            });

        base.ConfigureWebHost(builder);
    }
}
