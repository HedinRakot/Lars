using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTemsAPI.Database.Migrations.Migrations;

namespace MyTemsAPI.IntegrationTests.TestSetup;

public class DatabaseSetup : IDisposable
{
    private readonly string _myTemsAPIConnectionString;

    public DatabaseSetup(string configFile)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(configFile, true, true)
            .Build();

        _myTemsAPIConnectionString = configuration.GetConnectionString("MyTemsDb");

        var connectionStringBuilder = new SqlConnectionStringBuilder(_myTemsAPIConnectionString);
        var initialCatalog = connectionStringBuilder.InitialCatalog;
        connectionStringBuilder.Remove("Initial Catalog");

        using (var sqlConnection = new SqlConnection(connectionStringBuilder.ConnectionString))
        {
            sqlConnection.Open();

            var command = $"SELECT * FROM sys.databases WHERE NAME ='{initialCatalog}'";

            using var sqlCommand = new SqlCommand(command, sqlConnection);
            using var sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                var dropCommand = $"USE master; ALTER DATABASE {initialCatalog} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE {initialCatalog}";
                using var dropSqlCommand = new SqlCommand(dropCommand, sqlConnection);
                dropSqlCommand.ExecuteNonQuery();
            }
        }
        var dbContextOptions = new DbContextOptionsBuilder<TestDatabaseDbContext>()
            .UseSqlServer(_myTemsAPIConnectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;

        using var dbContext = new TestDatabaseDbContext(dbContextOptions);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
            .AddSqlServer()
            .WithGlobalConnectionString(_myTemsAPIConnectionString)
            .ScanIn(typeof(Initial).Assembly).For.Migrations()
            )
            .BuildServiceProvider(false)
            .GetRequiredService<IMigrationRunner>()
            .MigrateUp();
    }

    public void Dispose()
    {
        //var connectionStringBuilder = new SqlConnectionStringBuilder(_myTemsAPIConnectionString);
        //var initialCatalog = connectionStringBuilder.InitialCatalog;
        //connectionStringBuilder.Remove("Initial Catalog");

        //using var sqlConnection = new SqlConnection(connectionStringBuilder.ConnectionString);
        //sqlConnection.Open();

        //var dropCommand = $"USE master; ALTER DATABASE {initialCatalog} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE {initialCatalog}";
        //using var dropSqlCommand = new SqlCommand(dropCommand, sqlConnection);
        //dropSqlCommand.ExecuteNonQuery();
    }

    private void GetCollection()
    {
         new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
            .AddSqlServer()
            .WithGlobalConnectionString(_myTemsAPIConnectionString)
            .ScanIn(typeof(Initial).Assembly).For.Migrations()
            )
            .BuildServiceProvider(false)
            .GetRequiredService<IMigrationRunner>()
            .MigrateUp();
    
    }
}
