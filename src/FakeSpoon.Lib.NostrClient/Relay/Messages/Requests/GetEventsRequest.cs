using FakeSpoon.Lib.NostrClient.Relay.Messages;
using Newtonsoft.Json;

namespace FakeSpoon.Lib.NostrClient.Relay.Requests;

public class GetEventsRequest(string subscription, NostrFilter nostrFilter) : IRelayMessage
{
    [RelayRequestIndex(0)]
    public RelayMessageType MessageType { get; init; } = RelayMessageType.Request;
    
    [RelayRequestIndex(1)]
    public string Subscription { get; init; } = subscription;

    [RelayRequestIndex(2)]
    public NostrFilter NostrFilter { get; init; } = nostrFilter;
}