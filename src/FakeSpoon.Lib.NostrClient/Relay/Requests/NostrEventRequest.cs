using FakeSpoon.Lib.NostrClient.Models;
using FakeSpoon.Lib.NostrClient.Relay.Messages;

namespace FakeSpoon.Lib.NostrClient.Relay.Requests;

public class PublishEventRequest : IRelayRequest
{
    // for deserialization in tests
    private PublishEventRequest() 
    {
        Event = null!;
    }

    public PublishEventRequest(NostrEvent eventData)
    {
        Event = eventData;
    }

    public RelayMessageType MessageType { get; init; } = RelayMessageType.Event;

    public NostrEvent Event { get; init; }
}