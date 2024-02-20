using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTemsAPI.Database;
using MyTemsAPI.Domain;
using System.Reflection;


namespace MyTemsAPI.IntegrationTests.TestSetup;

public class IntegrationTestsFixture : WebApplicationFactory<Program>
{
    private readonly DatabaseSetup _dbSetup;
    public IntegrationTestsFixture()
    {
        _dbSetup = new DatabaseSetup("appsettings.integrationtests.json");
    }

    protected override void Dispose(bool disposing)
    {
        try
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _dbSetup.Dispose();
            }
        }
        catch (Exception ex)
        {
            //logging
        }
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests")
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile(Path.Combine(
                    Path.GetDirectoryName(Assembly.GetAssembly(typeof(IntegrationTestsFixture)).Location),
                    "appsettings.integrationtests.json"
                ));
            });
        base.ConfigureWebHost(builder);
    }

    public List<Product> GetProducts()
    {
        using var scope = Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return dbContext.Products.ToList();
    }

    public Product GetProduct(long id)
    {
        using var scope = Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return dbContext.Products.FirstOrDefault(x => x.Id == id);
    }
}
