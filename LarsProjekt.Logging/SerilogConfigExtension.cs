using LarsProjekt.Logging.Tracing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using System.Diagnostics;

namespace LarsProjekt.Logging;

public static class SerilogConfigExtension
{
    public static void AddSerilogWithTracing(WebApplicationBuilder builder, string connectionString)
    {
        var columnOptions = new ColumnOptions
        {
            AdditionalColumns =
            [
        new SqlColumn
            {ColumnName = "ProcessId", DataType = SqlDbType.BigInt, NonClusteredIndex = true },

        new SqlColumn
            {ColumnName = "ProcessName", DataType = SqlDbType.NVarChar },

        new SqlColumn
            {ColumnName = "MachineName", DataType = SqlDbType.NVarChar },
            ]
        };

        var loggerConfiguration = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithProcessId()
        .Enrich.WithProcessName()
        .Enrich.WithMachineName()
        .WriteTo.MSSqlServer(
            connectionString: builder.Configuration.GetConnectionString(connectionString),
            sinkOptions: new MSSqlServerSinkOptions
            {                
                AutoCreateSqlTable = true,
                TableName = "FatalLogs",
            },
            columnOptions: columnOptions);
        Log.Logger = loggerConfiguration.CreateBootstrapLogger();

        loggerConfiguration = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithProcessId()
        .Enrich.WithProcessName()
        .Enrich.WithMachineName()
        .WriteTo.MSSqlServer(
            connectionString: builder.Configuration.GetConnectionString(connectionString),
            sinkOptions: new MSSqlServerSinkOptions
            {
                AutoCreateSqlTable = true,
                TableName = "Logs",
            },
            columnOptions: columnOptions);

        var logger = loggerConfiguration.CreateLogger();
        builder.Host.UseSerilog(logger);

        builder.Services.AddSingleton<ITracingManager, TracingManager>();
        builder.Services.AddSingleton<TracingListener>();
    }

    public static void AddSerilogRequestLoggingWithTracingListener(WebApplication? app)
    {
        app.UseSerilogRequestLogging();

        var tracingListener = app.Services.GetRequiredService<TracingListener>();
        DiagnosticListener.AllListeners.Subscribe(tracingListener);
    }
}
