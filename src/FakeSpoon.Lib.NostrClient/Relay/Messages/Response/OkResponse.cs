using Newtonsoft.Json;

namespace FakeSpoon.Lib.NostrClient.Relay.Messages.Response;

[JsonConverter(typeof(RelayMessageConverter))]
public class OkResponse : RelayResponse
{
    [RelayRequestIndex(0)]
    public override RelayMessageType MessageType { get; init; } = RelayMessageType.Ok;
    
    [RelayRequestIndex(1)]
    public string? EventId { get; init; }
    
    [RelayRequestIndex(2)]
    public bool Accepted { get; init; }
    
    [RelayRequestIndex(3)]
    public string? Message { get; init; }
}