using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Application;
using Infrastructure;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry;
using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Extensions.Logging;

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

        collection.AddOpenTelemetry().UseAzureMonitor(o =>
        {
            o.ConnectionString = "InstrumentationKey=53439a70-5ae8-4eb7-be1d-f18993da33dc;IngestionEndpoint=https://australiacentral-0.in.applicationinsights.azure.com/;LiveEndpoint=https://australiacentral.livediagnostics.monitor.azure.com/";
        });

        // Create a new tracer provider builder and add an Azure Monitor trace exporter to the tracer provider builder.
        // It is important to keep the TracerProvider instance active throughout the process lifetime.
        var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddAzureMonitorTraceExporter();

        //// Add an Azure Monitor metric exporter to the metrics provider builder.
        //// It is important to keep the MetricsProvider instance active throughout the process lifetime.
        var metricsProvider = Sdk.CreateMeterProviderBuilder()
            .AddAzureMonitorMetricExporter();   

        //// Create a new logger factory.
        //// It is important to keep the LoggerFactory instance active throughout the process lifetime.
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddOpenTelemetry(options =>
            {
                options.AddAzureMonitorLogExporter(o =>
                {
                    o.ConnectionString = "InstrumentationKey=53439a70-5ae8-4eb7-be1d-f18993da33dc;IngestionEndpoint=https://australiacentral-0.in.applicationinsights.azure.com/;LiveEndpoint=https://australiacentral.livediagnostics.monitor.azure.com/";
                });
            });
        });

        collection
            .AddSingleton(tracerProvider)
            .AddSingleton(metricsProvider)
            .AddSingleton(loggerFactory);
    }
}