using FakeSpoon.Lib.NostrClient.Events;

namespace FakeSpoon.Lib.NostrClient.Relay.Messages;

public class EventMessage : RelayMessage
{
    public string? Subscription { get; init; }

    public NostrEvent? Event { get; init; }

    public RelayMessageType MessageType { get; init; } = RelayMessageType.Event;
}