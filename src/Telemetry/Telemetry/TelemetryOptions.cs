namespace Telemetry.Telemetry;

public class TelemetryOptions
{
    public string ServiceName { get; set; } = null!;
    public string OtlpEndpoint { get; set; } = "https://otlp.nr-data.net:4317";
    public string? OtlpHeaders { get; set; }

    public bool UseConsoleExporter { get; set; } = false;
    public bool EnableSql { get; set; } = true;
    public bool EnableHttpClient { get; set; } = true;
    public bool EnableRuntime { get; set; } = false;
}
