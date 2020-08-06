using System;
using NServiceBus.Extensions.Diagnostics.OpenTelemetry.Implementation;
using OpenTelemetry.Instrumentation;
using OpenTelemetry.Trace;

namespace NServiceBus.Extensions.Diagnostics.OpenTelemetry
{
    public class NServiceBusReceiveInstrumentation : IDisposable
    {
        private readonly DiagnosticSourceSubscriber _diagnosticSourceSubscriber;

        public NServiceBusReceiveInstrumentation(ActivitySourceAdapter activitySource, NServiceBusInstrumentationOptions options)
        {
            _diagnosticSourceSubscriber = new DiagnosticSourceSubscriber(new ProcessMessageListener(activitySource, options), null);
            _diagnosticSourceSubscriber.Subscribe();
        }

        public void Dispose()
            => _diagnosticSourceSubscriber?.Dispose();
    }
}
