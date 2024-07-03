using FakeSpoon.Lib.NostrClient.Relay.WebSocket;
using Microsoft.Extensions.Logging;

namespace FakeSpoon.Lib.NostrClient.Relay;

/// <summary>
/// Nostr client that connects to a multiple relays. 
/// Subscribe to `Streams` to handle received messages.
/// </summary>
public class MultiRelayClient : IRelayClient
{
    private readonly ILogger<SingleRelayClient>? _logger;
    private readonly List<SingleRelayClient> _clients = new();
    private readonly Dictionary<SingleRelayClient, List<IDisposable>> _subscriptionsPerClient = new();

    public MultiRelayClient(ILogger<SingleRelayClient>? logger)
    {
        _logger = logger;
    }

    public MultiRelayClient(ILogger<SingleRelayClient>? logger, params IRelayWebsocketClient[] communicators)
    {
        _logger = logger;
        foreach (var communicator in communicators)
        {
            RegisterCommunicator(communicator);
        }
    }

    public MultiRelayClient(ILogger<SingleRelayClient>? logger, params SingleRelayClient[] clients)
    {
        _logger = logger;
        foreach (var client in clients)
        {
            RegisterClient(client);
        }
    }

    /// <summary>
    /// Provided message streams
    /// </summary>
    public RelayMessageStreams MessageStreams { get; } = new();

    /// <summary>
    /// Registered clients
    /// </summary>
    public IReadOnlyCollection<SingleRelayClient> Clients => _clients.ToArray();

    /// <summary>
    /// Send message to all communicators/relays
    /// </summary>
    public void Send<T>(T request)
    {
        foreach (var client in _clients)
        {
            client.Send(request);
        }
    }

    /// <summary>
    /// Send message to the specific communicator/relay.
    /// Return false if communicator wasn't found. 
    /// </summary>
    public bool SendTo<T>(string communicatorName, T request)
    {
        var found = FindClient(communicatorName);
        if (found == null)
            return false;
        found.Send(request);
        return true;
    }

    public void Dispose()
    {
        var subs = _subscriptionsPerClient.SelectMany(x => x.Value);
        foreach (var subscription in subs)
        {
            subscription.Dispose();
        }

        foreach (var client in _clients)
        {
            client.Dispose();
        }

        _subscriptionsPerClient.Clear();
        _clients.Clear();
    }

    /// <summary>
    /// Register a new communicator and forward messages. 
    /// Given communicator won't be disposed automatically.
    /// </summary>
    public void RegisterCommunicator(IRelayWebsocketClient communicator)
    {
        var client = new SingleRelayClient(communicator, _logger);
        RegisterClient(client);
    }

    /// <summary>
    /// Register a new client and forward messages. 
    /// Every client will be disposed automatically.
    /// </summary>
    public void RegisterClient(SingleRelayClient client)
    {
        _clients.Add(client);

        // forward all streams
        ForwardStream(client, client.MessageStreams.EventStream, MessageStreams.EventSubject);
        ForwardStream(client, client.MessageStreams.NoticeStream, MessageStreams.NoticeSubject);
        ForwardStream(client, client.MessageStreams.OkStream, MessageStreams.OkSubject);
        ForwardStream(client, client.MessageStreams.EoseStream, MessageStreams.EoseSubject);
    }

    /// <summary>
    /// Remove registration of the client by a given communicator name.
    /// Unsubscribe from all streams. 
    /// Returns true if client was found.
    /// </summary>
    public bool RemoveRegistration(string communicatorName)
    {
        var found = FindClient(communicatorName);
        if (found == null)
            return false;

        _clients.Remove(found);
        if (!_subscriptionsPerClient.ContainsKey(found))
            return true;

        var subs = _subscriptionsPerClient[found];
        foreach (var sub in subs)
        {
            sub.Dispose();
        }

        _subscriptionsPerClient.Remove(found);
        return true;
    }

    /// <summary>
    /// Find registered client by a given communicator name.
    /// Returns null if not found. 
    /// </summary>
    public SingleRelayClient? FindClient(string communicatorName)
    {
        return _clients.FirstOrDefault(x => x.RelayWebsocketClient.Name == communicatorName);
    }

    /// <summary>
    /// Find all registered clients by a given communicator name.
    /// Returns empty collection if not found. 
    /// </summary>
    public IReadOnlyCollection<SingleRelayClient> FindClients(string communicatorName)
    {
        return _clients.Where(x => x.RelayWebsocketClient.Name == communicatorName).ToArray();
    }

    /// <summary>
    /// Find registered client by a given communicator.
    /// Returns null if not found. 
    /// </summary>
    public SingleRelayClient? FindClient(IRelayWebsocketClient communicator)
    {
        return _clients.FirstOrDefault(x => x.RelayWebsocketClient == communicator);
    }

    /// <summary>
    /// Return true if client is registered 
    /// </summary>
    public bool Contains(SingleRelayClient client)
    {
        return _clients.Any(x => x == client);
    }

    private void ForwardStream<T>(SingleRelayClient client, IObservable<T> source, IObserver<T> target)
    {
        if (!_subscriptionsPerClient.ContainsKey(client))
        {
            _subscriptionsPerClient[client] = new List<IDisposable>();
        }
        var subscriptions = _subscriptionsPerClient[client];

        var sub = source.Subscribe(target);
        subscriptions.Add(sub);
    }
}