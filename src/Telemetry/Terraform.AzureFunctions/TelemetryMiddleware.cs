using System.Diagnostics;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Options;

namespace Telemetry.Telemetry.AzureFunctions;

internal sealed class TelemetryMiddleware : IFunctionsWorkerMiddleware
{
    private readonly IOptions<TelemetryOptions> _options;

    public TelemetryMiddleware(IOptions<TelemetryOptions> options)
    {
        _options = options;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var activity = GetActivity(context);

        using (activity)
        {
            try
            {
                SetActivityTags(context, activity);
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            }
        }
    }

    private Activity? GetActivity(FunctionContext context)
    {
        var activitySource = new ActivitySource(_options.Value.ServiceName);
        var headers = GetHeaders(context);

        if (headers is null)
            return activitySource.StartActivity(context.FunctionDefinition.Name, ActivityKind.Server);

        // Determine parent trace from Headers
        var traceParent = headers.TryGetValue("traceparent", out var traceParentValue) ? traceParentValue : null;
        var traceState = headers.TryGetValue("tracestate", out var traceStateValue) ? traceStateValue : null;

        ActivityContext.TryParse(traceParent, traceState, out var parentContext);
        
        return activitySource.StartActivity(context.FunctionDefinition.Name, ActivityKind.Server, parentContext);
    }

    private static IDictionary<string, string>? GetHeaders(FunctionContext context)
    {
        var hasHeaders = context.BindingContext.BindingData.TryGetValue("Headers", out var headersJson);
        var headers = hasHeaders && headersJson != null
            ? JsonSerializer.Deserialize<Dictionary<string, string>>(headersJson.ToString() ?? string.Empty)
            : null;

        return headers;
    }

    private void SetActivityTags(FunctionContext context, Activity? activity)
    {
        // Thrive Client Headers
        var headers = GetHeaders(context);
        if (headers != null)
        {
            if(headers.TryGetValue("Thrive-Client-Name", out var clientNameValue))
                activity?.SetTag("thrive.client.name", clientNameValue);
            
            if(headers.TryGetValue("Thrive-Client-Version", out var clientVersionValue))
                activity?.SetTag("thrive.client.version", clientVersionValue);
            
            if( headers.TryGetValue("Thrive-Client-Environment", out var clientEnvironmentValue))
                activity?.SetTag("thrive.client.environment", clientEnvironmentValue);
        }

        activity?
            .SetTag("thrive.function.id", context.FunctionDefinition.Id)
            .SetTag("thrive.function.name", context.FunctionDefinition.Name)
            .SetTag("thrive.function.entrypoint", context.FunctionDefinition.EntryPoint)
            .SetTag("thrive.function.pathtoassembly", context.FunctionDefinition.PathToAssembly);
    }
}
