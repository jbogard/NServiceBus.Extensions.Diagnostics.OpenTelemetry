using System.Diagnostics;
using System.Linq;
using System.Text;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Settings;
using OpenTelemetry.Instrumentation;
using OpenTelemetry.Trace;

namespace NServiceBus.Extensions.Diagnostics.OpenTelemetry.Implementation
{
    internal class SendMessageListener : ListenerHandler
    {
        private readonly NServiceBusInstrumentationOptions _options;
        private readonly ActivitySourceAdapter _activitySource;

        public SendMessageListener(ActivitySourceAdapter activitySource, NServiceBusInstrumentationOptions options) 
            : base("NServiceBus.Extensions.Diagnostics.OutgoingPhysicalMessage")
        {
            _activitySource = activitySource;
            _options = options;
        }

        public override void OnStartActivity(Activity activity, object payload)
        {
            if (!(payload is IOutgoingPhysicalMessageContext context))
            {
                return;
            }

            activity.SetKind(ActivityKind.Producer);

            context.Headers.TryGetValue(Headers.MessageIntent, out var intent);

            var routes = context.RoutingStrategies
                .Select(r => r.Apply(context.Headers))
                .Select(t => t switch
                {
                    UnicastAddressTag u => u.Destination,
                    MulticastAddressTag m => m.MessageType.Name,
                    _ => null
                })
                .ToList();

            var operationName = $"{intent ?? activity.OperationName} {string.Join(", ", routes)}";

            activity.DisplayName = operationName;

            _activitySource.Start(activity);

            if (activity.IsAllDataRequested)
            {
                activity.AddTag("messaging.message_id", context.MessageId);
                activity.AddTag("messaging.message_payload_size_bytes", context.Body.Length.ToString());

                activity.ApplyContext(context.Builder.Build<ReadOnlySettings>(), context.Headers);

                if (_options.CaptureMessageBody)
                {
                    activity.AddTag("messaging.message_payload", Encoding.UTF8.GetString(context.Body));
                }
            }
        }

        public override void OnStopActivity(Activity activity, object payload) => 
            _activitySource.Stop(activity);
    }
}