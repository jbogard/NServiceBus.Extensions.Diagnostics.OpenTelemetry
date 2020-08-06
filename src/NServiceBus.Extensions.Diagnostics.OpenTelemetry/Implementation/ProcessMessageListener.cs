using System.Diagnostics;
using System.Text;
using NServiceBus.Pipeline;
using NServiceBus.Settings;
using OpenTelemetry.Instrumentation;
using OpenTelemetry.Trace;

namespace NServiceBus.Extensions.Diagnostics.OpenTelemetry.Implementation
{
    internal class ProcessMessageListener : ListenerHandler
    {
        private readonly NServiceBusInstrumentationOptions _options;
        private readonly ActivitySourceAdapter _activitySource;

        public ProcessMessageListener(ActivitySourceAdapter activitySource, NServiceBusInstrumentationOptions options) 
            : base("NServiceBus.Extensions.Diagnostics.IncomingPhysicalMessage")
        {
            _activitySource = activitySource;
            _options = options;
        }

        public override void OnStartActivity(Activity activity, object payload)
        {
            if (!(payload is IIncomingPhysicalMessageContext context))
            {
                return;
            }

            var settings = context.Builder.Build<ReadOnlySettings>();

            activity.SetKind(ActivityKind.Consumer);
            activity.DisplayName = settings.LogicalAddress().ToString();

            _activitySource.Start(activity);

            if (activity.IsAllDataRequested)
            {
                activity.AddTag("messaging.message_id", context.Message.MessageId);
                activity.AddTag("messaging.operation", "process");
                activity.AddTag("messaging.message_payload_size_bytes", context.Message.Body.Length.ToString());

                if (_options.CaptureMessageBody)
                {
                    activity.AddTag("messaging.message_payload", Encoding.UTF8.GetString(context.Message.Body));
                }

                activity.ApplyContext(settings, context.MessageHeaders);
            }
        }

        public override void OnStopActivity(Activity activity, object payload) => 
            _activitySource.Stop(activity);
    }
}