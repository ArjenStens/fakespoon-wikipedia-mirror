using Newtonsoft.Json;

namespace FakeSpoon.Lib.NostrClient.Relay.Messages.Response;

[JsonConverter(typeof(RelayRequestConverter))]
public class NoticeResponse : RelayResponse
{
    [RelayRequestIndex(0)]
    public override RelayMessageType MessageType { get; init; } = RelayMessageType.Notice;
    
    [RelayRequestIndex(1)]
    public string? Message { get; init; }
}