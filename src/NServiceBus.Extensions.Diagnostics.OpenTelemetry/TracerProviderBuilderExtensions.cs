namespace OpenTelemetry.Trace
{
    public static class TracerProviderBuilderExtensions
    {
        public static TracerProviderBuilder AddNServiceBusInstrumentation(this TracerProviderBuilder builder) 
            => builder.AddSource("NServiceBus.Extensions.Diagnostics");
    }
}