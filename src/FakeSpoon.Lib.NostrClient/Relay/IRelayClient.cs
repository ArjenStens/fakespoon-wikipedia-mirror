using FakeSpoon.Lib.NostrClient.Relay.Messages;

namespace FakeSpoon.Lib.NostrClient.Relay;

public interface IRelayClient : IDisposable
{
    RelayMessageStreams MessageStreams { get; }
    void Send(IRelayMessage request);
}