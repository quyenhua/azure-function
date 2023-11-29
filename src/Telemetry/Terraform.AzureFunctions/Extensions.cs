using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;

namespace Telemetry.Telemetry.AzureFunctions;

public static class Extensions
{
    /// <summary>
    /// Adds TelemetryMiddleware to Azure Function Worker to support OTel in Function app.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IFunctionsWorkerApplicationBuilder AddTelemetryMiddleware(this IFunctionsWorkerApplicationBuilder builder)
    {
        return builder.UseMiddleware<TelemetryMiddleware>();
    }
}
