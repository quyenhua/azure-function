using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Application;
using Infrastructure;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using Azure.Monitor.OpenTelemetry.Exporter;

namespace FunctionApp;

public class Startup(IConfiguration configuration)
{
    private readonly IConfiguration configuration = configuration;

    public void ConfigureServices(IServiceCollection collection)
    {
        collection.AddApplication();

        // TODO The Azure Function should not have any dependency on Infrastructure other than DI
        // Determine a better technique
        collection.AddInfrastructure(configuration);

        // Create a new tracer provider builder and add an Azure Monitor trace exporter to the tracer provider builder.
        // It is important to keep the TracerProvider instance active throughout the process lifetime.
        var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddAzureMonitorTraceExporter();

        // Add an Azure Monitor metric exporter to the metrics provider builder.
        // It is important to keep the MetricsProvider instance active throughout the process lifetime.
        var metricsProvider = Sdk.CreateMeterProviderBuilder()
            .AddAzureMonitorMetricExporter();

        // Create a new logger factory.
        // It is important to keep the LoggerFactory instance active throughout the process lifetime.
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddOpenTelemetry(options =>
            {
                options.AddAzureMonitorLogExporter();
            });
        });
    }
}