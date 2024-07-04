using FakeSpoon.Lib.NostrClient.Events;
using Newtonsoft.Json;

namespace FakeSpoon.Lib.NostrClient.Relay.Messages.Requests;

[JsonConverter(typeof(RelayRequestConverter))]
public class PublishEventRequest : IRelayMessage
{
    [RelayRequestIndex(0)]
    public RelayMessageType MessageType { get; init; } = RelayMessageType.Event;
    
    [RelayRequestIndex(1)]
    public NostrEvent Event { get; init; }
}