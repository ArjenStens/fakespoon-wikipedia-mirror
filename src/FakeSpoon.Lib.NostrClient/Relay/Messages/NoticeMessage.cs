namespace FakeSpoon.Lib.NostrClient.Relay.Messages;

public class NoticeMessage : RelayMessage
{
    public string? Message { get; init; }
    public RelayMessageType MessageType { get; init; } = RelayMessageType.Notice;
}