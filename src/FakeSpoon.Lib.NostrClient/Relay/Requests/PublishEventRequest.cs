using FakeSpoon.Lib.NostrClient.Events;
using FakeSpoon.Lib.NostrClient.Relay.Messages;
using Newtonsoft.Json;

namespace FakeSpoon.Lib.NostrClient.Relay.Requests;

[JsonConverter(typeof(RelayRequestConverter))]
public class PublishEventRequest : IRelayRequest
{
    // for deserialization in tests
    private PublishEventRequest() 
    {
        Event = null!;
    }

    public PublishEventRequest(NostrEvent @event)
    {
        Event = @event;
    }

    [RelayRequestIndex(0)]
    public RelayMessageType MessageType { get; init; } = RelayMessageType.Event;
    
    [RelayRequestIndex(1)]
    public NostrEvent Event { get; init; }
}