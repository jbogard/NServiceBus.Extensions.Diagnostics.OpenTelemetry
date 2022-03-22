using NServiceBus.Extensions.Diagnostics;

namespace OpenTelemetry.Metrics;

public static class MeterBuilderProviderExtensions
{
    public static MeterProviderBuilder AddNServiceBusInstrumentation(
        this MeterProviderBuilder builder)
        => builder.AddMeter(typeof(DiagnosticsMetricsFeature).Assembly.GetName().Name);
}