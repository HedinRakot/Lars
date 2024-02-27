using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.Transport.SqlServer;
using System.Data.SqlClient;

namespace LarsProjekt.NServiceBus;

public static class ConfigExtension
{
    public static async Task AddNServiceBus(ConfigurationManager configuration, IServiceCollection services, string endpointName, string connectionString)
    {
        EndpointConfiguration endpointConfiguration;
        string? nserviceBusConnectionString;
        ConfigureEndpoint(configuration, endpointName, connectionString, out endpointConfiguration, out nserviceBusConnectionString);
        ConfigureTransport(endpointConfiguration, nserviceBusConnectionString);
        await ConfigureSqlPersistence(endpointConfiguration, nserviceBusConnectionString);

        await AddServices(services, endpointConfiguration);

        static void ConfigureEndpoint(ConfigurationManager configuration, string endpointName, string connectionString, out EndpointConfiguration endpointConfiguration, out string? nserviceBusConnectionString)
        {
            endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            nserviceBusConnectionString = configuration.GetConnectionString(connectionString);
        }

        static void ConfigureTransport(EndpointConfiguration endpointConfiguration, string? nserviceBusConnectionString)
        {
            var transportConfig = new SqlServerTransport(nserviceBusConnectionString)
            {
                DefaultSchema = "dbo",
                TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive,
                Subscriptions = {
                CacheInvalidationPeriod = TimeSpan.FromMinutes(1),
                SubscriptionTableName = new SubscriptionTableName(
                table: "Subscriptions",
                schema: "dbo")
                }
            };
            transportConfig.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
            transportConfig.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>(transportConfig);
            //transport.RouteToEndpoint(typeof(TestCommand), "MyTemsAPI");
        }

        static async Task ConfigureSqlPersistence(EndpointConfiguration endpointConfiguration, string? nserviceBusConnectionString)
        {
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
            dialect.Schema("dbo");
            persistence.ConnectionBuilder(() => new SqlConnection(nserviceBusConnectionString));
            persistence.TablePrefix("");

            await SqlServerHelper.CreateSchema(nserviceBusConnectionString, "dbo");
        }

        static async Task AddServices(IServiceCollection services, EndpointConfiguration endpointConfiguration)
        {
            var endpointContainer = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, services);
            var endpointInstance = await endpointContainer.Start(services.BuildServiceProvider());

            services.AddSingleton<IMessageSession>(endpointInstance);
        }
    }
}

