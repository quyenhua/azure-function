using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Telemetry.Telemetry;

public static class Extensions
{
    /// <summary>
    /// Adds OpenTelemetry services to the host.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="prefix"></param>
    /// <param name="configureTracer"></param>
    /// <param name="configureMeter"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ConfigureOpenTelemetry<T>(this T builder, 
        Action<TracerProviderBuilder>? configureTracer = null, 
        Action<MeterProviderBuilder>? configureMeter = null,
        string prefix = "")
        where T : IHostBuilder
    {
        builder.ConfigureServices((context, services) =>
        {
            services.AddOptions<TelemetryOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    var section = configuration.GetTelemetryOptionsSection(prefix);
                    section.Bind(settings);
                });

            services.AddTracing(context, prefix, configureTracer);
            services.AddMetrics(context, prefix, configureMeter);
        });
        
        return builder;
    }

    /// <summary>
    /// Adds Open Telemetry OTLP Exporter to log builder.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="context"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public static ILoggingBuilder AddOpenTelemetryExporter(this ILoggingBuilder builder, HostBuilderContext context, string prefix = "")
    {
        var options = context.Configuration.GetTelemetryOptions(prefix);
        var resourceBuilder = GetResourceBuilder(context, prefix);

        builder.AddOpenTelemetry(otlpOptions =>
        {
            otlpOptions.IncludeFormattedMessage = true;
            otlpOptions.ParseStateValues = true;
            otlpOptions.IncludeScopes = true;

            otlpOptions
                .SetResourceBuilder(resourceBuilder)
                .AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri(options.OtlpEndpoint);
                    o.Headers = options.OtlpHeaders;
                });
        });

        return builder;
    }

    private static IServiceCollection AddTracing(this IServiceCollection services, HostBuilderContext context, string prefix = "", Action<TracerProviderBuilder>? configure = null)
    {
        var options = context.Configuration.GetTelemetryOptions(prefix);
        var resourceBuilder = GetResourceBuilder(context, prefix);
        
        services
            .AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
                    .SetResourceBuilder(resourceBuilder)
                    .SetSampler(new AlwaysOnSampler())
                    .AddSource(options.ServiceName)
                    .AddAspNetCoreInstrumentation(o =>
                    {
                        o.RecordException = true;
                    });

                if (options.EnableSql)
                    builder.AddSqlClientInstrumentation(o =>
                    {
                        o.SetDbStatementForText = true;
                        o.SetDbStatementForStoredProcedure = true;
                        o.RecordException = true;
                        o.EnableConnectionLevelAttributes = true;
                    });

                if (options.EnableHttpClient)
                    builder.AddHttpClientInstrumentation(o =>
                    {
                        o.RecordException = true;
                     });

                builder.AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri(options.OtlpEndpoint);
                    o.Headers = options.OtlpHeaders;
                });

                if (options.UseConsoleExporter)
                    builder.AddConsoleExporter();

                configure?.Invoke(builder);
            });

        return services;
    }

    private static IServiceCollection AddMetrics(this IServiceCollection services, HostBuilderContext context, string prefix = "", Action<MeterProviderBuilder>? configure = null)
    {
        var options = context.Configuration.GetTelemetryOptions(prefix);
        var resourceBuilder = GetResourceBuilder(context, prefix);
        services.AddScoped<IMeterFactory, MeterFactory>();

        services
            .AddOpenTelemetry()
            .WithMetrics(builder =>
            {
                builder
                    .SetResourceBuilder(resourceBuilder)
                    .AddMeter(options.ServiceName)
                    .AddAspNetCoreInstrumentation();

                if (options.EnableRuntime)
                    builder.AddRuntimeInstrumentation();
            
                if (options.EnableHttpClient)
                    builder.AddHttpClientInstrumentation();

                if (options.UseConsoleExporter)
                    builder.AddConsoleExporter((o, readerOptions) =>
                    {
                        readerOptions.TemporalityPreference = MetricReaderTemporalityPreference.Cumulative;
                    });

                builder
                    .AddOtlpExporter((o, metricReaderOptions) =>
                    {
                        o.Endpoint = new Uri(options.OtlpEndpoint);
                        o.Headers = options.OtlpHeaders;

                        // New Relic requires the exporter to use delta aggregation temporality.
                        // The OTLP exporter defaults to using cumulative aggregation temporarily.
                        metricReaderOptions.TemporalityPreference = MetricReaderTemporalityPreference.Delta;
                    });
            
                configure?.Invoke(builder);
            });

        return services;
    }
    
    private static ResourceBuilder GetResourceBuilder(HostBuilderContext context, string prefix = "")
    {
        var configuration = context.Configuration;
        var environment = context.HostingEnvironment;
        var options = configuration.GetTelemetryOptions(prefix);

        var version = GetVersion();

        var attributes = new KeyValuePair<string, object>[]
        {
            new("environment", environment.EnvironmentName), new("version", version)
        };

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName: options.ServiceName, serviceVersion: version)
            .AddAttributes(attributes)
            .AddTelemetrySdk();

        return resourceBuilder;
    }

    private static TelemetryOptions GetTelemetryOptions(this IConfiguration configuration, string prefix = "")
    {
        var telemetryOptions = new TelemetryOptions();
        var section = configuration.GetTelemetryOptionsSection(prefix);
        section.Bind(telemetryOptions);

        return telemetryOptions;
    }

    private static IConfigurationSection GetTelemetryOptionsSection(this IConfiguration configuration, string prefix = "")
    {
        return configuration.GetSection($"{prefix}Telemetry");
    }

    private static string GetVersion()
    {
        var assembly = Assembly.GetEntryAssembly();
        var version = assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "unknown";
        return version;
    }
}
