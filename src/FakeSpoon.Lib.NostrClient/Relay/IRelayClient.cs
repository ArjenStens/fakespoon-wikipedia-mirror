using FakeSpoon.Lib.NostrClient.Relay.Messages;
using FakeSpoon.Lib.NostrClient.Relay.Requests;

namespace FakeSpoon.Lib.NostrClient.Relay;

public interface IRelayClient : IDisposable
{
    /// <summary>
    /// Provided message streams
    /// </summary>
    RelayMessageStreams MessageStreams { get; }

    /// <summary>
    /// Serializes request and sends message via websocket communicator. 
    /// It logs and re-throws every exception. 
    /// </summary>
    /// <param name="request">Request/message to be sent</param>
    void Send(IRelayMessage request);
}