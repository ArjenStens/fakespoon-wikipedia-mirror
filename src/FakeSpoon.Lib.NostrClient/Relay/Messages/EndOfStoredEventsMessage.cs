namespace FakeSpoon.Lib.NostrClient.Relay.Messages;

public class EndOfStoredEventsMessage : RelayMessage
{
    public string? Subscription { get; init; }
    public RelayMessageType MessageType { get; init; } = RelayMessageType.EndOfStoredEvents;
}