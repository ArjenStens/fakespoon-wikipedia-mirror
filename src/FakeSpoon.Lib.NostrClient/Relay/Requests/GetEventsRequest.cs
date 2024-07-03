using FakeSpoon.Lib.NostrClient.Relay.Messages;

namespace FakeSpoon.Lib.NostrClient.Relay.Requests;

public class GetEventsRequest(string subscription, NostrFilter nostrFilter) : IRelayRequest
{
    public string Subscription { get; init; } = subscription;

    public NostrFilter NostrFilter { get; init; } = nostrFilter;
    public RelayMessageType MessageType { get; init; } = RelayMessageType.Request;
}