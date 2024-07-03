using System.Diagnostics;
using FakeSpoon.Lib.NostrClient.Relay.WebSocket;
using Newtonsoft.Json;
using Websocket.Client;

namespace FakeSpoon.Lib.NostrClient.Relay.Messages;

[DebuggerDisplay("[{Source.Name}] {Message}")]
public class RawRelayMessage
{
    public ResponseMessage? Message { get; init; }
        
    [JsonIgnore]
    public IRelayWebsocketClient? Source { get; internal set; }
}