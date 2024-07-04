using Newtonsoft.Json;

namespace FakeSpoon.Lib.NostrClient.Relay.Messages.Response;

[JsonConverter(typeof(RelayRequestConverter))]
public class EndOfStoredEventsResponse : RelayResponse
{
    [RelayRequestIndex(0)]
    public override RelayMessageType MessageType { get; init; } = RelayMessageType.EndOfStoredEvents;
    
    [RelayRequestIndex(1)]
    public string? Subscription { get; init; }
}