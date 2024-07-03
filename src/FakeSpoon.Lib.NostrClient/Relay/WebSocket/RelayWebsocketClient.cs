using System.Net.WebSockets;
using Websocket.Client;

namespace FakeSpoon.Lib.NostrClient.Relay.WebSocket;

public class RelayWebsocketClient : WebsocketClient, IRelayWebsocketClient
{
    /// <inheritdoc />
    public RelayWebsocketClient(Uri url, Func<ClientWebSocket>? clientFactory = null)
        : base(url, clientFactory)
    {
    }
}