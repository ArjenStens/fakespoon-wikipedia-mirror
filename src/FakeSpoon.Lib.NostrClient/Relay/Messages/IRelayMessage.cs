using FakeSpoon.Lib.NostrClient.Relay.WebSocket;

namespace FakeSpoon.Lib.NostrClient.Relay.Messages;

public interface IRelayMessage
{
    
    
}

public class RelayMessage : IRelayMessage
{
    public RelayMessageType MessageType { get; init; } = RelayMessageType.Unknown;
    
    public object[] AdditionalData { get; private set; } = Array.Empty<object>();
    
    public IRelayWebsocketClient Source { get; internal set; }
    
    public DateTime ReceivedTimestamp { get; internal set; } = DateTime.UtcNow;
}