using FakeSpoon.Lib.NostrClient.Relay.Messages;

namespace FakeSpoon.Lib.NostrClient.Relay.Requests;

public class CloseSubscriptionRequest(string subscription) : IRelayRequest
{
    public RelayMessageType MessageType { get; init; } = RelayMessageType.Close;

    public string Subscription { get; init; } = subscription;
}