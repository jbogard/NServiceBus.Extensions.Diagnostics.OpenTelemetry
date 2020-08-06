using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NServiceBus.Settings;
using NServiceBus.Transport;

namespace NServiceBus.Extensions.Diagnostics.OpenTelemetry.Implementation
{
    internal static class ActivityExtensions
    {
        public static void ApplyContext(this Activity activity, ReadOnlySettings settings,
            IReadOnlyDictionary<string, string> contextHeaders)
        {
            var transportDefinition = settings.Get<TransportDefinition>();
            activity.AddTag("messaging.system", transportDefinition.GetType().Name.Replace("Transport", null).ToLowerInvariant());
            activity.AddTag("messaging.destination", settings.LogicalAddress().ToString());
            if (contextHeaders.TryGetValue(Headers.ConversationId, out var conversationId))
            {
                activity.AddTag("messaging.conversation_id", conversationId);
            }

            if (contextHeaders.TryGetValue(Headers.MessageIntent, out var intent)
                && Enum.TryParse<MessageIntentEnum>(intent, out var intentValue))
            {
                var routingPolicy = settings.Get<TransportInfrastructure>().OutboundRoutingPolicy;

                var kind = GetDestinationKind(intentValue, routingPolicy);

                if (kind != null)
                {
                    activity.AddTag("messaging.destination_kind", kind);
                }
            }

            foreach (var header in contextHeaders.Where(header => header.Key.StartsWith("NServiceBus.")))
            {
                activity.AddTag($"messaging.{header.Key.ToLowerInvariant()}", header.Value);
            }
        }

        private static string GetDestinationKind(MessageIntentEnum intentValue, OutboundRoutingPolicy routingPolicy) =>
            intentValue switch
            {
                MessageIntentEnum.Send => ConvertPolicyToKind(routingPolicy.Sends),
                MessageIntentEnum.Publish => ConvertPolicyToKind(routingPolicy.Publishes),
                MessageIntentEnum.Subscribe => ConvertPolicyToKind(routingPolicy.Sends),
                MessageIntentEnum.Unsubscribe => ConvertPolicyToKind(routingPolicy.Sends),
                MessageIntentEnum.Reply => ConvertPolicyToKind(routingPolicy.Replies),
                _ => null
            };

        private static string ConvertPolicyToKind(OutboundRoutingType type) =>
            type switch
            {
                OutboundRoutingType.Unicast => "queue",
                OutboundRoutingType.Multicast => "topic",
                _ => null
            };
    }
}