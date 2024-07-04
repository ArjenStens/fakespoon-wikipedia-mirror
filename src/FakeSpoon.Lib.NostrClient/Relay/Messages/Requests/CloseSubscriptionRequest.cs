namespace FakeSpoon.Lib.NostrClient.Relay.Messages.Requests;

public class CloseSubscriptionRequest(string subscription) : IRelayMessage
{
    public RelayMessageType MessageType { get; init; } = RelayMessageType.Close;

    public string Subscription { get; init; } = subscription;
}