using FakeSpoon.Lib.NostrClient.Events;

namespace FakeSpoon.Lib.NostrClient.Relay.Messages.Response;

public class EventResponse : RelayResponse
{
    public override RelayMessageType MessageType { get; init; } = RelayMessageType.Event;
    
    public string? Subscription { get; init; }

    public NostrEvent? Event { get; init; }

}