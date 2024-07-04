namespace FakeSpoon.Lib.NostrClient.Relay.Messages;

public class EndOfStoredEventsResponse : RelayResponse
{
    public string? Subscription { get; init; }
    public RelayMessageType MessageType { get; init; } = RelayMessageType.EndOfStoredEvents;
}