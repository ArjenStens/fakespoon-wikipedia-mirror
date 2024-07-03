using FakeSpoon.Lib.NostrClient.Relay.Messages;

namespace FakeSpoon.Lib.NostrClient.Relay.Requests;

public interface IRelayRequest
{
    public RelayMessageType MessageType { get; init; }
}