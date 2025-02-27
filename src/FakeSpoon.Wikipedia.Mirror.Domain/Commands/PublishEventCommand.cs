using System.Net.WebSockets;
using FakeSpoon.Lib.Cqe.Base;
using FakeSpoon.Lib.NostrClient.Events;
using FakeSpoon.Lib.NostrClient.Extensions;
using FakeSpoon.Lib.NostrClient.Keys;
using FakeSpoon.Lib.NostrClient.Relay;
using FakeSpoon.Lib.NostrClient.Relay.Messages.Requests;
using FakeSpoon.Lib.NostrClient.Relay.WebSocket;
using FakeSpoon.Lib.NostrClient.Utils;
using FakeSpoon.Wikipedia.Mirror.Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FakeSpoon.Wikipedia.Mirror.Domain.Commands;

public class PubCommand : ICommand
{
    required public NostrEvent Event { get; set; }
}

public class PubCommandHandler(
    ILogger<PubCommand> Logger,
    ICommandHandler<SendEventToRelaysCommand> handler,
    ILogger<SingleRelayClient> relayLogger,
    IOptions<NostrOptions> config) : ICommandHandler<PubCommand>
{
    public async Task Execute(PubCommand cmd)
    {
        var privatekey = Privatekey.FromBech32(config.Value.Nsec);
        var pubkey = privatekey.DerivePublicKey();

        cmd.Event.Pubkey = pubkey;
        cmd.Event.Sign(privatekey);

        var relays = new[]
        {
            // new Uri("wss://nos.lol/"),
            new Uri("wss://relay.damus.io/")
        };

        // using var multiClient = new MultiRelayClient(relayLogger);
        var multiClient = new MultiRelayClient(relayLogger);
        var relayWebsocketClients = new List<IRelayWebsocketClient>();

        foreach (var relay in relays)
        {
            var client = CreateRelayWebsocketClient(relay);
            relayWebsocketClients.Add(client);
            multiClient.RegisterRelayWebsocketClient(client);
            
            multiClient.MessageStreams.NoticeStream.Subscribe(msg =>
            {
                Console.WriteLine(msg);
            });
            
            multiClient.MessageStreams.OkStream.Subscribe(msg =>
            {
                Console.WriteLine(msg);
            });

            await client.Start();
            
            var serialized = JsonConvert.SerializeObject(cmd.Event, SerializerSettings.Settings);
            multiClient.Send(new PublishEventRequest {Event = cmd.Event});

        }
    }

    IRelayWebsocketClient CreateRelayWebsocketClient(Uri uri)
    {
        var comm = new RelayWebsocketClient(uri, () =>
        {
            var client = new ClientWebSocket();
            client.Options.SetRequestHeader("Origin", "http://localhost");
            return client;
        });

        comm.Name = uri.Host;
        comm.ReconnectTimeout = null; //TimeSpan.FromSeconds(30);
        comm.ErrorReconnectTimeout = TimeSpan.FromSeconds(60);

        comm.ReconnectionHappened.Subscribe(info =>
            Logger.LogInformation("[{relay}] Reconnection happened, type: {type}", comm.Name, info.Type));
        comm.DisconnectionHappened.Subscribe(info =>
            Logger.LogInformation("[{relay}] Disconnection happened, type: {type}, reason: {reason}", comm.Name, info.Type, info.CloseStatus));
        return comm;
    }
}