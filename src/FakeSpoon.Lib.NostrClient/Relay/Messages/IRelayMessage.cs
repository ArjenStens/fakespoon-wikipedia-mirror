namespace FakeSpoon.Lib.NostrClient.Relay.Messages;

public interface IRelayMessage
{
    public RelayMessageType MessageType { get; init; }
}