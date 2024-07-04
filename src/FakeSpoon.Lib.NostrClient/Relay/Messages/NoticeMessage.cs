using FakeSpoon.Lib.NostrClient.Relay.Requests;
using Newtonsoft.Json;

namespace FakeSpoon.Lib.NostrClient.Relay.Messages;

[JsonConverter(typeof(RelayRequestConverter))]
public class NoticeResponse : RelayResponse
{
    [RelayRequestIndex(0)]
    public RelayMessageType MessageType { get; init; } = RelayMessageType.Notice;
    
    [RelayRequestIndex(1)]
    public string? Message { get; init; }
}