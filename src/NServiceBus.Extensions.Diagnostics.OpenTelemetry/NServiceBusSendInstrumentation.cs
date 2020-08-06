using System;
using NServiceBus.Extensions.Diagnostics.OpenTelemetry.Implementation;
using OpenTelemetry.Instrumentation;
using OpenTelemetry.Trace;

namespace NServiceBus.Extensions.Diagnostics.OpenTelemetry
{
    public class NServiceBusSendInstrumentation : IDisposable
    {
        private readonly DiagnosticSourceSubscriber _diagnosticSourceSubscriber;

        public NServiceBusSendInstrumentation(ActivitySourceAdapter activitySource, NServiceBusInstrumentationOptions options)
        {
            _diagnosticSourceSubscriber = new DiagnosticSourceSubscriber(
                new SendMessageListener(activitySource, options), null);
            _diagnosticSourceSubscriber.Subscribe();
        }

        public void Dispose()
            => _diagnosticSourceSubscriber?.Dispose();
    }
}