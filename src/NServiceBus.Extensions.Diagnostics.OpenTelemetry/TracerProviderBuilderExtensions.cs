using System;
using OpenTelemetry.Trace;

namespace NServiceBus.Extensions.Diagnostics.OpenTelemetry
{
    public static class TracerProviderBuilderExtensions
    {
        public static TracerProviderBuilder AddNServiceBusInstrumentation(this TracerProviderBuilder builder)
            => builder.AddNServiceBusInstrumentation(null);

        public static TracerProviderBuilder AddNServiceBusInstrumentation(this TracerProviderBuilder builder, Action<NServiceBusInstrumentationOptions> configureInstrumentationOptions)
        {
            var options = new NServiceBusInstrumentationOptions();

            configureInstrumentationOptions ??= opt => { };
            
            configureInstrumentationOptions(options);
            
            return builder
                .AddInstrumentation(t => new NServiceBusReceiveInstrumentation(t, options))
                .AddInstrumentation(t => new NServiceBusSendInstrumentation(t, options));
        }
    }
}