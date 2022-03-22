using NServiceBus.Extensions.Diagnostics;

namespace OpenTelemetry.Trace;

public static class TracerProviderBuilderExtensions
{
    public static TracerProviderBuilder AddNServiceBusInstrumentation(this TracerProviderBuilder builder) 
        => builder.AddSource(typeof(DiagnosticsFeature).Assembly.GetName().Name);
}