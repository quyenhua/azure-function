using System.Diagnostics.Metrics;
using Microsoft.Extensions.Options;

namespace Telemetry;
/// <summary>
/// Meter Factory to create global meter based on Telemetry Options Service Name.
/// </summary>
public interface IMeterFactory
{
    Meter CreateMeter(string? version = null);
}

internal class MeterFactory : IMeterFactory
{
    private readonly IOptions<TelemetryOptions> _options;

    public MeterFactory(IOptions<TelemetryOptions> options)
    {
        _options = options;
    }

    public Meter CreateMeter(string? version = null) => new(_options.Value.ServiceName, version);
}
