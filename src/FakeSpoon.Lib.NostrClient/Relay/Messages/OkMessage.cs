namespace FakeSpoon.Lib.NostrClient.Relay.Messages;

public class OkMessage : RelayMessage
{
    /// <summary>
    /// Related event id
    /// </summary>
    public string? EventId { get; init; }

    /// <summary>
    /// Returns true when the event was accepted and stored on the server (even for duplicates). 
    /// Returns false when the event was rejected and not stored.
    /// </summary>
    public bool Accepted { get; init; }

    /// <summary>
    /// Additional information as to why the command succeeded or failed.
    /// </summary>
    public string? Message { get; init; }

    public RelayMessageType MessageType { get; init; } = RelayMessageType.Ok;
}

public class UnknownMessage : IRelayMessage
{
}