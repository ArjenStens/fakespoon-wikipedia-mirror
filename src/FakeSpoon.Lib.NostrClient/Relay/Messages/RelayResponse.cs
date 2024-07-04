using FakeSpoon.Lib.NostrClient.Relay.WebSocket;
using Newtonsoft.Json;

namespace FakeSpoon.Lib.NostrClient.Relay.Messages;

public abstract class RelayResponse : IRelayMessage
{
    public RelayMessageType MessageType { get; init; } = RelayMessageType.Unknown;
    
    // public object[] AdditionalData { get; private set; } = Array.Empty<object>();
    
    [JsonIgnore]
    public IRelayWebsocketClient Source { get; internal set; }
    
    [JsonIgnore]
    public DateTime ReceivedTimestamp { get; internal set; } = DateTime.UtcNow;
}