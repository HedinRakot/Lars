using Microsoft.EntityFrameworkCore;

namespace MyTemsAPI.IntegrationTests.TestSetup;

internal sealed class TestDatabaseDbContext : DbContext
{
    public TestDatabaseDbContext(DbContextOptions<TestDatabaseDbContext> options) : base(options)
    {        
    }
}
